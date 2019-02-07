namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameInComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Author");
        }
    }
}
