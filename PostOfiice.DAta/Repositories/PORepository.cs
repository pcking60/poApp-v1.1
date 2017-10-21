using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PostOfiice.DAta.Repositories
{
    public interface IPORepository : IRepository<PO>
    {
        IEnumerable<PO> GetAllPOByDistrictId(int districtId);
        PO GetPOByCurrentUser(string userName);
    }

    public class PORepository : RepositoryBase<PO>, IPORepository
    {
        public PORepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override PO Add(PO entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override void Update(PO entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }

        public override PO Delete(int id)
        {
            return base.Delete(id);
        }

        public override IEnumerable<PO> GetMulti(Expression<Func<PO, bool>> predicate, string[] includes = null)
        {
            return base.GetMulti(predicate, includes);
        }

        public IEnumerable<PO> GetAllPOByDistrictId(int districtId)
        {
            var po = from p in this.DbContext.PostOffices
                     join d in this.DbContext.Districts
                     on p.DistrictID equals d.ID
                     where d.ID == districtId
                     select p;
            return po;
        }

        public PO GetPOByCurrentUser(string userName)
        {
            var po = from p in DbContext.PostOffices
                     join u in DbContext.Users
                     on p.ID equals u.POID
                     where u.UserName == userName
                     select p;
            return po.FirstOrDefault();
        }
    }
}