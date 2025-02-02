using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using P7CreateRestApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            var users = await _userRepository.FindAll();

            return Ok(users);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Les informations utilisateur sont invalides.");
            }
            else
            {
                _userRepository.Add(user);
                return Ok();
            }
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody] UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }
            /*
            if (!user.Password.Contains("h"))
            {
                ModelState.AddModelError(nameof(user.Password),"pas de H");
                return BadRequest(ModelState);
            }/**/
            
            _userRepository.Add(new User
            {
                UserName = user.UserName,
                Password = user.Password,
                FullName = user.FullName,
                Role = user.Role
            });/**/
            /*_userRepository.Add(user);/**/

            var users = await _userRepository.FindAll();

            return Ok(users);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {//???
            User? user = _userRepository.FindById(id);
            
            if (user == null)
                throw new ArgumentException("Invalid user Id:" + id);

            return Ok(user);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            User? userResearch = _userRepository.FindById(id);
            if (userResearch == null)
                return BadRequest("L'ID utilisateur est invalide.");
            if (user.Id != id)
                return BadRequest("Les informations utilisateur sont invalides.");

            _userRepository.Update(id, user);

            var users = await _userRepository.FindAll();

            return Ok(users);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? userResearch = _userRepository.FindById(id);
            if (userResearch == null)
                return BadRequest("L'ID utilisateur est invalide.");

            _userRepository.Delete(id);

            var users = await _userRepository.FindAll();

            return Ok(users);
        }

        [HttpGet]
        [Route("/secure/article-details")]
        public async Task<ActionResult<List<User>>> GetAllUserArticles()
        {//???
            return Ok();
        }
    }
}