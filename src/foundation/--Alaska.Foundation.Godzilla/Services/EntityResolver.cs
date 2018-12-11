using Alaska.Foundation.Core.Utils;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Attributes;
using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Alaska.Foundation.Godzilla.Services
{
    internal class EntityResolver
    {
        public EntityResolver()
        { }

        public EntityCollectionBase ResolveCollection<T>()
        {
            return ResolveCollection(typeof(T));
        }

        public EntityCollectionBase ResolveCollection(Type elementType)
        {
            var collectionName = ResolveCollectionName(elementType);
            var entityCollectionType = ResolveCollectionType(elementType);
            return (EntityCollectionBase)Activator.CreateInstance(entityCollectionType);
        }

        public Type ResolveCollectionType(Type elementType)
        {
            var rootType = ResolveRootTemplateType(elementType);
            return typeof(EntityCollection<>).MakeGenericType(rootType);
        }

        public string ResolveCollectionName(Type elementType)
        {
            var root = ResolveRootTemplateType(elementType);
            var template = root.GetCustomAttribute<TemplateAttribute>();
            return $"entities-{template.Id}";
        }

        public IRelationshipDefinition ResolveRelationshipDefinition<T>()
        {
            return ResolveRelationshipDefinition(typeof(T));
        }

        public IRelationshipDefinition ResolveRelationshipDefinition(Type relationType)
        {
            var relationAttribute = relationType.GetCustomAttribute<RelationshipAttribute>();
            if (relationAttribute == null)
                throw new InvalidOperationException($"Invalid relationship type {relationType.FullName}");

            return relationAttribute;
        }

        public Guid ResolveTemplateId<T>()
        {
            return ResolveTemplateId(typeof(T));
        }

        public Guid ResolveTemplateId(IEntity entity)
        {
            return ResolveTemplateId(entity.GetType());
        }

        public Guid ResolveTemplateId(Type elementType)
        {
            var template = elementType
                .GetCustomAttribute<TemplateAttribute>();
            if (template == null)
                throw new InvalidOperationException("Missing type template");
            return new Guid(template.Id);
        }

        private Type ResolveRootTemplateType(Type elementType)
        {
            var baseTypes = ReflectionUtil.GetBaseTypes(elementType);
            return baseTypes
                .LastOrDefault(x => IsTemplatedType(x));
        }

        private bool IsTemplatedType(Type elementType)
        {
            return elementType.GetCustomAttribute<TemplateAttribute>() != null;
        }
    }
}
