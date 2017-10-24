namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unitStatistic : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("getUnitStatistic",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String()
                },
                @"select d.Name as Unit, sum(td.Money) as TotalMoney
                from Districts d
	            inner
	            join
	            PostOffices po
	            on d.ID = po.DistrictID
	            inner
	            join 
	            ApplicationUsers u 
	            on po.ID = u.POID
	            inner 
	            join 
	            Transactions ts 
	            on u.Id = ts.UserId
                inner
                join TransactionDetails td
                on ts.ID = td.TransactionId
                inner
                join PropertyServices ps
                on ps.ID = td.PropertyServiceId
                inner
                join Services s
                on s.ID = ps.ServiceId
                where ts.Status=1 and ts.CreatedDate <= CAST(@toDate as Date) and ts.CreatedDate >= CAST(@fromDate as Date) and ps.Name not like N'Sản lượng'
                group by d.Name");
        }
        
        public override void Down()
        {
            DropStoredProcedure("getUnitStatistic");
        }
    }
}
