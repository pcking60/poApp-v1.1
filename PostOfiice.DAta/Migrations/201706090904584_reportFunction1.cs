namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class reportFunction1 : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("reportFunction1", p => new {
                fromDate = p.String(),
                toDate = p.String()
            },
            @"select sg.Name, sum(Money) as TotalMoney, cast(s.VAT as decimal(18,2)),cast((sum(Money)-sum(Money)/VAT) as decimal(18,4)) as Tax, cast((sum(Money)/VAT) as decimal(18,4)) as Revenue
            from Transactions t
            inner join TransactionDetails td 
            on t.ID=td.TransactionId
            inner join Services s
            on s.ID = t.ServiceId
            inner join ServiceGroups sg
            on s.GroupID = sg.ID
            where t.CreatedDate>=CAST(@fromDate as date) and t.CreatedDate<=cast(@toDate as date)
            group by sg.Name, s.VAT"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("reportFunction1");
        }
    }
}
