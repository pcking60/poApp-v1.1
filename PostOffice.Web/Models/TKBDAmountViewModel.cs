using System;

namespace PostOffice.Web.Models
{
    public class TKBDAmountViewModel
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Account { get; set; }
        public decimal Amount { get; set; }
        public decimal? TotalMoney { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public DateTimeOffset? TransactionDate { get; set; }
        public string TransactionUser { get; set; }
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
    }
}