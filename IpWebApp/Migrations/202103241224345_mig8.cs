namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "PaymentIssued", c => c.Double());
            AddColumn("dbo.Clients", "PaymentReceived", c => c.Double());
            AddColumn("dbo.Records", "Inventor", c => c.String());
            AddColumn("dbo.Records", "Classes", c => c.String());
            AddColumn("dbo.Records", "ExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "ExpirationDate");
            DropColumn("dbo.Records", "Classes");
            DropColumn("dbo.Records", "Inventor");
            DropColumn("dbo.Clients", "PaymentReceived");
            DropColumn("dbo.Clients", "PaymentIssued");
        }
    }
}
