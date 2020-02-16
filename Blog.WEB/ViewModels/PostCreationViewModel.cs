using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.WEB.ViewModels
{
    public class PostCreationViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Title")]
        public string Title { get; set; } = "";
        [Required]
        [Display(Name = "Text of post")]
        public string Text { get; set; } = "";
        
        public string AuthorId { get; set; }
        /*public IFormFile Image { get; set; } = null;*/
    }
}
