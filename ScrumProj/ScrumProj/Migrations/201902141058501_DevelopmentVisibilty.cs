namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DevelopmentVisibilty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DevelopmentProjects", "Visibility", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DevelopmentProjects", "Visibility");
        }
    }
}
