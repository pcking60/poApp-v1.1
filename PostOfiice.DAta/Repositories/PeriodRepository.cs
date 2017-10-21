using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;

namespace PostOfiice.DAta.Repositories
{
    public interface IPeriodRepository : IRepository<Period>
    {
    }

    public class PeriodRepository : RepositoryBase<Period>, IPeriodRepository
    {
        public PeriodRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}