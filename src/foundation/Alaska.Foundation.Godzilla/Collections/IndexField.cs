using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Collections
{
    public class IndexField<T> : IIndexField<T>
    {
        protected Expression<Func<T, object>> _field;
        protected IndexSortOrder _sort;

        public IndexField(Expression<Func<T, object>> field)
            : this(field, IndexSortOrder.Asc)
        { }

        public IndexField(Expression<Func<T, object>> field, IndexSortOrder sortOrder)
        {
            _field = field;
            _sort = sortOrder;
        }

        public Expression<Func<T, object>> Field => _field;
        public IndexSortOrder SortOrder => _sort;
    }
}
