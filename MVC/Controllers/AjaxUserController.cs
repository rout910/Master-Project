using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class AjaxUserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepositories;

        public AjaxUserController(IUserRepository userRepositories, IHttpContextAccessor httpContextAccessor)
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] tbluser user)
        {
            Console.WriteLine("USSSSEEEEEEEEERRRRRRR::::" + user.c_emailid);
            Console.WriteLine("PASSSSSWORD::::" + user.c_password);
            _userRepositories.Login(user);

            var role = HttpContext.Session.GetString("role");
            if (role == "Admin")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "AjaxEmp") });
            }
            else
            {
                return Json(new { success = true, redirectUrl = Url.Action("User", "AjaxEmp") });
            }
        }

        [HttpPost]
        public IActionResult Register([FromBody] tbluser user)
        {
            try
            {
                user.c_userrole = "User";
                _userRepositories.Register(user);
                return Json(new { success = true, redirectUrl = Url.Action("Login", "AjaxUser") });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message; // Set the error message in ViewBag
                return View(); // Return the view with the error message
            }
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
