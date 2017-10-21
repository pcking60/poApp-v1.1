namespace PostOffice.Web.Models
{
    public class TKBD_Export_Detail_Template_ViewModel
    {
        public int STT { get; set; }
        public int Month { get; set; }
        public string Account { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public string FullName { get; set; }
    }
}