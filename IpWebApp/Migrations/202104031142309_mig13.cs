namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "VatNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "VatNumber");
        }
    }
}
