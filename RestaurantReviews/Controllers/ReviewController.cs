using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Models;
using RestaurantReviews.UnitOfWork;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestaurantReviews.Models.API.Controllers
{
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        // the dbPath is for use with this exercise code only in
        // instantiating the Unit Of Work class. It's used for 
        // Sqlite usage in the Unit Tests, but a larger-scale 
        // application would not use this type of connection, but instead 
        // register a connection string from appsettins.json in the 
        // startup.cs class's ConfigureServices method similar to this:
        //      services.AddDbContext<ApplicationDbContext>(options =>
        //          options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        //
        string dbPath = string.Empty;

        [HttpGet]
        [Route("api/GetRestaurantsByCityState/{city}/{state}")]
        public async Task<ICollection<Restaurant>> GetRestaurantsByCityState(string city, string state)
        {
            ICollection<Restaurant> foundRestaurants = null; 
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(dbPath))
            {
                var _results = await uow.FindRestaurantsByCityStateAsync(city, state);
                if (_results.Successful)
                {
                    foundRestaurants = _results.Results;
                }
                else 
                {
                    // here we can add special handling in the case of failure
                }
            }
            return foundRestaurants;
        }

        [HttpPost]
        [Route("api/PostRestaurant")]
        public async Task<Restaurant> PostRestaurant([FromBody]Restaurant restaurant)
        {
            Restaurant savedRestaurant = null;

            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(dbPath))
            {
                var _results = await uow.AddRestaurantAsync(restaurant);
                if (_results.Successful) 
                {
                    savedRestaurant = _results.Results;
                }
                else
                {
                    // here we can add special handling in the case of failure
                }
            }

            return savedRestaurant;
        }

        [HttpPost]
        [Route("api/PostReview")]
        public async Task<RestaurantReview> PostReview([FromBody]RestaurantReview review)
        {
            RestaurantReview savedReview = null;

            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(dbPath))
            {
                var _results = await uow.AddRestaurantReviewAsync(review);
                if (_results.Successful)
                {
                    savedReview = _results.Results;
                }
                else
                {
                    // here we can add special handling in the case of failure
                }
            }

            return savedReview;
        }

        [HttpGet]
        [Route("api/GetReviewsByUser/{username}")]
        public async Task<ICollection<RestaurantReview>> GetReviewsByUser(string username)
        {
            ICollection<RestaurantReview> foundReviews = null;

            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(dbPath))
            {
                var _results = await uow.GetReviewsByUserAsync(username);
                if (_results.Successful)
                {
                    foundReviews = _results.Results;
                }
                else
                {
                    // here we can add special handling in the case of failure
                }
            }
            return foundReviews;
        }

        [HttpDelete]
        [Route("api/DeleteReview/{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(dbPath))
            {
                var _results = await uow.DeleteRestaurantReviewAsync(reviewId);
                if (_results.Successful)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
