namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Notes");
        }
    }
}
