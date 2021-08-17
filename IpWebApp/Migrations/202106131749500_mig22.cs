namespace IpWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig22 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        ApplicantId = c.Int(nullable: false, identity: true),
                        ApplicantName = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Formation = c.String(),
                        POA = c.String(),
                    })
                .PrimaryKey(t => t.ApplicantId);
            
            CreateTable(
                "dbo.RecordApplicants",
                c => new
                    {
                        Record_RecordId = c.Int(nullable: false),
                        Applicant_ApplicantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Record_RecordId, t.Applicant_ApplicantId })
                .ForeignKey("dbo.Records", t => t.Record_RecordId, cascadeDelete: true)
                .ForeignKey("dbo.Applicants", t => t.Applicant_ApplicantId, cascadeDelete: true)
                .Index(t => t.Record_RecordId)
                .Index(t => t.Applicant_ApplicantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecordApplicants", "Applicant_ApplicantId", "dbo.Applicants");
            DropForeignKey("dbo.RecordApplicants", "Record_RecordId", "dbo.Records");
            DropIndex("dbo.RecordApplicants", new[] { "Applicant_ApplicantId" });
            DropIndex("dbo.RecordApplicants", new[] { "Record_RecordId" });
            DropTable("dbo.RecordApplicants");
            DropTable("dbo.Applicants");
        }
    }
}
