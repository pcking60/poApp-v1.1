namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rank : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("rank",
                    p => new {
                        month1 = p.Int(),
                        month2 = p.Int()
                    },
                    @"select m2.CreatedBy,m2.total-m1.total as cl from (select MONTH, CreatedBy, SUM(TotalMoney) as total
                    from TKBDAmounts
                    where Month=@month1
                    group by Month, CreatedBy) m1
                    join (select MONTH, CreatedBy, SUM(TotalMoney)as total
                    from TKBDAmounts
                    where Month=@month2
                    group by Month, CreatedBy) m2
                    on m1.CreatedBy=m2.CreatedBy
                    order by cl desc");
        }
        
        public override void Down()
        {
            DropStoredProcedure("rank");
        }
    }
}
