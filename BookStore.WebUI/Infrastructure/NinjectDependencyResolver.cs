using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Moq;
using BookStore.Domain.Entities;
using BookStore.Domain.Abstract;
using BookStore.Domain.Concrete;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Infrastructure.Concrete;

namespace BookStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IBookRepository>().To<EFBookRepository>();
            kernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
            kernel.Bind<IUserRepository>().To<EFUserRepository>();
            kernel.Bind<IOrderRepository>().To<EFOrderRepository>();
            kernel.Bind<IEFDbContext>().To<EFDbContext>();
            
            EmailSettings emailSettings = new EmailSettings
            {
                MailToAddress = ConfigurationManager.AppSettings["LogistMail"],
                MailFromAddress = ConfigurationManager.AppSettings["AdminMail"],
                UseSsl = true,
                Username = ConfigurationManager.AppSettings["AdminMail"],
                Password = ConfigurationManager.AppSettings["AdminPasswordMail"],
                ServerName = ConfigurationManager.AppSettings["AdminServerMail"],
                ServerPort =  Convert.ToInt32(ConfigurationManager.AppSettings["AdminMailPort"]),
                FileLocation = ConfigurationManager.AppSettings["MailsLocation"], 
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}