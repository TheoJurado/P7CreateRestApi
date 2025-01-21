using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradeController : ControllerBase
    {
        private ITradeRepository _tradeRepository;

        public TradeController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult Home()
        {
            // TODO: find all Trade, add to model
            var trades = _tradeRepository.FindAll();

            return Ok(trades);
        }

        [HttpGet]
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

        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody]Trade trade)
        {//???
            // TODO: check data valid and save to db, after saving return Trade list
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _tradeRepository.Add(trade);

            var trades = _tradeRepository.FindAll();

            return Ok(trades);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            return Ok();
        }

        [HttpPost]
        [Route("update/{id}")]
        public IActionResult UpdateTrade(int id, [FromBody] Trade trade)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTrade(int id)
        {
            // TODO: Find Trade by Id and delete the Trade, return to Trade list
            return Ok();
        }
    }
}