using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;

namespace Blog.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity element);
        void AddRange(IEnumerable<TEntity> elements);

        void Remove(TEntity element);
        bool Remove(long elementId);
        void RemoveRange(IEnumerable<TEntity> elements);

        void Update(TEntity element);

        TEntity Get(long id);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> predicate);
    }
}
