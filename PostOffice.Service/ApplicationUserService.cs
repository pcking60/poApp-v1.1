using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System;

namespace PostOffice.Service
{
    public interface IApplicationUserService
    {
        int getNoUserByPoID(int PoID);

        IEnumerable<ApplicationUser> GetAllByPOID(int id);

        ApplicationUser getByUserName(string userName);

        bool CheckRole(string userName, string roleName);
        int getPoId(string userName);
        ApplicationUser getByUserId(string userId);
    }

    public class ApplicationUserService : IApplicationUserService
    {
        private IApplicationUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicationUserService(IUnitOfWork unitOfWork,
           IApplicationUserRepository user)
        {
            this._userRepository = user;
            this._unitOfWork = unitOfWork;
        }

        public int getNoUserByPoID(int PoID)
        {
            return _userRepository.getNoUserByPoID(PoID);
        }

        public ApplicationUser getByUserName(string userName)
        {
            return _userRepository.getByUserName(userName);
        }

        public IEnumerable<ApplicationUser> GetAllByPOID(int id)
        {
            return _userRepository.GetAllByPoId(id);
        }

        public bool CheckRole(string userName, string roleName)
        {
            return _userRepository.CheckRole(userName, roleName);
        }

        public int getPoId(string userName)
        {
            return _userRepository.getPoId(userName);
        }


        public ApplicationUser getByUserId(string userId)
        {
            return _userRepository.getByUserId(userId);
        }
    }
}