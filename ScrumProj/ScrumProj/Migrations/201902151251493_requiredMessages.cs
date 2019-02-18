namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredMessages : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DevelopmentProjects", "Content", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DevelopmentProjects", "Content", c => c.String(maxLength: 1000));
        }
    }
}
