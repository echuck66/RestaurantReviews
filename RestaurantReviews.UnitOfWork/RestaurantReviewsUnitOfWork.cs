using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantReviews.Models;
using RestaurantReviews.Repository;


namespace RestaurantReviews.UnitOfWork
{
    public class RestaurantReviewsUnitOfWork : IDisposable
    {
        ISystemRepository repository;

        public RestaurantReviewsUnitOfWork(string dbFilePath)
        {
            repository = new SystemRepository(dbFilePath);
        }

        public RestaurantReviewsUnitOfWork(ISystemRepository dbRepository)
        {
            repository = dbRepository;
        }

        public void Dispose()
        {
            this.repository?.Dispose();
        }

        /// <summary>
        /// The following method could be used to get a list of reviews by restaurant
        /// </summary>
        //public async Task<GetReviewsResults> GetRestaurantReviewsAsync(Restaurant restaurant)
        //{
        //    GetReviewsResults _results = new GetReviewsResults();
        //    try
        //    {
        //        var reviews = await this.repository.GetReviewsForRestaurantAsync(restaurant);
        //        _results.Results = reviews;
        //        _results.Successful = true;
        //        if (reviews.Count == 0)
        //        {
        //            _results.Message = "No reviews found for this restaurant";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _results.Successful = false;
        //        _results.Exception = ex;
        //    }
        //    return _results;
        //}

        /// <summary>
        /// Gets the reviews by user async.
        /// </summary>
        /// <returns>The reviews by user async.</returns>
        /// <param name="username">Username.</param>
        public async Task<GetReviewsResults> GetReviewsByUserAsync(string username)
        {
            GetReviewsResults _results = new GetReviewsResults();

            try
            {
                var reviews = await repository.GetReviewsByUserAsync(username);
                _results.Results = reviews;
                _results.Successful = true;
                if (reviews.Count == 0)
                {
                    _results.Message = "No reviews found for this user";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.Exception = ex;
            }

            return _results;
        }

        /// <summary>
        /// Adds the restaurant async.
        /// </summary>
        /// <returns>The restaurant async.</returns>
        /// <param name="restaurant">Restaurant.</param>
        public async Task<AddRestaurantResults> AddRestaurantAsync(Restaurant restaurant)
        {
            AddRestaurantResults _results = new AddRestaurantResults();

            try
            {
                var savedRestaurant = await repository.AddUpdateRestaurantAsync(restaurant);
                await repository.SaveChangesAsync();

                _results.Results = savedRestaurant;
                _results.Successful = true;

            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.Exception = ex;
            }

            return _results;
        }

        /// <summary>
        /// Adds the restaurant review async.
        /// </summary>
        /// <returns>The restaurant review async.</returns>
        /// <param name="review">Review.</param>
        public async Task<AddReviewResults> AddRestaurantReviewAsync(RestaurantReview review)
        {
            AddReviewResults _results = new AddReviewResults();

            try
            {
                var savedReview = await repository.AddUpdateReviewAsync(review);
                await repository.SaveChangesAsync();

                _results.Results = savedReview;
                _results.Successful = true;

            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.Exception = ex;
            }

            return _results;
        }

        /// <summary>
        /// Deletes the restaurant review async.
        /// </summary>
        /// <returns>The restaurant review async.</returns>
        /// <param name="review">Review.</param>
        public async Task<DeleteReviewResults> DeleteRestaurantReviewAsync(Guid reviewId)
        {
            DeleteReviewResults _results = new DeleteReviewResults();

            try
            {
                bool isDeleted = await repository.DeleteReviewAsync(reviewId);
                await repository.SaveChangesAsync();

                _results.Results = isDeleted;
                _results.Successful = true;
                if (!isDeleted)
                {
                    _results.Message = "No matching review found";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.Exception = ex;
            }

            return _results;
        }

        /// <summary>
        /// Finds the restaurants by city and state async.
        /// </summary>
        /// <returns>The restaurants by city state async.</returns>
        /// <param name="city">City.</param>
        /// <param name="state">State.</param>
        public async Task<FindRestaurantsByCityResults> FindRestaurantsByCityStateAsync(string city, string state)
        {
            FindRestaurantsByCityResults _results = new FindRestaurantsByCityResults();

            try
            {
                if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(state))
                {
                    throw new Exception("City and State are both required");
                }
                var restaurants = await repository.FindRestaurantsByCityStateAsync(city, state);
                _results.Results = restaurants;
                _results.Successful = true;
                if (restaurants.Count == 0)
                {
                    _results.Message = "Unable to locate any Restaurants in the city/state provided";
                }
            }
            catch (Exception ex)
            {
                _results.Successful = false;
                _results.Exception = ex;
            }

            return _results;
        }
    
    }
}
