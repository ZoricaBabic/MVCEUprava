using MVCEUprava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCEUprava.App_Start
{
    public class AdminAuthorize: AuthorizeAttribute
    {
        private LicneKarteDBEntities db = new LicneKarteDBEntities();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
            {
                return false;
            }
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string name = ticket.Name;
            //string email = FormsAuthentication.FormsCookieName;
            if (name != "admin@gmail.com")
            {
                return false;
            }
            return true;
        }
    }
}