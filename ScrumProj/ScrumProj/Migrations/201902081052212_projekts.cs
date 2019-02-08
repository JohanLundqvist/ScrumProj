namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projekts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DevelopmentProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Content = c.String(maxLength: 1000),
                        Cat = c.Int(nullable: false),
                        UploadedFile_FileId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Files", t => t.UploadedFile_FileId)
                .Index(t => t.UploadedFile_FileId);
            
            AddColumn("dbo.ProfileModels", "DevelopmentProject_Id", c => c.Int());
            CreateIndex("dbo.ProfileModels", "DevelopmentProject_Id");
            AddForeignKey("dbo.ProfileModels", "DevelopmentProject_Id", "dbo.DevelopmentProjects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DevelopmentProjects", "UploadedFile_FileId", "dbo.Files");
            DropForeignKey("dbo.ProfileModels", "DevelopmentProject_Id", "dbo.DevelopmentProjects");
            DropIndex("dbo.DevelopmentProjects", new[] { "UploadedFile_FileId" });
            DropIndex("dbo.ProfileModels", new[] { "DevelopmentProject_Id" });
            DropColumn("dbo.ProfileModels", "DevelopmentProject_Id");
            DropTable("dbo.DevelopmentProjects");
        }
    }
}
