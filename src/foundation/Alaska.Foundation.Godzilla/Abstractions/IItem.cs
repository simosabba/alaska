using Alaska.Foundation.Godzilla.Entities.Templates;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IItemBase
    {
        Guid ItemId { get; }
        string Name { get; }
        string Path { get; }
        bool IsLeaf { get; }
        Template Template { get; }

        IEntityInfo Info { get; }

        IItem GetParent();
        IEnumerable<IItem> GetParents();
        IEnumerable<IItem> GetChildren();

        IEnumerable<IItem<TEntity>> GetChildren<TEntity>() where TEntity : IEntity;
        IEnumerable<IItem<TEntity>> GetChildren<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity;

        IEnumerable<IItem> GetDescendants();
        IEnumerable<IItem> GetDescendants(uint depth);

        IEnumerable<IItem<TEntity>> GetDescendants<TEntity>() where TEntity : IEntity;
        IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(uint depth) where TEntity : IEntity;

        IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity;
        IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(Expression<Func<TEntity, bool>> filter, uint depth) where TEntity : IEntity;

        IItem AddChild(IEntity value);

        void Rename(string newName);

        void Update();
        void Update(IEntity entity);

        void Move(IEntity newParent);
        void Move(Guid newParentId);

        void Delete();

        void Protect();
        void Unprotect();

        bool IsValidChild(string name, Guid template);
        bool IsValidChild(string name, Guid template, out IEnumerable<string> validationErrors);

        bool IsAncestorOf(IItemBase item);
        bool IsDescendantOf(IItemBase item);

        void CommitChanges();
        void UndoPendingChanges();

        IRelationship<TRelation> AddRelation<TRelation, TItem>(IItem<TItem> target) where TItem : IEntity;
        IRelationship<TRelation> AddRelation<TRelation, TItem>(IItem<TItem> target, TRelation data) where TItem : IEntity;
        IRelationship<TRelation> AddRelation<TRelation>(IItem target);
        IRelationship<TRelation> AddRelation<TRelation>(IItem target, TRelation data);
        IRelationship<TRelation> AddRelation<TRelation>(IEntity target);
        IRelationship<TRelation> AddRelation<TRelation>(IEntity target, TRelation data);
        IRelationship<TRelation> AddRelation<TRelation>(Guid targetId);
        IRelationship<TRelation> AddRelation<TRelation>(Guid targetId, TRelation data);

        IRelationship<TRelation> AddRelationFrom<TRelation, TItem>(IItem<TItem> source) where TItem : IEntity;
        IRelationship<TRelation> AddRelationFrom<TRelation, TItem>(IItem<TItem> source, TRelation data) where TItem : IEntity;
        IRelationship<TRelation> AddRelationFrom<TRelation>(IItem source);
        IRelationship<TRelation> AddRelationFrom<TRelation>(IItem source, TRelation data);
        IRelationship<TRelation> AddRelationFrom<TRelation>(IEntity source);
        IRelationship<TRelation> AddRelationFrom<TRelation>(IEntity source, TRelation data);
        IRelationship<TRelation> AddRelationFrom<TRelation>(Guid sourceId);
        IRelationship<TRelation> AddRelationFrom<TRelation>(Guid sourceId, TRelation data);

        void UpdateRelation<TRelation>(Guid relationId, TRelation data);
        void DeleteRelation(Guid relationId);
        void DeleteRelation(IRelationshipBase relation);
        void DeleteRelations(IEnumerable<Guid> relationsId);
        void DeleteRelations<TRelation>();
        void DeleteRelations<TRelation>(IEnumerable<IRelationship<TRelation>> relations);

        IRelationship<TRelation> GetRelation<TRelation>();
        IEnumerable<IRelationship<TRelation>> GetRelations<TRelation>();
        IEnumerable<IRelationship> GetRelations();
        IEnumerable<IEntityRelationshipDetails> GetDetailedRelations();

        bool HasRelationship<TRelationsip>();
        bool HasRelationship<TRelationsip>(Guid entity);
        bool HasRelationship<TRelationsip>(IEnumerable<Guid> entities);
        bool HasInboundRelationship<TRelationsip>();
        bool HasInboundRelationship<TRelationsip>(Guid fromEntity);
        bool HasInboundRelationship<TRelationsip>(IEnumerable<Guid> fromEntities);
        bool HasOutboundRelationship<TRelationsip>();
        bool HasOutboundRelationship<TRelationsip>(Guid toEntity);
        bool HasOutboundRelationship<TRelationsip>(IEnumerable<Guid> toEntities);
    }

    public interface IItem : IItemBase
    {
        IEntity Value { get; }
    }

    public interface IItem<TEntity> : IItemBase
        where TEntity : IEntity
    {
        TEntity Value { get; }
    }
}
