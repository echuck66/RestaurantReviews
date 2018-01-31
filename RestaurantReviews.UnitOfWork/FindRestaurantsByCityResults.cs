using System;
using System.Collections.Generic;
using RestaurantReviews.Models;

namespace RestaurantReviews.UnitOfWork
{
    public class FindRestaurantsByCityResults : UnitOfWorkResults<ICollection<Restaurant>>
    {
        
        public FindRestaurantsByCityResults()
        {
        }
    }
}
