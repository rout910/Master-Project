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
        private readonly IEmpRepository _emprepo;

         public CrudApiController(IEmpRepository empRepository)
        {
            _emprepo = empRepository;
        }

        
        [HttpGet]
        public List<tblemp> GetAll()
        {
                return _emprepo.GetAll();
                // int.Parse(HttpContext.User.Claims.First(i =>i.Type == "UserId").Value);
               
        }
        // [HttpGet("{id}")]
        // public tblemp GetOne(int id)
        // {
        //     return _emprepo.GetOne(id);
        // }


        [HttpPost]
        [Route("Insert")]

        public IActionResult Insert(tblemp stud)
        {
            _emprepo.Insert(stud);
            return Ok();
        }


        [HttpPut]
        public void Update(tblemp stud)
        {
            _emprepo.Update(stud);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _emprepo.Delete(id);
        }
    }
}