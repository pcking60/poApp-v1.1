using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        IEnumerable<Service> GetByAlias(string alias);

        IEnumerable<Service> GetAllByServiceGroupID(int id);

        IEnumerable<Service> GetAllByUserId(string userId);
    }
    
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        IEnumerable<Service> IServiceRepository.GetByAlias(string alias)
        {
            return this.DbContext.Services.Where(x => x.Alias == alias).ToList();
        }

        public override Service Add(Service entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override void Update(Service entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }

        public override Service Delete(int id)
        {
            return base.Delete(id);
        }

        public IEnumerable<Service> GetAllByServiceGroupID(int id)
        {
            var no = from s in this.DbContext.Services
                     join sv in this.DbContext.ServiceGroups
                     on s.GroupID equals sv.ID
                     where sv.ID == id
                     select s;
            return no;
        }

        public IEnumerable<Service> GetAllByUserId(string userId)
        {
            var q = from ts in DbContext.Transactions
                    join s in DbContext.Services
                    on ts.ServiceId equals s.ID
                    where ts.UserId == userId
                    select s;
            return q.ToList().Distinct().OrderBy(x=>x.Name);
        }
    }
}