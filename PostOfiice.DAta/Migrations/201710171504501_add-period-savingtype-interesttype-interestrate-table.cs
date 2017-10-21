namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addperiodsavingtypeinteresttypeinterestratetable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestRates",
                c => new
                    {
                        PeriodId = c.String(nullable: false, maxLength: 128),
                        InterestTypeId = c.String(nullable: false, maxLength: 128),
                        SavingTypeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PeriodId, t.InterestTypeId, t.SavingTypeId })
                .ForeignKey("dbo.InterestTypes", t => t.InterestTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .ForeignKey("dbo.SavingTypes", t => t.SavingTypeId, cascadeDelete: true)
                .Index(t => t.PeriodId)
                .Index(t => t.InterestTypeId)
                .Index(t => t.SavingTypeId);
            
            CreateTable(
                "dbo.InterestTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SavingTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestRates", "SavingTypeId", "dbo.SavingTypes");
            DropForeignKey("dbo.InterestRates", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.InterestRates", "InterestTypeId", "dbo.InterestTypes");
            DropIndex("dbo.InterestRates", new[] { "SavingTypeId" });
            DropIndex("dbo.InterestRates", new[] { "InterestTypeId" });
            DropIndex("dbo.InterestRates", new[] { "PeriodId" });
            DropTable("dbo.SavingTypes");
            DropTable("dbo.Periods");
            DropTable("dbo.InterestTypes");
            DropTable("dbo.InterestRates");
        }
    }
}
