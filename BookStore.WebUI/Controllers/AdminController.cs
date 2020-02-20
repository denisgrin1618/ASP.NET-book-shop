using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using System.Configuration;

namespace BookStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private IBookRepository repositoryBook;
        private ICategoryRepository repositoryCategory;
        private IUserRepository repositoryUser;
        private int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfBooksOnThePage"]); //9

        public AdminController(IBookRepository repoBook, ICategoryRepository repoCategory, IUserRepository repoUser)
        {
            repositoryBook = repoBook;
            repositoryCategory = repoCategory;
            repositoryUser = repoUser;
        }

        public ViewResult Books(int page = 1)
        {
            ViewBag.SelectedAdminTab = "Books";
            BooksListViewModel model = new BooksListViewModel
            {
                Books = repositoryBook.Books
                    .OrderBy(p => p.BookID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repositoryBook.Books.Count()
                },
                CurrentCategory = null
            };

            return View(model);
        }

        public ViewResult Categories(int page = 1)
        {
            ViewBag.SelectedAdminTab = "Categories";
            CategoriesListViewModel model = new CategoriesListViewModel
            {
                Categories = repositoryCategory.Categories
                    .OrderBy(p => p.CategoryID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repositoryCategory.Categories.Count()
                }
            };

            return View(model);
        }

        public ViewResult Users(int page = 1)
        {
            ViewBag.SelectedAdminTab = "Users";
            UsersListViewModel model = new UsersListViewModel
            {
                Users = repositoryUser.Users
                    .OrderBy(p => p.UserID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repositoryUser.Users.Count()
                }
            };

            return View(model);
        }

        public ViewResult EditBook(int BookId)
        {
            Book product = repositoryBook.Books.FirstOrDefault(p => p.BookID == BookId);
            ViewBag.SelectedAdminTab = "Books";
            return View(product);
        }

        public ViewResult Orders(int UserID)
        {
            User user = repositoryUser.Users.FirstOrDefault(p => p.UserID == UserID);
            ViewBag.SelectedAdminTab = "Users";
            ViewBag.UserName = user.Name;
            return View(user.Orders);
        }

        [HttpPost]
        public ActionResult EditBook(Book book, HttpPostedFileBase image, Category category)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                     book.ImageMimeType = image.ContentType;
                     book.ImageData = new byte[image.ContentLength];
                     image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }
                repositoryBook.SaveBook(book);
                TempData["message"] = string.Format("Книга {0} была сохранена", book.Name);
                ViewBag.SelectedAdminTab = "Books";
                return RedirectToAction("Books");
            }
            else
            {
                ViewBag.SelectedAdminTab = "Books";
                ViewBag.ErrorValidation = true;
                return View(book);
            }
        }

        public ViewResult CreateBook()
        {
            ViewBag.SelectedAdminTab = "Books";
            return View("EditBook", new Book());
        }

        [HttpPost]
        public ActionResult DeleteBook(int BookID)
        {
            Book deletedBook = repositoryBook.DeleteBook(BookID);
            if (deletedBook != null)
            {
                TempData["message"] = string.Format("Книга {0} была удалена", deletedBook.Name);
            }
            ViewBag.SelectedAdminTab = "Books";
            return RedirectToAction("Books");
        }

        public PartialViewResult DropdownCategories(string category)
        {
            IEnumerable<Category> categories = repositoryCategory.Categories
                                            .Select(x => x)
                                            .Distinct()
                                            .OrderBy(x => x.Name);
            
            ViewBag.SelectedCategory = category;
            return PartialView(categories);
        }

        public ViewResult CreateCategory()
        {
            ViewBag.SelectedAdminTab = "Categories";
            return View("EditCategory", new Category());
        }

        public ViewResult EditCategory(int CategoryID)
        {
            Category category = repositoryCategory.Categories.FirstOrDefault(p => p.CategoryID == CategoryID);
            ViewBag.SelectedAdminTab = "Categories";
            return View(category);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int CategoryID)
        {
            Category deletedCategory = repositoryCategory.DeleteCategory(CategoryID);
            if (deletedCategory != null)
            {
                TempData["message"] = string.Format("Категория {0} была удалена", deletedCategory.Name);
            }
            ViewBag.SelectedAdminTab = "Categories";
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                repositoryCategory.SaveCategory(category);
                TempData["message"] = string.Format("Категория {0} была сохранена", category.Name);
                ViewBag.SelectedAdminTab = "Categories";
                return RedirectToAction("Categories");
            }
            else
            {
                ViewBag.SelectedAdminTab = "Categories";
                ViewBag.ErrorValidation = true;
                return View(category);
            }
        }

        public ViewResult EditUser(int UserID)
        {
            User user = repositoryUser.Users.FirstOrDefault(p => p.UserID == UserID);
            ViewBag.SelectedAdminTab = "Users";
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {               
                repositoryUser.SaveUser(user);
                TempData["message"] = string.Format("Пользователь {0} был сохранен", user.Name);
                ViewBag.SelectedAdminTab = "Users";
                return RedirectToAction("Users");
            }
            else
            {
                ViewBag.SelectedAdminTab = "Users";
                ViewBag.ErrorValidation = true;
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(int UserID)
        {
            User deletedUser = repositoryUser.DeleteUser(UserID);
            if (deletedUser != null)
            {
                TempData["message"] = string.Format("Пользователь {0} был удален", deletedUser.Name);
            }
            ViewBag.SelectedAdminTab = "Users";
            return RedirectToAction("Users");
        }

        public ViewResult CreateUser()
        {
            ViewBag.SelectedAdminTab = "Users";
            return View("EditUser", new User());
        }

        

    }
}
