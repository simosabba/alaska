using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class LinqExtensions
    {
        public static string JoinString(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings);
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> collection, int count)
        {
            return collection.Reverse().Skip(count).Reverse();
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> collection)
        {
            return collection.Where(x => x != null);
        }

        public static IEnumerable<T> DistinctByProperty<T>(this IEnumerable<T> collection, Func<T, object> keySelector)
        {
            return collection
                .GroupBy(x => keySelector(x))
                .Select(x => x.First());
        }

        public static bool IsEqualTo<T>(this IEnumerable<T> collection, IEnumerable<T> other)
        {
            if (collection.Count() != other.Count())
                return false;

            var collectionList = collection.ToList();
            var otherList = other.ToList();

            for (var i = 0; i < collectionList.Count; i++)
                if (!collectionList[i].Equals(otherList[i]))
                    return false;

            return true;
        }
    }
}
