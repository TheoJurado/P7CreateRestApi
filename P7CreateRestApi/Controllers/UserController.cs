using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using P7CreateRestApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            //log
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a consulté la liste des utilisateurs", userName);
            var users = await _userRepository.FindAll();

            return Ok(users);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddUser([FromBody] User user)
        {
            //log
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                _logger.LogInformation("L'utilisateur {User} a échoué a ajouter un utilisateur", userName);
                return BadRequest("Les informations utilisateur sont invalides.");
            }
            else
            {
                _userRepository.Add(user);
                _logger.LogInformation("L'utilisateur {User} a ajouté un utilisateur : {NewUser}", userName, user.Id);
                return Ok();
            }
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody] UserViewModel user)
        {
            //log
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("L'utilisateur {User} a échoué a valider un utilisateur", userName);
                return BadRequest("Model invalide");
            }
            
            _userRepository.Add(new User
            {
                UserName = user.UserName,
                Password = user.Password,
                FullName = user.FullName,
                Role = user.Role
            });
            _logger.LogInformation("L'utilisateur {User} a validé un utilisateur : {NewUser}", userName, user.FullName);

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
            //log
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";

            User? userResearch = _userRepository.FindById(id);
            if (userResearch == null)
                return BadRequest("L'ID utilisateur est invalide.");
            if (user.Id != id)
                return BadRequest("Les informations utilisateur sont invalides.");

            _userRepository.Update(id, user);
            _logger.LogInformation("L'utilisateur {User} a mis à jour un utilisateur : {NewUser}", userName, user.Id);
            var users = await _userRepository.FindAll();

            return Ok(users);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            //log
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";

            User? userResearch = _userRepository.FindById(id);
            if (userResearch == null)
                return BadRequest("L'ID utilisateur est invalide.");

            _userRepository.Delete(id);
            _logger.LogInformation("L'utilisateur {User} a supprimé un utilisateur : {OldUser}", userName, id);

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