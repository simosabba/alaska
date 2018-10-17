using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IDatabaseCollection
    {
    }

    public interface IDatabaseCollection<T>
        where T : IDatabaseCollectionElement
    {
        string CollectionName { get; }
        IQueryable<T> AsQueryable();
        IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : T;
        long Count();
        long Count<TDerived>() where TDerived : T;
        bool ContainsItem(Guid id);
        bool ContainsItem(T item);
        bool ContainsItem(Expression<Func<T, bool>> filter);
        bool ContainsItem<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        IEnumerable<TField> SelectFieldValues<TField>(Expression<Func<T, TField>> selector);
        IEnumerable<Guid> GetAllId();
        IEnumerable<Guid> GetId(Expression<Func<T, bool>> filter);
        IEnumerable<Guid> GetId<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        T GetItem(Guid id);
        T GetItem(Expression<Func<T, bool>> filter);
        TDerived GetItem<TDerived>(Guid id) where TDerived : T;
        TDerived GetItem<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        //T GetItem(Filter<T> filter);
        //TDerived GetItem<TDerived>(Filter<TDerived> filter) where TDerived : T;
        IEnumerable<T> GetItems();
        IEnumerable<TDerived> GetItems<TDerived>() where TDerived : T;
        IEnumerable<T> GetItems(Expression<Func<T, bool>> filter);
        IEnumerable<TDerived> GetItems<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        IEnumerable<T> GetItems(Expression<Func<T, object>> field, Regex pattern);
        IEnumerable<TDerived> GetItems<TDerived>(Expression<Func<TDerived, object>> field, Regex pattern) where TDerived : T;
        //IEnumerable<T> GetItems(Filter<T> filter);
        //IEnumerable<TDerived> GetItems<TDerived>(Filter<TDerived> filter) where TDerived : T;
        //IPartialCollection<T> GetItems(Expression<Func<T, bool>> filter, FilterOptions<T> options);
        //IPartialCollection<TDerived> GetItems<TDerived>(Expression<Func<TDerived, bool>> filter, FilterOptions<TDerived> options) where TDerived : T;
        T AddItem(T item);
        IEnumerable<T> AddItems(IEnumerable<T> items);
        T EnsureItem(T item);
        void UpdateItem(T item);
        void UpdateItems(IEnumerable<T> items);
        void DeleteItem(T item);
        void DeleteItem(Guid id);
        void DeleteItems(IEnumerable<T> items);
        void DeleteItems(Expression<Func<T, bool>> filter);
        //void DeleteItems(Filter<T> filter);
        void DeleteItems<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        //void DeleteItems<TDerived>(Filter<TDerived> filter) where TDerived : T;
        void DeleteAllItems();
        void DeleteAllItems<TDerived>() where TDerived : T;
    }
}
