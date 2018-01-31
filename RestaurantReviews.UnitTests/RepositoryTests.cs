using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantReviews.Repository;

namespace RestaurantReviews.Models.API.UnitTests
{
    [TestClass]
    public class RepositoryTests
    {
        SqliteConnection connection;
        SystemRepository repository;
        SystemRepository validationRepository;

        public RepositoryTests()
        {
        }

        [TestInitialize]
        public void SetupTestData()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<RestaurantReviewsContext>()
                    .UseSqlite(connection)
                    .Options;

            RestaurantReviewsContext context = new RestaurantReviewsContext(options);

            repository = new SystemRepository(context);
            validationRepository = new SystemRepository(context);
        }


        [TestCleanup]
        public void CleanupDataContext()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
                repository.Dispose();
                validationRepository.Dispose();
            }
        }

        [TestMethod]
        public async Task AddRestaurantTests()
        {
            RestaurantAddress address = new RestaurantAddress()
            {
                streetAddress = "100 Main St",
                addrLine2 = string.Empty,
                city = "Pittsburgh",
                state = "PA",
                zipcode = "15108"
            };

            Restaurant restaurant = new Restaurant()
            {
                name = "Restaurant 1",
                address = address,
                addressId = address.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                phoneNumber = "412-444-4444",
                acceptsReservations = false
            };

            Restaurant savedRestaurant = await repository.AddUpdateRestaurantAsync(restaurant);
            await repository.SaveChangesAsync();

            Restaurant testRestaurant = await validationRepository.GetRestaurantAsync(savedRestaurant.id);
            Assert.AreEqual(savedRestaurant.id, testRestaurant.id);
            Assert.AreEqual(savedRestaurant.addressId, testRestaurant.addressId);
        }

        [TestMethod]
        public async Task FindRestaurantsByCityTests()
        {
            RestaurantAddress address1 = new RestaurantAddress()
            {
                streetAddress = "100 Main St",
                addrLine2 = string.Empty,
                city = "Pittsburgh",
                state = "PA",
                zipcode = "15108"
            };

            Restaurant restaurant1 = new Restaurant()
            {
                name = "Restaurant 1",
                address = address1,
                addressId = address1.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                phoneNumber = "412-444-4444",
                acceptsReservations = false
            };


            RestaurantAddress address2 = new RestaurantAddress()
            {
                streetAddress = "100 Main St",
                addrLine2 = string.Empty,
                city = "Boston",
                state = "MA",
                zipcode = "15108"
            };

            Restaurant restaurant2 = new Restaurant()
            {
                name = "Restaurant 2",
                address = address2,
                addressId = address2.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                phoneNumber = "412-555-5555",
                acceptsReservations = false
            };
            
            Restaurant savedRestaurant1 = await repository.AddUpdateRestaurantAsync(restaurant1);
            await repository.SaveChangesAsync();

            Restaurant savedRestaurant2 = await repository.AddUpdateRestaurantAsync(restaurant2);
            await repository.SaveChangesAsync();

            var foundRestaurants1 = await repository.FindRestaurantsByCityStateAsync("pittsburgh", "pa");
            Assert.IsNotNull(foundRestaurants1);
            Assert.IsTrue(foundRestaurants1.Count == 1);

            var foundRestaurants2 = await repository.FindRestaurantsByCityStateAsync("boston", "ma");
            Assert.IsNotNull(foundRestaurants2);
            Assert.IsTrue(foundRestaurants2.Count == 1);
        }

        [TestMethod]
        public async Task PostReviewTests() 
        {
            RestaurantAddress address = new RestaurantAddress()
            {
                streetAddress = "100 Main St",
                addrLine2 = string.Empty,
                city = "Pittsburgh",
                state = "PA",
                zipcode = "15108",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            Restaurant restaurant = new Restaurant()
            {
                name = "Restaurant 1",
                address = address,
                addressId = address.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                phoneNumber = "412-444-4444",
                acceptsReservations = false
            };

            Restaurant savedRestaurant = await repository.AddUpdateRestaurantAsync(restaurant);
            await repository.SaveChangesAsync();

            RestaurantReview review1 = new RestaurantReview()
            {
                restaurant = savedRestaurant,
                restaurantId = savedRestaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly and the food was great!",
                userRating = 5,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            RestaurantReview review2 = new RestaurantReview()
            {
                restaurant = savedRestaurant,
                restaurantId = savedRestaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly but the food was only ok.",
                userRating = 3,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            List<RestaurantReview> reviews = new List<RestaurantReview>()
            {
                review1,
                review2
            };

            foreach(RestaurantReview review in reviews)
            {
                RestaurantReview savedReview = await repository.AddUpdateReviewAsync(review);

                // Ensure the Restaurant Record was unchanged
                Restaurant validationRestaurant = await validationRepository.GetRestaurantAsync(savedRestaurant.id);
                Assert.IsNotNull(validationRepository);
                Assert.AreEqual(savedRestaurant.id, validationRestaurant.id);
                Assert.AreEqual(savedRestaurant.name, validationRestaurant.name);
                Assert.IsNotNull(savedRestaurant.address);
                Assert.AreEqual(savedRestaurant.addressId, validationRestaurant.addressId);

                Assert.IsNotNull(savedReview);
                Assert.AreEqual(savedReview.id, review.id);
                Assert.AreEqual(savedReview.restaurantId, review.restaurantId);
                Assert.AreEqual(savedReview.reviewDate, review.reviewDate);
                Assert.AreEqual(savedReview.reviewText, review.reviewText);
                Assert.AreEqual(savedReview.username, review.username);

            }

            // Test the average ratings value after multiple ratings have been saved
            Restaurant testRestaurantReviews = await validationRepository.GetRestaurantAsync(restaurant.id);
            Assert.IsTrue(testRestaurantReviews.averageUserRating  > 0);
            int expectedRating = (review1.userRating + review2.userRating) / 2;
            Assert.AreEqual(expectedRating, testRestaurantReviews.averageUserRating);
        }
    
        [TestMethod]
        public async Task GetReviewsByUserTests()
        {
            RestaurantAddress address = new RestaurantAddress()
            {
                streetAddress = "100 Main St",
                addrLine2 = string.Empty,
                city = "Pittsburgh",
                state = "PA",
                zipcode = "15108",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            Restaurant restaurant = new Restaurant()
            {
                name = "Restaurant 1",
                address = address,
                addressId = address.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                phoneNumber = "412-444-4444",
                acceptsReservations = false
            };

            Restaurant savedRestaurant = await repository.AddUpdateRestaurantAsync(restaurant);
            await repository.SaveChangesAsync();

            RestaurantReview review1 = new RestaurantReview()
            {
                restaurant = savedRestaurant,
                restaurantId = savedRestaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly and the food was great!",
                userRating = 5,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            RestaurantReview review2 = new RestaurantReview()
            {
                restaurant = savedRestaurant,
                restaurantId = savedRestaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly but the food was only ok.",
                userRating = 3,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            List<RestaurantReview> reviews = new List<RestaurantReview>()
            {
                review1,
                review2
            };

            foreach (RestaurantReview review in reviews)
            {
                RestaurantReview savedReview = await repository.AddUpdateReviewAsync(review);


                // Ensure the Restaurant Record was unchanged
                Restaurant validationRestaurant = await validationRepository.GetRestaurantAsync(savedRestaurant.id);
                Assert.IsNotNull(validationRepository);
                Assert.AreEqual(savedRestaurant.id, validationRestaurant.id);
                Assert.AreEqual(savedRestaurant.name, validationRestaurant.name);
                Assert.IsNotNull(savedRestaurant.address);
                Assert.AreEqual(savedRestaurant.addressId, validationRestaurant.addressId);

                // Check the saved Review
                Assert.IsNotNull(savedReview);
                Assert.AreEqual(savedReview.id, review.id);
                Assert.AreEqual(savedReview.restaurantId, review.restaurantId);
                Assert.AreEqual(savedReview.reviewDate, review.reviewDate);
                Assert.AreEqual(savedReview.reviewText, review.reviewText);
                Assert.AreEqual(savedReview.username, review.username);

            }
            await repository.SaveChangesAsync();

            var reviewsByUser = await validationRepository.GetReviewsByUserAsync(review1.username);

            Assert.IsTrue(reviewsByUser.Count == 2);

        }
    

        [TestMethod]
        public async Task DeleteReviewTests()
        {
            RestaurantAddress address = new RestaurantAddress()
            {
                streetAddress = "100 Main St",
                addrLine2 = string.Empty,
                city = "Pittsburgh",
                state = "PA",
                zipcode = "15108",
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now
            };

            Restaurant restaurant = new Restaurant()
            {
                name = "Restaurant 1",
                address = address,
                addressId = address.id,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                phoneNumber = "412-444-4444",
                acceptsReservations = false
            };

            Restaurant savedRestaurant = await repository.AddUpdateRestaurantAsync(restaurant);
            await repository.SaveChangesAsync();

            Assert.IsNotNull(savedRestaurant);

            RestaurantReview review1 = new RestaurantReview()
            {
                restaurant = savedRestaurant,
                restaurantId = savedRestaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly and the food was great!",
                userRating = 5,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            RestaurantReview review2 = new RestaurantReview()
            {
                restaurant = savedRestaurant,
                restaurantId = savedRestaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly but the food was only ok.",
                userRating = 3,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            List<RestaurantReview> reviews = new List<RestaurantReview>()
            {
                review1,
                review2
            };

            foreach (RestaurantReview review in reviews)
            {
                RestaurantReview savedReview = await repository.AddUpdateReviewAsync(review);

                Assert.IsNotNull(savedReview);

                // Ensure the Restaurant Record was unchanged
                Restaurant validationRestaurant = await validationRepository.GetRestaurantAsync(savedRestaurant.id);
                Assert.IsNotNull(validationRepository);
                Assert.AreEqual(savedRestaurant.id, validationRestaurant.id);
                Assert.AreEqual(savedRestaurant.name, validationRestaurant.name);
                Assert.IsNotNull(savedRestaurant.address);
                Assert.AreEqual(savedRestaurant.addressId, validationRestaurant.addressId);

                // Check the saved Review
                Assert.IsNotNull(savedReview);
                Assert.AreEqual(savedReview.id, review.id);
                Assert.AreEqual(savedReview.restaurantId, review.restaurantId);
                Assert.AreEqual(savedReview.reviewDate, review.reviewDate);
                Assert.AreEqual(savedReview.reviewText, review.reviewText);
                Assert.AreEqual(savedReview.username, review.username);

            }
            await repository.SaveChangesAsync();

            // Delete one review of two and save changes
            await repository.DeleteReviewAsync(review1.id);
            await repository.SaveChangesAsync();

            var reviewsByUser = await validationRepository.GetReviewsByUserAsync(review1.username);

            Assert.IsTrue(reviewsByUser.Count == 1);

            List<RestaurantReview> reviewsFound = new List<RestaurantReview>();
            reviewsFound.AddRange(reviewsByUser);

            RestaurantReview testReview = reviewsFound[0];
            Assert.AreEqual(testReview.id, review2.id);

        }
    
    }
}
