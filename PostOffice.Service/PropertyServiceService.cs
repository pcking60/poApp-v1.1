using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOffice.Service
{
    public interface IPropertyServiceService
    {
        PropertyService Add(PropertyService pro);

        void Update(PropertyService pro);

        PropertyService Delete(int id);

        IEnumerable<PropertyService> GetAll();

        IEnumerable<PropertyService> GetAll(string keyword);

        IEnumerable<PropertyService> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<PropertyService> GetListPropertyByServiceId(int serviceId);

        PropertyService GetById(int id);

        void Save();
    }
    public class PropertyServiceService : IPropertyServiceService
    {
        private IPropertyServiceRepository _propertyServiceRepository;
        private IUnitOfWork _unitOfWork;

        public PropertyServiceService (IPropertyServiceRepository prop, IUnitOfWork unit)
        {
            this._propertyServiceRepository = prop;
            this._unitOfWork = unit;
        }

        public PropertyService Add(PropertyService pro)
        {
            return _propertyServiceRepository.Add(pro);
        }

        public PropertyService Delete(int id)
        {
            return _propertyServiceRepository.Delete(id);
        }

        public IEnumerable<PropertyService> GetAll()
        {
            return _propertyServiceRepository.GetAll();
        }

        public IEnumerable<PropertyService> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _propertyServiceRepository.GetMulti(x => x.Name.Contains(keyword)||x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _propertyServiceRepository.GetAll();
            }
        }

        public PropertyService GetById(int id)
        {
            return _propertyServiceRepository.GetSingleByID(id);
        }

        public IEnumerable<PropertyService> GetListPropertyByServiceId(int serviceId)
        {
            var listProp = _propertyServiceRepository.GetMulti(x => x.ServiceID == serviceId).ToList();
            return listProp;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<PropertyService> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _propertyServiceRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));

            totalRow = query.OrderByDescending(x => x.CreatedDate).Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(PropertyService pro)
        {
            _propertyServiceRepository.Update(pro);
        }
    }
}
