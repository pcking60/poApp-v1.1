using PostOffice.Common.ViewModels.ExportModel;
using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PostOfiice.DAta.Repositories
{
    public interface ITKBDRepository : IRepository<TKBDAmount>
    {
        IEnumerable<TKBD_Export_Template> Export_By_Time(int month, int year);
        IEnumerable<TKBD_Export_Template> Export_By_Time_User(int month, int year, string currentUserId);
        IEnumerable<TKBD_Export_Template> Export_By_Time_District(int month, int year, int districtId);
        IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po(int month, int year, int districtId, int poId);
        IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po_User(int month, int year, int districtId, int poId, string userId);

        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time(int month, int year);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_User(int month, int year, string currentUserId);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District(int month, int year, int districtId);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po(int month, int year, int districtId, int poId);
        IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po_User(int month, int year, int districtId, int poId, string userId);
    }

    public class TKBDRepository : RepositoryBase<TKBDAmount>, ITKBDRepository
    {
        public TKBDRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override TKBDAmount Add(TKBDAmount entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time(int month, int year)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time @month,@year", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_District(int month, int year, int districtId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_District @month,@year,@districtId", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po(int month, int year, int districtId, int poId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_District_PO @month,@year,@districtId,@poId", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_District_Po_User(int month, int year, int districtId, int poId, string userId)
        {
            var parameters = new SqlParameter[] {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@userId", userId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_District_PO_User @month,@year,@districtId,@poId,@userId", parameters);
        }

        public IEnumerable<TKBD_Export_Template> Export_By_Time_User(int month, int year, string currentUserId)
        {
            var parameters = new SqlParameter[]
             {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@currentUserId", currentUserId)
             };
            return DbContext.Database.SqlQuery<TKBD_Export_Template>("Export_TKBD_By_Time_User @month,@year,@currentUserId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time(int month, int year)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time @month,@year", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District(int month, int year, int districtId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@districtId", districtId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_District @month,@year,@districtId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po(int month, int year, int districtId, int poId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_District_Po @month,@year,@districtId,@poId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_District_Po_User(int month, int year, int districtId, int poId, string userId)
        {
            var parameters = new SqlParameter[]
           {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@districtId", districtId),
                new SqlParameter("@poId", poId),
                new SqlParameter("@userId", userId)
           };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_District_Po_User @month,@year,@districtId,@poId,@userId", parameters);
        }

        public IEnumerable<TKBD_Export_Detail_Template> Export_TKBD_Detail_By_Time_User(int month, int year, string currentUserId)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@month", month),
                new SqlParameter("@year", year),
                new SqlParameter("@currentUserId", currentUserId)
            };
            return DbContext.Database.SqlQuery<TKBD_Export_Detail_Template>("Export_TKBD_Detail_By_Time_User @month,@year,@currentUserId", parameters);
        }

        public override void Update(TKBDAmount entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}