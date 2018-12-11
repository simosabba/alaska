//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Mongo.Discriminators
//{
//    public class MongoEntityCollectionTypeDiscriminator : MongoTypeDiscriminatorBase
//    {
//        private readonly EntityCollectionReference _collectionReference;

//        public MongoEntityCollectionTypeDiscriminator(EntityCollectionReference collectionReference)
//        {
//            _collectionReference = collectionReference;
//        }

//        public override IEnumerable<Type> KnownTypes => _collectionReference.EntityDescendantTypes
//            .Where(x => x.ImplementsInterface(typeof(IDatabaseCollectionElement)))
//            .ToList();

//        public override string GetTypeId(Type type)
//        {
//            return _collectionReference.GetTemplate(type).Id.ToString();
//        }

//        public override Type GetItemType(Guid typeId)
//        {
//            return _collectionReference.GetTemplate(typeId).TypeInfo.Type;
//        }
//    }
//}
