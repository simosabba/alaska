using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Extensions
{
    internal static class TypeExtensions
    {
        public static Type GetBaseType(this Type value)
        {
            var baseType = value.GetTypeInfo().BaseType;
            return baseType == typeof(object) ? null : baseType;
        }

        public static bool IsDerivedFrom(this Type value, Type baseType)
        {
            var typeInfo = value.GetTypeInfo();
            return typeInfo.IsClass && typeInfo.IsSubclassOf(baseType);
        }

        public static bool ImplementsInterface(this Type value, Type interfaceType)
        {
            var typeInfo = value.GetTypeInfo();
            return typeInfo.ImplementedInterfaces.Contains(interfaceType);
        }

        public static IEnumerable<Type> GetTypeHierarchy(this Type value)
        {
            var t = new List<Type>
            {
                value
            };
            t.AddRange(value.GetBaseTypes());
            return t;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type value)
        {
            var t = new List<Type>();

            var baseType = value.GetTypeInfo().BaseType;
            if (baseType != null)
            {
                t.Add(baseType);
                t.AddRange(baseType.GetBaseTypes());
            }

            return t;
        }

        public static IEnumerable<Type> GetDescendants(this Type value)
        {
            return new List<Type> { value }.Union(value.GetDerivedTypes());
        }

        //TODO: risolvere le classi derivate non solo dell'assembly di appartenenza della classe base ma di tutti
        public static IEnumerable<Type> GetDerivedTypes(this Type value)
        {
            return value.GetDerivedTypes(value.GetTypeInfo().Assembly);
        }

        public static IEnumerable<Type> GetDerivedTypes(this Type value, IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(x => value.GetDerivedTypes(x));
        }

        public static IEnumerable<Type> GetDerivedTypes(this Type value, Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(x => x.IsDerivedFrom(value));
        }
    }
}
