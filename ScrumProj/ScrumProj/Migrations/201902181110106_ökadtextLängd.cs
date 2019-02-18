namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ökadtextLängd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DevelopmentProjects", "Content", c => c.String(nullable: false, maxLength: 1500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DevelopmentProjects", "Content", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
