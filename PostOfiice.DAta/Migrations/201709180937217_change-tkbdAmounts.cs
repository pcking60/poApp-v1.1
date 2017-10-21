namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetkbdAmounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TKBDAmounts", "TotalMoney", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TKBDAmounts", "TotalMoney");
        }
    }
}
