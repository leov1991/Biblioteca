using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public class BranchHours
    {
        #region PUBLIC PROPERTIES

        public int Id { get; set; }
        public LibraryBranch Branch { get; set; }

        [Range(0, 6)]
        public int DayOfWeek { get; set; }

        [Range(0, 23)]
        public int OpenTIme { get; set; }

        [Range(0, 23)]
        public int CloseTime { get; set; }


        #endregion

        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public BranchHours()
        {

        }

        #endregion
    }
}
