namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class exporttcbcbytimedistrict : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "Export_TCBC_BY_TIME_DISTRICT",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String(),
                    districtId = p.Int()
                },
                @"select
		            rs.ServiceName,
		            rs.Quantity,
		            rs.VAT,
		            sum(rs.Fee) as Fee,
		            sum(rs.TotalColection) as [TotalColection],
		            sum(rs.TotalPay) as [TotalPay],
		            sum(rs.Sales) - sum(rs.Sales)/rs.VAT as Tax,
		            sum(rs.EarnMoney) as EarnMoney
                from
                (select
		            sl.Name as ServiceName,
		            convert(int,sum(sl.Money)) as [Quantity],
		            sl.VAT as VAT,
		            ISNULL(SUM(st3.money),0) as Fee,
		            ISNULL(sum(st.Money),0) as [TotalColection],
		            ISNULL(sum(st1.Money),0) as [TotalPay],
		            convert(decimal(16,2),ISNULL(st.Money *st.[Percent],0) + ISNULL(st1.Money *st1.[Percent],0) + Isnull(sl.Money*sl.[Percent],0) + ISNULL(st3.Money *st3.[Percent],0)) as Sales,
		            convert(decimal(16,2),(ISNULL(st.Money *st.[Percent],0)+ISNULL(st1.Money *st1.[Percent],0))/ISNULL(sl.VAT,0) + (sl.Money/sl.VAT)*sl.[Percent] + ISNULL(st3.Money *st3.[Percent],0)/sl.VAT) as EarnMoney
    			from
    	            (select s.Name, sum(td.Money) as Money, ps.[Percent]
    	            from ServiceGroups sg
    	                inner join Services s
    	                on sg.ID = s.GroupID
    	                inner join Transactions ts
    	                on s.ID = ts.ServiceId
    	                inner join TransactionDetails td
    	                on ts.ID = td.TransactionId
    	                inner join PropertyServices ps
    	                on td.PropertyServiceId = ps.ID
                        inner join ApplicationUsers u
    	                on ts.UserId = u.Id
    	                inner join PostOffices p
    	                on u.POID = p.ID
    	                inner join Districts d
    	                on p.DistrictID = d.ID
    	            where
							(ps.Name like N'Số tiền%' or ps.Name like N'Phí%')
							and ps.Name not like N'%cước%'
							and ts.Status=1 and ts.IsCash=1 and sg.MainServiceGroupId=3 and (sg.ID=93 or sg.ID=75) and CAST( ts.TransactionDate as date) between CAST(@fromDate as date) and CAST(@toDate as date) and d.ID=@districtId
    	                group by s.Name, ps.[Percent]
    	                ) st
    	            full outer join
    	            (select s.Name, sum(td.Money) as Money, ps.Name as PName, s.VAT, ps.[Percent]
    	            from ServiceGroups sg
    	                inner join Services s
    	                on sg.ID = s.GroupID
    	                inner join Transactions ts
    	                on s.ID = ts.ServiceId
    	                inner join TransactionDetails td
    	                on ts.ID = td.TransactionId
    	                inner join PropertyServices ps
    	                on td.PropertyServiceId = ps.ID
    	                inner join ApplicationUsers u
    	                on ts.UserId = u.Id
    	                inner join PostOffices p
    	                on u.POID = p.ID
    	                inner join Districts d
    	                on p.DistrictID = d.ID
    	            where ps.Name like N'Sản lượng%' and ts.Status=1 and sg.MainServiceGroupId=3 and CAST( ts.TransactionDate as date) between CAST(@fromDate as date) and CAST(@toDate as date) and d.ID=@districtId
    	            group by s.Name, ps.name, s.VAT, ps.[Percent]
    	                ) sl
    	            on sl.Name = st.Name
    	            full outer join
    	                (select s.Name, sum(td.Money) as Money, ps.[Percent]
    	            from ServiceGroups sg
    	                inner join Services s
    	                on sg.ID = s.GroupID
    	                inner join Transactions ts
    	                on s.ID = ts.ServiceId
    	                inner join TransactionDetails td
    	                on ts.ID = td.TransactionId
    	                inner join PropertyServices ps
    	                on td.PropertyServiceId = ps.ID
                        inner join ApplicationUsers u
    	                on ts.UserId = u.Id
    	                inner join PostOffices p
    	                on u.POID = p.ID
    	                inner join Districts d
    	                on p.DistrictID = d.ID
    	            where (ps.Name like N'Số tiền%' or ps.Name like N'Phí%')
							and ps.Name not like N'%cước%'
							and ts.Status=1 and ts.IsCash=1 and sg.MainServiceGroupId=3 and sg.ID=94 and CAST( ts.TransactionDate as date) between CAST(@fromDate as date) and CAST(@toDate as date) and d.ID=@districtId
    	            group by s.Name, ps.[Percent]
    	                ) st1
    	            on sl.Name = st1.name
					full outer join
					(select s.Name, sum(td.Money) as Money, ps.[Percent]
    	                from ServiceGroups sg
    	                inner join Services s
    	                on sg.ID = s.GroupID
    	                inner join Transactions ts
    	                on s.ID = ts.ServiceId
    	                inner join TransactionDetails td
    	                on ts.ID = td.TransactionId
    	                inner join PropertyServices ps
    	                on td.PropertyServiceId = ps.ID
                        inner join ApplicationUsers u
    	                on ts.UserId = u.Id
    	                inner join PostOffices p
    	                on u.POID = p.ID
    	                inner join Districts d
    	                on p.DistrictID = d.ID
    	            where
							ps.Name like N'%cước%'
							and ts.Status=1 and ts.IsCash=1 and sg.MainServiceGroupId=3 and (sg.ID=93 or sg.ID=75) and CAST( ts.TransactionDate as date) between CAST(@fromDate as date) and CAST(@toDate as date) and d.ID=@districtId
    	                group by s.Name, ps.[Percent]
    	                ) st3
					on sl.Name = st3.Name
    	        group by sl.Name, sl.Money, st.Money, st1.Money, sl.VAT, st1.[Percent], st.[Percent], sl.[Percent], st3.Name, st3.[Percent], st3.Money) rs
            group by rs.ServiceName, rs.Quantity, rs.VAT");
        }

        public override void Down()
        {
            DropStoredProcedure("Export_TCBC_BY_TIME_DISTRICT");
        }
    }
}