using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class AjaxEmpController : Controller
    {
        private readonly ILogger<AjaxEmpController> _logger;
        private readonly IEmpRepository _empRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AjaxEmpController(ILogger<AjaxEmpController> logger, IEmpRepository empRepository, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _empRepository = empRepository;
            _webHostEnvironment = webHostEnvironment;
        }
          [HttpGet]
        public IActionResult User()
        {
            var user = HttpContext.Session.GetString("username");
            Console.WriteLine("USER    : : : : : : ::::    " + user);
            List<tblemp> employees = _empRepository.GetEmployeeFromUserName(user);
            return View(employees);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _empRepository.GetAll();
            return View(employees);
        }

        [HttpGet]
        public IActionResult GetAllDept()
        {
            List<tbldept> departments = _empRepository.GetDept();
            return Json(departments);
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
            var departments = _empRepository.GetDept();
            ViewBag.Departments = new SelectList(departments, "c_depid", "c_dename");
            return View("Insert"); // Returning a view instead of JSON
        }

        [HttpPost]
        public IActionResult Insert(tblemp employee, IFormFile c_empimage)
        {
            try
            {
                // Check if the image is provided and valid
                if (c_empimage != null && c_empimage.Length > 0 && IsImage(c_empimage))
                {
                    // Generate a unique file name
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + c_empimage.FileName;
                    // Define the file path where the image will be saved
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                    // Save the image to the file system
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        c_empimage.CopyTo(stream);
                    }

                    // Assign the file name to the employee object
                    employee.c_empimage = uniqueFileName;
                }

                // Insert the employee into the repository
                _empRepository.Insert(employee);

                // Return a success message
                return Json(new { success = true, message = "Employee inserted successfully" });
            }
            catch (Exception ex)
            {
                // Return an error message if an exception occurs
                return Json(new { success = false, message = $"Error inserting employee: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            
            try
            {
                // Fetch employee details by id from the database
                var employee =  _empRepository.GetOne(id);// Implement this method to fetch employee by id

                if (employee != null)
                {
                    // Return JSON response with employee details
                    return Json(employee);
                }
                else
                {
                    return Json(new { error = "Employee not found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error fetching employee details: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {

            

            var departments = _empRepository.GetDept();
            ViewBag.Departments = new SelectList(departments, "c_depid", "c_dename");

            var employee = _empRepository.GetOne(id);

            employee.c_empimage = Path.Combine("/images/", employee.c_empimage);
            // return View(employee); // Returning a view instead of JSON
             return Json(employee);
        }

       [HttpPost]
public IActionResult Update(tblemp employee, IFormFile c_empimage)
{
    try
    {
        // Check if the image is provided and valid
        if (c_empimage != null && c_empimage.Length > 0 && IsImage(c_empimage))
        {
            // Generate a unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + c_empimage.FileName;
            // Define the file path where the image will be saved
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

            // Save the image to the file system
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                c_empimage.CopyTo(stream);
            }

            // Assign the file name to the employee object
            employee.c_empimage = uniqueFileName;
        }

        // Update the employee in the repository
        _empRepository.Update(employee);

        // Return a success message
        return Json(new { success = true, message = "Employee updated successfully" });
    }
    catch (Exception ex)
    {
        // Return an error message if an exception occurs
        return Json(new { success = false, message = $"Error updating employee: {ex.Message}" });
    }
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

        // Helper method to check if the file is an image
        private bool IsImage(IFormFile file)
        {
            return file.ContentType.StartsWith("image");
        }
    }
}
