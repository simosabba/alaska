using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Exceptions;
using Alaska.Foundation.Godzilla.Extensions;
using Alaska.Foundation.Godzilla.Items;
using Alaska.Foundation.Godzilla.Services;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Alaska.Foundation.Godzilla.Collections
{
    internal class HierarchyCollection : DatabaseCollection<HierarchyEntry>
    {
        private readonly PathBuilder _pathBuilder;

        public HierarchyCollection(
            DatabaseCollectionOptions options,
            PathBuilder pathBuilder)
            : base(options)
        {
            _pathBuilder = pathBuilder;
        }

        internal void CreateRoot(IEntity rootEntity)
        {
            var rootEntry = new HierarchyEntry
            {
                CreationTime = DateTime.Now,
                Name = string.Empty,
                Path = _pathBuilder.RootPath,
                ItemId = rootEntity.Id,
                ParentId = Guid.Empty,
                //CollectionName = GetCollectionName(rootEntity.GetType()),
                //TemplateId = GetTemplateId(rootEntity.GetType()).ToString(),
                Origin = ItemOrigin.System,
                State = ItemState.Protected,
            };
            AddItem(rootEntry);
        }

        public HierarchyEntry Root => GetItem(x => x.Path == _pathBuilder.RootPath);

        public HierarchyEntry GetParent(Guid entityId)
        {
            var parentId = GetHierarchyEntry(entityId, true).ParentId;
            return GetHierarchyEntry(parentId, true);
        }

        public HierarchyEntry GetHierarchyItem(IEntity entry)
        {
            return GetHierarchyItem(entry.Id);
        }

        public HierarchyEntry GetHierarchyItem(Guid entityId)
        {
            return GetHierarchyEntry(entityId, false);
        }

        public HierarchyEntry GetHierarchyItem(string path)
        {
            return GetHierarchyEntry(path, false);
        }

        public IEnumerable<HierarchyEntry> GetHierarchyItems(IEnumerable<IEntity> entries)
        {
            return GetHierarchyItems(entries.Select(x => x.Id));
        }

        public IEnumerable<HierarchyEntry> GetHierarchyItems(IEnumerable<Guid> entitiesId)
        {
            return GetItems(x => entitiesId.Contains(x.ItemId));
        }

        public IEnumerable<HierarchyEntry> GetChildren(IEntity entity)
        {
            return GetChildren(entity.Id);
        }

        public IEnumerable<HierarchyEntry> GetChildren(Guid entityId)
        {
            return GetItems(x => x.ParentId == entityId);
        }

        public IEnumerable<HierarchyEntry> GetDescendants(IEntity entity)
        {
            var itemPath = GetHierarchyEntry(entity, true).Path;
            return GetItems(x => x.Path != itemPath && x.Path.ToLower().StartsWith(itemPath));
        }

        public IEnumerable<HierarchyEntry> GetDescendants(IEntity entity, int depth)
        {
            var itemPath = GetHierarchyEntry(entity, true).Path;
            var pathsRegex = _pathBuilder.GetDescendantsRegex(itemPath, depth);
            return GetItems(x => x.Path, pathsRegex);
        }

        public IEnumerable<HierarchyEntry> GetDescendantsAndSelf(IEntity entity)
        {
            return GetDescendantsAndSelf(new List<IEntity> { entity });
        }

        public IEnumerable<HierarchyEntry> GetDescendantsAndSelf(IEnumerable<IEntity> entities)
        {
            var hierarchyEntities = GetHierarchyItems(entities.Select(x => x.Id));
            var paths = hierarchyEntities.Select(x => x.Path);
            var pathsRegex = new Regex($"^({string.Join("|", paths)})", RegexOptions.IgnoreCase);
            var items = GetItems(x => x.Path, pathsRegex);
            return items.GroupBy(x => x.Id).Select(x => x.First()).ToList();
        }

        internal IEnumerable<Guid> GetAllEntitiesId()
        {
            return SelectFieldValues(x => x.ItemId).ToList();
        }

        internal IEnumerable<Guid> GetEntitiesId(Expression<Func<HierarchyEntry, bool>> filter)
        {
            return SelectFieldValues(x => x.ItemId, filter).ToList();
        }

        public bool Exists(string path)
        {
            return GetHierarchyEntry(path, false) != null;
        }

        public bool ContainsHierarchyItem(Guid entityId)
        {
            return ContainsItem(x => x.ItemId == entityId);
        }

        public bool IsRootEntity(IEntity entity)
        {
            return Root.ItemId.Equals(entity.Id);
        }

        public bool HasChildren(IEntity entity)
        {
            return ContainsItem(x => x.ParentId == entity.Id);
        }

        public void SetUpdated(IEntity entity, DateTime whenUpdated)
        {
            SetUpdated(new List<IEntity> { entity }, whenUpdated);
        }

        public void SetUpdated(IEnumerable<IEntity> entities, DateTime whenUpdated)
        {
            var items = GetHierarchyItems(entities);
            foreach (var item in items)
            {
                item.UpdateTime = whenUpdated;
            }
            UpdateItems(items);
        }

        public void Move(IEntity item, IEntity newParent)
        {
            var hierarchiyItem = GetHierarchyEntry(item, true);
            if (hierarchiyItem.ParentId == newParent.Id)
                throw new HierarchyItemUpdateException("New parent and current parent are equals");

            var currentPath = hierarchiyItem.Path;
            var destinationPath = GetHierarchyEntry(newParent, true).Path;
            var newPath = _pathBuilder.NestPath(currentPath, destinationPath);

            hierarchiyItem.ParentId = newParent.Id;

            UpdateItem(hierarchiyItem);
            ReplacePath(currentPath, newPath);
        }

        public void Rename(IEntity item, string newName)
        {
            var hierarchiyItem = GetHierarchyEntry(item, true);
            if (hierarchiyItem.Name == newName)
                throw new HierarchyItemUpdateException("New name and current name are equals");

            var currentPath = hierarchiyItem.Path;
            var newPath = _pathBuilder.RenameLeaf(currentPath, newName);

            hierarchiyItem.Name = newName;
            hierarchiyItem.Path = newPath;

            UpdateItem(hierarchiyItem);
            ReplacePath(currentPath, newPath);
        }

        private void ReplacePath(string currentPath, string newPath)
        {
            var entries = GetItems(x => x.Path.StartsWith(currentPath.ToLower())).ToList();
            entries.ForEach(x => x.Path = x.Path.ReplaceStart(currentPath, newPath, StringComparison.Ordinal));
            UpdateItems(entries);
        }

        public void Delete(IEntity item)
        {
            Delete(new List<IEntity> { item });
        }

        public void Delete(IEnumerable<IEntity> items)
        {
            var idList = items.Select(x => x.Id).ToList();
            DeleteItems(x => idList.Contains(x.ItemId));
        }

        public HierarchyEntry Insert(IEntity item, IEntity parent)
        {
            var items = Insert(new List<IEntity> { item }, parent);
            if (!items.Any())
                throw new HierarchyItemUpdateException($"Error inserting hierarchy entry {item.Id}");
            return items.First();
        }

        public IEnumerable<HierarchyEntry> Insert(IEnumerable<IEntity> items, IEntity parent)
        {
            var parentPath = GetHierarchyEntry(parent, true).Path;
            var state = ItemProtectionContext.ProtectionState;
            var origin = ItemProtectionContext.Origin;
            var entries = items.Select(x => new HierarchyEntry
            {
                CreationTime = DateTime.Now,
                Path = _pathBuilder.AddChild(parentPath, x.EntityName),
                Name = x.EntityName,
                ItemId = x.Id,
                ParentId = parent.Id,
                //CollectionName = GetCollectionName(x.GetType()),
                //TemplateId = GetTemplateId(x.GetType()).ToString(),
                Origin = origin,
                State = state,
            }).ToList();

            return AddItems(entries);
        }

        public void Protect(IEntity entity)
        {
            SetProtectionState(entity, ItemState.Protected);
        }

        public void Unprotect(IEntity entity)
        {
            SetProtectionState(entity, ItemState.Unprotected);
        }

        protected void SetProtectionState(IEntity entity, ItemState state)
        {
            var entry = GetHierarchyEntry(entity, true);
            entry.State = state;
            UpdateItem(entry);
        }

        protected HierarchyEntry GetHierarchyEntry(IEntity item, bool throwIfNotFound)
        {
            return GetHierarchyEntry(item.Id, throwIfNotFound);
        }

        protected HierarchyEntry GetHierarchyEntry(Guid itemId, bool throwIfNotFound)
        {
            var hierarchyItem = GetItem(x => x.ItemId == itemId);
            if (hierarchyItem == null && throwIfNotFound)
                throw new HierarchyItemNotFoundException($"Hierarchy item not found for item {itemId}");
            return hierarchyItem;
        }

        protected HierarchyEntry GetHierarchyEntry(string path, bool throwIfNotFound)
        {
            var hierarchyItem = GetItem(x =>
                x.Path == path.EnsureSuffix(_pathBuilder.PathSeparator).ToLower());

            if (hierarchyItem == null && throwIfNotFound)
                throw new HierarchyItemNotFoundException($"Hierarchy item not found for path {path}");
            return hierarchyItem;
        }

        //protected string GetCollectionName(Type type)
        //{
        //    return Resolver.GetCollectionName(type);
        //}

        //protected Guid GetTemplateId(Type type)
        //{
        //    return TypesMap.GetTemplateId(type);
        //}
    }
}
