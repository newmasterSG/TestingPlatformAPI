using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Application.Services;
using TestingPlatform.Domain.Entities;
using TestingPlatform.WebAPI.Models;

namespace TestingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            IUserService userService,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }

            var canSignIn = await _signInManager.CanSignInAsync(user);

            if (!canSignIn)
            {
                return Unauthorized();
            }

            var token = _userService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
