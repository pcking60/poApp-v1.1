using System;

namespace PostOffice.Common.ViewModels
{
    public class statisticReportViewModel
    {
        public string PoName { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string ServiceName { get; set; }
        public string UserName { get; set; }
    }
}