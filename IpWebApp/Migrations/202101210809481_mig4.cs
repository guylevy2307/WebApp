namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subtasks",
                c => new
                    {
                        SubtaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Deadline = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        MainTaskId = c.Int(nullable: false),
                        MainTask_TaskId = c.Int(),
                    })
                .PrimaryKey(t => t.SubtaskId)
                .ForeignKey("dbo.Tasks", t => t.MainTask_TaskId)
                .Index(t => t.MainTask_TaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subtasks", "MainTask_TaskId", "dbo.Tasks");
            DropIndex("dbo.Subtasks", new[] { "MainTask_TaskId" });
            DropTable("dbo.Subtasks");
        }
    }
}
