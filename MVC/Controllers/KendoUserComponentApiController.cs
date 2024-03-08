using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repositories;
using Npgsql;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class KendoUserComponentApiController : Controller
    {
        private readonly ILogger<KendoUserComponentApiController> _logger;
        private readonly NpgsqlConnection _conn;
        private readonly IUserRepository _userRepo;


        public KendoUserComponentApiController(ILogger<KendoUserComponentApiController> logger, NpgsqlConnection connection, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
            _conn = connection;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Insert()
        {
            return View();
        }

         [HttpPost]
        public IActionResult Insert([FromForm]tbluser user)
        {
            user.c_userrole = "User";
            _userRepo.Register(user);
            return View();
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
        var role = HttpContext.Session.GetString("c_userrole");
        if (role == "Admin")
        {
            return RedirectToAction("Privacy", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Home");
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