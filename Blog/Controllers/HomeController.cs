using Blog.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Blog.Data.Repository;
using Blog.ViewModels;
using Blog.Data.Files;
using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;
        private IFiles _files;
        private IConfiguration _configuration;
        private static ILogger _logger;

        public HomeController(IRepository repository, IFiles files,IConfiguration configuration )
        {
            _repository = repository;
            _files = files;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        public IActionResult Post(int Id)
        {
            var post = _repository.GetPost(Id);
           
            return View(post);
        }

        public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret, ILogger logger)
        {
            HttpClient httpClient = new HttpClient();
            _logger = logger;
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                logger.LogError("Error while sending request to ReCaptcha");
                return false;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel commentViewModel)
        {
            ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;
            
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = commentViewModel.PostId });

            if (ReCaptchaPassed(
            Request.Form["g-recaptcha-response"], // that's how you get it from the Request object
            _configuration.GetSection("GoogleReCaptcha:secret").Value,
            _logger
            ))
            {
                var post = _repository.GetPost(commentViewModel.PostId);
            post.Comments = post.Comments ?? new List<Comment>();
            post.Comments.Add(new Comment
            {
                Text = commentViewModel.Message,
                Created = DateTime.Now,
                UserName = User.Identity.Name
            }); ;
            _repository.UpdatePost(post);
            await _repository.SaveChangesAsync();
                return RedirectToAction("Post", post);
            }
            return RedirectToAction("Post", new { id = commentViewModel.PostId });
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image (string image)
        {
            var afterDot = image.Substring(image.LastIndexOf('.')+1);
            var img = _files.GetImageStream(image);
            if (img == null)
                return null;
            return new FileStreamResult(img, $"image/{afterDot} ");
        }

        [HttpGet]
        public IActionResult EditPost(int? id)
        {
            if (id == null)
                return View(new PostViewModel());
            else
            {
                var post = _repository.GetPost((int)id);
                return View(new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body
                    
                });

            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost(PostViewModel postViewModel)
        {
            var post = new Post
            {
                Id = postViewModel.Id,
                Title = postViewModel.Title,
                Body = postViewModel.Body,
                Image = await _files.SaveImage(postViewModel.Image),
                UserName = User.Identity.Name
            };
            if (post.Id > 0)
                _repository.UpdatePost(post);
            else
                _repository.AddPost(post);
            if (await _repository.SaveChangesAsync())
                return RedirectToAction("Index");
            else
                return View(post);
        }


    }
}
