namespace PostOffice.Common.ViewModels.ExportModel
{
    public class TKBD_Export_Detail_Template
    {
        public int Month { get; set; }
        public string Account { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
    }
}