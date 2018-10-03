using Alaska.Foundation.Core.Exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Utils
{
    public static class Check
    {
        public static void IsDerivedFrom<T>(Type baseType)
        {
            IsDerivedFrom(typeof(T), baseType);
        }

        public static void IsDerivedFrom(Type type, Type baseType)
        {
            if (!ReflectionUtil.IsDerivedFrom(type, baseType))
                throw new AssertionException($"Type {type.FullName} is not derived from {baseType.FullName}");
        }

        public static void IsAnyNotNull(params object[] values)
        {
            if (values.All(x => x == null))
                throw new AssertionException("Expected at least one non null value");
        }

        public static void LengthEquals<T>(IEnumerable<T> collection, uint size, string message)
        {
            EvaluateAndEnrichException(() => LengthEquals(collection, size), message);
        }

        public static void LengthEquals<T>(IEnumerable<T> collection, uint size)
        {
            IsNotNull(collection);
            var length = collection.Count();
            if (length != size)
                throw new AssertionException($"Expected collection equals {size} and current size={length}");
        }

        public static void LengthGreaterThen<T>(IEnumerable<T> collection, uint minSize, string message)
        {
            EvaluateAndEnrichException(() => LengthGreaterThen(collection, minSize), message);
        }

        public static void LengthGreaterThen<T>(IEnumerable<T> collection, uint minSize)
        {
            IsNotNull(collection);
            var length = collection.Count();
            if (length <= minSize)
                throw new AssertionException($"Expected collection length greater then {minSize} and current size={length}");
        }

        public static void LengthGreaterOrEqualsThen<T>(IEnumerable<T> collection, uint minSize, string message)
        {
            EvaluateAndEnrichException(() => LengthGreaterOrEqualsThen(collection, minSize), message);
        }

        public static void LengthGreaterOrEqualsThen<T>(IEnumerable<T> collection, uint minSize)
        {
            IsNotNull(collection);
            var length = collection.Count();
            if (length < minSize)
                throw new AssertionException($"Expected collection length greater or equals then {minSize} and current size={length}");
        }

        public static void LengthLessThen<T>(IEnumerable<T> collection, uint maxSize, string message)
        {
            EvaluateAndEnrichException(() => LengthLessThen(collection, maxSize), message);
        }

        public static void LengthLessThen<T>(IEnumerable<T> collection, uint maxSize)
        {
            IsNotNull(collection);
            var length = collection.Count();
            if (length >= maxSize)
                throw new AssertionException($"Expected collection length less then {maxSize} and current size={length}");
        }

        public static void LengthLessOrEqualsThen<T>(IEnumerable<T> collection, uint maxSize, string message)
        {
            EvaluateAndEnrichException(() => LengthLessOrEqualsThen(collection, maxSize), message);
        }

        public static void LengthLessOrEqualsThen<T>(IEnumerable<T> collection, uint maxSize)
        {
            IsNotNull(collection);
            var length = collection.Count();
            if (length > maxSize)
                throw new AssertionException($"Expected collection length less or equals then {maxSize} and current size={length}");
        }

        public static void IsNotNull(object value, string message)
        {
            EvaluateAndEnrichException(() => IsNotNull(value), message);
        }

        public static void IsNotNull(object value)
        {
            if (value == null)
                throw new AssertionException($"Null value");
        }

        public static void IsNull(object value, string message)
        {
            EvaluateAndEnrichException(() => IsNull(value), message);
        }

        public static void IsNull(object value)
        {
            if (value != null)
                throw new AssertionException($"Null value");
        }

        public static void IsNotNullOrWhiteSpace(string value, string message)
        {
            EvaluateAndEnrichException(() => IsNotNullOrWhiteSpace(value), message);
        }

        public static void IsNotNullOrWhiteSpace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new AssertionException($"Null or empty string");
        }

        public static void IsNullOrWhiteSpace(string value, string message)
        {
            EvaluateAndEnrichException(() => IsNullOrWhiteSpace(value), message);
        }

        public static void IsNullOrWhiteSpace(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                throw new AssertionException($"Not null or empty string");
        }

        public static void IsNullNullOrEmpty<T>(IEnumerable<T> collection, string message)
        {
            EvaluateAndEnrichException(() => IsNullNullOrEmpty(collection), message);
        }

        public static void IsNullNullOrEmpty<T>(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new AssertionException($"Null collection");
            if (collection.Count() == 0)
                throw new AssertionException($"Empty collection");
        }

        public static void IsNotEmpty(Guid value, string message)
        {
            EvaluateAndEnrichException(() => IsNotEmpty(value), message);
        }

        public static void IsNotEmpty(Guid value)
        {
            if (value == Guid.Empty)
                throw new AssertionException($"Empty guid");
        }

        private static void EvaluateAndEnrichException(Action action, string message)
        {
            try
            {
                action();
            }
            catch (AssertionException e)
            {
                throw new AssertionException(message, e);
            }
        }
    }
}
