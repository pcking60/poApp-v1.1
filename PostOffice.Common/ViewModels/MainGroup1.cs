namespace PostOffice.Common.ViewModels
{
    public class MainGroup1
    {
        public int? STT { get; set; }
        public string ServiceName { get; set; }
        public int? Quantity { get; set; }
        public float? VAT { get; set; }
        public decimal? TotalCash { get; set; }
        public decimal? VatOfTotalCash { get; set; }
        public decimal? TotalDebt { get; set; }
        public decimal? VatOfTotalDebt { get; set; }
        public decimal? EarnMoney { get; set; }
    }
}