using Blog.DAL.Data;
using Blog.DAL.Interfaces;
using Blog.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        private readonly IFiles files;
        public IPostRepository PostRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public UnitOfWork(AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;
            PostRepository = new PostRepository(_appDbContext,files);
            CommentRepository = new CommentRepository(_appDbContext);
        }
        public async Task<int> Commit()
        {
            return await _appDbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
