using atm.helpers;
using atm.services;
using atm.services.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace atm.Controllers
{
    public class HomeController : AtmControllerBase
    {
        protected IMenuService MenuService { get; set; }


        public HomeController(IMenuService menuService, IAuthorizationService authorizationService) : base(authorizationService)
        {
            MenuService = menuService;
        }

        // GET: Home
        public ActionResult Index()
        {
            var names = User.Identity.Name.Split('\\');

            var userName = names[1];

            var firstPageUrlUserCanAccess = MenuService.GetFirstPageUrlForUserName(userName);
            if (firstPageUrlUserCanAccess == "")
            {
                return Redirect("~/Apps/ATM/CustomErrors/401.aspx");
            }
            return Redirect(firstPageUrlUserCanAccess);
        }

        public ActionResult Menu()
        {
            var names = User.Identity.Name.Split('\\');
            var userName = names[1];

            Menu model= MenuService.GetByUserName(userName);
            return PartialView("_menu", model);
        }
    }
}