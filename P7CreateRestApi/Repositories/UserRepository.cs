using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        public LocalDbContext DbContext { get; }

        public UserRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public User? FindByUserName(string userName)
        {
            return DbContext.Users.Where(user => user.UserName == userName)
                                  .FirstOrDefault();
        }

        public async Task<List<User>> FindAll()
        {
            return await DbContext.Users.ToListAsync();
        }

        public void Add(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        public User? FindById(int id)
        {
            User? user = DbContext.Users.Find(id);
            if (user == null)
                return null;
            else
                return user;
        }

        public void Update(int id, User user)
        {
            User? userResearch = DbContext.Users.Find(id);
            if( userResearch == null)
                return;
            if (user.Id != id)
                return;

            userResearch = user;
            DbContext.Users.Update(userResearch);
            DbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            User? userResearch = DbContext.Users.Find(id);
            if(userResearch == null)
                return;
            DbContext.Remove(userResearch);
            DbContext.SaveChanges();
        }
    }
}