namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig18 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "Record_RecordId", "dbo.Records");
            DropIndex("dbo.Tasks", new[] { "Record_RecordId" });
            RenameColumn(table: "dbo.Tasks", name: "Record_RecordId", newName: "RecordId");
            AddColumn("dbo.Records", "Image", c => c.Binary());
            AlterColumn("dbo.Tasks", "RecordId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "RecordId");
            AddForeignKey("dbo.Tasks", "RecordId", "dbo.Records", "RecordId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "RecordId", "dbo.Records");
            DropIndex("dbo.Tasks", new[] { "RecordId" });
            AlterColumn("dbo.Tasks", "RecordId", c => c.Int());
            DropColumn("dbo.Records", "Image");
            RenameColumn(table: "dbo.Tasks", name: "RecordId", newName: "Record_RecordId");
            CreateIndex("dbo.Tasks", "Record_RecordId");
            AddForeignKey("dbo.Tasks", "Record_RecordId", "dbo.Records", "RecordId");
        }
    }
}
