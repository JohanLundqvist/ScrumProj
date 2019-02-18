namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nyTabell : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        MeetingId = c.Int(nullable: false, identity: true),
                        MeetingTitle = c.String(nullable: false, maxLength: 30),
                        Time = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MeetingId);
            
            CreateTable(
                "dbo.MeetingParticipants",
                c => new
                    {
                        MeetingId = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.MeetingId, t.UserID })
                .ForeignKey("dbo.Meetings", t => t.MeetingId, cascadeDelete: true)
                .ForeignKey("dbo.ProfileModels", t => t.UserID, cascadeDelete: true)
                .Index(t => t.MeetingId)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetingParticipants", "UserID", "dbo.ProfileModels");
            DropForeignKey("dbo.MeetingParticipants", "MeetingId", "dbo.Meetings");
            DropIndex("dbo.MeetingParticipants", new[] { "UserID" });
            DropIndex("dbo.MeetingParticipants", new[] { "MeetingId" });
            DropTable("dbo.MeetingParticipants");
            DropTable("dbo.Meetings");
        }
    }
}
