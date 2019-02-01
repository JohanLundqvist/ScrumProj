namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fulkod : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Entries", "file_FileId", "dbo.Files");
            DropIndex("dbo.Entries", new[] { "file_FileId" });
            AddColumn("dbo.Entries", "fileId", c => c.Int(nullable: true));
            DropColumn("dbo.Entries", "file_FileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entries", "file_FileId", c => c.Int());
            DropColumn("dbo.Entries", "fileId");
            CreateIndex("dbo.Entries", "file_FileId");
            AddForeignKey("dbo.Entries", "file_FileId", "dbo.Files", "FileId");
        }
    }
}
