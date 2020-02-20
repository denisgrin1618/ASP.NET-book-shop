using System.Collections.Generic;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Models
{
    public class CategoriesListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}