using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = "";
        public DateTime Created { get; set; }
        [Required]
        public int PostId { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
