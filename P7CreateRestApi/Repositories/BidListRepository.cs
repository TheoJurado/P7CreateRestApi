﻿using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class BidListRepository : IBidListRepository
    {
        public LocalDbContext DbContext { get; }

        public BidListRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public BidList FindByName(string BidListName)
        {
            return DbContext.Bids.Where(BidList => BidList.CreationName == BidListName)
                                  .FirstOrDefault();
        }

        public async Task<List<BidList>> FindAll()
        {
            return await DbContext.Bids.ToListAsync();
        }

        public void Add(BidList bidList)
        {
            DbContext.Bids.Add(bidList);
        }

        public BidList FindById(int id)
        {
            BidList bidList = DbContext.Bids.Find(id);
            if (bidList == null)
                return null;
            else
                return bidList;
        }
    }
}
