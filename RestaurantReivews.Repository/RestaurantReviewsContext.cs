using System;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore;
using RestaurantReviews.Models;

namespace RestaurantReviews.Repository
{
    public class RestaurantReviewsContext : DbContext
    {
        string _dbFilePath;

        private RestaurantReviewsContext()
        {
            // force instantiating with another constructor
        }

        public RestaurantReviewsContext(string dbFilePath)
        {
            _dbFilePath = dbFilePath;
            Database.EnsureCreated();
        }

        public RestaurantReviewsContext(DbContextOptions<RestaurantReviewsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename ={_dbFilePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Restaurant> Restaurant { get; set; }

        public DbSet<RestaurantReview> RestaurantReview { get; set; }

        public DbSet<RestaurantAddress> RestaurantAddress { get; set; }
    }
}
