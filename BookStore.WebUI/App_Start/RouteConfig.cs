using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null,
                            "",
                            new
                            {
                                controller = "Book",
                                action = "List",
                                category = (string)null,
                                page = 1
                            });

            routes.MapRoute(null,
                            "Page{page}",
                            new { controller = "Book", action = "List", category = (string)null },
                            new { page = @"\d+" }
                             );

            routes.MapRoute(null,
                           "Admin/{action}/Page{page}",
                           new { controller = "Admin", page = 1 },
                           new { page = @"\d+" }
                           );

            routes.MapRoute(null,
                            "Admin",
                            new { controller = "Admin", action = "Books" }
                            );    
                
            routes.MapRoute(null,
                            "{category}",
                            new { controller = "Book", action = "List", page = 1 }
                            );

            routes.MapRoute(null,
                            "{category}/Page{page}",
                            new { controller = "Book", action = "List" },
                             new { page = @"\d+" }
                            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}