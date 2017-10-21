using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System;
using System.Collections.Generic;

namespace PostOffice.Service
{
    public interface IServiceService
    {
        Model.Models.Service Add(Model.Models.Service service);

        void Update(Model.Models.Service service);

        Model.Models.Service Delete(int id);

        IEnumerable<PostOffice.Model.Models.Service> Getall();

        IEnumerable<PostOffice.Model.Models.Service> Getall(string keyword);

        Model.Models.Service GetById(int id);

        IEnumerable<Model.Models.Service> GetAllByServiceGroupID(int id);

        IEnumerable<Model.Models.Service> GetAllByUserId(string userId);

        void Save();
    }

    public class ServiceService : IServiceService
    {
        private IServiceRepository _serviceRepository;
        private IUnitOfWork _unitOfWork;

        public ServiceService(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
        {
            this._serviceRepository = serviceRepository;
            this._unitOfWork = unitOfWork;
        }

        public Model.Models.Service Add(Model.Models.Service service)
        {
            return _serviceRepository.Add(service);
        }

        public Model.Models.Service Delete(int id)
        {
            return _serviceRepository.Delete(id);
        }

        public IEnumerable<Model.Models.Service> Getall()
        {
            return _serviceRepository.GetAll();
        }

        public IEnumerable<Model.Models.Service> Getall(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _serviceRepository.GetMulti(x => x.Name.Contains(keyword) || x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _serviceRepository.GetAll();
            }
        }

        public IEnumerable<Model.Models.Service> GetAllByParentId(int parentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model.Models.Service> GetAllByServiceGroupID(int id)
        {
            return _serviceRepository.GetAllByServiceGroupID(id);
        }

        public IEnumerable<Model.Models.Service> GetAllByUserId(string userId)
        {
            return _serviceRepository.GetAllByUserId(userId);
        }

        public Model.Models.Service GetById(int id)
        {
            return _serviceRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Model.Models.Service service)
        {
            _serviceRepository.Update(service);
        }
    }
}