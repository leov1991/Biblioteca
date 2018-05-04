using System;

namespace Library.Data
{
    /// <summary>
    /// Allows us to know which library card requested a certain library asset that was checked out
    /// <summary>
    public class Hold
    {
        #region PUBLIC PROPERTIES

        public int Id { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }


        #endregion


        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public Hold()
        {

        }

        #endregion
    }
}
