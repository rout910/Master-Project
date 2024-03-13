using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace MVC.Controllers
{
    public class MVCCrudApiController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IEmpRepository _repo;
         private readonly IHttpContextAccessor _httpContextAccessor;

        public MVCCrudApiController(IEmpRepository repo, IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _environment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            return View();
        }

        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _repo.GetAll();
            return Json(cities);
        }

 [HttpGet]
        public IActionResult User()
        {
            var user = HttpContext.Session.GetString("username");
            Console.WriteLine("USER    : : : : : : ::::    " + user);
            List<tblemp> employees = _repo.GetEmployeeFromUserName(user);
            return View(employees);
        }

       //   [HttpGet]
        // public IActionResult User()
        // {
        //       var user = HttpContext.Session.GetString("username");
        //         Console.WriteLine("USER    : : : : : : ::::    " + user);
        //         List<tblemp> employees = _repo.GetEmployeeFromUserName(user);
        //         return View(employees);
        // }


        [HttpPost]
        public IActionResult add(tblemp emp,IFormFile c_profiles)
        {
            var filePath = Path.Combine(@"C:\Users\bharg\OneDrive\Desktop\Bhargav\WebApp\WebApp\wwwroot\", "images", c_profiles.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                c_profiles.CopyTo(stream);
            }

            emp.c_empimage = c_profiles.FileName;
            _repo.Insert(emp);
             return Json(new { success = true});  
        }
        [HttpGet]
        public IActionResult FetchStates()
        {
            List<tbldept> states = _repo.GetDept();
            return Json(states);
        }
       [HttpPost]
public IActionResult UploadImage(IFormFile file)
{
    if (file != null && file.Length > 0)
    {
        var uploads = Path.Combine(_environment.WebRootPath, "images"); // Assuming you have a folder named 'image' in wwwroot
        var filePath = Path.Combine(uploads, file.FileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        var imageUrl= file.FileName; // Assuming your image URL is relative
        return Json(new { imageUrl });
    }
    return Json(new { error = "No file uploaded or file is empty." });
}
        [HttpPost]
        public IActionResult UpdateCity(tblemp city)
        {
                _repo.Update(city);
                return Json(new { success = true });
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
                Console.WriteLine("Received id:" +id);
                _repo.Delete(id);
                return Json(new { success = true, message = "City deleted." });
        }

        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
