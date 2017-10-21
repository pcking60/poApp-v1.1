using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System;

namespace PostOffice.Service
{
    public interface IPOService
    {
        PO Add(PO po);

        PO Delete(int id);

        void Update(PO po);

        void Save();

        PO GetByID(int id);

        IEnumerable<PO> Getall();
        IEnumerable<PO> GetAllPOByDistrictId(int districtID);
        PO GetPOByCurrentUser(string userName);
    }

    public class POService : IPOService
    {
        private IPORepository _poRepository;
        private IUnitOfWork _uniOfWork;

        public POService(IPORepository poRepository, IUnitOfWork unitOfWork)
        {
            this._poRepository = poRepository;
            this._uniOfWork = unitOfWork;
        }

        public PO Add(PO po)
        {
            return _poRepository.Add(po);
        }

        public PO Delete(int id)
        {
            return _poRepository.Delete(id);
        }

        public IEnumerable<PO> Getall()
        {
            return _poRepository.GetAll();
        }

        public IEnumerable<PO> GetAllPOByDistrictId(int districtID)
        {
            return _poRepository.GetAllPOByDistrictId(districtID);
        }

        public PO GetByID(int id)
        {
            return _poRepository.GetSingleByID(id);
        }

        public PO GetPOByCurrentUser(string userName)
        {
            return _poRepository.GetPOByCurrentUser(userName);
        }

        public void Save()
        {
            _uniOfWork.Commit();
        }

        public void Update(PO po)
        {
            _poRepository.Update(po);
        }
    }
}