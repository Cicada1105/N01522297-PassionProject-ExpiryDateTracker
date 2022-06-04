namespace N01522297_PassionProject_ExpiryDateTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pantries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pantries",
                c => new
                    {
                        PantryId = c.Int(nullable: false, identity: true),
                        PantryName = c.String(),
                    })
                .PrimaryKey(t => t.PantryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pantries");
        }
    }
}
