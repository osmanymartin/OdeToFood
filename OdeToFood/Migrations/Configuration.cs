namespace OdeToFood.Migrations
{
    using OdeToFood.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<OdeToFood.Models.OdeToFoodDB3>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OdeToFood.Models.OdeToFoodDB3 context)
        {
            context.Restaurants.AddOrUpdate(
                r => r.Name,
                new Restaurant { Name = "LA mermelada", City = "Baltimore", Country = "USA" },
                 new Restaurant { Name = "LA chusmita", City = "jaguey", Country = "cuba" },
                new Restaurant
                {
                    Name = "chamaca",
                    City = "ginebra",
                    Country = "swuiden",
                    Reviews = new List<RestaurantReview> { 
                        new RestaurantReview { Rating = 9, Body = "Wonderful food!!", ReviewerName="Osmany" } }
                });

            //for (int i = 0; i < 1000; i++)
            //{
            //    context.Restaurants.AddOrUpdate(
            //        r => r.Name,
            //        new Restaurant { Name = i.ToString(), City = "Atlanta", Country = "USA" });
            //}

            SeedMembership();

        }

        private void SeedMembership()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection3",
                    "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("admin"))
            {
                roles.CreateRole("admin");
            }

            if (membership.GetUser("osmany", false) == null)
            {
                membership.CreateUserAndAccount("osmany", "osmany");
            }

            if (!roles.GetRolesForUser("osmany").Contains("admin"))
            {
                roles.AddUsersToRoles(new[] { "osmany" }, new[] { "admin" });
            }
        }
    }
}
