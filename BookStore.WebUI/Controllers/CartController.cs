using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IBookRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int BookID, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(p => p.BookID == BookID);
            if (book != null)
            {
                cart.AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int BookID, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(p => p.BookID == BookID);
            if (book != null)
            {
                cart.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {

            return PartialView(cart);
        }

        [HttpPost]
        public ActionResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            ViewBag.ServerValidation = true;
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails, (User)Session["CurrentUser"]);
                cart.Clear();
                ViewBag.ServerValidation = false;
                return View("Completed");
            }
            else
            {
                ViewBag.ErrorValidation = true;
                return View(shippingDetails);
            }
        }

        public ViewResult Checkout()
        {
            ViewBag.ServerValidation = true;
            return View(new ShippingDetails());
        }

    }

}
