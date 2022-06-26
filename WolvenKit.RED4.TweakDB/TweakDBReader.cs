using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using WolvenKit.Common.FNV1A;
using WolvenKit.Core.Extensions;
using WolvenKit.RED4.IO;
using WolvenKit.RED4.Types;
using WolvenKit.RED4.Types.Exceptions;

namespace WolvenKit.RED4.TweakDB;

public class TweakDBReader : Red4Reader
{
    private const uint s_recordSeed = 0x5EEDBA5E;

    private static readonly Dictionary<ulong, string> s_typeHashes = new();
    private static readonly Dictionary<uint, Type> s_recordHashes = new();

    public TweakDBReader(Stream input) : base(input)
    {
    }

    public TweakDBReader(Stream input, Encoding encoding) : base(input, encoding)
    {
    }

    public TweakDBReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
    {
    }

    public TweakDBReader(BinaryReader reader) : base(reader)
    {
    }

    static TweakDBReader()
    {
        foreach (var enumType in Enum.GetValues<ETweakType>())
        {
            var type = GetTypeFromEnum(enumType);
            var arrType = typeof(CArray<>).MakeGenericType(type);

            var redName = RedReflection.GetRedTypeFromCSType(type);

            s_typeHashes.Add(FNV1A64HashAlgorithm.HashString(redName), redName);
            s_typeHashes.Add(FNV1A64HashAlgorithm.HashString($"array:{redName}"), $"array:{redName}");
        }

        var gameDataRegex = new Regex("gamedata(.*)_Record");
        foreach (var (redName, type) in RedReflection.GetTypes())
        {
            var match = gameDataRegex.Match(redName);
            if (match.Success)
            {
                s_recordHashes[Core.Murmur3.Murmur32.Hash(match.Groups[1].Value, s_recordSeed)] = type;
            }
        }
    }

    public EFileReadErrorCodes ReadFile(out TweakDB file)
    {
        var id = BaseStream.ReadStruct<uint>();
        if (id != TweakDB.Magic)
        {
            file = null;
            return EFileReadErrorCodes.NoTweakDB;
        }

        var fileHeader = BaseStream.ReadStruct<Header>();
        if (fileHeader.blobVersion != TweakDB.BlobVersion || fileHeader.parserVersion != TweakDB.ParserVersion)
        {
            file = null;
            return EFileReadErrorCodes.UnsupportedVersion;
        }

        file = new TweakDB();

        ReadFlats(fileHeader.flatsOffset, file.Flats);
        ReadRecords(fileHeader.recordsOffset, file.Records);
        file.Queries = ReadQueries(fileHeader.queriesOffset);
        file.GroupTags = ReadGroupTags(fileHeader.groupTagsOffset);

        return EFileReadErrorCodes.NoError;
    }

    private void ReadFlats(int offset, FlatsPool pool)
    {
        Position = offset;

        var flatTypeCounts = new Dictionary<ulong, uint>();
        var numFlatTypes = BaseReader.ReadInt32();
        for (int i = 0; i < numFlatTypes; i++)
        {
            var typeHash = BaseReader.ReadUInt64();
            var typeCount = BaseReader.ReadUInt32();
            flatTypeCounts.Add(typeHash, typeCount);
        }

        var flatTypeValues = new Dictionary<ulong, List<IRedType>>();
        foreach (var (typeHash, typeCount) in flatTypeCounts)
        {
            flatTypeValues[typeHash] = new List<IRedType>();
            var type = s_typeHashes[typeHash];
            var redTypeInfos = RedReflection.GetRedTypeInfos(type);

            var numValues = BaseReader.ReadUInt32();
            for (int j = 0; j < numValues; j++)
            {
                flatTypeValues[typeHash].Add(Read(redTypeInfos));
            }

            var numKeys = BaseReader.ReadUInt32();
            for (int j = 0; j < numKeys; j++)
            {
                var keyHash = ReadTweakDBID();
                var valueIndex = BaseReader.ReadInt32();

                pool.Add(keyHash, flatTypeValues[typeHash][valueIndex]);
            }
        }
    }

    private void ReadRecords(int offset, RecordsPool pool)
    {
        Position = offset;

        var numRecords = BaseReader.ReadInt32();
        for (int i = 0; i < numRecords; i++)
        {
            pool.Add(BaseReader.ReadUInt64(), s_recordHashes[BaseReader.ReadUInt32()]);
        }
    }

    private Dictionary<TweakDBID, List<TweakDBID>> ReadQueries(int offset)
    {
        var result = new Dictionary<TweakDBID, List<TweakDBID>>();


        Position = offset;

        var numQueries = BaseReader.ReadInt32();
        for (int i = 0; i < numQueries; i++)
        {
            var tdbName = ReadTweakDBID();
            result.Add(tdbName, new List<TweakDBID>());

            var numResults = BaseReader.ReadUInt32();
            for (int j = 0; j < numResults; j++)
            {
                result[tdbName].Add(ReadTweakDBID());
            }
        }

        return result;
    }

    private Dictionary<TweakDBID, byte> ReadGroupTags(int offset)
    {
        var result = new Dictionary<TweakDBID, byte>();


        Position = offset;

        var numGroupTags = BaseReader.ReadInt32();
        for (int i = 0; i < numGroupTags; i++)
        {
            result.Add(ReadTweakDBID(), BaseReader.ReadByte());
        }

        return result;
    }

    private static Type GetTypeFromEnum(ETweakType enumType)
        => enumType switch
        {
            ETweakType.CName => typeof(CName),
            ETweakType.CString => typeof(CString),
            ETweakType.TweakDBID => typeof(TweakDBID),
            ETweakType.CResource => typeof(CResourceAsyncReference<CResource>),
            ETweakType.CFloat => typeof(CFloat),
            ETweakType.CBool => typeof(CBool),
            ETweakType.CUint8 => typeof(CUInt8),
            ETweakType.CUint16 => typeof(CUInt16),
            ETweakType.CUint32 => typeof(CUInt32),
            ETweakType.CUint64 => typeof(CUInt64),
            ETweakType.CInt8 => typeof(CInt8),
            ETweakType.CInt16 => typeof(CInt16),
            ETweakType.CInt32 => typeof(CInt32),
            ETweakType.CInt64 => typeof(CInt64),
            ETweakType.CColor => typeof(CColor),
            ETweakType.CEulerAngles => typeof(EulerAngles),
            ETweakType.CQuaternion => typeof(Quaternion),
            ETweakType.CVector2 => typeof(Vector2),
            ETweakType.CVector3 => typeof(Vector3),
            ETweakType.LocKey => typeof(gamedataLocKeyWrapper),
            _ => throw new ArgumentOutOfRangeException(nameof(enumType))
        };

    public override CName ReadCName() => BaseReader.ReadLengthPrefixedString();

    public override IRedResourceAsyncReference ReadCResourceAsyncReference(List<RedTypeInfo> redTypeInfos, uint size)
    {
        if (redTypeInfos.Count != 2)
        {
            throw new TodoException();
        }

        var type = RedReflection.GetFullType(redTypeInfos);
        var result = (IRedResourceAsyncReference)System.Activator.CreateInstance(type, (CName)BaseReader.ReadUInt64());

        return result;
    }

    public override void ReadClass(RedBaseClass cls, uint size)
    {
        var typeInfo = RedReflection.GetTypeInfo(cls.GetType());

        #region initial checks

        // ... okay CDPR, is that a joke or what?
        int zero = _reader.ReadByte();
        if (zero != 0)
        {
            throw new Exception($"Tried parsing a CVariable: zero read {zero}.");
        }

        #endregion

        while (true)
        {
            #region Header

            var propRedName = BaseReader.ReadLengthPrefixedString();
            if (propRedName == "None")
            {
                break;
            }
            var propRedType = BaseReader.ReadLengthPrefixedString();
            var propSize = _reader.ReadUInt32() - 4;

            #endregion

            var nativeProp = typeInfo.GetNativePropertyInfoByName(propRedName);
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

            cls.SetProperty(propRedName, value);
        }
    }
}
