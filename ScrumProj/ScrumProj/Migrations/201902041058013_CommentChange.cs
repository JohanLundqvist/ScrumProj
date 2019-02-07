namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Entry_Id", "dbo.Entries");
            DropIndex("dbo.Comments", new[] { "Entry_Id" });
            AddColumn("dbo.Comments", "EntryId", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "Entry_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Entry_Id", c => c.Int());
            DropColumn("dbo.Comments", "EntryId");
            CreateIndex("dbo.Comments", "Entry_Id");
            AddForeignKey("dbo.Comments", "Entry_Id", "dbo.Entries", "Id");
        }
    }
}
