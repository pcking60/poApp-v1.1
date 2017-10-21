using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { set; get; }
        public string FullName { set; get; }
        public DateTimeOffset? BirthDay { set; get; }
        public string Bio { set; get; }
        public string Email { set; get; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string PasswordHash { set; get; }
        public string UserName { set; get; }
        public int POID { get; set; }        
        public string PhoneNumber { set; get; }
        public bool Status { get; set; }
        public string Address { get; set; }

        public IEnumerable<ApplicationGroupViewModel> Groups { set; get; }

        #region
        public string POName { get; set; }
        public string GroupName { get; set; }

        public decimal? TotalEarn { get; set; }
        #endregion
    }
}