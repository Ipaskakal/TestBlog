
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Configuration;
using Blog.WEB.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Blog.WEB.Controllers
{
    public class HomeController : Controller
    {
        const int ON_PAGE = 10;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private DAL.Interfaces.IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(DAL.Interfaces.IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
       
        public IActionResult Index(int id)// id of page of posts
        {
            var model = new ListPostViewModel();
            model.PageCount = _unitOfWork.PostRepository.GetPageCount(ON_PAGE);
            if (id > model.PageCount)
                id = model.PageCount;
            model.PageNum = id;
            var Posts = _unitOfWork.PostRepository.GetPage(model.PageNum, ON_PAGE);//Get posts displaying on this page
            if (Posts != null)
            {
                _mapper.Map(Posts, model.BlogPosts);
                int i = 0;
                foreach (var post in Posts)
                {
                    model.BlogPosts[i].CommentsCount = post.Comments.Count;
                    i++;
                } 
                return View(model);
            }
            return View("Error");
        }

        [HttpPost]
        public IActionResult Create(PostCreationViewModel model) //Checking is new post is valid and saving it to database
        {
            if (model != null)
            {
                

                if (ModelState.IsValid)
                {
                    var entity = new DAL.Models.Post();
                    model.AuthorId = _userManager.GetUserId(User); //Setting current user as author
                    _mapper.Map(model, entity);
                    entity.Created = DateTime.Now;
                    _unitOfWork.PostRepository.Add(entity);
                    var x= _unitOfWork.Commit();
                    return RedirectToAction("Index", "Home");
                }
            }
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }


        /*
             

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
                }*/


    }
}
