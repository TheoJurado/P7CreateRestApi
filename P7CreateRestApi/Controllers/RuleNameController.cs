using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using System.Diagnostics;

namespace Dot.Net.WebApi.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class RuleNameController : ControllerBase
    {
        private IRuleNameRepository _ruleNameRepository;

        public RuleNameController(IRuleNameRepository ruleRepository)
        {
            _ruleNameRepository = ruleRepository;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Home()
        {
            // TODO: find all RuleName, add to model
            var rules = await _ruleNameRepository.FindAll();

            return Ok(rules);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddRuleName([FromBody]RuleName rule)
        {
            if (rule == null)
            {
                return BadRequest("Les informations sont invalides.");
            }
            else
            {
                _ruleNameRepository.Add(rule);
                return Ok();
            }
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate([FromBody]RuleName rule)
        {
            // TODO: check data valid and save to db, after saving return RuleName list
            if (!ModelState.IsValid)
            {
                return BadRequest("Model invalide");
            }

            _ruleNameRepository.Add(rule);

            var rules = await _ruleNameRepository.FindAll();

            return Ok(rules);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            var rule = _ruleNameRepository.FindById(id);
            if (rule == null)
                return BadRequest("Invalid Id:" + id);

            return Ok(rule);
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] RuleName rule)
        {
            // TODO: check required fields, if valid call service to update RuleName and return RuleName list
            var ruleResearch = _ruleNameRepository.FindById(id);
            if (ruleResearch == null)
                return BadRequest("L'ID est invalide.");
            if (rule.Id != id)
                return BadRequest("Les informations sont invalides.");

            _ruleNameRepository.Update(id, rule);

            var rules = await _ruleNameRepository.FindAll();

            return Ok(rules);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            // TODO: Find RuleName by Id and delete the RuleName, return to Rule list
            var ruleResearch = _ruleNameRepository.FindById(id);
            if (ruleResearch == null)
                return BadRequest("L'ID est invalide.");

            _ruleNameRepository.Delete(id);

            var rules = await _ruleNameRepository.FindAll();

            return Ok(rules);
        }
    }
}