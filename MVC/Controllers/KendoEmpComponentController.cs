using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers;

public class KendoEmpComponentController : Controller
{
    private readonly ILogger<KendoEmpComponentController> _logger;
    private readonly IEmpRepository _empRepo;
    private readonly IWebHostEnvironment _env;

    public KendoEmpComponentController(ILogger<KendoEmpComponentController> logger, IEmpRepository empRepo, IWebHostEnvironment env)
    {
        _logger = logger;
        _empRepo = empRepo;
        _env = env;
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

        var departments = _empRepo.GetDept();
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


    public static string img="";
    [HttpPost]
    public IActionResult Create(tblemp emp,IFormFile file)
    {
            

            emp.c_empimage = img;
            _empRepo.Insert(emp);
            return Json(emp);
        
      
    
   
    }

    [HttpPost]
public IActionResult UploadImage(IFormFile file)
{
    if (file != null && file.Length > 0)
    {
        var uploads = Path.Combine(_env.WebRootPath, "images"); // Assuming you have a folder named 'image' in wwwroot
        var filePath = Path.Combine(uploads, file.FileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }

        var imageUrl = file.FileName; // Assuming your image URL is relative
        img = imageUrl;
        return Json(new { imageUrl });
    }
    return Json(new { error = "No file uploaded or file is empty." });
}

[HttpGet]
public IActionResult GetEmployee(int id)
{
     var departments = _empRepo.GetDept();
     ViewBag.Dept = departments;
    var emp = _empRepo.GetOne(id);
    return View(emp);
}

[HttpGet]
public IActionResult Update(int id)
{
    var emp = _empRepo.GetOne(id);
    return View(emp);   
}
    

    // [HttpPost]
    // public IActionResult Update(tblemp emp)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         _empRepo.Update(emp);
    //         return RedirectToAction("Index"); 
            
    //     }
    //     return View(emp);
    // }

    [HttpPost]
public IActionResult Update(tblemp emp)
{
    if (ModelState.IsValid)
    {
        _empRepo.Update(emp);
        Console.WriteLine("---------");
        return RedirectToAction("Index"); 
    }
    return View(emp);
}

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _empRepo.Delete(id);
        return RedirectToAction("Index");
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
