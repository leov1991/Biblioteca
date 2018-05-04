using Library.Data;
using Library.Models.Patrons;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Library.Controllers
{
    public class PatronController : Controller
    {

        #region PRIVATE MEMBERS

        private IPatron _patron;

        #endregion

        #region CONSTRUCTOR
        public PatronController(IPatron patron)
        {
            _patron = patron;

        }
        #endregion

        public IActionResult Index()
        {
            var allPatrons = _patron.GetAll();

            var patronModels = allPatrons.Select(p => new PatronDetailModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                LibraryCardId = p.LibraryCard.Id,
                OverdueFees = p.LibraryCard.Fees,
                HomeLibraryBranch = p.HomeLibraryBranch.Name

            }).ToList();

            var model = new PatronIndexModel
            {
                Patrons = patronModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var patron = _patron.Get(id);

            var model = new PatronDetailModel
            {
                Id = patron.Id,
                FirstName = patron.FirstName,
                LastName = patron.LastName,
                Address = patron.Address,
                LibraryCardId = patron.LibraryCard.Id,
                HomeLibraryBranch = patron.HomeLibraryBranch.Name,
                MemberSince = patron.LibraryCard.Created,
                OverdueFees = patron.LibraryCard.Fees,
                Telephone = patron.TelephoneNumber,
                AssetsCheckedOut = _patron.GetCheckouts(id),
                CheckoutHistory = _patron.GetCheckoutHistory(id),
                Holds = _patron.GetHolds(id)
            };

            return View(model);
        }
    }
}