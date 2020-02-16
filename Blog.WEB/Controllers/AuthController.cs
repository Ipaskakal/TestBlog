using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.WEB.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WEB.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (login != null)
            {
                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, false).ConfigureAwait(true);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(true);
            return RedirectToAction("Index", "Home");
            
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel regist)
        {
            if(!ModelState.IsValid || regist==null)
            {
                return View(regist);
            }
            var user = new IdentityUser
            {
                UserName = regist.UserName,
                Email = regist.Email,
            };
            var result = await _userManager.CreateAsync(user, regist.Password).ConfigureAwait(true);
            if (result.Succeeded)
            {
                 await _signInManager.SignInAsync(user, false).ConfigureAwait(true);
                return RedirectToAction("Index", "Home");
            }
            return View(regist);

        }
    }
}