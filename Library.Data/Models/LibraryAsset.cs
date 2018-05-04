using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public abstract class LibraryAsset
    {
        #region PUBLIC PROPERTIES

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public string ImageUrl { get; set; }

        public int NumberOfCopies { get; set; }


        public virtual LibraryBranch Location { get; set; }

        #endregion


        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public LibraryAsset()
        {

        }

        #endregion
    }
}
