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
    [Route("[controller]")]
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
        
        [Route("Login")]
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