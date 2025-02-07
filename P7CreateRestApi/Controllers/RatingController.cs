using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using System.Data;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        // TODO: Inject Rating service
        private IRatingRepository _ratingRepository;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IRatingRepository rateRepository, ILogger<RatingController> logger)
        {
            _ratingRepository = rateRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all Rating, add to model
            var rates = await _ratingRepository.FindAll();
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a consulté la liste des notations.", userName);

            return Ok(rates);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddRatingForm([FromBody]Rating rating)
        {
            if (rating == null)
            {
                return BadRequest("Les informations sont invalides.");
            }
            else
            {
                _ratingRepository.Add(rating);
                var userName = User.Identity?.Name ?? "Utilisateur inconnu";
                _logger.LogInformation("L'utilisateur {User} a ajouté une notation : {Rating}", userName, rating.Id);
                return Ok();
            }
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody]Rating rating)
        {
            // TODO: check data valid and save to db, after saving return Rating list
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }

            _ratingRepository.Add(rating);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a validé une notation : {Rating}", userName, rating.Id);

            var rates = await _ratingRepository.FindAll();

            return Ok(rates);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            var rate = _ratingRepository.FindById(id);
            if (rate == null)
                return BadRequest("Invalid Id:" + id);

            return Ok(rate);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            var rateResearch = _ratingRepository.FindById(id);
            if (rateResearch == null)
                return BadRequest("L'ID est invalide.");
            if (rating.Id != id)
                return BadRequest("Les informations sont invalides.");

            _ratingRepository.Update(id, rating);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a mis à jour une notation : {Rating}", userName, rating.Id);

            var rates = await _ratingRepository.FindAll();

            return Ok(rates);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list
            var rateResearch = _ratingRepository.FindById(id);
            if (rateResearch == null)
                return BadRequest("L'ID est invalide.");

            _ratingRepository.Delete(id);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a supprimé une notation : {Rating}", userName, id);

            var rates = await _ratingRepository.FindAll();

            return Ok(rates);
        }
    }
}