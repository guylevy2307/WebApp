namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatorMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applicants", "creatorId", c => c.String());
            AddColumn("dbo.Locations", "creatorId", c => c.String());
            AddColumn("dbo.TamplateTasks", "creatorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TamplateTasks", "creatorId");
            DropColumn("dbo.Locations", "creatorId");
            DropColumn("dbo.Applicants", "creatorId");
        }
    }
}
