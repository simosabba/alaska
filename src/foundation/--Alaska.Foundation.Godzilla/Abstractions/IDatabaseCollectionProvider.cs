using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IDatabaseCollectionProvider<T>
        where T : IDatabaseCollectionElement
    {
        void Configure(Settings.DatabaseCollectionProviderOptions options);
        long Count();
        long Count<TDerived>() where TDerived : T;
        bool Contains(Guid itemId);
        bool Contains(Expression<Func<T, bool>> filter);
        bool Contains<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        IEnumerable<TField> SelectFieldValues<TField>(Expression<Func<T, TField>> selector);
        IEnumerable<TField> SelectFieldValues<TField>(Expression<Func<T, TField>> selector, Expression<Func<T, bool>> filter);
        IEnumerable<Guid> GetAllId();
        IEnumerable<Guid> GetId(Expression<Func<T, bool>> filter);
        IEnumerable<Guid> GetId<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        IEnumerable<T> Get();
        IEnumerable<TDerived> Get<TDerived>() where TDerived : T;
        T Get(Guid itemId);
        TDerived Get<TDerived>(Guid itemId) where TDerived : T;
        IEnumerable<T> Get(Expression<Func<T, bool>> filter);
        IEnumerable<TDerived> Get<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        IEnumerable<T> Get(Expression<Func<T, object>> field, Regex pattern);
        IEnumerable<TDerived> Get<TDerived>(Expression<Func<TDerived, object>> field, Regex pattern) where TDerived : T;
        //IEnumerable<T> Get(Filter<T> filter);
        //IEnumerable<TDerived> Get<TDerived>(Filter<TDerived> filter) where TDerived : T;
        //IPartialCollection<T> Get(Expression<Func<T, bool>> filter, FilterOptions<T> options);
        //IPartialCollection<TDerived> Get<TDerived>(Expression<Func<TDerived, bool>> filter, FilterOptions<TDerived> options) where TDerived : T;
        void Update(T item);
        void Update(IEnumerable<T> items);
        void Delete(Guid itemId);
        void Delete(Expression<Func<T, bool>> filter);
        void Delete<TDerived>(Expression<Func<TDerived, bool>> filter) where TDerived : T;
        //void Delete(Filter<T> filter);
        //void Delete<TDerived>(Filter<TDerived> filter) where TDerived : T;
        void Add(T item);
        void Add(IEnumerable<T> items);

        void CreateIndex(string name, IEnumerable<IIndexField<T>> fields, IIndexOptions options);
        void CreateIndex<TDerived>(string name, IEnumerable<IIndexField<TDerived>> fields, IIndexOptions options) where TDerived : T;
        void DeleteIndex(string name);
        bool ContainsIndex(string name);
        IEnumerable<string> GetIndexes();
        IQueryable<T> AsQueryable();
        IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : T;
    }
}
