/*using System;
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

  
        [HttpGet]
        public  IActionResult Remove(int id, int page)
        {
            if (!_unitOfWork.PostRepository.Remove(id))
            _unitOfWork.Commit();
            return RedirectToAction("Index", "Home", new { id = page }); 
        }

        

        


    }
}*/