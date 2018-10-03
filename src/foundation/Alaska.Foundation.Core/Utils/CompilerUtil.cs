using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Utils
{
    public class CompilerUtil
    {
        private static CompilerUtil _Instance = new CompilerUtil();
        public static CompilerUtil Current => _Instance;
        private CompilerUtil() { }

        private Dictionary<string, MethodInfo> _methodsCache = new Dictionary<string, MethodInfo>();

        public MethodInfo GetGenericMethod<TMethod>(Type type, string methodName, BindingFlags bindingFlags, params Type[] inputTypes)
        {
            var key = GetGenericMethodKey<TMethod>(type, methodName, bindingFlags, inputTypes);
            if (!_methodsCache.ContainsKey(key))
            {
                var methodDefinition = ReflectionUtil.GetGenericMethod(type, bindingFlags, methodName, inputTypes);
                var genericMethod = methodDefinition.MakeGenericMethod(typeof(TMethod));
                _methodsCache[key] = genericMethod;
            }
            return _methodsCache[key];
        }

        private string GetGenericMethodKey<TMethod>(Type type, string methodName, BindingFlags bindingFlags, params Type[] inputTypes)
        {
            var keyString = $"{type.FullName}|{methodName}|{bindingFlags}|{string.Join(";", inputTypes.Select(x => x.FullName))}|{typeof(TMethod).FullName}";
            return keyString.GetHashCode().ToString();
        }
    }
}
