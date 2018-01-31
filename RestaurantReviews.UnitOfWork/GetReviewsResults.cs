using System;
using System.Collections.Generic;
using RestaurantReviews.Models;

namespace RestaurantReviews.UnitOfWork
{
    public class GetReviewsResults : UnitOfWorkResults<ICollection<RestaurantReview>>
    {
        
        public GetReviewsResults()
        {
        }
    }
}
