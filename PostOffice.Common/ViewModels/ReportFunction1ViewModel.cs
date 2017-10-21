using System;

namespace PostOffice.Common.ViewModels
{
    public class ReportFunction1ViewModel
    {
        public string FunctionName { get; set; }
        public string Unit { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CreatedBy { get; set; }
    }
}