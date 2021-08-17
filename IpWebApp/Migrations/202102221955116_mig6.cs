namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "ContactName", c => c.String());
            AddColumn("dbo.Clients", "ContactEmail", c => c.String());
            AddColumn("dbo.Records", "ApplicationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Records", "NextActionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Clients", "Currency", c => c.Int());
            DropColumn("dbo.Clients", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Email", c => c.String());
            AlterColumn("dbo.Clients", "Currency", c => c.String());
            DropColumn("dbo.Records", "NextActionDate");
            DropColumn("dbo.Records", "ApplicationDate");
            DropColumn("dbo.Clients", "ContactEmail");
            DropColumn("dbo.Clients", "ContactName");
        }
    }
}
