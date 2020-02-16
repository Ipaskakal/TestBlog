using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.WEB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DAL.Interfaces.IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminController(UserManager<IdentityUser> userManager, DAL.Interfaces.IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public IActionResult EditPost(int id)
        {
            var model = new PostCreationViewModel();
            var post = _unitOfWork.PostRepository.Get(id);
            if (post != null)
            {
                _mapper.Map(post, model);
                return View("Create", model);
            }
            return View("Error");
        }

        [HttpPost]
        public IActionResult Edit(PostCreationViewModel model) //Checking is edited post is valid and saving it to database 
        {
            if (ModelState.IsValid)
            {
                var entity = _unitOfWork.PostRepository.Get((long)model.Id);
                if (entity == null)
                    return View("Error");
                _mapper.Map(model, entity);
                _unitOfWork.PostRepository.Update(entity);
                _unitOfWork.Commit();
                return RedirectToAction("Show", "Post", new { id = model.Id }); //Opening edited post
            }
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id, int page)
        {
            if (!_unitOfWork.PostRepository.Remove(id))
                return View("Error"); //There's no such post in database
            await _unitOfWork.Commit().ConfigureAwait(true);
            return RedirectToAction("Index", "Home", new { id = page }); //Return to homepage
        }

        [HttpPost]
        public IActionResult Create(PostCreationViewModel model) //Checking is new post is valid and saving it to database
        {
            if (ModelState.IsValid)
            {
                var entity = new DAL.Models.Post();
                model.AuthorId = _userManager.GetUserId(User); //Setting current user as author
                _mapper.Map(model, entity);
                entity.Created = DateTime.Now;
                _unitOfWork.PostRepository.Add(entity);
                _unitOfWork.Commit();
                return RedirectToAction("Show", "Post", new { id = model.Id }); //Openint edited post
            }
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }


    }
}