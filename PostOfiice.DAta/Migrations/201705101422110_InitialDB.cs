namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PropertyServices", new[] { "ServiceId" });
            DropIndex("dbo.TransactionDetails", new[] { "TransactionId" });
            AddColumn("dbo.Services", "PayMethodID", c => c.Int());
            AddColumn("dbo.TransactionDetails", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "ServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "TransactionDetailId", c => c.Int(nullable: false));
            AlterColumn("dbo.PropertyServices", "Percent", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.PropertyServices", "ServiceID");
            CreateIndex("dbo.TransactionDetails", "TransactionID");
            DropColumn("dbo.TransactionDetails", "Money");
            DropColumn("dbo.Transactions", "Quantity");
            DropTable("dbo.PropertyPOs");
            DropTable("dbo.PropertyServiceDetails");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PropertyServiceDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Money = c.Decimal(precision: 18, scale: 2),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        MetaDescription = c.String(),
                        MetaKeyWord = c.String(),
                        Status = c.Boolean(nullable: false),
                        UpdatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PropertyPOs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Transactions", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.TransactionDetails", "Money", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropIndex("dbo.TransactionDetails", new[] { "TransactionID" });
            DropIndex("dbo.PropertyServices", new[] { "ServiceID" });
            AlterColumn("dbo.PropertyServices", "Percent", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Transactions", "TransactionDetailId");
            DropColumn("dbo.Transactions", "ServiceId");
            DropColumn("dbo.TransactionDetails", "Quantity");
            DropColumn("dbo.Services", "PayMethodID");
            CreateIndex("dbo.TransactionDetails", "TransactionId");
            CreateIndex("dbo.PropertyServices", "ServiceId");
        }
    }
}
