namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Deadline = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Record_RecordId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Records", t => t.Record_RecordId)
                .Index(t => t.Record_RecordId);
            
            AddColumn("dbo.Records", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Records", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Record_RecordId", "dbo.Records");
            DropIndex("dbo.Tasks", new[] { "Record_RecordId" });
            DropColumn("dbo.Records", "Country");
            DropColumn("dbo.Records", "Type");
            DropTable("dbo.Tasks");
        }
    }
}
