namespace PostOffice.Common.ViewModels.ExportModel
{
    public class Export_By_Service_Group_And_Time
    {
        public string ServiceName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Money { get; set; }
        public decimal? Vat { get; set; }
    }
}