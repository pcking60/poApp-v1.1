using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;

namespace PostOffice.Service
{
    public interface IMainServiceGroupService
    {
        MainServiceGroup Add(MainServiceGroup ServiceGroup);

        void update(MainServiceGroup ServiceGroup);

        MainServiceGroup Delete(int id);

        IEnumerable<MainServiceGroup> GetAll();

        IEnumerable<MainServiceGroup> GetAll(string keyword);

        MainServiceGroup GetById(int id);

        void Save();
    }

    public class MainServiceGroupService : IMainServiceGroupService
    {
        private IMainServiceGroupRepository _mainServiceGroupRepository;
        private IUnitOfWork _unitOfWork;

        public MainServiceGroupService(IMainServiceGroupRepository main, IUnitOfWork unit)
        {
            this._mainServiceGroupRepository = main;
            this._unitOfWork = unit;
        }

        public MainServiceGroup Add(MainServiceGroup ServiceGroup)
        {
            return _mainServiceGroupRepository.Add(ServiceGroup);
        }

        public MainServiceGroup Delete(int id)
        {
            return _mainServiceGroupRepository.Delete(id);
        }

        public IEnumerable<MainServiceGroup> GetAll()
        {
            return _mainServiceGroupRepository.GetAll();
        }

        public IEnumerable<MainServiceGroup> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _mainServiceGroupRepository.GetMulti(x => x.Name.Contains(keyword) || x.MetaDescription.Contains(keyword));
            else
                return _mainServiceGroupRepository.GetAll();
        }

        public MainServiceGroup GetById(int id)
        {
            return _mainServiceGroupRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void update(MainServiceGroup ServiceGroup)
        {
            _mainServiceGroupRepository.Update(ServiceGroup);
        }
    }
}