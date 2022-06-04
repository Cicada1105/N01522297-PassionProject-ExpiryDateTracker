namespace N01522297_PassionProject_ExpiryDateTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pantryuser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserFName = c.String(),
                        UserLName = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            AddColumn("dbo.Pantries", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Pantries", "UserID");
            AddForeignKey("dbo.Pantries", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pantries", "UserID", "dbo.Users");
            DropIndex("dbo.Pantries", new[] { "UserID" });
            DropColumn("dbo.Pantries", "UserID");
            DropTable("dbo.Users");
        }
    }
}
