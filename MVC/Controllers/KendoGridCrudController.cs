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
    public class KendoGridCrudController : Controller
    {
        private readonly ILogger<KendoGridCrudController> _logger;
        private readonly IEmpRepository _empRepo;
        private readonly IWebHostEnvironment _environment;
        public KendoGridCrudController(ILogger<KendoGridCrudController> logger,IEmpRepository empRepo,IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
              _empRepo = empRepo;
              _environment = webHostEnvironment;
        }

       [Produces("application/json")]
       [HttpGet]
        public IActionResult Index()
        {
           
            return View();
        }

        [Produces("application/json")]
       [HttpGet]
        public IActionResult GetAll()
        {
            var emp =_empRepo.GetAll();
            return Json(emp);
        }
        public IActionResult FetchStates()
        {
            List<tbldept> states = _empRepo.GetDept();
            return Json(states);
        }
        [HttpPost]
        public IActionResult add(tblemp stud)
        {
            _empRepo.Insert(stud);
             return Json(new { success = true, newCityId = stud.c_empid});  
        }
      
        [HttpPost]
        public IActionResult UploadPhoto(IFormFile photo)
        {
            try
            {
                if (photo != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                    string filepath = Path.Combine(_environment.WebRootPath, "images", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }

                    var stateModel = new tblemp { c_empimage = filename };
                    _empRepo.Insert(stateModel);

                    return Json(new { success = true, filename });
                }
                else
                {
                    return Json(new { success = false, message = "No file uploaded" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}