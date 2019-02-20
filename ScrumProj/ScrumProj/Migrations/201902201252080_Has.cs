namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Has : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MeetingTimes", "MeetingId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MeetingTimes", "MeetingId");
        }
    }
}
