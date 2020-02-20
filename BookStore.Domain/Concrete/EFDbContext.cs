using BookStore.Domain.Entities;
using System.Data.Entity;
using BookStore.Domain.Abstract;

namespace BookStore.Domain.Concrete
{
    public class EFDbContext : DbContext, IEFDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<ShippingDetails> ShippingDetailss { get; set; }
        public void SaveChangesContext()
        {
            this.SaveChanges();
        }

    }


    public class DbInitializer : CreateDatabaseIfNotExists<EFDbContext>
    {
        protected override void Seed(EFDbContext db)
        {

            Category category = new Category { Name = "ASP", Description = "ASP.NET (Active Server Pages для .NET) - Технология создания веб-приложений и веб-сервисов от компании Майкрософт." };

            Book book1 = new Book
            {
                Name = "Admin",
                Author = "Марк Симан",
                Category = category,
                ISBN = "978-5-496-00657-6",
                NumberOfPages = 364,
                Price = 40,
                Publisher = "Питер",
                Year = 2014,
                Description = "Внедрение зависимостей в .NET: Внедрение зависимостей позволяет уменьшить сильное связывание между программными компонентами. Вместо жесткого кодирования зависимостей (например, драйвера какой-либо базы данных) внедряется список сервисов, в которых может нуждаться компонент. После этого сервисы подключаются третьей стороной. Такой подход обеспечивает лучшее управление будущими изменениями и решение проблем в разрабатываемом программном обеспечении."

            };

            Book book2 = new Book
            {
                Name = "Microsoft ASP.NET 4 с примерами на C# 2010 для профессионалов",
                Author = "Мэтью Мак-Дональд",
                Category = category,
                ISBN = "978-5-496-00657-6",
                NumberOfPages = 1424,
                Price = 140,
                Publisher = "Вильямс",
                Year = 2011,
                Description = "ASP.NET 4 представляет собой новую версию революционной технологии ASP.NET от Microsoft. Она является основным стандартом для создания динамических веб-страниц на платформе Windows. Эта книга предлагает исчерпывающий, снабженный большим числом примеров подход к изучению построения и развертывания динамических веб-решений от Microsoft. Настоящее издание было полностью обновлено и дополнено с учетом последней версии ASP.NET, и теперь включает описание ASP.NET MVC, ASP.NET AJAX 4, ASP.NET Dynamic Data и Silverlight 3."

            };

            Book book3 = new Book
            {
                Name = "ASP.NET MVC Framework с примерами на C# для профессионалов",
                Author = "Стивен Сандерсон",
                Category = category,
                ISBN = "978-5-496-00657-6",
                NumberOfPages = 510,
                Price = 40,
                Publisher = "Вильямс",
                Year = 2010,
                Description = "Новая среда ASP.NET MVC Framework представляет собой самое значительное изменение в программных средствах разработки веб-приложений от корпорации Microsoft после первого выпуска платформы ASP.NET в 2002 году. Она дает разработчикам больше возможностей для управления HTML-разметкой, схемой URL и обработкой запросов и ответов."
            };
            db.Books.Add(book1);
            db.Books.Add(book2);
            db.Books.Add(book3);

        }
    }
}
