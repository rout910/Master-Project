using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WEBAPI.Models;
using WEBAPI.Repositories;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KendoUserComponentApiController : ControllerBase
    {
        private readonly ILogger<KendoUserComponentApiController> _logger;
        private readonly IUserRepository _userRepo;

        public KendoUserComponentApiController(ILogger<KendoUserComponentApiController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        [HttpPost]
        [Route("Insert")]
        public IActionResult Insert([FromBody] tbluser user)
        {
            _userRepo.Register(user);
            return Ok(); // Assuming a successful insertion. You can also return a specific status code.
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] tbluser user)
        {
            if (_userRepo.Login(user))
            {
                if (HttpContext.Session.GetString("c_userrole") == "Admin")
                {
                    return RedirectToAction("Index", "KendoEmpComponent");
                }
                else
                {
                    return RedirectToAction("Create", "KendoEmpComponent");
                }
            }
            else
            {
                return Unauthorized(); // Or you can return any other appropriate status code.
            }
        }

        
    }
}
