using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.Domain.Concrete
{

    public class EFUserRepository : IUserRepository
    {
        //private EFDbContext context = new EFDbContext();
        private IEFDbContext context = DependencyResolver.Current.GetService<IEFDbContext>();

        public IQueryable<User> Users
        {
            get { return context.Users; }
        }

        public void SaveUser(User user)
        {
            if (user.UserID == 0)
            {
                context.Users.Add(user);
            }
            else
            {
                User dbEntry = context.Users.Find(user.UserID);
                if (dbEntry != null)
                {
                    dbEntry.Name = user.Name;
                    dbEntry.Password = user.Password;
                    dbEntry.Email = user.Email;
                }
            }
            context.SaveChangesContext();
        }

        public User DeleteUser(int UserID)
        {
            User dbEntry = context.Users.Find(UserID);
            if (dbEntry != null)
            {
                context.Users.Remove(dbEntry);
                context.SaveChangesContext();
            }
            return dbEntry;
        }

        public User GetByName(string name)
        {
            return context.Users.FirstOrDefault(c => c.Name == name);
        }
    }
}
