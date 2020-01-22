using MVCEUprava.Models;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCEUprava.App_Start
{
    public class AllUsersAuthorize: AuthorizeAttribute
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
            if(name == "admin@gmail.com")
            {
                return true;
            }
            if (db.tblKorisnikAplikacijes.Where(x => x.Email == name).ToList().Count() == 0)
            {
                return false;
            }
            return true;
        }
    }
}