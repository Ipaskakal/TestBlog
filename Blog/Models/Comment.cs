using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.DAL.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string Text { get; set; } //Comment text
        public DateTime Created { get; set; } //Time of creation

        public IdentityUser Author { get; set; }

        public string AuthorId { get; set; }
        public Post Post { get; set; }

        
        

    }
}
