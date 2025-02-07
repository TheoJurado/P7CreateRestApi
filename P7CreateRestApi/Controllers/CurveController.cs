using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class CurveController : ControllerBase
    {
        // TODO: Inject Curve Point service
        private ICurvePointRepository _curveRepository;
        private readonly ILogger<TradeController> _logger;

        public CurveController(ICurvePointRepository curveRepository, ILogger<TradeController> logger)
        {
            _curveRepository = curveRepository;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,SuperRole")]
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            var curves = await _curveRepository.FindAll();
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a consulté la liste des courbes.", userName);

            return Ok(curves);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddCurvePoint([FromBody]CurvePoint curvePoint)
        {
            if (curvePoint == null)
            {
                return BadRequest("Les informations sont invalides.");
            }
            else
            {
                _curveRepository.Add(curvePoint);
                var userName = User.Identity?.Name ?? "Utilisateur inconnu";
                _logger.LogInformation("L'utilisateur {User} a ajouté une courbe : {Curve}", userName, curvePoint.Id);
                return Ok();
            }
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody]CurvePoint curvePoint)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }

            _curveRepository.Add(curvePoint);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a validé une courbe : {Curve}", userName, curvePoint.Id);

            var curves = await _curveRepository.FindAll();

            return Ok(curves);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            var curve = _curveRepository.FindById(id);
            if (curve == null)
                return BadRequest("Invalid Id:" + id);

            return Ok(curve);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            var curveResearch = _curveRepository.FindById(id);
            if (curveResearch == null)
                return BadRequest("L'ID est invalide.");
            if (curvePoint.Id != id)
                return BadRequest("Les informations sont invalides.");

            _curveRepository.Update(id, curvePoint);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a mis à jour une courbe : {Curve}", userName, curvePoint.Id);

            var curves = await _curveRepository.FindAll();

            return Ok(curves);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            // TODO: Find Curve by Id and delete the Curve, return to Curve list
            var curveResearch = _curveRepository.FindById(id);
            if (curveResearch == null)
                return BadRequest("L'ID est invalide.");

            _curveRepository.Delete(id);
            var userName = User.Identity?.Name ?? "Utilisateur inconnu";
            _logger.LogInformation("L'utilisateur {User} a supprimé une courbe : {Curve}", userName, id);

            var curves = await _curveRepository.FindAll();

            return Ok(curves);
        }
    }
}