//using Alaska.Foundation.Godzilla.Abstractions;
//using Alaska.Foundation.Godzilla.Attributes;
//using Alaska.Foundation.Godzilla.Entities.Common;
//using Alaska.Foundation.Godzilla.Entities.Templates;
//using Alaska.Foundation.Godzilla.Entries;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Entities.Relationships
//{
//    [DefaultArea(GodzillaCore.Areas.RelationshipDefinition)]
//    [Template(GodzillaCore.Templates.RelationshipDefinition)]
//    public class RelationshipDefinition : IEntity
//    {
//        public Guid Id { get; set; }
//        public string EntityName { get; set; }
//        public RelationDirection Direction { get; set; }
//        public DataTypeInfo TypeInfo { get; set; }
//        public List<TypeReference> BaseTypes { get; set; }
//        public bool IsActive { get; set; }
//    }
//}
