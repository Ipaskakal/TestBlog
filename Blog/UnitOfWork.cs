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
            files = new Files();
            PostRepository = new PostRepository(_appDbContext, files);
            CommentRepository = new CommentRepository(_appDbContext, files);
        }
        public  int Commit()
        {
            return  _appDbContext.SaveChanges();
        }
        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
