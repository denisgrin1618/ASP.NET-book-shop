using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository repository;
        public CategoryController(ICategoryRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(Category category = null)
        {
            ViewBag.SelectedCategoryName = (category == null ? "" : category.Name);
            IEnumerable<Category> categories = repository.Categories
                                            .Select(x => x)
                                            .Distinct()
                                            .OrderBy(x => x.Name);
            return PartialView(categories);
        }



    }
}
