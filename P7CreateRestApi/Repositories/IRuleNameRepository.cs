using Dot.Net.WebApi.Controllers;

namespace P7CreateRestApi.Repositories
{
    public interface IRuleNameRepository
    {
        RuleName FindByName(string ruleNameName);
        Task<List<RuleName>> FindAll();
        void Add(RuleName ruleName);
        RuleName FindById(int id);
    }
}
