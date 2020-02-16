using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Blog.DAL.Data;
using Microsoft.AspNetCore.Http;
using Blog.DAL.Interfaces;
using System.Threading.Tasks;

namespace Blog.DAL.Repositories
{
    public class Repository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _DbContext;

        public Repository(AppDbContext dbContext)
        {
            _DbContext = dbContext;

        }
        protected DbSet<TEntity> DbSet
        {
            get
            {
                return _DbContext.Set<TEntity>();
            }
        }

        public void Add(TEntity element)
        {
            DbSet.Add(element);
        }


        public void AddRange(IEnumerable<TEntity> elements)
        {
            DbSet.AddRange(elements);
        }

        public void Remove(TEntity element)
        {
            DbSet.Remove(element);
        }
        public bool Remove(long id)
        {
            var entity = DbSet.Find(id);
            if (entity == null)
                return false;
            DbSet.Remove(entity);
            return true;
        }
        public void RemoveRange(IEnumerable<TEntity> elements)
        {
            DbSet.RemoveRange(elements);
        }

        public void Update(TEntity element)
        {
            DbSet.Update(element);
        }

        public TEntity Get(long id)
        {
            return DbSet.Find(id);
        }
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }
        public IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }
    }
}
