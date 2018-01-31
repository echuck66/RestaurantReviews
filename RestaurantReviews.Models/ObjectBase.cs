using System;
namespace RestaurantReviews.Models
{
    public abstract class ObjectBase
    {
        public Guid id { get; set; }

        #region Audit/Tracking Fields

        /// <note>
        /// Note: the following fields would typically be used for audit-tracking 
        /// purposes, but for the sake of simplicity, we'll assume a larger system
        /// authentication mechanism woule be in-place to obtain the username for any 
        /// authenticated users, should this information wish to be tracked
        /// </note>
        //public string addedByUsername { get; set; }
        //public string addedByIPAddress { get; set; }
        //public string modifiedByUsername { get; set; }
        //public string modifiedByIPAddress { get; set; }

        public DateTime dateCreated { get; set; }

        public DateTime dateModified { get; set; }

        #endregion

        public ObjectBase()
        {
            this.id = Guid.NewGuid();
        }
    }
}
