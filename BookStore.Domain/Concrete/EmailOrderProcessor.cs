using System.Net.Mail;
using System.Text;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System.Net;
using System.Web.Mvc;
using System;
using System.Web;
using System.Net.Mime;

namespace BookStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress     = "asdfjfkdkfjsdkfjsdfgvjmvgjtk@mail.ru";
        public string MailFromAddress   = "denis.grin1618@gmail.com";
        public bool UseSsl              = true;
        public string Username          = "denis.grin1618@gmail.com";
        public string Password          = "leonardo1618";
        public string ServerName        = "smtp.gmail.com";
        public int ServerPort           = 25; //587; //25 //465
        public bool WriteAsFile         = true;
        public string FileLocation      = @"c:\books_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        private IOrderRepository repository = DependencyResolver.Current.GetService<IOrderRepository>();

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo, User user)
        {
            //Записать заказ в БД
            SaveOrder(cart, shippingInfo, user);

            //Отправить админу письмо с заказом и деталями доставки
            SendOrdersToAdminMail(cart, shippingInfo);

            //отравим письмо клиенту
            SendMailToClient(cart, shippingInfo, user);
          
        }

        private void SendMailToClient(Cart cart, ShippingDetails shippingInfo, User user)
        {
            if (user == null || user.Email == "")
                return;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = GetBodyMail(cart, shippingInfo);

                MailMessage mailMessage = new MailMessage(
                                                emailSettings.MailFromAddress, // From
                                                user.Email, // To
                                                "Новый заказ!", // Subject
                                                body.ToString()); // Body

                mailMessage.BodyEncoding = Encoding.GetEncoding(1251);
                mailMessage.SubjectEncoding = Encoding.GetEncoding(1251);

                try { smtpClient.Send(mailMessage); }
                catch(Exception exp){}
                
            }
        }

        private void SaveOrder(Cart cart, ShippingDetails shippingInfo, User user)
        {
            Order order = new Order();
            order.Date = DateTime.Now;
            order.User = user;
            order.ShippingDetails = shippingInfo;
            foreach (var item in cart.Lines)
            {
                order.OrderLines.Add(new OrderLine { Book = item.Book, Quantity = item.Quantity, Price = item.Book.Price });
            }

            repository.SaveOrder(order);
        }

        private void SendOrdersToAdminMail(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                smtpClient.EnableSsl = true;

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = GetBodyMail(cart, shippingInfo);
                

                MailMessage mailMessage = new MailMessage(
                                                emailSettings.MailFromAddress, // From
                                                emailSettings.MailToAddress, // To
                                                "Новый заказ!", // Subject
                                                body.ToString()); // Body
                mailMessage.BodyEncoding = Encoding.GetEncoding("Windows-1254");
                mailMessage.SubjectEncoding = Encoding.GetEncoding("Windows-1254");  
                
                try { smtpClient.Send(mailMessage); }
                catch (Exception exp) { }

            }
        }

        private StringBuilder GetBodyMail(Cart cart, ShippingDetails shippingInfo)
        {
            StringBuilder body = new StringBuilder()
                .AppendLine("Был оформлен новый заказ")
                .AppendLine("---")
                .AppendLine("Книги:");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Book.Price * line.Quantity;
                body.AppendFormat("{0} x {1} (итого: {2:c}", line.Quantity, line.Book.Name, subtotal);
            }

            body.AppendFormat("Сумма заказа: {0:c}", cart.ComputeTotalValue())
            .AppendLine("---")
            .AppendLine("Адрес отправки:")
            .AppendLine(shippingInfo.Name)
            .AppendLine(shippingInfo.Address)
            .AppendLine(shippingInfo.Description ?? "")
            .AppendLine("---");

            return body;
        }
    }

}