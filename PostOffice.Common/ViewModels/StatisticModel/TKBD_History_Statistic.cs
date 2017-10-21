using System;

namespace PostOffice.Common.ViewModels.StatisticModel
{
    public class TKBD_History_Statistic
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Account { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? Money { get; set; }
        public decimal? Rate { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }

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

        public string UpdatedBy
        {
            get; set;
        }

        public DateTime? UpdatedDate
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
    }
}