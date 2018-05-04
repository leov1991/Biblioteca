using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public class LibraryBranch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El nombre no debe superar los 30 caracteres.")]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Telephone { get; set; }

        public string Description { get; set; }

        public DateTime OpenDate { get; set; }

        public string ImageUrl { get; set; }

        public virtual IEnumerable<Patron> Patrons { get; set; }
        public virtual IEnumerable<LibraryAsset> LibraryAssets { get; set; }



        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public LibraryBranch()
        {

        }

        #endregion
    }
}
