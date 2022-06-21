using System;
using System.IO;
using System.Text;
using Splat;
using WolvenKit.Common.Services;
using WolvenKit.RED4.IO;
using WolvenKit.RED4.Types;
using WolvenKit.RED4.Types.Exceptions;

namespace WolvenKit.RED4.Archive.IO
{
    public partial class CR2WReader : Red4Reader
    {
        private ILoggerService _logger;

        public CR2WReader(Stream input) : this(input, Encoding.UTF8, false)
        {
        }

        public CR2WReader(Stream input, Encoding encoding) : this(input, encoding, false)
        {
        }

        public CR2WReader(Stream input, Encoding encoding, bool leaveOpen) : this(new BinaryReader(input, encoding, leaveOpen))
        {
        }

        public CR2WReader(BinaryReader reader) : base(reader)
        {
            _logger = Locator.Current.GetService<ILoggerService>();
        }

        public override void ReadClass(RedBaseClass cls, uint size)
        {
            var typeInfo = RedReflection.GetTypeInfo(cls);

            if (cls is IRedCustomData customCls)
            {
                customCls.CustomRead(this, size);
                return;
            }

            var startPos = _reader.BaseStream.Position;

            #region initial checks

            // ... okay CDPR, is that a joke or what?
            int zero = _reader.ReadByte();
            if (zero != 0)
            {
                throw new Exception($"Tried parsing a CVariable: zero read {zero}.");
            }

            #endregion

            #region parse sequential variables

            while (true)
            {
                #region Header

                var propRedName = ReadCName();
                if (propRedName == "")
                {
                    break;
                }

                var propRedType = ReadCName();
                var propSize = _reader.ReadUInt32() - 4;

                #endregion

                var nativeProp = RedReflection.GetNativePropertyInfo(cls.GetType(), propRedName);
                if (nativeProp == null)
                {
                    // Handle dynamic props
                    throw new DoNotMergeIntoMainBeforeFixedException();
                }

                var redTypeInfos = RedReflection.GetRedTypeInfos(propRedType);
                foreach (var redTypeInfo in redTypeInfos)
                {
                    if (redTypeInfo is SpecialRedTypeInfo)
                    {
                        // Handle unknown rtti type
                        throw new DoNotMergeIntoMainBeforeFixedException();
                    }
                }

                if (nativeProp.Type != RedReflection.GetFullType(redTypeInfos))
                {
                    // Handle type mismatch
                    throw new DoNotMergeIntoMainBeforeFixedException();
                }

                var value = Read(redTypeInfos, propSize);

                if (!typeInfo.SerializeDefault && !nativeProp.SerializeDefault && RedReflection.IsDefault(cls.GetType(), propRedName, value))
                {
                    // Handle invalid default value
                    throw new DoNotMergeIntoMainBeforeFixedException();
                }

                #region Post processing

                if (value is IRedBufferPointer buf)
                {
                    buf.GetValue().ParentTypes.Add($"{cls.GetType().Name}.{propRedName}");
                    buf.GetValue().Parent = cls;
                }

                if (value is IRedArray arr)
                {
                    if (typeof(IRedBufferPointer).IsAssignableFrom(arr.InnerType))
                    {
                        foreach (IRedBufferPointer entry in arr)
                        {
                            entry.GetValue().ParentTypes.Add($"{cls.GetType().Name}.{propRedName}");
                            entry.GetValue().Parent = cls;
                        }
                    }
                }

                #endregion Post processing

                cls.SetProperty(propRedName, value);
            }

            #endregion

            var endPos = _reader.BaseStream.Position;
            var bytesRead = endPos - startPos;

            if (cls is IRedAppendix app)
            {
                app.Read(this, (uint)(size - bytesRead));
            }

            if (bytesRead != size)
            {
                //throw new InvalidParsingException($"Read bytes not equal to expected bytes. Difference: {bytesread - size}");
            }
        }

        public override SharedDataBuffer ReadSharedDataBuffer(uint size)
        {
            var innerSize = BaseReader.ReadUInt32();
            if (size != innerSize + 4)
            {
                throw new TodoException("ReadSharedDataBuffer");
            }

            var result = base.ReadSharedDataBuffer(innerSize);

            if (_parseBuffer)
            {
                using var ms = new MemoryStream(result.Buffer.GetBytes());
                using var br = new BinaryReader(ms, Encoding.Default, true);

                using var cr2wReader = new CR2WReader(br);
                cr2wReader.ParsingError += HandleParsingError;

                var readResult = cr2wReader.ReadFile(out var c, true);
                if (readResult == EFileReadErrorCodes.NoCr2w)
                {
                    throw new TodoException("ReadSharedDataBuffer");
                }

                result.File = c;
            }

            return result;
        }
    }
}
