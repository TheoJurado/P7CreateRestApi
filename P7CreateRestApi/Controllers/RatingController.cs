using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using System.Data;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        // TODO: Inject Rating service
        private IRatingRepository _ratingRepository;

        public RatingController(IRatingRepository rateRepository)
        {
            _ratingRepository = rateRepository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all Rating, add to model
            var rates = _ratingRepository.FindAll();

            return Ok(rates);
        }

        [HttpGet]
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
                return Ok();
            }
        }

        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody]Rating rating)
        {
            // TODO: check data valid and save to db, after saving return Rating list
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }

            _ratingRepository.Add(rating);

            var rates = _ratingRepository.FindAll();

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
        public IActionResult UpdateRating(int id, [FromBody] Rating rating)
        {
            // TODO: check required fields, if valid call service to update Rating and return Rating list
            var rateResearch = _ratingRepository.FindById(id);
            if (rateResearch == null)
                return BadRequest("L'ID est invalide.");
            if (rating.Id != id)
                return BadRequest("Les informations sont invalides.");

            _ratingRepository.Update(id, rating);

            var rates = _ratingRepository.FindAll();

            return Ok(rates);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteRating(int id)
        {
            // TODO: Find Rating by Id and delete the Rating, return to Rating list
            var rateResearch = _ratingRepository.FindById(id);
            if (rateResearch == null)
                return BadRequest("L'ID est invalide.");

            _ratingRepository.Delete(id);

            var rates = _ratingRepository.FindAll();

            return Ok(rates);
        }
    }
}