using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Services
{
    public abstract class EntityContext : IEntityContext
    {
        private readonly EntityContextOptions _options;
        private PathBuilder _pathBuilder;

        public EntityContext(EntityContextOptions options)
        {
            _options = options;
            _pathBuilder = new PathBuilder(options);
        }
    }
}
