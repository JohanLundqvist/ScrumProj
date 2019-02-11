namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tjo1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProfileModels", "DevelopmentProject_Id", "dbo.DevelopmentProjects");
            DropIndex("dbo.ProfileModels", new[] { "DevelopmentProject_Id" });
            CreateTable(
                "dbo.ProjectParticipants",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.UserID })
                .ForeignKey("dbo.DevelopmentProjects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.ProfileModels", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.UserID);
            
            DropColumn("dbo.ProfileModels", "DevelopmentProject_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProfileModels", "DevelopmentProject_Id", c => c.Int());
            DropForeignKey("dbo.ProjectParticipants", "UserID", "dbo.ProfileModels");
            DropForeignKey("dbo.ProjectParticipants", "ProjectId", "dbo.DevelopmentProjects");
            DropIndex("dbo.ProjectParticipants", new[] { "UserID" });
            DropIndex("dbo.ProjectParticipants", new[] { "ProjectId" });
            DropTable("dbo.ProjectParticipants");
            CreateIndex("dbo.ProfileModels", "DevelopmentProject_Id");
            AddForeignKey("dbo.ProfileModels", "DevelopmentProject_Id", "dbo.DevelopmentProjects", "Id");
        }
    }
}
