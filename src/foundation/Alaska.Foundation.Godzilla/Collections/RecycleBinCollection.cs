using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Collections
{
    internal class RecycleBinCollection : DatabaseCollection<RecycleBinElement>
    {
        public RecycleBinCollection(DatabaseCollectionOptions options)
            : base(options)
        { }
    }
}
