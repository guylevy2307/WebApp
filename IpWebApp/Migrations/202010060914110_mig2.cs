namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        RecordId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        RenewalDate = c.DateTime(nullable: false),
                        RegistrationNumber = c.String(),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Records", "ClientId", "dbo.Clients");
            DropIndex("dbo.Records", new[] { "ClientId" });
            DropTable("dbo.Records");
        }
    }
}
