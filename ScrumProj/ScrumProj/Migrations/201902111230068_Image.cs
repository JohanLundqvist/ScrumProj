namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Image : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "ImgUrl", c => c.String());
            DropColumn("dbo.Entries", "image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entries", "image", c => c.Binary());
            DropColumn("dbo.Entries", "ImgUrl");
        }
    }
}
