namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revenusStatistic : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("getRevenueStatistic",
                p => new {
                    fromDate = p.String(),
                    toDate = p.String()
                }
                ,
                @"select CAST(ts.TransactionDate as Date) as TransactionDate, sum(td.Money) as totalMoney
                from Transactions ts
                inner
                join TransactionDetails td
                on ts.ID = td.TransactionId
                inner
                join PropertyServices ps
                on ps.ID = td.PropertyServiceId
                inner
                join Services s
                on s.ID = ps.ServiceId
                where ts.Status=1 and ts.TransactionDate <= CAST(@toDate as Date) and ts.TransactionDate >= CAST(@fromDate as Date) and ps.Name not like N'Sản lượng'
                group by CAST(ts.TransactionDate as Date)");
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.getRevenueStatistic");
        }
    }
}

