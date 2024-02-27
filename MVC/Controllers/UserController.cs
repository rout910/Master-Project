using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userrepo;

        public UserController(ILogger<UserController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userrepo = userRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(tbluser user)
        {
            if (_userrepo.Login(user))
            {
                var role = HttpContext.Session.GetString("role");
                if (role == "Admin")
                {
                    return RedirectToAction("Index", "AdminCRUD");
                }
                else
                {
                    return RedirectToAction("GetAll", "Crud");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(tbluser user)
        {
            if (_userrepo.IsUser(user.c_emailid))
            {
                ViewBag.msg = "*User already exists, please use a different email address";
                return View();
            }
            else
            {
                _userrepo.Register(user);
                ViewBag.SuccessMessage = "Registered Successfully";
                return RedirectToAction("Login", "User");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
