//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Mongo.Discriminators
//{
//    internal static class MongoCustomTypeDiscriminatorCollection
//    {
//        private static Dictionary<Type, MongoEntityCollectionTypeDiscriminator> _Discriminators = new Dictionary<Type, MongoEntityCollectionTypeDiscriminator>();

//        public static void SetDiscriminator<T>(MongoEntityCollectionTypeDiscriminator discriminator)
//        {
//            _Discriminators.Add(typeof(T), discriminator);
//        }

//        public static MongoEntityCollectionTypeDiscriminator GetDiscriminator<T>()
//        {
//            return GetDiscriminator(typeof(T));
//        }

//        public static MongoEntityCollectionTypeDiscriminator GetDiscriminator(Type type)
//        {
//            return _Discriminators.ContainsKey(type) ?
//                _Discriminators[type] :
//                null;
//        }
//    }
//}
