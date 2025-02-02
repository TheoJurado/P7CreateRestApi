using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories;
using System.Data;
using System.Reflection;


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
            DbContext.SaveChanges();
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
            
            ruleResearch.Name = ruleName.Name;
            ruleResearch.Description = ruleName.Description;
            ruleResearch.Json = ruleName.Json;
            ruleResearch.Template = ruleName.Template;
            ruleResearch.SqlStr = ruleName.SqlStr;
            ruleResearch.SqlPart = ruleName.SqlPart;

            DbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var ruleResearch = DbContext.RuleNames.Find(id);
            if (ruleResearch == null)
                return;
            DbContext.Remove(ruleResearch);
            DbContext.SaveChanges();
        }
    }
}
