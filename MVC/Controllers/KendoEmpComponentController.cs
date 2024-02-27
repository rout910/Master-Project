using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers;

public class KendoEmpComponentController : Controller
{
    private readonly ILogger<KendoEmpComponentController> _logger;
    private readonly IEmpRepository _empRepo;

    public KendoEmpComponentController(ILogger<KendoEmpComponentController> logger, IEmpRepository empRepo)
    {
        _logger = logger;
        _empRepo = empRepo;
    }

    
    
        public IActionResult Index()
        {
            var emp = _empRepo.GetAll();
            return View(emp);
        }

        
        public IActionResult GetDeptName()
        {
            var emp = _empRepo.GetDept();
            return Json(emp);

        }

        [HttpGet]
        public IActionResult Create()
        {
            
            var departments =  _empRepo.GetDept();
                if (departments == null)
    {
        // Handle the case where departments are not retrieved properly
        // You may want to return an error view or display an error message
        // For now, returning an empty list to prevent null reference exception
        departments = new List<tbldept>(); // Change Department to your department class
    }
           
            
           // ViewBag.Departments = departments;
            return View();

        }

       

        [HttpPost]
        public IActionResult Create(tblemp emp)
        {
            if (ModelState.IsValid)
    {
        _empRepo.Insert(emp);
        return Json(emp);
    }
    else
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage);
        return BadRequest(errors);
    }
        }

        [HttpPut]
        public IActionResult Update(int id, [FromBody] tblemp emp)
        {
            if (ModelState.IsValid)
            {
                _empRepo.Update(emp);
                return Json(emp);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _empRepo.Delete(id);
            return Ok();
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
