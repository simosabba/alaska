using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class RelationshipFilter : RelationshipFilterBase
    {
        private IEnumerable<Guid> _entities;

        public RelationshipFilter(Type relationshipType)
            : this(relationshipType, null)
        { }

        public RelationshipFilter(Type relationshipType, IEnumerable<Guid> entities)
            : base(relationshipType)
        {
            _entities = entities;
        }

        protected override IEnumerable<Guid> ExecuteQuery(IQueryable<RelationshipEntryBase> query)
        {
            if (_entities != null)
                query = query.Where(x => 
                    _entities.Contains(x.SourceEntityId) ||
                    _entities.Contains(x.TargetEntityId));

            var ids = query.Select(x => new { x.SourceEntityId, x.TargetEntityId }).ToList();
            return ids
                .Select(x => x.SourceEntityId)
                .Union(ids.Select(x => x.TargetEntityId))
                .Distinct()
                .ToList();
        }
    }
}
