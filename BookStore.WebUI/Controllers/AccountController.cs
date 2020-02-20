using System.Web.Mvc;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;

namespace BookStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Books", "Admin"));
                }
                else
                {  
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    ViewBag.ErrorValidation = true;
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorValidation = true;
                return View();
            }
        }

    }

}
