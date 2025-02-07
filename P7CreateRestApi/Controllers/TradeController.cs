using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using P7CreateRestApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class TradeController : ControllerBase
    {
        private ITradeRepository _tradeRepository;
        private readonly ILogger<TradeController> _logger;

        public TradeController(ITradeRepository tradeRepository, ILogger<TradeController> logger)
        {
            _tradeRepository = tradeRepository;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,SuperRole")]
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all Trade, add to model
            var trades = await _tradeRepository.FindAll();
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a consulté la liste des trades.", userName);

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
                var userName = User.Identity?.Name ?? "Utilisateur inconnu";
                _logger.LogInformation("L'utilisateur {User} a ajouté un trade : {Trade}", userName, trade.TradeId);
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
                _logger.LogInformation("Model invalide : {trade}", trade);
                return BadRequest("Model invalide");
            }

            _tradeRepository.Add(trade);

            //log
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a validé un trade : {Trade}", userName, trade.TradeId);

            var trades = await _tradeRepository.FindAll();

            return Ok(trades);
        }

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
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a mis à jour un trade : {Trade}", userName, trade.TradeId);

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
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a supprimé un trade : {Trade}", userName, id);

            var trades = await _tradeRepository.FindAll();

            return Ok(trades);
        }
    }
}