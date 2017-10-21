namespace PostOffice.Common.ViewModels.ExportModel
{
    public class Export_By_Service_Group_And_Time_District_Po_BCCP
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