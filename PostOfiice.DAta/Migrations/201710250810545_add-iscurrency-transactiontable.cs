namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addiscurrencytransactiontable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "IsCurrency", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "IsCurrency");
        }
    }
}
