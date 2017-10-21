using System;

namespace PostOffice.Common.ViewModels
{
    public class ReportTemplate
    {
        public string FunctionName { get; set; }
        public string District { get; set; }
        public string Unit { get; set; }
        public string user { get; set; }
        public string Service { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CreatedBy { get; set; }
    }
}