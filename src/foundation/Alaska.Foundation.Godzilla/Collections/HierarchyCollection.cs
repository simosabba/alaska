using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Collections
{
    internal class HierarchyCollection : DatabaseCollection<HierarchyEntry>
    {
        public HierarchyCollection(DatabaseCollectionOptions options)
            : base(options)
        { }
    }
}
