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
        public IActionResult Add([FromForm] tblemp emp)
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
