using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        //private EFDbContext context = new EFDbContext();
        private IEFDbContext context = DependencyResolver.Current.GetService<IEFDbContext>();

        public IQueryable<Book> Books
        {
            get { return context.Books; }
        }

        public void SaveBook(Book book)
        {
            if (book.BookID == 0)
            {
                book.Category = context.Categories.Find(book.Category.CategoryID);
                context.Books.Add(book);
            }
            else
            {
                Book dbEntry = context.Books.Find(book.BookID);
                if (dbEntry != null)
                {
                    dbEntry.Name            = book.Name;
                    dbEntry.Author          = book.Author;
                    dbEntry.ISBN            = book.ISBN;
                    dbEntry.NumberOfPages   = book.NumberOfPages;
                    dbEntry.Publisher       = book.Publisher;
                    dbEntry.Year            = book.Year;
                    dbEntry.Description     = book.Description;
                    dbEntry.Price           = book.Price;
                    dbEntry.Category        = context.Categories.Find(book.Category.CategoryID) ?? dbEntry.Category;
                    dbEntry.ImageData       = book.ImageData;
                    dbEntry.ImageMimeType   = book.ImageMimeType;
                }
            }
            context.SaveChangesContext();
        }

        public Book DeleteBook(int BookID)
         {
             Book dbEntry = context.Books.Find(BookID);
             if (dbEntry != null)
             {
                 context.Books.Remove(dbEntry);
                 context.SaveChangesContext();
             }
             return dbEntry;
         }


    }
}
