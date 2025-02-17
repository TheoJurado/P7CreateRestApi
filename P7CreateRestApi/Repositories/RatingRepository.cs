﻿using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        public LocalDbContext DbContext { get; }

        public RatingRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Rating FindByNumber(byte ratingNumber)
        {
            return DbContext.Ratings.Where(rating => rating.OrderNumber == ratingNumber)
                                  .FirstOrDefault();
        }

        public async Task<List<Rating>> FindAll()
        {
            return await DbContext.Ratings.ToListAsync();
        }

        public void Add(Rating rating)
        {
            DbContext.Ratings.Add(rating);
            DbContext.SaveChanges();
        }

        public Rating FindById(int id)
        {
            Rating rating = DbContext.Ratings.Find(id);
            if (rating == null)
                return null;
            else
                return rating;
        }

        public void Update(int id, Rating rating)
        {
            var ratingResearch = DbContext.Ratings.Find(id);
            if (ratingResearch == null)
                return;

            rating.Id = id; // S'assurer que l'ID reste inchangé
            var existingEntity = DbContext.RuleNames.Local.FirstOrDefault(e => e.Id == id);
            DbContext.Entry(existingEntity).State = EntityState.Detached;
            DbContext.Ratings.Update(rating);

            DbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var ratingResearch = DbContext.Ratings.Find(id);
            if (ratingResearch == null)
                return;
            DbContext.Remove(ratingResearch);
            DbContext.SaveChanges();
        }
    }
}
