using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories;
using System.Data;


namespace Dot.Net.WebApi.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {
        public LocalDbContext DbContext { get; }

        public RuleNameRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public RuleName FindByName(string ruleNameName)
        {
            return DbContext.RuleNames.Where(ruleName => ruleName.Name == ruleNameName)
                                  .FirstOrDefault();
        }

        public async Task<List<RuleName>> FindAll()
        {
            return await DbContext.RuleNames.ToListAsync();
        }

        public void Add(RuleName ruleName)
        {
            DbContext.RuleNames.Add(ruleName);
        }

        public RuleName FindById(int id)
        {
            RuleName ruleName = DbContext.RuleNames.Find(id);
            if (ruleName == null)
                return null;
            else
                return ruleName;
        }

        public void Update(int id, RuleName ruleName)
        {
            var ruleResearch = DbContext.RuleNames.Find(id);
            if (ruleResearch == null)
                return;
            if (ruleName.Id != id)
                return;

            ruleResearch = ruleName;
            DbContext.RuleNames.Update(ruleResearch);
        }
        public void Delete(int id)
        {
            var ruleResearch = DbContext.RuleNames.Find(id);
            if (ruleResearch == null)
                return;
            DbContext.Remove(ruleResearch);
        }
    }
}
