namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig26Alon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "ParentId", c => c.Int());
            AddColumn("dbo.Records", "Parent_RecordId", c => c.Int());
         AddColumn("dbo.Tasks", "Assignee", c => c.String());
            CreateIndex("dbo.Records", "Parent_RecordId");
            AddForeignKey("dbo.Records", "Parent_RecordId", "dbo.Records", "RecordId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Records", "Parent_RecordId", "dbo.Records");
            DropIndex("dbo.Records", new[] { "Parent_RecordId" });
           DropColumn("dbo.Tasks", "Assignee");
            DropColumn("dbo.Records", "Parent_RecordId");
            DropColumn("dbo.Records", "ParentId");
        }
    }
}
