﻿using WolvenKit.RED4.Types;
using WolvenKit.RED4.Types.Compression;

namespace WolvenKit.RED4
{
    public class RedBuffer : IRedBuffer
    {
        private byte[] _data;

        public RedBuffer() { }

        public uint Flags { get; set; }
        public byte[] Data
        {
            get => _data;
            set
            {
                _data = value;

                MemSize = (uint)_data.Length;

                IsCompressed = false;
                IsIncompressable = false;
            }
        }

        public bool IsCompressed { get; set; }
        public bool IsIncompressable { get; private set; }

        public uint MemSize { get; set; }

        public void Compress()
        {
            if (IsCompressed || IsIncompressable)
            {
                return;
            }

            if (OodleLZHelper.CompressBuffer(_data, out _data) == OodleLZHelper.Status.Compressed)
            {
                IsCompressed = true;
            }
            else
            {
                IsIncompressable = true;
            }
        }

        public void Decompress()
        {
            if (!IsCompressed)
            {
                return;
            }

            OodleLZHelper.DecompressBuffer(_data, out _data);

            IsCompressed = false;
        }

        public static RedBuffer CreateBuffer(uint flags, byte[] data)
        {
            return new RedBuffer { Flags = flags, Data = data, IsCompressed = false, MemSize = (uint)data.Length };
        }

        public static RedBuffer CreateCompressedBuffer(uint flags, byte[] data, uint memSize)
        {
            return new RedBuffer { Flags = flags, Data = data, IsCompressed = true, MemSize = memSize };
        }
    }
}