using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories
{
    public interface ITradeRepository
    {
        Trade FindByName(string tradeName);
        Task<List<Trade>> FindAll();
        void Add(Trade trade);
        Trade FindById(int id);
    }
}
