namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Export_TKBD_By_Time_District_PO : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
               "Export_TKBD_By_Time_District_PO",
               p => new
               {
                   month = p.Int(),
                   year = p.Int(),
                   districtId = p.Int(),
                   PoId = p.Int()
               },
               @"select
	                t.Month,
	                convert(int, count(t.Account)) as Quantity,
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
                where t.Status=1 and t.Month=@month and t.Year=@year and d.ID = @districtId and p.ID = @PoId
                group by t.Month, t.CreatedBy, t.Year");
        }
        
        public override void Down()
        {
            DropStoredProcedure("Export_TKBD_By_Time_District_PO");
        }
    }
}
