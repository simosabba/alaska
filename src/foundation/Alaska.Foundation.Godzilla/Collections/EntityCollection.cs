using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Alaska.Foundation.Godzilla.Collections
{
    internal class EntityCollection<T> :
            EntityCollectionBase,
            IEntityCollection<T>
            where T : IEntity
    {
        #region Init

        private readonly IDatabaseCollection<T> _collection;
        private readonly EntityContext _context;

        public EntityCollection(
            EntityContext context,
            string collectionName)
        {
            _context = context;
            _collection = new DatabaseCollection<T>(collectionName, context.Options.Collections);
        }
        
        #endregion

        #region Count

        public long Count()
        {
            return _collection.Count();
        }

        public long Count<TEntity>()
            where TEntity : T
        {
            return _collection.Count<TEntity>();
        }

        #endregion

        #region Contains

        public bool ContainsEntity(Guid id)
        {
            return _collection.ContainsItem(id);
        }

        bool ContainsEntity(T item)
        {
            return _collection.ContainsItem(item);
        }

        public bool ContainsEntity(Expression<Func<T, bool>> filter)
        {
            return _collection.ContainsItem(filter);
        }

        public bool ContainsEntity<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : T
        {
            return _collection.ContainsItem<TEntity>(filter);
        }

        #endregion

        #region Queryable

        public IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public IQueryable<TDerived> AsQueryable<TDerived>()
            where TDerived : T
        {
            return _collection.AsQueryable<TDerived>();
        }

        #endregion

        #region Get

        public T GetEntity(string id)
        {
            return GetEntity(new Guid(id));
        }

        public T GetEntity(Guid id)
        {
            return _collection.GetItem(id);
        }

        public T GetEntity(Expression<Func<T, bool>> filter)
        {
            return _collection.GetItem(filter);
        }

        public TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : T
        {
            return _collection.GetItem<TEntity>(filter);
        }

        public TEntity GetEntity<TEntity>(Guid id)
            where TEntity : T
        {
            return _collection.GetItem<TEntity>(id);
        }

        public IEnumerable<T> GetEntities(IEnumerable<Guid> id)
        {
            return _collection.GetItems(x => id.Contains(x.Id));
        }

        public IEnumerable<T> GetEntities()
        {
            return _collection.GetItems();
        }

        public IEnumerable<T> GetEntities(Expression<Func<T, bool>> filter)
        {
            return _collection.GetItems(filter);
        }

        public override IEnumerable<IEntity> GetEntitiesExcept(IEnumerable<Guid> id)
        {
            return _collection.GetItems(x => !id.Contains(x.Id))
                .Select(x => (IEntity)x);
        }

        public override IEnumerable<TEntity> GetEntitiesExcept<TEntity>(IEnumerable<Guid> id)
        {
            var getEntitiesMethod = this.GetType().GetMethod("GetDerivedEntitiesExcept", BindingFlags.Instance | BindingFlags.NonPublic);
            var method = getEntitiesMethod.MakeGenericMethod(typeof(TEntity));
            return (IEnumerable<TEntity>)method.Invoke(this, new object[] { id });
        }

        public override IEnumerable<TEntity> GetEntities<TEntity>(IEnumerable<Guid> id)
        {
            var getEntitiesMethod = this.GetType().GetMethod("GetDerivedEntities", BindingFlags.Instance | BindingFlags.NonPublic);
            var method = getEntitiesMethod.MakeGenericMethod(typeof(TEntity));
            return (IEnumerable<TEntity>)method.Invoke(this, new object[] { id });
        }

        internal IEnumerable<TDerived> GetDerivedEntities<TDerived>(IEnumerable<Guid> id)
            where TDerived : T
        {
            return _collection.GetItems<TDerived>(x => id.Contains(x.Id));
        }

        internal IEnumerable<TDerived> GetDerivedEntitiesExcept<TDerived>(IEnumerable<Guid> id)
            where TDerived : T
        {
            return _collection.GetItems<TDerived>(x => !id.Contains(x.Id));
        }

        public IEnumerable<TEntity> GetEntities<TEntity>()
            where TEntity : T
        {
            return _collection.GetItems<TEntity>();
        }

        public IEnumerable<TEntity> GetEntities<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : T
        {
            return _collection.GetItems<TEntity>(filter);
        }

        public IEnumerable<TEntity> GetEntities<TEntity>(Expression<Func<TEntity, object>> field, Regex pattern)
            where TEntity : T
        {
            return _collection.GetItems<TEntity>(field, pattern);
        }

        //public IPartialCollection<TDerived> GetEntities<TDerived>(Expression<Func<TDerived, bool>> filter, FilterOptions<TDerived> options)
        //    where TDerived : T
        //{
        //    return _collection.GetItems(filter, options);
        //}

        //public override IEnumerable<TEntity> GetEntities<TEntity>(Filter<TEntity> filter)
        //{
        //    var getEntitiesMethod = CompilerUtil.Current.GetGenericMethod<TEntity>(_collection.GetType(), "GetItems", BindingFlags.Public | BindingFlags.Instance, typeof(Filter<>));
        //    var result = getEntitiesMethod.Invoke(_collection, new object[] { filter });
        //    return (IEnumerable<TEntity>)result;
        //}

        public override IEnumerable<Guid> GetEntitiesId()
        {
            return _collection.GetAllId();
        }

        public IEnumerable<Guid> GetEntitiesId(Expression<Func<T, bool>> filter)
        {
            return _collection.GetId(filter);
        }

        public IEnumerable<T> GetEntities(Expression<Func<T, object>> field, Regex pattern)
        {
            return _collection.GetItems(field, pattern);
        }

        //public IEnumerable<T> GetEntities(Filter<T> filter)
        //{
        //    return _collection.GetItems(filter);
        //}

        //public IPartialCollection<T> GetEntities(Expression<Func<T, bool>> filter, FilterOptions<T> options)
        //{
        //    return _collection.GetItems(filter, options);
        //}

        internal override IEntity GetEntityFromCollection(Guid id)
        {
            return GetEntity(id);
        }

        internal override IEnumerable<IEntity> GetEntitiesFromCollection(IEnumerable<Guid> id)
        {
            return GetEntities(id).Select(x => (IEntity)x);
        }

        #endregion

        #region Add

        public override IEntity AddEntity(IEntity item)
        {
            return _context.Insert(item).Value;
        }

        public T AddEntity(T item)
        {
            return _context.Insert(item).Value;
        }

        public IEnumerable<T> AddEntities(IEnumerable<T> items)
        {
            return _context.Insert(items).Select(x => x.Value);
        }

        public override IEnumerable<IEntity> AddEntities(IEnumerable<IEntity> items)
        {
            return _context.Insert(items).Select(x => x.Value);
        }

        internal override IEnumerable<TEntity> AddEntitiesIntoCollection<TEntity>(IEnumerable<TEntity> items)
        {
            return AddEntitiesIntoCollection(items.Select(x => (IEntity)x)).Select(x => (TEntity)x);
        }

        internal override TEntity AddEntityToCollection<TEntity>(TEntity item)
        {
            return (TEntity)AddEntityToCollection((IEntity)item);
        }

        internal IEnumerable<T> AddEntitiesIntoCollection(IEnumerable<T> items)
        {
            return _collection.AddItems(items);
        }

        internal override IEnumerable<IEntity> AddEntitiesIntoCollection(IEnumerable<IEntity> items)
        {
            return AddEntitiesIntoCollection(items.Select(x => (T)x)).Select(x => (IEntity)x);
        }

        internal override IEntity AddEntityToCollection(IEntity item)
        {
            return AddEntityToCollection((T)item);
        }

        internal T AddEntityToCollection(T item)
        {
            return (T)_collection.AddItem(item);
        }

        #endregion

        #region Update

        public void UpdateEntity(T item)
        {
            _collection.UpdateItem(item);
        }

        public void UpdateEntities(IEnumerable<T> items)
        {
            _collection.UpdateItems(items);
        }

        public override void UpdateEntity(IEntity item)
        {
            UpdateEntity((T)item);
        }

        public override void UpdateEntities(IEnumerable<IEntity> items)
        {
            UpdateEntities(items.Select(x => (T)x));
        }

        #endregion

        #region Delete

        public void DeleteEntities<TEntity>(Expression<Func<TEntity, bool>> filter)
            where TEntity : T
        {
            var items = GetEntities<TEntity>(filter);
            DeleteEntities(items.Select(x => (T)x));
        }

        //public void DeleteItems<TEntity>(Filter<TEntity> filter)
        //    where TEntity : T
        //{
        //    var items = GetEntities<TEntity>(filter);
        //    DeleteEntities(items.Select(x => (T)x));
        //}

        public override void DeleteEntity(IEntity item)
        {
            _context.Delete(item);
        }

        public void DeleteEntity(T item)
        {
            _context.Delete(item);
        }

        public override void DeleteEntities(IEnumerable<IEntity> items)
        {
            _context.Delete(items);
        }

        public void DeleteEntities(IEnumerable<T> items)
        {
            _context.Delete(items.Select(x => (IEntity)x));
        }

        public void DeleteEntities(Expression<Func<T, bool>> filter)
        {
            var items = GetEntities(filter);
            DeleteEntities(items);
        }

        //public void DeleteEntities(Filter<T> filter)
        //{
        //    var items = GetEntities(filter);
        //    DeleteEntities(items);
        //}

        public void DeleteAllItems()
        {
            var items = GetEntities();
            DeleteEntities(items);
        }

        public void DeleteAllItems<TEntity>()
            where TEntity : T
        {
            var items = GetEntities<TEntity>(x => true);
            DeleteEntities(items.Select(x => (IEntity)x));
        }

        public override void DeleteEntity(string id)
        {
            DeleteEntity(new Guid(id));
        }

        public override void DeleteEntity(Guid id)
        {
            _context.Delete(id);
        }

        internal override void DeleteEntityFromCollection(Guid id)
        {
            _collection.DeleteItem(id);
        }

        internal override void DeleteEntitiesFromCollection(IEnumerable<Guid> ids)
        {
            _collection.DeleteItems(ids);
        }

        internal override void DeleteEntitiesFromCollection(IEnumerable<IEntity> items)
        {
            DeleteEntitiesFromCollection(items.Select(x => (T)x));
        }

        internal void DeleteEntitiesFromCollection(IEnumerable<T> items)
        {
            _collection.DeleteItems(items);
        }

        internal override void DeleteEntityFromCollection(IEntity item)
        {
            _collection.DeleteItem(item.Id);
        }

        internal void DeleteEntityFromCollection(T item)
        {
            _collection.DeleteItem(item.Id);
        }

        #endregion

        #region Types

        private bool IsNominalType<TEntity>()
        {
            return typeof(TEntity) == typeof(T);
        }

        #endregion
    }
}
