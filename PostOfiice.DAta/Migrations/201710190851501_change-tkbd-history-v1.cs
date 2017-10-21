namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetkbdhistoryv1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TKBDHistories", "CustomerName", c => c.String());
            AddColumn("dbo.TKBDHistories", "ServiceId", c => c.String());
            DropColumn("dbo.TKBDHistories", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TKBDHistories", "Name", c => c.String());
            DropColumn("dbo.TKBDHistories", "ServiceId");
            DropColumn("dbo.TKBDHistories", "CustomerName");
        }
    }
}
