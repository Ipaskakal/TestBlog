
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
        const int ON_PAGE = 5;
        private readonly IMapper _mapper;
        private readonly DAL.Interfaces.IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(DAL.Interfaces.IUnitOfWork unitOfWork, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       
        public IActionResult Index(int id)// id of page of posts
        {
            var model = new ListPostViewModel
            {
                PageCount = _unitOfWork.PostRepository.GetPageCount(ON_PAGE)
            };
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

                model.AuthorId = _userManager.GetUserId(User); //Setting current user as author

                if (ModelState.IsValid)
                {
                    var entity = new DAL.Models.Post();
                    _mapper.Map(model, entity);
                    entity.Created = DateTime.Now;
                    entity.Image = model.Image;
                    _unitOfWork.PostRepository.Add(entity);
                    int x = _unitOfWork.Commit();
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




      

    }
}
