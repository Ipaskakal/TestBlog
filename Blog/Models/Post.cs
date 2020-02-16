using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.DAL.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Title { get; set; } //Title of post
        public string Text { get; set; } //Text of post
        /*public string Picture { get; set; } */
        public DateTime Created { get; set; } //Time of creation
        public ICollection<Comment> Comments { get; set; }
        public string AuthorId { get; set; }
        public IdentityUser Author { get; set; }

    }
}
