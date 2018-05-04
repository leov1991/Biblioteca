using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    /// <summary>
    ///
    /// <summary>
    public class LibraryContext : DbContext
    {
        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public LibraryContext(DbContextOptions options) : base(options) { }

        // DBSets
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public DbSet<LibraryBranch> LibraryBranches { get; set; }
        public DbSet<BranchHours> BranchHours { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<LibraryAsset> LibraryAssets { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<Status> Statuses { get; set; }



        #endregion
    }
}
