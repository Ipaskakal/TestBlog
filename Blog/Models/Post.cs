using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }="";
        public string Body { get; set; } = "";

        public string Image { get; set; } = "";

        public DateTime Created { get; set; } = DateTime.Now;

        public List<Comment> Comments { get; set; }

        [Required]
        public string UserName { get; set; }

    }
}
