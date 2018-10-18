//using Alaska.Foundation.Godzilla.Entries;
//using Alaska.Foundation.Godzilla.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Mongo.Discriminators
//{
//    public class MongoRelationshipTypeDiscriminator : MongoTypeDiscriminatorBase
//    {
//        private EntityContext _context;

//        public MongoRelationshipTypeDiscriminator(EntityContext context)
//        {
//            _context = context;
//        }

//        private RelationshipCollectionReference Relationships => _context.Resolver.RelationshipCollection;

//        public override IEnumerable<Type> KnownTypes => Relationships.RelationshipTypes;

//        public override Type GetItemType(Guid typeId)
//        {
//            var relationDefinition = Relationships.GetRelationship(typeId);
//            if (relationDefinition == null)
//                throw new InvalidOperationException($"Cannot find relation definition for relation type id {typeId}");

//            var relationType = relationDefinition.TypeInfo.Type;
//            return typeof(RelationshipEntry<>).MakeGenericType(relationType);
//        }

//        public override string GetTypeId(Type type)
//        {
//            var relationType = type.GetGenericArguments().FirstOrDefault();
//            if (relationType == null)
//                throw new InvalidOperationException($"Cannot find generic type for type {type.FullName}");
//            var relationDefinition = Relationships.GetRelationship(relationType);
//            if (relationDefinition == null)
//                throw new InvalidOperationException($"Cannot find relation definition for relation type {relationType.FullName}");

//            return relationDefinition.Id.ToString();
//        }
//    }
//}
