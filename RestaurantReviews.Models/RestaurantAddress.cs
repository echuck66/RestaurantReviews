using System;
namespace RestaurantReviews.Models
{
    public class RestaurantAddress : ObjectBase
    {
        public string streetAddress { get; set; }

        public string addrLine2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public RestaurantAddress()
        {
        }
    }
}
