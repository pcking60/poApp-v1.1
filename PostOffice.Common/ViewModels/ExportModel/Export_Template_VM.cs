namespace PostOffice.Common.ViewModels.ExportModel
{
    public class Export_Template_VM
    {
        public int? STT { get; set; }
        public string ServiceName { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalCash { get; set; }
        public decimal? TotalDebt { get; set; }
        public decimal? TotalMoneyBeforeVat { get; set; }
        public decimal? TotalVat { get; set; }
        public decimal? EarnMoney { get; set; }
    }
}