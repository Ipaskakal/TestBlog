
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
        public int Id { get; set; }
        
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";

        public UserViewModel Author { get; set; }

        /*public IFormFile Image { get; set; } = null;*/
        public List<CommentViewModel> DisplayedComments { get; }
        public int PageNum { get; set; }
        public int PageCount { get; set; }
    }
}
