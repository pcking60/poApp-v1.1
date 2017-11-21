namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsomecolumnhistorytkbd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TKBDHistories", "Month", c => c.Int());
            AddColumn("dbo.TKBDHistories", "Year", c => c.Int());
            AddColumn("dbo.TKBDHistories", "TimeCode", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TKBDHistories", "TimeCode");
            DropColumn("dbo.TKBDHistories", "Year");
            DropColumn("dbo.TKBDHistories", "Month");
        }
    }
}
