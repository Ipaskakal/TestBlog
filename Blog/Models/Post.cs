using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.DAL.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Title { get; set; } //Title of post
        public string Text { get; set; } //Text of post
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public DateTime Created { get; set; } //Time of creation
        public ICollection<Comment> Comments { get; set; }=new List<Comment>();
        public string AuthorId { get; set; }
        public IdentityUser Author { get; set; }

    }
}
