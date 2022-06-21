using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WolvenKit.Core.Extensions;
using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.IO;
using WolvenKit.RED4.Types;
using WolvenKit.RED4.Types.Exceptions;

namespace WolvenKit.RED4.Archive.IO
{
    public partial class RedPackageReader : Red4Reader
    {
        public RedPackageSettings Settings = new();

        public RedPackageReader(Stream input) : this(new BinaryReader(input, Encoding.UTF8, false))
        {
        }

        public RedPackageReader(BinaryReader reader) : base(reader)
        {
        }

        public override void ReadClass(RedBaseClass cls, uint size)
        {
            var typeInfo = RedReflection.GetTypeInfo(cls);

            var baseOff = BaseStream.Position;
            var fieldCount = _reader.ReadUInt16();
            var fields = BaseStream.ReadStructs<RedPackageFieldHeader>(fieldCount);

            foreach (var f in fields)
            {
                var propRedName = GetStringValue(f.nameID);
                var propRedType = GetStringValue(f.typeID);

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

                BaseStream.Position = baseOff + f.offset;

                var value = Read(redTypeInfos);

                if (!typeInfo.SerializeDefault && !nativeProp.SerializeDefault && RedReflection.IsDefault(cls.GetType(), propRedName, value))
                {
                    // Handle invalid default value
                    throw new DoNotMergeIntoMainBeforeFixedException();
                }

                cls.SetProperty(propRedName, value);
            }
        }

        public override TweakDBID ReadTweakDBID()
        {
            if (header.version == 2 || header.version == 3)
            {
                var length = _reader.ReadInt16();
                return Encoding.UTF8.GetString(_reader.ReadBytes(length));
            }

            if (header.version == 04)
            {
                return base.ReadTweakDBID();
            }

            throw new NotImplementedException(nameof(ReadTweakDBID));
        }

        public override IRedBitField ReadCBitField(List<RedTypeInfo> redTypeInfos, uint size)
        {
            if (redTypeInfos.Count != 1)
            {
                throw new TodoException();
            }

            var cnt = _reader.ReadByte();

            var enumString = "";
            for (var i = 0; i < cnt; i++)
            {
                var index = _reader.ReadUInt16();
                if (index == 0)
                {
                    throw new TodoException();
                }

                if (!string.IsNullOrEmpty(enumString))
                {
                    enumString += ", ";
                }

                enumString += GetStringValue(index);
            }

            var type = RedReflection.GetFullType(redTypeInfos);
            if (string.IsNullOrEmpty(enumString))
            {
                return (IRedBitField)System.Activator.CreateInstance(type);
            }
            return (IRedBitField)System.Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { Enum.Parse(redTypeInfos[0].RedObjectType, enumString) }, null);
        }

        public override IRedHandle ReadCHandle(List<RedTypeInfo> redTypeInfos, uint size)
        {
            var type = RedReflection.GetFullType(redTypeInfos);
            var result = (IRedHandle)System.Activator.CreateInstance(type);

            int pointer;
            if (header.version == 2)
            {
                pointer = _reader.ReadInt16();
            }
            else if (header.version == 3 || header.version == 04)
            {
                pointer = _reader.ReadInt32();
            }
            else
            {
                throw new NotImplementedException(nameof(ReadCHandle));
            }

            if (!_handleQueue.ContainsKey(pointer))
            {
                _handleQueue.Add(pointer, new List<IRedBaseHandle>());
            }

            _handleQueue[pointer].Add(result);

            return result;
        }

        public override IRedWeakHandle ReadCWeakHandle(List<RedTypeInfo> redTypeInfos, uint size)
        {
            var type = RedReflection.GetFullType(redTypeInfos);
            var result = (IRedWeakHandle)System.Activator.CreateInstance(type);

            var pointer = _reader.ReadInt32();
            if (!_handleQueue.ContainsKey(pointer))
            {
                _handleQueue.Add(pointer, new List<IRedBaseHandle>());
            }

            _handleQueue[pointer].Add(result);

            return result;
        }

        public override IRedResourceAsyncReference ReadCResourceAsyncReference(List<RedTypeInfo> redTypeInfos, uint size)
        {
            if (redTypeInfos.Count != 2)
            {
                throw new TodoException();
            }

            var type = RedReflection.GetFullType(redTypeInfos);
            var result = (IRedResourceAsyncReference)System.Activator.CreateInstance(type);

            var index = _reader.ReadInt16();

            if (index >= 0 && index < importsList.Count)
            {
                var import = (PackageImport)importsList[index - 0];

                result.DepotPath = import.DepotPath != "" ? import.DepotPath : import.Hash;
                result.Flags = import.Flags;
            }
            else
            {
                // TODO: Find a better way (written as -1)
                result.DepotPath = "INVALID";
                result.Flags = InternalEnums.EImportFlags.Default;
            }

            return result;
        }

        public override IRedResourceReference ReadCResourceReference(List<RedTypeInfo> redTypeInfos, uint size)
        {
            if (redTypeInfos.Count != 2)
            {
                throw new TodoException();
            }

            var type = RedReflection.GetFullType(redTypeInfos);
            var result = (IRedResourceReference)System.Activator.CreateInstance(type);

            var index = _reader.ReadInt16();

            if (index >= 0 && index < importsList.Count)
            {
                var import = (PackageImport)importsList[index - 0];

                result.DepotPath = import.DepotPath != "" ? import.DepotPath : import.Hash;
                result.Flags = import.Flags;
            }
            else
            {
                // TODO: Find a better way (written as -1)
                result.DepotPath = "INVALID";
                result.Flags = InternalEnums.EImportFlags.Default;
            }

            return result;
        }

        public override NodeRef ReadNodeRef()
        {
            var length = _reader.ReadInt16();
            return Encoding.UTF8.GetString(_reader.ReadBytes(length));
        }

        public override LocalizationString ReadLocalizationString() =>
            new()
            {
                Unk1 = _reader.ReadUInt64(),
                Value = ReadSimpleString()
            };

        public string ReadSimpleString()
        {
            var length = _reader.ReadUInt16();
            return Encoding.UTF8.GetString(_reader.ReadBytes(length));
        }

        public override CString ReadCString() => ReadSimpleString();

    }
}
