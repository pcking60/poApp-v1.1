using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;

namespace PostOffice.Service
{
    public interface IInterestRateService
    {
        InterestRate GetByCondition(string savingTypeId, string periodId, string interestTypeId);
    }

    public class InterestRateService : IInterestRateService
    {
        public IInterestRateRepository _interestRateRepository;
        public IUnitOfWork _unitOfWork;

        public InterestRateService(IInterestRateRepository interestRateRepository)
        {
            _interestRateRepository = interestRateRepository;
        }
        public InterestRate GetByCondition(string savingTypeId, string periodId, string interestTypeId)
        {
            return _interestRateRepository.GetByCondition(savingTypeId, periodId, interestTypeId);
        }
    }
}