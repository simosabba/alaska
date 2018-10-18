using Alaska.Foundation.Godzilla.Mongo.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Discriminators
{
    public abstract class MongoTypeDiscriminatorBase : IDiscriminatorConvention
    {
        public abstract IEnumerable<Type> KnownTypes { get; }

        public abstract string GetTypeId(Type type);

        public abstract Type GetItemType(Guid typeId);

        public virtual string ElementName
        {
            get { return "_t"; }
        }

        public virtual Type GetActualType(IBsonReader bsonReader, Type nominalType)
        {
            var bookmark = bsonReader.GetBookmark();
            bsonReader.ReadStartDocument();
            if (!bsonReader.FindElement(ElementName))
            {
                bsonReader.ReturnToBookmark(bookmark);
                return nominalType;
            }

            var typeValue = bsonReader.ReadString();
            bsonReader.ReturnToBookmark(bookmark);
            var type = GetItemType(new Guid(typeValue));
            if (type == null)
                throw new MongoDerivedTypeResolutionException($"Cannot resolve derived type {typeValue}");

            return type;
        }

        public virtual MongoDB.Bson.BsonValue GetDiscriminator(Type nominalType, Type actualType)
        {
            if (nominalType == actualType)
                return BsonNull.Value;
            return GetTypeId(actualType);
        }
    }
}
