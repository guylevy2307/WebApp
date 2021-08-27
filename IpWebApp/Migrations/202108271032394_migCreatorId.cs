namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migCreatorId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "creatorId", c => c.String());
            AddColumn("dbo.Clients", "creatorId", c => c.String());
            AddColumn("dbo.Subtasks", "creatorId", c => c.String());
            AddColumn("dbo.Tasks", "creatorId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "creatorId");
            DropColumn("dbo.Subtasks", "creatorId");
            DropColumn("dbo.Clients", "creatorId");
            DropColumn("dbo.Records", "creatorId");
        }
    }
}
