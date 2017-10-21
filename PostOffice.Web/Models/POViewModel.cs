using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class POViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public int Code { get; set; }
        public int? POStyle { get; set; }

        public string POAddress { get; set; }

        public string POMobile { get; set; }

        [Required]
        public int DistrictID { get; set; }

        public virtual DistrictViewModel District { get; set; }


        public int NoUser { get; set; }
        public IEnumerable<ApplicationUserViewModel> Staffs { get; set; }

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

        //mình khai báo biến này. sau đó trên api mình gọi hàm getDistrictNameById.
        //rồi gán data đó vào biến này. như vậy trên client Ichir cần gọi item.DistrictName. rất là khỏe và rõ ràng

        public string DistrictName { get; set; }
    }
}