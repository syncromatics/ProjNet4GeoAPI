using System;
using System.Collections.Generic;
using System.Reflection;

namespace ProjNet
{
    internal static class TypeExtensions
    {
        internal static IEnumerable<ConstructorInfo> GetConstructors(this Type type) => type.GetTypeInfo().DeclaredConstructors;
        internal static bool IsAssignableFrom(this Type type, Type other) => type.GetTypeInfo().IsAssignableFrom(other.GetTypeInfo());
    }
}
