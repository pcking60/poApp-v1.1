using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface IInterestRateRepository : IRepository<InterestRate>
    {
        InterestRate GetByCondition(string savingTypeId, string periodId, string interestTypeId);
    }

    public class InterestRateRepository : RepositoryBase<InterestRate>, IInterestRateRepository
    {
        public InterestRateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public InterestRate GetByCondition(string savingTypeId, string periodId, string interestTypeId)
        {
            return DbContext.InterestRates.Where(x => x.InterestTypeId == interestTypeId && x.SavingTypeId == savingTypeId && x.PeriodId == periodId).SingleOrDefault();
        }
    }
}