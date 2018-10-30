using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using atm.services;
using atm.services.models;
using SygmaFramework;
using System.DirectoryServices.AccountManagement;
using System.Configuration;

namespace atm.Controllers
{
    public abstract class AtmControllerBase : Controller
    {
        public const char DIR_SEPERATOR = '\\';

        public IAuthorizationService AuthorizationService { get; set; }

        protected string CurrentAPIEnvironment
        {
            get
            {
                if (ConfigurationManager.AppSettings["APIUrl"].Contains("localhost"))
                    return "DEV";
                else if (ConfigurationManager.AppSettings["APIUrl"].Contains("7078"))
                    return "QA";
                else if (ConfigurationManager.AppSettings["APIUrl"].Contains("7870"))
                    return "PROD";
                else if (ConfigurationManager.AppSettings["APIUrl"].Contains("8707"))
                    return "STG";
                else return "DEV";
            }
        }

        protected string UserName
        {
            get { return GetUserName(this.ControllerContext.HttpContext); }
        }

        protected string LastFirstName
        {
            get
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
                return $"{userPrincipal.Surname}, {userPrincipal.GivenName}";
            }
        }

        protected string DisplayName
        {
            get
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
                return $"{userPrincipal.DisplayName}";
            }
        }

        protected AtmControllerBase(IAuthorizationService authorizationService)
        {
            AuthorizationService = authorizationService;
            ViewBag.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetUserName(HttpContextBase context)
        {
            var user = context.User.Identity.Name;
            var names = user.Split(DIR_SEPERATOR);
            return names.LastOrDefault();
        }

        public static bool IsSiteAdmin(HttpRequest Request)
        {
            string fullUserName = Request.ServerVariables["LOGON_USER"].ToString();
            return IsSiteAdmin(Request, fullUserName);
        }
        public static bool IsSiteAdmin(HttpRequest Request, string userName)
        {
            return (userName.EndsWith("bspa6589") || userName.EndsWith("msiv2896") || userName.EndsWith("mbai5105") || userName.EndsWith("skru7538"));
        }

        public static string GetUserName(string fullUserName)
        {
            int seperatorIndex = fullUserName.LastIndexOf(DIR_SEPERATOR);
            return fullUserName.Substring(seperatorIndex + 1);
        }
    }
}