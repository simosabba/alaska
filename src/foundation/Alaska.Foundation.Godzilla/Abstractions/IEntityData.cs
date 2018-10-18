using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    internal interface IEntityData
    {
        IEntity Entity { get; }
        IEntityInfo Info { get; }
        bool IsLeaf { get; }
    }

    internal interface IEntityData<T>
        where T : IEntity
    {
        T Entity { get; }
        IEntityInfo Info { get; }
        bool IsLeaf { get; }
    }
}
