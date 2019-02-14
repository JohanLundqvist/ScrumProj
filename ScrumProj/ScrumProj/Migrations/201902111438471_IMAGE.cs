namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IMAGE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entries", "ImgUrl", c => c.String());
        }
    }
}