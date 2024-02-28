using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MVC.Repositories;


namespace MVC.Controllers
{
   //
   [Route("[controller]")]
    public class MVCCrudApiController : Controller
    {
        private readonly IEmpRepository _emp;
        private readonly ILogger<MVCCrudApiController> _logger;

        public MVCCrudApiController(ILogger<MVCCrudApiController> logger,IEmpRepository empRepository)
        {
            _logger = logger;
            _emp = empRepository;
           
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

         [Route("Update")]
         [HttpGet("{id})")]
         
        public IActionResult Update(int id)
        {
            ViewBag.c_empid = id;
            ViewBag.Title = "Update";
            return View(id);
            var department = _emp.GetDept();
            ViewBag.Departments = department;
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}