using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PostOffice.Service
{
    public interface IDistrictService
    {
        District Add(District District);

        void Update(District District);

        District Delete(int id);

        IEnumerable<District> GetAll();

        IEnumerable<District> GetAll(string keyword);

        IEnumerable<District> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<string> GetListDistrictByName(string name);        

        District GetById(int id);
        District GetDistrictByUserName(string userName);
        District GetDistrictByUserId(string userId);

        void Save();
    }

    public class DistrictService : IDistrictService
    {
        private IDistrictRepository _DistrictRepository;

        private IUnitOfWork _unitOfWork;

        public DistrictService(IDistrictRepository DistrictRepository, IUnitOfWork unitOfWork)
        {
            this._DistrictRepository = DistrictRepository;
            this._unitOfWork = unitOfWork;
        }

        public District Add(District District)
        {
            return _DistrictRepository.Add(District);
        }

        public District Delete(int id)
        {
            return _DistrictRepository.Delete(id);
        }

        public IEnumerable<District> GetAll()
        {
            return _DistrictRepository.GetAll();
        }

        public IEnumerable<District> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _DistrictRepository.GetMulti(x => x.Name.Contains(keyword));
            else
                return _DistrictRepository.GetAll();
        }

        public District GetById(int id)
        {
            return _DistrictRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(District District)
        {
            _DistrictRepository.Update(District);
        }

        public IEnumerable<string> GetListDistrictByName(string name)
        {
            return _DistrictRepository.GetMulti(x => x.Status && x.Name.Contains(name)).Select(y => y.Name);
        }

        public IEnumerable<District> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _DistrictRepository.GetMulti(x => x.Status && x.Name.Contains(keyword));

            totalRow = query.OrderByDescending(x => x.CreatedDate).Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public District GetDistrictByUserId(string userId)
        {
            return _DistrictRepository.GetDistrictByUserId(userId);
        }

        public District GetDistrictByUserName(string userName)
        {
            return _DistrictRepository.GetDistrictByUserName(userName);
        }
    }
}