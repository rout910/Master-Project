using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    //[Route("[controller]")]
    public class KendoUserComponentController : Controller
    {
        private readonly ILogger<KendoUserComponentController> _logger;

        private readonly IUserRepository _userRepo;

        public KendoUserComponentController(ILogger<KendoUserComponentController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Insert(tbluser user)
        {
            _userRepo.Register(user);
            return RedirectToAction("Login", "KendoUserComponent");

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(tbluser user)
        {
            if (_userRepo.Login(user))
            {
                if (HttpContext.Session.GetString("c_userrole") == "Admin")
                {
                    return RedirectToAction("Index", "KendoEmpComponent");

                }
                else
                {
                    return RedirectToAction("Create", "KendoEmpComponent");

                }
            }
            else
            {
                return View();
            }
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}