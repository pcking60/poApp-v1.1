using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface IPropertyServiceRepository : IRepository<PropertyService> { }

    public class PropertyServiceRepository : RepositoryBase<PropertyService>, IPropertyServiceRepository
    {
        public PropertyServiceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public override PropertyService Add(PropertyService entity)
        {
            entity.CreatedDate = DateTime.Now;
            return base.Add(entity);
        }
        public override void Update(PropertyService entity)
        {
            entity.UpdatedDate = DateTime.Now;
            base.Update(entity);
        }
    }
}