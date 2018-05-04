using Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
    /// <summary>
    ///
    /// <summary>
    public class PatronService : IPatron
    {
        #region PRIVATE MEMBERS
        private LibraryContext _context;
        #endregion

        #region CONSTRUCTOR   

        public PatronService(LibraryContext context)
        {
            _context = context;
        }

        #endregion

        public void Add(Patron patron)
        {
            _context.Add(patron);
            _context.SaveChanges();
        }

        public Patron Get(int id)
        {
            return GetAll()
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Patron> GetAll()
        {
            return _context.Patrons
                .Include(p => p.LibraryCard)
                .Include(p => p.HomeLibraryBranch);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronID)
        {
            var cardId = Get(patronID).LibraryCard.Id;

            return _context.CheckoutHistories
                .Include(c => c.LibraryCard)
                .Include(c => c.LibraryAsset)
                .Where(c => c.LibraryCard.Id == cardId)
                .OrderByDescending(c => c.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int patronID)
        {
            var cardId = Get(patronID).LibraryCard.Id;


            return _context.Checkouts
                .Include(c => c.LibraryCard)
                .Include(c => c.LibraryAsset)
                .Where(c => c.LibraryCard.Id == cardId);

        }

        public IEnumerable<Hold> GetHolds(int patronID)
        {
            var cardId = Get(patronID).LibraryCard.Id;

            return _context.Holds
                .Include(h => h.LibraryCard)
                .Include(h => h.LibraryAsset)
                .Where(h => h.LibraryCard.Id == cardId)
                .OrderByDescending(h => h.HoldPlaced);
        }
    }
}
