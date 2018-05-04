using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public class Book : LibraryAsset
    {

        #region PUBLIC PROPERTIES

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string DeweyIndex { get; set; }

        #endregion

        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public Book()
        {

        }

        #endregion
    }
}
