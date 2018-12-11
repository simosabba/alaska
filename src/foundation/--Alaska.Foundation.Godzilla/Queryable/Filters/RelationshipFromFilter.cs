using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Entries;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class RelationshipFromFilter : RelationshipFilterBase
    {
        private IEnumerable<Guid> _fromEntities;

        public RelationshipFromFilter(Type relationshipType)
            : this(relationshipType, null)
        { }

        public RelationshipFromFilter(Type relationshipType, IEnumerable<Guid> fromEntities)
            : base(relationshipType)
        {
            _fromEntities = fromEntities;
        }

        protected override IEnumerable<Guid> ExecuteQuery(IQueryable<RelationshipEntryBase> query)
        {
            if (_fromEntities != null)
                query = query.Where(x => _fromEntities.Contains(x.SourceEntityId));

            return query
                .Select(x => x.TargetEntityId)
                .ToList();
        }
    }
}
