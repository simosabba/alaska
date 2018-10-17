using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Collections
{
    public class DatabaseCollection<T> : DatabaseCollectionBase<T, IDatabaseCollectionProvider<T>>
        where T : IDatabaseCollectionElement
    {
        public DatabaseCollection()
        { }
    }
}
