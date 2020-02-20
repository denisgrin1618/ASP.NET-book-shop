using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class AboutController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.ActiveTab = "About";
            return View();
        }

    }
}
