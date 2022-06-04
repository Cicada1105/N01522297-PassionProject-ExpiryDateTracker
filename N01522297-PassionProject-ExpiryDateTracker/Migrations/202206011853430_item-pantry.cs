namespace N01522297_PassionProject_ExpiryDateTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itempantry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "PantryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "PantryID");
            AddForeignKey("dbo.Items", "PantryID", "dbo.Pantries", "PantryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "PantryID", "dbo.Pantries");
            DropIndex("dbo.Items", new[] { "PantryID" });
            DropColumn("dbo.Items", "PantryID");
        }
    }
}
