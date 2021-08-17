namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TamplateTasks",
                c => new
                    {
                        TamplateTaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        AddDays = c.Int(nullable: false),
                        AddMonths = c.Int(nullable: false),
                        AddYears = c.Int(nullable: false),
                        Pricing = c.Double(nullable: false),
                        RecoredType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TamplateTaskId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TamplateTasks");
        }
    }
}
