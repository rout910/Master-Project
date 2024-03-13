using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Microsoft.AspNetCore.Http;

using MVC.Repositories;
using Npgsql;

namespace MVC.controller
{
    // [Route("[controller]")]
    public class MVCUserAPIController : Controller
    {
         private readonly ILogger<MVCUserAPIController> _logger;
        private readonly NpgsqlConnection _conn;
        private readonly IUserRepository _userRepo;


        public MVCUserAPIController(ILogger<MVCUserAPIController> logger, NpgsqlConnection connection, IUserRepository userRepo)
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
        public IActionResult Register()
        {
            return View();
        }

         [HttpPost]
        public IActionResult Register([FromForm]tbluser user)
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
    Console.WriteLine("USERNAME::::" + user.c_username);
    Console.WriteLine("PASSWORD::::" + user.c_password);

    if (_userRepo.Login(user))
    {
        if (user.c_username != null) // Check if username is not null
        {
            string username = user.c_username;
            HttpContext.Session.SetString("username", username);
        }

        var role = HttpContext.Session.GetString("role");
        if (role == "Admin")
        {
            return RedirectToAction("Index", "MVCCrudApi");
        }
        else
        {
            return RedirectToAction("User", "MVCCrudApi");
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