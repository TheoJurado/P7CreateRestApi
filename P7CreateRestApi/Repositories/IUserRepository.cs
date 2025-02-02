using Dot.Net.WebApi.Domain;
using P7CreateRestApi;

namespace P7CreateRestApi.Repositories
{
    public interface IUserRepository
    {
        User? FindByUserName(string userName);
        Task<List<User>> FindAll();
        void Add(User user);
        User? FindById(int id);
        public void Update(int id, User user);
        public void Delete(int id);
    }
}
