using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    internal class ItemQueryBuilder
    {
        public ItemQueryBuilder()
        {
        }

        //public Expression<Func<HierarchyEntry, bool>> HierarchyQuery => GetHierarchyQuery();

        //protected Expression<Func<HierarchyEntry, bool>> GetHierarchyQuery()
        //{
        //    var stringFilters = GetFilters<HierarchyStringFieldFilter>();
        //}

        //protected Expression<Func<HierarchyEntry, bool>> GetHierarchyStringFieldFilter(HierarchyStringFieldFilter filter)
        //{
        //    var value = filter.Value;
        //    switch (filter.Operator)
        //    {
        //        case StringOperator.Equals:
        //            switch (filter.FilterField)
        //            {
        //                case HierarchyStringFieldFilter.Field.Name:
        //                    return x => x.Name.ToLower() == value.ToLower();

        //                case HierarchyStringFieldFilter.Field.Path:
        //                    return x => x.Path.ToLower() == value.ToLower();
        //            }
        //            break;
        //        case StringOperator.StartsWith:
        //            switch (filter.FilterField)
        //            {
        //                case HierarchyStringFieldFilter.Field.Name:
        //                    return x => x.Name.ToLower().StartsWith(value.ToLower());
        //                case HierarchyStringFieldFilter.Field.Path:
        //                    return x => x.Path.ToLower().StartsWith(value.ToLower());
        //            }
        //            break;
        //    }

        //    throw new NotImplementedException($"Filter {filter.ToString()} not implemented");
        //}

        //protected IEnumerable<TFilter> GetFilters<TFilter>()
        //    where TFilter : QueryFilter
        //{
        //    return _filters
        //        .Where(x => x is TFilter)
        //        .Cast<TFilter>();
        //}
    }
}
