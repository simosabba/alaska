using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Settings
{
    public class EntityContextOptions<T> : EntityContextOptions
        where T : EntityContext
    { }

    public abstract class EntityContextOptions
    {
        public string PathSeparator { get; set; } = "/";

        public DatabaseCollectionOptions Collections { get; set; } = new DatabaseCollectionOptions();
    }
}
