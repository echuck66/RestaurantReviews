using System;
namespace RestaurantReviews.UnitOfWork
{
    public class UnitOfWorkResults<T> 
    {
        public T Results { get; set; }

        public bool Successful { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public UnitOfWorkResults()
        {
        }
    }
}
