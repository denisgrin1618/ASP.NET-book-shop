using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{

    public class Category
    {

        public Category()
        {
            Books = new List<Book>();
        }

        [HiddenInput(DisplayValue = false)]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите название категории")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Пожалуйста введите описание категории")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }

    public class Book
    {
        [HiddenInput(DisplayValue = false)]
        public int BookID { get; set; }

        [Required(ErrorMessage = "Пожалуйста укажите категорию")]
        [Display(Name = "Категория")]
        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите название книги")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Издатель")]
        public string Publisher { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }
        public string ISBN { get; set; }

        [Display(Name = "Количество страниц")]
        public int NumberOfPages { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Пожалуйста введите описание книги")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста введите положительную цену")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        
    }

    public class User
    {
        public User() 
        {
            Orders = new List<Order>();
        }

        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите логин")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Электронная почта")]
        [RegularExpression("[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}", ErrorMessage="Неправильный формат Emal")]
        public string Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public Order() {
            OrderLines = new List<OrderLine>();
        }

        [HiddenInput(DisplayValue = false)]
        public int OrderID { get; set; }

        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Покупатель")]
        public virtual User User { get; set; }

        public virtual ShippingDetails ShippingDetails { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }

    public class OrderLine
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderLineID { get; set; }

        public virtual Order Order { get; set; }

        public virtual Book Book { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

    public class ShippingDetails
    {
        [HiddenInput(DisplayValue = false)]
        public int ShippingDetailsID { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите полное имя получателя")]
        [Display(Name = "ФИО получателя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите адрес")]
        [Display(Name = "Адресс")]
        public string Address { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Комментарий к посылке")]
        public string Description { get; set; }

    }
}
