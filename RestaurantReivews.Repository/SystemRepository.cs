using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantReviews.Models;

namespace RestaurantReviews.Repository
{
    public class SystemRepository : ISystemRepository
    {
        private RestaurantReviewsContext context;

        public SystemRepository(string dbFilePath)
        {
            context = new RestaurantReviewsContext(dbFilePath);
        }

        public SystemRepository(RestaurantReviewsContext testContext)
        {
            context = testContext;
        }

        private SystemRepository()
        {
        }
        
        public void Dispose()
        {
            this.context?.Dispose();
        }

        public async Task<Restaurant> AddUpdateRestaurantAsync(Restaurant restaurant)
        {
            if (!context.Restaurant.Any(r => r.id == restaurant.id))
            {
                restaurant.dateCreated = DateTime.Now;
                await Task.Run(() => 
                               context.Restaurant.Add(restaurant));
                await Task.Run(() => 
                               context.Entry(restaurant).State = Microsoft.EntityFrameworkCore.EntityState.Added);

                if (restaurant.address != null)
                {
                    await Task.Run(() =>
                                   context.Entry(restaurant.address).State = Microsoft.EntityFrameworkCore.EntityState.Added);
                }
            }
            else
            {
                restaurant.dateModified = DateTime.Now;
                await Task.Run(() => 
                               context.Restaurant.Attach(restaurant));
                
                await Task.Run(() =>                 
                               context.Entry(restaurant).State = Microsoft.EntityFrameworkCore.EntityState.Modified);                       
                if (restaurant.address != null)
                {
                    await Task.Run(() =>
                                   context.Entry(restaurant.address).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                }
            }

            return restaurant;
        }

        public async Task<Restaurant> GetRestaurantAsync(Guid id)
        {
            return await Task.Run(() => GetRestaurant(id));
        }

        private Restaurant GetRestaurant(Guid id)
        {
            Restaurant restaurant = context.Restaurant.Find(id);
            if (restaurant != null)
            {
                context.Entry(restaurant).Collection(r => r.reviews).Load();

                int avgRating = 0;
                int ratingSum = 0;
                int ratingCount = 0;
                foreach(RestaurantReview review in restaurant.reviews)
                {
                    ratingSum += review.userRating;
                    ratingCount++;
                }
                avgRating = ratingCount > 0 ? ratingSum / ratingCount : 0;
                restaurant.averageUserRating = avgRating;
            }
            return restaurant;
        }

        public async Task<RestaurantReview> AddUpdateReviewAsync(RestaurantReview review)
        {
            if (review.restaurant != null && context.Restaurant.Any(r => r.id == review.restaurantId))
            {
                review.dateCreated = DateTime.Now;
                if (!context.RestaurantReview.Any(r => r.id == review.id))
                {
                    await Task.Run(() =>
                                   context.RestaurantReview.Add(review));

                    await Task.Run(() =>
                                   context.Entry(review).State = Microsoft.EntityFrameworkCore.EntityState.Added);
                    await Task.Run(() =>
                                       context.Entry(review.restaurant).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged);


                }
                else
                {
                    review.dateModified = DateTime.Now;
                    await Task.Run(() =>
                                   context.RestaurantReview.Attach(review));

                    await Task.Run(() =>
                                   context.Entry(review).State = Microsoft.EntityFrameworkCore.EntityState.Modified);
                    if (review.restaurant != null)
                    {
                        await Task.Run(() =>
                                       context.Entry(review.restaurant).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged);
                    }
                }
            }
            else 
            {
                throw new Exception("Each submitted review must reference an existing Restaurant");
            }

            return review;
        }

        public async Task<bool> DeleteReviewAsync(Guid reviewId)
        {
            bool _deleted = false;
            if (context.RestaurantReview.Any(r => r.id == reviewId))
            {
                RestaurantReview review = context.RestaurantReview.FirstOrDefault(r => r.id == reviewId);
                await Task.Run(() =>
                               context.Entry(review).State = Microsoft.EntityFrameworkCore.EntityState.Deleted);
                
                _deleted = true;
            }
            return _deleted;
        }

        public async Task<ICollection<Restaurant>> FindRestaurantsByCityStateAsync(string city, string state)
        {
            List<Restaurant> matches = new List<Restaurant>();

            var rs = await Task.Run(() => from r in context.Restaurant
                                          join a in context.RestaurantAddress on r.addressId equals a.id
                                    where (a.city.ToLower().Contains(city.ToLower()) &&
                                           (a.state.ToLower() == state.ToLower()))
                                          select r);
            
            if (rs != null)
            {
                foreach (var rest in rs)
                {
                    await context.Entry(rest).Reference(r => r.address).LoadAsync();
                    await context.Entry(rest).Collection(r => r.reviews).LoadAsync();
                    int avgRating = 0;
                    int ratingSum = 0;
                    int ratingCount = 0;
                    foreach(var review in rest.reviews)
                    {
                        ratingSum += review.userRating;
                        ratingCount++;
                    }
                    avgRating = ratingCount > 0 ? ratingSum / ratingCount : 0;
                    rest.averageUserRating = avgRating;
                }
                matches.AddRange(rs.ToList());
            }

            return matches;
        }


        public async Task<ICollection<RestaurantReview>> GetReviewsByUserAsync(string username)
        {
            List<RestaurantReview> reviews = new List<RestaurantReview>();

            if (context.RestaurantReview.Any(r => r.username.ToLower().Contains(username.ToLower())))
            {
                reviews = await Task.Run(() => context.RestaurantReview.Where(r => r.username.ToLower().Contains(username.ToLower())).ToList());
            }

            return reviews;
        }

        public async Task<ICollection<RestaurantReview>> GetReviewsForRestaurantAsync(Restaurant restaurant)
        {
            List<RestaurantReview> reviews = new List<RestaurantReview>();

            if (context.Restaurant.Any(r => r.id == restaurant.id))
            {
                await Task.Run(() => context.Restaurant.Attach(restaurant));
                await context.Entry(restaurant).Collection(r => r.reviews).LoadAsync();
                if (restaurant.reviews?.Count > 0)
                {
                    reviews.AddRange(restaurant.reviews);
                }
            }

            return reviews;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
