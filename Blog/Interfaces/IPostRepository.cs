
using Blog.DAL.Data;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.DAL.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        
        IEnumerable<Post> GetPage(int page, int onPage);
        int GetPageCount(int onPage);

        void Add(Post element, IFormFile Image);
    }
}
