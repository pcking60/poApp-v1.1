using PostOffice.Common.ViewModels;
using PostOffice.Common.ViewModels.ExportModel;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface IStatisticRepository : IRepository<UnitStatisticViewModel>
    {
        IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate);

        IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate);

        IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId);

        IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId, int unitId);

        IEnumerable<ReportFunction1> RP1(string fromDate, string toDate, int districtId, int unitId);

        IEnumerable<RP1Advance> RP1Advance();

        IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time(string fromDate, string toDate, int mainGroup);

        IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_Manager(string fromDate, string toDate, int mainGroup, int poId);

        IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_User(string fromDate, string toDate, int mainGroup, string userId);

        IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_District(string fromDate, string toDate, int mainGroup, int districtId);

        IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_District_Po(string fromDate, string toDate, int mainGroup, int districtId, int poId);

        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_User_BCCP(string fromDate, string toDate, int districtId, int poId, string userId);

        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_BCCP(string fromDate, string toDate, int districtId, int poId);

        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_BCCP(string fromDate, string toDate, int districtId);

        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_BCCP(string fromDate, string toDate);

        IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_District_Po_User_TCBC(string fromDate, string toDate, int districtId, int poId, string userId);

        IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_District_Po_TCBC(string fromDate, string toDate, int districtId, int poId);

        IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_District_TCBC(string fromDate, string toDate, int districtId);

        IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_TCBC(string fromDate, string toDate);
        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_User_PPTT(string fromDate, string toDate, int districtId, int poId, string userId);

        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_PPTT(string fromDate, string toDate, int districtId, int poId);

        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_PPTT(string fromDate, string toDate, int districtId);

        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_PPTT(string fromDate, string toDate);

        IEnumerable<RP2_1> RP2_1();
    }

    public class StatisticRepository : RepositoryBase<UnitStatisticViewModel>, IStatisticRepository
    {
        public StatisticRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time(string fromDate, string toDate, int mainGroup)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@mainGroup", mainGroup)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time>("Export_By_Service_Group_And_Time @fromDate,@toDate,@mainGroup", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_BCCP(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_BCCP @fromDate,@toDate", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_District(string fromDate, string toDate, int mainGroup, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@mainGroup", mainGroup),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time>("Export_By_Service_Group_And_Time_District @fromDate,@toDate,@mainGroup,@districtId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_BCCP(string fromDate, string toDate, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_District_BCCP @fromDate,@toDate,@districtId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_District_Po(string fromDate, string toDate, int mainGroup, int districtId, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@mainGroup", mainGroup),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time>("Export_By_Service_Group_And_Time_District_Po @fromDate,@toDate,@mainGroup,@districtId,@poId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_BCCP(string fromDate, string toDate, int districtId, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_District_Po_BCCP @fromDate,@toDate,@districtId,@poId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_PPTT(string fromDate, string toDate, int districtId, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_District_Po_PPTT @fromDate,@toDate,@districtId,@poId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_District_Po_TCBC(string fromDate, string toDate, int districtId, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_TCBC>("Export_By_Service_Group_And_Time_District_Po_TCBC @fromDate,@toDate,@districtId,@poId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_User_BCCP(string fromDate, string toDate, int districtId, int poId, string userId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@userId", userId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_District_Po_User_BCCP @fromDate,@toDate,@districtId,@poId,@userId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_User_PPTT(string fromDate, string toDate, int districtId, int poId, string userId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@userId", userId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_District_Po_User_PPTT @fromDate,@toDate,@districtId,@poId,@userId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_District_Po_User_TCBC(string fromDate, string toDate, int districtId, int poId, string userId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@userId", userId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_TCBC>("Export_By_Service_Group_And_Time_District_Po_User_TCBC @fromDate,@toDate,@districtId,@poId,@userId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_PPTT(string fromDate, string toDate, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_District_PPTT @fromDate,@toDate,@districtId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_District_TCBC(string fromDate, string toDate, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_TCBC>("Export_By_Service_Group_And_Time_District_TCBC @fromDate,@toDate,@districtId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_Manager(string fromDate, string toDate, int mainGroup, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@mainGroup", mainGroup),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time>("Export_By_Service_Group_And_Time_Manager @fromDate,@toDate,@mainGroup,@poId", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_PPTT(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time_District_Po_BCCP>("Export_By_Service_Group_And_Time_PPTT @fromDate,@toDate", parameters);
        }

        public IEnumerable<Export_By_Service_Group_TCBC> Export_By_Service_Group_And_Time_TCBC(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_TCBC>("Export_By_Service_Group_And_Time_TCBC @fromDate,@toDate", parameters);
        }

        public IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time_User(string fromDate, string toDate, int mainGroup, string userId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate),
                new SqlParameter("@mainGroup", mainGroup),
                new SqlParameter("@userId", userId)
            };
            return DbContext.Database.SqlQuery<Export_By_Service_Group_And_Time>("Export_By_Service_Group_And_Time_User @fromDate,@toDate,@mainGroup,@userId", parameters);
        }

        public IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate", toDate)
            };
            return DbContext.Database.SqlQuery<UnitStatisticViewModel>("getUnitStatistic @fromDate,@toDate", parameters);
        }

        public IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate",toDate)
            };
            return DbContext.Database.SqlQuery<ReportFunction1>("reportFunction1 @fromDate,@toDate", parameters);
        }

        public IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate",toDate),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<ReportFunction1>("reportFunction1_1 @fromDate,@toDate,@districtId", parameters);
        }

        public IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId, int unitId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate",toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@unitId", unitId)
            };
            return DbContext.Database.SqlQuery<ReportFunction1>("reportFunction1_2 @fromDate,@toDate,@districtId,@unitId", parameters);
        }

        public IEnumerable<ReportFunction1> RP1(string fromDate, string toDate, int districtId, int unitId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@fromDate", fromDate),
                new SqlParameter("@toDate",toDate),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@unitId", unitId)
            };
            return DbContext.Database.SqlQuery<ReportFunction1>("RP1 @fromDate,@toDate,@districtId,@unitId", parameters);
        }

        public IEnumerable<RP1Advance> RP1Advance()
        {
            var query = ((from td in DbContext.TransactionDetails
                          join t in DbContext.Transactions
                          on td.TransactionId equals t.ID
                          join s in DbContext.Services
                          on t.ServiceId equals s.ID
                          where
                            t.ServiceId == 1556 || t.ServiceId == 1600
                          group new { s, td } by new
                          {
                              s.Name,
                              s.VAT
                          } into g
                          select g).ToList()
                        .Select(g => new RP1Advance
                        {
                            Revenue = (g.Sum(p => p.td.Money) / Convert.ToDecimal(g.Key.VAT)),
                            Tax = (g.Sum(p => p.td.Money) - g.Sum(p => p.td.Money) / Convert.ToDecimal(g.Key.VAT)),
                            TotalMoney = g.Sum(p => p.td.Money)
                        })).ToList();

            return query;
        }

        public IEnumerable<RP2_1> RP2_1()
        {
            var query = ((from td in DbContext.TransactionDetails
                          join t in DbContext.Transactions
                          on td.TransactionId equals t.ID
                          join s in DbContext.Services
                          on t.ServiceId equals s.ID
                          join sg in DbContext.ServiceGroups
                          on s.GroupID equals sg.ID
                          where
                            sg.MainServiceGroupId == 1
                          group new { s, td } by new
                          {
                              s.Name,
                              s.VAT
                          } into g
                          select g).ToList()
                        .Select(g => new RP2_1
                        {
                            //Revenue = (g.Sum(p => p.td.Money) / Convert.ToDecimal(g.Key.VAT)),
                            //Tax = (g.Sum(p => p.td.Money) - g.Sum(p => p.td.Money) / Convert.ToDecimal(g.Key.VAT)),
                            //TotalMoney = g.Sum(p => p.td.Money)
                        })).ToList();

            return query;
        }
    }
}