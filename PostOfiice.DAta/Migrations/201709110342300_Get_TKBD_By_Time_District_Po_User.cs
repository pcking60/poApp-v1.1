namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Get_TKBD_By_Time_District_Po_User : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "Get_TKBD_By_Time_District_Po_User",
                p => new
                {
                    fromDate = p.String(),
                    toDate = p.String(),
                    districtId = p.Int(),
                    poId = p.Int(),
                    selectedUser = p.String()
                },
                @"select 
	                h.Id,
    	            h.Account,
    	            h.CustomerId,
    	            h.Name,
    	            convert(datetime,h.TransactionDate) as TransactionDate,
    	            convert(decimal(16,4), h.Money) as Money,
    	            convert(decimal(16,4), h.Rate) as Rate,
    	            h.CreatedBy,
    	            h.UserId,
					h.CreatedDate,
					h.MetaDescription,
					h.MetaKeyWord,
					h.Status,
					h.UpdatedBy,
					h.UpdatedDate,
                    u.FullName
                from 
	                TKBDHistories h
	                inner join ApplicationUsers u
	                on h.UserId = u.Id
	                inner join PostOffices p
	                on u.POID = p.ID	
	                inner join Districts d
	                on p.DistrictID = d.ID
                where 
	                h.Status=1 and h.UserId=@selectedUser and (h.TransactionDate>=CAST(@fromDate as date) and h.TransactionDate<=cast(@toDate as date)) and d.ID=@districtID and p.ID=@poId");
        }
        
        public override void Down()
        {
            DropStoredProcedure("Get_TKBD_By_Time_District_Po_User");
        }
    }
}
