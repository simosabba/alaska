using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Extensions
{
    public static class JsonExtensions
    {
        public static bool JsonEquals(this object value, object other)
        {
            if (value == null || other == null)
                return false;

            return value.SerializeStrongTypedJson() == other.SerializeStrongTypedJson();
        }

        public static object JsonStrongTypedClone(this object value)
        {
            return value.SerializeStrongTypedJson().DeserializeStrongTypedJson();
        }

        public static T JsonClone<T>(this T value)
        {
            return value.SerializeStrongTypedJson().DeserializeStrongTypedJson<T>();
        }

        public static bool CanDeserializeJson(this string value, Type type)
        {
            string message;
            return CanDeserializeJson(value, type, out message);
        }

        public static bool CanDeserializeJson(this string value, Type type, out string errorMessage)
        {
            try
            {
                JsonConvert.DeserializeObject(value, type);
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        public static bool CanDeserializeJson<T>(this string value)
        {
            try
            {
                value.DeserializeJson<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static object DeserializeJson(this string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public static T DeserializeJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static object DeserializeStrongTypedJson(this string value)
        {
            return JsonConvert.DeserializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        }

        public static T DeserializeStrongTypedJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        }

        public static string SerializeJson(this object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public static string SerializeStrongTypedJson(this object value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects
            });
        }
    }
}
