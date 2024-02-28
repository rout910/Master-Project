using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
   // [Route("[controller]")]
    public class KendoGridCrudApiController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IEmpRepository _repo;
        public KendoGridCrudApiController( IEmpRepository repo,IWebHostEnvironment webHostEnvironment)
        {
            _repo=repo;
            _environment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Produces("application/json")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _repo.GetAll();
            return Json(cities);
        }
        [HttpPost]
        public IActionResult add(tblemp emp)
        {
            _repo.Insert(emp);
             return Json(new { success = true});  
        }
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

        var imageUrl = "/images/" + file.FileName; // Assuming your image URL is relative
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}