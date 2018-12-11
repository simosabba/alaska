using Alaska.Foundation.Core.Logging;
using Alaska.Foundation.Core.Utils;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Exceptions;
using Alaska.Foundation.Godzilla.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Alaska.Foundation.Godzilla.Collections
{
    internal class DatabaseCollection<TElement> : IDatabaseCollection<TElement>
            where TElement : IDatabaseCollectionElement
    {
        #region Init
        
        private readonly IDatabaseCollectionProvider<TElement> _provider;
        private readonly DatabaseCollectionOptions _options;

        public DatabaseCollection(string collectionName, DatabaseCollectionOptions options)
        {
            _options = options;
            _provider = CreateProviderInstance(collectionName, options);
        }

        protected bool IsLogDisabled => _options.DisableLogs;
        protected IDatabaseCollectionProvider<TElement> Provider => _provider;
        
        private IDatabaseCollectionProvider<TElement> CreateProviderInstance(string collectionName, DatabaseCollectionOptions options)
        {
            var provider = (IDatabaseCollectionProvider<TElement>)Activator.CreateInstance(options.ProviderType);
            provider.Configure(new DatabaseCollectionProviderOptions
            {
                CollectionName = collectionName,
                ConnectionString = options.ConnectionString
            });
            return provider;
        }
        
        #endregion

        #region Collection Methods

        public IQueryable<TElement> AsQueryable()
        {
            try
            {
                return _provider.AsQueryable();
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error converting to queryable", e);
            }
        }

        public IQueryable<TDerived> AsQueryable<TDerived>()
            where TDerived : TElement
        {
            try
            {
                return _provider.AsQueryable<TDerived>();
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error converting to queryable", e);
            }
        }

        public TElement AddItem(TElement item)
        {
            try
            {
                EnsureItemId(item);
                CheckInputItem(item);
                if (ContainsItem(item))
                    throw new CollectionException($"Item {item.Id} already present");

                if (!IsLogDisabled)
                    Logger.Current.LogDebug($"Inserting item with id {item.Id}");
                _provider.Add(item);
                return item;
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error adding item {item.Id}", e);
            }
        }

        public IEnumerable<TElement> AddItems(IEnumerable<TElement> items)
        {
            try
            {
                foreach (var item in items)
                {
                    EnsureItemId(item);
                    CheckInputItem(item);
                }

                if (!IsLogDisabled)
                    Logger.Current.LogDebug("Inserting items");
                _provider.Add(items);
                return items;
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error adding items", e);
            }
        }

        public TElement EnsureItem(TElement item)
        {
            try
            {
                EnsureItemId(item);
                CheckInputItem(item);
                if (ContainsItem(item))
                {
                    if (!IsLogDisabled)
                        Logger.Current.LogDebug($"Updating item with id {item.Id}");
                    _provider.Update(item);
                }
                else
                {
                    if (!IsLogDisabled)
                        Logger.Current.LogDebug($"Inserting item with id {item.Id}");
                    _provider.Add(item);
                }
                return item;
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error ensuring item {item.GetItemId()}", e);
            }
        }

        public bool ContainsItem(TElement item)
        {
            try
            {
                CheckInputItem(item);
                return _provider.Contains(item.Id);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error checking item {item.Id}", e);
            }
        }

        public bool ContainsItem(Expression<Func<TElement, bool>> filter)
        {
            try
            {
                return _provider.Contains(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error checking item", e);
            }
        }

        public bool ContainsItem<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : TElement
        {
            try
            {
                return _provider.Contains<TDerived>(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error checking item", e);
            }
        }

        public bool ContainsItem(Guid id)
        {
            try
            {
                Check.IsNotEmpty(id);
                return _provider.Contains(id);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error checking item {id}", e);
            }
        }

        public void DeleteAllItems()
        {
            try
            {
                if (!IsLogDisabled)
                    Logger.Current.LogInformation("Deleting all items");
                _provider.Delete(x => true);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error deleting all items", e);
            }
        }

        public void DeleteAllItems<TDerived>() where TDerived : TElement
        {
            try
            {
                if (!IsLogDisabled)
                    Logger.Current.LogInformation("Deleting all items");
                _provider.Delete<TDerived>(x => true);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error deleting all items", e);
            }
        }

        public void DeleteItem(Guid id)
        {
            try
            {
                Check.IsNotEmpty(id);

                if (!IsLogDisabled)
                    Logger.Current.LogDebug($"Deleting item with id {id}");
                _provider.Delete(id);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error deleting item {id}", e);
            }
        }

        public void DeleteItem(TElement item)
        {
            try
            {
                CheckInputItem(item);

                if (!IsLogDisabled)
                    Logger.Current.LogDebug($"Deleting item with id {item.Id}");
                _provider.Delete(item.Id);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error deleting item {item.Id}", e);
            }
        }

        public void DeleteItems(IEnumerable<TElement> items)
        {
            var itemIdList = items.Select(x => x.Id);
            DeleteItems(itemIdList);
        }

        public void DeleteItems(IEnumerable<Guid> idList)
        {
            DeleteItems(x => idList.Contains(x.Id));
        }

        public void DeleteItems(Expression<Func<TElement, bool>> filter)
        {
            try
            {
                Check.IsNotNull(filter);

                if (!IsLogDisabled)
                    Logger.Current.LogDebug($"Deleting items with filter {filter.ToString()}");
                _provider.Delete(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error deleting items", e);
            }
        }

        public void DeleteItems<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : TElement
        {
            try
            {
                Check.IsNotNull(filter);
                var elementsToDelete = _provider.Get(filter);

                if (!IsLogDisabled)
                    Logger.Current.LogDebug($"Deleting items with filter {filter.ToString()}");
                _provider.Delete<TDerived>(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error deleting items", e);
            }
        }

        //public void DeleteItems(Filter<TElement> filter)
        //{
        //    try
        //    {
        //        Check.IsNotNull(filter);
        //        var elementsToDelete = _provider.Get(filter);
        //        if (!_isLogDisabled)
        //            Logger.Debug($"Deleting items with filter {filter.ToString()}");
        //        _provider.Delete(filter);
        //    }
        //    catch (CollectionException)
        //    {
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new CollectionException("Error deleting items", e);
        //    }
        //}

        //public void DeleteItems<TDerived>(Filter<TDerived> filter) where TDerived : TElement
        //{
        //    try
        //    {
        //        Check.IsNotNull(filter);
        //        var elementsToDelete = _provider.Get(filter);

        //        if (!_isLogDisabled)
        //            Logger.Debug($"Deleting items with filter {filter.ToString()}");
        //        _provider.Delete<TDerived>(filter);
        //    }
        //    catch (CollectionException)
        //    {
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new CollectionException("Error deleting items", e);
        //    }
        //}

        public long Count()
        {
            try
            {
                return _provider.Count();
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error counting collection items", e);
            }
        }

        public long Count<TDerived>() where TDerived : TElement
        {
            try
            {
                return _provider.Count<TDerived>();
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error counting collection items", e);
            }
        }

        public IEnumerable<TField> SelectFieldValues<TField>(Expression<Func<TElement, TField>> selector)
        {
            try
            {
                return _provider.SelectFieldValues(selector);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error selecting field values", e);
            }
        }

        public IEnumerable<TField> SelectFieldValues<TField>(Expression<Func<TElement, TField>> selector, Expression<Func<TElement, bool>> filter)
        {
            try
            {
                return _provider.SelectFieldValues(selector, filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error selecting field values", e);
            }
        }

        public IEnumerable<Guid> GetAllId()
        {
            try
            {
                return _provider.GetAllId();
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error getting all item id", e);
            }
        }

        public IEnumerable<Guid> GetId(Expression<Func<TElement, bool>> filter)
        {
            try
            {
                return _provider.GetId(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error getting items id", e);
            }
        }

        public IEnumerable<Guid> GetId<TDerived>(Expression<Func<TDerived, bool>> filter)
            where TDerived : TElement
        {
            try
            {
                return _provider.GetId<TDerived>(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error getting items id", e);
            }
        }

        public TElement GetItem(Guid id)
        {
            try
            {
                Check.IsNotEmpty(id);

                if (!IsLogDisabled)
                    Logger.Current.LogTrace($"Searching item with id {id}");
                return _provider.Get(id);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error searching item {id}", e);
            }
        }

        public TElement GetItem(Expression<Func<TElement, bool>> filter)
        {
            return GetItems(filter).FirstOrDefault();
        }

        public TDerived GetItem<TDerived>(Guid id) where TDerived : TElement
        {
            try
            {
                if (!IsLogDisabled)
                    Logger.Current.LogTrace($"Searching item {id}");
                return _provider.Get<TDerived>(id);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error searching item {id}", e);
            }
        }

        public TDerived GetItem<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : TElement
        {
            return GetItems<TDerived>(filter).FirstOrDefault();
        }

        //public TElement GetItem(Filter<TElement> filter)
        //{
        //    return GetItems(filter).FirstOrDefault();
        //}

        //public TDerived GetItem<TDerived>(Filter<TDerived> filter) where TDerived : TElement
        //{
        //    return GetItems<TDerived>(filter).FirstOrDefault();
        //}

        public IEnumerable<TElement> GetItems()
        {
            try
            {
                if (!IsLogDisabled)
                    Logger.Current.LogTrace("Searching all items");
                return _provider.Get();
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error getting all items", e);
            }
        }

        public IEnumerable<TDerived> GetItems<TDerived>()
            where TDerived : TElement
        {
            try
            {
                if (!IsLogDisabled)
                    Logger.Current.LogTrace($"Searching all items of type {typeof(TDerived).Name}");
                return _provider.Get<TDerived>();
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error getting all items of type {typeof(TDerived).Name}", e);
            }
        }

        public IEnumerable<TElement> GetItems(Expression<Func<TElement, bool>> filter)
        {
            try
            {
                Check.IsNotNull(filter);

                if (!IsLogDisabled)
                    Logger.Current.LogTrace($"Searching items with filter {filter.ToString()}");
                return _provider.Get(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error searching item", e);
            }
        }

        public IEnumerable<TDerived> GetItems<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : TElement
        {
            try
            {
                Check.IsNotNull(filter);

                if (!IsLogDisabled)
                    Logger.Current.LogTrace($"Searching items with filter {filter.ToString()}");
                return _provider.Get<TDerived>(filter);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error searching item", e);
            }
        }

        public IEnumerable<TElement> GetItems(Expression<Func<TElement, object>> field, Regex pattern)
        {
            try
            {
                Check.IsNotNull(field);
                Check.IsNotNull(pattern);

                if (!IsLogDisabled)
                    Logger.Current.LogTrace($"Searching items with pattern {pattern.ToString()}");
                return _provider.Get(field, pattern);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error searching item", e);
            }
        }

        public IEnumerable<TDerived> GetItems<TDerived>(Expression<Func<TDerived, object>> field, Regex pattern) where TDerived : TElement
        {
            try
            {
                Check.IsNotNull(field);
                Check.IsNotNull(pattern);

                if (!IsLogDisabled)
                    Logger.Current.LogTrace($"Searching items with pattern {pattern.ToString()}");
                return _provider.Get<TDerived>(field, pattern);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException("Error searching item", e);
            }
        }

        //public IEnumerable<TElement> GetItems(Filter<TElement> filter)
        //{
        //    try
        //    {
        //        Check.IsNotNull(filter);

        //        if (!_isLogDisabled)
        //            Logger.Trace($"Searching items with filter {filter.ToString()}");
        //        return _provider.Get(filter);
        //    }
        //    catch (CollectionException)
        //    {
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new CollectionException("Error searching items", e);
        //    }
        //}

        //public IEnumerable<TDerived> GetItems<TDerived>(Filter<TDerived> filter) where TDerived : TElement
        //{
        //    try
        //    {
        //        Check.IsNotNull(filter);

        //        if (!_isLogDisabled)
        //            Logger.Trace($"Searching items with filter {filter.ToString()}");
        //        return _provider.Get<TDerived>(filter);
        //    }
        //    catch (CollectionException)
        //    {
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new CollectionException("Error searching items", e);
        //    }
        //}

        //public IPartialCollection<TElement> GetItems(Expression<Func<TElement, bool>> filter, FilterOptions<TElement> options)
        //{
        //    try
        //    {
        //        Check.IsNotNull(filter);
        //        Check.IsNotNull(options);

        //        if (!_isLogDisabled)
        //            Logger.Trace($"Searching item with filter {filter.ToString()} and options {options.ToString()}");
        //        return _provider.Get(filter, options);
        //    }
        //    catch (CollectionException)
        //    {
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new CollectionException(e.Message, e);
        //    }
        //}

        //public IPartialCollection<TDerived> GetItems<TDerived>(Expression<Func<TDerived, bool>> filter, FilterOptions<TDerived> options) where TDerived : TElement
        //{
        //    try
        //    {
        //        Check.IsNotNull(filter);
        //        Check.IsNotNull(options);

        //        if (!_isLogDisabled)
        //            Logger.Trace($"Searching item with filter {filter.ToString()} and options {options.ToString()}");
        //        return _provider.Get<TDerived>(filter, options);
        //    }
        //    catch (CollectionException)
        //    {
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new CollectionException(e.Message, e);
        //    }
        //}

        public void UpdateItem(TElement item)
        {
            try
            {
                CheckInputItem(item);
                if (!ContainsItem(item))
                    throw new CollectionException($"Item {item.Id} not found");

                if (!IsLogDisabled)
                    Logger.Current.LogDebug($"Updating item with id {item.ToString()}");
                _provider.Update(item);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error updating item {item.Id}", e);
            }
        }

        public void UpdateItems(IEnumerable<TElement> items)
        {
            try
            {
                Check.IsNotNull(items);
                if (!IsLogDisabled)
                    Logger.Current.LogDebug($"Updating items");
                _provider.Update(items);
            }
            catch (CollectionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new CollectionException($"Error updating items", e);
            }
        }

        #endregion

        #region Indexes
        
        internal void CreateIndex(IDatabaseCollectionIndex<TElement> index)
        {
            try
            {
                _provider.CreateIndex(index.Name, index.Fields, index.Options);
            }
            catch (Exception e)
            {
                throw new ContextInitializationException($"Error creating collection index {index.Name}", e);
            }
        }

        internal void CreateIndex<TDerived>(IDatabaseCollectionIndex<TDerived> index)
            where TDerived : TElement
        {
            try
            {
                _provider.CreateIndex<TDerived>(index.Name, index.Fields, index.Options);
            }
            catch (Exception e)
            {
                throw new ContextInitializationException($"Error crating collection index {index.Name}", e);
            }
        }

        internal void DeleteIndex(string name)
        {
            try
            {
                _provider.DeleteIndex(name);
            }
            catch (Exception e)
            {
                throw new ContextInitializationException($"Error deleting collection index {name}", e);
            }
        }

        #endregion

        #region Checks

        protected void EnsureItemId(TElement item)
        {
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();
        }

        protected void CheckInputItem(TElement item)
        {
            Check.IsNotNull(item);
            Check.IsNotEmpty(item.Id);
        }

        protected void CheckInputItemId(string itemId)
        {
            Check.IsNotNullOrWhiteSpace(itemId);
        }

        #endregion
    }
}
