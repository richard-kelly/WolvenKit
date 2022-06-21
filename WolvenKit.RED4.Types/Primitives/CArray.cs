using System;
using System.Collections.Generic;

namespace WolvenKit.RED4.Types
{
    public static class CArray
    {
        public static IRedArray Create(Type innerType)
        {
            var genericType = typeof(CArray<>);
            var constructedType = genericType.MakeGenericType(innerType);

            return (IRedArray)System.Activator.CreateInstance(constructedType);
        }
    }

    [RED("array")]
    public class CArray<T> : CArrayBase<T>, IRedArray<T> where T : IRedType
    {
        public CArray() : base()
        {

        }

        public CArray(int size) : base(size)
        {

        }

        public CArray(List<T> list) : base(list)
        {

        }

        public override object DeepCopy()
        {
            var other = new CArray<T>();

            foreach (var element in _internalList)
            {
                if (element is IRedCloneable cl)
                {
                    other.Add((T)cl.DeepCopy());
                }
                else
                {
                    other.Add(element);
                }
            }

            return other;
        }
    }
}
