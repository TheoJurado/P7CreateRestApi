using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BidListController : ControllerBase
    {
        private IBidListRepository _bidRepository;

        public BidListController(IBidListRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }


        [HttpPost]
        [Route("validate")]
        public IActionResult Validate([FromBody] BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }

            _bidRepository.Add(bidList);

            var bids = _bidRepository.FindAll();

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
        public IActionResult UpdateBid(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            var bidResearch = _bidRepository.FindById(id);
            if (bidResearch == null)
                return BadRequest("L'ID est invalide.");
            if (bidList.BidListId != id)
                return BadRequest("Les informations sont invalides.");

            _bidRepository.Update(id, bidList);

            var bids = _bidRepository.FindAll();

            return Ok(bids);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteBid(int id)
        {
            var bidResearch = _bidRepository.FindById(id);
            if (bidResearch == null)
                return BadRequest("L'ID est invalide.");

            _bidRepository.Delete(id);

            var bids = _bidRepository.FindAll();

            return Ok(bids);
        }
    }
}