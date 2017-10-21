using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PostOffice.Service
{
    public interface ITransactionService
    {
        Transaction Add(Transaction transaction);

        void Update(Transaction transaction);

        Transaction Delete(int id);

        IEnumerable<Transaction> GetAll();

        IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate);

        IEnumerable<Transaction> GetAllBy_UserName_Now(string userName);

        IEnumerable<Transaction> GetAllBy_UserName_7_Days(string userName);

        IEnumerable<Transaction> GetAllBy_UserName_30_Days(string userName);

        IEnumerable<Transaction> GetAllByTime(DateTime fromDate, DateTime toDate, string userName, string userId, int serviceId);
        IEnumerable<Transaction> General_statistic(DateTime fromDate, DateTime toDate, int districtId, int poId, string currentUserName, string selecttedUserId, int serviceId);

        IEnumerable<Transaction> GetAll(string keyword);

        IEnumerable<Transaction> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        Transaction GetById(int id);

        IEnumerable<Transaction> GetAllByMainGroupId(DateTime fromDate, DateTime toDate, int mainGroupId);

        IEnumerable<Transaction> GetAllBy_Time_DistrictID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int id);

        IEnumerable<Transaction> GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int poId, int id);

        void Save();
    }

    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private IUnitOfWork _unitOfWork;
        private IApplicationUserRepository _userRepository;
        private IApplicationGroupRepository _groupRepository;
        private IDistrictRepository _districtRepository;

        public TransactionService(IDistrictRepository districtRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IApplicationUserRepository userRepository, IApplicationGroupRepository groupRepository)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _districtRepository = districtRepository;
        }

        public Transaction Add(Transaction transaction)
        {
            return _transactionRepository.Add(transaction);
        }

        public Transaction Delete(int id)
        {
            return _transactionRepository.Delete(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public IEnumerable<Transaction> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _transactionRepository.GetMulti(x => x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _transactionRepository.GetAll();
            }
        }

        public IEnumerable<Transaction> GetAllBy_UserName_Now(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = _userRepository.getByUserName(userName);
                var date = DateTime.Now.Date;
                return _transactionRepository.GetMulti(x => x.UserId == user.Id && x.Status == true && DbFunctions.TruncateTime(x.TransactionDate) == date).ToList();
            }
            else
            {
                return null;
            }
            
        }

        public Transaction GetById(int id)
        {
            return _transactionRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Transaction> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var _q = _transactionRepository.GetMulti(x => x.Status && x.MetaDescription.Contains(keyword));

            totalRow = _q.OrderByDescending(x => x.CreatedDate).Count();

            return _q.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(Transaction transaction)
        {
            _transactionRepository.Update(transaction);
        }

        public IEnumerable<Transaction> GetAllByTime(DateTime fromDate, DateTime toDate, string userName, string userId, int serviceId)
        {
            var user = _userRepository.getByUserName(userName);
            var listGroup = _groupRepository.GetListGroupByUserId(user.Id);

            bool IsManager = false;
            bool IsAdministrator = false;

            foreach (var item in listGroup)
            {
                string name = item.Name;
                if (name == "Manager")
                {
                    IsManager = true;
                }
                if (name == "Administrator")
                {
                    IsAdministrator = true;
                }
            }
            if (IsAdministrator)
            {
                if (!string.IsNullOrEmpty(userId) && serviceId != 0)
                {
                    return _transactionRepository.GetAll(fromDate, toDate, userId, serviceId);
                }
                else
                {
                    if (string.IsNullOrEmpty(userId) && serviceId == 0)
                    {
                        return _transactionRepository.GetAll(fromDate, toDate);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(userId))
                        {
                            return _transactionRepository.GetAll(fromDate, toDate, serviceId);
                        }
                        else
                        {
                            return _transactionRepository.GetAll(fromDate, toDate, userId);
                        }
                    }
                }
            }
            else
            {
                if (IsManager)
                {
                    if (!string.IsNullOrEmpty(userId) && serviceId != 0)
                    {
                        return _transactionRepository.GetAllByTimeAndPOID(fromDate, toDate, user.POID, userId, serviceId);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(userId) && serviceId == 0)
                        {
                            return _transactionRepository.GetAllByTimeAndPOID(fromDate, toDate, user.POID);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(userId))
                            {
                                return _transactionRepository.GetAllByTimeAndPOID(fromDate, toDate, user.POID, serviceId);
                            }
                            else
                            {
                                return _transactionRepository.GetAllByTimeAndPOID(fromDate, toDate, user.POID, userId);
                            }
                        }
                    }
                }
                else
                {
                    return _transactionRepository.GetAllByTimeAndUsername(fromDate, toDate, user.UserName);
                }
            }
        }

        public IEnumerable<Transaction> GetAll(DateTime fromDate, DateTime toDate)
        {
            return _transactionRepository.GetAll(fromDate, toDate);
        }

        public IEnumerable<Transaction> GetAllByMainGroupId(DateTime fromDate, DateTime toDate, int mainGroupId)
        {
            return _transactionRepository.GetAllByMainGroupId(fromDate, toDate, mainGroupId);
        }

        public IEnumerable<Transaction> GetAllBy_Time_DistrictID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int id)
        {
            return _transactionRepository.GetAllBy_Time_DistrictID_MainGroupId(fromDate, toDate, districtId, id);
        }

        public IEnumerable<Transaction> GetAllBy_Time_DistrictID_POID_MainGroupId(DateTime fromDate, DateTime toDate, int districtId, int poId, int id)
        {
            return _transactionRepository.GetAllBy_Time_DistrictID_POID_MainGroupId(fromDate, toDate, districtId, poId, id);
        }

        public IEnumerable<Transaction> GetAllBy_UserName_7_Days(string userName)
        {
            var user = _userRepository.getByUserName(userName);
            var date = DateTime.Now.Date;
            var date1 = DateTime.Now.AddDays(-7);
            return _transactionRepository.GetMulti(x => x.UserId == user.Id && x.Status == true && (DbFunctions.TruncateTime(x.TransactionDate) <= date && DbFunctions.TruncateTime(x.TransactionDate) > date1)).ToList();
        }

        public IEnumerable<Transaction> GetAllBy_UserName_30_Days(string userName)
        {
            var user = _userRepository.getByUserName(userName);
            var date = DateTime.Now.Date;
            var date1 = DateTime.Now.AddDays(-30);
            return _transactionRepository.GetMulti(x => x.UserId == user.Id && x.Status == true && (DbFunctions.TruncateTime(x.TransactionDate) <= date && DbFunctions.TruncateTime(x.TransactionDate) > date1)).ToList();
        }

        public IEnumerable<Transaction> General_statistic(DateTime fromDate, DateTime toDate, int districtId, int poId, string currentUserName, string selecttedUserId, int serviceId)
        {
            var user = _userRepository.getByUserName(currentUserName);
            var listGroup = _groupRepository.GetListGroupByUserId(user.Id);

            bool IsManager = false;
            bool IsAdministrator = false;

            foreach (var item in listGroup)
            {
                string name = item.Name;
                if (name == "Manager")
                {
                    IsManager = true;
                }
                if (name == "Administrator")
                {
                    IsAdministrator = true;
                }
            }
            if (IsAdministrator)
            {
                if (districtId == 0) // user not select anything
                {
                    return _transactionRepository.GetAll(fromDate, toDate);
                }
                else
                {
                    if (poId == 0) // user select only district 
                    {
                        return _transactionRepository.Get_by_time_districtId(fromDate, toDate, districtId);
                    }
                    else 
                    {
                        if (string.IsNullOrEmpty(selecttedUserId)) // user select only district and po
                        {
                            return _transactionRepository.Get_by_time_districtId_poId(fromDate, toDate, districtId, poId);
                        }
                        else
                        {
                            if (serviceId == 0) // user select district, po and user
                            {
                                return _transactionRepository.Get_by_time_districtId_poId_userId(fromDate, toDate, districtId, poId, selecttedUserId);
                            }
                            else // user select district, po, user and service
                            {
                                return _transactionRepository.Get_by_time_districtId_poId_userId_serviceId(fromDate, toDate, districtId, poId, selecttedUserId, serviceId);
                            }
                        }
                    }                    
                }                
            }
            else
            {
                if (IsManager)
                {
                    districtId = _districtRepository.GetDistrictByUserName(currentUserName).ID;
                    if (poId == 0) // user select only district 
                    {
                        return _transactionRepository.Get_by_time_districtId(fromDate, toDate, districtId);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(selecttedUserId)) // user select only district and po
                        {
                            return _transactionRepository.Get_by_time_districtId_poId(fromDate, toDate, districtId, poId);
                        }
                        else
                        {
                            if (serviceId == 0) // user select district, po and user
                            {
                                return _transactionRepository.Get_by_time_districtId_poId_userId(fromDate, toDate, districtId, poId, selecttedUserId);
                            }
                            else // user select district, po, user and service
                            {
                                return _transactionRepository.Get_by_time_districtId_poId_userId_serviceId(fromDate, toDate, districtId, poId, selecttedUserId, serviceId);
                            }
                        }
                    }
                }
                else
                {
                    return _transactionRepository.GetAllByTimeAndUsername(fromDate, toDate, user.UserName);
                }
            }
        }
    }
}