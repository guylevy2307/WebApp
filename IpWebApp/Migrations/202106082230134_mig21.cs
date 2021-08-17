namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "ContactPosition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "ContactPosition");
        }
    }
}
