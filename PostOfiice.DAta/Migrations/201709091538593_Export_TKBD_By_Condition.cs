namespace PostOfiice.DAta.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Export_TKBD_By_Condition : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "Export_TKBD_By_Time",
                p => new
                {
                    month = p.Int(),
                    year = p.Int(),
                },
                @"select
	                t.Month,
	                convert(int, count(t.Account)) as Quantity,
	                convert(decimal(16,2),
	                sum(t.Amount)) as DTTL,
	                t.CreatedBy
                from TKBDAmounts t
               where t.Status=1 and t.Month=@month and t.Year=@year
                group by t.Month, t.CreatedBy, t.Year");
        }

        public override void Down()
        {
            DropStoredProcedure("Export_TKBD_By_Time");
        }
    }
}