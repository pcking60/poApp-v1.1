using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOffice.Common.ViewModels.ExportModel
{
    public class Export_Template_TCBC
    {
        public int? STT { get; set; }
        public string ServiceName { get; set; }
        public int? Quantity { get; set; }
        public float? Fee { get; set; }
        public decimal? TotalColection { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? Sale { get; set; }
        public decimal? Tax { get; set; }
        public decimal? EarnMoney { get; set; }
    }
}