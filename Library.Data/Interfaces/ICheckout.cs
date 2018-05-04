using System;
using System.Collections.Generic;

namespace Library.Data
{
    public interface ICheckout
    {
        Checkout GetById(int id);
        Checkout GetLatestCheckout(int assetId);

        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        IEnumerable<Checkout> GetAll();
        IEnumerable<Hold> GetCurrentHolds(int id);

        string GetCurrentCheckoutPatron(int assetId);
        string GetCurrentHoldPatronName(int holdId);

        void MarkLost(int assetid);
        void MarkFound(int assetid);
        void Add(Checkout checkout);
        void CheckOutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
        void PlaceHold(int assetId, int libraryCarId);

        DateTime GetCurrentHoldPlaced(int holdId);

        bool IsCheckedOut(int assetId);





    }
}
