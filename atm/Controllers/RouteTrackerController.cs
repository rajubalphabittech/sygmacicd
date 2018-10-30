using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using atm.helpers;
using atm.Helpers;
using atm.Models;
using atm.services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using atm.services.models;
using PagedList;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace atm.Controllers
{
    [AtmAuthorize(Path = SecuredFeatures.ROUTETRACKER)]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class RouteTrackerController : AtmControllerBase
    {
        public IRouteService RouteService { get; set; }
        public ICenterService CenterService { get; }
        public ILookUpService LookUpService { get; }
        public ICommentService CommentService { get; }

        public RouteTrackerController(
            IRouteService routeService,
            ICenterService centerService,
            ILookUpService lookUpService,
            ICommentService commentService,
            IAuthorizationService authorizationService) : base(authorizationService)
        {
            RouteService = routeService;
            CenterService = centerService;
            LookUpService = lookUpService;
            CommentService = commentService;
        }

        public async Task<ActionResult> Index()
        {
            if (CurrentAPIEnvironment != "PROD")
                ViewBag.Title = $"Route Tracker (ATM VER: {ViewBag.Version}, API: {CurrentAPIEnvironment})";
            else
                ViewBag.Title = "Route Tracker";
            var ModifiedTypes = new List<SelectListItem>
                {
                new SelectListItem { Text = "Removed", Value="removed"},
                new SelectListItem { Text = "Added", Value="added"},
                new SelectListItem { Text = "Time Adjusted", Value="timeChanged"}
                };

            var model = new RouteTrackerViewModel
            {
                CenterSelectList = new SelectList(await CenterService.GetAll(UserName), "SygmaCenterNo", "Center"),
                //FilterByDispatchDay = new MultiSelectList((await LookUpService.GetAllRouteFilters((int)RouteFilterTypes.RouteNumbers)).OrderBy(x => x.DisplayOrder), "Value", "Text"),
                ModifiedTypeSelectList = new SelectList(ModifiedTypes, "Value", "Text"),
                ConceptTypeList = new SelectList(await LookUpService.GetAllConcepts(), "ConceptId", "Concept")
            };

            var dates = (await LookUpService.GetAllRouteFilters((int)RouteFilterTypes.DateRange)).Where(s => s.IsActive == 1).OrderBy(x => x.DisplayOrder);
            foreach (var d in dates)
            {
                if (d.Value == "0") d.Text = d.Text + " - " + (((((int)DateTime.Today.DayOfWeek)) % 7) + 1).ToString() + "00s";
                else if (d.Value == "1") d.Text = d.Text + " - " + (((((int)DateTime.Today.DayOfWeek) + 1) % 7) + 1).ToString() + "00s";
                else if (d.Value == "3") d.Text = d.Text + " - " + (DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? "7" : (((((int)DateTime.Today.DayOfWeek) - 1) % 7) + 1).ToString()) + "00s";
            }
            model.DateRangeList = new SelectList(dates, "Value", "Text");

            return View(model);
        }

        public async Task<ActionResult> List(SearchRouteSortableViewModel criteria)
        {
            var model = new RouteListViewModel(await RouteService.SearchAsync(criteria))
            {
                RouteColumnOption = new ColumnOptionViewModel(await LookUpService.GetColumnOptionsForUserAndPage(UserName, "ROUTETRACKERROUTES"), ColumnTypes.Route)
            };
            return PartialView("_routeList", model);
        }

        public async Task<ActionResult> Stops(int routeId)
        {
            var routeWithStops = await RouteService.GetByRouteIdAsync(routeId);
            var model = new RouteStopViewModel(routeWithStops)
            {
                StopColumnOption = new ColumnOptionViewModel(await LookUpService.GetColumnOptionsForUserAndPage(UserName, "ROUTETRACKERSTOPS"), ColumnTypes.Stop)
            };
            return PartialView("_stopList", model);
        }

        public async Task<ActionResult> ExcelExport(int sygmaCenterNumber, string startDate, string stopDate)
        {
            //TODO: Update to pass in the Route Number, Route Name, 
            var RoutesList = new List<Route>(await RouteService.GetRoutesWithStops("", "", sygmaCenterNumber.ToString(), DateTime.Parse(startDate), DateTime.Parse(stopDate), "", "", true));

            using (var p = new ExcelPackage())
            {
                //A workbook must have at least on cell, so lets add one... 
                var worksheetName = DateTime.Parse(startDate).ToString("dd.M.yyyy") + " - " + DateTime.Parse(stopDate).ToString("dd.M.yyyy");
                var ws = p.Workbook.Worksheets.Add(worksheetName);
                //To set values in the spreadsheet use the Cells indexer.
                ws.Cells[1, 1].Value = "Route";
                ws.Cells[1, 2].Value = "Stop";
                ws.Cells[1, 3].Value = "Bill To";
                ws.Cells[1, 4].Value = "Ship To";
                ws.Cells[1, 5].Value = "Customer Name";
                ws.Cells[1, 6].Value = "City";
                ws.Cells[1, 7].Value = "St";
                ws.Cells[1, 8].Value = "Phone No.";
                ws.Cells[1, 9].Value = "Normal Delivery Day & Time";
                ws.Cells[1, 10].Value = "Total Hours Route is Running Early or Late";
                ws.Cells[1, 11].Value = "Enter \"Early\" if Route is Running Early or Enter \"Late\" if Route is Running Late";
                ws.Cells[1, 12].Value = "Adjusted Delivery Date & Time Early or Late";
                ws.Cells[1, 13].Value = "Manager Contact AM";
                ws.Cells[1, 14].Value = "Manager Contact PM";
                ws.Cells[1, 15].Value = "Email";
                ws.Cells[1, 16].Value = "Concept";
                ws.Cells[1, 17].Value = "Last Customer Comm.";

                int currentRow = 2;
                int i = 0;
                RoutesList.OrderBy(x => x.RouteNumber);
                while (i < RoutesList.Count)
                {
                    var currentRoute = RoutesList[i];
                    int e = 0;
                    //TODO: Should we handle when a route doesn't have stops?
                    if (currentRoute.NumberOfStops > 0)
                    {
                        var stopsAsList = new List<Stop>(currentRoute.RouteStops);

                        while (e < stopsAsList.Count)
                        {
                            var currentRouteStop = stopsAsList[e];
                            //Don't show stop 0 in the list of stops
                            if (currentRouteStop.StopNumber > 0)
                            {
                                //get time difference data
                                var timeDiff = DateTime.Compare(currentRouteStop.PlannedDeliveryDateTime.Value, currentRouteStop.AdjustedDeliveryDateTime.Value);//currentRouteStop.ScheduledDeliveryDateTime - currentRouteStop.OriginalScheduledDeliveryDateTime;
                                var earlyVSLate = "";
                                if (timeDiff < 0) { earlyVSLate = "Late"; }
                                else if (timeDiff > 0) { earlyVSLate = "Early"; }
                                var formattedPhone = CreateFormattedPhoneNumber(currentRouteStop.Phone);

                                var dateTimeDiff = currentRouteStop.PlannedDeliveryDateTime - currentRouteStop.AdjustedDeliveryDateTime;
                                var formattedDateTimeDiff = dateTimeDiff.HasValue ? dateTimeDiff.GetValueOrDefault().Duration() : dateTimeDiff;

                                ws.Cells[currentRow, 1].Value = currentRoute.RouteNumber;
                                ws.Cells[currentRow, 2].Value = currentRouteStop.StopNumber;
                                ws.Cells[currentRow, 3].Value = currentRouteStop.BillTo;
                                ws.Cells[currentRow, 4].Value = currentRouteStop.ShipTo;
                                ws.Cells[currentRow, 5].Value = currentRouteStop.CustomerName;
                                ws.Cells[currentRow, 6].Value = currentRouteStop.City;
                                ws.Cells[currentRow, 7].Value = currentRouteStop.State;
                                ws.Cells[currentRow, 8].Value = formattedPhone;
                                ws.Cells[currentRow, 9].Value = currentRouteStop.PlannedDeliveryDateTime.Value.ToString("MM/dd/yyyy hh:mm tt");
                                ws.Cells[currentRow, 10].Value = formattedDateTimeDiff.ToString();
                                ws.Cells[currentRow, 11].Value = earlyVSLate;
                                ws.Cells[currentRow, 12].Value = currentRouteStop.AdjustedDeliveryDateTime.Value.ToString("MM/dd/yyyy hh:mm tt");
                                ws.Cells[currentRow, 15].Value = currentRouteStop.EmailAddress;
                                ws.Cells[currentRow, 16].Value = currentRouteStop.Concept.ToString();
                                ws.Cells[currentRow, 17].Value = currentRouteStop.LastCustomerCommunicationComment.ToString();

                                //csv = csv + string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", currentRoute.RouteNumber, 
                            }
                            e++;
                            currentRow++;
                        }//close while loop
                    }
                    i++;
                }

                return File(p.GetAsByteArray(), "application/excel", "RouteList.xlsx");//(new System.Text.UTF8Encoding().GetBytes(p), "text/csv", "RouteList.csv");
            }

        }

        public async Task<JsonResult> ConceptsForCenter(int SygmaCenterNumber)
        {
            var model = await LookUpService.GetConceptsForCenter(SygmaCenterNumber);
            model = model.OrderBy(o => o.Concept).ToList();
            return Json(JsonConvert.SerializeObject(model), JsonRequestBehavior.AllowGet);
            //return Json(List<ConceptType>RouteFilterService.GetConceptsForCenter(SygmaCenterNumber));
        }

        public async Task<ActionResult> ColumnOption()
        {
            var rco = new ColumnOptionViewModel(await LookUpService.GetColumnOptionsForUserAndPage(UserName, "ROUTETRACKERROUTES"), ColumnTypes.Route);
            var sco = new ColumnOptionViewModel(await LookUpService.GetColumnOptionsForUserAndPage(UserName, "ROUTETRACKERSTOPS"), ColumnTypes.Stop);
            RouteTrackerColumnOptionViewModel model = new RouteTrackerColumnOptionViewModel
            {
                RouteColumnOption = rco,
                StopColumnOption = sco
            };
            return PartialView("_trackerColumnOptions", model);
        }

        [HttpPost]
        public async Task SaveColumnOption(UpdateRouteTrackerColumnOptionViewModel model)
        {
            await LookUpService.UpdateColumnOptionAsync(UserName, "ROUTETRACKERROUTES", JsonConvert.SerializeObject(model.RouteColumnOption));
            await LookUpService.UpdateColumnOptionAsync(UserName, "ROUTETRACKERSTOPS", JsonConvert.SerializeObject(model.StopColumnOption));
        }

        [HttpPost]
        public async Task<ActionResult> ChangeStopTime(UpdateRouteWithStopsViewModel route)
        {
            var updatedRouteStopList = new List<atm.services.models.UpdateStop>();
            var stops = route.Stops;

            //Get the current version of the route from the API
            var existingRoute = await RouteService.GetByRouteIdAsync(route.RouteId);
            var existingRouteStops = existingRoute.RouteStops;

            for (int x = 0; x < stops.Count(); x++)
            {
                var existingStop = existingRouteStops.Single(s => s.RoutePlanId == stops[x].RoutePlanId);
                var updatedStop = new UpdateStop()
                {
                    RoutePlanId = stops[x].RoutePlanId,
                    RouteId = stops[x].RouteId,
                    BillTo = stops[x].BillTo,
                    ShipTo = stops[x].ShipTo,
                    AdjustedDeliveryDateTime = stops[x].AdjustedDeliveryDateTime,
                    ScheduledDeliveryDateTime = stops[x].ScheduledDeliveryDateTime,
                    Comment = stops[x].LastCustomerCommunicationComment,
                    EarlyLate = stops[x].EarlyLate,
                    RoutePlanModificationTypeId = stops[x].RoutePlanModificationTypeId,
                    RouteNumber = stops[x].RouteNumber,
                    AdjustedRouteNumber = stops[x].AdjustedRouteNumber,
                    StopNumber = stops[x].AdjustedStopNumber,
                    AdjustedStopNumber = stops[x].AdjustedStopNumber,
                };
                //Set the comment to empty string if value is null otherwise leave it alone
                existingStop.Comment = existingStop.Comment ?? "";
                updatedStop.Comment = updatedStop.Comment ?? "";

                //if User has updated the Comment and/or ScheduledDeliveryDateTime
                if (!System.DateTime.Equals(updatedStop.AdjustedDeliveryDateTime, existingStop.AdjustedDeliveryDateTime) || !System.DateTime.Equals(updatedStop.ScheduledDeliveryDateTime, existingStop.ScheduledDeliveryDateTime) || updatedStop.Comment != existingStop.LastCustomerCommunicationComment)
                {
                    //If stop currently has RoutePlanModificationTypeId of NONE or stop was added and time modified.
                    if (updatedStop.RoutePlanModificationTypeId == 0 || updatedStop.RoutePlanModificationTypeId == 2)
                    {
                        //Set status to Time Changed.  If the original modification type was a 2, the API will reset it back to 2.
                        updatedStop.RoutePlanModificationTypeId = 3;
                    }

                    if (updatedStop.StopNumber != existingStop.StopNumber) updatedStop.IncludesStopChanges = true;
                    if (!System.DateTime.Equals(updatedStop.ScheduledDeliveryDateTime, existingStop.ScheduledDeliveryDateTime))
                    {
                        updatedStop.IncludesStopScheduledDateTimeChanges = true;
                        if (!System.DateTime.Equals(updatedStop.AdjustedDeliveryDateTime, updatedStop.ScheduledDeliveryDateTime)) updatedStop.IncludesStopAdjustedDateTimeChanges = true;
                    }
                    else
                    {
                        if (!System.DateTime.Equals(updatedStop.AdjustedDeliveryDateTime, existingStop.AdjustedDeliveryDateTime)) updatedStop.IncludesStopAdjustedDateTimeChanges = true;
                    }


                    //Save the stop
                    updatedRouteStopList.Add(updatedStop);
                }
            }

            var updateRouteWithStops = new UpdateRouteWithStops()
            {
                SygmaCenterNo = route.SygmaCenterNo,
                RouteNumber = route.RouteNumber,
                RouteId = route.RouteId,
                RouteName = route.RouteName,
                RouteStops = updatedRouteStopList
            };

            await RouteService.UpdateRouteAsync(route.RouteNumber, route.SygmaCenterNo, updateRouteWithStops);

            foreach (var stop in updateRouteWithStops.RouteStops)
            {
                if (!string.IsNullOrEmpty(stop.Comment.Trim()))
                {
                    var existingStop = existingRouteStops.Single(s => s.RoutePlanId == stop.RoutePlanId);
                    if (existingStop.LastCustomerCommunicationComment != stop.Comment)
                    {
                        await CommentService.AddCommentAsync(new services.models.AddComment
                        {
                            CenterNumber = updateRouteWithStops.SygmaCenterNo,
                            CreatedBy = LastFirstName.ToUpperInvariant(),
                            Status = 3, // non-internal
                            PrimaryRecordId = $"{((int)stop.BillTo).ToString()}-{((int)stop.ShipTo).ToString()}",
                            SecondaryRecordId = stop.RoutePlanId.ToString(),
                            Screen = "RT",
                            ShortComment = "Customer Communication",
                            LongComment = stop.Comment
                        });
                    }
                }

                if (stop.IncludesStopChanges)
                {
                    var existingStop = existingRouteStops.Single(s => s.RoutePlanId == stop.RoutePlanId);
                    await CommentService.AddCommentAsync(new services.models.AddComment
                    {
                        CenterNumber = updateRouteWithStops.SygmaCenterNo,
                        CreatedBy = LastFirstName.ToUpperInvariant(),
                        Status = 2, // internal
                        PrimaryRecordId = $"{((int)stop.BillTo).ToString()}-{((int)stop.ShipTo).ToString()}",
                        SecondaryRecordId = stop.RoutePlanId.ToString(),
                        Screen = "RT",
                        ShortComment = "Stop Adjustment",
                        LongComment = $"Stop moved from Stop: {stop.StopNumber} at {existingStop.AdjustedDeliveryDateTime} to Stop: {stop.AdjustedStopNumber} at {stop.AdjustedDeliveryDateTime}"
                    });
                }

                if (stop.IncludesStopAdjustedDateTimeChanges)
                {
                    var existingStop = existingRouteStops.Single(s => s.RoutePlanId == stop.RoutePlanId);
                    await CommentService.AddCommentAsync(new services.models.AddComment
                    {
                        CenterNumber = updateRouteWithStops.SygmaCenterNo,
                        CreatedBy = LastFirstName.ToUpperInvariant(),
                        Status = 2, // internal
                        PrimaryRecordId = $"{((int)stop.BillTo).ToString()}-{((int)stop.ShipTo).ToString()}",
                        SecondaryRecordId = stop.RoutePlanId.ToString(),
                        Screen = "RT",
                        ShortComment = "Time Adjustment",
                        LongComment = $"Stop {stop.StopNumber} adjusted delivery was changed from {existingStop.AdjustedDeliveryDateTime} to {stop.AdjustedDeliveryDateTime}"
                    });
                }
                if (stop.IncludesStopScheduledDateTimeChanges)
                {
                    var existingStop = existingRouteStops.Single(s => s.RoutePlanId == stop.RoutePlanId);
                    await CommentService.AddCommentAsync(new services.models.AddComment
                    {
                        CenterNumber = updateRouteWithStops.SygmaCenterNo,
                        CreatedBy = LastFirstName.ToUpperInvariant(),
                        Status = 2, // internal
                        PrimaryRecordId = $"{((int)stop.BillTo).ToString()}-{((int)stop.ShipTo).ToString()}",
                        SecondaryRecordId = stop.RoutePlanId.ToString(),
                        Screen = "RT",
                        ShortComment = "Time Adjustment",
                        LongComment = $"Stop {stop.StopNumber} scheduled delivery was changed from {existingStop.ScheduledDeliveryDateTime} to {stop.ScheduledDeliveryDateTime}"
                    });
                }
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.Created);
        }

        public async Task<ActionResult> MovedStopPopup(int routePlanId, int modifiedType)
        {
            //call service layer to get collection of stops with the route plan ID
            var matchingStopCollection = new List<Stop>(await RouteService.GetStopsByRoutePlanIdAsync(routePlanId));
            //stop collection should only ever have 2 entries
            if (matchingStopCollection.Count != 2)
            {
                throw new UnexpectedStopException("An unexpected number of Stops was encountered");
            }
            //decide which stop is destination based on direction you are coming from
            int desiredRouteId = matchingStopCollection.First(s => s.RoutePlanModificationTypeId != modifiedType).RouteId;

            //get route info from routeID
            var model = new MovedStopPopupRouteStopViewModel(await RouteService.GetByRouteIdAsync(desiredRouteId))
            {
                //need both route and stop column options
                StopColumnOption = new ColumnOptionViewModel(await LookUpService.GetColumnOptionsForUserAndPage(UserName, "ROUTETRACKERSTOPS"), ColumnTypes.Stop),
                RouteColumnOption = new ColumnOptionViewModel(await LookUpService.GetColumnOptionsForUserAndPage(UserName, "ROUTETRACKERROUTES"), ColumnTypes.Route)
            };

            //send model of route info to popup
            return PartialView("_movedStopPopup", model);
        }

        private string CreateFormattedPhoneNumber(string rawPhoneNum)
        {
            //Remove all non numeric characters
            var numericPhone = Regex.Replace(rawPhoneNum, "[^\\d]", "");
            var formattedPhoneNum = "";
            switch (numericPhone.Length)
            {
                case 10:
                    //If you have a full phone number with area code
                    formattedPhoneNum = Regex.Replace(numericPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
                    break;
                case 7:
                    //If you have a phone number minus area code
                    formattedPhoneNum = Regex.Replace(numericPhone, @"(\d{3})(\d{4})", "$1-$2");
                    break;
                default:
                    formattedPhoneNum = numericPhone;
                    break;
            }

            return formattedPhoneNum;
        }

        private int IsStopEarlyOrLate(Stop stop, bool currentStop)
        {
            int result = 0;

            // waterfall planned - scheduled - adjusted to make sure all fields have date time to reduce null check
            if (!stop.AdjustedDeliveryDateTime.HasValue)
            {
                if (!stop.ScheduledDeliveryDateTime.HasValue)
                {
                    stop.ScheduledDeliveryDateTime = stop.PlannedDeliveryDateTime;
                }
                stop.AdjustedDeliveryDateTime = stop.ScheduledDeliveryDateTime;
            }

            if (stop.StopNumber == 0)
            {
                // if this is a dispatch (stop 0)
                if (!stop.TelogisArrivalDeliveryDateTime.HasValue)
                {
                    // if route has NOT been dispatched
                    if (stop.ScheduledDeliveryDateTime == stop.AdjustedDeliveryDateTime)
                    {
                        if (DateTime.Now > stop.ScheduledDeliveryDateTime.Value.AddMinutes(60))
                        {
                            // if NOW is more than an hour after Scheduled - then LATE
                            result = 1;
                        }
                        else if (DateTime.Now < stop.ScheduledDeliveryDateTime.Value.AddMinutes(-60))
                        {
                            // if NOW is more than an hour PRIOR Scheduled - then EARLY
                            result = -1;
                        }
                    }
                    else
                    {
                        if (stop.AdjustedDeliveryDateTime.Value > stop.ScheduledDeliveryDateTime.Value.AddMinutes(60))
                        {
                            // if Adjusted is more than an hour after Scheduled - then LATE
                            result = 1;
                        }
                        else if (stop.AdjustedDeliveryDateTime.Value < stop.ScheduledDeliveryDateTime.Value.AddMinutes(-60))
                        {
                            // if Adjusted is more than an hour PRIOR Scheduled - then EARLY
                            result = -1;
                        }
                    }
                }
            }
            else
            {
                if (stop.TelogisArrivalDeliveryDateTime.HasValue)
                {
                    // arrived
                    if (stop.TelogisArrivalDeliveryDateTime.Value > stop.ScheduledDeliveryDateTime.Value.AddMinutes(60))
                    {
                        // if arrived is more than an hour after Scheduled - then LATE
                        result = 1;
                    }
                    else if (stop.TelogisArrivalDeliveryDateTime.Value < stop.ScheduledDeliveryDateTime.Value.AddMinutes(-60))
                    {
                        // if arrived is more than an hour PRIOR to Scheduled - then EARLY
                        result = -1;
                    }
                }
                else
                {
                    // not arrived
                    if (DateTime.Now > stop.ScheduledDeliveryDateTime.Value.AddMinutes(60))
                    {
                        // if NOW is more than an hour after Scheduled - then LATE
                        result = 1;
                    }
                    else if (DateTime.Now < stop.ScheduledDeliveryDateTime.Value.AddMinutes(-60))
                    {
                        // if NOW is more than an hour PRIOR to Scheduled - then EARLY
                        result = -1;
                    }
                }
            }

            return result;
        }

    }
    public class UnexpectedStopException : Exception
    {
        public UnexpectedStopException(string message) : base(message)
        {
        }
    }
}