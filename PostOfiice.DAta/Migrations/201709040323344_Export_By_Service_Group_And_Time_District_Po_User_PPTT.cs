namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Export_By_Service_Group_And_Time_District_Po_User_PPTT : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                   "Export_By_Service_Group_And_Time_District_Po_User_PPTT",
                   p => new
                   {
                       fromDate = p.String(),
                       toDate = p.String(),
                       districtId = p.Int(),
                       poId = p.Int(),
                       userId = p.String()
                   },
                   @"select                          
                        sl.Name as ServiceName, sum(sl.Money) as [Quantity], 				        
                        sl.VAT as VAT,
                        ISNULL(sum(st1.Money),0) as [TotalCash], 
				        ISNULL(convert(decimal(16,2), (st1.Money - st1.Money/sl.VAT)),0) as VatOfTotalCash,
				        ISNULL(sum(st.Money),0) as [TotalDebt], 
				        ISNULL(convert(decimal(16,2), (st.Money - st.Money/sl.VAT)),0) as VatOfTotalDebt, 
				        convert(decimal(16,2),(ISNULL(st.Money *st.[Percent],0)+ISNULL(st1.Money *st1.[Percent],0))/ISNULL(sl.VAT,0) + (sl.Money/sl.VAT)*sl.[Percent]) as EarnMoney				        
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
	                    where (ps.Name like N'Số tiền%' or ps.Name like N'Phí%') and ts.Status=1 and ts.IsCash=0 and sg.MainServiceGroupId=2 and CAST( ts.TransactionDate as date) between CAST(@fromDate as date) and CAST(@toDate as date) and d.ID=@districtId and p.ID=@poId and u.Id=@userId
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
	                    where ps.Name like N'Sản lượng%' and ts.Status=1 and sg.MainServiceGroupId=2 and CAST( ts.TransactionDate as date) between CAST(@fromDate as date) and CAST(@toDate as date) and d.ID=@districtId and p.ID=@poId and u.Id=@userId
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
	                    where (ps.Name like N'Số tiền%' or ps.Name like N'Phí%') and ts.Status=1 and ts.IsCash=1 and sg.MainServiceGroupId=2 and CAST( ts.TransactionDate as date) between CAST(@fromDate as date) and CAST(@toDate as date) and d.ID=@districtId and p.ID=@poId and u.Id=@userId
	                    group by s.Name, ps.[Percent]
	                    ) st1
	                on sl.Name = st1.name
	            group by sl.Name, sl.Money, st.Money, st1.Money, sl.VAT, st1.[Percent], st.[Percent], sl.[Percent]");
        }
        
        public override void Down()
        {
            DropStoredProcedure("Export_By_Service_Group_And_Time_District_Po_User_PPTT");
        }
    }
}
