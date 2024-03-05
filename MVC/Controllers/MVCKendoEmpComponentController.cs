using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using System.Collections.Generic;

namespace MVC.Controllers
{
    public class MVCKendoEmpComponentController : Controller
    {
        private readonly IEmpRepository _empRepo;

        public MVCKendoEmpComponentController(IEmpRepository empRepo)
        {
            _empRepo = empRepo;
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult Index()
        {
            var emp = _empRepo.GetAll();
            return View(emp);
        }
        

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetDeptName()
        {
            var emp = _empRepo.GetDept();
            return Json(emp);
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Create([FromBody] tblemp emp)
        {
            _empRepo.Insert(emp);
            return Json(emp);
        }
        [HttpGet]
        public IActionResult CreateOrUpdate(int? id)
        {
            if (id == null)
            {
                return View(new tblemp());
            }
            else
            {
                var emp = _empRepo.GetOne(id.Value);
                if (emp == null)
                {
                    return NotFound();
                }
                return View(emp);
            }
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(tblemp emp)
        {
            if (ModelState.IsValid)
            {
                if (emp.c_empid == 0)
                {
                    _empRepo.Insert(emp);
                }
                else
                {
                    _empRepo.Update(emp);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(emp);
        }

        

        [HttpPost]
        [Produces("application/json")]
        public IActionResult UploadImage([FromBody] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Process the file upload
                return Json(new { message = "File uploaded successfully." });
            }
            else
            {
                return BadRequest(new { error = "No file uploaded or file is empty." });
            }
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetEmployee(int id)
        {
            var emp = _empRepo.GetOne(id);
            return Json(emp);
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult Update(int id, [FromBody] tblemp emp)
        {
            if (ModelState.IsValid)
            {
                _empRepo.Update(emp);
                return Json(emp);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult Delete(int id)
        {
            _empRepo.Delete(id);
            return Json(new { message = "Employee deleted successfully." });
        }

        public IActionResult Privacy()
        {
            return Json(new { message = "Privacy policy" });
        }

        public IActionResult Error()
        {
            return NotFound(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
