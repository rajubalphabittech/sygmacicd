using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using atm.helpers;
using atm.Helpers;
using atm.Models;
using atm.services;
using atm.services.models;
using Newtonsoft.Json;

namespace atm.Controllers
{
    [AtmAuthorize(Path = SecuredFeatures.ROUTEMANAGER)]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class RouteManagerController : AtmControllerBase
    {
        public IRouteService RouteService { get; set; }
        public ICenterService CenterService { get; }
        public ILookUpService LookUpService { get; }

        public RouteManagerController(
            IRouteService routeService,
            ICenterService centerService,
            ILookUpService lookUpService,
            IAuthorizationService authorizationService) : base(authorizationService)
        {
            RouteService = routeService;
            CenterService = centerService;
            LookUpService = lookUpService;
        }

        public async Task<ActionResult> Index()
        {
            if (CurrentAPIEnvironment != "PROD")
                ViewBag.Title = $"Route Manager (ATM VER: {ViewBag.Version}, API: {CurrentAPIEnvironment})";
            else
                ViewBag.Title = "Route Manager";

            var model = new RouteManagerViewModel
            {
                CenterSelectList = new SelectList(await CenterService.GetAll(UserName), "SygmaCenterNo", "Center")
            };
            return View(model);
        }

        public async Task<ActionResult> List(string position, SearchRouteSortableViewModel criteria)
        {
            if (criteria.FilterStartDate.HasValue)
            {
                criteria.FilterStartDate = criteria.FilterStartDate.Value.AddHours(12);
            }
            if (criteria.FilterEndDate.HasValue)
            {
                criteria.FilterEndDate = criteria.FilterEndDate.Value.AddHours(12);
            }

            var model = new RouteListViewModel(await RouteService.SearchAsync(criteria))
            {
                Position = position,
                SortField = criteria.SortField,
                SortDirection = criteria.SortDirection,
                StopProximitySearch = criteria.NearRoutePlanId != 0
            };

            return PartialView("_routeList", model);
        }

        public async Task<ActionResult> Stops(int id, int centerNumber, string routeNumber, string position)
        {
            var model = new RouteStopViewModel(await RouteService.GetByRouteIdAndCenterNumberAndRouteNumberAsync(id, centerNumber, routeNumber));
            model.Position = position;
            return PartialView("_stopList", model);
        }

        public async Task<JsonResult> Details(int id, int centerNumber, string routeNumber)
        {
            var raw = await RouteService.GetByRouteIdAndCenterNumberAndRouteNumberAsync(id, centerNumber, routeNumber);
            var model = new RouteForMapModel(raw);
            var result = JsonConvert.SerializeObject(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Center(int sygmaCenterNo)
        {
            var model = await CenterService.GetLocationByNoAsync(sygmaCenterNo);
            return Json(JsonConvert.SerializeObject(model), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Destination(int id, int centerNumber, string routeNumber, DateTime sourceStopPlannedDeliveryDateTime)
        {
            if (id != 0)
            {
                var model = new RouteViewModel(await RouteService.GetByRouteIdAndCenterNumberAndRouteNumberAsync(id, centerNumber, routeNumber));
                return Json(JsonConvert.SerializeObject(model), JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (string.IsNullOrEmpty(routeNumber)) return null;

                var weekending = Dates.GetWeekending(sourceStopPlannedDeliveryDateTime.Date).AddHours(12);
                var startWeek = weekending.AddDays(-6);
                var criteria = new SearchRoute
                {
                    CenterNumber = centerNumber,
                    RouteNumber = routeNumber,
                    FilterStartDate = startWeek,
                    FilterEndDate = weekending
                };

                var routes = await RouteService.SearchAsync(criteria);
                if (routes == null || !routes.Any() || routes.Count() == 0) return null;

                var model = new RouteViewModel(routes.FirstOrDefault());
                return Json(JsonConvert.SerializeObject(model), JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Move(int id, int centerNumber, int routePlanId = 0, int destinationRouteId = 0)
        {
            StopMoveViewModel model = new StopMoveViewModel
            {
                SourceRouteId = id,
                SourceRoutePlanId = routePlanId,
                SygmaCenterNo = centerNumber,
                DestinationRouteId = destinationRouteId,
                RouteList = (await LookUpService.GetRoutesForCenterThisWeek(centerNumber)).OrderBy(x => x.Value)
            };
            return PartialView("_stopMove", model);
        }

        [HttpPost]
        public async Task<ActionResult> Move(MoveStopViewModel model)
        {
            try
            {
                model.CreatedBy = LastFirstName.ToUpperInvariant();
                await RouteService.MoveStopAsync(model);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                if (e.Message.Contains("404"))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
                if (e.Message.Contains("409"))
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Conflict);
                throw e;
            }
        }
        
        public async Task<JsonResult> StopsData(int id, int centerNumber)
        {
            var model = await RouteService.GetByRouteIdAndCenterNumberAndRouteNumberAsync(id, centerNumber);
            return Json(JsonConvert.SerializeObject(model), JsonRequestBehavior.AllowGet);
        }
    }
}