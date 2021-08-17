namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Records", "RenewalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Records", "RenewalDate", c => c.DateTime(nullable: false));
        }
    }
}
