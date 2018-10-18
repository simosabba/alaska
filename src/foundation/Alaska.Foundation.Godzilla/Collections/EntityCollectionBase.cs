//using Alaska.Foundation.Godzilla.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Alaska.Foundation.Godzilla.Collections
//{
//    public abstract class EntityCollectionBase
//    {
//        private readonly Type _entityType;

//        public EntityCollectionBase(Type entityType)
//        {
//            _entityType = entityType;
//        }

//        public Type GetEntityType()
//        {
//            return _entityType;
//        }

//        public abstract string CollectionName { get; }

//        internal abstract IEnumerable<TEntity> AddEntitiesIntoCollection<TEntity>(IEnumerable<TEntity> items) where TEntity : IEntity;
//        internal abstract TEntity AddEntityToCollection<TEntity>(TEntity item) where TEntity : IEntity;
//        internal abstract IEnumerable<IEntity> AddEntitiesIntoCollection(IEnumerable<IEntity> items);
//        internal abstract IEntity AddEntityToCollection(IEntity item);
//        internal abstract void DeleteEntityFromCollection(Guid id);
//        internal abstract void DeleteEntitiesFromCollection(IEnumerable<Guid> ids);
//        internal abstract void DeleteEntitiesFromCollection(IEnumerable<IEntity> items);
//        internal abstract void DeleteEntityFromCollection(IEntity item);
//        internal abstract IEntity GetEntityFromCollection(Guid id);
//        internal abstract IEnumerable<IEntity> GetEntitiesFromCollection(IEnumerable<Guid> id);

//        public abstract IEnumerable<IEntity> AddEntities(IEnumerable<IEntity> items);
//        public abstract IEntity AddEntity(IEntity item);
//        public abstract void DeleteEntities(IEnumerable<IEntity> items);
//        public abstract void DeleteEntity(IEntity item);
//        public abstract void DeleteEntity(string id);
//        public abstract void DeleteEntity(Guid id);
//        public abstract IEnumerable<TEntity> GetEntities<TEntity>(IEnumerable<Guid> id) where TEntity : IEntity;
//        public abstract IEnumerable<TEntity> GetEntitiesExcept<TEntity>(IEnumerable<Guid> id) where TEntity : IEntity;
//        public abstract IEnumerable<IEntity> GetEntitiesExcept(IEnumerable<Guid> id);
//        public abstract IEnumerable<Guid> GetEntitiesId();
//        public abstract void UpdateEntities(IEnumerable<IEntity> items);
//        public abstract void UpdateEntity(IEntity item);
//    }
//}
