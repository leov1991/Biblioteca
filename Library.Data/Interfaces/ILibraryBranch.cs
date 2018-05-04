using System.Collections.Generic;

namespace Library.Data
{
    public interface ILibraryBranch
    {
        IEnumerable<LibraryBranch> GetAll();
        IEnumerable<Patron> GetPatrons(int branchId);
        IEnumerable<LibraryAsset> GetAssets(int branchId);
        IEnumerable<string> GetBranchHours(int branchId);

        LibraryBranch Get(int id);

        void Add(LibraryBranch branch);

        bool IsBranchOpen(int branchId);

    }
}
