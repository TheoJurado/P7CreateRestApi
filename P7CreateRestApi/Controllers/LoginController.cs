using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        //private readonly UserManager<IdentityUser> _userManager;
        private IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(IJwtTokenService jwtTokenService, IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            //_userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //TODO: implement the UserManager from Identity to validate User and return a security token.
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            string wrongLogin = "Invalid username or password";
            var user = _userRepository.FindByUserName(model.Username);
            if (user == null)//if user not found
                return Unauthorized(wrongLogin);

            IdentityUser temporaryUser = new IdentityUser { PasswordHash = user.PasswordHash, };
            var result = await _userManager.CheckPasswordAsync(temporaryUser, model.Password);
            if (!result)//if password dont match
                return Unauthorized(wrongLogin);

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