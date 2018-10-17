using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IRelationship<TRelationship> : IRelationshipBase
    {
        TRelationship Data { get; }
    }

    public interface IRelationship : IRelationshipBase
    {
        object Data { get; }
    }

    public interface IRelationshipBase
    {
        Guid Id { get; }
        Guid SourceEntityId { get; }
        Guid TargetEntityId { get; }
        Guid RelationshipId { get; }
        DateTime CreationTime { get; }
        DateTime UpdateTime { get; }

        void Delete();
        void CommitChanges();
        void UndoPendingChanges();

        void ChangeSourceItem(IItemBase newSourceItem);
        void ChangeTargetItem(IItemBase newTargetItem);
        void ChangeLinkedItems(IItemBase newSourceItem, IItemBase newTargetItem);

        void ChangeSourceItem(IEntity newSourceEntity);
        void ChangeTargetItem(IEntity newTargetEntity);
        void ChangeLinkedItems(IEntity newSourceEntity, IEntity newTargetEntity);

        IItem GetSourceItem();
        IItem GetTargetItem();

        IItem<TEntity> GetSourceItem<TEntity>() where TEntity : IEntity;
        IItem<TEntity> GetTargetItem<TEntity>() where TEntity : IEntity;

        TEntity GetSourceEntity<TEntity>() where TEntity : IEntity;
        TEntity GetTargetEntity<TEntity>() where TEntity : IEntity;
    }
}
