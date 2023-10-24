using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Interfaces;

namespace TestingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _useService;
        public UserController(IUserService useService)
        {
            _useService = useService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsers(string attribute = "name", string order = "asc", int page = 1, int pageSize = 10)
        {
            var users = await _useService.GetAllAsync(page, pageSize, attribute, order);
            
            if(!users.Any())
            {
                return NotFound(page);
            }
            
            return new JsonResult(users);
        }
        
        
        
    }
}
