using System;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class PropertyServiceViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }
       
        public decimal? Percent { get; set; }

        public int ServiceID { get; set; }

        #region

        public string ServiceName { get; set; }

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