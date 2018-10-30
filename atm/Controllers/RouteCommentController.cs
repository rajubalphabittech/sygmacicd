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
    public class RouteCommentController : AtmControllerBase
    {
        public ICommentService CommentService { get; }
        public IRouteService RouteService { get; }
        public ILookUpService LookUpService { get; }

        public RouteCommentController(
            ICommentService commentService,
            IRouteService routeService,
            ILookUpService lookUpService,
            IAuthorizationService authorizationService) : base(authorizationService)
        {
            CommentService = commentService;
            RouteService = routeService;
            LookUpService = lookUpService;
        }

        public async Task<ActionResult> ListByBillToShipTo(int billTo, int shipTo, int centerNumber, int routePlanId, int stopNumber, string screen)
        {
            var model = new RouteCommentListViewModel(await CommentService.GetCommentsByRoutePlanId(routePlanId, centerNumber), billTo, shipTo, centerNumber, routePlanId.ToString(), stopNumber, screen);
            ViewBag.UserName = this.UserName;
            ViewBag.LastFirstName = this.LastFirstName;
            return PartialView("_list", model);
        }

        public async Task<ActionResult> ListByRouteId(int routeId, int centerNumber)
        {
            var route = await RouteService.GetByRouteIdAndCenterNumberAndRouteNumberAsync(routeId, centerNumber);
            var comments = await CommentService.GetCommentsByRoutePlanIds(route.RouteStops.Select(r => r.RoutePlanId).ToArray(), centerNumber);
            var routeComments = from comment in comments
                                group comment by new { comment.SecondaryRecordId, comment.PrimaryRecordId, comment.CenterNumber } into groupcomments
                                select new RouteAggregateCommentViewModel
                                {
                                    RouteId = route.RouteId,
                                    RoutePlanId = int.Parse(groupcomments.Key.SecondaryRecordId),
                                    RouteNumber = route.RouteNumber,
                                    StopNumber = (int)(route.RouteStops.SingleOrDefault(s => s.RoutePlanId.ToString() == groupcomments.Key.SecondaryRecordId).StopNumber),
                                    Comments = ToRouteCommentViewModel(groupcomments.OrderByDescending(s => s.CreatedDate).ToList())
                                };

            ViewBag.UserName = this.UserName;
            ViewBag.LastFirstName = this.LastFirstName;
            return PartialView("_listByRoute", routeComments.OrderBy(r => r.StopNumber));
        }

        private List<RouteCommentViewModel> ToRouteCommentViewModel(List<Comment> comments)
        {
            var list = new List<RouteCommentViewModel>();
            foreach (var c in comments)
            {
                list.Add(new RouteCommentViewModel(c));
            }
            return list;
        }

        public async Task<ActionResult> LastCustomerCommunication(int centerNumber, int routePlanId)
        {
            var comments = await CommentService.GetCommentsByRoutePlanId(routePlanId, centerNumber);
            var model = comments.Where(c => c.ShortComment == "Customer Communication").OrderByDescending(c => c.CreatedDate).ThenByDescending(c => c.CommentId).FirstOrDefault();
            return Json(JsonConvert.SerializeObject(model), JsonRequestBehavior.AllowGet);
        }

        // GET: Comment/Create
        public ActionResult Create(int billTo, int shipTo, int centerNumber, int routePlanId, int stopNumber, string screen)
        {
            var model = new CreateRouteCommentModel
            {
                BillTo = billTo,
                ShipTo = shipTo,
                CenterNumber = centerNumber,
                RoutePlanId = routePlanId,
                StopNumber = stopNumber,
                Screen = screen
            };
            return PartialView("_create", model);
        }

        // POST: Comment/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateRouteCommentModel comment)
        {
            try
            {
                await CommentService.AddCommentAsync(new services.models.AddComment
                {
                    CenterNumber = comment.CenterNumber,
                    CreatedBy = LastFirstName.ToUpperInvariant(),
                    Status = (comment.IsInternal ? 2 : 3),
                    PrimaryRecordId = $"{comment.BillTo}-{comment.ShipTo}",
                    SecondaryRecordId = comment.RoutePlanId.ToString(),
                    Screen = comment.Screen,
                    ShortComment = comment.Category,
                    LongComment = comment.LongComment
                });

                return RedirectToAction("ListByBillToShipTo", new { billTo = comment.BillTo, shipTo = comment.ShipTo, stopNumber = comment.StopNumber, routePlanId = comment.RoutePlanId, centerNumber = comment.CenterNumber, screen = comment.Screen });
            }
            catch
            {
                return null;
            }
        }

        // GET: Comment/CommonCreate
        public async Task<ActionResult> CommonCreate(int centerNumber, string screen)
        {
            var model = new CreateRouteCommentModelWithOptions
            {
                CenterNumber = centerNumber,
                Screen = screen,
                RouteList = new SelectList((await LookUpService.GetRoutesForCenterThisWeek(centerNumber)).OrderBy(x => x.Value), "Key", "Value")
            };
            return PartialView("_commonCreate", model);
        }

        // POST: Comment/CommonCreate
        [HttpPost]
        public async Task<ActionResult> CommonCreate(CreateRouteCommentModel comment)
        {
            try
            {
                await CommentService.AddCommentAsync(new services.models.AddComment
                {
                    CenterNumber = comment.CenterNumber,
                    CreatedBy = LastFirstName.ToUpperInvariant(),
                    Status = (comment.IsInternal ? 2 : 3),
                    PrimaryRecordId = $"{comment.BillTo}-{comment.ShipTo}",
                    SecondaryRecordId = comment.RoutePlanId.ToString(),
                    Screen = comment.Screen,
                    ShortComment = comment.Category,
                    LongComment = comment.LongComment
                });

                return RedirectToAction("ListByBillToShipTo", new { billTo = comment.BillTo, shipTo = comment.ShipTo, stopNumber = comment.StopNumber, routePlanId = comment.RoutePlanId, centerNumber = comment.CenterNumber, screen = comment.Screen });
            }
            catch
            {
                return null;
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id, int billTo, int shipTo, int centerNumber, int routePlanId, int stopNumber, string comment, string category, bool isInternal, string screen)
        {
            var model = new UpdateRouteCommentModel
            {
                CommentId = id,
                IsInternal = isInternal,
                Category = category,
                LongComment = comment,
                BillTo = billTo,
                ShipTo = shipTo,
                CenterNumber = centerNumber,
                RoutePlanId = routePlanId,
                StopNumber = stopNumber,
                Screen = screen
            };
            return PartialView("_edit", model);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, UpdateRouteCommentModel comment)
        {
            try
            {
                var oldComment = new RouteCommentViewModel(await CommentService.GetByCommentIdAsync(id));
                await CommentService.UpdateCommentAsync(new services.models.UpdateComment
                {
                    CommentId = id,
                    Status = (comment.IsInternal ? 2 : 3),
                    ShortComment = comment.Category,
                    LongComment = comment.LongComment,
                    UpdatedBy = LastFirstName.ToUpperInvariant()
                });

                return RedirectToAction("ListByBillToShipTo", new { billTo = comment.BillTo, shipTo = comment.ShipTo, stopNumber = comment.StopNumber, routePlanId = oldComment.RoutePlanId, centerNumber = comment.CenterNumber, screen = comment.Screen });
            }
            catch
            {
                return null;
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<JsonResult> RouteStops(int routeId, int centerNumber)
        {
            var route = await RouteService.GetByRouteIdAndCenterNumberAndRouteNumberAsync(routeId, centerNumber);
            List<BasicStopModel> model = new List<BasicStopModel>();
            foreach (var stop in route.RouteStops.OrderBy(s => s.StopNumber))
            {
                model.Add(new BasicStopModel
                {
                    RoutePlanId = stop.RoutePlanId,
                    BillTo = (int)stop.BillTo,
                    ShipTo = (int)stop.ShipTo,
                    CenterNumber = stop.SygmaCenterNo,
                    StopNumber = (int)stop.StopNumber
                });
            }
            return Json(JsonConvert.SerializeObject(model), JsonRequestBehavior.AllowGet);
        }
    }
}
