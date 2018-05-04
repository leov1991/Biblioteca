using Library.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
    /// <summary>
    /// The service for handling the library checkout system
    /// <summary>
    public class CheckoutService : ICheckout
    {
        #region PRIVATE MEMBERS

        private LibraryContext _context;

        #endregion

        #region CONSTRUCTOR   

        public CheckoutService(LibraryContext context)
        {
            _context = context;
        }

        #endregion

        public void Add(Checkout checkout)
        {
            _context.Add(checkout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistories
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == id);
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Where(h => h.LibraryAsset.Id == id);
        }

        public Checkout GetLatestCheckout(int assetId)
        {
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == assetId)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();
        }

        public Checkout GetById(int id)
        {
            return GetAll().FirstOrDefault(c => c.Id == id);
        }

        // MarkFound and MarkLost could look nicer with a status enum instead of just strings
        public void MarkFound(int assetid)
        {
            UpdateAssetStatus(assetid, "Available");
            RemoveExistingCheckouts(assetid);
            CloseCheckoutHistory(assetid);
            _context.SaveChanges();
        }

        public void MarkLost(int assetid)
        {
            UpdateAssetStatus(assetid, "Lost");
            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCarId)
        {
            // Get item
            var item = _context.LibraryAssets
                .Include(a => a.Status)
              .FirstOrDefault(a => a.Id == assetId);

            // Get card
            var libraryCard = _context.LibraryCards
               .FirstOrDefault(card => card.Id == libraryCarId);

            if (item.Status.Name.Equals("Available"))
            {
                UpdateAssetStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = DateTime.Now,
                LibraryAsset = item,
                LibraryCard = libraryCard

            };

            _context.Add(hold);

            _context.SaveChanges();

        }

        public void CheckInItem(int assetId)
        {
            var item = _context.LibraryAssets
               .FirstOrDefault(a => a.Id == assetId);

            // Remove any axisting checkouts
            RemoveExistingCheckouts(assetId);

            // Close any existing checkout history
            CloseCheckoutHistory(assetId);

            // Look for existing holds on the item
            var currentHolds = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == assetId);
            // If there are any holds, checkout the item to the libraryCard with the earliest hold
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
                return;
            }


            // Otherwise, update the status to available
            UpdateAssetStatus(assetId, "Available");

            _context.SaveChanges();
        }

        public void CheckOutItem(int assetId, int libraryCardId)
        {
            if (IsCheckedOut(assetId))
            {
                return;
                // TO DO: Add logic to handle feedback to the user
            }

            // Get item
            var item = _context.LibraryAssets
               .FirstOrDefault(a => a.Id == assetId);

            // Update to checked out
            UpdateAssetStatus(assetId, "Checked Out");

            // Get library card
            var libraryCard = _context.LibraryCards
                .Include(card => card.Checkouts)
                .FirstOrDefault(card => card.Id == libraryCardId);

            // Create new checkout
            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = DateTime.Now,
                Until = GetDefaultCheckoutTime(DateTime.Now)
            };

            // Add to the database
            _context.Add(checkout);


            // Create new checkout history
            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = DateTime.Now,
                LibraryAsset = item,
                LibraryCard = libraryCard

            };

            // Add to database
            _context.Add(checkoutHistory);

            _context.SaveChanges();

        }

        public string GetCurrentHoldPatronName(int holdId)
        {
            // Look for the hold
            var hold = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id == holdId);

            // Get the card
            var cardId = hold?.LibraryCard.Id;

            // Find the patron with the card id
            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron?.FirstName + " " + patron?.LastName;


        }

        public DateTime GetCurrentHoldPlaced(int holdId)
        {
            return _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id == holdId)
                .HoldPlaced;
        }

        public string GetCurrentCheckoutPatron(int assetId)
        {
            var checkout = GetCheckoutByAssetId(assetId);

            if (null == checkout)
                return "";

            var cardId = checkout.LibraryCard.Id;
            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.LastName;

        }

        public bool IsCheckedOut(int assetId)
        {
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == assetId)
                .Any();
        }


        #region PRIVATE METHODS

        private void UpdateAssetStatus(int assetId, string newStatus)
        {
            // Get item
            var item = _context.LibraryAssets
                .FirstOrDefault(a => a.Id == assetId);

            // Mark item
            _context.Update(item);

            item.Status = _context.Statuses.FirstOrDefault(s => s.Name == newStatus);
        }

        private void RemoveExistingCheckouts(int assetid)
        {
            // Remove existing checkouts
            var checkout = _context.Checkouts.FirstOrDefault(c => c.LibraryAsset.Id == assetid);

            if (null != checkout)
                _context.Remove(checkout);

        }

        private void CloseCheckoutHistory(int assetid)
        {
            // Close Checkout history
            var history = _context.CheckoutHistories
                .FirstOrDefault(ch => ch.LibraryAsset.Id == assetid &&
                ch.CheckedIn == null);

            if (null != history)
            {
                _context.Update(history);
                history.CheckedIn = DateTime.Now;

            }
        }

        private DateTime GetDefaultCheckoutTime(DateTime utcNow)
        {
            return utcNow.AddDays(30);
        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context.Checkouts
                .Include(c => c.LibraryAsset)
                .Include(C => C.LibraryCard)
                .FirstOrDefault(c => c.LibraryAsset.Id == assetId);
        }




        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            // Get earlierst hold
            var earliestHold = currentHolds
                .OrderBy(holds => holds.HoldPlaced).
                FirstOrDefault();

            // Get the card of the earliest hold
            var card = earliestHold.LibraryCard;

            // Remove earliest hold
            _context.Remove(earliestHold);

            _context.SaveChanges();

            CheckOutItem(assetId, card.Id);
        }





        #endregion
    }
}
