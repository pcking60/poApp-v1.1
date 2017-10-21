using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostOffice.Web.Models
{
    public class TKBD_Export_Template_ViewModel
    {
        public int STT { get; set; }
        public int Month { get; set; }
        public int Quantity { get; set; }
        public decimal DTTL { get; set; }
        public string CreatedBy { get; set; }
        public string FullName { get; set; }
    }
}