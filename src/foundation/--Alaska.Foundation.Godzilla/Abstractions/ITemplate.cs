using Alaska.Foundation.Godzilla.Entities.Common;
using Alaska.Foundation.Godzilla.Entities.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface ITemplate
    {
        string EntityName { get; }
        DataTypeInfo TypeInfo { get; }
        IconInfo DefaultIcon { get; }
        IconInfo UserIcon { get; }
        bool IsActive { get; }
        string TypeSchema { get; }
    }
}
