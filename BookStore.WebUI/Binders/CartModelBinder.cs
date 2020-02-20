
using System;
using System.Web.Mvc;
using BookStore.Domain.Entities;

namespace BookStore.WebUI.Binders
{
  
    public class CartModelBinder : IModelBinder
    {            
        private const string sessionKey = "Cart";
            
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // получим карзину из сесии
            Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];

            // создадим новую карзину если ее нет
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }

            return cart;
        }
    }
    

}