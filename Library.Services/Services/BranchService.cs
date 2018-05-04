using Library.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
    /// <summary>
    ///
    /// <summary>
    public class BranchService : ILibraryBranch
    {
        #region PRIVATE MEMBER

        private LibraryContext _context;

        #endregion

        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public BranchService(LibraryContext context)
        {
            _context = context;
        }



        #endregion


        public void Add(LibraryBranch branch)
        {
            _context.Add(branch);
            _context.SaveChanges();
        }

        public LibraryBranch Get(int id)
        {
            return GetAll()
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<LibraryBranch> GetAll()
        {
            return _context.LibraryBranches
                .Include(b => b.Patrons)
                .Include(b => b.LibraryAssets);
        }

        public IEnumerable<LibraryAsset> GetAssets(int branchId)
        {
            return _context.LibraryBranches
                .Include(b => b.LibraryAssets)
                .FirstOrDefault(p => p.Id == branchId).LibraryAssets;
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context.BranchHours.Where(h => h.Branch.Id == branchId);
            return DataHelpers.HumanizeBusinessHours(hours);
        }

        public IEnumerable<Patron> GetPatrons(int branchId)
        {
            return _context.LibraryBranches
                .Include(p => p.Patrons)
                .FirstOrDefault(p => p.Id == branchId).Patrons;
        }

        public bool IsBranchOpen(int branchId)
        {
            var curretnTimeHour = DateTime.Now.Hour;
            var curretnDayOfWeek = (int)DateTime.Now.DayOfWeek + 1; // Because days in database start with index 1
            var hours = _context.BranchHours.Where(h => h.Branch.Id == branchId);
            var daysHours = hours.FirstOrDefault(h => h.DayOfWeek == curretnDayOfWeek);

            return curretnTimeHour < daysHours.CloseTime && curretnTimeHour > daysHours.OpenTIme;


        }
    }
}
