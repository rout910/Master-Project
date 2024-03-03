using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class AjaxEmpController : Controller
    {
        private readonly ILogger<AjaxEmpController> _logger;
        private readonly IEmpRepository _empRepository;

        public AjaxEmpController(ILogger<AjaxEmpController> logger, IEmpRepository empRepository)
        {
            _logger = logger;
            _empRepository = empRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _empRepository.GetAll();
            return View(employees);
            return View();
        }
         public IActionResult GetAllDept()
        {
            List<tbldept> states = _empRepository.GetDept();
            return Json(states);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
           
            var employees = _empRepository.GetAll();
            return Json(employees);
        }

        [HttpGet]
        public IActionResult Insert()
        {
            var courses = _empRepository.GetDept();
            ViewBag.Courses = new SelectList(courses, "c_depid", "c_dename");
            return Json(new { success = true, message = "Insert page" });
        }

        [HttpPost]
        public IActionResult Insert(tblemp stud)
        {
            _empRepository.Insert(stud);
            return Json(new { success = true, message = "Employee inserted successfully" });
        }

        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            var employee = _empRepository.GetOne(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Json(employee);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var departments = _empRepository.GetDept();
            ViewBag.Course = departments;
            var employee = _empRepository.GetOne(id);
            return Json(employee);
        }

        [HttpPost]
        public IActionResult Update(tblemp employee)
        {
            _empRepository.Update(employee);
            return Json(new { success = true, message = "Employee updated successfully" });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _empRepository.GetOne(id);
            return Json(employee);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _empRepository.Delete(id);
            return Json(new { success = true, message = "Employee deleted successfully" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
