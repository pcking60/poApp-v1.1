namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Export_TKBD_By_Time_District : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
               "Export_TKBD_By_Time_District",
               p => new
               {
                   month = p.Int(),
                   year = p.Int(),
                   districtId = p.Int()
               },
               @"select
	                t.Month,
	                convert(int, count(t.Account)) as Quantity,
CONVERT(decimal(16,2), SUM(t.TotalMoney)) as EndPeriod,
	                convert(decimal(16,2),
	                sum(t.Amount)) as DTTL,
	                t.CreatedBy
                from 
                    TKBDAmounts t
                    inner join ApplicationUsers u
	                on t.CreatedBy = u.UserName
	                inner join PostOffices p
	                on u.POID = p.ID
	                inner join Districts d
	                on p.DistrictID = d.ID
                where t.Status=1 and t.Month=@month and t.Year=@year and d.ID = @districtId
                group by t.Month, t.CreatedBy, t.Year");
        }
        
        public override void Down()
        {
            DropStoredProcedure("Export_TKBD_By_Time_District");
        }
    }
}
