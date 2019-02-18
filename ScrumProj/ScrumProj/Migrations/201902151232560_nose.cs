namespace ScrumProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nose : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WantMailOrNoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogPost = c.Boolean(nullable: false),
                        Mail = c.Boolean(nullable: false),
                        Sms = c.Boolean(nullable: false),
                        Category = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WantMailOrNoes");
        }
    }
}
