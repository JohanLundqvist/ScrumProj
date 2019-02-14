namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PushNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note = c.String(),
                        ProfileModelId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ProfileModels", "NewPushNote", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProfileModels", "NewPushNote");
            DropTable("dbo.PushNotes");
        }
    }
}
