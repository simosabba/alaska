using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alaska.Foundation.Godzilla.Collections
{
    public class GorillaCollection<T> : DatabaseCollection<T>
        where T : IEntity
    {
        public IEntity AddItem(IEntity entity)
        {
            return base.AddItem((T)entity);
        }

        public IEnumerable<IEntity> AddItems(IEnumerable<IEntity> entities)
        {
            return base.AddItems(entities.Select(x => (T)x)).Select(x => (IEntity)x);
        }
    }
}
