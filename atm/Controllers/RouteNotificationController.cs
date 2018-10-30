using atm.Models;
using atm.services;
using atm.services.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace atm.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class RouteNotificationController : AtmControllerBase
    {
        public INotificationService NotificationService { get; }
        public IRouteService RouteService { get; set; }

        public RouteNotificationController(
            INotificationService notificationService,
            IRouteService routeService,
            IAuthorizationService authorizationService) : base(authorizationService)
        {
            NotificationService = notificationService;
            RouteService = routeService;
        }

        public ActionResult Index()
        {
            if (CurrentAPIEnvironment != "PROD")
                ViewBag.Title = $"Route Notification (ATM VER: {ViewBag.Version}, API: {CurrentAPIEnvironment})";
            else
                ViewBag.Title = "Route Notification";

            return View();
        }

        public async Task<ActionResult> List(SearchRouteSortableViewModel criteria)
        {
            criteria.DeepSearch = true;
            criteria.FilterStartDate = criteria.FilterStartDate.Value.AddHours(-12);
            criteria.FilterEndDate = criteria.FilterEndDate.Value.AddHours(12);
            var model = new RouteNotificationListViewModel((await RouteService.SearchAsync(criteria)).Where(r => r.RouteWasModified));
            return PartialView("_routeList", model);
        }

        [HttpPost]
        public async Task<ActionResult> Notify(string[] tos)
        {
            var notifications = new List<Notification>();
            foreach (var to in tos)
            {
                var item = new Notification
                {
                    To = new[] { "jsetiabudi@sygmanetwork.com" },
                    From = "jsetiabudi@sygmanetwork.com",
                    Subject = "TEST EMAIL",
                    Cc = null,
                    Body = $"Email to {to}"
                };
                notifications.Add(item);
            }
            await NotificationService.SendAsync(notifications);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}
