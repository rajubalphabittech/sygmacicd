using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class RouteManagerPageSteps
    {
        private RouteManagerPage routeManagerPage;

        [BeforeScenario()]
        public void Init()
        {
            this.routeManagerPage = Driver.Pages["RouteManagerPage"] as RouteManagerPage;
        }

        [Then(@"I should see All Stop Markers Rendered in the Google Maps window")]
        public void ThenIShouldSeeAllStopMarkersRenderedInTheGoogleMapsWindow()
        {
            var routeManagerPage = Driver.GetPage("RouteManagerPage") as RouteManagerPage;
            //Get the Route Id for the last route selected in Google Maps
            var routeId = ScenarioContext.Current["LastSelectedRouteId"];
            routeManagerPage.VerifyRouteLoadedInGoogleMaps(Convert.ToInt32(routeId));
        }

        [Then(@"each Stop Marker in the route should have the stop number in the label")]
        public void ThenEachStopMarkerInTheRouteShouldHaveTheStopNumberInTheLabel()
        {
            var routeId = ScenarioContext.Current["LastSelectedRouteId"].ToString();
            IList<string> expectedStopNumbers = DbHelper.GetAllStopNumberExcludingRemovedStopFromAPIForRouteId(routeId);
            IList<string> actualStopMarkerLabels = routeManagerPage.GetStopMarkerLabelsFromGoogleMaps();
            actualStopMarkerLabels.Should().BeEquivalentTo(expectedStopNumbers);
        }

        /// <summary>
        /// Selects a random route for either the left or right routes section
        /// </summary>
        /// <param name="givenRoutePanel"></param>
        [When(@"I select a route (with|without) orders from the (Left|Right) Route section")]
        [Given(@"I select a route (with|without) orders from the (Left|Right) Route section")]
        public void WhenISelectARouteFromTheGivenRouteSection(string withOrWithoutOrders, string givenRoutePanel)
        {
            //Uncheck the Weight Filter to increase the number of results
            routeManagerPage.LeftRoutesFilterWeightCheckbox.UnCheck();
            routeManagerPage.LeftDateRange.SelectByText("Current Week", "Left Route Panel Date Range");
            var centerNumber = Convert.ToInt16(ScenarioContext.Current["CenterNumber"]);
            //Retrieve the start and end dates from the UI
            var startDate = Convert.ToDateTime(routeManagerPage.LeftRoutesPanelStartDate.GetText());
            var endDate = Convert.ToDateTime(routeManagerPage.LeftRoutesPanelEndDate.GetText());
            var randomRoute = DbHelper.FindRandomRouteWhichHasOrders(centerNumber, startDate, endDate);
            //Select the route id we found above
            routeManagerPage.SelectRouteToShowInGMapsByGivenRouteIdAndTargetPanel(randomRoute.Item1, givenRoutePanel);
        }

        //[Given(@"I select a second route from the Left Route section")]
        //public void GivenISelectASecondRouteFromTheLeftRouteSection()
        //{
        //    //Select a random route here
        //    routeManagerPage.SelectRouteToShowInGMapsByGivenRouteIdAndTargetPanel(124, givenRoutePanel);
        //}

        //[When(@"I expand the stops list for the second route")]
        //public void WhenIExpandTheStopsListForTheSecondRoute()
        //{
        //    routeManagerPage.ExpandLeftPanelStopsListForGivenRouteNumber(ScenarioContext.Current["Route1Number"].ToString());
        //}

        /// <summary>
        /// Selects a Route from the given Route panel and click the expand stops chevron
        /// </summary>
        /// <param name="givenRoutePanel"></param>
        [Given(@"I select and display stops for a route in the (Left|Right) Route section")]
        [When(@"I select and display stops for a route in the (Left|Right) Route section")]
        public void GivenISelectAndDisplayStopsForARouteInTheGivenRouteSection(string givenRoutePanel)
        {
            //Uncheck the Weight Filter to increase the number of results
            routeManagerPage.LeftRoutesFilterWeightCheckbox.UnCheck();
            var centerNumber = Convert.ToInt16(ScenarioContext.Current["CenterNumber"]);
            //Retrieve the start and end dates from the UI
            var startDate = Convert.ToDateTime(routeManagerPage.LeftRoutesPanelStartDate.GetText());
            var endDate = Convert.ToDateTime(routeManagerPage.LeftRoutesPanelEndDate.GetText());
            int? alreadySelectedRoute = null;
            if (ScenarioContext.Current.ContainsKey("SelectedRouteId"))
            {
                //If we have already selected a route, then we can't use that route id again
                alreadySelectedRoute = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            }
            //Find a random route id and exclude the last selected route if it exists
            var randomRoute = DbHelper.FindRandomRouteWhichHasOrders(centerNumber, startDate, endDate, alreadySelectedRoute);
            ScenarioContext.Current["SelectedRouteId"] = randomRoute.Item1;
            ScenarioContext.Current["SelectedRouteNumber"] = randomRoute.Item2;
            //Select the route id we found above
            routeManagerPage.SelectRouteToShowInGMapsByGivenRouteIdAndTargetPanel(randomRoute.Item1, givenRoutePanel);
            //Expand stops for the route we just selected
            routeManagerPage.ExpandStopsListForGivenRouteId(randomRoute.Item1.ToString(), givenRoutePanel);
        }

        [When(@"I click the chevron to expand the stops list")]
        public void WhenIClickTheChevronToExpandTheStopsList()
        {
            //Get the Route Id of the last route that was selected
            var routeId = ScenarioContext.Current["LastSelectedRouteId"].ToString();
            //Expand the Stops list for the given route id, which should already be selected in the Left Route Panel
            routeManagerPage.ExpandStopsListForGivenRouteId(routeId, "left");
        }

        [When(@"I deselect the selected route")]
        public void WhenIDeselectTheSelectedRoute()
        {
            var routeId = ScenarioContext.Current["LastSelectedRouteId"].ToString();
            routeManagerPage.DeSelectRouteFromTheLeftRoutesPanelGivenRouteId(routeId);
        }

        [Then(@"The list of stops for the (first|second) selected route should display")]
        public void ThenTheListOfStopsForTheGivenRouteShouldDisplay(string indexOfRoute)
        {
            //Select which index we need from the selected routes
            var givenRouteIndex = indexOfRoute == "first" ? 0 : 1;
            //Retrieve the list of selected routes
            Dictionary<string, Dictionary<string, string>> selectedRoutes = ((Dictionary<string, Dictionary<string, string>>)ScenarioContext.Current["SelectedRoutes"]); ;
            //Get the data for the selected route
            Dictionary<string, string> selectedRoute = selectedRoutes[selectedRoutes.ElementAt(givenRouteIndex).Key];
            //Get the RouteNumber key
            var routeNumber = selectedRoute["RouteNumber"];
            routeManagerPage.VerifyStopsListLoadedForExpectedRouteNumber(routeNumber);
        }

        [Then(@"I should not see the list of stops displayed")]
        public void ThenIShouldNotSeeTheListOfStopsDisplayed()
        {
            routeManagerPage.VerifyStopsListNotLoaded();
        }

        [Given(@"I use the API to find a stop which is eligible for stop move")]
        public void GivenIUseTheAPIToFindAStopEligibleForStopMove()
        {
            routeManagerPage.ClearFiltersAndBroadenSearchResults();
            int centerNumber = Int32.Parse(ScenarioContext.Current["CenterNumber"].ToString());
            DateTime startDate = DateTime.Parse(ScenarioContext.Current["FromDateLeftPanel"].ToString());
            DateTime endDate = DateTime.Parse(ScenarioContext.Current["ToDateLeftPanel"].ToString());
            //Use the API to find a stop number eligible for stop move
            DbHelper.FindRandomRouteAndStopNumberWhichHasAnOrderAndNotDispatched(centerNumber, startDate, endDate);
        }

        [Given(@"I open the move stop prompt for a stop eligible for stop move")]
        [When(@"I open the move stop prompt for a stop eligible for stop move")]
        public void WhenIOpenTheMoveStopPromptForStopEligibleForStopMove()
        {
            var sourceRouteId = ScenarioContext.Current["SourceRouteId"].ToString();
            var sourceStopNumber = Convert.ToInt32(ScenarioContext.Current["SourceStopNumber"]);
            //Search for and then select the Source Route from the Left Route Panel
            routeManagerPage.SelectRouteToShowInGMapsByGivenRouteIdAndTargetPanel(Convert.ToInt32(sourceRouteId), "left");
            routeManagerPage.ExpandStopsListForGivenRouteId(sourceRouteId, "left"); //Expand the Stops for this route
            routeManagerPage.OpenMoveStopPromptForGivenStopNumber(sourceStopNumber);
        }

        [Given(@"I select the route found using the API and display the stops table")]
        public void GivenISelectTheRouteFoundUsingTheAPIAndDisplayTheStopstable()
        {
            var sourceRouteId = Convert.ToInt32(ScenarioContext.Current["SourceRouteId"]);
            var sourceRouteNumber = ScenarioContext.Current["SourceRouteNumber"].ToString();
            var sourceStopNumber = Convert.ToInt32(ScenarioContext.Current["SourceStopNumber"]);
            //Search for and then select the Source Route from the Left Routes Panel
            routeManagerPage.SelectRouteToShowInGMapsByGivenRouteIdAndTargetPanel(sourceRouteId, "left");
            routeManagerPage.ExpandStopsListForGivenRouteId(sourceRouteId.ToString(), "left"); //Expand the Stops for this route
        }

        [Given(@"I enter all target route information using a stop number that already exists")]
        public void GivenIEnterAllTargetRouteInformationUsingAStopNumberThatAlreadyExists()
        {
            int centerNumber = Int32.Parse(ScenarioContext.Current["CenterNumber"].ToString());
            int sourceRouteId = Int32.Parse(ScenarioContext.Current["SourceRouteId"].ToString());
            DateTime startDate = DateTime.Parse(ScenarioContext.Current["SourceRouteDispatchDay"].ToString()).Date;
            DateTime endDate = startDate.AddDays(1);

            DbHelper.FindAnEligibleTargetRouteNumber(centerNumber, startDate, endDate, sourceRouteId);
            var targetRouteId = ScenarioContext.Current["TargetRouteId"].ToString();
            //Get all the stops for the targe route
            List<string> currentTargetRouteStops = DbHelper.GetStopNumbersFromAPIForRouteId(targetRouteId);
            //Take the first existing stop number to ensure the stop number is in use
            var targetStopNumber = currentTargetRouteStops.FirstOrDefault();
            string targetRouteNumber = ScenarioContext.Current["TargetRouteNumber"].ToString();
            string comment = $"Automated test moved Route: {ScenarioContext.Current["SourceRouteNumber"]} Stop: {ScenarioContext.Current["SourceStopNumber"]} to Route: {targetRouteNumber} Stop: {targetStopNumber}";
            routeManagerPage.PopulateStopMovePopup(comment, targetRouteNumber, targetStopNumber);
        }

        [Given(@"I enter all target route information using an incorrectly formatted date time value")]
        public void GivenIEnterAllTargetRouteInformationUsingAnIncorrectlyFormattedDateTimeValue()
        {
            int centerNumber = Int32.Parse(ScenarioContext.Current["CenterNumber"].ToString());
            int sourceRouteId = Int32.Parse(ScenarioContext.Current["SourceRouteId"].ToString());
            DateTime startDate = DateTime.Parse(ScenarioContext.Current["SourceRouteDispatchDay"].ToString()).Date;
            DateTime endDate = startDate.AddDays(1);
            DbHelper.FindAnEligibleTargetRouteNumber(centerNumber, startDate, endDate, sourceRouteId); //Search for an eligible target route using the API

            ScenarioContext.Current["TargetStopNumber"] = DbHelper.FindNonDuplicateTargetStopNumberForGivenRouteId(ScenarioContext.Current["TargetRouteId"].ToString());
            string targetStopNumber = ScenarioContext.Current["TargetStopNumber"].ToString();
            string targetRouteNumber = ScenarioContext.Current["TargetRouteNumber"].ToString();
            string comment = $"Automated test moved Route: {ScenarioContext.Current["SourceRouteNumber"]} Stop: {ScenarioContext.Current["SourceStopNumber"]} to Route: {targetRouteNumber} Stop: {targetStopNumber}";
            //Entering and incorrect format of "25/07/2018 1011 AM"
            string dispatchDateTime = DateTime.Now.ToString("dd/MM/yyyy hh mm tt");
            routeManagerPage.PopulateStopMovePopup(comment, targetRouteNumber, targetStopNumber, null, null, dispatchDateTime);
        }

        /// <summary>
        /// Step Def to fill out the Target Route Number, Stop Number, Delivery Date/Time, and Comment
        /// </summary>
        [Given, When(@"I enter all target route information")]
        public void GivenIEnterAllTargetRouteInformation()
        {
            int centerNumber = Int32.Parse(ScenarioContext.Current["CenterNumber"].ToString());
            var sourceRouteId = ScenarioContext.Current["SourceRouteId"].ToString();
            DateTime startDate = DateTime.Parse(ScenarioContext.Current["SourceRouteDispatchDay"].ToString()).Date;
            DateTime endDate = startDate.AddDays(1);
            //Use the API to find a stop number eligible for stop move
            DbHelper.FindAnEligibleTargetRouteNumber(centerNumber, startDate, endDate, Int32.Parse(sourceRouteId));
            string targetRouteId = ScenarioContext.Current["TargetRouteId"].ToString();
            //TODO: refactor the setting of target route data to the TestDataHelper class
            string targetRouteNumber = ScenarioContext.Current["TargetRouteNumber"].ToString();
            ScenarioContext.Current["TargetStopNumber"] = DbHelper.FindNonDuplicateTargetStopNumberForGivenRouteId(targetRouteId);
            string targetStopNumber = ScenarioContext.Current["TargetStopNumber"].ToString();
            string sourceStopNumber = ScenarioContext.Current["SourceStopNumber"].ToString();
            string sourceRouteNumber = ScenarioContext.Current["SourceRouteNumber"].ToString();
            string comment = $"Automated test moved Route: {sourceRouteNumber} Stop: {sourceStopNumber} to Route: {targetRouteNumber} Stop: {targetStopNumber}";
            string dispatchDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"); //Valid Date format using the current date/time

            //Save off the Source and Target Route capacity totals for use later
            routeManagerPage.SaveSourceRouteTotalCapacityTotalsForGivenRouteId(sourceRouteId);
            routeManagerPage.SaveTargetRouteTotalCapacityTotalsForGivenRouteId(targetRouteId);
            routeManagerPage.PopulateStopMovePopup(comment, targetRouteNumber, targetStopNumber);
        }

        [When(@"I save the stop move prompt")]
        public void WhenISaveTheStopMovePrompt()
        {
            routeManagerPage.SaveStopMove();
        }

        [Then(@"the target route should show the added stop in the database")]
        public void ThenTheTargetRouteShouldShowTheAddedStopInTheDatabase()
        {
            //Retrieve the actual stops from the database
            Driver.Browser.RobustWait();
            List<string> targetRouteStops = DbHelper.GetStopNumbersFromAPIForRouteId(ScenarioContext.Current["TargetRouteId"].ToString());
            var targetStopNumber = ScenarioContext.Current["TargetStopNumber"].ToString();
            targetRouteStops.Should().Contain(targetStopNumber, $"the target stop number {targetStopNumber} should have been added to {ScenarioContext.Current["TargetRouteNumber"].ToString()}");
        }

        [Then(@"the Stop Move (Target|Source) Route section should calculate Capacity totals impacts correctly")]
        public void ThenTheStopMoveSectionShouldCalculateCapacityTotalsImpactsCorrectly(string targetOrSource)
        {
            if (targetOrSource == "Target") routeManagerPage.VerifyStopMoveTargetRouteCapacityTotalsCalculateCorrectly();

            else routeManagerPage.VerifyStopMoveSourceRouteCapacityTotalsCalculateCorrectly();
        }

        [Given(@"I view the left routes panel")]
        public void GivenIViewTheLeftRoutesPanel()
        {
            routeManagerPage.SelectLeftPanelDateRange(null, 1); //Make sure today is selected in the date range
        }

        [When(@"I filter using a (valid|invalid) ShipTo and BillTo number in the left routes panel")]
        public void GivenIFilterUsingInvalidShipToAndBillToNumberInTheLeftRoutesPanel(string validOrInvalidValues)
        {
            routeManagerPage.ClearFiltersAndBroadenSearchResults();
            var shipToValue = "";
            var billToValue = "";
            if (validOrInvalidValues == "valid")
            {
                //If we need valid values then go retrieve them from the DBHelper
                int centerNumber = Int32.Parse(ScenarioContext.Current["CenterNumber"].ToString());
                DateTime fromDate = DateTime.Parse(ScenarioContext.Current["FromDateLeftPanel"].ToString());
                DateTime toDate = DateTime.Parse(ScenarioContext.Current["ToDateLeftPanel"].ToString());
                DbHelper.FindValidBillToShipToValues(centerNumber, fromDate, toDate);
                billToValue = ScenarioContext.Current["ValidBillTo"].ToString();
                shipToValue = ScenarioContext.Current["ValidShipTo"].ToString();
            }
            else
            {
                billToValue = "99999"; //Assuming that 99999 will not be a valid BillTo for any center
                shipToValue = "99999"; //Assuming that 99999 will not be a valid ShipTo for any center

            }
            routeManagerPage.LeftShipTo.EnterText(shipToValue, "Left Route Panel Ship To");
            routeManagerPage.LeftBillTo.EnterText(billToValue, "Left Routes Panel Bill To To");
            routeManagerPage.LeftSearchButton.Click(); //Click search
            Driver.Browser.RobustWait(); //Have to add a robust wait here due to how js is firing
        }

        [Then(@"I should NOT see any routes returned")]
        public void ThenIShouldNOTSeeAnyRoutesReturned()
        {
            //Gets the count of routes in the left routes panel
            var actualNumberRoutesDisplayed = routeManagerPage.RouteCards("left").Count();
            actualNumberRoutesDisplayed.Should().Be(0, "No routes should have been displayed in the Left Routes panel.");
        }

        [Then(@"I should only see routes which have contain the BillTo and ShipTo values entered")]
        public void ThenIShouldOnlySeeRoutesWhichContainTheBillToAndShipToValuesEntered()
        {
            int centerNumber = Int32.Parse(ScenarioContext.Current["CenterNumber"].ToString());
            DateTime fromDate = DateTime.Parse(ScenarioContext.Current["FromDateLeftPanel"].ToString());
            DateTime toDate = DateTime.Parse(ScenarioContext.Current["ToDateLeftPanel"].ToString());
            var billTo = Convert.ToInt32(ScenarioContext.Current["ValidBillTo"]);
            var shipTo = Convert.ToInt32(ScenarioContext.Current["ValidShipTo"]);
            //Retrieve all routes which have the billto and ship to we search
            List<string> expectedRouteNumbers = DbHelper.GetExpectedRoutesWithGivenBillToAndShipTo(centerNumber, fromDate, toDate, billTo, shipTo);
            //Retrieve all routes returned in the Route Manager
            List<string> actualRouteNumbers = routeManagerPage.GetRouteNumbersDisplayedInLeftRoutesPanel();
            actualRouteNumbers.Should().BeEquivalentTo(expectedRouteNumbers);
        }

        [Then(@"the Stop Move icon is disabled for stops (without|with) an order and shows a tooltip of ""(.*)""")]
        public void ThenTheStopMoveIconIsDisabledForStopsWithoutAnOrderAndShowsATooltipOf(string withOrWithOutOrder, string expectedToolTip)
        {
            routeManagerPage.VerifyStopMoveIconIsDisabledAndToolTipMatchesForAllStops(withOrWithOutOrder, expectedToolTip);
        }

        [Then(@"the Scheduled Delivery should appear in the Source Route section of the Stop Move popup")]
        public void ThenTheScheduledDeliveryShouldAppearInTheSourceRouteSectionOfTheStopMovePopup()
        {
            routeManagerPage.VerifyStopMoveSourceRouteScheduledDeliveryMatchesExpectedScheduledDelivery();
        }

        [Given(@"I attempt to move the stop using the Quick Stop Move popup")]
        public void GivenIAttemptToMoveTheStopUsingTheQuickStopMovePopup()
        {
            //Open the Quick Stop Move popup
            routeManagerPage.OpenQuickStopMoveModal();
            var sourceRouteNumber = ScenarioContext.Current["SourceRouteNumber"].ToString();
            var sourceRouteId = Convert.ToInt32(ScenarioContext.Current["SourceRouteId"].ToString());
            var sourceStopNumber = ScenarioContext.Current["SourceStopNumber"].ToString();
            var centerNumber = Convert.ToInt32(ScenarioContext.Current["CenterNumber"].ToString());
            DateTime startDate = Convert.ToDateTime(ScenarioContext.Current["FromDateLeftPanel"]);
            DateTime endDate = Convert.ToDateTime(ScenarioContext.Current["ToDateLeftPanel"]);
            //Search for an eligible target route using the API
            DbHelper.FindAnEligibleTargetRouteNumber(centerNumber, startDate, endDate, sourceRouteId);
            var targetRouteNumber = ScenarioContext.Current["TargetRouteNumber"].ToString();
            var targetRouteId = ScenarioContext.Current["TargetRouteId"].ToString();
            //Search for an eligible target stop number using the API
            ScenarioContext.Current["TargetStopNumber"] = DbHelper.FindNonDuplicateTargetStopNumberForGivenRouteId(targetRouteId);
            string comment = $"Automated test trying to move Route: {sourceRouteNumber} Stop: {sourceStopNumber} and randomly selecting a target route number and stop number";
            //populate the stop move using a random target route & stop. Also use the default value for the New Delivery Date/Time
            routeManagerPage.PopulateStopMovePopup(comment, targetRouteNumber, ScenarioContext.Current["TargetStopNumber"].ToString(), sourceRouteNumber, sourceStopNumber);
            routeManagerPage.SaveStopMoveAndVerifySuccessfulSave();
        }

        [Given(@"I open the Quick Stop Move popup")]
        public void GivenIOpenTheQuickStopMovePopup()
        {
            var routeManagerPage = Driver.GetPage("RouteManagerPage") as RouteManagerPage;
            routeManagerPage.OpenQuickStopMoveModal();
        }

        [When(@"I select a (Source|Destination) Route which has orders and has been dispatched")]
        public void WhenISelectASourceRouteWhichHasOrdersAndHasBeenDispatched(string sourceOrDestinationRoute)
        {
            var routeManagerPage = Driver.GetPage("RouteManagerPage") as RouteManagerPage;
            routeManagerPage.SelectLeftPanelDateRange("Current Week", null);
            var centerNumber = Convert.ToInt32(ScenarioContext.Current["CenterNumber"]);
            DateTime startDate = Convert.ToDateTime(ScenarioContext.Current["FromDateLeftPanel"]);
            DateTime endDate = Convert.ToDateTime(ScenarioContext.Current["ToDateLeftPanel"]);
            //Use the API to find a random route which has order and has also been dispatched
            var selectedRouteData = DbHelper.FindRandomRouteWithOrdersAndIsDispatched(centerNumber, startDate, endDate);
            if (sourceOrDestinationRoute == "Source")
            {
                //Select the route number we found above from the Source Route Dropdown
                routeManagerPage.FromRouteNumber.SelectByValue(selectedRouteData.Item1.ToString(), "Source Route Number DropDown");
            }
            else
            {
                //Select the route number we found above from the Destination Route Dropdown
                routeManagerPage.ToNewRouteNumber.SelectByValue(selectedRouteData.Item1.ToString(), "Destination Route Number DropDown");
            }
            ScenarioContext.Current[$"Selected{sourceOrDestinationRoute}RouteId"] = selectedRouteData.Item1;
            ScenarioContext.Current[$"Selected{sourceOrDestinationRoute}RouteNumber"] = selectedRouteData.Item2;
        }

        [When(@"I select a (Source|Destination) Route from the Quick Stop Move popup which has orders and has been dispatched")]
        public void WhenISelectASourceRouteFromTheQuickStopMovePopupWhichHasOrdersAndHasBeenDispatched(string sourceOrDestinationRoute)
        {
            var routeManagerPage = Driver.GetPage("RouteManagerPage") as RouteManagerPage;
            //Expand search results be selecting current week
            routeManagerPage.SelectLeftPanelDateRange("Current Week", null);
            routeManagerPage.OpenQuickStopMoveModal();
            var centerNumber = Convert.ToInt32(ScenarioContext.Current["CenterNumber"]);
            DateTime startDate = Convert.ToDateTime(ScenarioContext.Current["FromDateLeftPanel"]);
            DateTime endDate = Convert.ToDateTime(ScenarioContext.Current["ToDateLeftPanel"]);
            //Use the API to find a random route which has order and has also been dispatched
            var selectedRouteData = DbHelper.FindRandomRouteWithOrdersAndIsDispatched(centerNumber, startDate, endDate);
            if (sourceOrDestinationRoute == "Source")
            {
                //Select the route number we found above from the Source Route Dropdown
                routeManagerPage.FromRouteNumber.SelectByText(selectedRouteData.Item2, "Source Route Number DropDown");
            }
            else
            {
                //Select the route number we found above from the Destination Route Dropdown
                routeManagerPage.ToNewRouteNumber.SelectByText(selectedRouteData.Item2, "Destination Route Number DropDown");
            }
            ScenarioContext.Current[$"Selected{sourceOrDestinationRoute}RouteId"] = selectedRouteData.Item1;
            ScenarioContext.Current[$"Selected{sourceOrDestinationRoute}RouteNumber"] = selectedRouteData.Item2;
        }

    }
}
