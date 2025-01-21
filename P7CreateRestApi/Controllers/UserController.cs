using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Home()
        {
            var users = _userRepository.FindAll();

            return Ok();
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

        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody]User user)
        {//??? comme trade ?
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           
            //_userRepository.Add(user);



            return Ok();
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {//???
            User user = _userRepository.FindById(id);
            
            if (user == null)
                throw new ArgumentException("Invalid user Id:" + id);

            return Ok();
        }

        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            User? userResearch = _userRepository.FindById(id);
            if (userResearch == null)
                return BadRequest("L'ID utilisateur est invalide.");
            if (user.Id != id)
                return BadRequest("Les informations utilisateur sont invalides.");

            _userRepository.Update(id, user);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser(int id)
        {
            User? userResearch = _userRepository.FindById(id);
            if (userResearch == null)
                return BadRequest("L'ID utilisateur est invalide.");

            _userRepository.Delete(id);

            return Ok();
        }

        [HttpGet]
        [Route("/secure/article-details")]
        public async Task<ActionResult<List<User>>> GetAllUserArticles()
        {//???
            return Ok();
        }
    }
}