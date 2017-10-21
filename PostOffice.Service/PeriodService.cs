using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PostOffice.Service
{
    public interface IPeriodService : IRepository<Period>
    {
        void Save();
    }

    public class PeriodService : IPeriodService
    {
        private IPeriodRepository _periodRepository;
        private IUnitOfWork _unitOfWork;

        public PeriodService(IPeriodRepository periodRepository, IUnitOfWork unitOfWork)
        {
            _periodRepository = periodRepository;
            _unitOfWork = unitOfWork;
        }

        public Period Add(Period entity)
        {
            return _periodRepository.Add(entity);
        }

        public bool CheckContains(Expression<Func<Period, bool>> predicate)
        {
            return _periodRepository.CheckContains(predicate);
        }

        public int Count(Expression<Func<Period, bool>> where)
        {
            return _periodRepository.Count(where);
        }

        public Period Delete(int id)
        {
            return _periodRepository.Delete(id);
        }

        public Period Delete(Period entity)
        {
            return _periodRepository.Delete(entity);
        }

        public void DeleteMulti(Expression<Func<Period, bool>> where)
        {
            _periodRepository.DeleteMulti(where);
        }

        public IEnumerable<Period> GetAll(string[] includes = null)
        {
            return _periodRepository.GetAll(includes);
        }

        public IEnumerable<Period> GetMulti(Expression<Func<Period, bool>> predicate, string[] includes = null)
        {
            return _periodRepository.GetAll(includes);
        }

        public IEnumerable<Period> GetMultiPaging(Expression<Func<Period, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public Period GetSingleByCondition(Expression<Func<Period, bool>> expression, string[] includes = null)
        {
            return _periodRepository.GetSingleByCondition(expression);
        }

        public Period GetSingleByID(int id)
        {
            return _periodRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Period entity)
        {
            _periodRepository.Update(entity);
        }
    }
}