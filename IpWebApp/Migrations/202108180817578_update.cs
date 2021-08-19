namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "PriorityCountry", c => c.String(nullable: false));
            DropColumn("dbo.Locations", "CityName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "CityName", c => c.String(nullable: false));
            DropColumn("dbo.Locations", "PriorityCountry");
        }
    }
}
