using System.Linq;
using BookStore.Domain.Entities;
using System.Data.Entity;

namespace BookStore.Domain.Abstract
{

    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int BookID);
    }

    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        void SaveCategory(Category category);
        Category DeleteCategory(int CategoryID);
        Category GetByName(string name);
    }

    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void SaveUser(User user);
        User DeleteUser(int UserID);
        User GetByName(string name);
    }

    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
        IQueryable<Order> GetByUser(string userName);
    }

    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails, User user);
    }

    public interface IEFDbContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderLine> OrderLines { get; set; }
        DbSet<ShippingDetails> ShippingDetailss { get; set; }
        
        void SaveChangesContext();
    }


}
