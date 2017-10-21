using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;
using System.Collections.Generic;

namespace PostOfiice.DAta.Repositories
{
    public interface IMainServiceGroupRepository:IRepository<MainServiceGroup> {
        
    }

    public class MainServiceGroupRepository : RepositoryBase<MainServiceGroup>, IMainServiceGroupRepository
    {
        public MainServiceGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public override MainServiceGroup Add(MainServiceGroup entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }

        public override void Update(MainServiceGroup entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}