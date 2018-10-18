using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alaska.Foundation.Godzilla.Collections;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class RelationshipToFilter : RelationshipFilterBase
    {
        private IEnumerable<Guid> _toEntities;

        public RelationshipToFilter(Type relationshipType)
                    : this(relationshipType, null)
        { }

        public RelationshipToFilter(Type relationshipType, IEnumerable<Guid> toEntities)
            : base(relationshipType)
        {
            _toEntities = toEntities;
        }

        protected override IEnumerable<Guid> ExecuteQuery(IQueryable<RelationshipEntryBase> query)
        {
            if (_toEntities != null)
                query = query.Where(x => _toEntities.Contains(x.TargetEntityId));

            return query
                .Select(x => x.SourceEntityId)
                .ToList();
        }
    }
}
