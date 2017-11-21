using PostOffice.Model.Models;
using System;

namespace PostOffice.Web.Models
{
    public class TKBDHistoryViewModel
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ServiceId { get; set; }
        public string Account { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? Money { get; set; }
        public decimal? Rate { get; set; }       
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? TimeCode { get; set; }
        public string CreatedBy
        {
            get; set;
        }

        public DateTime? CreatedDate
        {
            get; set;
        }
        public bool Status
        {
            get; set;
        }
    }
}