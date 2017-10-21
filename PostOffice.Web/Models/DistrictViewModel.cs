using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class DistrictViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string CreatedBy
        {
            get; set;
        }

        public int Code { get; set; }

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

        public virtual IEnumerable<POViewModel> PostOffices { get; set; }

        //tách ra ko hồi sau nhìn lại rối đó bạn.
        public int NoPO { get; set; }
    }
}