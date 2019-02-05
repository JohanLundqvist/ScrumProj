namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorOnEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Entries", "Author");
        }
    }
}
