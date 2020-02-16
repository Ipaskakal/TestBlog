using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WEB.ViewModels
{
    public class ListPostItemViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        /*public string MainPicture { get; set; }*/
        public UserViewModel Author { get; set; }

        public string DateTimeStr { get; set; }

        public int CommentsCount { get; set; }
    }
}
