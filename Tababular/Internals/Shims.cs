#if NETSTANDARD1_6
using System;
using System.Reflection;

namespace Tababular.Internals
{
    static class TypeExtensions
    {
        public static PropertyInfo[] GetProperties(this Type type)
        {
            return type.GetTypeInfo().GetProperties();
        }
    }
}
#endif