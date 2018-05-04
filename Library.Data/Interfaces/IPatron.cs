using System.Collections.Generic;

namespace Library.Data
{
    public interface IPatron
    {
        Patron Get(int id);
        IEnumerable<Patron> GetAll();
        void Add(Patron patron);

        IEnumerable<CheckoutHistory> GetCheckoutHistory(int patronID);
        IEnumerable<Hold> GetHolds(int patronID);
        IEnumerable<Checkout> GetCheckouts(int patronID);
    }
}
