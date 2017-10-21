using PostOffice.Common.ViewModels;
using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetAllByUserName(string userName);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate);
        IEnumerable<Transaction> Get_by_time_districtId(DateTime fromDate, DateTime toDate, int districtId);
        IEnumerable<Transaction> Get_by_time_districtId_poId(DateTime fromDate, DateTime toDate, int districtId, int poId);
        IEnumerable<Transaction> Get_by_time_districtId_poId_userId(DateTime fromDate, DateTime toDate, int districtId, int poId, string userId);
        IEnumerable<Transaction> Get_by_time_districtId_poId_userId_serviceId(DateTime fromDate, DateTime toDate, int districtId, int poId, string userId, int serviceId);
        IEnumerable<Transaction> GetAllByMainGroupId(DateTime fromDate, DateTime toDate, int mainGroupId);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId, int serviceId);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, int serviceId);

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id);

        IEnumerable<Transaction> GetAllBy_Time_DistrictID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int id);
        IEnumerable<Transaction> GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int poId, int id);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId, int serviceId);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId);

        IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, int serviceId);

        IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string Username, int serviceId);

        IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string Username);

        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
    }

    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override Transaction Add(Transaction entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public IEnumerable<Transaction> GetAllByUserName(string userName)
        {
            var date = DateTime.Now;
            var pos = from po in this.DbContext.PostOffices
                      join u in this.DbContext.Users
                      on po.ID equals u.POID
                      where u.UserName == userName
                      select po.ID;

            int p = pos.First();
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.UserName == userName && ts.Status == true && ts.TransactionDate == date
                                  select ts;

            return listTransaction;
        }

        public override void Update(Transaction entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("getRevenueStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string userName)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where u.UserName == userName && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.Status==true
                        select ts;

            var q = DbContext.Transactions.Where(x => DbFunctions.TruncateTime(x.TransactionDate) >= fromDate && DbFunctions.TruncateTime(x.TransactionDate) <= toDate && x.Status == true && x.UserId == userName).ToList();
            return query;
        }

        public IEnumerable<Transaction> GetAllByTimeAndUsername(DateTime fromDate, DateTime toDate, string Username, int serviceId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where u.UserName == Username && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId && ts.Status == true
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= (fromDate) && DbFunctions.TruncateTime(ts.TransactionDate) <= (toDate) && ts.Status == true)
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId, int serviceId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId && u.Id == userId && ts.Status == true
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, int serviceId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId && ts.Status == true
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate, string userId)
        {
            var query = from u in DbContext.Users
                        join ts in DbContext.Transactions
                        on u.Id equals ts.UserId
                        where (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && u.Id == userId && ts.Status == true
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.Status == true
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId, int serviceId)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && u.Id == userId && ts.ServiceId == serviceId && ts.Status == true
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, string userId)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && u.Id == userId && ts.Status == true
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllByTimeAndPOID(DateTime fromDate, DateTime toDate, int id, int serviceId)
        {
            var listTransaction = from u in this.DbContext.Users
                                  join ts in this.DbContext.Transactions
                                  on u.Id equals ts.UserId
                                  where u.POID == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.ServiceId == serviceId && ts.Status == true
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllByMainGroupId(DateTime fromDate, DateTime toDate, int mainGroupId)
        {
            var listTransaction = from g in DbContext.ServiceGroups
                                  join mg in DbContext.MainServiceGroups                                   
                                  on g.MainServiceGroupId equals mg.Id
                                  join s in DbContext.Services
                                  on g.ID equals s.GroupID
                                  join ts in DbContext.Transactions
                                  on s.ID equals ts.ServiceId
                                  where mg.Id == mainGroupId && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate) && ts.Status == true
                                  select ts;

            return listTransaction;
        }

        public IEnumerable<Transaction> GetAllBy_Time_DistrictID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int id)
        {
            var query = from g in DbContext.ServiceGroups
                        join mg in DbContext.MainServiceGroups
                        on g.MainServiceGroupId equals mg.Id
                        join s in DbContext.Services
                        on g.ID equals s.GroupID
                        join ts in DbContext.Transactions
                        on s.ID equals ts.ServiceId
                        join u in DbContext.Users
                        on ts.UserId equals u.Id
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        where  p.DistrictID == districtId && ts.Status == true && mg.Id==id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate)
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int poId, int id)
        {
            var query = from g in DbContext.ServiceGroups
                        join mg in DbContext.MainServiceGroups
                        on g.MainServiceGroupId equals mg.Id
                        join s in DbContext.Services
                        on g.ID equals s.GroupID
                        join ts in DbContext.Transactions
                        on s.ID equals ts.ServiceId
                        join u in DbContext.Users
                        on ts.UserId equals u.Id
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        where p.DistrictID == districtId && ts.Status == true && p.ID==poId && mg.Id == id && (DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate)
                        select ts;
            return query;
        }

        public IEnumerable<Transaction> Get_by_time_districtId(DateTime fromDate, DateTime toDate, int districtId)
        {
            var query = from ts in DbContext.Transactions
                        join u in DbContext.Users
                        on ts.UserId equals u.Id
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        join d in DbContext.Districts
                        on p.DistrictID equals d.ID
                        where DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate && d.ID == districtId && ts.Status == true
                        select ts;
            return query.OrderBy(x => x.ID).ToList();
        }
        public IEnumerable<Transaction> Get_by_time_districtId_poId(DateTime fromDate, DateTime toDate, int districtId, int poId)
        {
            var query = from ts in DbContext.Transactions
                        join u in DbContext.Users
                        on ts.UserId equals u.Id
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        join d in DbContext.Districts
                        on p.DistrictID equals d.ID
                        where DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate && d.ID == districtId && p.ID == poId && ts.Status == true
                        select ts;
            return query.OrderBy(x => x.ID).ToList();
        }

        public IEnumerable<Transaction> Get_by_time_districtId_poId_userId(DateTime fromDate, DateTime toDate, int districtId, int poId, string userId)
        {
            var query = from ts in DbContext.Transactions
                        join u in DbContext.Users
                        on ts.UserId equals u.Id
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        join d in DbContext.Districts
                        on p.DistrictID equals d.ID
                        where DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate && d.ID == districtId && p.ID == poId && u.Id==userId && ts.Status == true
                        select ts;
            return query.OrderBy(x => x.ID).ToList();
        }

        public IEnumerable<Transaction> Get_by_time_districtId_poId_userId_serviceId(DateTime fromDate, DateTime toDate, int districtId, int poId, string userId, int serviceId)
        {
            var query = from ts in DbContext.Transactions
                        join u in DbContext.Users
                        on ts.UserId equals u.Id
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        join d in DbContext.Districts
                        on p.DistrictID equals d.ID
                        where DbFunctions.TruncateTime(ts.TransactionDate) >= fromDate 
                                && DbFunctions.TruncateTime(ts.TransactionDate) <= toDate 
                                && d.ID == districtId 
                                && p.ID == poId 
                                && u.Id == userId 
                                && ts.ServiceId==serviceId 
                                && ts.Status == true
                        select ts;
            return query.OrderBy(x => x.ID).ToList();
        }
    }
}