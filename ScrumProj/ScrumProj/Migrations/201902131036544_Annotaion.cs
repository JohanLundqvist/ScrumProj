namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Annotaion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entries", "Content", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entries", "Content", c => c.String(maxLength: 1000));
        }
    }
}
