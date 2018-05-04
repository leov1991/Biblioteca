using System;
using System.Collections.Generic;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public class LibraryCard
    {

        #region PUBLIC PROPERTIES

        public int Id { get; set; }

        public decimal Fees { get; set; }

        public DateTime Created { get; set; }

        public virtual IEnumerable<CheckoutHistory> Checkouts { get; set; }


        #endregion

        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public LibraryCard()
        {

        }

        #endregion
    }
}
