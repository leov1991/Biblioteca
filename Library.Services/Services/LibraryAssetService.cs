using Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services
{
    /// <summary>
    ///
    /// <summary>
    public class LibraryAssetService : ILibraryAsset
    {
        #region PRIVATE MEMBERS

        private LibraryContext _context;

        #endregion

        #region CONSTRUCTOR   

        /// <summary>
        /// Default constructor
        /// <summary>
        public LibraryAssetService()
        {

        }


        /// <summary>
        /// Constructor that receives a DbContext
        /// </summary>
        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }

        #endregion

        public void Add(LibraryAsset libraryAsset)
        {
            _context.LibraryAssets.Add(libraryAsset);
            _context.SaveChanges();
        }

        /// <summary>
        /// List of <see cref="LibraryAsset"/>  with their Status and Location
        /// </summary>
        /// <returns>A list of <see cref="LibraryAsset"/></returns>
        public IEnumerable<LibraryAsset> GetAll()
        {
            return _context.LibraryAssets
                 .Include(asset => asset.Status)
                 .Include(asset => asset.Location);

        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Where(asset => asset.Id == id).Any();

            var isVideo = _context.LibraryAssets.OfType<Video>().Where(asset => asset.Id == id).Any();

            return isBook ?
                _context.Books.FirstOrDefault(book => book.Id == id).Author :
                _context.Videos.FirstOrDefault(video => video.Id == id).Director
                ?? "Desconocido";

        }

        public LibraryAsset GetByID(int id)
        {
            return GetAll()
                 .FirstOrDefault(asset => asset.Id == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            //return _context.LibraryAssets.FirstOrDefault(asset => asset.Id == id).Location;
            return GetByID(id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            // If the passed id corresponds to a book....
            if (_context.Books.Any(book => book.Id == id))
                return _context.Books
                    .FirstOrDefault(book => book.Id == id).DeweyIndex;


            else return string.Empty;
        }

        public string GetIsbn(int id)
        {
            // If the passed id corresponds to a book....
            if (_context.Books.Any(book => book.Id == id))
                return _context.Books
                    .FirstOrDefault(book => book.Id == id).ISBN;


            else return string.Empty;
        }

        public string GetTitle(int id)
        {
            return GetByID(id).Title;
        }

        public string GetType(int id)
        {
            var book = _context.LibraryAssets.OfType<Book>().Where(b => b.Id == id);

            return book.Any() ? "Libro" : "Película";
        }

    }
}

