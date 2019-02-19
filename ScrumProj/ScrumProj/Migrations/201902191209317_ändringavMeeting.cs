namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ändringavMeeting : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Meetings", "Time", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Meetings", "Time", c => c.DateTime(nullable: false));
        }
    }
}
