using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Items
{
    public abstract class RelationshipBase
    {
        private readonly EntityContext _context;
        protected RelationshipEntryBase _relationship;

        internal RelationshipBase(
            EntityContext context,
            RelationshipEntryBase relationship)
        {
            _context = context;
            _relationship = relationship;
        }

        private EntityContext Context => _context;

        public Guid Id => _relationship.Id;
        public Guid SourceEntityId => _relationship.SourceEntityId;
        public Guid TargetEntityId => _relationship.TargetEntityId;
        public Guid RelationshipId => _relationship.RelationshipId;
        public DateTime CreationTime => _relationship.CreationTime;
        public DateTime UpdateTime => _relationship.UpdateTime;

        public void Delete()
        {
            Context.Relationships.DeleteRelation(Id);
        }

        public void CommitChanges()
        {
            Context.Relationships.UpdateRelation(_relationship);
        }

        public void UndoPendingChanges()
        {
            _relationship = Context.Relationships.GetRelation(Id);
        }

        public void ChangeSourceItem(IItemBase newSourceItem)
        {
            var relationship = Context.Relationships.GetRelation(Id);
            _relationship.SourceEntityId = relationship.SourceEntityId = newSourceItem.ItemId;
            Context.Relationships.UpdateRelation(relationship);
        }

        public void ChangeTargetItem(IItemBase newTargetItem)
        {
            var relationship = Context.Relationships.GetRelation(Id);
            _relationship.TargetEntityId = relationship.TargetEntityId = newTargetItem.ItemId;
            Context.Relationships.UpdateRelation(relationship);
        }

        public void ChangeLinkedItems(IItemBase newSourceItem, IItemBase newTargetItem)
        {
            var relationship = Context.Relationships.GetRelation(Id);
            _relationship.SourceEntityId = relationship.SourceEntityId = newSourceItem.ItemId;
            _relationship.TargetEntityId = relationship.TargetEntityId = newTargetItem.ItemId;
            Context.Relationships.UpdateRelation(relationship);
        }

        public void ChangeSourceItem(IEntity newSourceEntity)
        {
            var relationship = Context.Relationships.GetRelation(Id);
            _relationship.SourceEntityId = relationship.SourceEntityId = newSourceEntity.Id;
            Context.Relationships.UpdateRelation(relationship);
        }

        public void ChangeTargetItem(IEntity newTargetEntity)
        {
            var relationship = Context.Relationships.GetRelation(Id);
            _relationship.TargetEntityId = relationship.TargetEntityId = newTargetEntity.Id;
            Context.Relationships.UpdateRelation(relationship);
        }

        public void ChangeLinkedItems(IEntity newSourceEntity, IEntity newTargetEntity)
        {
            var relationship = Context.Relationships.GetRelation(Id);
            _relationship.SourceEntityId = relationship.SourceEntityId = newSourceEntity.Id;
            _relationship.TargetEntityId = relationship.TargetEntityId = newTargetEntity.Id;
            Context.Relationships.UpdateRelation(relationship);
        }

        public IItem GetSourceItem()
        {
            return Context.GetItem(_relationship.SourceEntityId);
        }

        public IItem GetTargetItem()
        {
            return Context.GetItem(_relationship.TargetEntityId);
        }

        public TEntity GetSourceEntity<TEntity>()
            where TEntity : IEntity
        {
            var item = GetSourceItem<TEntity>();
            return item == null ?
                default(TEntity) :
                item.Value;
        }

        public IItem<TEntity> GetSourceItem<TEntity>()
            where TEntity : IEntity
        {
            return Context.GetItem<TEntity>(_relationship.SourceEntityId);
        }

        public TEntity GetTargetEntity<TEntity>()
            where TEntity : IEntity
        {
            var item = GetTargetItem<TEntity>();
            return item == null ?
                default(TEntity) :
                item.Value;
        }

        public IItem<TEntity> GetTargetItem<TEntity>()
            where TEntity : IEntity
        {
            return Context.GetItem<TEntity>(_relationship.TargetEntityId);
        }
    }
}
