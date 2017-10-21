using System;

namespace PostOffice.Common.ViewModels.StatisticModel
{
    public class TKBD_History
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }
        public string Name { get; set; }

        public string Account { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? Money { get; set; }
        public decimal? Rate { get; set; }
        public string UserId { get; set; }
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