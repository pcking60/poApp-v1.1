using System;

namespace PostOffice.Common.ViewModels
{
    public class ReportServiceViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float? VAT { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}