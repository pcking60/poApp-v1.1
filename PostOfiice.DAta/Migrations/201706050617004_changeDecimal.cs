namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PropertyServices", "Percent", c => c.Decimal(precision: 18, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PropertyServices", "Percent", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
