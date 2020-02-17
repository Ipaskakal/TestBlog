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
using Blog.DAL.Models;
using System.IO;

namespace Blog.DAL.Repositories
{
    public class Repository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _DbContext;
        private readonly IFiles _files;
        public Repository(AppDbContext dbContext, IFiles files)
        {
            _DbContext = dbContext;
            _files = files;

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
            if (element is Post)
            {
                (element as Post).ImagePath = _files.SaveImage((element as Post).Image );
            }
            
            DbSet.Add(element);
        }

        public FileStream GetImageStream(string image)
        {
            return _files.GetImageStream(image);
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
