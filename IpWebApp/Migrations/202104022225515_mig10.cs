namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Client_ClientId", c => c.Int());
            CreateIndex("dbo.Tasks", "Client_ClientId");
            AddForeignKey("dbo.Tasks", "Client_ClientId", "dbo.Clients", "ClientId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Client_ClientId", "dbo.Clients");
            DropIndex("dbo.Tasks", new[] { "Client_ClientId" });
            DropColumn("dbo.Tasks", "Client_ClientId");
        }
    }
}
