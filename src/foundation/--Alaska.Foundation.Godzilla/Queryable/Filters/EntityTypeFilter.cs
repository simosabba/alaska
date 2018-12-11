using System;
using System.Collections.Generic;
using System.Text;
using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Services;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class EntityTypeFilter : QueryFilter
    {
        private bool _negate;
        private Type _entityType;
        
        public EntityTypeFilter(Type entityType, bool negate)
        {
            _entityType = entityType;
            _negate = negate;
        }

        public override string Representation => $"Entity {(_negate ? "is not" : "is")} {_entityType.FullName}";

        public override IEnumerable<Guid> Execute(EntityContext context)
        {
            var template = context.Resolver.GetTemplate(_entityType);
            if (template == null)
                throw new InvalidOperationException($"Template not found for type {_entityType.FullName}");

            var templateId = template.Id.ToString();
            return _negate ?
                context.Hierarchy.GetEntitiesId(x => x.TemplateId != templateId) :
                context.Hierarchy.GetEntitiesId(x => x.TemplateId == templateId);
        }
    }
}
