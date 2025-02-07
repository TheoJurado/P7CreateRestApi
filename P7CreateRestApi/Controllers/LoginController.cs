using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        //private readonly UserManager<IdentityUser> _userManager;
        private IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IJwtTokenService jwtTokenService, IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<LoginController> logger)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //TODO: implement the UserManager from Identity to validate User and return a security token.
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)//if user not found
                return Unauthorized("User not found in Identity.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);/*
            if (result.IsNotAllowed)
                return Unauthorized("User is not allowed to sign in");
            if (result.IsLockedOut)
                return Unauthorized("User is locked out");
            if (result.RequiresTwoFactor)
                return Unauthorized("Two-factor authentication is required");*/

            if (!result.Succeeded)//if password dont match
                return Unauthorized("Invalid username or password");

            // Generate the JWT Token
            var token = _jwtTokenService.GenerateJwtToken(user);
            _logger.LogInformation("L'utilisateur {User} s'est connecté", user.UserName);

            return Ok(new
            {
                Token = token,
                UserName = user.UserName
            });
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}