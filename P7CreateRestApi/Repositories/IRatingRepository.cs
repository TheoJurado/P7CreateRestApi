using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;

namespace P7CreateRestApi.Repositories
{
    public interface IRatingRepository
    {
        Rating FindByNumber(byte ratingNumber);
        Task<List<Rating>> FindAll();
        void Add(Rating rating);
        Rating FindById(int id);
        public void Update(int id, Rating rating);
        public void Delete(int id);
    }
}
