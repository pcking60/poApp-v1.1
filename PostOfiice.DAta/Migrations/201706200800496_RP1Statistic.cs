namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RP1Statistic : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "RP1",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String(),
                    districtId = p.Int(),
                    unitId = p.Int()
                },
                @"if(@districtId=0 and @unitId=0)
		            begin
			            (select row_number() over(order by sg.ID) as Id, sg.Name, cast((sum(Money)/VAT) as decimal(18,4)) as Revenue, cast((sum(Money)-sum(Money)/VAT) as decimal(18,4)) as Tax, sum(Money) as TotalMoney
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
			            where cast(t.CreatedDate as date)>=CAST(@fromDate as date) and cast(t.CreatedDate as date)<=cast(@toDate as date) 
			            group by sg.Name, s.VAT, sg.ID)
		            end
	            else
	            begin
		            if(@districtId=0)
		            begin
			            (select row_number() over(order by sg.ID) as Id, sg.Name, cast((sum(Money)/VAT) as decimal(18,4)) as Revenue, cast((sum(Money)-sum(Money)/VAT) as decimal(18,4)) as Tax, sum(Money) as TotalMoney
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
			            where cast(t.CreatedDate as date)>=CAST(@fromDate as date) and cast(t.CreatedDate as date)<=cast(@toDate as date) and p.ID=@unitId
			            group by sg.Name, s.VAT, sg.ID)
		            end
		            else
		            begin
			            if(@unitId=0)
			            begin
				            (select row_number() over(order by sg.ID) as Id, sg.Name, cast((sum(Money)/VAT) as decimal(18,4)) as Revenue, cast((sum(Money)-sum(Money)/VAT) as decimal(18,4)) as Tax, sum(Money) as TotalMoney
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
				            where cast(t.CreatedDate as date)>=CAST(@fromDate as date) and cast(t.CreatedDate as date)<=cast(@toDate as date) and d.ID=@districtId
				            group by sg.Name, s.VAT, sg.ID)
			            end
			            else
			            begin
				            (select row_number() over(order by sg.ID) as Id, sg.Name, cast((sum(Money)/VAT) as decimal(18,4)) as Revenue, cast((sum(Money)-sum(Money)/VAT) as decimal(18,4)) as Tax, sum(Money) as TotalMoney
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
				            where cast(t.CreatedDate as date)>=CAST(@fromDate as date) and cast(t.CreatedDate as date)<=cast(@toDate as date) and d.ID=@districtId and p.ID=@unitId
				            group by sg.Name, s.VAT, sg.ID)
			            end
		            end
	            end"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("RP1");
        }
    }
}
