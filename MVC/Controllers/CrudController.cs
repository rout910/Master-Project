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

        public CrudController(ILogger<CrudController> logger, IEmpRepository empRepository)
        {
            _logger = logger;
            _empRepository = empRepository;
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
        public IActionResult Insert(tblemp employee)
        {
            _empRepository.Insert(employee);
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

            var departments = _empRepository.GetDept();


            // Populate ViewBag.Courses with the courses collection
            ViewBag.Course = departments;


            var employee = _empRepository.GetOne(id);
            return View(employee);

        }

        [HttpPost]
        public IActionResult Update(tblemp employee)
        {

            _empRepository.Update(employee);
            return RedirectToAction("GetAll");
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




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}