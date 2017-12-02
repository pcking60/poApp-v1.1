using PostOffice.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostOffice.Web.Models
{
    public class TransactionViewModel
    {
        public int ID { get; set; }

        public int ServiceId { get; set; }

        public string UserId { get; set; }

        public int? Quantity { get; set; }       

        public decimal? TotalMoney { get; set; }

        public decimal? TotalCash { get; set; }

        public decimal? TotalDebt { get; set; }

        public decimal? EarnMoney { get; set; }
        public float? VAT { get; set; }
        public decimal? VatOfTotalMoney { get; set; }
        public decimal? VatOfTotalCash { get; set; }
        public decimal? VatOfTotalDebt { get; set; }
        public decimal? TotalVat { get; set; }
        public bool IsCash { get; set; }
        public bool IsCurrency { get; set; }
        public decimal? TotalMoneySent { get; set; }
        public decimal? TotalFee { get; set; }
        public decimal? Fee { get; set; }
        public decimal? TotalMoneyReceive { get; set; }
        public decimal? TotalCurrency { get; set; }
        public bool IsReceive { get; set; }
        public decimal? TotalMoneyBeforeVat { get; set; }
        public decimal? TotalColection { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? Sales { get; set; }
        public int groupId { get; set; }
        [Required]
        public DateTimeOffset TransactionDate { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
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