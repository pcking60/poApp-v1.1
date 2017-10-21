using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostOfiice.DAta.Repositories
{
    public interface IServiceGroupRepository : IRepository<ServiceGroup>
    {
        IEnumerable<ServiceGroup> GetByAlias(string alias);

        IEnumerable<Service> GetAllByServiceGroupId(int id);

        ServiceGroup GetSigleByServiceId(int id);
        int GetGroupIdByServiceId(int serviceId);
    }

    public class ServiceGroupRepository : RepositoryBase<ServiceGroup>, IServiceGroupRepository
    {
        public ServiceGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ServiceGroup> GetByAlias(string alias)
        {
            return this.DbContext.ServiceGroups.Where(x => x.Alias == alias).ToList();
        }

        public override ServiceGroup Add(ServiceGroup entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override void Update(ServiceGroup entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }

        public override ServiceGroup Delete(int id)
        {
            return base.Delete(id);
        }

        public IEnumerable<Service> GetAllByServiceGroupId(int id)
        {
            var no = from s in this.DbContext.Services
                     join sg in this.DbContext.ServiceGroups
                     on s.GroupID equals sg.ID
                     where sg.ID == id
                     select s;
            return no;
        }

        public ServiceGroup GetSigleByServiceId(int id)
        {
            var query = from s in DbContext.Services
                        join sg in DbContext.ServiceGroups
                        on s.GroupID equals sg.ID
                        join ts in DbContext.Transactions
                        on s.ID equals ts.ServiceId
                        where ts.ID == id
                        select sg;
            return query.FirstOrDefault();
        }

        public int GetGroupIdByServiceId(int serviceId)
        {
            var query = from s in DbContext.Services
                        join sg in DbContext.ServiceGroups
                        on s.GroupID equals sg.ID
                        where s.ID == serviceId
                        select sg;
            return query.FirstOrDefault().ID;
        }
    }
}