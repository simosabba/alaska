using Alaska.Foundation.Core.Logging;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Converters;
using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Exceptions;
using Alaska.Foundation.Godzilla.Models;
using Alaska.Foundation.Godzilla.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Items
{
    public abstract class ItemBase
    {
        #region Init
        
        protected IEntity _value;
        private IEntityInfo _info;
        private EntityContext _context;

        internal ItemBase(
            EntityContext context,
            IEntity entity, 
            IEntityInfo info)
        {
            _value = entity;
            _info = info;
        }

        protected EntityContext Context => _context;

        public Guid ItemId => _info.ItemId;
        public string Name => _value.EntityName;
        public Guid TemplateId => _context.Resolver.ResolveTemplateId(_value.GetType());
        //public Template Template => Context.TemplatesCollection.GetTemplate(TemplateId);
        public IEntityInfo Info => _info;

        #endregion

        #region Management Items

        //private EntityMap EntityMap => EntityConfigurator.Current.Map;

        #endregion

        #region Common Properties

        public string Path => _info.Path;
        public bool IsLeaf => !Context.HasChildren(_value);

        #endregion

        #region Relations

        public IRelationship<TRelation> AddRelation<TRelation, TItem>(IItem<TItem> target)
            where TItem : IEntity
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelation<TRelation>(target.ItemId, data);
        }

        public IRelationship<TRelation> AddRelation<TRelation, TItem>(IItem<TItem> target, TRelation data)
            where TItem : IEntity
        {
            return AddRelation<TRelation>(target.ItemId, data);
        }

        public IRelationship<TRelation> AddRelation<TRelation>(IItem target)
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelation<TRelation>(target, data);
        }

        public IRelationship<TRelation> AddRelation<TRelation>(IItem target, TRelation data)
        {
            return AddRelation<TRelation>(target.ItemId, data);
        }

        public IRelationship<TRelation> AddRelation<TRelation>(IEntity target)
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelation<TRelation>(target, data);
        }

        public IRelationship<TRelation> AddRelation<TRelation>(IEntity target, TRelation data)
        {
            return AddRelation<TRelation>(target.Id, data);
        }

        public IRelationship<TRelation> AddRelation<TRelation>(Guid targetId)
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelation<TRelation>(targetId, data);
        }

        public IRelationship<TRelation> AddRelation<TRelation>(Guid targetId, TRelation data)
        {
            var relation = Context.Relationships.AddRelation<TRelation>(ItemId, targetId, data);
            return new Relationship<TRelation>(Context, relation);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation, TItem>(IItem<TItem> source)
            where TItem : IEntity
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelationFrom<TRelation>(source.ItemId, data);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation, TItem>(IItem<TItem> source, TRelation data)
            where TItem : IEntity
        {
            return AddRelationFrom<TRelation>(source.ItemId, data);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation>(IItem source)
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelationFrom<TRelation>(source, data);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation>(IItem source, TRelation data)
        {
            return AddRelationFrom<TRelation>(source.ItemId, data);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation>(IEntity source)
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelationFrom<TRelation>(source, data);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation>(IEntity source, TRelation data)
        {
            return AddRelationFrom<TRelation>(source.Id, data);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation>(Guid sourceId)
        {
            var data = Activator.CreateInstance<TRelation>();
            return AddRelationFrom<TRelation>(sourceId, data);
        }

        public IRelationship<TRelation> AddRelationFrom<TRelation>(Guid sourceId, TRelation data)
        {
            var relation = Context.Relationships.AddRelation<TRelation>(sourceId, ItemId, data);
            return new Relationship<TRelation>(Context, relation);
        }

        public void UpdateRelation<TRelation>(Guid relationId, TRelation data)
        {
            Context.Relationships.UpdateRelation(relationId, data);
        }

        public void DeleteRelations<TRelation>()
        {
            Context.Relationships.DeleteRelations<TRelation>();
        }

        public void DeleteRelations<TRelation>(IEnumerable<IRelationship<TRelation>> relations)
        {
            Context.Relationships.DeleteRelations(relations.Select(x => x.Id));
        }

        public void DeleteRelations(IEnumerable<Guid> relationsId)
        {
            Context.Relationships.DeleteRelations(relationsId);
        }

        public void DeleteRelation(IRelationshipBase relation)
        {
            DeleteRelation(relation.Id);
        }

        public void DeleteRelation(Guid relationId)
        {
            Context.Relationships.DeleteRelation(relationId);
        }

        public IRelationship<TRelation> GetRelation<TRelation>()
        {
            var relations = GetRelations<TRelation>();
            if (relations.Count() > 1)
                throw new InvalidOperationException($"More than one relationships of type {typeof(TRelation)} found for item {ItemId}");
            return relations.FirstOrDefault();
        }

        public IEnumerable<IRelationship<TRelation>> GetRelations<TRelation>()
        {
            return GetRelationshipEntries<TRelation>()
                .Select(x => new Relationship<TRelation>(Context, x))
                .ToList();
        }

        public IEnumerable<IRelationship> GetRelations()
        {
            return GetRelationshipEntries()
                .Select(x => new Relationship(Context, x))
                .ToList();
        }

        //public IEnumerable<IEntityRelationshipDetails> GetDetailedRelations()
        //{
        //    var relations = GetRelationshipEntries();
        //    var relatedItemsId = relations
        //            .Select(x => x.SourceEntityId)
        //            .Union(relations.Select(x => x.TargetEntityId));
        //    var relatedItems = Context.GetItems(relatedItemsId)
        //        .ToDictionary(x => x.Value.Id);

        //    var results = new List<EntityRelationshipData>();
        //    foreach (var relation in relations)
        //    {
        //        var sourceEntity = relatedItems[relation.SourceEntityId];
        //        if (sourceEntity == null)
        //        {
        //            Logger.Current.LogWarning($"Source item {relation.SourceEntityId} not found for relation {relation.Id}");
        //            continue;
        //        }

        //        var targetEntity = relatedItems[relation.TargetEntityId];
        //        if (targetEntity == null)
        //        {
        //            Logger.Current.LogWarning($"Target item {relation.TargetEntityId} not found for relation {relation.Id}");
        //            continue;
        //        }

        //        var item = new EntityRelationshipData
        //        {
        //            Relation = relation,
        //            SourceEntity = sourceEntity,
        //            TargetEntity = targetEntity,
        //        };
        //        results.Add(item);
        //    }

        //    return results;
        //}

        //public bool Is<TEntity>()
        //    where TEntity : IEntity
        //{
        //    return Template.TypeInfo.Type == typeof(TEntity);
        //}

        public bool HasRelationship<TRelationsip>()
        {
            return GetRelationshipEntries<TRelationsip>().Any();
        }

        public bool HasRelationship<TRelationsip>(Guid entity)
        {
            return GetRelationshipEntries<TRelationsip>()
                .Any(x => x.TargetEntityId == entity || x.SourceEntityId == entity);
        }

        public bool HasRelationship<TRelationsip>(IEnumerable<Guid> entities)
        {
            return GetRelationshipEntries<TRelationsip>()
                .Any(x =>
                    entities.Contains(x.TargetEntityId) ||
                    entities.Contains(x.SourceEntityId));
        }

        public bool HasInboundRelationship<TRelationsip>()
        {
            return GetRelationshipEntriesTo<TRelationsip>().Any();
        }

        public bool HasInboundRelationship<TRelationsip>(Guid fromEntity)
        {
            return GetRelationshipEntriesTo<TRelationsip>().Any(
                x => x.SourceEntityId == fromEntity);
        }

        public bool HasInboundRelationship<TRelationsip>(IEnumerable<Guid> fromEntities)
        {
            return GetRelationshipEntriesTo<TRelationsip>().Any(
                x => fromEntities.Contains(x.SourceEntityId));
        }

        public bool HasOutboundRelationship<TRelationsip>()
        {
            return Context.Relationships.GetRelationsFrom<TRelationsip>(this._value).Any();
        }

        public bool HasOutboundRelationship<TRelationsip>(Guid toEntity)
        {
            return GetRelationshipEntriesFrom<TRelationsip>().Any(
                x => x.TargetEntityId == toEntity);
        }

        public bool HasOutboundRelationship<TRelationsip>(IEnumerable<Guid> toEntities)
        {
            return GetRelationshipEntriesFrom<TRelationsip>().Any(
                x => toEntities.Contains(x.TargetEntityId));
        }

        private IEnumerable<RelationshipEntryBase> GetRelationshipEntries()
        {
            return Context.Relationships
                .GetRelations(_value);
        }

        private IEnumerable<RelationshipEntry<TRelation>> GetRelationshipEntries<TRelation>()
        {
            return Context.Relationships
                .GetRelations<TRelation>(_value);
        }

        private IEnumerable<RelationshipEntry<TRelation>> GetRelationshipEntriesTo<TRelation>()
        {
            return Context.Relationships
                .GetRelationsTo<TRelation>(_value);
        }

        private IEnumerable<RelationshipEntry<TRelation>> GetRelationshipEntriesFrom<TRelation>()
        {
            return Context.Relationships
                .GetRelationsFrom<TRelation>(_value);
        }

        #endregion

        #region Hierarchical Relations

        public bool IsDescendantOf(IItemBase item)
        {
            return _context.PathBuilder.IsDescendantPath(_info.Path, item.Path);
        }

        public bool IsAncestorOf(IItemBase item)
        {
            return Context.PathBuilder.IsAncestorPath(_info.Path, item.Path);
        }

        #endregion

        #region Get Parent

        public IItem GetParent()
        {
            return Context.GetParent(_value);
        }

        public IEnumerable<IItem> GetParents()
        {
            var parents = new List<IItem>();
            var parent = GetParent();
            if (parent != null)
            {
                parents.Add(parent);
                parents.AddRange(parent.GetParents());
            }
            return parents;
        }

        #endregion

        #region Get Children

        public IEnumerable<IItem> GetChildren()
        {
            return Context.GetChildren(_value);
        }

        public IEnumerable<IItem<TEntity>> GetChildren<TEntity>() where TEntity : IEntity
        {
            return Context.GetChildren<TEntity>(_value);
        }

        public IEnumerable<IItem<TEntity>> GetChildren<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity
        {
            return Context.GetChildren<TEntity>(_value, filter);
        }

        #endregion

        #region Get Descendants 

        //public IEnumerable<IItem> GetDescendants()
        //{
        //    return Context.GetDescendants(_value, null);
        //}

        //public IEnumerable<IItem> GetDescendants(uint depth)
        //{
        //    return Context.GetDescendants(_value, (int)depth);
        //}

        public IEnumerable<IItem<TEntity>> GetDescendants<TEntity>() where TEntity : IEntity
        {
            return Context.GetDescendants<TEntity>(_value, null);
        }

        public IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(uint depth) where TEntity : IEntity
        {
            return Context.GetDescendants<TEntity>(_value, (int)depth);
        }

        public IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity
        {
            return Context.GetDescendants<TEntity>(_value, filter, null);
        }

        public IEnumerable<IItem<TEntity>> GetDescendants<TEntity>(Expression<Func<TEntity, bool>> filter, uint depth) where TEntity : IEntity
        {
            return Context.GetDescendants<TEntity>(_value, filter, (int)depth);
        }

        #endregion

        #region Add

        public IItem AddChild(IEntity value)
        {
            EnsureItemId(value);
            Context.Insert(value, _value);
            return Context.GetItem(value.Id);
        }

        private void EnsureItemId(IEntity value)
        {
            if (value.Id == Guid.Empty)
                value.Id = Guid.NewGuid();
        }

        #endregion

        #region Edit

        public void Rename(string newName)
        {
            Context.Rename(_value, newName);
        }

        public void Update()
        {
            Update(_value);
        }

        public void Update(IEntity entity)
        {
            if (_value.Id != entity.Id)
                throw new CollectionItemUpdateException($"Current entity id and input entity id don't match ({_value.Id} - {entity.Id})");

            if (!_value.GetType().Equals(entity.GetType()))
                throw new CollectionItemUpdateException($"Current entity type and input entity type don't match ({_value.GetType().FullName} - {entity.GetType().FullName})");

            _value = entity;
            CommitChanges();
        }

        public void Move(Guid newParentId)
        {
            var newParent = Context.GetItem(newParentId);
            Move(newParent.Value);
        }

        public void Move(IEntity newParent)
        {
            Context.Move(_value, newParent);
            Reload();
        }

        public void CommitChanges()
        {
            Context.Update(_value);
            Reload();
        }

        public void UndoPendingChanges()
        {
            Reload();
        }

        private void Reload()
        {
            var item = Context.GetItem(ItemId);
            _value = item.Value;
            _info = item.Info;
        }

        #endregion

        #region Delete

        public void Delete()
        {
            Context.Delete(_value);
        }

        #endregion

        #region Protection

        public void Protect()
        {
            Context.ProtectItem(_value);
        }

        public void Unprotect()
        {
            Context.UnprotectItem(_value);
        }

        #endregion

        #region Validation

        public bool IsValidChild(string name, Guid templateId)
        {
            try
            {
                Context.ValidatePath(name, _value);
                return true;
            }
            catch (DuplicateEntityException)
            {
                return false;
            }
        }

        public bool IsValidChild(string name, Guid templateId, out IEnumerable<string> validationErrors)
        {
            try
            {
                Context.ValidatePath(name, _value);
                validationErrors = null;
                return true;
            }
            catch (DuplicateEntityException e)
            {
                validationErrors = new List<string> { e.Message };
                return false;
            }
        }

        #endregion
    }
}
