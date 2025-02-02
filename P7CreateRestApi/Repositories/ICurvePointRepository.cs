using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories
{
    public interface ICurvePointRepository
    {
        CurvePoint FindByCurveId(byte curvePointCID);
        Task<List<CurvePoint>> FindAll();
        void Add(CurvePoint rating);
        CurvePoint FindById(int id);
        public void Update(int id, CurvePoint curve);
        public void Delete(int id);
    }
}
