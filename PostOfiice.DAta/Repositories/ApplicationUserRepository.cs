using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        int getNoUserByPoID(int PoID);

        IEnumerable<ApplicationUser> GetAllByPoId(int id);

        int getNoUserByGroup(int GroupId);

        int getPoId(string userName);

        bool CheckRole(string userName, string roleName);

        ApplicationUser getByUserName(string userName);
        ApplicationUser getByUserId(string userId);
    }

    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool CheckRole(string userName, string roleName)
        {
            var query = from u in DbContext.Users
                        join ug in DbContext.ApplicationUserGroups
                        on u.Id equals ug.UserId
                        join g in DbContext.ApplicationGroups
                        on ug.GroupId equals g.ID
                        where u.UserName == userName
                        select g;
            var isValid = false;
            foreach (var item in query)
            {
                if(item.Name==roleName)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        public IEnumerable<ApplicationUser> GetAllByPoId(int id)
        {
            return DbContext.Users.Where(x => x.POID == id).ToList();
        }

        public ApplicationUser getByUserName(string userName)
        {
            ApplicationUser user = new ApplicationUser();
            user = this.DbContext.Users.Where(x => x.UserName.Equals(userName)).FirstOrDefault();
            return user;
        }

        public int getNoUserByGroup(int GroupId)
        {
            var query = from ug in this.DbContext.ApplicationUserGroups
                        join g in this.DbContext.ApplicationGroups
                        on ug.GroupId equals g.ID
                        join u in this.DbContext.Users
                        on ug.UserId equals u.Id
                        where g.ID == GroupId
                        select u;
            return query.Count();
        }

        public int getNoUserByPoID(int PoID)
        {
            return this.DbContext.Users.Where(x => x.POID == PoID).Count();
        }

        public int getPoId(string userName)
        {
          
            var query = DbContext.Users.Where(x => x.UserName == userName).FirstOrDefault();
            if (query != null)
            {
                return query.POID;
            }
            else
            {
                return 0;
            }    
        }

        public ApplicationUser getByUserId(string userId)
        {
            ApplicationUser user = new ApplicationUser();
            user = DbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
            return user;
        }

    }
}