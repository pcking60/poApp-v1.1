namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class reportFunction1_2 : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "reportFunction1_2",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String(),
                    districtId = p.Int(),
                    unitId = p.Int()
                },
                @"select sg.Name, sum(Money) as TotalMoney, cast(s.VAT as decimal(18,2)),cast((sum(Money)-sum(Money)/VAT) as decimal(18,4)) as Tax, cast((sum(Money)/VAT) as decimal(18,4)) as Revenue
                from Districts d
	            inner join PostOffices p
	            on d.ID= p.DistrictID
	            inner join ApplicationUsers u
	            on p.ID = u.POID
	            inner join Transactions t
	            on u.Id = t.UserId
                inner join TransactionDetails td 
                on t.ID=td.TransactionId
                inner join Services s
                on s.ID = t.ServiceId
                inner join ServiceGroups sg
                on s.GroupID = sg.ID
                where t.CreatedDate>=CAST(@fromDate as date) and t.CreatedDate<=cast(@toDate as date) and d.ID=@districtId and p.ID=@unitId 
                group by sg.Name, s.VAT");
        }

        public override void Down()
        {
            DropStoredProcedure("reportFunction1_2");
        }
    }
}
