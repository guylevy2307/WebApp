namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subtasks", "Pricing", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subtasks", "Pricing");
        }
    }
}
