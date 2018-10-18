using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Services
{
    public abstract class EntityContext : IEntityContext
    {
        private readonly EntityContextOptions _options;
        private readonly PathBuilder _pathBuilder;
        private readonly EntityCollectionResolver _resolver;
        private readonly HierarchyCollection _hierarchyCollection;
        private readonly RecycleBinCollection _recycleBinCollection;

        public EntityContext(EntityContextOptions options)
        {
            _options = options ?? throw new ArgumentException(nameof(options));
            _pathBuilder = new PathBuilder(options);
            _resolver = new EntityCollectionResolver();
            _hierarchyCollection = new HierarchyCollection(options.Collections, _resolver, _pathBuilder);
            _recycleBinCollection = new RecycleBinCollection(options.Collections);
        }
    }
}
