using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entries
{
    internal class HierarchyEntry : IDatabaseCollectionElement, IEntityInfo
    {
        public HierarchyEntry()
        { }

        public Guid Id { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public Guid ItemId { get; set; }

        public Guid ParentId { get; set; }

        public string CollectionName { get; set; }

        public string TemplateId { get; set; }

        public ItemOrigin Origin { get; set; }

        public ItemState State { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
