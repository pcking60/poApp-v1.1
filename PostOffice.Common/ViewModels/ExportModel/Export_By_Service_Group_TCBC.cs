namespace PostOffice.Common.ViewModels.ExportModel
{
    public class Export_By_Service_Group_TCBC
    {
        public int? STT { get; set; }
        public string ServiceName { get; set; }
        public int? Quantity { get; set; }
        public float? VAT { get; set; }
        public decimal? TotalColection { get; set; }
        public decimal? VatOfTotalColection { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? VatOfTotalPay { get; set; }
        public decimal? EarnMoney { get; set; }
    }
}