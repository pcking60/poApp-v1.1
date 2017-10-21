using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionDetailRepository : IRepository<TransactionDetail>
    {
        IEnumerable<TransactionDetail> GetAllByCondition(string condition);

        IEnumerable<TransactionDetail> GetAllByTransactionId(int transactionId);

        TransactionDetail GetAllByCondition(string condition, int transactionId);
        decimal? GetTotalMoneyByTransactionId(int id);
    }

    public class TransactionDetailRepository : RepositoryBase<TransactionDetail>, ITransactionDetailRepository
    {
        public TransactionDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<TransactionDetail> GetAllByCondition(string condition)
        {
            condition = "Sản lượng";
            var query = from ps in DbContext.PropertyServices
                        join td in DbContext.TransactionDetails
                        on ps.ID equals td.PropertyServiceId
                        where ps.Name != condition
                        select td;
            return query;
        }

        public TransactionDetail GetAllByCondition(string condition, int transactionId)
        {
            var query = from ps in DbContext.PropertyServices
                        join td in DbContext.TransactionDetails
                        on ps.ID equals td.PropertyServiceId
                        where ps.Name == condition && td.TransactionId == transactionId
                        select td;
            return query.FirstOrDefault();
        }

        public IEnumerable<TransactionDetail> GetAllByTransactionId(int transactionId)
        {
            var query = DbContext.TransactionDetails.Where(x => x.TransactionId == transactionId).ToList();
            return query;
        }

        public decimal? GetTotalMoneyByTransactionId(int id)
        {
            var condition = "Sản lượng";
            var listTransactionDetails = from ps in DbContext.PropertyServices
                                         join td in DbContext.TransactionDetails
                                         on ps.ID equals td.PropertyServiceId                                      
                                         where ps.Name != condition && td.TransactionId == id
                                         select td;
            decimal? sum = 0;
            foreach (var item in listTransactionDetails)
            {
                sum += DbContext.TransactionDetails.Where(x=>x.ID == item.ID).Sum(x => x.Money);
            }
            return sum;
        }
    }
}