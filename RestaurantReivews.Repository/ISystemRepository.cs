using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReviews.Models;

namespace RestaurantReviews.Repository
{
    /// <summary>
    /// System repository interface
    /// 
    /// Database methods used to accomplish the following:
    /// 1) Get a list of restaurants by city
    /// 2) Post a restaurant that is not in the database
    /// 3) Post a review for a restaurant
    /// 4) Get of a list of reviews by user
    /// 5) Delete a review
    /// </summary>
    public interface ISystemRepository : IDisposable
    {
        // 1) Get a list of restaurants by city
        Task<ICollection<Restaurant>> FindRestaurantsByCityStateAsync(string city, string state);

        // 2) Post a restaurant that is not in the database (also allows for updates)
        Task<Restaurant> AddUpdateRestaurantAsync(Restaurant restaurant);

        // BONUS: Get a single restaurant by id value
        Task<Restaurant> GetRestaurantAsync(Guid id);

        // 3) Post a review for a restaurant (also allows for updates)
        Task<RestaurantReview> AddUpdateReviewAsync(RestaurantReview review);

        // BONUS: allows for retrieval of reviews by restaurant (also included in Restaurant 
        //        objects found in item 1) above
        Task<ICollection<RestaurantReview>> GetReviewsForRestaurantAsync(Restaurant restaurant);

        // 4) Get of a list of reviews by user
        Task<ICollection<RestaurantReview>> GetReviewsByUserAsync(string username);

        // 5) Delete a review
        Task<bool> DeleteReviewAsync(Guid reviewId);

        // Saves all changes from UoW request
        Task SaveChangesAsync();

    }
}
