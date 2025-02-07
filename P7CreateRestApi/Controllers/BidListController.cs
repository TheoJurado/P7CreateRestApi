using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class BidListController : ControllerBase
    {
        private IBidListRepository _bidRepository;
        private readonly ILogger<BidListController> _logger;

        public BidListController(IBidListRepository bidRepository, ILogger<BidListController> logger)
        {
            _bidRepository = bidRepository;
            _logger = logger;
        }


        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody] BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }

            _bidRepository.Add(bidList);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a validé une offre : {Bid}", userName, bidList.BidListId);

            var bids = await _bidRepository.FindAll();

            return Ok(bids);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            var bid = _bidRepository.FindById(id);
            if (bid == null)
                return BadRequest("Invalid Id:" + id);

            return Ok(bid);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            var bidResearch = _bidRepository.FindById(id);
            if (bidResearch == null)
                return BadRequest("L'ID est invalide.");
            if (bidList.BidListId != id)
                return BadRequest("Les informations sont invalides.");

            _bidRepository.Update(id, bidList);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a mis à jour une offre : {Bid}", userName, bidList.BidListId);

            var bids = await _bidRepository.FindAll();

            return Ok(bids);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            var bidResearch = _bidRepository.FindById(id);
            if (bidResearch == null)
                return BadRequest("L'ID est invalide.");

            _bidRepository.Delete(id);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a supprimé une offre : {Bid}", userName, id);

            var bids = await _bidRepository.FindAll();

            return Ok(bids);
        }
    }
}