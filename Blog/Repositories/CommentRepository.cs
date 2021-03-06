﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Blog.DAL.Interfaces;
using Blog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Blog.DAL.Data;

namespace Blog.DAL.Repositories
{
    class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext dbContext, IFiles files) : base(dbContext, files)
        {
        }

        public int GetCommentsPageCount(long postId, int onPage)
        {
            int count = DbSet.Where(x => x.PostId == postId).Count();
            return count / onPage + (count % onPage == 0 ? 0 : 1);
        }

        public List<long> GetCommentsByPostId(long postId)
        {
            return DbSet.Where(x => x.PostId == postId).Select(x=>x.Id).ToList();
        }
        public IEnumerable<Models.Comment> GetCommentsPage(long postId, int page, int onPage)
        {
            if (page > GetCommentsPageCount(postId, onPage))
                return null;
            return DbSet
                .Where(x => x.PostId == postId)
                .OrderByDescending(x => x.Created)
                .Skip(onPage * (page - 1)).Take(onPage)
                .Include(x => x.Author)
                .ToList();
        }

        private IEnumerable<Models.Comment> GetPage(IQueryable<Models.Comment> query, int page, int onPage)
        {
            return query.OrderByDescending(x => x.Created).Skip(onPage * (page - 1)).Take(onPage).ToList();
        }
    }
}
