using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Utils
{
    public static class ReflectionUtil
    {
        private static readonly string[] WellKnownAssemblies = 
        {
            "DnsClient",
            "NJsonSchema",
        };

        private static readonly string[] WellKnownNamespacePrefixes = 
        {
            "System.",
            "Microsoft.",
            "Castle.",
            "Newtonsoft.",
            "MongoDB.",
        };

        //public static IEnumerable<Assembly> GetNonSystemAssemblies()
        //{
        //    if (DependencyContext.Default != null)
        //    {
        //        var runtimeLibraries = DependencyContext.Default.RuntimeLibraries
        //        .ToList();

        //        var runtimeAssemblies = runtimeLibraries
        //            .Select(GetCorrespondingAssembly)
        //            .WhereNotNull()
        //            .Where(x => !IsSystemAssembly(x) && IsValidAssembly(x))
        //            .ToList();

        //        return runtimeAssemblies;
        //    }

        //    return AppDomain.CurrentDomain.GetAssemblies()
        //        .Where(x => !x.IsDynamic && !IsSystemAssembly(x) && IsValidAssembly(x))
        //        .ToList();
        //}

        //public static Assembly GetCorrespondingAssembly(CompilationLibrary library)
        //{
        //    try
        //    {
        //        return Assembly.Load(new AssemblyName(library.Name));
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Current.LogWarning(e, $"Could not open assembly for library{library?.Name}");
        //        return null;
        //    }
        //}

        //public static Assembly GetCorrespondingAssembly(RuntimeLibrary library)
        //{
        //    try
        //    {
        //        return Assembly.Load(new AssemblyName(library.Name));
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Current.LogError(e, $"Could not open assembly for library{library?.Name}");
        //        return null;
        //    }
        //}
            
        public static bool IsSystemAssembly(Assembly assembly)
        {
            return IsSystemAssembly(assembly.GetName().Name) ||
                string.IsNullOrWhiteSpace(assembly.Location);
        }

        public static bool IsSystemAssembly(string assemblyName)
        {
            return WellKnownNamespacePrefixes.Any(x => assemblyName.StartsWith(x))
                || WellKnownAssemblies.Any(x => assemblyName.Equals(x));
        }

        public static bool IsValidAssembly(Assembly assembly)
        {
            try
            {
                assembly.GetExportedTypes();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Type GetBaseType(Type value)
        {
            var baseType = value.GetTypeInfo().BaseType;
            return baseType == typeof(object) ? null : baseType;
        }

        public static bool IsBaseTypeOf(Type value, Type baseType)
        {
            var typeInfo = value.GetTypeInfo();
            return typeInfo.IsClass && typeInfo.BaseType.Equals(baseType);
        }

        public static bool IsDerivedFrom(Type value, Type baseType)
        {
            var typeInfo = value.GetTypeInfo();
            return typeInfo.IsClass && typeInfo.IsSubclassOf(baseType);
        }

        public static bool HasAttribute<T>(Type value)
            where T : Attribute
        {
            var typeInfo = value.GetTypeInfo();
            return typeInfo.IsClass && typeInfo.GetCustomAttribute<T>(true) != null;
        }

        public static bool ImplementsInterface(Type value, Type interfaceType)
        {
            var typeInfo = value.GetTypeInfo();
            return typeInfo.ImplementedInterfaces.Contains(interfaceType);
        }

        public static IEnumerable<Type> GetTypeHierarchy(Type value)
        {
            var t = new List<Type> { value };
            t.AddRange(GetBaseTypes(value));
            return t;
        }

        public static IEnumerable<Type> GetBaseTypes(Type value)
        {
            var t = new List<Type>();

            var baseType = value.GetTypeInfo().BaseType;
            if (baseType != null)
            {
                t.Add(baseType);
                t.AddRange(GetBaseTypes(baseType));
            }

            return t;
        }

        public static IEnumerable<Type> GetAllNonAbstractDerivedTypes(Type value)
        {
            return GetAllDerivedTypes(value)
                .Where(x => !x.IsAbstract)
                .ToList();
        }

        public static IEnumerable<Type> GetAllNonAbstractDerivedTypes(Type value, IEnumerable<Assembly> assemblies)
        {
            return GetAllDerivedTypes(value, assemblies)
                .Where(x => !x.IsAbstract)
                .ToList();
        }

        public static IEnumerable<Type> GetAllDerivedTypes(Type value)
        {
            return new List<Type> { value }.Union(GetDerivedTypes(value));
        }

        public static IEnumerable<Type> GetDerivedTypes(Type value)
        {
            return GetDerivedTypes(value, value.GetTypeInfo().Assembly);
        }

        public static IEnumerable<Type> GetAllDerivedTypes(Type value, IEnumerable<Assembly> assemblies)
        {
            return new List<Type> { value }.Union(GetDerivedTypes(value, assemblies));
        }

        public static IEnumerable<Type> GetDerivedTypes(Type value, IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(x => GetDerivedTypes(value, x));
        }

        public static IEnumerable<Type> GetDerivedTypes(Type value, Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes().Where(x => IsDerivedFrom(x, value));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error loading derived types for assembly {assembly.FullName}", e);
            }
        }

        public static IEnumerable<Type> GetTypesWithAttribute<T>(IEnumerable<Assembly> assemblies)
            where T : Attribute
        {
            return assemblies.SelectMany(x => GetTypesWithAttribute<T>(x));
        }

        public static IEnumerable<Type> GetTypesWithAttribute<T>(Assembly assembly)
            where T : Attribute
        {
            try
            {
                return assembly.GetExportedTypes().Where(x => HasAttribute<T>(x));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error loading types with attribute {typeof(T).FullName} for assembly {assembly.FullName}", e);
            }
        }

        public static MethodInfo GetGenericMethod(Type type, BindingFlags bindingFlags, string name)
        {
            return GetGenericMethods(type, bindingFlags, name, (Type[])null)
                .FirstOrDefault();
        }

        public static IEnumerable<MethodInfo> GetGenericMethods(Type type, BindingFlags bindingFlags, string name)
        {
            return GetGenericMethods(type, bindingFlags, name, (Type[])null);
        }

        public static MethodInfo GetGenericMethod(Type type, BindingFlags bindingFlags, string name, params string[] inputTypes)
        {
            return GetGenericMethods(type, bindingFlags, name, inputTypes)
                .FirstOrDefault();
        }

        public static IEnumerable<MethodInfo> GetGenericMethods(Type type, BindingFlags bindingFlags, string name, params string[] inputTypes)
        {
            return type.GetTypeInfo()
                .GetMethods(bindingFlags)
                .Where(x => x.Name.Equals(name) && x.IsGenericMethod && HasMatchingInputParameters(x, inputTypes));
        }

        public static MethodInfo GetGenericMethod(Type type, BindingFlags bindingFlags, string name, params Type[] inputTypes)
        {
            return GetGenericMethods(type, bindingFlags, name, inputTypes)
                .FirstOrDefault();
        }

        public static IEnumerable<MethodInfo> GetGenericMethods(Type type, BindingFlags bindingFlags, string name, params Type[] inputTypes)
        {
            return type.GetTypeInfo()
                .GetMethods(bindingFlags)
                .Where(x => x.Name.Equals(name) && x.IsGenericMethod && (inputTypes == null || HasMatchingInputParameters(x, inputTypes)));
        }

        private static bool HasMatchingInputParameters(MethodInfo method, string[] parameterTypes)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != parameterTypes.Length)
                return false;

            foreach (var index in Enumerable.Range(0, parameters.Length))
            {
                var methodType = parameters[index].ParameterType;
                var paramType = parameterTypes[index];
                if (!methodType.ToString().Equals(paramType))
                    return false;
            }

            return true;
        }

        private static bool HasMatchingInputParameters(MethodInfo method, Type[] parameterTypes)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != parameterTypes.Length)
                return false;

            foreach (var index in Enumerable.Range(0, parameters.Length))
            {
                var methodType = parameters[index].ParameterType;
                var paramType = parameterTypes[index];
                if (methodType.Name != paramType.Name || methodType.Namespace != paramType.Namespace)
                    return false;
            }

            return true;
        }
    }
}
