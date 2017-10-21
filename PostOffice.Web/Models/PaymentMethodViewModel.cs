using System;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class PaymentMethodViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int? Value { get; set; }

        public string Description { get; set; }

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

        [Required]
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
    }
}