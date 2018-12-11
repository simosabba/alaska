//using Alaska.Foundation.Godzilla.Abstractions;
//using Alaska.Foundation.Godzilla.Entries;
//using Alaska.Foundation.Godzilla.Items;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Services
//{
//    internal class DataIntegrityHelper
//    {
//        private EntityContext _context;

//        public DataIntegrityHelper(EntityContext context)
//        {
//            _context = context;
//        }

//        public IEnumerable<IItem> GetOrphanItems()
//        {
//            var allHierarchyItemsId = _context.Hierarchy
//                .GetAllEntitiesId();

//            var allParentsId = _context.Hierarchy
//                .SelectFieldValues(x => x.ParentId)
//                .ToList();

//            var missingParentsId = allParentsId
//                .Except(allHierarchyItemsId)
//                .ToList();

//            var orphansHierarchyItemsId = _context.Hierarchy
//                .GetItems(x => missingParentsId.Contains(x.ParentId))
//                .ToList();

//            return _context.ResolveItems(orphansHierarchyItemsId)
//                .Where(x => !_context.Hierarchy.IsRootEntity(x.Value))
//                .ToList();
//        }

//        public IEnumerable<HierarchyEntry> GetOrphanHierarchyEntries()
//        {
//            var allEntitiesId = GetCollections()
//                .SelectMany(x => x.GetEntitiesId())
//                .ToList();

//            return _context.Hierarchy
//                .GetItems(x => !allEntitiesId.Contains(x.ItemId))
//                .ToList();
//        }

//        public IEnumerable<EntityCollectionMap> GetOrphanEntities()
//        {
//            var nonOrphanItemsId = _context.Hierarchy.GetAllEntitiesId();

//            var orphanItems = GetCollections()
//                .Select(c => new EntityCollectionMap
//                {
//                    Collection = c,
//                    Entities = c.GetEntitiesExcept(nonOrphanItemsId).ToList(),
//                })
//                .ToList();

//            return orphanItems;
//        }

//        public void DeleteOrphanHierarchyEntries()
//        {
//            using (new ItemProtectionContext(ItemOrigin.System))
//            {
//                var orphans = GetOrphanHierarchyEntries();
//                _context.Hierarchy.DeleteItems(orphans);
//            }
//        }

//        public void DeleteOrphanHierarchyEntry(Guid entryId)
//        {
//            using (new ItemProtectionContext(ItemOrigin.System))
//            {
//                var entry = _context.Hierarchy.GetItem(entryId);
//                if (entry == null)
//                    throw new HierarchyItemNotFoundException($"Hierarchy item {entryId} not found");

//                if (_context.ResolveItem(entry) != null)
//                    throw new InvalidOperationException($"Hierarchy item {entryId} not orphan");

//                _context.Hierarchy.DeleteItem(entry);
//            }
//        }

//        public void DeleteOrphanItems()
//        {
//            using (new ItemProtectionContext(ItemOrigin.System))
//            {
//                var orphanItems = GetOrphanItems();
//                _context.Delete(orphanItems);
//            }
//        }

//        public void DeleteOrphanEntities()
//        {
//            using (new ItemProtectionContext(ItemOrigin.System))
//            {
//                var orphansData = GetOrphanEntities();
//                foreach (var orphan in orphansData)
//                    orphan.Collection.DeleteEntitiesFromCollection(orphan.Entities);
//            }
//        }

//        public void DeleteOrphanEntity(string collectionName, Guid entityId)
//        {
//            using (new ItemProtectionContext(ItemOrigin.System))
//            {
//                var entity = _context.GetItem(entityId);
//                if (entity != null)
//                    throw new InvalidOperationException($"Entity {entityId} not orphan");

//                var collection = _context.Resolver.GetCollection(collectionName);
//                collection.DeleteEntityFromCollection(entityId);
//            }
//        }

//        private IEnumerable<IEntityCollection> GetCollections()
//        {
//            return _context.Resolver.Collections
//                .Select(x => x.Collection)
//                .ToList();
//        }
//    }
//}
