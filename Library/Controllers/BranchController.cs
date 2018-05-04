using Library.Data;
using Library.Models.Branches;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Library.Controllers
{
    public class BranchController : Controller
    {
        #region PRIVATE MEMBERS

        private ILibraryBranch _branch;

        #endregion

        public BranchController(ILibraryBranch branch)
        {
            _branch = branch;

        }

        public IActionResult Index()
        {
            var branches = _branch.GetAll().Select(branch => new BranchDetailModel
            {
                Id = branch.Id,
                IsOpen = _branch.IsBranchOpen(branch.Id),
                NumberOfAssets = _branch.GetAssets(branch.Id).Count(),
                NumberOfPatrons = _branch.GetPatrons(branch.Id).Count(),
                Name = branch.Name,


            });

            var model = new BranchIndexModel
            {
                Branches = branches.ToList()
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branch = _branch.Get(id);

            var model = new BranchDetailModel
            {
                Id = branch.Id,
                Address = branch.Address,
                Description = branch.Description,
                ImageUrl = branch.ImageUrl,
                IsOpen = _branch.IsBranchOpen(branch.Id),
                NumberOfAssets = _branch.GetAssets(branch.Id).Count(),
                NumberOfPatrons = _branch.GetPatrons(branch.Id).Count(),
                Name = branch.Name,
                Telephone = branch.Telephone,
                OpenDate = branch.OpenDate.ToString("d"),
                HoursOpen = _branch.GetBranchHours(branch.Id),
                AssetsValue = _branch.GetAssets(branch.Id).Sum(a => a.Cost)
            };

            return View(model);
        }
    }
}