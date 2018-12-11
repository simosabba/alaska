using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public enum ItemOrigin { System, Application }
    public enum ItemState { Protected, Unprotected }

    public interface IEntityInfo
    {
        string Path { get; }
        string Name { get; }
        Guid ItemId { get; }
        ItemOrigin Origin { get; }
        ItemState State { get; }
        DateTime CreationTime { get; }
        DateTime UpdateTime { get; }
    }
}
