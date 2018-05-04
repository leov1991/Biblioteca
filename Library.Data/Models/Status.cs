using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public class Status
    {

        #region PUBLIC PROPERTIES

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }



        #endregion

        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public Status()
        {

        }

        #endregion
    }
}
