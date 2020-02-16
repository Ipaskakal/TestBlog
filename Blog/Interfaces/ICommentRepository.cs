using System;
using System.Collections.Generic;
using System.Text;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        int GetCommentsPageCount(long postId, int onPage);
        IEnumerable<Comment> GetCommentsPage(long postId, int page, int onPage);
    }
}
