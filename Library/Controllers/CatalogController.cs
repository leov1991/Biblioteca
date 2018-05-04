using Library.Data;
using Library.Models.Catalog;
using Library.Models.Checkouts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Library.Controllers
{
    public class CatalogController : Controller
    {
        #region PRIVATE MEMBERS

        ILibraryAsset _assets;
        ICheckout _checkouts;

        #endregion

        public CatalogController(ILibraryAsset assets, ICheckout checkouts)
        {

            _assets = assets;
            _checkouts = checkouts;

        }

        public IActionResult Index()
        {
            // Retrieve assets from database
            var assetModels = _assets.GetAll();

            // Map to the viewmodel
            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                    Type = _assets.GetType(result.Id),
                    Title = result.Title,
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id)

                });

            var model = new AssetIndexModel
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetByID(id);
            var currentHolds = _checkouts.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = _checkouts.GetCurrentHoldPlaced(a.Id).ToString(),
                    PatronName = _checkouts.GetCurrentHoldPatronName(a.Id)


                });

            var model = new AssetDetailModel
            {
                Id = id,
                Title = asset.Title,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                Cost = asset.Cost,
                Isbn = _assets.GetIsbn(id),
                Type = _assets.GetType(id),
                DeweyCallNumber = _assets.GetDeweyIndex(id),
                Year = asset.Year,
                CheckoutHistory = _checkouts.GetCheckoutHistory(id),
                PatronName = _checkouts.GetCurrentCheckoutPatron(id),
                Holds = currentHolds,
                LatestCheckout = _checkouts.GetLatestCheckout(id)

            };

            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = _assets.GetByID(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id),
                HoldCount = _checkouts.GetCurrentHolds(id).Count()
            };

            return View(model);
        }

        public IActionResult CheckIn(int id)
        {
            _checkouts.CheckInItem(id);
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkouts.CheckOutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkouts.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult MarkLost(int assetId)
        {
            _checkouts.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult MarkFound(int assetId)
        {
            _checkouts.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult Hold(int id)
        {
            var asset = _assets.GetByID(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id),
            };

            return View(model);

        }

    }
}
