namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meetingtimes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeetingTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time1 = c.String(),
                        Time1Votes = c.Int(nullable: true),
                        Time2 = c.String(),
                        Time2Votes = c.Int(nullable: true),
                        Time3 = c.String(),
                        Time3Votes = c.Int(nullable: true),
                        Time4 = c.String(),
                        Time4Votes = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MeetingTimes");
        }
    }
}
