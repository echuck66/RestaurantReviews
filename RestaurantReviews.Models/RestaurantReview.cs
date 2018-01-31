using System;

namespace RestaurantReviews.Models
{
    public class RestaurantReview : ObjectBase
    {

        public int userRating { get; set; }

        public string username { get; set; }

        public string reviewText { get; set; }

        public Guid restaurantId { get; set; }

        public virtual Restaurant restaurant { get; set; }

        public DateTime reviewDate { get; set; }

        public RestaurantReview()
        {
        }
    }
}
