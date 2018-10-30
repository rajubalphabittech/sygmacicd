using System.Linq;
using System.Web;
using System.Web.Mvc;
using atm.services;
using Ninject;

namespace atm.helpers
{
    public class AtmAuthorizeAttribute : AuthorizeAttribute
    {
        [Inject] public IAuthorizationService AuthorizationService { private get; set; }

        public string Path { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //if not logged, it will work as normal Authorize and redirect to the Login
                base.HandleUnauthorizedRequest(filterContext);

            }
            else
            {
                //logged and wihout the role to access it - redirect to the custom controller action
                filterContext.Result = new RedirectResult("~/Apps/ATM/CustomErrors/401.aspx");
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized) return false;

            var user = httpContext.User.Identity.Name;
            var names = user.Split('\\');
            var userName = names.LastOrDefault();
            return AuthorizationService.IsValiduser(userName, Path); ;
        }

    }
}