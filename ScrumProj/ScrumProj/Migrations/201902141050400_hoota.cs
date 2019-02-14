namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hoota : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PushNotes", "TypeOfNote", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PushNotes", "TypeOfNote");
        }
    }
}
