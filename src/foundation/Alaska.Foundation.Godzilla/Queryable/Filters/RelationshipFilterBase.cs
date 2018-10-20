using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal abstract class RelationshipFilterBase : QueryFilter
    {
        private Type _relationshipType;
        
        public RelationshipFilterBase(Type relationshipType)
        {
            _relationshipType = relationshipType;
        }

        public override IEnumerable<Guid> Execute(EntityContext context)
        {
            var relationship = context.Resolver.ResolveRelationshipDefinition(_relationshipType);
            if (relationship == null)
                throw new InvalidOperationException($"Cannot resolve relationship {_relationshipType.FullName}");

            var relationshipId = relationship.Id;
            var query = context.Relationships.AsQueryable()
                .Where(x => x.RelationshipId == relationshipId);

            return ExecuteQuery(query);
        }

        protected abstract IEnumerable<Guid> ExecuteQuery(IQueryable<RelationshipEntryBase> query);

        public override string Representation => $"{this.GetType().Name} {_relationshipType.FullName}";
    }
}
