namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableTKBDHistory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TKBDHistories", "TransactionDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TKBDHistories", "TransactionDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
