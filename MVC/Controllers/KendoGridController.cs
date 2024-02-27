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
    // [Route("[controller]")]
    public class KendoGridController : Controller
    {
        private readonly ILogger<KendoGridController> _logger;
         private readonly IUserRepository _userRepo;
        public KendoGridController(ILogger<KendoGridController> logger,IUserRepository userRepo)
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
            return RedirectToAction("Login","KendoGridCrud");

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
public IActionResult Login(tbluser user)
{
    if(_userRepo.Login(user))
    {
        if(HttpContext.Session.GetString("c_userrole")=="Admin")
        {
            return RedirectToAction("Index", "KendoGridCrud");
        
        }
        else
        {
            return RedirectToAction("Index", "Home");
        
            }
    }
    else{
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