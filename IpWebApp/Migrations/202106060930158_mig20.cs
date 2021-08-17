namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TamplateTasks", "dateType", c => c.Int(nullable: false));
            DropColumn("dbo.TamplateTasks", "RecoredType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TamplateTasks", "RecoredType", c => c.Int(nullable: false));
            DropColumn("dbo.TamplateTasks", "dateType");
        }
    }
}
