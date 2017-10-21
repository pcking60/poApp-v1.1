namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TKBD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TKBDAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Month = c.Int(nullable: false),
                        Account = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                "dbo.TKBDHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(),
                        Name = c.String(),
                        TransactionDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Money = c.Decimal(precision: 18, scale: 2),
                        Rate = c.Decimal(precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(),
                        MetaDescription = c.String(),
                        MetaKeyWord = c.String(),
                        Status = c.Boolean(nullable: false),
                        UpdatedBy = c.String(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Id, t.Account })
                .ForeignKey("dbo.ApplicationUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TKBDHistories", "UserId", "dbo.ApplicationUsers");
            DropIndex("dbo.TKBDHistories", new[] { "UserId" });
            DropTable("dbo.TKBDHistories");
            DropTable("dbo.TKBDAmounts");
        }
    }
}
