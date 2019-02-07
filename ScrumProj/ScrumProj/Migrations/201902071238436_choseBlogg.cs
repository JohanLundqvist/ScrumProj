namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class choseBlogg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "Formal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Entries", "Formal");
        }
    }
}
