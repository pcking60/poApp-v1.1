using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface IDistrictRepository : IRepository<District>
    {
        District GetDistrictByUserId(string userId);
        District GetDistrictByUserName(string userName);
    }

    public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override District Add(District entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override District Delete(District entity)
        {
            return base.Delete(entity);
        }

        public District GetDistrictByUserId(string userId)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        join d in DbContext.Districts
                        on p.DistrictID equals d.ID
                        where u.Id == userId
                        select d;
            return query.FirstOrDefault();
        }

        public District GetDistrictByUserName(string userName)
        {
            var query = from u in DbContext.Users
                        join p in DbContext.PostOffices
                        on u.POID equals p.ID
                        join d in DbContext.Districts
                        on p.DistrictID equals d.ID
                        where u.UserName == userName
                        select d;
            return query.FirstOrDefault();
        }

        public override void Update(District entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}