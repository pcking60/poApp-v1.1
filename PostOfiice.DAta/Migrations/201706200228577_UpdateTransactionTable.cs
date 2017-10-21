namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTransactionTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "IsCash", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "IsCash");
        }
    }
}
