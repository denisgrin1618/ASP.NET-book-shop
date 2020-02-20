using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BookStore.WebUI.Infrastructure;
using BookStore.WebUI.Infrastructure.Concrete;
using BookStore.Domain.Concrete;
using BookStore.Domain.Entities;
using System.Data.Entity;
using BookStore.WebUI.Binders;

namespace BookStore.WebUI
{

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<EFDbContext>(new DbInitializer());
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            ModelBinderProviders.BinderProviders.Add(new CategoryModelBinderProvider());
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}