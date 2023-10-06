using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;


namespace User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly WebApplication _DbContext;
        public UserController(WebApplication dbContext)
        {
            _DbContext = dbContext;
        }
      
    }
}
