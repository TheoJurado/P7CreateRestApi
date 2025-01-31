using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private ITradeRepository _tradeRepository;

        public TradeController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all Trade, add to model
            var trades = await _tradeRepository.FindAll();

            return Ok(trades);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddTrade([FromBody]Trade trade)
        {
            if (trade == null)
            {
                return BadRequest("Les informations sont invalides.");
            }
            else
            {
                _tradeRepository.Add(trade);
                return Ok();
            }
        }
        
        [Authorize(Roles = "Admin,SuperRole")]
        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody]Trade trade)
        {
            // TODO: check data valid and save to db, after saving return Trade list
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }

            _tradeRepository.Add(trade);

            var trades = await _tradeRepository.FindAll();

            return Ok(trades);
        }/**/
        /*
        [HttpPost("validate")]
        [AllowAnonymous]
        public async Task<IActionResult> Validate([FromBody] Trade trade)
        {
            var user = User.Identity;
            var isAuthenticated = user.IsAuthenticated;
            var userName = user.Name ?? "Non authentifié";

            return Ok(new
            {
                IsAuthenticated = isAuthenticated,
                UserName = userName,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }/**/

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            var trade = _tradeRepository.FindById(id);
            if (trade == null)
                return BadRequest("Invalid Id:" + id);

            return Ok(trade);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTrade(int id, [FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            var tradeResearch = _tradeRepository.FindById(id);
            if (tradeResearch == null)
                return BadRequest("L'ID est invalide.");
            if (trade.TradeId != id)
                return BadRequest("Les informations sont invalides.");

            _tradeRepository.Update(id, trade);

            var trades = await _tradeRepository.FindAll();

            return Ok(trades);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            // TODO: Find Trade by Id and delete the Trade, return to Trade list
            var tradeResearch = _tradeRepository.FindById(id);
            if (tradeResearch == null)
                return BadRequest("L'ID est invalide.");

            _tradeRepository.Delete(id);

            var trades = await _tradeRepository.FindAll();

            return Ok(trades);
        }
    }
}