namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetkbdAmounts1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TKBDAmounts", "TotalMoney", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TKBDAmounts", "TotalMoney", c => c.String());
        }
    }
}
