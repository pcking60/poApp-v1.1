namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetkbdAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TKBDAmounts", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TKBDAmounts", "Year");
        }
    }
}
