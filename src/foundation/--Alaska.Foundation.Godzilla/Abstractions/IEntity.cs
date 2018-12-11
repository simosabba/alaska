using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IEntity : IDatabaseCollectionElement
    {
        string EntityName { get; }
    }
}
