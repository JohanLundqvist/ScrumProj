namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HasVodetdOrNot : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HasVotedOrNoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        MeetingId = c.Int(nullable: false),
                        Hasvoted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HasVotedOrNoes");
        }
    }
}
