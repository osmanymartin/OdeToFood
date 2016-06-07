using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OdeToFood.Models
{
    public interface IOdeToFoodDB : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
    }

    public class OdeToFoodDB3 : DbContext, IOdeToFoodDB
    {
        public OdeToFoodDB3()
            : base("name=DefaultConnection3")
        {

        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantReview> Reviews { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        IQueryable<T> IOdeToFoodDB.Query<T>()
        {
            return Set<T>();
        }
    }
}