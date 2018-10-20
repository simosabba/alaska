using Alaska.Foundation.Core.Utils;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Entities.Containers;
using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Exceptions;
using Alaska.Foundation.Godzilla.Items;
using Alaska.Foundation.Godzilla.Queryable;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Alaska.Foundation.Godzilla.Services
{
    public abstract class EntityContext : IEntityContext
    {
        private readonly EntityContextOptions _options;
        private readonly PathBuilder _pathBuilder;
        private readonly EntityResolver _resolver;
        private readonly HierarchyCollection _hierarchyCollection;
        private readonly RelationshipCollection _relationshipCollection;
        private readonly RecycleBinCollection _recycleBinCollection;

        public EntityContext(EntityContextOptions options)
        {
            _options = options ?? throw new ArgumentException(nameof(options));
            _pathBuilder = new PathBuilder(options);
            _resolver = new EntityResolver();
            _hierarchyCollection = new HierarchyCollection(options.Collections, _resolver, _pathBuilder);
            _relationshipCollection = new RelationshipCollection(options.Collections, _resolver);
            _recycleBinCollection = new RecycleBinCollection(options.Collections);
        }

        internal EntityResolver Resolver => _resolver;
        internal EntityContextOptions Options => _options;
        internal HierarchyCollection Hierarchy => _hierarchyCollection;
        internal RelationshipCollection Relationships => _relationshipCollection;
        internal PathBuilder PathBuilder => _pathBuilder;

        #region Collections

        //public TCollection GetCollection<TCollection>()
        //    where TCollection : EntityCollectionBase
        //{
        //    return _resolver.GetCollection<TCollection>();
        //}

        public EntityCollectionBase GetEntityCollection<TEntity>()
            where TEntity : IEntity
        {
            return GetEntityCollection(typeof(TEntity));
        }

        public EntityCollectionBase GetEntityCollection(Type entityType)
        {
            return _resolver.ResolveCollection(entityType);
        }

        public EntityCollectionBase GetEntityCollection(IEntity entity)
        {
            return _resolver.ResolveCollection(entity.GetType());
        }

        #endregion

        #region Insert

        public IItem<TEntity> Insert<TEntity>(TEntity item)
            where TEntity : IEntity
        {
            return Insert<TEntity>(new List<TEntity> { item }).First();
        }

        public IEnumerable<IItem<TEntity>> Insert<TEntity>(List<TEntity> items)
            where TEntity : IEntity
        {
            return Insert<TEntity>(items.Select(x => (TEntity)x));
        }

        public IEnumerable<IItem<TEntity>> Insert<TEntity>(IEnumerable<TEntity> items)
            where TEntity : IEntity
        {
            var area = GetCollectionArea<TEntity>();
            if (area == null)
                throw new InvalidOperationException($"No default area defined for collection {typeof(TEntity).FullName}");

            return Insert<TEntity>(items, area);
        }

        public IItem<TEntity> Insert<TEntity>(TEntity item, IEntity parent)
            where TEntity : IEntity
        {
            return Insert<TEntity>(new List<TEntity> { item }, parent).First();
        }

        public IItem<TEntity> Insert<TEntity>(TEntity item, string path)
            where TEntity : IEntity
        {
            var parent = GetItem(path);
            return Insert<TEntity>(item, parent.Value);
        }

        public IEnumerable<IItem<TEntity>> Insert<TEntity>(IEnumerable<TEntity> items, IEntity parent)
            where TEntity : IEntity
        {
            var resolvedItems = items.ToList();
            if (!resolvedItems.Any())
                return new List<IItem<TEntity>>();

            var entities = resolvedItems.Select(x => (IEntity)x).ToList();
            EnsureIdAndName(entities);
            ValidateData(entities);
            ValidatePath(entities, parent);
            var newEntities = GetEntityCollection(typeof(TEntity)).AddEntitiesIntoCollection(resolvedItems);
            var hierarchyEntries = Hierarchy.Insert(entities, parent);
            return MapData<TEntity>(newEntities, hierarchyEntries);
        }

        public IItem Insert(IEntity item, string path)
        {
            var parent = GetItem(path);
            return Insert(item, parent.Value);
        }

        public IItem Insert(IEntity item, IEntity parent)
        {
            EnsureIdAndName(item);
            ValidateData(item);
            ValidatePath(item, parent);
            var newEntity = GetEntityCollection(item).AddEntityToCollection(item);
            var hierarchyEntry = Hierarchy.Insert(item, parent);
            return CreateItem(newEntity, hierarchyEntry);
        }

        #endregion

        #region Delete

        public void DeleteDescendants(Guid entityId)
        {
            var item = GetItem(entityId);
            if (item == null)
                throw new ItemNotFoundException($"Item {entityId} not found");
            DeleteDescendants(item.Value);
        }

        //public void DeleteDescendants(IEntity item)
        //{
        //    var descendants = GetDescendants(item, null)
        //        .Select(x => x.Value)
        //        .ToList();
        //    Delete(descendants);
        //}

        public void Delete(Guid entityId)
        {
            var item = GetItem(entityId);
            if (item == null)
                throw new ItemNotFoundException($"Item {entityId} not found");
            Delete(item.Value);
        }

        public void Delete(IEntity item)
        {
            Delete(new List<IEntity> { item });
        }

        public void Delete(IEnumerable<IItem> items)
        {
            Delete(items.Select(x => x.Value));
        }

        public void Delete(IEnumerable<IEntity> items)
        {
            var resolvedItems = items.ToList();
            if (!resolvedItems.Any())
                return;

            var allItemsToDelete = Hierarchy.GetDescendantsAndSelf(resolvedItems);
            var groupedItems = allItemsToDelete.GroupBy(x => x.CollectionName);
            foreach (var group in groupedItems)
                Delete(group.Key, group.ToList());
        }

        private void Delete(string collectionName, IEnumerable<HierarchyEntry> hierarchyItems)
        {
            AssertAreUnprotected(hierarchyItems);

            var collection = _resolver.ResolveCollection(collectionName);
            var idList = hierarchyItems.Select(x => x.ItemId);
            var entities = collection.GetEntitiesFromCollection(idList);

            RecycleItems(entities, hierarchyItems);

            collection.DeleteEntitiesFromCollection(entities);
            Hierarchy.DeleteItems(hierarchyItems);
        }

        private void RecycleItems(IEnumerable<IEntity> items, IEnumerable<HierarchyEntry> hierarchyEntries)
        {
            var hierarchyEntriesDict = hierarchyEntries.ToDictionary(x => x.ItemId);
            var thrashItems = items.Select(x => new RecycleBinElement
            {
                TemplateId = _resolver.ResolveTemplateId(x).ToString(),
                Name = x.EntityName,
                Value = x,
                HierarchyEntry = hierarchyEntriesDict[x.Id],
                DeletionState = DeletionState.SoftDeleted,
                DeletionTime = DateTime.Now,
            });

            _recycleBinCollection.AddItems(thrashItems);
        }

        #endregion

        #region Move

        public void Move(IEntity item, IEntity newParent)
        {
            AssertIsUnprotected(item);
            Hierarchy.Move(item, newParent);
        }

        #endregion

        #region Rename

        public void Rename(IEntity item, string newName)
        {
            AssertIsUnprotected(item);

            var parent = Hierarchy.GetParent(item.Id);
            var newPath = _pathBuilder.JoinPath(parent.Path, newName);
            if (Hierarchy.Exists(newPath))
                throw new PathAlreadyExistsException($"Path {newPath} already exists");

            var collection = GetEntityCollection(item);
            item.EntityName = newName;

            Hierarchy.Rename(item, newName);
            collection.UpdateEntity(item);
        }

        #endregion

        #region Update

        public void Update(IEntity item)
        {
            AssertIsUnprotected(item);
            ValidateData(item);

            GetEntityCollection(item).UpdateEntity(item);
            Hierarchy.SetUpdated(item, DateTime.Now);
        }

        public void Update<TEntity>(TEntity item)
            where TEntity : IEntity
        {
            Update<TEntity>(new List<TEntity> { item });
        }

        public void Update<TEntity>(IEnumerable<TEntity> items)
            where TEntity : IEntity
        {
            var resolvedItems = items.ToList();
            if (!resolvedItems.Any())
                return;

            var entities = resolvedItems.Select(x => (IEntity)x);
            AssertIsPresentiId(entities);
            AssertAreUnprotected(entities);
            ValidateData(entities);
            GetEntityCollection(typeof(TEntity)).UpdateEntities(entities);
            Hierarchy.SetUpdated(entities, DateTime.Now);
        }

        #endregion

        #region Get Item

        public IItem GetRootItem()
        {
            var root = Hierarchy.Root;
            if (root == null)
                throw new HierarchyItemNotFoundException("Cannot find root item");
            var value = GetEntityFromHierarchy(root);
            return CreateItem(value, root);
        }

        public IEnumerable<IItem<TEntity>> GetItems<TEntity>(IEnumerable<Guid> id)
            where TEntity : IEntity
        {
            var hierarchyItems = Hierarchy.GetHierarchyItems(id);
            return ResolveItems<TEntity>(hierarchyItems);
        }

        //public IEnumerable<IItem> GetItems(IEnumerable<Guid> id)
        //{
        //    var hierarchyItems = Hierarchy.GetHierarchyItems(id);
        //    return ResolveItems(hierarchyItems);
        //}

        public IItem GetItem(Guid entityId)
        {
            var hierarchyItem = Hierarchy.GetHierarchyItem(entityId);
            if (hierarchyItem == null)
                return null;

            var value = GetEntityFromHierarchy(hierarchyItem);
            return CreateItem(value, hierarchyItem);
        }

        public IItem GetItem(string path)
        {
            return GetItem(path, false);
        }

        internal IItem GetItem(string path, bool throwIfNotFound)
        {
            var hierarchyItem = Hierarchy.GetHierarchyItem(path);
            if (hierarchyItem == null)
            {
                if (throwIfNotFound)
                    throw new HierarchyItemNotFoundException($"No hierarchy item found for path {path}");
                return null;
            }

            var value = GetEntityFromHierarchy(hierarchyItem);
            return CreateItem(value, hierarchyItem);
        }

        public IItem<TEntity> GetItem<TEntity>(string path)
            where TEntity : IEntity
        {
            return GetItem<TEntity>(path, false);
        }

        internal IItem<TEntity> GetItem<TEntity>(string path, bool throwIfNotFound)
            where TEntity : IEntity
        {
            var hierarchyItem = Hierarchy.GetHierarchyItem(path);
            if (hierarchyItem == null)
            {
                if (throwIfNotFound)
                    throw new HierarchyItemNotFoundException($"No hierarchy item found for path {path}");
                return null;
            }

            var value = GetEntityFromHierarchy<TEntity>(hierarchyItem);
            return CreateItem<TEntity>(value, hierarchyItem);
        }

        public IItem<TEntity> GetItem<TEntity>(Guid entityId)
            where TEntity : IEntity
        {
            var entity = GetEntityCollection<TEntity>().GetEntities<TEntity>(new List<Guid> { entityId }).FirstOrDefault();
            if (entity == null)
                return null;
            return ResolveItems<TEntity>(new List<TEntity> { entity }).FirstOrDefault();
        }

        public bool ContainsItem(Guid entityId)
        {
            return Hierarchy.ContainsHierarchyItem(entityId);
        }

        private IEntity GetEntityFromHierarchy(HierarchyEntry hierarchyItem)
        {
            var collection = _resolver.GetCollection(hierarchyItem.CollectionName);
            var entityItem = collection.GetEntityFromCollection(hierarchyItem.ItemId);
            if (entityItem == null)
                throw new CollectionItemNotFoundException($"No collection item found for id {hierarchyItem.ItemId} into collection {hierarchyItem.CollectionName}");

            return entityItem;
        }

        private TEntity GetEntityFromHierarchy<TEntity>(HierarchyEntry hierarchyItem)
            where TEntity : IEntity
        {
            var collection = _resolver.GetCollection(hierarchyItem.CollectionName);
            var entityItem = collection
                .GetEntities<TEntity>(new List<Guid> { hierarchyItem.ItemId })
                .FirstOrDefault();

            if (entityItem == null)
                throw new CollectionItemNotFoundException($"No collection item found for id {hierarchyItem.ItemId} into collection {hierarchyItem.CollectionName}");

            return entityItem;
        }

        #endregion

        #region Get Entities

        public TEntity GetEntity<TEntity>(Guid id)
            where TEntity : IEntity
        {
            return GetEntityCollection<TEntity>()
                .GetEntities<TEntity>(new List<Guid> { id })
                .FirstOrDefault();
        }

        public IEnumerable<TEntity> GetEntities<TEntity>(IEnumerable<Guid> id)
            where TEntity : IEntity
        {
            return GetEntityCollection<TEntity>().GetEntities<TEntity>(id);
        }

        #endregion

        #region Search Items

        public IItem<TEntity> FindOne<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : IEntity
        {
            var entity = FindEntities(filter).FirstOrDefault();
            if (entity == null)
                return null;
            return ResolveItems<TEntity>(new List<TEntity> { entity }).FirstOrDefault();
        }

        public IEnumerable<IItem<TEntity>> Find<TEntity, TRelationsip>(Expression<Func<TEntity, bool>> filter, Func<TRelationsip, bool> relationshipFilter)
            where TEntity : IEntity
        {
            var resultItemsId = FindEntitiesId<TEntity, TRelationsip>(filter, relationshipFilter);
            return GetItems<TEntity>(resultItemsId);
        }

        public IEnumerable<IItem<TEntity>> Find<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : IEntity
        {
            var entities = FindEntities(filter);
            return ResolveItems<TEntity>(entities);
        }

        public IEnumerable<IItem<TEntity>> Find<TEntity>()
            where TEntity : IEntity
        {
            var templates = new List<string> { _resolver.ResolveTemplateId<TEntity>().ToString() };
            return Find<TEntity>(x => templates.Contains(x.TemplateId));
        }

        private IEnumerable<IItem<TEntity>> Find<TEntity>(Expression<Func<HierarchyEntry, bool>> filter)
            where TEntity : IEntity
        {
            var hierarchyItems = Hierarchy.GetItems(filter);
            var templates = new List<string> { _resolver.ResolveTemplateId<TEntity>().ToString() };
            //TODO: aggiungere template derivati
            var templateMatchingItems = hierarchyItems.Where(x => templates.Contains(x.TemplateId));
            return ResolveItems<TEntity>(templateMatchingItems);
        }

        //private IEnumerable<IItem> Find(Func<HierarchyEntry, bool> filter)
        //{
        //    var hierarchyItems = Hierarchy.GetItems(x => filter(x));
        //    return ResolveItems(hierarchyItems);
        //}

        private IEnumerable<Guid> FindEntitiesId<TEntity, TRelationsip>(Expression<Func<TEntity, bool>> filter, Func<TRelationsip, bool> relationshipFilter)
        {
            var itemsId = FindEntitiesId<TEntity>(filter);
            var relations = _relationshipCollection.GetEntitiesRelations<TRelationsip>(itemsId);
            var filteredRelations = relations.Where(x => relationshipFilter(x.Data)).ToList();
            var filteredItemsByRelations = filteredRelations
                .Select(x => x.SourceEntityId)
                .Union(filteredRelations.Select(x => x.TargetEntityId))
                .Distinct()
                .ToList();

            return itemsId
                .Intersect(filteredItemsByRelations)
                .ToList();
        }

        private IEnumerable<Guid> FindEntitiesId<TEntity>(Expression<Func<TEntity, bool>> filter)
        {
            return InvokeEntityCollectionMethod<TEntity, IEnumerable<Guid>>("GetEntitiesId", filter);
        }

        private IEnumerable<TEntity> FindEntities<TEntity>(Expression<Func<TEntity, bool>> filter)
        {
            return InvokeEntityCollectionMethod<TEntity, IEnumerable<TEntity>>("GetEntities", filter);
        }

        private TResult InvokeEntityCollectionMethod<TEntity, TResult>(string methodName, Expression<Func<TEntity, bool>> filter)
        {
            var collection = GetEntityCollection(typeof(TEntity));
            var methods = collection.GetType()
                .GetMethods()
                .Where(x => x.Name.Equals(methodName) && x.IsPublic && x.IsGenericMethod)
                .ToList();

            var getMethod = ReflectionUtil.GetGenericMethod(
                collection.GetType(),
                BindingFlags.Instance | BindingFlags.Public,
                methodName,
                "System.Linq.Expressions.Expression`1[System.Func`2[TEntity,System.Boolean]]");

            var g = getMethod.MakeGenericMethod(typeof(TEntity));
            return (TResult)g.Invoke(collection, new object[] { filter });
        }

        #endregion

        #region Queryable

        public IQueryable<IQueryableItem> AsItemQueryable()
        {
            return new EntityContextQueryableData<IQueryableItem>(this);
        }

        public IQueryable<IQueryableItem<TEntity>> AsItemQueryable<TEntity>()
            where TEntity : IEntity
        {
            return new EntityContextQueryableData<IQueryableItem<TEntity>>(this);
        }

        public IQueryable<TEntity> AsQueryable<TEntity>()
            where TEntity : IEntity
        {
            var collection = GetEntityCollection(typeof(TEntity));

            //TEntity is the base type for the collection
            if (collection is EntityCollection<TEntity> baseTypeCollection)
                return baseTypeCollection.AsQueryable();

            //TEntity is a derived type for the collection
            var asQueryable = ReflectionUtil.GetGenericMethods(
                collection.GetType(),
                BindingFlags.Instance | BindingFlags.Public,
                "AsQueryable")
                .Where(x => x.IsGenericMethodDefinition)
                .FirstOrDefault();
            var asDerivedQueryable = asQueryable.MakeGenericMethod(typeof(TEntity));
            return (IQueryable<TEntity>)asDerivedQueryable.Invoke(collection, new object[0]);
        }

        #endregion

        #region Explore Items

        public string GetPath(IEntity entity)
        {
            var item = Hierarchy.GetParent(entity.Id);
            if (item == null)
                throw new HierarchyItemNotFoundException($"No hierarchy item found for entity {entity.Id}");
            return item.Path;
        }

        public bool HasChildren(IEntity entity)
        {
            return Hierarchy.HasChildren(entity);
        }

        public IItem GetParent(IEntity entity)
        {
            var hierarchyItem = Hierarchy.GetParent(entity.Id);
            if (hierarchyItem == null)
            {
                if (Hierarchy.IsRootEntity(entity))
                    return null;
                throw new HierarchyItemNotFoundException($"No hierarchy parent found for entity {entity.Id}");
            }

            var value = GetEntityFromHierarchy(hierarchyItem);
            return CreateItem(value, hierarchyItem);
        }

        public IEnumerable<string> GetChildrenNames(IEntity entity)
        {
            var hierarchyItems = Hierarchy.GetChildren(entity.Id);
            return hierarchyItems
                //.Select(x => FilterAndCleanOrphans(x))
                .Where(x => x != null)
                .Select(x => x.Name);
        }

        public IEnumerable<IItem> GetChildren(IEntity entity)
        {
            return GetChildren(entity.Id);
        }

        //public IEnumerable<IItem> GetChildren(Guid entityId)
        //{
        //    var hierarchyItems = Hierarchy.GetChildren(entityId);
        //    var items = ResolveItems(hierarchyItems);

        //    return items
        //        .Where(x => x.Value != null)
        //        .OrderBy(x => x.Name);
        //}

        public IEnumerable<IItem<TEntity>> GetChildren<TEntity>(IEntity entity)
            where TEntity : IEntity
        {
            var hierarchyItems = Hierarchy.GetChildren(entity);
            var items = ResolveItems<TEntity>(hierarchyItems);

            return items
                .Where(x => x.Value != null)
                .OrderBy(x => x.Name);
        }

        public IEnumerable<IItem<TEntity>> GetChildren<TEntity>(IEntity entity, Expression<Func<TEntity, bool>> filter)
            where TEntity : IEntity
        {
            var hierarchyItems = Hierarchy.GetChildren(entity);
            var items = ResolveItems<TEntity>(hierarchyItems, filter);

            return items
                .Where(x => x.Value != null)
                .OrderBy(x => x.Name);
        }

        //public IEnumerable<IItem> GetDescendants(IEntity entity, int? depth)
        //{
        //    var hierarchyItems = depth.HasValue ?
        //        Hierarchy.GetDescendants(entity, depth.Value) :
        //        Hierarchy.GetDescendants(entity);
        //    var items = ResolveItems(hierarchyItems);

        //    return items
        //        .Where(x => x.Value != null)
        //        .OrderBy(x => x.Name);
        //}

        public IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(IEntity entity)
            where TEntity : IEntity
        {
            return GetDescendants<TEntity>(entity, null, null);
        }

        public IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(IEntity entity, int? depth)
            where TEntity : IEntity
        {
            return GetDescendants<TEntity>(entity, null, depth);
        }

        public IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(IEntity entity, Expression<Func<TEntity, bool>> filter, int? depth)
            where TEntity : IEntity
        {
            var hierarchyItems = depth.HasValue ?
                Hierarchy.GetDescendants(entity, depth.Value) :
                Hierarchy.GetDescendants(entity);

            var items = ResolveItems<TEntity>(hierarchyItems, filter);

            return items
                .Where(x => x.Value != null)
                .OrderBy(x => x.Name);
        }

        //internal IItem ResolveItem(HierarchyEntry entry)
        //{
        //    return ResolveItems(new List<HierarchyEntry> { entry }).FirstOrDefault();
        //}

        private IEnumerable<IItem<TEntity>> ResolveItems<TEntity>(IEnumerable<TEntity> entries)
            where TEntity : IEntity
        {
            if (!entries.Any())
                return new List<IItem<TEntity>>();

            var hierarchyItems = Hierarchy
                .GetHierarchyItems(entries.Select(x => (IEntity)x))
                .ToList();

            return MapData<TEntity>(entries, hierarchyItems);
        }

        //internal IEnumerable<IItem> ResolveItems(IEnumerable<HierarchyEntry> entries)
        //{
        //    var entitiesInfo = new Dictionary<Guid, EntityData>();
        //    foreach (var entry in entries)
        //        entitiesInfo[entry.ItemId] = new EntityData(null, entry);

        //    var groups = entries.GroupBy(x => x.CollectionName);
        //    foreach (var group in groups)
        //    {
        //        var collection = _resolver.GetCollection(group.Key);
        //        var entities = collection.GetEntitiesFromCollection(group.Select(x => x.ItemId));
        //        entities.ToList().ForEach(x => entitiesInfo[x.Id].Entity = x);
        //    }

        //    return entitiesInfo.Values
        //        .Where(x => x.Entity != null)
        //        .Select(CreateItem)
        //        .ToList();
        //}

        //private IEnumerable<IItem<TEntity>> ResolveItems<TEntity>(IEnumerable<HierarchyEntry> entries)
        //    where TEntity : IEntity
        //{
        //    return ResolveItems<TEntity>(entries, null);
        //}

        //private IEnumerable<IItem<TEntity>> ResolveItems<TEntity>(IEnumerable<HierarchyEntry> entries, Expression<Func<TEntity, bool>> filter)
        //    where TEntity : IEntity
        //{
        //    if (!entries.Any())
        //        return new List<IItem<TEntity>>();

        //    var entitiesInfo = new Dictionary<Guid, EntityData<TEntity>>();
        //    foreach (var entry in entries)
        //        entitiesInfo[entry.ItemId] = new EntityData<TEntity>(default(TEntity), entry);
            
        //    var collectionName = 
        //    var group = entries
        //        .GroupBy(x => x.CollectionName)
        //        .First(x => x.Key.Equals(collectionName, StringComparison.OrdinalIgnoreCase));

        //    var collection = _resolver.GetCollection(group.Key);
        //    var idList = group.Select(x => x.ItemId).ToList();
        //    var idFilter = FilterBuilder<TEntity>.Filter(x => idList.Contains(x.Id));
        //    var entityFilter = filter == null ?
        //        idFilter :
        //        FilterBuilder<TEntity>.And(idFilter, FilterBuilder<TEntity>.Filter(filter));

        //    var entities = collection.GetEntities<TEntity>(entityFilter);
        //    entities.ToList().ForEach(x => entitiesInfo[x.Id].Entity = x);

        //    return entitiesInfo.Values
        //        .Where(x => x.Entity != null)
        //        .Select(CreateItem<TEntity>)
        //        .ToList();
        //}

        #endregion

        #region Conversions

        private IItem CreateItem(IEntity entity, IEntityInfo info)
        {
            return CreateItem(new EntityData(entity, info));
        }

        private IItem CreateItem(IEntityData data)
        {
            return new Item(this, data);
        }

        private IItem<TEntity> CreateItem<TEntity>(TEntity entity, IEntityInfo info)
            where TEntity : IEntity
        {
            return CreateItem<TEntity>(new EntityData<TEntity>(entity, info));
        }

        private IItem<TEntity> CreateItem<TEntity>(IEntityData<TEntity> data)
            where TEntity : IEntity
        {
            return new Item<TEntity>(this, data);
        }

        #endregion

        #region Protection

        public void ProtectItem(IEntity entity)
        {
            Hierarchy.Protect(entity);
        }

        public void UnprotectItem(IEntity entity)
        {
            Hierarchy.Unprotect(entity);
        }

        private void AssertIsPresentiId(IEnumerable<IEntity> entries)
        {
            entries.ToList().ForEach(x => Check.IsNotEmpty(x.Id));
        }

        private void AssertAreUnprotected(IEnumerable<IEntity> entries)
        {
            if (IsInsideProtectedContext())
                return;

            var hierarchyEntries = Hierarchy.GetHierarchyItems(entries);
            AssertAreUnprotected(hierarchyEntries);
        }

        private void AssertAreUnprotected(IEnumerable<HierarchyEntry> entries)
        {
            if (IsInsideProtectedContext())
                return;

            entries.ToList().ForEach(x => AssertIsUnprotected(x));
        }

        private void AssertIsUnprotected(IEntity entry)
        {
            if (IsInsideProtectedContext())
                return;

            var hierarchyEntry = Hierarchy.GetHierarchyItem(entry);
            AssertIsUnprotected(hierarchyEntry);
        }

        private void AssertIsUnprotected(HierarchyEntry entry)
        {
            if (IsInsideProtectedContext())
                return;

            if (entry.State != ItemState.Unprotected)
                throw new InvalidItemStateException($"Cannot modify items under protection mode (itemId: {entry.ItemId})");
        }

        private bool IsInsideProtectedContext()
        {
            return ItemProtectionContext.ProtectionState == ItemState.Protected;
        }

        #endregion

        #region Validation

        internal void ValidatePath(IEnumerable<IEntity> entities, IEntity parent)
        {
            var duplicateNames = entities.GroupBy(x => x.EntityName)
                .Where(x => x.Count() > 1)
                .Select(x => x.First());
            if (duplicateNames.Any())
                throw new DuplicateEntityException($"Duplicate entity names: {string.Join(", ", duplicateNames)}");

            var childrenNames = GetChildrenNames(parent);
            var existingNames = entities
                .Select(x => x.EntityName)
                .Where(x => childrenNames.Contains(x, StringComparer.OrdinalIgnoreCase));
            if (existingNames.Any())
                throw new DuplicateEntityException($"Items already exists with same name: {string.Join(", ", existingNames)} under choosen parent");
        }

        internal void ValidatePath(IEntity entity, IEntity parent)
        {
            ValidatePath(entity.EntityName, parent);
        }

        internal void ValidatePath(string name, IEntity parent)
        {
            var childrenNames = GetChildrenNames(parent);
            if (childrenNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                throw new DuplicateEntityException($"Items already exists with same name: {name} under choosen parent");
        }

        internal void ValidateData(IEnumerable<IEntity> entities)
        {
            entities.ToList().ForEach(x => ValidateData(x));
        }

        internal void ValidateData(IEntity entity)
        {
            //todo
            //_entityValidationHelper.Validate(entity);
        }

        //private HierarchyEntry FilterAndCleanOrphans(HierarchyEntry entry)
        //{
        //    var resolvedItem = ResolveItem(entry);
        //    if (resolvedItem == null)
        //    {
        //        Hierarchy.DeleteItem(entry);
        //        return null;
        //    }
        //    return entry;
        //}

        private void EnsureIdAndName(IEnumerable<IEntity> entity)
        {
            entity.ToList().ForEach(EnsureIdAndName);
        }

        private void EnsureIdAndName(IEntity entity)
        {
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.EntityName))
                entity.EntityName = entity.Id.ToString();
        }

        private IEnumerable<IItem> MapData(IEnumerable<IEntity> entities, IEnumerable<HierarchyEntry> hierarchyEntries)
        {
            var items = new List<IItem>();
            var hierarchyDict = hierarchyEntries.ToDictionary(x => x.ItemId);
            foreach (var entity in entities)
            {
                if (!hierarchyDict.ContainsKey(entity.Id))
                    continue;
                var hierarchyItem = hierarchyDict[entity.Id];
                var item = CreateItem(entity, hierarchyItem);
                items.Add(item);
            }
            return items;
        }

        private IEnumerable<IItem<TEntity>> MapData<TEntity>(IEnumerable<TEntity> entities, IEnumerable<HierarchyEntry> hierarchyEntries)
            where TEntity : IEntity
        {
            var items = new List<IItem<TEntity>>();
            var hierarchyDict = hierarchyEntries.ToDictionary(x => x.ItemId);
            foreach (var entity in entities)
            {
                if (!hierarchyDict.ContainsKey(entity.Id))
                    continue;
                var hierarchyItem = hierarchyDict[entity.Id];
                var item = CreateItem<TEntity>(entity, hierarchyItem);
                items.Add(item);
            }
            return items;
        }

        #endregion

        #region Areas

        public CollectionArea GetCollectionArea<TEntity>()
        {
            return GetCollectionArea(typeof(TEntity));
        }

        public CollectionArea GetCollectionArea(Type entityType)
        {
            //var id = _entityTypesMap.GetTemplateId(entityType);
            //return ContainerCollection.GetCollectionArea(id);
        }

        #endregion
    }
}
