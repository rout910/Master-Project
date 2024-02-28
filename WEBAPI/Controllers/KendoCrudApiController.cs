using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WEBAPI.Models;
using WEBAPI.Repositories;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KendoCrudApiController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IEmpRepository _repo;

        public KendoCrudApiController(IEmpRepository repo, IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _environment = webHostEnvironment;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var cities = _repo.GetAll();
            return Ok(cities);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] tblemp emp)
        {
            _repo.Insert(emp);
            return Ok(new { success = true });
        }

        [HttpGet("FetchStates")]
        public IActionResult FetchStates()
        {
            List<tbldept> states = _repo.GetDept();
            return Ok(states);
        }

        [HttpPost("UpdateCity")]
        public IActionResult UpdateCity([FromBody] tblemp city)
        {
            _repo.Update(city);
            return Ok(new { success = true });
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Console.WriteLine("Received id:" + id);
            _repo.Delete(id);
            return Ok(new { success = true, message = "City deleted." });
        }

        // [HttpPost("UploadImage")]
        // public IActionResult UploadImage([FromForm] IFormFile file)
        // {
        //     if (file != null && file.Length > 0)
        //     {
        //         var uploads = Path.Combine(_environment.WebRootPath, "images"); // Assuming you have a folder named 'images' in wwwroot
        //         var filePath = Path.Combine(uploads, file.FileName);

        //         using (var fileStream = new FileStream(filePath, FileMode.Create))
        //         {
        //             file.CopyTo(fileStream);
        //         }

        //         var imageUrl = "/images/" + file.FileName; // Assuming your image URL is relative
        //         return Ok(new { imageUrl });
        //     }
        //     return BadRequest(new { error = "No file uploaded or file is empty." });
        // }
    }
}
