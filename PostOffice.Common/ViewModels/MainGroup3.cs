using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOffice.Common.ViewModels
{
    public class MainGroup3
    {
        public int? STT { get; set; }
        public string ServiceName { get; set; }
        public int? Quantity { get; set; }
        public float? VAT { get; set; }
        public decimal? TotalMoneySent { get; set; }       
        public decimal? TotalMoneyReceive { get; set; }
        public decimal? EarnMoney { get; set; }
    }
}
