namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class p : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WantMailOrNoes", "Project", c => c.Boolean(nullable: false));
            DropColumn("dbo.WantMailOrNoes", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WantMailOrNoes", "Category", c => c.String());
            DropColumn("dbo.WantMailOrNoes", "Project");
        }
    }
}
