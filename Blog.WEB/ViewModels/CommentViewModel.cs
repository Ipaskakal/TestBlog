using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WEB.ViewModels
{
    public class CommentViewModel
    {   [Required]
        public int PostId { get; set; }
        [Required]
        public string Message { get; set; }

        public DataType Creation { get; set; }

        public string Username { get; set; }
    }
}
