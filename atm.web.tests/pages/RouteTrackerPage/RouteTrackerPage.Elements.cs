using OpenQA.Selenium;
using System.Collections.Generic;
using atm.web.tests.common;
using System.Linq;

namespace atm.web.tests.pages
{
    public partial class RouteTrackerPage
    {
        private IWebElement NavbarRouteLink { get { return driver.FindElement(By.LinkText("Route")); } }
        private IWebElement RouteTrackerLink { get { return driver.FindElement(By.LinkText("Tracker")); } }

        //***BEGIN Search and Filter Fields***
        public override IWebElement CenterDropDown => driver.FindElement(By.Id("selectCenterDropdown"));
        private IWebElement DateRange { get { return driver.FindElement(By.Id("dateRangeDropdown")); } }
        public IWebElement FromDate { get { return driver.FindElement(By.Id("startDateInput")); } }
        public IWebElement ToDate { get { return driver.FindElement(By.Id("endDateInput")); } }
        public IWebElement BillTo { get { return driver.FindElement(By.Id("routeStopBillTo")); } }
        public IWebElement ShipTo { get { return driver.FindElement(By.Id("routeStopShipTo")); } }
        public IWebElement SearchButton { get { return driver.FindElement(By.Id("searchFilterButton")); } }
        private IWebElement SearchBox => driver.FindElement(By.Id("routeSearch"));
        public IWebElement WithOrderOption => driver.FindElement(By.Id("RouteOrderSelectionDiv"));
        public IWebElement RouteModificationTypes => driver.FindElement(By.Id("modifiedTypeSelectListDiv"));
        public IWebElement RouteNotificationButton => driver.FindElement(By.Id("routeNotificationButton"));
        public IWebElement ActivityLogAddCommentButton => driver.FindElement(By.Id("trackerAddCommentButton"));
        private IWebElement ColumnOptions => driver.FindElement(By.Id("columnOptionsButton"));  //Requires custom wait to use. Please use OpenColumnOptions method instead.
        private IWebElement RowCountIndicator => driver.FindElement(By.Id("totalRowsLabel"));  //Refers to the 'Route Count: x' field
        //***END Search and Filter Fields***

        //***BEGIN Route Table Fields ***
        public IWebElement RoutesTable { get { return driver.FindElement(By.Id("routes-table")); } }
        private IList<IWebElement> Routes { get { return driver.FindElements(By.Name("route-row")); } } //Locates all routes in the Routes tables. Some may be hidden.
        public IList<IWebElement> DisplayedRoutes => Routes.Where(route => route.Displayed == true).ToList(); //List of the Routes which are displayed
        // Finds the TR element for a given route id in the Routes Table. There are 2 trs we find. The first is the route. The second is the stops table.
        private IWebElement RouteRow(int routeId) => driver.FindElements(Selectors.SelectorByTagAndAttributeValue("tr", "data-route-id", routeId.ToString())).First();
        //Returns the Adjusted Dispatch Time td element for the given route id
        public IWebElement AdjustedDispatchTimeColumn(int routeId) => driver.FindElement(By.Id($"AdjustedDispatchTime_{routeId}"));
        //Returns the Scheduled Dispatch Time td element for the given route id
        public IWebElement ScheduledDispatchTimeColumn(int routeId) => driver.FindElement(By.Id($"ScheduledDispatchTime_{routeId}"));
        public IWebElement RouteNumberColumn(int routeId) => RouteRow(routeId).FindElement(By.ClassName("RouteNumber"));
        public IWebElement TotalNumberOfOrdersVsTotalStopsForRoute(int routeId) => driver.FindElement(By.Id($"RouteNumberTotalNumberOfOrdersVsTotalStops_{routeId}"));
        //Find the cases column for the given route id
        public IWebElement CasesColumn(int routeId) => RouteRow(routeId).FindElement(By.ClassName("Cases"));
        public IWebElement CubesColumn(int routeId) => RouteRow(routeId).FindElement(By.ClassName("Cubes"));
        public IWebElement LBsColumn(int routeId) => RouteRow(routeId).FindElement(By.ClassName("LBs"));
        private IWebElement RouteActivityLogLink(int routeId) => driver.FindElement(By.Id($"button-container-{routeId}")).FindElement(By.ClassName("fa-comments"));
        //Find the Title bar for the Route Activity Log popup
        public IWebElement RouteActivityLogTitle => driver.FindElement(Selectors.SelectorByTagAndAttributeValue("div", "aria-describedby", "dialog-window")).FindElement(By.ClassName("ui-dialog-titlebar"));
        //***END Route Table Fields ***

        //***BEGIN Stop Table Fields ***
        private IWebElement StopsTable(int routeId) => driver.FindElement(By.Id("route-" + routeId));
        private IWebElement StopRow(int routeId, int stopNumber) => driver.FindElement(By.Id($"{routeId}-row-{stopNumber}"));
        public IList<IWebElement> AllStopRowsForRoute(int routeId) => StopsTable(routeId).FindElements(By.ClassName("tracker-stop-row"));
        //Retrieves All of the stop rows for ONLY Removed Stops in a given Route Id
        public IList<IWebElement> AllRemovedStopRowsForRoute(int routeId) => AllStopRowsForRoute(routeId).Where(stop => stop.GetAttribute("data-stop-modified-type") == "1" || stop.GetAttribute("data-stop-modified-type") == "4").ToList();
        private IWebElement ShowStopsTable(int routeId) => driver.FindElement(By.Id($"button-container-{routeId}")).FindElement(By.ClassName("fa-plus"));
        private IWebElement HideStopsTable(int routeId) => driver.FindElement(By.Id($"button-container-{routeId}")).FindElement(By.ClassName("fa-minus"));
        //Adjusted Delivery Date and TIme inputs for the given Route and Stop Number
        private IWebElement AdjustedDeliveryDate(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("adjusted-date"));
        private IWebElement AdjustedDeliveryTime(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("adjusted-time"));
        //Scheduled Delivery Date and Time inputs for the given Route and Stop Number
        private IWebElement ScheduledDeliveryDate(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("scheduled-date"));
        private IWebElement ScheduledDeliveryTime(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("scheduled-time"));
        private IWebElement Comment(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("stop-comment"));
        private IWebElement RouteDetailsLink(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("fa-info-circle"));
        private IWebElement ActualArrivalTime(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("arrival-date-time"));
        private IWebElement ActualArrivalTimeHeader(int routeId) => StopsTable(routeId).FindElement(Selectors.SelectorByAttributeValue("data-column-id", "arrivalDelivery"));
        private IWebElement AdjustedOffsetHours(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(Selectors.SelectorByAttributeValue("data-column-id", "AdjustedOffsetHours")).FindElement(By.TagName("input"));
        private IWebElement AdjustedCascadeButton(int routeId, int stopNumber)
        {
            //example locator format: "10002-stop-5-CascadeChanges"
            var locator = $"{routeId}-stop-{stopNumber}-AdjustedCascadeChanges";
            return driver.FindElement(By.Id(locator));
        }
        private IWebElement ScheduledOffsetHours(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(Selectors.SelectorByAttributeValue("data-column-id", "ScheduledOffsetHours")).FindElement(By.TagName("input"));
        private IWebElement ScheduledCascadeButton(int routeId, int stopNumber)
        {
            //example locator format: "10002-stop-5-CascadeChanges"
            var locator = $"{routeId}-stop-{stopNumber}-ScheduledCascadeChanges";
            return driver.FindElement(By.Id(locator));
        }
        //Returns the Activity Log link within the given row of the stops table
        private IWebElement StopActivityLogLink(int routeId, int stopNumber) => StopRow(routeId, stopNumber).FindElement(By.ClassName("comment-dialog-trigger"));
        //Window that popups up to say if save was successful or failed
        private IWebElement SaveConfirmationResponse => driver.FindElement(By.ClassName("toast-is-shown"));
        //***END Stop Table Fields ***

        //***BEGIN Route Details Modal Fields***
        private IWebElement RouteDetailsModelTitle => driver.FindElement(By.Id("MovedStopRouteDialogTitle"));
        //***End Route Details Modal Fields***

        //***BEGIN Column Options Popup Fields***
        public IWebElement RoutesMoveAllColumnsRight => driver.FindElement(By.Id("routeColumnMoveAllRight"));
        public IWebElement RoutesSelectedColumns => driver.FindElement(By.Id("routeSelectedColumns"));
        public IWebElement RoutesAvailableColumns => driver.FindElement(By.Id("routeAvailableColumns"));
        private IWebElement RoutesMoveColumnRight => driver.FindElement(By.Id("routeColumnMoveRight"));
        public IWebElement RoutesMoveColumnUp => driver.FindElement(By.Id("routeColumnMoveUp"));
        public IWebElement RoutesMoveColumnDown => driver.FindElement(By.Id("routeColumnMoveDown"));
        public IWebElement StopsMoveAllColumnsRight => driver.FindElement(By.Id("stopColumnMoveAllRight"));
        public IWebElement StopsSelectedColumns => driver.FindElement(By.Id("stopSelectedColumns"));
        public IWebElement StopsAvailableColumns => driver.FindElement(By.Id("stopAvailableColumns"));
        private IWebElement StopsMoveColumnRight => driver.FindElement(By.Id("stopColumnMoveRight"));
        public IWebElement StopsMoveColumnUp => driver.FindElement(By.Id("stopColumnMoveUp"));
        public IWebElement StopsMoveColumnDown => driver.FindElement(By.Id("stopColumnMoveDown"));
        private IWebElement ColumnOptionsOkButton => driver.FindElement(By.Id("columnOptionsDialogOk"));
        private IWebElement ColumnOptionsCancelButton => driver.FindElement(By.Id("columnOptionsDialogCancel"));
        public IWebElement ResetToDefault => driver.FindElement(By.Id("columnOptionsDialogReset"));
        //***END Column Options Popup Fields***

        //***BEGIN Activity Log Popup Fields***
        //Find the only dialog title shown as we can only see 1 at a time
        private IWebElement ActivityLogTitle => driver.FindDisplayedElements(By.ClassName("ui-dialog-title")).FirstOrDefault();
        private IWebElement ActivityLogTable => driver.FindElement(By.ClassName("comment-table"));
        private IWebElement ActivityLogAddComment => driver.FindElement(By.ClassName("comment-add-span"));
        private IWebElement ActivityLogComment => driver.FindElement(By.Id("comment-box"));
        private IWebElement ActivityLogCommentCategory => driver.FindElement(By.Id("comment-category"));
        private IWebElement ActivityLogCommentIsInteral => driver.FindElement(By.Id("comment-is-internal"));
        public IWebElement ActivityLogSaveComment => driver.FindElement(By.XPath("//button[.='Save']"));
        public IWebElement ActivityLogOk => driver.FindElement(By.XPath("//button[.='OK']"));
        public IWebElement ActivityLogCallLogging => driver.FindElement(By.XPath("//button[.='Call Logging']"));
        public IWebElement ActivityLogClose => driver.FindElement(By.XPath("//button[.='Close']"));
        //Comments rows in the Activity Log for a given stop number
        private IList<IWebElement> ActivityLogComments => driver.FindElements(By.ClassName("comment-edit"));
        //Comments rows in the Aggregate Activity Log popup
        private IList<IWebElement> AggregateActivityLogComments => driver.FindElement(By.ClassName("comment-table")).FindElements(By.ClassName("stop-comment-row"));

        //Activity Log Quick Add Comment Fields
        public IWebElement ActivityLogCommentRouteNumber => driver.FindElement(By.Id("commentRouteNumber"));
        public IWebElement ActivityLogCommentStopNumber => driver.FindElement(By.Id("commentStopNumber"));
        //***END Activity Log Popup Fields ***
    }
}
