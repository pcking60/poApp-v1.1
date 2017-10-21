select sl.Name, sum(sL.Money) as [sanluong], sum(st.Money) as [cash],convert(decimal(16,2), (st.Money/sl.VAT)) as VAT from 
	(select s.Name, sum(td.Money) as Money, ps.Name as PName, s.VAT
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
	where ps.Name like N'Sản lượng%' and ts.Status=1 and sg.MainServiceGroupId=1 
	group by s.Name, ps.name, s.VAT
	) sl	
	full outer join 
	(select s.Name, sum(td.Money) as Money
	from ServiceGroups sg
	inner join Services s
	on sg.ID = s.GroupID
	inner join Transactions ts
	on s.ID = ts.ServiceId
	inner join TransactionDetails td
	on ts.ID = td.TransactionId
	inner join PropertyServices ps
	on td.PropertyServiceId = ps.ID
	where (ps.Name like N'Số tiền%' or ps.Name like N'Phí%') and ts.Status=1 and ts.IsCash=0 and sg.MainServiceGroupId=1 
	group by s.Name
	) st	
	on st.Name = sl.Name
		group by sl.Name, sl.Money, st.Money ,sl.VAT