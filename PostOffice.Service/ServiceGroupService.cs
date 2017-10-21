using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System;

namespace PostOffice.Service
{
    public interface IServiceGroupService
    {
        ServiceGroup Add(ServiceGroup ServiceGroup);

        void update(ServiceGroup ServiceGroup);

        ServiceGroup Delete(int id);

        IEnumerable<ServiceGroup> GetAll();

        IEnumerable<ServiceGroup> GetAll(string keyword);

        IEnumerable<ServiceGroup> GetAllByParentId(int parentId);

        ServiceGroup GetById(int id);

        IEnumerable<Model.Models.Service> GetAllByServiceGroupId(int id);
        ServiceGroup GetSigleByServiceId(int id);
        int GetGroupIdByServiceId(int serviceId);

        void Save();
    }

    public class ServiceGroupService : IServiceGroupService
    {
        private IServiceGroupRepository _serviceGroupRepository;
        private IUnitOfWork _unitOfWork;

        public ServiceGroupService(IServiceGroupRepository serviceGroupRepository, IUnitOfWork unitOfWork)
        {
            this._serviceGroupRepository = serviceGroupRepository;
            this._unitOfWork = unitOfWork;
        }

        public ServiceGroup Add(ServiceGroup ServiceGroup)
        {
            return _serviceGroupRepository.Add(ServiceGroup);
        }

        public ServiceGroup Delete(int id)
        {
            return _serviceGroupRepository.Delete(id);
        }

        public IEnumerable<ServiceGroup> GetAll()
        {
            return _serviceGroupRepository.GetAll();
        }

        public IEnumerable<ServiceGroup> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _serviceGroupRepository.GetMulti(x => x.Name.Contains(keyword) || x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _serviceGroupRepository.GetAll();
            }
        }

        public IEnumerable<ServiceGroup> GetAllByParentId(int parentId)
        {
            return _serviceGroupRepository.GetMulti(x => x.MainServiceGroupId == parentId && x.Status);
        }

        public IEnumerable<Model.Models.Service> GetAllByServiceGroupId(int id)
        {
            return _serviceGroupRepository.GetAllByServiceGroupId(id);
        }

        public ServiceGroup GetById(int id)
        {
            return _serviceGroupRepository.GetSingleByID(id);
        }

        public int GetGroupIdByServiceId(int serviceId)
        {
            return _serviceGroupRepository.GetGroupIdByServiceId(serviceId);
        }

        public ServiceGroup GetSigleByServiceId(int id)
        {
            return _serviceGroupRepository.GetSigleByServiceId(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void update(ServiceGroup ServiceGroup)
        {
            _serviceGroupRepository.Update(ServiceGroup);
        }
    }
}