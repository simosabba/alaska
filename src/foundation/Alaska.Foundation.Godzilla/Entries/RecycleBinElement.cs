using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entries
{
    public enum DeletionState { SoftDeleted, HardDeleted }

    internal class RecycleBinElement : IDatabaseCollectionElement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public HierarchyEntry HierarchyEntry { get; set; }
        public string TemplateId { get; set; }
        public object Value { get; set; }
        public DeletionState DeletionState { get; set; }
        public DateTime DeletionTime { get; set; }
    }
}
