﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Configuration;
using Blog.WEB.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Blog.WEB.Controllers
{
    public class PostController : Controller
    {
        private const int ON_PAGE = 10;

        private readonly DAL.Interfaces.IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private ILogger _logger;


        public PostController(UserManager<IdentityUser> userManager, DAL.Interfaces.IUnitOfWork unitOfWork, IMapper mapper,
            IConfiguration configuration, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Post(long id, int? page) //Show post, id is post id,  page is for comments
        {
            if (page == null)
                page = 1;
            var post = _unitOfWork.PostRepository.Get(id);
            if (post != null)
            {
                var model = new PostViewModel();
                _mapper.Map(post, model);
                model.PageCount = _unitOfWork.CommentRepository.GetCommentsPageCount(id, ON_PAGE);
                if (model.PageCount == 0)
                {
                    page = 1;
                }
                else
                {
                    if (page > model.PageCount)
                        page = model.PageCount;
                }
                model.PageNum = (int)page;
                var comments = _unitOfWork.CommentRepository.GetCommentsPage(post.Id, model.PageNum, ON_PAGE); //Comments on this page
                if (comments != null)
                {
                    CommentViewModel temp;
                    foreach (var comment in comments)
                    {
                        temp = new CommentViewModel();
                        _mapper.Map(comment, temp);
                        model.DisplayedComments.Add(temp);

                    }
                }

                return View(model);
            }
            return View("Error");
        }

        public IActionResult Delete(long id, int page, long postId)
        {
            if (!_unitOfWork.CommentRepository.Remove(id))
                return View("Error"); 
            _unitOfWork.Commit();
            return RedirectToAction( "Post", new { id = postId, page });
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel model)
        {
            ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;

            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = model.PostId });

            if (ReCaptchaPassed(
            Request.Form["g-recaptcha-response"], 
            _configuration.GetSection("GoogleReCaptcha:secret").Value,
            _logger
            ).Result)
            {
                var entity = new DAL.Models.Comment
                {
                    AuthorId = _userManager.GetUserId(User),
                    PostId = model.PostId,
                    Created = DateTime.Now,
                    Text = model.Text
                };
                _unitOfWork.CommentRepository.Add(entity);
                _unitOfWork.Commit();
                return RedirectToAction( "Post", new { id = model.PostId });
            }
            return RedirectToAction("Post", new { id = model.PostId }); //If not - retry
        }

        public async Task<bool>  ReCaptchaPassed(string gRecaptchaResponse, string secret, ILogger logger)
        {
            HttpClient httpClient = new HttpClient();
            _logger = logger;
            HttpResponseMessage res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
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

    }
}