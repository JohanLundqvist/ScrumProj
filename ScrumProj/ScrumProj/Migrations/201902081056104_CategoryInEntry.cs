namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryInEntry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryInEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntryId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CategoryInEntries");
        }
    }
}
