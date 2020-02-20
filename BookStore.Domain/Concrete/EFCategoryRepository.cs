using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.Domain.Concrete
{

    public class EFCategoryRepository : ICategoryRepository
    {
        //private EFDbContext context = new EFDbContext();
        private IEFDbContext context = DependencyResolver.Current.GetService<IEFDbContext>();

        public IQueryable<Category> Categories
        {
            get { return context.Categories; }
        }

        public void SaveCategory(Category category)
        {
            if (category.CategoryID == 0)
            {
                context.Categories.Add(category);
            }
            else
            {
                Category dbEntry = context.Categories.Find(category.CategoryID);
                if (dbEntry != null)
                {
                    dbEntry.Name = category.Name;
                    dbEntry.Description = category.Description;               
                }
            }
            context.SaveChangesContext();
        }

        public Category DeleteCategory(int CategoryID)
        {
            Category dbEntry = context.Categories.Find(CategoryID);
            if (dbEntry != null)
            {
                context.Categories.Remove(dbEntry);
                context.SaveChangesContext();
            }
            return dbEntry;
        }

        public Category GetByName(string name)
        {
            return context.Categories.FirstOrDefault(c => c.Name == name);
        }
    }
}
