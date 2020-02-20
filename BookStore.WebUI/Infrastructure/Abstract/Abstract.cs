using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.WebUI.Infrastructure.Abstract
{
    public enum StatusMessege { Default, Primary, Success, Info, Warning, Danger};


    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}