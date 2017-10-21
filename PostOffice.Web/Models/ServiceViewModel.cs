using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PostOffice.Web.Models
{
    public class ServiceViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Alias { get; set; }

        public int GroupID { get; set; }

        public string Unit { get; set; }

        public decimal? BuyIn { get; set; }
        public decimal? SoldOut { get; set; }
        public float? VAT { get; set; }
        public int? PayMethodID { get; set; }

        public string Description { get; set; }
        public virtual IEnumerable<PaymentMethodViewModel> PaymentMethods { get; set; }
        public virtual ServiceGroupViewModel ServiceGroup { get; set; }
        //[2]
        public string GroupName { get; set; }
        public int NoService { get; set; }
        #region common

        public string CreatedBy
        {
            get; set;
        }

        public DateTime? CreatedDate
        {
            get; set;
        }

        public string MetaDescription
        {
            get; set;
        }

        public string MetaKeyWord
        {
            get; set;
        }

        public bool Status
        {
            get; set;
        }

        public string UpdatedBy
        {
            get; set;
        }

        public DateTime? UpdatedDate
        {
            get; set;
        }

        #endregion common
    }

}