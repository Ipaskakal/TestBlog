using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        ICommentRepository CommentRepository { get; }
        int Commit();
    }
}
