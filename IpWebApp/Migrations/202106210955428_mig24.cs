namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "Priority", c => c.Boolean(nullable: false));
            AddColumn("dbo.Records", "PriorityCountry", c => c.String());
            AddColumn("dbo.Records", "PriorityNumber", c => c.String());
            AddColumn("dbo.Records", "PriorityDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "PriorityDate");
            DropColumn("dbo.Records", "PriorityNumber");
            DropColumn("dbo.Records", "PriorityCountry");
            DropColumn("dbo.Records", "Priority");
        }
    }
}
