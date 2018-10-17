using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Collections
{
    public class IndexDefinition<T> : IDatabaseCollectionIndex<T>
    {
        protected string _name;

        public IndexDefinition(string name)
        {
            _name = name;
        }

        public string Name => _name;
        public IEnumerable<IndexField<T>> Fields { get; set; }
        public IndexOptions Options { get; set; }

        IIndexOptions IDatabaseCollectionIndex.Options => Options;

        IEnumerable<IIndexField<T>> IDatabaseCollectionIndex<T>.Fields => Fields;
    }
}
