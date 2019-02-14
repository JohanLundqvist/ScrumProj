namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fil_till_projekt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DevelopmentProjects", "UploadedFile_FileId", "dbo.Files");
            DropIndex("dbo.DevelopmentProjects", new[] { "UploadedFile_FileId" });
            CreateTable(
                "dbo.DevFiles",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        Content = c.Binary(),
                        Name = c.String(),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.DevelopmentProjects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            DropColumn("dbo.DevelopmentProjects", "UploadedFile_FileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DevelopmentProjects", "UploadedFile_FileId", c => c.Int());
            DropForeignKey("dbo.DevFiles", "ProjectId", "dbo.DevelopmentProjects");
            DropIndex("dbo.DevFiles", new[] { "ProjectId" });
            DropTable("dbo.DevFiles");
            CreateIndex("dbo.DevelopmentProjects", "UploadedFile_FileId");
            AddForeignKey("dbo.DevelopmentProjects", "UploadedFile_FileId", "dbo.Files", "FileId");
        }
    }
}
