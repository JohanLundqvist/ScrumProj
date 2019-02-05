namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enyfileeeh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entries", "Title", c => c.String());
            AddColumn("dbo.Entries", "image", c => c.Binary());
            AddColumn("dbo.Entries", "file_FileId", c => c.Int());
            CreateIndex("dbo.Entries", "file_FileId");
            AddForeignKey("dbo.Entries", "file_FileId", "dbo.Files", "FileId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entries", "file_FileId", "dbo.Files");
            DropIndex("dbo.Entries", new[] { "file_FileId" });
            DropColumn("dbo.Entries", "file_FileId");
            DropColumn("dbo.Entries", "image");
            DropColumn("dbo.Entries", "Title");
        }
    }
}
