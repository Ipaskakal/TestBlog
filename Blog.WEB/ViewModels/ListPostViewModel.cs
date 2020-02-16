using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WEB.ViewModels
{
    public class ListPostViewModel
    {
        public ListPostViewModel()
        {
            BlogPosts = new List<ListPostItemViewModel>();
        }
        public List<ListPostItemViewModel> BlogPosts { get;  }

        public int PageNum { get; set; }
        public int PageCount { get; set; }
    }
}
