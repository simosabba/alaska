using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public enum IndexSortOrder { Asc, Desc }

    public interface IIndexField<T>
    {
        Expression<Func<T, object>> Field { get; }
        IndexSortOrder SortOrder { get; }
    }
}
