using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nethereum.ABI.Decoders
{
    public class ArrayTypeDecoder : TypeDecoder
    {
        protected ABIType ElementType { get; set; }

        public ArrayTypeDecoder(ABIType elementType)
        {
            this.ElementType = elementType;
        }

        public override bool IsSupportedType(Type type)
        {
            return GetIListElementType(type) != null;
        }

        public override object Decode(byte[] encoded, Type type)
        {

            if (!ElementType.IsDynamic())
            {
                return DecodeStaticElementType(encoded, type);
            }
            else
            {
                throw new NotSupportedException("Arrays containing Dynamic Types are not supported");
            }
        }

        protected virtual object DecodeStaticElementType(byte[] encoded, Type type)
        {
            var decoded = (IList)Activator.CreateInstance(type);

            if (decoded == null) throw new Exception("Only types that implement IList<T> are supported to decoded Array Types");

            var elementType = GetIListElementType(type);

            if (elementType == null) throw new Exception("Only types that implement IList<T> are supported to decoded Array Types");

            var currentIndex = 0;

            while (currentIndex != encoded.Length)
            {
                var encodedElement = encoded.Skip(currentIndex).Take(ElementType.FixedSize).ToArray();
                decoded.Add(ElementType.Decode(encodedElement, elementType));
                var newIndex = currentIndex + ElementType.FixedSize;
                currentIndex = newIndex;
            }

            return decoded;
        }

        protected static Type GetIListElementType(Type listType)
        {
            var enumType = listType
                .GetInterfaces()
                .Where(i => i.GetTypeInfo().IsGenericType && i.GenericTypeArguments.Length == 1)
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            return enumType?.GenericTypeArguments[0];
        }
    }
}