using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace BookStore.Domain.Concrete
{
    public class EFOrderRepository : IOrderRepository
    {
        //private EFDbContext context = new EFDbContext();
        private IEFDbContext context = DependencyResolver.Current.GetService<IEFDbContext>();

        public IQueryable<Order> Orders
        {
            get { return context.Orders; }
        }

        public void SaveOrder(Order order)
        {
            ShippingDetails shippingDetail = null;
            if (order.ShippingDetails != null && order.ShippingDetails.ShippingDetailsID == 0)
            {
                shippingDetail = new ShippingDetails
                {
                    Name = order.ShippingDetails.Name,
                    Address = order.ShippingDetails.Address,
                    Description = order.ShippingDetails.Description
                };
                context.ShippingDetailss.Add(shippingDetail);
                context.SaveChangesContext();
            }
            else
            {
                shippingDetail = context.ShippingDetailss.Find(shippingDetail.ShippingDetailsID);
            }

            User user = (order.User == null ? null : context.Users.Find(order.User.UserID));
            List<OrderLine> OrderLines = order.OrderLines.ToList();

            //сохраним сам заказ
            if (order.OrderID == 0)
            {
                order.ShippingDetails = shippingDetail;
                order.User = user;
                order.OrderLines = null;
                context.Orders.Add(order);
            }
            else
            {
                Order dbEntry = context.Orders.Find(order.OrderID);
                if (dbEntry != null)
                {
                    dbEntry.Date = order.Date;
                    dbEntry.ShippingDetails = shippingDetail;
                    dbEntry.User = user;
                }
            }
            context.SaveChangesContext();

            //Добавим строки заказа
            foreach (OrderLine line in OrderLines)
            {
                OrderLine newOrderLine = new OrderLine { 
                    Book = context.Books.Find(line.Book.BookID),
                    Price = line.Price,
                    Quantity = line.Quantity,
                    Order = context.Orders.Find(order.OrderID)
                };
                context.OrderLines.Add(newOrderLine);
            }
            context.SaveChangesContext();
        }

        public IQueryable<Order> GetByUser(string userName)
        {
            return context.Orders.Where(c => c.User.Name == userName);
        }

        
    }
}
