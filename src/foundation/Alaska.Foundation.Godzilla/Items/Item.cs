using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Items
{
    public sealed class Item : ItemBase, IItem, IQueryableItem
    {
        internal Item(IEntityData data)
            : base(data.Entity, data.Info)
        { }

        public IEntity Value => _value;
    }

    public sealed class Item<TEntity> : ItemBase, IItem<TEntity>, IQueryableItem<TEntity>
        where TEntity : IEntity
    {
        internal Item(IEntityData<TEntity> data)
            : base(data.Entity, data.Info)
        { }

        public TEntity Value => (TEntity)_value;

        public bool Matches(Expression<Func<TEntity, bool>> filter)
        {
            return filter.Compile()(Value);
        }
    }
}
