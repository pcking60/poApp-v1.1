namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetkbdhistory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TKBDHistories", "TransactionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TKBDHistories", "TransactionDate", c => c.DateTimeOffset(precision: 7));
        }
    }
}
