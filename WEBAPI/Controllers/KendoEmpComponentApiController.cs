using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WEBAPI.Models;
using WEBAPI.Repositories;
using System.Collections.Generic;
using System.IO;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KendoEmpComponentApiController : ControllerBase
    {
        private readonly ILogger<KendoEmpComponentApiController> _logger;
        private readonly IEmpRepository _empRepo;
        private readonly IWebHostEnvironment _env;

        public KendoEmpComponentApiController(ILogger<KendoEmpComponentApiController> logger, IEmpRepository empRepo, IWebHostEnvironment env)
        {
            _logger = logger;
            _empRepo = empRepo;
            _env = env;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            var emp = _empRepo.GetAll();
            return Ok(emp);
        }

        [HttpGet]
        [Route("GetEmployee/{id}")]
        public IActionResult GetEmployee(int id)
        {
            var emp = _empRepo.GetOne(id);
            return Ok(emp);
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromForm] tblemp emp, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploads = Path.Combine(_env.ContentRootPath, "..", "MVC", "wwwroot", "images");

                // Ensure the directory exists or create it if it doesn't
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var filePath = Path.Combine(uploads, file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                emp.c_empimage = file.FileName;
            }
    
            _empRepo.Insert(emp);
            return Ok(emp);
        }

       
        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromForm] tblemp emp,IFormFile file)
        {

            if (file != null && file.Length > 0)
            {
                var uploads = Path.Combine(_env.ContentRootPath, "..", "MVC", "wwwroot", "images");

                // Ensure the directory exists or create it if it doesn't
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var filePath = Path.Combine(uploads, file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                emp.c_empimage = file.FileName;
            }
            if (ModelState.IsValid)
            {
                _empRepo.Update(emp);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _empRepo.Delete(id);
            return Ok();
        }

       

        

       

      

       
    }
}
