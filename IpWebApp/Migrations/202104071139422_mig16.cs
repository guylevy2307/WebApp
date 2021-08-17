namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig16 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Records", "ApplicationDate", c => c.DateTime());
            AlterColumn("dbo.Records", "RegistrationDate", c => c.DateTime());
            AlterColumn("dbo.Records", "NextActionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Records", "NextActionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Records", "RegistrationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Records", "ApplicationDate", c => c.DateTime(nullable: false));
        }
    }
}
