using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public class RouteTrackerPageSteps
    {
        private RouteTrackerPage routeTrackerPage;

        [BeforeScenario()]
        public void Init()
        {
            this.routeTrackerPage = Driver.Pages["RouteTrackerPage"] as RouteTrackerPage;
        }

        [Given(@"I view stops for a route which hasn't been dispatched")]
        public void GivenIViewStopsForARouteWhichHasntBeenDispatched()
        {
            //Select Routes for tomorrow so that we know they havn't been dispatched
            routeTrackerPage.SelectTextInDateRange("Tomorrow");
            //Just select the first route shown
            routeTrackerPage.LoadStopsForTheFirstRouteListed();
        }

        [Given, When(@"I view Stops for the first route listed")]
        public void GivenIViewStopsForTheFirstRouteListed()
        {
            routeTrackerPage.LoadStopsForTheFirstRouteListed();
        }

        [When(@"I view the Stops Table for a random route")]
        [Given(@"I view the Stops Table for a random route")]
        public void WhenIViewTheStopsTableForARandomRoute()
        {
            //Select a random route id from the routes shown
            routeTrackerPage.GetRandomRouteFromRoutesShown();
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            routeTrackerPage.ExpandStopsTableForGivenRouteId(routeId);
        }

        [Then(@"I should see a list of all stops for that route")]
        public void ThenIShouldSeeAListOfAllStopsForThatRoute()
        {
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            routeTrackerPage.VerifyStopsLoadedForGivenRoute(routeId);
        }

        [Given(@"I set the (Scheduled|Adjusted) Offset Hours to ""(.*)"" hours for stop ""(.*)""")]
        public void GivenIChangeTheOffsetHoursByForStop(string scheduledOrAdjusted, string offsetHoursValue, string stopNumber)
        {
            //Retrieve the Route Id we opened stops for
            var routeId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            //Save the StopNumber Updated for use later
            ScenarioContext.Current["StopNumUpdated"] = stopNumber;
            //Save the adjusted time for use later
            ScenarioContext.Current["StopAdjustedTime"] = stopNumber;
            routeTrackerPage.EnterGivenOffsetHoursForGivenStop(offsetHoursValue, Int32.Parse(stopNumber), routeId, scheduledOrAdjusted);
        }

        [Given(@"I Submit the changes to the route stops")]
        [When(@"I Submit the changes to the route stops")]
        public void WhenISubmitTheChangesToTheRouteStops()
        {
            routeTrackerPage.SubmitChangesForRouteStops(Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]));
        }

        [Given(@"I enter a custom date range")]
        public void GivenIEnterACustomDateRange()
        {
            var fromDate = DateTime.Today;
            //15 days in the past
            var today = DateTime.Now;
            //Get the date for the upcoming saturday
            var toDate = today.AddDays(6 - (int)today.DayOfWeek);
            routeTrackerPage.EnterCustomDateRange(fromDate, toDate);
        }

        [Then(@"I should see a list of results returned within that date range")]
        public void ThenIShouldSeeAListOfResultsReturnedWithinThatDateRange()
        {
            var fromDatevalue = ScenarioContext.Current["FromDateValue"].ToString();
            var toDateValue = ScenarioContext.Current["ToDateValue"].ToString();
            routeTrackerPage.VerifyRoutesLoadedForGivenDateRange(Convert.ToDateTime(fromDatevalue), Convert.ToDateTime(toDateValue));
        }

        [Given(@"I set the Adjusted Delivery to ""(.*)"" for a random stop")]
        public void GivenISetTheAdjustedDeliveryTo(string adjustedDeliveryValue)
        {
            int selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            //Randomly select one of the stops for the given route and saves off to Scenario Context
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(selectedRouteId);
            int selectedStopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            //Assumes that you have a space between the date and time values (mm/dd/yy hh:mm zz)
            string[] splitAdjustedDeliveryValues = adjustedDeliveryValue.Split(' ');
            var adjustedDeliveryDateValue = splitAdjustedDeliveryValues[0];
            var adjustedDeliveryTimeValue = string.Join(" ", splitAdjustedDeliveryValues, 1, 2);
            routeTrackerPage.EnterGivenAdjustedDeliveryValueForAGivenStopNumber(adjustedDeliveryDateValue, adjustedDeliveryTimeValue, selectedRouteId, selectedStopNumber);
        }

        [Given(@"I increase the (Adjusted|Scheduled) Delivery value by ""(.*)"" minutes for a random stop")]
        public void GivenIIncreaseTheAdjustedDeliveryValueForARandomStop(string adjustedOrScheduledDelivery, string numberMinutes)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            int selectedRouteid = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            //Randomly select one of the stops for the given route and saves off to Scenario Context
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(selectedRouteid);
            int selectedStopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            if (adjustedOrScheduledDelivery == "Adjusted")
            {
                routeTrackerPage.IncrimentAdjustedDeliveryForGivenRouteIdAndStopNumber(numberMinutes, selectedRouteid, selectedStopNumber);
            }
            else
            {
                routeTrackerPage.IncrimentScheduledDeliveryForGivenRouteIdAndStopNumber(numberMinutes, selectedRouteid, selectedStopNumber);
            }
        }

        [Given(@"I update the (Adjusted|Scheduled) Delivery value for a random route and stop number")]
        public void GivenIUpdateTheAdjustedDeliveryValueForARandomRouteAndStopNumber(string adjustedOrScheduledDelivery)
        {
            //Saves off the Route Id and Route Number for a route shown into the ScenarioContext.current
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            routeTrackerPage.GetRandomRouteFromRoutesShown();
            //Grab the Route Id and Route Number we just saved off
            var randomRouteId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            var randomRouteNumber = ScenarioContext.Current["SelectedRouteNumber"].ToString();
            routeTrackerPage.ExpandStopsTableForGivenRouteId(randomRouteId); //Opens the stops table for this route
            //Saves off the stop number and stop index for a randomly selected stop within the given route id
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(randomRouteId);
            //Retrieve the stop number we found above
            var randomStopNumber = Convert.ToInt32(ScenarioContext.Current["SelectedStopNumber"]);
            if (adjustedOrScheduledDelivery == "Adjusted")
            {
                routeTrackerPage.IncrimentAdjustedDeliveryForGivenRouteIdAndStopNumber("60", randomRouteId, randomStopNumber);
            }
            else
            {
                routeTrackerPage.IncrimentScheduledDeliveryForGivenRouteIdAndStopNumber("60", randomRouteId, randomStopNumber);
            }
        }

        [Given(@"I increase the Adjusted Delivery value by ""(.*)"" minutes for Stop ""(.*)""")]
        public void GivenIIncreaseTheAdjustedDeliveryValueByMinutesForStop(string numberMinutes, int stopNumber)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            int selectedRouteid = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            ScenarioContext.Current["SelectedStopNumber"] = stopNumber;
            routeTrackerPage.IncrimentAdjustedDeliveryForGivenRouteIdAndStopNumber(numberMinutes, selectedRouteid, stopNumber);
        }

        [Given(@"I set the (Scheduled|Adjusted) Offset Hours to ""(.*)"" for a random stop")]
        public void GivenISetTheOffsetHoursToForARandomStop(string scheduledOrAdjusted, string offsetHoursValue)
        {
            int selectedRouteid = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            //Randomly select one of the stops for the given route and saves off to Scenario Context
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(selectedRouteid);
            int selectedStopNumber = Convert.ToInt32(ScenarioContext.Current["SelectedStopNumber"]);
            ScenarioContext.Current["ExpectedOffsetHoursValue"] = offsetHoursValue;
            routeTrackerPage.EnterGivenOffsetHoursForGivenStop(offsetHoursValue, selectedStopNumber, selectedRouteid, scheduledOrAdjusted);
        }

        [Given(@"I set the Adjusted Delivery value to ""(.*)"" for stop ""(.*)""")]
        [When(@"I set the Adjusted Delivery value to ""(.*)"" for stop ""(.*)""")]
        public void GivenISetTheAdjustedDeliveryValueToForStop(string adjustedDeliveryValue, int stopNumber)
        {
            ScenarioContext.Current["SelectedStopNumber"] = stopNumber; //Save off this stop number for use later
            int routeId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            string[] splitAdjustedDeliveryValues = adjustedDeliveryValue.Split(' ');
            //Gets the Date from adjustedDeliveryValue
            var adjustedDeliveryDateValue = splitAdjustedDeliveryValues[0];
            //If a correctly formatted time was given in adjustedDeliveryValue, then use it, otherwise use an empty string
            string adjustedDeliveryTimeValue = splitAdjustedDeliveryValues.Length > 2 ? string.Join(" ", splitAdjustedDeliveryValues, 1, 2) : "";
            routeTrackerPage.EnterGivenAdjustedDeliveryValueForAGivenStopNumber(adjustedDeliveryDateValue, adjustedDeliveryTimeValue, routeId, stopNumber);
        }

        [Then(@"I should see the Adjusted Delivery value equal to the Scheduled Delivery")]
        [Given(@"I should see the Adjusted Delivery value equal to the Scheduled Delivery")]
        public void ThenIShouldSeeTheAdjustedDeliveryValueEqualToTheScheduledDelivery()
        {
            int routeId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            int stopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            var expectedAdjustedDeliveryValue = routeTrackerPage.RetrieveScheduledDeliveryValueForGivenRouteAndStopNumber(routeId, stopNumber);
            //Retrieve the value from the Adjusted Delivery Date
            var actualAdjustedDeliveryValue = routeTrackerPage.RetrieveAdjustedDeliveryValueForGivenRouteAndStopNumber(routeId, stopNumber);
            string actualOffsetHoursValue = routeTrackerPage.RetrieveOffsetHoursValueForGivenRouteAndStop(routeId, stopNumber);

            //Verify Adjusted Delivery value == Scheduled Delivery value
            actualAdjustedDeliveryValue.Should().Be(expectedAdjustedDeliveryValue, "the Adjusted Delivery Date value should have reset to the Scheduled Delivery Date value");
            actualOffsetHoursValue.Should().Be("0.0");
        }

        [Then(@"I should see the Adjusted Dispatch Time value stay the same")]
        public void ThenIShouldSeeTheAdjustedDispatchTimeValueStayTheSame()
        {
            int selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            int stopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            //Using the initial value for the Adjusted Delivery value as the expected Adjusted Delivery Value
            DateTime expectedAdjustedDispatchTime = DateTime.Parse(ScenarioContext.Current[$"InitialStopNumber{stopNumber}AdjustedDeliveryValue"].ToString());
            DateTime actualAdjustedDispatchTime = DateTime.Parse(routeTrackerPage.AdjustedDispatchTimeColumn(selectedRouteId).GetText());

            actualAdjustedDispatchTime.Should().Be(expectedAdjustedDispatchTime);
        }

        [Given(@"I search for a custom date range from routes from yesterday")]
        public void GivenISearchForACustomDateRangeFromRoutesFromYesterday()
        {
            var fromDate = DateTime.Today.AddDays(-1);
            var toDate = DateTime.Today.AddDays(-2);
            routeTrackerPage.EnterCustomDateRange(fromDate, toDate);
        }

        [Given(@"I set the Date Range to (Today|Tomorrow|Yesterday|Current Week|Custom)")]
        [When(@"I set the Date Range to (Today|Tomorrow|Yesterday|Current Week|Custom)")]
        public void GivenISetTheDateRangeTo(string dateRangeValue)
        {
            routeTrackerPage.SelectTextInDateRange(dateRangeValue);
        }

        [Then(@"the Adjusted Delivery value should have changed to the entered value")]
        public void ThenTheAdjustedDeliveryValueShouldHaveChangedToTheEnteredValue()
        {
            int selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            int stopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            var actualAdjustedDeliveryValue = Convert.ToDateTime(routeTrackerPage.RetrieveAdjustedDeliveryValueForGivenRouteAndStopNumber(selectedRouteId, stopNumber));
            var expectedAdjustedDeliveryValue = Convert.ToDateTime(ScenarioContext.Current[$"ExpectedStopNumber{stopNumber}AdjustedDeliveryValue"]);

            actualAdjustedDeliveryValue.Should().Be(expectedAdjustedDeliveryValue);
        }

        [When(@"I change the Scheduled Delivery time for a random stop")]
        public void WhenIChangeTheScheduledDeliveryTimeForARandomStop()
        {
            int selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            //Randomly select one of the stops for the given route and saves off to Scenario Context
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(selectedRouteId);
            int selectedStopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            routeTrackerPage.IncrimentScheduledDeliveryForGivenRouteIdAndStopNumber("30", selectedRouteId, selectedStopNumber);
        }

        [Then(@"I should see the Row Count indicator set to ""(.*)""")]
        public void ThenIShouldSeeTheRowCountIndicatorSetTo(int expectedRowCount)
        {
            var actualRowCountIndicatorValue = routeTrackerPage.GetNumberOfRoutesShown();
            var expectedRowCountIndicatorValue = expectedRowCount;
            actualRowCountIndicatorValue.Should().Be(expectedRowCountIndicatorValue);
        }

        [Then(@"the Scheduled Delivery should be disabled for all stops in the selected route")]
        public void ThenTheScheduledDeliveryShouldBeDisabledForAllStopsInTheSelectedRoute()
        {
            int selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            routeTrackerPage.VerifyAdjustedDeliveryEnabledForAllNonRemovedStopsInGivenRoute(selectedRouteId);
        }

        [Then(@"I should see the (From Date|To Date) set to (today's|tomorrow's|yesterday's|day after tomorrow's) date")]
        public void ThenIShouldSeeTheFromDateOrToDateFromDateSetToSpecifiedDate(string fromOrToDate, string specifiedDay)
        {
            DateTime expectedDateValue = DateTime.MaxValue;
            //Create the expectedDateValue based on which day is expected
            switch (specifiedDay)
            {
                case "today's":
                    expectedDateValue = DateTime.Now;
                    break;
                case "tomorrow's":
                    expectedDateValue = DateTime.Now.AddDays(1.0);
                    break;
                case "yesterday's":
                    expectedDateValue = DateTime.Now.AddDays(-1.0);
                    break;
                case "day after tomorrow's":
                    expectedDateValue = DateTime.Now.AddDays(2.0);
                    break;
            }
            DateTime actualDateValue = DateTime.MinValue;

            if (fromOrToDate == "From Date")
            {
                actualDateValue = DateTime.Parse(routeTrackerPage.FromDate.GetText());
                ScenarioContext.Current["FromDate"] = actualDateValue.AddHours(12.0); //Set the FromDate to Date shown at noon
            }
            else
            {
                actualDateValue = DateTime.Parse(routeTrackerPage.ToDate.GetText());
                ScenarioContext.Current["ToDate"] = actualDateValue.AddHours(12.0); //Set the ToDate to Date shown at noon

            }
            actualDateValue.Should().BeSameDateAs(expectedDateValue); //Verify the actual value matches the expected date
        }

        [Then(@"I should only see Routes displayed which are within the date range displayed")]
        public void ThenIShouldOnlySeeRoutesDisplayedWhichAreWithinTheDateRangeDisplayed()
        {
            routeTrackerPage.AssertThatAllRoutesShownAreWithinSpecifiedDateRange(DateTime.Parse(ScenarioContext.Current["FromDate"].ToString()), DateTime.Parse(ScenarioContext.Current["ToDate"].ToString()));
        }

        [Given(@"I enter a comment for a random stop")]
        public void GivenIEnterACommentForARandomStop()
        {
            int routeId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            //Randomly select one of the stops for the given route and saves off to Scenario Context
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(routeId);
            int stopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            var commentValue = $"Activity Log added comment on {DateTime.Now.ToString("MM/dd/yy HH:mm:ss")}";
            ScenarioContext.Current["ExpectedCommentCategory"] = "Customer Communication"; //Save off expected Comment Category
            ScenarioContext.Current["ExpectedCommentIsInternal"] = false; //Save off expected Is Internal Checkbox value
            ScenarioContext.Current["ExpectedComment"] = commentValue;

            routeTrackerPage.EnterGivenCommentValueForAGivenStopNumber(commentValue, routeId, stopNumber);
        }

        [When(@"I click the (Scheduled|Adjusted) Cascade button for the above selected stop")]
        public void WhenIClickTheCascadeButtonForTheAboveSelectedStop(string scheduledOrAdjusted)
        {
            var selectedStopNumber = Convert.ToInt32(ScenarioContext.Current["SelectedStopNumber"]);
            var selectedRouteId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            routeTrackerPage.ClickCascadeButtonForGivenStopNumber(selectedStopNumber, selectedRouteId, scheduledOrAdjusted);
        }

        [Then(@"I should see the (Scheduled|Adjusted) Offset Hours field set to the value entered for every stop below")]
        public void ThenIShouldSeeTheOffsetHoursFieldSetToTheValueEnteredForEveryStopBelow(string scheduledOrAdjusted)
        {
            var selectedStopIndex = Int32.Parse(ScenarioContext.Current["SelectedStopIndex"].ToString());
            var selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            var expectedOffsetHoursValue = ScenarioContext.Current["ExpectedOffsetHoursValue"].ToString();
            //Used to verify that the Offset Hours field was updated for every stop below the one at the SelectedStopIndex in our Scenario context
            routeTrackerPage.VerifyOffsetHoursValueForAllStopsBelowGivenStopNumberIndex(selectedRouteId, selectedStopIndex, expectedOffsetHoursValue, scheduledOrAdjusted);
        }

        [Given(@"I enter an invalid ShipTo and BillTo number")]
        public void GivenIEnterAnInvalidShipToAndBillToNumber()
        {
            routeTrackerPage.BillTo.EnterText("999", "Bill-To"); //Assuming 999 will always be an invalid Bill-To Value
            routeTrackerPage.ShipTo.EnterText("999", "Ship-To"); //Assuming 99 will always be an invalid Ship-To Value
        }

        [When(@"I click the Search button")]
        public void WhenIClickTheSearchButton()
        {
            routeTrackerPage.SearchButton.Click();
            Driver.Browser.RobustWait(); //Manually wait for JS to Fire
        }

        [Then(@"I should see (.*) routes returned")]
        public void ThenIShouldSeeRoutesReturned(int expectedNumberOfRoutes)
        {
            var actualNumberRouteShown = routeTrackerPage.GetNumberOfRoutesShown();
            actualNumberRouteShown.Should().Be(expectedNumberOfRoutes); //Verify the number of rows exactly matches
        }

        [Given(@"I search for a (ShipTo|BillTo) value of ""(.*)""")]
        [When(@"I search for a (ShipTo|BillTo) value of ""(.*)""")]
        public void WhenISearchByGivenFilterAndValue(string field, string value)
        {
            switch (field)
            {
                case "ShipTo":
                    routeTrackerPage.ShipTo.EnterText(value, "Ship To");
                    break;
                case "BillTo":
                    routeTrackerPage.BillTo.EnterText(value, "Ship To");
                    break;
                default:
                    throw new Exception($"field name: '{field}' is not recognized");
            }
            routeTrackerPage.SearchButton.Click(); //trigger a search for the enter value
        }

        [Then(@"I should see ""(.*)"" in the (ShipTo|BillTo) field")]
        public void ThenIShouldSeeInTheShipToField(string expectedValue, string field)
        {
            switch (field)
            {
                case "ShipTo":
                    routeTrackerPage.ShipTo.GetText().Should().Be(expectedValue);
                    break;
                case "BillTo":
                    routeTrackerPage.BillTo.GetText().Should().Be(expectedValue);
                    break;
                default:
                    throw new Exception($"field name: '{field}' is not recognized");
            }
        }

        [When(@"I reload the Stops Table for the selected route")]
        public void WhenIReloadTheStopTableForTheSelectedRoute()
        {
            var selectedRouteId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            routeTrackerPage.CloseAndReopenStopsTableForGivenRoute(selectedRouteId);
        }

        [Then(@"the Adjusted Delivery value for Stop ""(.*)"" should have been updated")]
        public void ThenTheAdjustedDeliveryValueForStopShouldHaveBeenUpdated(int stopNumber)
        {
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            var actualAdjustedDeliveryValue = Convert.ToDateTime(routeTrackerPage.RetrieveAdjustedDeliveryValueForGivenRouteAndStopNumber(routeId, stopNumber));
            //Retrieve the Adjusted Delivery DateTime and then format it as needed
            var expectedAdjustedDeliveryValue = Convert.ToDateTime(ScenarioContext.Current[$"ExpectedStopNumber{stopNumber}AdjustedDeliveryValue"]);

            actualAdjustedDeliveryValue.Should().Be(expectedAdjustedDeliveryValue);
        }

        [Then(@"the Arrival Delivery Date column should be empty for all stops that haven't been delivered")]
        public void ThenTheArrivalDeliveryDateColumnShouldBeEmptyForAllStopsThatHavenTBeenDelivered()
        {
            routeTrackerPage.VerifyActualArrivalTimeIsEmptyForNotDeliveredStopsInGivenRouteId(Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]));
        }

        [When(@"I set the (With Order Option|Route Modification Types|Concept Types|Early/Late|Bill To|Ship To) filter to ""(.*)""")]
        [Given(@"I set the (With Order Option|Route Modification Types|Concept Types|Early/Late|Bill To|Ship To) filter to ""(.*)""")]
        public void WhenISetTheWithOrderOptionFilterTo(string filterName, string filterText)
        {
            switch (filterName)
            {
                case "With Order Option":
                    routeTrackerPage.WithOrderOption.BootstrapMultiSelectByText(filterText, "With Order Option Filter");
                    break;
                case "Route Modification Types":
                    routeTrackerPage.RouteModificationTypes.BootstrapMultiSelectByText(filterText, "Route Modification Types Filter");
                    break;
                default:
                    throw new Exception($"{filterName} is not defined.");
            }
        }

        [Then(@"the routes shown should have at least one stop with an order and at least one stop without an order")]
        public void ThenRoutesShownShouldHaveAtLeastStopWithAnOrderAndAtLeastStopWithoutAnOrder()
        {
            routeTrackerPage.VerifyAllRouteShownHaveGivenOrderFilter("Some Orders");
        }

        [Then(@"the routes shown should have an order for all stops excluding removed stops")]
        public void ThenRoutesShownShouldHaveAnOrderForAllStops()
        {
            routeTrackerPage.VerifyAllRouteShownHaveGivenOrderFilter("All Orders Only");
        }

        [Then(@"the routes shown should have no orders for all stops")]
        public void ThenTheRoutesShownShouldHaveNoOrdersForAllStops()
        {
            routeTrackerPage.VerifyAllRouteShownHaveGivenOrderFilter("No Orders");
        }

        [Then(@"I should see the Comment textbox disabled for all removed stops and shows a tooltip of ""(.*)""")]
        public void ThenIShouldSeeTheCommentTextboxAsDisabledForAllRemovedStopsAndShowsATooltipOf(string expectedToolTip)
        {
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            //Get all removed stops from the list of stops in the given route
            IList<IWebElement> removedStops = routeTrackerPage.AllRemovedStopRowsForRoute(routeId);
            //Open the Aggregate Activity Log for our route we selected
            routeTrackerPage.OpenAggregateActivityLogViewerForGivenRouteId(routeId);
            //Verify the comment boxes in the removed stops are disabled and the tooltip matches expected value
            routeTrackerPage.VerifyCommentEnabledOrDisabledAndToolTipMatchesForGivenStopList(removedStops, true, expectedToolTip);
        }


        [Then(@"the Route capacity should match the sum of stops capacity for all routes shown")]
        public void ThenTheRouteCapacityShouldMatchTheSumOfStopsCapacityForAllRoutesShown()
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            routeTrackerPage.VerifyCapacityTotalsCalculatedCorrectlyForDisplayedRoutes();
        }

        [Then(@"Every stop in the current route should have an Adjusted Delivery time")]
        public void ThenEveryStopInTheCurrentRouteShouldHaveAnAdjustedDeliveryTime()
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            routeTrackerPage.VerifyAllStopsforGivenRouteIdHaveAdjustedDeliveryTime(routeId);
        }

        [Then(@"I should see the total number of orders versus stops for the routes shown")]
        public void ThenIShouldSeeTheTotalNumberOfOrdersVersusStopsForTheRoutesShown()
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            routeTrackerPage.VerifyTotalNumberOfOrdersVersusStopsCalculatesCorrectlyForDisplayedRoutes();
        }

        [Then(@"I should see the stops table load successfully")]
        public void ThenIShouldSeeTheStopsTableLoadSuccessfully()
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            //Verify the stops table loaded for this route id
            routeTrackerPage.VerifyStopsLoadedForGivenRoute(routeId);
        }

        [Then(@"the ""(.*)"" column header should have a tooltip which says ""(.*)""")]
        public void ThenTheColumnHeaderShouldHaveATooltipWhichSays(string columnName, string expectedToolTip)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            //Get the route id of the currently selected route
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            //Retrieve the tooltip from specified columnName
            routeTrackerPage.VerifyGivenColumnHeaderTooltipMatchesExpectedValue(routeId, columnName, expectedToolTip);
        }

        [Then(@"I should see the Adjusted Dispatch value match the Adjusted Delivery value for Stop (.*)")]
        public void ThenIShouldSeeTheAdjustedDispatchValueMatchTheAdjustedDeliveryValueForStop(int p0)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            //Gets the AdjustedDispatch value from the UI
            var adjustedDispatchValue = Convert.ToDateTime(routeTrackerPage.AdjustedDispatchTimeColumn(routeId).GetText());
            //Gets the AdjustedDelivery value from the UI for stop 0 in the given routeId
            var adjustedDeliveryValue = Convert.ToDateTime(routeTrackerPage.RetrieveAdjustedDeliveryValueForGivenRouteAndStopNumber(routeId, 0));
            adjustedDispatchValue.Should().Be(adjustedDeliveryValue);
        }
    }
}
