using System;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class TransactionDetailViewModel
    {
        public int ID { get; set; }

        [Required]
        public int TransactionID { get; set; }

        [Required]
        public int PropertyServiceId { get; set; }

        public int Quantity { get; set; }

        public decimal? Money { get; set; }

        #region

        public string ServiceName { get; set; }

        public string PropertyServiceName { get; set; }

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

        #endregion
    }
}