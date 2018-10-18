using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IQueryableItem<TEntity>
            where TEntity : IEntity
    {
        bool Matches(Expression<Func<TEntity, bool>> filter);
        bool HasRelationship<TRelationsip>();
        //bool HasRelationship<TRelationsip>(IEntity entity);
        //bool HasRelationship<TRelationsip>(IEnumerable<IEntity> entities);
        bool HasInboundRelationship<TRelationsip>();
        //bool HasInboundRelationship<TRelationsip>(IEntity from);
        //bool HasInboundRelationship<TRelationsip>(IEnumerable<IEntity> from);
        bool HasOutboundRelationship<TRelationsip>();
        //bool HasOutboundRelationship<TRelationsip>(IEntity to);
        //bool HasOutboundRelationship<TRelationsip>(IEnumerable<IEntity> to);
    }

    public interface IQueryableItem
    {
        bool Is<TEntity>() where TEntity : IEntity;
        bool HasRelationship<TRelationsip>();
        //bool HasRelationship<TRelationsip>(IEntity entity);
        //bool HasRelationship<TRelationsip>(IEnumerable<IEntity> entities);
        bool HasInboundRelationship<TRelationsip>();
        //bool HasInboundRelationship<TRelationsip>(IEntity from);
        //bool HasInboundRelationship<TRelationsip>(IEnumerable<IEntity> from);
        bool HasOutboundRelationship<TRelationsip>();
        //bool HasOutboundRelationship<TRelationsip>(IEntity to);
        //bool HasOutboundRelationship<TRelationsip>(IEnumerable<IEntity> to);
    }
}
