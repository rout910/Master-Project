using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Repositories;
using MVC.Models;
using Microsoft.AspNetCore.Http;

namespace MVC.Controllers
{
    public class AjaxUserController : Controller
    {
        private readonly IUserRepositories _userRepositories;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUserRepositories? userRepositories;

        public AjaxUserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepositories = userRepositories;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] tbluser user)
        {
            _userRepositories.Login(user);

            var role = HttpContext.Session.GetString("role");
            if (role == "Admin")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "AjaxEmp") });
            }
            else
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index1", "AjaxEmp") });
            }
        }

        [HttpPost]
        public IActionResult Register([FromBody] tbluser user)
        {
            user.c_userrole = "User";
            _userRepositories.Register(user);
            return Json(new { message = "Registration successful" });
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

    internal interface IUserRepositories
    {
        void Login(tbluser user);
        void Register(tbluser user);
    }
}
