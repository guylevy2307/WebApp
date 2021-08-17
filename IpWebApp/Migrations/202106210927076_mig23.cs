namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "Notes");
        }
    }
}
