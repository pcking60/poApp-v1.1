using System.Collections.Generic;

namespace PostOffice.Web.Models
{
    public class ApplicationGroupViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public IEnumerable<ApplicationRoleViewModel> Roles { set; get; }

        #region
        public int NoUser { get; set; }
        #endregion
    }
}