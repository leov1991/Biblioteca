using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public class CheckoutHistory
    {
        #region PUBLIC PROPERTIES

        public int Id { get; set; }

        [Required]
        public LibraryAsset LibraryAsset { get; set; }

        [Required]
        public LibraryCard LibraryCard { get; set; }

        [Required]
        public DateTime CheckedOut { get; set; }

        public DateTime? CheckedIn { get; set; }


        #endregion

        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public CheckoutHistory()
        {

        }

        #endregion
    }
}
