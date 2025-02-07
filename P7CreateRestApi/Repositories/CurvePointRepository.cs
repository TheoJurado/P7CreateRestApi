using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Repositories;


namespace Dot.Net.WebApi.Repositories
{
    public class CurvePointRepository : ICurvePointRepository
    {
        public LocalDbContext DbContext { get; }

        public CurvePointRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public CurvePoint FindByCurveId(byte curvePointCID)
        {
            return DbContext.Curves.Where(curvePointName => curvePointName.CurveId == curvePointCID)
                                  .FirstOrDefault();
        }

        public async Task<List<CurvePoint>> FindAll()
        {
            int number = DbContext.Curves.Count();
            return await DbContext.Curves.ToListAsync();
        }

        public void Add(CurvePoint curvePoint)
        {
            DbContext.Curves.Add(curvePoint);
            DbContext.SaveChanges();
        }

        public CurvePoint FindById(int id)
        {
            CurvePoint curvePoint = DbContext.Curves.Find(id);
            if (curvePoint == null)
                return null;
            else
                return curvePoint;
        }
        public void Update(int id, CurvePoint curve)
        {
            var curveResearch = DbContext.Curves.Find(id);
            if (curveResearch == null)
                return;

            curve.Id = id; // S'assurer que l'ID reste inchangé
            var existingEntity = DbContext.RuleNames.Local.FirstOrDefault(e => e.Id == id);
            DbContext.Entry(existingEntity).State = EntityState.Detached;
            DbContext.Curves.Update(curve);

            DbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var curveResearch = DbContext.Curves.Find(id);
            if (curveResearch == null)
                return;
            DbContext.Remove(curveResearch);
            DbContext.SaveChanges();
        }
    }
}
