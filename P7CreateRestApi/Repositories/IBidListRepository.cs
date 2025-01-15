using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories
{
    public interface IBidListRepository
    {
        BidList FindByName(string BidListName);
        Task<List<BidList>> FindAll();
        void Add(BidList bidList);
        BidList FindById(int id);
    }
}
