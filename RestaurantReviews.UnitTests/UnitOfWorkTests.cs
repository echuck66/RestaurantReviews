using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantReviews.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestaurantReviews.UnitOfWork;

namespace RestaurantReviews.Models.API.UnitTests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        Mock<ISystemRepository> repositoryMock;

        public UnitOfWorkTests()
        {
        }

        [TestInitialize]
        public void InitializeRepository()
        {
            repositoryMock = new Mock<ISystemRepository>();
        }

        [TestCleanup]
        public void CleanupResources()
        {
            repositoryMock = null;
        }

        [TestMethod]
        public async Task GetReviewsByUserAsyncTests()
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


            RestaurantReview review1 = new RestaurantReview()
            {
                restaurant = restaurant,
                restaurantId = restaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly and the food was great!",
                userRating = 5,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            RestaurantReview review2 = new RestaurantReview()
            {
                restaurant = restaurant,
                restaurantId = restaurant.id,
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

            // Test the 'Happy Path'
            repositoryMock.Setup(m => m.GetReviewsByUserAsync(It.IsAny<string>())).ReturnsAsync(reviews);  
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.GetReviewsByUserAsync("user1@nowhere.net");

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Results);
                Assert.AreEqual(_results.Results.Count, 2);
                Assert.IsTrue(_results.Successful);
            }

            // Test Exception Handling
            Exception ex = new Exception("Any Exception");
            repositoryMock.Setup(m => m.GetReviewsByUserAsync(It.IsAny<string>())).ThrowsAsync(ex);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.GetReviewsByUserAsync("user1@nowhere.net");

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.IsNull(_results.Results);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task AddRestaurantAsyncTests()
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

            // Test the 'Happy Path'
            repositoryMock.Setup(m => m.AddUpdateRestaurantAsync(It.IsAny<Restaurant>())).ReturnsAsync(restaurant1);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.AddRestaurantAsync(restaurant1);
                Assert.IsNotNull(_results);
                Assert.IsTrue(_results.Successful);
                Assert.IsNotNull(_results.Results);
                Assert.IsInstanceOfType(_results.Results, typeof(Restaurant));
            }

            // Test NULL value for Restaurant
            Exception exNullReference = new NullReferenceException();
            repositoryMock.Setup(m => m.AddUpdateRestaurantAsync(It.IsAny<Restaurant>())).ThrowsAsync(exNullReference);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.AddRestaurantAsync(null);

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.IsNull(_results.Results);
                Assert.IsFalse(_results.Successful);
            }

            // Test Exception Handling
            Exception ex = new Exception("Any Exception");
            repositoryMock.Setup(m => m.AddUpdateRestaurantAsync(It.IsAny<Restaurant>())).ThrowsAsync(ex);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.AddRestaurantAsync(restaurant1);

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.IsNull(_results.Results);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task AddRestaurantReviewAsyncTests()
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

            RestaurantReview review1 = new RestaurantReview()
            {
                restaurant = restaurant,
                restaurantId = restaurant.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly and the food was great!",
                userRating = 5,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            // Test the 'Happy Path'
            repositoryMock.Setup(m => m.AddUpdateReviewAsync(It.IsAny<RestaurantReview>())).ReturnsAsync(review1);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.AddRestaurantReviewAsync(review1);
                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Results);
                Assert.IsInstanceOfType(_results.Results, typeof(RestaurantReview));
                Assert.AreEqual(_results.Results.id, review1.id);
            }

            // Test NULL review
            Exception exNullReference = new NullReferenceException();
            repositoryMock.Setup(m => m.AddUpdateReviewAsync(It.IsAny<RestaurantReview>())).ThrowsAsync(exNullReference);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.AddRestaurantReviewAsync(null);

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.IsNull(_results.Results);
                Assert.IsFalse(_results.Successful);
            }

            // Test Exception Handling
            Exception ex = new Exception("Any Exception");
            repositoryMock.Setup(m => m.AddUpdateReviewAsync(It.IsAny<RestaurantReview>())).ThrowsAsync(ex);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.AddRestaurantReviewAsync(review1);

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.IsNull(_results.Results);
                Assert.IsFalse(_results.Successful);
            }

        }

        [TestMethod]
        public async Task DeleteRestaurantReviewAsyncTests()
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

            RestaurantReview review1 = new RestaurantReview()
            {
                restaurant = restaurant1,
                restaurantId = restaurant1.id,
                reviewDate = DateTime.Now,
                reviewText = "This restaurant served us quickly and the food was great!",
                userRating = 5,
                dateCreated = DateTime.Now,
                dateModified = DateTime.Now,
                username = "user1@nowhere.net"
            };

            // Test the 'Happy Path'
            bool isDeleted = true;
            repositoryMock.Setup(m => m.DeleteReviewAsync(It.IsAny<Guid>())).ReturnsAsync(isDeleted);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.DeleteRestaurantReviewAsync(review1.id);
                Assert.IsNotNull(_results);
                Assert.IsTrue(_results.Results);
                Assert.IsNull(_results.Exception);
            }

            // Test Review NOT FOUND scenario
            isDeleted = false;
            repositoryMock.Setup(m => m.DeleteReviewAsync(It.IsAny<Guid>())).ReturnsAsync(isDeleted);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.DeleteRestaurantReviewAsync(review1.id);
                Assert.IsNotNull(_results);
                Assert.IsFalse(_results.Results);
                Assert.IsNull(_results.Exception);
                Assert.AreEqual(_results.Message, "No matching review found");
            }

            // Test Exception Handling
            Exception ex = new Exception("Any Exception");
            repositoryMock.Setup(m => m.DeleteReviewAsync(It.IsAny<Guid>())).ThrowsAsync(ex);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.DeleteRestaurantReviewAsync(review1.id);

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.IsFalse(_results.Successful);
            }
        }

        [TestMethod]
        public async Task GetListOfRestaurantsByCityStateTests()
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
                city = "Pittsburgh",
                state = "PA",
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

            List<Restaurant> restaurants = new List<Restaurant>();
            restaurants.Add(restaurant1);
            restaurants.Add(restaurant2);

            // Test the 'Happy Path'
            repositoryMock.Setup(m => m.FindRestaurantsByCityStateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(restaurants);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.FindRestaurantsByCityStateAsync("pittsburgh", "pa");
                Assert.IsNotNull(_results);
                Assert.IsInstanceOfType(_results.Results, typeof(ICollection<Restaurant>));
                Assert.IsTrue(_results.Results.Count == 2);
            }

            // Test No restaurants found 
            restaurants.Clear();
            repositoryMock.Setup(m => m.FindRestaurantsByCityStateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(restaurants);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.FindRestaurantsByCityStateAsync("boston", "ma");
                Assert.IsNotNull(_results);
                Assert.IsInstanceOfType(_results.Results, typeof(ICollection<Restaurant>));
                Assert.IsTrue(_results.Results.Count == 0);
                Assert.AreEqual(_results.Message, "Unable to locate any Restaurants in the city/state provided");
            }

            // Test empty city/state Handling
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.FindRestaurantsByCityStateAsync(string.Empty, string.Empty);

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.AreEqual(_results.Exception.Message, "City and State are both required");
                Assert.IsNull(_results.Results);
            }

            // Test Exception Handling
            Exception ex = new Exception("Any Exception");
            repositoryMock.Setup(m => m.FindRestaurantsByCityStateAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(ex);
            using (RestaurantReviewsUnitOfWork uow = new RestaurantReviewsUnitOfWork(repositoryMock.Object))
            {
                var _results = await uow.FindRestaurantsByCityStateAsync("pittsburgh", "pa");

                Assert.IsNotNull(_results);
                Assert.IsNotNull(_results.Exception);
                Assert.IsNull(_results.Results);
            }
        }
    }
}
