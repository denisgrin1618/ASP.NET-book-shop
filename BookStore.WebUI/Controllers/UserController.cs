using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Infrastructure.Abstract;

namespace BookStore.WebUI.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository repository;

        public UserController(IUserRepository repo)
        {
            repository = repo;
        }

        [ChildActionOnly] 
        public PartialViewResult Registration(User user = null)
        {
            return PartialView(user == null ? new User() : user);
        }

        [HttpPost]
        public ActionResult RegistrationPost(User user)
        {
            if (ModelState.IsValid)
            {
                repository.SaveUser(user);
                return RedirectToAction("List", "Book");
            }
            else
            {
                return View(user);
            }
        }

        [ChildActionOnly] 
        public PartialViewResult Authorization()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AuthorizationPost(string Login, string Password)
        {
            Session["CurrentUser"] = null;
            User user = repository.GetByName(Login);
            if (user != null && user.Password == Password)
            {
                Session["CurrentUser"] = user;
            }

            return RedirectToAction("List", "Book");
        }

        [ChildActionOnly]
        public PartialViewResult NavMenu(string ActiveTab)
        {
            ViewBag.ActiveTab = ActiveTab;
            return PartialView();
        }

       
    }
}
