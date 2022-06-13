using System;
using System.Diagnostics;

namespace WolvenKit.RED4.Types
{
    [RED("String")]
    [DebuggerDisplay("{_value}", Type = "CString")]
    public readonly struct CString : IRedPrimitive<CString>, IEquatable<CString>, IComparable<CString>, IComparable
    {
        private readonly string _value = "\0";


        private CString(string value)
        {
            _value = value;
        }

        public static implicit operator CString(string value) => new(value);
        public static implicit operator string(CString value) => value._value;

        public static bool operator ==(CString a, CString b) => Equals(a, b);
        public static bool operator !=(CString a, CString b) => !(a == b);

        public override string ToString() => this;


        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value is not CString other)
            {
                throw new ArgumentException();
            }

            return this.CompareTo(other);
        }

        public int CompareTo(CString other) => string.Compare(this, other);


        public bool Equals(CString other) => string.Equals(_value, other._value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CString)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}
