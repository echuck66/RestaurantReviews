using System;
using System.Collections.Generic;

namespace RestaurantReviews.Models
{
    public class Restaurant : ObjectBase
    {
        
        public string name { get; set; }

        public Guid addressId { get; set; }

        public virtual RestaurantAddress address { get; set; }
        
        public string phoneNumber { get; set; }
        
        public int averageUserRating { get; set; }

        public bool acceptsReservations { get; set; }

        public virtual ICollection<RestaurantReview> reviews { get; set; }

        public Restaurant()
        {
            
        }

    }
}
