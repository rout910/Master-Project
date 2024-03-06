using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class CrudController : Controller
    {
        private readonly ILogger<CrudController> _logger;

        private readonly IEmpRepository _empRepository;

        private readonly IWebHostEnvironment _environment;

        public CrudController(ILogger<CrudController> logger, IEmpRepository empRepository, IWebHostEnvironment environment)
        {
            _logger = logger;
            _empRepository = empRepository;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            string username = HttpContext.Session.GetString("UserName");

            ViewBag.UserName = username;
            var employees = _empRepository.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Insert()
        {
            var courses = _empRepository.GetDept();
            ViewBag.Courses = new SelectList(courses, "c_depid", "c_dename");

            return View();
        }
        [HttpPost]
        public IActionResult Insert(tblemp emp, IFormFile photo)
        {
            if (photo != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");


                string uniqueFilename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFilename);


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                Console.WriteLine("Upload PHOTO ::::    " + uniqueFilename);
                emp.c_empimage = uniqueFilename;

                Console.WriteLine("C IMAGE : : : : :      " + emp.c_empimage);
            }
            else
            {
                Console.WriteLine("NOT FOUND");
            }
            Console.WriteLine(emp.c_empimage);
            _empRepository.Insert(emp);
            return RedirectToAction("GetAll");
        }



        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            // if(HttpContext.Session.GetString("username")==null)
            // {
            //     return RedirectToAction("Login","User");
            // }
            var employee = _empRepository.GetOne(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
[HttpGet]
public IActionResult Update(int id)
{
    var courses = _empRepository.GetDept();
    ViewBag.Courses = new SelectList(courses, "c_depid", "c_dename");

    var employee = _empRepository.GetOne(id);
    if (employee == null)
    {
        return NotFound();
    }

    // Assuming c_empimage contains the filename of the image
    employee.c_empimage = Path.Combine("/images/", employee.c_empimage);

    return View(employee);
}


       [HttpPost]
public IActionResult Update(tblemp employee, IFormFile photo)
{
    if (ModelState.IsValid)
    {
        if (photo != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");


                string uniqueFilename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFilename);


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                Console.WriteLine("Upload PHOTO ::::    " + uniqueFilename);
                employee.c_empimage = uniqueFilename;

                Console.WriteLine("C IMAGE : : : : :      " + employee.c_empimage);
            }
            else
            {
                Console.WriteLine("NOT FOUND");
            }
        _empRepository.Update(employee);
        Console.WriteLine("+++");
        return RedirectToAction("GetAll");
    }
    else
    {
        // Handle invalid model state, possibly return the same view with validation errors
        return View(employee);
    }
}


        [HttpGet]
        public IActionResult Delete(int id)
        {
            // if(HttpContext.Session.GetString("username")==null)
            // {
            //     return RedirectToAction("Login","User");
            // }
            var employee = _empRepository.GetOne(id);
            return View(employee);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _empRepository.Delete(id);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public IActionResult User()
        {
            var user = HttpContext.Session.GetString("username");
            Console.WriteLine("USER    : : : : : : ::::    " + user);
            List<tblemp> employees = _empRepository.GetEmployeeFromUserName(user);
            return View(employees);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}