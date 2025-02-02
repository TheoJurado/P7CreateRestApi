using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories;

namespace Dot.Net.WebApi.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        public LocalDbContext DbContext { get; }

        public TradeRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Trade FindByName(string tradeName)
        {
            return DbContext.Trades.Where(trade => trade.DealName == tradeName)
                                  .FirstOrDefault();
        }

        public async Task<List<Trade>> FindAll()
        {
            return await DbContext.Trades.ToListAsync();
        }

        public void Add(Trade trade)
        {
            DbContext.Trades.Add(trade);
            DbContext.SaveChanges();
        }

        public Trade FindById(int id)
        {
            Trade trade = DbContext.Trades.Find(id);
            if (trade == null)
                return null;
            else
                return trade;
        }

        public void Update(int id, Trade trade)
        {
            Trade? tradeResearch = DbContext.Trades.Find(id);
            if (tradeResearch == null)
                return;

            trade.TradeId = id; // S'assurer que l'ID reste inchangé
            var existingEntity = DbContext.RuleNames.Local.FirstOrDefault(e => e.Id == id);
            DbContext.Entry(existingEntity).State = EntityState.Detached;
            DbContext.Trades.Update(trade);

            DbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            Trade? tradeResearch = DbContext.Trades.Find(id);
            if (tradeResearch == null)
                return;
            DbContext.Remove(tradeResearch);
            DbContext.SaveChanges();
        }
    }
}
