using System;
using System.Diagnostics;
using System.Linq;

namespace WolvenKit.RED4.Types
{
    [DebuggerDisplay("{_value,nq}", Type = "CByteArray")]
    public sealed class WByteArray : IRedPrimitive<byte[]>, IEquatable<WByteArray>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly byte[] _value;

        public WByteArray()
        {
            _value = Array.Empty<byte>();
        }

        private WByteArray(byte[] data)
        {
            _value = data;
        }

        public static implicit operator WByteArray(byte[] value) => new(value);
        public static implicit operator byte[](WByteArray value) => value._value;

        public bool Equals(WByteArray other) => Equals(_value.Length, other._value.Length) && _value.SequenceEqual(other._value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((WByteArray)obj);
        }

        public override int GetHashCode() => (_value != null ? _value.GetHashCode() : 0);
    }
}
