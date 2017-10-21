using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class ServiceGroupViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Alias { get; set; }

        public string Description { get; set; }

        public int? MainServiceGroupId { get; set; }

        public int? DisplayOrder { get; set; }

        public string Image { get; set; }

        //[2]
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

        public virtual IEnumerable<ServiceViewModel> Services { get; set; }
        public int NoService { get; set; }
    }
}