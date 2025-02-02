using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using Dot.Net.WebApi.Domain;

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

        public LoginController(IJwtTokenService jwtTokenService, IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //TODO: implement the UserManager from Identity to validate User and return a security token.
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var user0 = _userRepository.FindByUserName(model.Username);
            if (user0 == null)//if user not found
                return Unauthorized("Invalid username or password");

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized("User not found in Identity.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);//PasswordSignInAsync
            if (result.IsNotAllowed)
                return Unauthorized("User is not allowed to sign in");
            if (result.IsLockedOut)
                return Unauthorized("User is locked out");
            if (result.RequiresTwoFactor)
                return Unauthorized("Two-factor authentication is required");

            if (!result.Succeeded)//if password dont match
                return Unauthorized("Invalid username or password");

            // Generate the JWT Token
            var token = _jwtTokenService.GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                UserName = user.UserName
            });
        }            
    }
}