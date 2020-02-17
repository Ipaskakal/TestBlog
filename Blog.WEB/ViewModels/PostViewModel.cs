
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WEB.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            DisplayedComments = new List<CommentViewModel>();
        }
        public long Id { get; set; }
        
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";

        public UserViewModel Author { get; set; }

        public string AuthorId { get; set; }

        public string ImagePath { get; set; } = null;
        public List<CommentViewModel> DisplayedComments { get; }
        public int PageNum { get; set; }
        public int PageCount { get; set; }
    }
}
