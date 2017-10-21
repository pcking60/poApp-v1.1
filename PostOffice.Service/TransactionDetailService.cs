using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostOffice.Service
{
    public interface ITransactionDetailService
    {
        TransactionDetail Add(TransactionDetail transactionDetail);

        void Update(TransactionDetail transactionDetail);

        TransactionDetail Delete(int id);

        IEnumerable<TransactionDetail> GetAll();

        IEnumerable<TransactionDetail> GetAll(string keyword);

        IEnumerable<TransactionDetail> GetAllByTransactionId(int transactionId);

        IEnumerable<TransactionDetail> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        TransactionDetail GetById(int id);

        decimal? GetTotalMoneyByTransactionId(int id);

        decimal? GetTotalEarnMoneyByTransactionId(int id);

        decimal? GetTotalEarnMoneyByUsername(string userName);

        IEnumerable<TransactionDetail> GetAllByCondition(string condition);
        TransactionDetail GetAllByCondition(string condition, int transactionId);

        void Save();
    }

    public class TransactionDetailService : ITransactionDetailService
    {
        private ITransactionDetailRepository _transactionDetailRepository;
        private ITransactionRepository _transactionRepository;
        private IPropertyServiceRepository _propertyServiceRepository;
        private IApplicationUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IServiceRepository _serviceRepository;

        public TransactionDetailService(IServiceRepository serviceRepository, ITransactionDetailRepository transactionDetailRepository, IApplicationUserRepository userRepository, IPropertyServiceRepository propertyServiceRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            this._transactionDetailRepository = transactionDetailRepository;
            _propertyServiceRepository = propertyServiceRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            this._unitOfWork = unitOfWork;
            _serviceRepository = serviceRepository;
        }

        public TransactionDetail Add(TransactionDetail transactionDetail)
        {
            return _transactionDetailRepository.Add(transactionDetail);
        }

        public TransactionDetail Delete(int id)
        {
            return _transactionDetailRepository.Delete(id);
        }

        public IEnumerable<TransactionDetail> GetAll()
        {
            return _transactionDetailRepository.GetAll();
        }

        public IEnumerable<TransactionDetail> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _transactionDetailRepository.GetMulti(x => x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _transactionDetailRepository.GetAll();
            }
        }

        public TransactionDetail GetById(int id)
        {
            return _transactionDetailRepository.GetSingleByID(id);
        }

        public decimal? GetTotalEarnMoneyByTransactionId(int id)
        {
            Transaction ts = _transactionRepository.GetSingleByID(id);
            Model.Models.Service s = _serviceRepository.GetSingleByID(ts.ServiceId);
            float? vat = s.VAT;
            decimal? earnTotal = 0;
            var listTransactionDetail = _transactionDetailRepository.GetMulti(x => x.TransactionId == id).ToList();
            foreach (var item in listTransactionDetail)
            {
                decimal? percent = _propertyServiceRepository.GetSingleByID(item.PropertyServiceId).Percent;
                if (vat > 0 && vat != null)
                {
                    earnTotal = earnTotal + percent * item.Money / Convert.ToDecimal(vat);
                }
                else
                {
                    earnTotal = earnTotal + percent * item.Money;
                }
            }
            int? quantity = _transactionRepository.GetSingleByID(id).Quantity;
            return earnTotal;
        }

        public decimal? GetTotalMoneyByTransactionId(int id)
        {
            //int? quantity = _transactionRepository.GetSingleByID(id).Quantity;
            //decimal? totalMoney = quantity * _transactionDetailRepository.GetMulti(x => x.TransactionId == id).Sum(x => x.Money);
            //string condition = "Sản lượng";
            //var listTransactionDetails = _transactionDetailRepository.GetAllByTransactionId(id);
            //int count = listTransactionDetails.Count();
            //decimal? sum = 0;
            //foreach (var item in listTransactionDetails)
            //{
            //    sum += _transactionDetailRepository.GetMulti(x => x.TransactionId == id && x.ID == item.ID).Sum(x => x.Money);
            //}
            return _transactionDetailRepository.GetTotalMoneyByTransactionId(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<TransactionDetail> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _transactionDetailRepository.GetMulti(x => x.Status && x.MetaDescription.Contains(keyword));

            totalRow = query.OrderByDescending(x => x.CreatedDate).Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(TransactionDetail transactionDetail)
        {
            _transactionDetailRepository.Update(transactionDetail);
        }

        public IEnumerable<TransactionDetail> GetAllByTransactionId(int transactionId)
        {
            return _transactionDetailRepository.GetMulti(x => x.TransactionId == transactionId);
        }

        public decimal? GetTotalEarnMoneyByUsername(string userName)
        {
            decimal? earnTotal = 0;
            var currentDate = DateTime.Now;
            var firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDateOfMonth = firstDateOfMonth.AddMonths(1).AddDays(-1);
            string userId = _userRepository.getByUserName(userName).Id;
            var listTransactions = _transactionRepository.GetMulti(x => x.UserId == userId && (x.TransactionDate>=firstDateOfMonth && x.TransactionDate<=lastDateOfMonth) && x.Status == true).ToList();
            foreach (var item in listTransactions)
            {
                var listTransactionDetail = _transactionDetailRepository.GetMulti(x => x.TransactionId == item.ID).ToList();
                var vat = _serviceRepository.GetSingleByID(item.ServiceId).VAT;
                foreach (var item1 in listTransactionDetail)
                {
                    decimal? percent = _propertyServiceRepository.GetSingleByID(item1.PropertyServiceId).Percent;
                    earnTotal = earnTotal + (percent * item1.Money)/ decimal.Parse(vat.ToString());
                }                
            }
            return earnTotal;
        }

        public IEnumerable<TransactionDetail> GetAllByCondition(string condition)
        {
            return _transactionDetailRepository.GetAllByCondition(condition);
        }

        public TransactionDetail GetAllByCondition(string condition, int transactionId)
        {
            return _transactionDetailRepository.GetAllByCondition(condition, transactionId);
        }
    }
}