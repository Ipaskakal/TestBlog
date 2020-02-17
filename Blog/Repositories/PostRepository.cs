using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Blog.DAL.Interfaces;
using Blog.DAL.Models;
using Blog.DAL.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Blog.DAL.Repositories
{
    class PostRepository : Repository<Post>, IPostRepository
    {
        

        public PostRepository(AppDbContext blogContext,IFiles files) : base(blogContext, files) 
        {
            
        }

        

        public new Post Get(long id)
        {
            return DbSet
                .Include(x => x.Comments)
                .Include(x => x.Author)
                .FirstOrDefault(x => x.Id == id);
        }


        public int GetPageCount(int onPage)
        {
            int count = DbSet.Count();
            return count / onPage + (count % onPage == 0 ? 0 : 1);
        }
        public IEnumerable<Post> GetPage(int page, int onPage)
        {
            int skippedCount = onPage * (page - 1);
            int postCount = DbSet.Count();
            if (page > GetPageCount(onPage))
                return null;
            if (postCount == 0)
                return new List<Post>();
            return DbSet.OrderByDescending(x => x.Created)
                .Skip(skippedCount)
                .Take(Math.Min(onPage, postCount - skippedCount))
                .Include(x => x.Author)
                .Include(x => x.Comments)
                
                .ToList();
        }

       
    }
}
