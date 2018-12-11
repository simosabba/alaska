using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IDatabaseCollectionIndex<T> : IDatabaseCollectionIndex
    {
        IEnumerable<IIndexField<T>> Fields { get; }
    }

    public interface IDatabaseCollectionIndex
    {
        IIndexOptions Options { get; }
        string Name { get; }
    }
}
