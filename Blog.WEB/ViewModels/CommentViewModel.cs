using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WEB.ViewModels
{
    public class CommentViewModel
    {   
        public long Id { get; set; }
        public long PostId { get; set; }
        [Required]
        public string Text { get; set; }

        public DateTime Created { get; set; }

        public IdentityUser Author { get; set; }
    }
}
