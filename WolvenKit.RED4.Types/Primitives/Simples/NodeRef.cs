using System;
using System.Diagnostics;

namespace WolvenKit.RED4.Types
{
    [RED("NodeRef")]
    [DebuggerDisplay("{_value}", Type = "NodeRef")]
    public readonly struct NodeRef : IRedString, IRedPrimitive<NodeRef>, IEquatable<NodeRef>, IComparable<NodeRef>, IComparable
    {
        public static NodeRef Empty = new(0);

        private readonly ulong _hash;


        private NodeRef(string value)
        {
            _hash = NodeRefPool.AddOrGetHash(value);
        }

        private NodeRef(ulong value)
        {
            _hash = value;
        }

        public string GetResolvedText() => NodeRefPool.ResolveHash(_hash);


        public static implicit operator NodeRef(string value) => new(value);
        public static implicit operator string(NodeRef value) => value.GetResolvedText();

        public static implicit operator NodeRef(ulong value) => new(value);
        public static implicit operator ulong(NodeRef value) => value._hash;

        public static bool operator ==(NodeRef a, NodeRef b) => Equals(a, b);
        public static bool operator !=(NodeRef a, NodeRef b) => !(a == b);

        public ulong GetRedHash() => _hash;

        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            if (!(value is NodeRef other))
            {
                throw new ArgumentException();
            }

            return this.CompareTo(other);
        }

        public int CompareTo(NodeRef other)
        {
            var strA = GetResolvedText();
            var strB = GetResolvedText();

            if (strA != null && strB != null)
            {
                return string.Compare(strA, strB, StringComparison.InvariantCulture);
            }

            return _hash.CompareTo(other._hash);
        }

        public override int GetHashCode() => _hash.GetHashCode();

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

            return Equals((NodeRef)obj);
        }

        public bool Equals(NodeRef other)
        {
            if (!Equals(_hash, other._hash))
            {
                return false;
            }

            return true;
        }

        public override string ToString() => (GetResolvedText() is var text && !string.IsNullOrEmpty(text)) ? text : _hash.ToString();
    }
}
