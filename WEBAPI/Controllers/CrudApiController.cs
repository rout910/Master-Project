using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEBAPI.Models;
using WEBAPI.Repositories;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudApiController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IEmpRepository _repo;

        public CrudApiController(IEmpRepository repo, IWebHostEnvironment webHostEnvironment)
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
        public IActionResult Add([FromForm] tblemp emp)
        {
            //  var filePath = Path.Combine(@"C:\Users\bharg\OneDrive\Desktop\Bhargav\WebApp\WebApp\wwwroot\", "images", c_profiles.FileName);

            // using (var stream = new FileStream(filePath, FileMode.Create))
            // {
            //     c_profiles.CopyTo(stream);
            // }

            // emp.c_empimage = c_profiles.FileName;
            _repo.Insert(emp);
            return Ok(new { success = true });
        }

        [HttpGet("FetchStates")]
        public IActionResult FetchStates()
        {
            List<tbldept> states = _repo.GetDept();
            return Ok(states);
        }

        [HttpPut("UpdateCity")]
        public IActionResult UpdateCity([FromForm] tblemp city)
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
       }
}