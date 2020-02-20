using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System.Configuration;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
        public int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfBooksOnThePage"]); //9

        public BookController(IBookRepository bookRepository)
        {
            this.repository = bookRepository;
        }

        public ViewResult List(Category category, int page = 1)
        {
            ViewBag.SelectedCategory = category;
            bool ShowAllBooks = category == null;
            int CategoryID = category != null ? category.CategoryID : 1 ;
            
            BooksListViewModel model = new BooksListViewModel
            {
                Books = repository.Books
                    .Where(p => ShowAllBooks || p.Category.CategoryID == CategoryID)
                    .OrderBy(p => p.BookID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = (category == null || category.CategoryID == -1)?
                                 repository.Books.Count() :
                                 category.Books.Count()
                },
                CurrentCategory = category
            };
            return View(model);
                         
        }

        public FileContentResult GetImage(int BookId)
        {
            Book prod = repository.Books.FirstOrDefault(p => p.BookID == BookId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        public ViewResult BookDetails(int BookId)
        {
            Book book = repository.Books.FirstOrDefault(p => p.BookID == BookId);
            ViewBag.SelectedCategory = book.Category;
            return View(book);
        }

        public ViewResult SearchBooks(string NameBookSearch, int page = 1)
        {
            IEnumerable<Book> FindedBooks = repository.Books.Where(p => p.Name.Contains(NameBookSearch));
            BooksListViewModel model = new BooksListViewModel
            {
                Books = FindedBooks
                        .OrderBy(p => p.BookID)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = FindedBooks.Count()
                },
                CurrentCategory = null
            };

            ViewBag.NameBookSearch = NameBookSearch;
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchBooks(string NameBookSearch)
        {
            int page = 1;
            IEnumerable<Book> FindedBooks = repository.Books.Where(p => p.Name.Contains(NameBookSearch));
            BooksListViewModel model = new BooksListViewModel
            {
                Books = FindedBooks
                        .OrderBy(p => p.BookID)
                        .Skip((page - 1) * PageSize)
                        .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = FindedBooks.Count()
                },
                CurrentCategory = null
            };

            ViewBag.NameBookSearch = NameBookSearch;
            return View("SearchBooks", model);
        }

    }
}
