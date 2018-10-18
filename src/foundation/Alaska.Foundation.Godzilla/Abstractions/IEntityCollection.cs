using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IEntityCollection<T> : IEntityCollection
        where T : IEntity
    {
        T GetEntity(string id);
        T GetEntity(Guid id);
        IEnumerable<T> GetEntities(IEnumerable<Guid> id);
        IQueryable<T> AsQueryable();
        IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : T;
    }

    public interface IEntityCollection
    {
        IEnumerable<TEntity> GetEntities<TEntity>(IEnumerable<Guid> id) where TEntity : IEntity;
        IEntity AddEntity(IEntity item);
        IEnumerable<IEntity> AddEntities(IEnumerable<IEntity> items);
        void UpdateEntity(IEntity item);
        void UpdateEntities(IEnumerable<IEntity> items);
        void DeleteEntity(IEntity item);
        void DeleteEntities(IEnumerable<IEntity> items);
        void DeleteEntity(Guid id);
    }
}
