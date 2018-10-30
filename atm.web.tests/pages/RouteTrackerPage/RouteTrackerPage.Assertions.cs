using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TechTalk.SpecFlow;

namespace atm.web.tests.pages
{
    public partial class RouteTrackerPage
    {
        // check every route row shown falls within the given From Date and To Date
        public void AssertThatAllRoutesShownAreWithinSpecifiedDateRange(DateTime fromDate, DateTime toDate)
        {
            for (var routeIndex = 0; routeIndex < Routes.Count(); routeIndex++)
            {
                DateTime routeDispatchTime = DateTime.Parse(Routes.ElementAt(routeIndex).GetAttribute("data-route-dispatch-time"));
                routeDispatchTime.Should().BeOnOrAfter(fromDate);
                routeDispatchTime.Should().BeOnOrBefore(toDate);
            }
        }

        private bool IsStopRowRemoved(IWebElement stopRow)
        {
            //Check if the given stop TR element has data-stop-modified-type attribute and its value is 1
            return stopRow.GetAttribute("data-stop-modified-type") == "1" ? true : false;
        }

        public void VerifyRoutesLoadedForGivenDateRange(DateTime fromDateValue, DateTime toDateValue)
        {
            //Check that some routes have loaded
            GetNumberOfRoutesShown().Should().BeGreaterOrEqualTo(1);
            //Check that one of the Routes is in the given date range
            IWebElement firstRoute = driver.FindElements(By.Name("route-row")).FirstOrDefault();
            //Get the Dispatch Date for the first route shown
            var dispatchDateForFirstRouteShown = Convert.ToDateTime(firstRoute.GetAttribute("data-route-dispatch-time"));
            //Assert this dispatch date is after the from date
            dispatchDateForFirstRouteShown.Should().BeOnOrAfter(fromDateValue);
            //Assert this dispatch date is after the to date
            dispatchDateForFirstRouteShown.Should().BeBefore(toDateValue);
        }
        //Checks that a table of stops is displayed for the given route name
        public void VerifyStopsLoadedForGivenRoute(int routeId)
        {
            //Get Expected Number Of Stops from Ui then add 1 for stop 0.
            //Note: this number will Not include removed stops, so we use greater than or equal to in the assertion
            var expectedNumberOfStops = Convert.ToInt32(RouteRow(routeId).GetAttribute("data-route-number-of-stops")) + 1;
            //Assert that the Stops Table is displayed
            StopsTable(routeId).Displayed.Should().BeTrue();
            AllStopRowsForRoute(routeId).Count.Should().BeGreaterOrEqualTo(expectedNumberOfStops, $"Expected at least {expectedNumberOfStops} stops to appear in stops table for route id {routeId}");
        }

        public void VerifyOffsetHoursValueForAllStopsBelowGivenStopNumberIndex(int routeId, int stopNumberIndex, string expectedOffsetHoursValue, string verifyScheduledOrAdjustedHours)
        {
            IList<IWebElement> stopRows = AllStopRowsForRoute(routeId); //Get all stop rows for given route
            //Get a list of all stops row which have a stop index greater than or equal to the given stopNumberIndex
            IEnumerable<IWebElement> targetStopRows = stopRows.Where(stop => Int32.Parse(stop.GetAttribute("data-stop-index").ToString()) >= stopNumberIndex && stop.GetAttribute("data-stop-modified-type") != "1");
            foreach (var stopRow in targetStopRows)
            {
                var currentStopNumber = Convert.ToInt32(stopRow.GetAttribute("data-adjusted-stop-number")); //Retrieve the current stop number
                //Get the Adjusted Offset Hours Value OR the Scheduled Offset Hours value
                var actualOffsetHoursValue = verifyScheduledOrAdjustedHours.ToLower() == "adjusted" ? AdjustedOffsetHours(routeId, currentStopNumber).GetText() : ScheduledOffsetHours(routeId, currentStopNumber).GetText();
                //Assert that the Offset Hours matches the expected value
                actualOffsetHoursValue.Should().Be(expectedOffsetHoursValue, $"the {verifyScheduledOrAdjustedHours} Offset Hours field for stop number {currentStopNumber} should have been as expected");
            }
        }

        public void VerifyAdjustedDeliveryEnabledForAllNonRemovedStopsInGivenRoute(int routeId)
        {
            //Double check that route has loaded
            VerifyStopsLoadedForGivenRoute(routeId);
            int numberOfStops = Int32.Parse(ScenarioContext.Current["SelectedRouteNumberOfStops"].ToString());
            int currentStopNumber = 0;
            for (var stopIndex = 0; stopIndex < numberOfStops - 1; stopIndex++)
            {
                //Get the stop number for the current index
                currentStopNumber = GetAdjustedStopNumberGivenStopIndex(routeId, stopIndex);
                //Verify the current stop isn't REMOVED status
                if (IsStopRowRemoved(StopRow(routeId, currentStopNumber)))
                {
                    //Only verify Scheduled Delivery is enabled for stops that aren't removed
                    IWebElement scheduledDelivery = ScheduledDeliveryDate(routeId, currentStopNumber);
                    //Verify that readonly property is true to know if disabled b/c the app actually uses readonly to do this
                    scheduledDelivery.IsReadOnly().Should().Be(false, "The field should have been enabled");
                    scheduledDelivery.Enabled.Should().Be(true, "The field should have been enabled");
                }
            }
        }

        public void VerifyStopChangesSavedSuccessfully()
        {
            //Note this Save Confirmation popup only appears for 3 seconds
            SaveConfirmationResponse.GetText().Should().Be("... Successful ...");
            Debug.WriteLine("Stop changes were saved successfully.");
        }

        public void VerifyActualArrivalTimeIsEmptyForNotDeliveredStopsInGivenRouteId(int routeId)
        {
            IList<IWebElement> StopRows = AllStopRowsForRoute(routeId);
            StopRows.Count().Should().BeGreaterOrEqualTo(1); //Verify stops are actually shown
            //For each stop
            foreach (IWebElement stop in StopRows)
            {
                //Retrieve stop number for the current stop card
                var stopNumber = stop.GetAttribute("data-adjusted-stop-number");
                var actualArrivalTime = ActualArrivalTime(routeId, Convert.ToInt32(stopNumber)).GetText();
                actualArrivalTime.Should().Be("", "the Actual Arrival Time column should be empty for stops which haven't been delivered in Telogis");
            }
        }

        private bool IsGivenColumnNameSelected(string columnName, string stopsOrRoutesList)
        {
            //Either use the Stops Selected Columns List or Routes Selected Column list to look at
            var selectedColumnList = stopsOrRoutesList == "Stops" ? StopsSelectedColumns : RoutesSelectedColumns;
            //Is the given column name in the selected columns list
            return selectedColumnList.GetOptionsText().Contains(columnName);
        }

        public void VerifyAllRouteShownHaveGivenOrderFilter(string orderFilterValue)
        {
            var limit = 0;
            DisplayedRoutes.Count().Should().BeGreaterOrEqualTo(1, "No Routes were returned for the specified criteria. Please verify if routes should have been returned.");
            //For every route shown
            foreach (var route in DisplayedRoutes)
            {
                //For now hardcoding to only check 20 routes
                if (limit < 20)
                {
                    //Use the API to find out what order classification the current route should be
                    var routeExpectedOrderFilterValue = DbHelper.GenerateOrderClassificationForRouteId(route.GetAttribute("data-route-id"));
                    orderFilterValue.Should().Be(routeExpectedOrderFilterValue, $"The route should have been clasified as {routeExpectedOrderFilterValue}, but was classified as {orderFilterValue} in the UI");
                }
                limit++;
            }
        }

        public void VerifyCommentEnabledOrDisabledAndToolTipMatchesForGivenStopList(IList<IWebElement> stopList, bool shouldCommentBeDisabled, string expectedToolTip)
        {
            //make sure I passed in at least 1 stop or throw exception
            stopList.Count().Should().BeGreaterOrEqualTo(1, "no stops were passed in");
            foreach (var stop in stopList)
            {
                var stopRouteId = Convert.ToInt32(stop.GetAttribute("data-route-id"));
                var stopNumber = stop.GetAttribute("data-adjusted-stop-number");
                IWebElement currentStopComment = Comment(stopRouteId, Convert.ToInt32(stopNumber));
                currentStopComment.IsReadOnly().Should().Be(shouldCommentBeDisabled, $"Comment textbox should have been disabled for removed stops, but stop number {stopNumber} was not disabled");
                currentStopComment.GetToolTip().Should().Be(expectedToolTip);
            }
        }

        //Verifies if a specified comment exists for the stop number
        public void VerifyAggregateActivityLogContainsGivenComment(int routeId, int stopNumber, string expectedCommentValue, string expectedCommentCategory)
        {
            ActivityLogTable.Displayed.Should().BeTrue("the Activity Log popup must be displayed");
            //IList<IWebElement> allCommentsForStopNumber = GetAggregateActivityLogCommentsForGivenStopNumber(stopNumber);
            List<Dictionary<string, string>> allCommentsForStopNumber = GetAggregateActivityLogCommentsForGivenStopNumber(stopNumber);
            //Gets the actual activity log comment and comment category IF the exact expectedCommentValue exists. Otherwise a null is returned.
            var actualActivityLogCommentInfo = allCommentsForStopNumber.Where(comment => comment["Comment"].Contains(expectedCommentValue)).ToList().FirstOrDefault();
            //Verify the Comment column contains the expected comment or throw error
            actualActivityLogCommentInfo.Should().NotBeNull($"should have found an activity log comment containing '{expectedCommentValue}'");
            //Verify the Comment Category matches expected value or throw error
            actualActivityLogCommentInfo["Comment Category"].Should().Be(expectedCommentCategory);
        }

        public void VerifyCapacityTotalsCalculatedCorrectlyForDisplayedRoutes()
        {
            var limit = 0;
            decimal actualTotalCubes;
            decimal actualTotalCases;
            decimal actualTotalLbs;
            int currentRouteId;
            string currentRouteNumber;
            DisplayedRoutes.Count().Should().BeGreaterOrEqualTo(1, "No Routes were found. Please verify why routes were not displayed.");
            //For every route shown
            foreach (var currentRoute in DisplayedRoutes)
            {
                //For now hardcoding to only check 20 routes
                if (limit < 20)
                {
                    currentRouteId = Convert.ToInt32(currentRoute.GetAttribute("data-route-id"));
                    currentRouteNumber = currentRoute.GetAttribute("data-number").Trim();
                    actualTotalCubes = Convert.ToDecimal(CubesColumn(currentRouteId).GetText()); //Retrieve the Total Cubes for the route
                    actualTotalCases = Convert.ToDecimal(CasesColumn(currentRouteId).GetText()); //Retrieve the Total Cases for the route
                    actualTotalLbs = Convert.ToDecimal(LBsColumn(currentRouteId).GetText()); //Retrieve the Total LBs for the route
                    //Use the API to calculate the expected capacity totals for the routeId
                    var expectedCapacityTotals = DbHelper.CalculateExpectedCapacityTotalsForRouteId(currentRouteId.ToString());
                    //Verify actual capacity total match expected
                    actualTotalCubes.Should().BeApproximately((expectedCapacityTotals.Item1), 1, $"Route {currentRouteNumber} Total Cubes should have been within 1 unit of expected");
                    actualTotalCases.Should().BeApproximately((expectedCapacityTotals.Item2), 1, $"Route {currentRouteNumber} Total Cases should have been within 1 unit of expected");
                    actualTotalLbs.Should().BeApproximately((expectedCapacityTotals.Item3), 1, $"Route {currentRouteNumber} Total LBs should have been within 1 unit of expected");
                }
                limit++;
            }
        }

        public void VerifyAllStopsforGivenRouteIdHaveAdjustedDeliveryTime(int routeId)
        {
            IList<IWebElement> stops = AllStopRowsForRoute(routeId);
            foreach (var stop in stops)
            {
                var stopNumber = stop.GetAttribute("data-adjusted-stop-number");
                IWebElement adjustedDeliveryDate = AdjustedDeliveryDate(routeId, Convert.ToInt32(stopNumber));
                IWebElement adjustedDeliveryTime = AdjustedDeliveryTime(routeId, Convert.ToInt32(stopNumber));
                //Verify the Adjusted Delivery Date input is not empty
                adjustedDeliveryDate.GetText().Should().NotBeEmpty($"Adjusted delivery date for stop number {stopNumber} should have NOT been empty");
                //Verify the Adjusted Delivery Time input is not empty
                adjustedDeliveryTime.GetText().Should().NotBeEmpty($"Adjusted delivery time for stop number {stopNumber} should have NOT been empty");
            }
        }

        public void VerifyTotalNumberOfOrdersVersusStopsCalculatesCorrectlyForDisplayedRoutes()
        {
            DisplayedRoutes.Count().Should().BeGreaterOrEqualTo(1, "No Routes were found. Please verify if routes should have been displayed.");
            var limit = 0;
            int currentRouteId;
            string currentRouteNumber;
            int currentTotalNumberOrders;
            int currentTotalNumberStops;

            foreach (var currentRoute in DisplayedRoutes)
            {
                //For now hardcoding to only check 20 routes
                if (limit < 20)
                {
                    currentRouteId = Convert.ToInt32(currentRoute.GetAttribute("data-route-id"));
                    currentRouteNumber = currentRoute.GetAttribute("data-number").Trim();
                    var expectedTotalOrdersAndTotalStops = DbHelper.CalculateExpectedTotalOrdersVersusTotalStopsForGivenRouteId(currentRouteId.ToString());
                    currentTotalNumberOrders = expectedTotalOrdersAndTotalStops.Item1;
                    currentTotalNumberStops = expectedTotalOrdersAndTotalStops.Item2;
                    //Example: (4/8 orders) where 4 orders exist and that route has 8 stops
                    var expectedTotalOrdersVsTotalStops = $"({currentTotalNumberOrders}/{currentTotalNumberStops} orders)";
                    //Retrieve the total number of orders out of the total number of routes from the UI
                    var actualTotalOrdersVsTotalStops = TotalNumberOfOrdersVsTotalStopsForRoute(currentRouteId).GetText();
                    actualTotalOrdersVsTotalStops.Should().Be(expectedTotalOrdersVsTotalStops, $"Route #{currentRouteNumber} had {currentTotalNumberOrders} in the the API");
                }
                limit++;
            }
        }

        public void VerifyGivenColumnHeaderTooltipMatchesExpectedValue(int routeId, string columnName, string expectedToolTip)
        {
            string actualToolTip;
            switch (columnName.Replace(" ", string.Empty))
            {
                case "ActualArrivalTime":
                    actualToolTip = ActualArrivalTimeHeader(routeId).GetToolTip();
                    break;
                default:
                    throw new Exception($"{columnName} was not defined in VerifyGivenColumnHeaderTooltipMatchesExpectedValue. Please add it to the switch statement there.");
            }
            actualToolTip.Should().Be(expectedToolTip); //Assert the tooltip found matches the expected tooltip
        }
    }
}
