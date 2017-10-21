using PostOffice.Common.ViewModels.StatisticModel;
using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface ITKBDHistoryRepository : IRepository<TKBDHistory>
    {
        IEnumerable<TKBDHistory> GetAllByUserName(string userName);

        IEnumerable<TKBD_History_Statistic> Get_By_Time(string fromDate, string toDate);

        IEnumerable<TKBD_History_Statistic> Get_By_Time_User(string fromDate, string toDate, string currentUserId);

        IEnumerable<TKBD_History_Statistic> Get_By_Time_District(string fromDate, string toDate, int districtId);

        IEnumerable<TKBD_History_Statistic> Get_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId);

        IEnumerable<TKBD_History_Statistic> Get_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string selectedUser);
    }

    public class TKBDHistoryRepository : RepositoryBase<TKBDHistory>, ITKBDHistoryRepository
    {
        public TKBDHistoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<TKBD_History_Statistic> Get_By_Time(string fromDate, string toDate)
        {
            
            var parameters1 = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<TKBD_History_Statistic>("Get_TKBD_By_Time @fromDate,@toDate", parameters1);
        }

        public IEnumerable<TKBD_History_Statistic> Get_By_Time_District(string fromDate, string toDate, int districtId)
        {
            var parameters2 = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<TKBD_History_Statistic>("Get_TKBD_By_Time_District @fromDate,@toDate,@districtId", parameters2);
        }

        public IEnumerable<TKBD_History_Statistic> Get_By_Time_District_Po(string fromDate, string toDate, int districtId, int poId)
        {
            var parameters3 = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<TKBD_History_Statistic>("Get_TKBD_By_Time_District_Po @fromDate,@toDate,@districtId,@poId", parameters3);
        }

        public IEnumerable<TKBD_History_Statistic> Get_By_Time_District_Po_User(string fromDate, string toDate, int districtId, int poId, string selectedUser)
        {
            var parameters4 = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@selectedUser", selectedUser)
            };
            return DbContext.Database.SqlQuery<TKBD_History_Statistic>("Get_TKBD_By_Time_District_Po_User @fromDate,@toDate,@districtId,@poId,@selectedUser", parameters4);
        }

        public IEnumerable<TKBDHistory> GetAllByUserName(string userName)
        {
            var pos = from po in DbContext.PostOffices
                      join u in DbContext.Users
                      on po.ID equals u.POID
                      where u.UserName == userName
                      select po.ID;
            int p = pos.First();

            var listTKBDHistories = (from po in DbContext.PostOffices
                                     join u in DbContext.Users
                                     on po.ID equals u.POID
                                     join h in DbContext.TKBDHistories
                                     on u.Id equals h.UserId
                                     where po.ID == p
                                     select h).AsEnumerable();

            return listTKBDHistories;
        }

        public IEnumerable<TKBD_History_Statistic> Get_By_Time_User(string fromDate, string toDate, string currentUserId)
        {
            var parameters1 = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@currentUserId", currentUserId)
            };
            return DbContext.Database.SqlQuery<TKBD_History_Statistic>("Get_TKBD_By_Time_User @fromDate,@toDate,@currentUserId", parameters1);
        }
    }
}