namespace N01522297_PassionProject_ExpiryDateTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<N01522297_PassionProject_ExpiryDateTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "N01522297_PassionProject_ExpiryDateTracker.Models.ApplicationDbContext";
        }

        protected override void Seed(N01522297_PassionProject_ExpiryDateTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
