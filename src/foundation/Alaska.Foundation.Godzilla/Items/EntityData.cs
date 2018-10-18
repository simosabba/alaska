using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Items
{
    internal class EntityData : IEntityData
    {
        public EntityData()
        { }

        public EntityData(IEntity entity, IEntityInfo info)
        {
            Entity = entity;
            Info = info;
        }

        public IEntity Entity { get; set; }
        public IEntityInfo Info { get; set; }
        public bool IsLeaf { get; set; }
    }

    internal class EntityData<T> : IEntityData<T>, IEntityData
        where T : IEntity
    {
        public EntityData()
        { }

        public EntityData(T entity, IEntityInfo info)
        {
            Entity = entity;
            Info = info;
        }

        public T Entity { get; set; }
        public IEntityInfo Info { get; set; }
        public bool IsLeaf { get; set; }

        IEntity IEntityData.Entity => Entity;
    }
}
