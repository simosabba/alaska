//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Collections
//{
//    internal class RelationshipCollection : DatabaseCollection<RelationshipEntryBase>,
//            IRelationshipCollection
//    {
//        private EntityContext _context;

//        public RelationshipCollection()
//        { }

//        public RelationshipEntry<T> AddRelation<T>(Guid sourceId, Guid targetId, T data)
//        {
//            return (RelationshipEntry<T>)AddItem(new RelationshipEntry<T>
//            {
//                RelationshipId = GetRelationshipId<T>(),
//                SourceEntityId = sourceId,
//                TargetEntityId = targetId,
//                Data = data,
//                CreationTime = DateTime.Now,
//                CreatedBy = _context.CurrentUser,
//            });
//        }

//        public void DeleteRelations<T>()
//        {
//            DeleteItems<RelationshipEntry<T>>(x => true);
//        }

//        public void DeleteRelations(IEnumerable<Guid> relations)
//        {
//            DeleteItems(relations);
//        }

//        public void DeleteRelation<T>(RelationshipEntry<T> relation)
//        {
//            DeleteItem(relation);
//        }

//        public void DeleteRelations<T>(IEnumerable<RelationshipEntry<T>> relations)
//        {
//            DeleteItems(relations);
//        }

//        public void DeleteRelation(Guid relationId)
//        {
//            DeleteItem(relationId);
//        }

//        public void UpdateRelation<T>(Guid relationId, T data)
//        {
//            var relation = GetItem<RelationshipEntry<T>>(relationId);
//            if (relation == null)
//                throw new RelationshipNotFoundException($"Relation {relationId} not found");

//            relation.Data = data;
//            UpdateRelation<T>(relation);
//        }

//        public void UpdateRelations<T>(IEnumerable<RelationshipEntry<T>> relations)
//        {
//            Check.IsNotNull(relations);
//            relations.ToList().ForEach(SetUpdateData);
//            UpdateItems(relations);
//        }

//        public void UpdateRelation<T>(RelationshipEntry<T> relation)
//        {
//            UpdateRelation((RelationshipEntryBase)relation);
//        }

//        public void UpdateRelation(RelationshipEntryBase relation)
//        {
//            Check.IsNotNull(relation);
//            SetUpdateData(relation);
//            UpdateItem(relation);
//        }

//        public RelationshipEntry<T> GetRelation<T>(Guid id)
//        {
//            Check.IsNotEmpty(id);
//            return (RelationshipEntry<T>)GetItem(id);
//        }

//        public RelationshipEntryBase GetRelation(Guid id)
//        {
//            Check.IsNotEmpty(id);
//            return GetItem(id);
//        }

//        public RelationshipEntry<T> GetEntityRelationFrom<T>(Guid entityId)
//        {
//            return GetItem<RelationshipEntry<T>>(x =>
//                entityId == x.SourceEntityId);
//        }

//        public IEnumerable<RelationshipEntry<T>> GetEntityRelationsFrom<T>(Guid entityId)
//        {
//            return GetItems<RelationshipEntry<T>>(x =>
//                entityId == x.SourceEntityId);
//        }

//        public IEnumerable<RelationshipEntry<T>> GetEntitiesRelationsFrom<T>(IEnumerable<Guid> entitiesId)
//        {
//            return GetItems<RelationshipEntry<T>>(x =>
//                entitiesId.Contains(x.SourceEntityId));
//        }

//        public RelationshipEntry<T> GetEntityRelationTo<T>(Guid entityId)
//        {
//            return GetItem<RelationshipEntry<T>>(x =>
//                entityId == x.TargetEntityId);
//        }

//        public IEnumerable<RelationshipEntry<T>> GetEntityRelationsTo<T>(Guid entityId)
//        {
//            return GetItems<RelationshipEntry<T>>(x =>
//                entityId == x.TargetEntityId);
//        }

//        public IEnumerable<RelationshipEntry<T>> GetEntitiesRelationsTo<T>(IEnumerable<Guid> entitiesId)
//        {
//            return GetItems<RelationshipEntry<T>>(x =>
//                entitiesId.Contains(x.TargetEntityId));
//        }

//        public RelationshipEntry<T> GetEntityRelation<T>(Guid entityId)
//        {
//            return GetItem<RelationshipEntry<T>>(x =>
//                entityId == x.SourceEntityId ||
//                entityId == x.TargetEntityId);
//        }

//        public IEnumerable<RelationshipEntry<T>> GetEntityRelations<T>(Guid entityId)
//        {
//            return GetItems<RelationshipEntry<T>>(x =>
//                entityId == x.SourceEntityId ||
//                entityId == x.TargetEntityId);
//        }

//        public IEnumerable<RelationshipEntry<T>> GetEntitiesRelations<T>(IEnumerable<Guid> entitiesId)
//        {
//            return GetItems<RelationshipEntry<T>>(x =>
//                entitiesId.Contains(x.SourceEntityId) ||
//                entitiesId.Contains(x.TargetEntityId));
//        }

//        public IEnumerable<RelationshipEntry<T>> GetRelationsFrom<T>(IEntity from)
//        {
//            Check.IsNotNull(from);

//            switch (GetRelationshipDirection<T>())
//            {
//                case RelationDirection.Monodirectional:
//                    return GetItems<RelationshipEntry<T>>(x =>
//                        x.SourceEntityId == from.Id);
//                case RelationDirection.Bidirectional:
//                    return GetItems<RelationshipEntry<T>>(x =>
//                        x.SourceEntityId == from.Id ||
//                        x.TargetEntityId == from.Id);
//                default:
//                    throw new NotImplementedException();
//            }
//        }

//        public IEnumerable<RelationshipEntry<T>> GetRelationsTo<T>(IEntity to)
//        {
//            Check.IsNotNull(to);

//            var id = GetRelationshipTypesId<T>();
//            switch (GetRelationshipDirection<T>())
//            {
//                case RelationDirection.Monodirectional:
//                    return GetItems<RelationshipEntry<T>>(x =>
//                        id.Contains(x.RelationshipId) &&
//                        x.TargetEntityId == to.Id);
//                case RelationDirection.Bidirectional:
//                    return GetItems<RelationshipEntry<T>>(x =>
//                        id.Contains(x.RelationshipId) &&
//                        (x.TargetEntityId == to.Id || x.SourceEntityId == to.Id));
//                default:
//                    throw new NotImplementedException();
//            }
//        }

//        public IEnumerable<RelationshipEntry<T>> GetRelations<T>(IEntity from, IEntity to)
//        {
//            Check.IsNotNull(from);
//            Check.IsNotNull(to);

//            switch (GetRelationshipDirection<T>())
//            {
//                case RelationDirection.Monodirectional:
//                    return GetItems<RelationshipEntry<T>>(x =>
//                        x.SourceEntityId == from.Id && x.TargetEntityId == to.Id
//                        );
//                case RelationDirection.Bidirectional:
//                    return GetItems<RelationshipEntry<T>>(x =>
//                        (x.SourceEntityId == from.Id && x.TargetEntityId == to.Id ||
//                        x.TargetEntityId == from.Id && x.SourceEntityId == to.Id)
//                        );
//                default:
//                    throw new NotImplementedException();
//            }
//        }

//        public IEnumerable<RelationshipEntry<T>> GetRelations<T>(IEntity entity)
//        {
//            Check.IsNotNull(entity);
//            var entityId = entity.Id;

//            var relations = GetItems<RelationshipEntry<T>>(x =>
//                x.SourceEntityId == entityId ||
//                x.TargetEntityId == entityId
//                );
//            return relations;
//        }

//        public IEnumerable<RelationshipEntryBase> GetRelations(IEntity entity)
//        {
//            Check.IsNotNull(entity);
//            return GetItems(x =>
//                x.SourceEntityId == entity.Id ||
//                x.TargetEntityId == entity.Id);
//        }

//        private void SetUpdateData(RelationshipEntryBase relation)
//        {
//            relation.UpdateTime = DateTime.Now;
//            relation.UpdatedBy = _context.CurrentUser;
//        }

//        private Guid GetRelationshipId<T>()
//        {
//            return GetRelationshipDefinition<T>().Id;
//        }

//        private List<Guid> GetRelationshipTypesId<T>()
//        {
//            return GetRelationshipTypesId<T>(true);
//        }

//        private List<Guid> GetRelationshipTypesId<T>(bool includeBaseTypes)
//        {
//            var definition = GetRelationshipDefinition<T>();
//            var idList = new List<Guid> { definition.Id };
//            if (includeBaseTypes)
//                idList.AddRange(definition.BaseTypes.Select(x => x.TypeId));
//            return idList;
//        }

//        private RelationDirection GetRelationshipDirection<T>()
//        {
//            return GetRelationshipDefinition<T>().Direction;
//        }

//        private RelationshipDefinition GetRelationshipDefinition<T>()
//        {
//            return _context.Resolver.GetRelationship(typeof(T));
//        }
//    }
//}
