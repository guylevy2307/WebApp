namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "BillingName", c => c.String());
            AddColumn("dbo.Clients", "BillingEmail", c => c.String());
            AddColumn("dbo.Clients", "Currency", c => c.String());
            AddColumn("dbo.Clients", "Referent", c => c.String());
            AddColumn("dbo.Clients", "Balance", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "Pricing", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Pricing");
            DropColumn("dbo.Clients", "Balance");
            DropColumn("dbo.Clients", "Referent");
            DropColumn("dbo.Clients", "Currency");
            DropColumn("dbo.Clients", "BillingEmail");
            DropColumn("dbo.Clients", "BillingName");
        }
    }
}
