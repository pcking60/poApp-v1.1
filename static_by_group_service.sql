select st.Name, sum(sL.Money) as [sanluong], sum(st.Money) as [sotien], convert(decimal(16,2), (st.Money/sl.VAT)) as VAT from 
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
	where ps.Name like N'S?n l??ng%'
	group by s.Name, ps.name, s.VAT
	) sl
	join (select s.Name, sum(td.Money) as Money
	from ServiceGroups sg
	inner join Services s
	on sg.ID = s.GroupID
	inner join Transactions ts
	on s.ID = ts.ServiceId
	inner join TransactionDetails td
	on ts.ID = td.TransactionId
	inner join PropertyServices ps
	on td.PropertyServiceId = ps.ID
	where ps.Name like N'S? ti?n%' or ps.Name like N'Phí%'
	group by s.Name
	) st
	on st.Name = sl.Name
	group by st.Name, sl.Money, st.Money, sl.VAT