//using Alaska.Foundation.Godzilla.Abstractions;
//using Alaska.Foundation.Godzilla.Mongo.Core;
//using Alaska.Foundation.Godzilla.Mongo.Discriminators;
//using Alaska.Foundation.Godzilla.Mongo.Extensions;
//using Alaska.Foundation.Godzilla.Mongo.Helpers;
//using Alaska.Foundation.Godzilla.Settings;
//using MongoDB.Bson.Serialization.Conventions;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace Alaska.Foundation.Godzilla.Mongo.Providers
//{
//    public class MongoDbCollectionProvider<T> : IDatabaseCollectionProvider<T>
//            where T : IDatabaseCollectionElement
//    {
//        private const string _IdFieldName = "_id";
//        private static readonly string[] _SystemIndexes = { "_id_" };

//        protected string _collectionName;
//        protected IMongoDatabase _database;
//        protected IMongoCollection<T> _collection;
//        private MongoEntityCollectionTypeDiscriminator _discriminator;
//        private DatabaseCollectionProviderOptions _options;

//        public MongoDbCollectionProvider(IMongoDatabase database, string collectionName)
//            : this(database, collectionName, null)
//        { }

//        public MongoDbCollectionProvider(IMongoDatabase database, string collectionName, MongoEntityCollectionTypeDiscriminator discriminator)
//            : base()
//        {
//            _collectionName = collectionName;
//            _database = database;
//            _collection = database.GetCollection<T>(collectionName);
//            _discriminator = discriminator;
//        }

//        public void Configure(DatabaseCollectionProviderOptions options)
//        {
//            _options = options;
//            var conventions = new ConventionPack
//            {
//                new GuidAsStringRepresentationConvention(new List<System.Reflection.Assembly>()),
//                new IgnoreExtraElementsConvention(true)
//            };
//            ConventionRegistry.Register("customConventions", conventions, x => true);
//        }

//        public IMongoDatabase Database => _database;
//        public IMongoCollection<T> Collection => _collection;

//        public IQueryable<T> AsQueryable()
//        {
//            return _collection.AsQueryable<T>();
//        }

//        public IQueryable<TDerived> AsQueryable<TDerived>()
//            where TDerived : T
//        {
//            return _collection
//                .OfType<TDerived>()
//                .AsQueryable<TDerived>();
//        }

//        public IEnumerable<TField> SelectFieldValues<TField>(Expression<Func<T, TField>> selector)
//        {
//            return _collection.AsQueryable()
//                .Select(selector)
//                .ToList();
//        }

//        public IEnumerable<TField> SelectFieldValues<TField>(Expression<Func<T, TField>> selector, Expression<Func<T, bool>> filter)
//        {
//            return _collection.AsQueryable()
//                .Where(filter)
//                .Select(selector)
//                .ToList();
//        }

//        public IEnumerable<Guid> GetAllId()
//        {
//            return _collection.AsQueryable()
//                .Select(x => x.Id)
//                .ToList();
//        }

//        public IEnumerable<T> Get()
//        {
//            return _collection.Find(x => true).ToList();
//        }

//        public IEnumerable<TDerived> Get<TDerived>()
//            where TDerived : T
//        {
//            return Get<TDerived>(false);
//        }

//        public IEnumerable<TDerived> Get<TDerived>(bool includeDerivedTypes)
//            where TDerived : T
//        {
//            if (!includeDerivedTypes || _discriminator == null)
//                return _collection
//                .OfType<TDerived>()
//                .Find(x => true)
//                .ToList();

//            var derivedTypesFilter = BuildDerivedFromFilter<TDerived>();
//            var results = _collection
//                .Find(derivedTypesFilter)
//                .ToList();
//            return results.Select(x => (TDerived)x).ToList();
//        }

//        public TDerived Get<TDerived>(Guid itemId)
//            where TDerived : T
//        {
//            return _collection
//                .OfType<TDerived>()
//                .Find(Eq<TDerived>(itemId))
//                .FirstOrDefault();
//        }

//        public T Get(Guid itemId)
//        {
//            return _collection
//                .Find(Eq(itemId))
//                .FirstOrDefault();
//        }

//        public IEnumerable<Guid> GetId(Expression<Func<T, bool>> filter)
//        {
//            return _collection.AsQueryable()
//                .Where(filter)
//                .Select(x => x.Id)
//                .ToList();
//        }

//        public IEnumerable<Guid> GetId<TDerived>(Expression<Func<TDerived, bool>> filter)
//            where TDerived : T
//        {
//            return _collection
//                .OfType<TDerived>()
//                .AsQueryable()
//                .Where(filter)
//                .Select(x => x.Id)
//                .ToList();
//        }

//        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
//        {
//            return _collection.Find(filter).ToList();
//        }

//        public IEnumerable<TDerived> Get<TDerived>(Expression<Func<TDerived, bool>> filter)
//            where TDerived : T
//        {
//            var results = _collection
//                .OfType<TDerived>()
//                .Find(filter)
//                .ToList();
//            return results;
//        }

//        public IEnumerable<TDerived> Get<TDerived>(Expression<Func<TDerived, object>> field, Regex pattern)
//            where TDerived : T
//        {
//            var filter = Builders<TDerived>.Filter.Regex(field, new MongoDB.Bson.BsonRegularExpression(pattern));
//            return _collection.OfType<TDerived>().Find(filter).ToList();
//        }

//        public IEnumerable<T> Get(Expression<Func<T, object>> field, Regex pattern)
//        {
//            var filter = Builders<T>.Filter.Regex(field, new MongoDB.Bson.BsonRegularExpression(pattern));
//            return _collection.Find(filter).ToList();
//        }

//        //public IEnumerable<TDerived> Get<TDerived>(Filter<TDerived> filter)
//        //    where TDerived : T

//        //{
//        //    var mongoFilter = MongoFiltersHelper<TDerived>.Build(filter);
//        //    return _collection.OfType<TDerived>().Find(mongoFilter).ToList();
//        //}

//        //public IEnumerable<T> Get(Filter<T> filter)
//        //{
//        //    var mongoFilter = MongoFiltersHelper<T>.Build(filter);
//        //    return _collection.Find(mongoFilter).ToList();
//        //}

//        //public IPartialCollection<TDerived> Get<TDerived>(Expression<Func<TDerived, bool>> filter, FilterOptions<TDerived> options)
//        //    where TDerived : T
//        //{
//        //    var find = _collection.OfType<TDerived>().Find(filter);

//        //    if (options.SortBy != null)
//        //        find = options.SortOrder == SortOrder.Asc ?
//        //            find.SortBy(options.SortBy) :
//        //            find.SortByDescending(options.SortBy);

//        //    if (options.Offset.HasValue)
//        //        find = find.Skip(options.Offset.Value);

//        //    if (options.TotItems.HasValue)
//        //        find = find.Limit(options.TotItems.Value);

//        //    var results = find.ToList();

//        //    var count = _collection.OfType<TDerived>().Count(filter);

//        //    return new PartialCollection<TDerived>
//        //    {
//        //        Offset = options.Offset ?? 0,
//        //        TotItems = options.TotItems ?? 0,
//        //        TotResults = count,
//        //        Data = results,
//        //    };
//        //}

//        //public IPartialCollection<T> Get(Expression<Func<T, bool>> filter, FilterOptions<T> options)
//        //{
//        //    var find = _collection.Find(filter);

//        //    if (options.SortBy != null)
//        //        find = options.SortOrder == SortOrder.Asc ?
//        //            find.SortBy(options.SortBy) :
//        //            find.SortByDescending(options.SortBy);

//        //    if (options.Offset.HasValue)
//        //        find = find.Skip(options.Offset.Value);

//        //    if (options.TotItems.HasValue)
//        //        find = find.Limit(options.TotItems.Value);

//        //    var results = find.ToList();

//        //    var count = _collection.Count(filter);

//        //    return new PartialCollection<T>
//        //    {
//        //        Offset = options.Offset ?? 0,
//        //        TotItems = options.TotItems ?? 0,
//        //        TotResults = count,
//        //        Data = results,
//        //    };
//        //}

//        public void Add(T item)
//        {
//            _collection.InsertOne(item);
//        }

//        public void Add(IEnumerable<T> items)
//        {
//            _collection.InsertMany(items);
//        }

//        public void Update(T item)
//        {
//            _collection.ReplaceOne(Eq(item), item);
//        }

//        public void Update(IEnumerable<T> items)
//        {
//            items.ToList().ForEach(x => Update(x));
//        }

//        public bool Contains(Guid itemId)
//        {
//            return Contains(x => x.Id == itemId);
//        }

//        public bool Contains(Expression<Func<T, bool>> filter)
//        {
//            return _collection.CountDocuments(filter) > 0;
//        }

//        public bool Contains<TDerived>(Expression<Func<TDerived, bool>> filter)
//            where TDerived : T
//        {
//            return _collection.OfType<TDerived>().CountDocuments(filter) > 0;
//        }

//        public void Delete(Guid itemId)
//        {
//            _collection.DeleteOne(Eq(itemId));
//        }

//        public void Delete<TDerived>(Expression<Func<TDerived, bool>> filter)
//            where TDerived : T
//        {
//            _collection.OfType<TDerived>().DeleteMany(filter);
//        }

//        public void Delete(Expression<Func<T, bool>> filter)
//        {
//            _collection.DeleteMany(filter);
//        }

//        //public void Delete(Filter<T> filter)
//        //{
//        //    var mongoFilter = MongoFiltersHelper<T>.Build(filter);
//        //    _collection.DeleteMany(mongoFilter);
//        //}

//        //public void Delete<TDerived>(Filter<TDerived> filter)
//        //    where TDerived : T
//        //{
//        //    var mongoFilter = MongoFiltersHelper<TDerived>.Build(filter);
//        //    _collection.OfType<TDerived>().DeleteMany(mongoFilter);
//        //}

//        private FilterDefinition<T> Eq(T item)
//        {
//            return Eq<T>(item);
//        }

//        private FilterDefinition<TDerived> Eq<TDerived>(TDerived item)
//            where TDerived : T
//        {
//            return Eq<TDerived>(item.Id);
//        }

//        private FilterDefinition<T> Eq(Guid id)
//        {
//            return Eq<T>(id);
//        }

//        private FilterDefinition<TDerived> Eq<TDerived>(Guid id)
//            where TDerived : T
//        {
//            return Builders<TDerived>.Filter.Eq(_IdFieldName, id.ToString());
//        }

//        private FilterDefinition<T> All()
//        {
//            return Builders<T>.Filter.Where(x => true);
//        }

//        private FilterDefinition<TDerived> All<TDerived>()
//            where TDerived : T
//        {
//            return Builders<TDerived>.Filter.OfType<TDerived>();
//        }

//        public long Count()
//        {
//            return _collection.CountDocuments(All());
//        }

//        public long Count<TDerived>()
//            where TDerived : T
//        {
//            return _collection.OfType<TDerived>().CountDocuments(All<TDerived>());
//        }

//        public void CreateIndex(string name, IEnumerable<IIndexField<T>> fields, IIndexOptions options)
//        {
//            var indexDefinition = MongoIndexHelper.CreateIndexDefinition(fields, options);
//            var indexOptions = MongoIndexHelper.CreateIndexOptions(name, options);
//            CreateIndex(indexDefinition, indexOptions);
//        }

//        public void CreateIndex(IndexKeysDefinition<T> index, CreateIndexOptions options)
//        {
//            _collection.Indexes.CreateOne(index, options);
//        }

//        public void CreateIndex<TDerived>(string name, IEnumerable<IIndexField<TDerived>> fields, IIndexOptions options)
//            where TDerived : T
//        {
//            var indexDefinition = MongoIndexHelper.CreateIndexDefinition(fields, options);
//            var indexOptions = MongoIndexHelper.CreateIndexOptions(name, options);
//            CreateIndex<TDerived>(indexDefinition, indexOptions);
//        }

//        public void CreateIndex<TDerived>(IndexKeysDefinition<TDerived> index, CreateIndexOptions options)
//            where TDerived : T
//        {
//            _collection.OfType<TDerived>().Indexes.CreateOne(index, options);
//        }

//        public void DeleteIndex(string name)
//        {
//            if (_SystemIndexes.Contains(name))
//                return;

//            _collection.Indexes.DropOne(name);
//        }

//        public bool ContainsIndex(string name)
//        {
//            return GetIndexes().Contains(name);
//        }

//        public IEnumerable<string> GetIndexes()
//        {
//            var indexes = _collection.Indexes.List().ToList();
//            return indexes.Select(x => x["name"].AsString).Except(_SystemIndexes);
//        }

//        private FilterDefinition<T> BuildDerivedFromFilter<TDerived>()
//        {
//            var type = typeof(TDerived);
//            var derivedTypes = _discriminator.KnownTypes
//                .Where(x => x.IsDerivedFrom(type))
//                .ToList();

//            if (!derivedTypes.Contains(type))
//                derivedTypes.Add(type);

//            var derivedTypeNames = derivedTypes
//                .Select(x => _discriminator.GetTypeId(x))
//                .ToList();

//            return Builders<T>.Filter.In(_discriminator.ElementName, derivedTypeNames);
//        }
//    }
//}
