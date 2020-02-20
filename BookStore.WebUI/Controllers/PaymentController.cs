using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ActiveTab = "Payment";
            return View();
        }

    }
}
