using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Items
{
    public sealed class Item : ItemBase, IItem, IQueryableItem
    {
        internal Item(EntityContext context, IEntityData data)
            : base(context, data.Entity, data.Info)
        { }

        public IEntity Value => _value;
    }

    public sealed class Item<TEntity> : ItemBase, IItem<TEntity>, IQueryableItem<TEntity>
        where TEntity : IEntity
    {
        internal Item(EntityContext context, IEntityData<TEntity> data)
            : base(context, data.Entity, data.Info)
        { }

        public TEntity Value => (TEntity)_value;

        public bool Matches(Expression<Func<TEntity, bool>> filter)
        {
            return filter.Compile()(Value);
        }
    }
}
