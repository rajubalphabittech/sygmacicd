using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace atm.web.tests.pages
{
    public partial class RouteManagerPage
    {
        /// <summary>
        /// Verify that a route is shown in G Maps
        /// </summary>
        /// <param name="routeIndex"></param>
        public void VerifyRouteLoadedInGoogleMaps(int routeId)
        {
            //Get the count of markers which includes ones that are not for stops
            int countOfAllGMapMarkers = driver.FindElements(By.XPath("//div[@class='pin-wrap']")).Count;
            //Subtract the count on non-stop markers to get only count of Stop Markers
            int actualCountStopMarkers = countOfAllGMapMarkers;
            //Get the number of stops which should appear in the Google Maps panel
            int expectedCountOfStopMarkers = DbHelper.GetAllStopNumberExcludingRemovedStopFromAPIForRouteId(routeId.ToString()).Count;
            actualCountStopMarkers.Should().Be(expectedCountOfStopMarkers, $"should have rendered {expectedCountOfStopMarkers} stop markers in Google Maps");
        }

        /// <summary>
        /// Method to verify the Stops List for the Left Routes panel loaded for the expected Route Number
        /// </summary>
        /// <param name="expectedRouteNumber"></param>
        public void VerifyStopsListLoadedForExpectedRouteNumber(string expectedRouteNumber)
        {
            //Verify the Stops List is visible
            LeftStopsContainer.Displayed.Should().BeTrue("Expected the Stop List to be displayed for the Left Routes Panel");
            //Verify the Stops List loaded for the expected route
            var routeLoadedInStopsList = LeftStopsContainerContent.GetAttribute("data-routenumber");
            routeLoadedInStopsList.Should().Be(expectedRouteNumber, $"should have loaded the stops list for Route {expectedRouteNumber}");
        }

        public void VerifyStopsListNotLoaded()
        {
            //Find out if Stops List is currently displayed
            var isStopsTableDisplayed = driver.ElementIsPresent(By.ClassName("stop-list-is-displayed"));
            isStopsTableDisplayed.Should().BeFalse("Expected the Stop List to NOT be displayed for the Left Routes Panel");
        }

        public void VerifyStopMoveTargetRouteCapacityTotalsCalculateCorrectly()
        {
            var targetRouteTotalWeight = new double();
            var sourceStopTotalWeight = new double();
            var actualTotalWeightUpdated = new double();
            double.TryParse(ScenarioContext.Current["TargetRouteInitialTotalWeight"].ToString(), out targetRouteTotalWeight);
            double.TryParse(ScenarioContext.Current["SourceStopInitialTotalWeight"].ToString(), out sourceStopTotalWeight);
            var expectedUpdatedTotalWeight = Math.Round(targetRouteTotalWeight + sourceStopTotalWeight, 0);
            double.TryParse(TargetRouteTotalWeightUpdated.GetText(), out actualTotalWeightUpdated);
            //Verify the Total Weight is within 1lb of the stop move is Source Route Source Weight + Target Stop Weight.
            actualTotalWeightUpdated.Should().BeInRange(expectedUpdatedTotalWeight - 1, expectedUpdatedTotalWeight + 1, "the Target Route total weight value was not calculated correctly");

            var targetRouteTotalCubes = new double();
            var sourceStopTotalCubes = new double();
            var actualTotalCubesUpdated = new double();
            double.TryParse(ScenarioContext.Current["TargetRouteInitialTotalCubes"].ToString(), out targetRouteTotalCubes);
            double.TryParse(ScenarioContext.Current["SourceStopInitialTotalCubes"].ToString(), out sourceStopTotalCubes);
            double expectedTotalCubesUpdated = Math.Round(targetRouteTotalCubes + sourceStopTotalCubes);
            double.TryParse(TargetRouteTotalCubesUpdated.GetText(), out actualTotalCubesUpdated);
            //Verify the Total Cubes with the stop move is Source Route Source Cubes + Target Stop Cubes
            actualTotalCubesUpdated.Should().Be(expectedTotalCubesUpdated, "the Target Route total cubes value was not calculated correctly");


            var targetRouteTotalCases = new double();
            var sourceStopTotalCases = new double();
            var actualTotalCasesUpdated = new double();
            double.TryParse(ScenarioContext.Current["TargetRouteInitialTotalCases"].ToString(), out targetRouteTotalCases);
            double.TryParse(ScenarioContext.Current["SourceStopInitialTotalCases"].ToString(), out sourceStopTotalCases);
            double expectedTotalCasesUpdated = Math.Round(targetRouteTotalCases + sourceStopTotalCases, 0);
            double.TryParse(TargetRouteTotalCasesUpdated.GetText(), out actualTotalCasesUpdated);
            //Verify the Total Cases with the stop move is Source Route Source Cases + Target Stop Cases
            actualTotalCasesUpdated.Should().Be(expectedTotalCasesUpdated, "the Target Route total cases value was not calculated correctly");
        }

        public void VerifyStopMoveSourceRouteCapacityTotalsCalculateCorrectly()
        {
            var sourceRouteTotalWeight = new double();
            var sourceStopTotalWeight = new double();
            var actualTotalWeightUpdated = new double();
            double.TryParse(ScenarioContext.Current["SourceRouteInitialTotalWeight"].ToString(), out sourceRouteTotalWeight);
            double.TryParse(ScenarioContext.Current["SourceStopInitialTotalWeight"].ToString(), out sourceStopTotalWeight);
            //Round the Source Route Total Weight and Source Stop Total Weight to 0 decimal places and then subtract them
            var expectedUpdatedTotalWeight = Math.Round(sourceRouteTotalWeight - sourceStopTotalWeight, 0);
            double.TryParse(SourceRouteTotalWeightUpdated.GetText(), out actualTotalWeightUpdated);
            //Verify the Total Weight with the stop move is Source Route Source Weight + Target Stop Weight
            actualTotalWeightUpdated.Should().BeInRange(expectedUpdatedTotalWeight - 1, expectedUpdatedTotalWeight + 1, "the Source Route total weight updated value was not calculated correctly");

            var sourceRouteTotalCubes = new double();
            var sourceStopTotalCubes = new double();
            var actualTotalCubesUpdated = new double();
            double.TryParse(ScenarioContext.Current["SourceRouteInitialTotalCubes"].ToString(), out sourceRouteTotalCubes);
            double.TryParse(ScenarioContext.Current["SourceStopInitialTotalCubes"].ToString(), out sourceStopTotalCubes);
            double expectedTotalCubesUpdated = Math.Round(sourceRouteTotalCubes - sourceStopTotalCubes, 0);
            double.TryParse(SourceRouteTotalCubesUpdated.GetText(), out actualTotalCubesUpdated);
            actualTotalCubesUpdated.Should().BeInRange(expectedTotalCubesUpdated - 1, expectedTotalCubesUpdated + 1, "the Source Route total cubes updated value was not calculated correctly");

            var sourceRouteTotalCases = new double();
            var sourceStopTotalCases = new double();
            var actualTotalCasesUpdated = new double();

            double.TryParse(ScenarioContext.Current["SourceRouteInitialTotalCases"].ToString(), out sourceRouteTotalCases);
            double.TryParse(ScenarioContext.Current["SourceStopInitialTotalCases"].ToString(), out sourceStopTotalCases);
            double.TryParse(SourceRouteTotalCasesUpdated.GetText(), out actualTotalCasesUpdated);
            double expectedTotalCasesUpdated = Math.Round(sourceRouteTotalCases - sourceStopTotalCases, 0);
            actualTotalCasesUpdated.Should().BeInRange(expectedTotalCasesUpdated - 1, expectedTotalCasesUpdated + 1, "the Source Route total cases updated value was not calculated correctly");
        }

        public void VerifyStopMoveIconIsDisabledAndToolTipMatchesForAllStops(string stopsWithOrWithoutOrder, string expectedStopMoveToolTip)
        {
            LeftStopCardsHeaders.Count().Should().BeGreaterOrEqualTo(1); //Verify there are stops actually in the stops table
            IList<IWebElement> StopCardsToTest = null;
            if (stopsWithOrWithoutOrder == "with")
            {
                //Find all the stops in the route with an order
                StopCardsToTest = LeftStopCardsHeaders.Where(stop => stop.GetAttribute("data-order-id") != "0").ToList();
            }
            else
            {
                //Otherwise find all stops without an order
                StopCardsToTest = LeftStopCardsHeaders.Where(stop => stop.GetAttribute("data-order-id") == "0").ToList();
            }

            foreach (IWebElement stop in StopCardsToTest)
            {
                //Retrieve stop number for the current stop card
                var stopNumber = stop.GetAttribute("data-stopnumber");
                //The data-canmove attribute is the only way we can tell if the button in disabled.
                //Readonly and Enabled will all return true because the disabling is done in the js
                IWebElement MoveStop = MoveStopButton(Convert.ToInt32(stopNumber));
                var canStopMove = MoveStop.GetAttribute("data-canmove").ToString();
                canStopMove.Should().Be("False", "the Move Stop icon should have been disabled");
                //Have to locate the icon within the MoveStop anchor to retrieve the tool tip
                var stopMoveToolTip = MoveStop.FindElement(By.ClassName("fa-arrows")).GetAttribute("title");
                stopMoveToolTip.Should().Be(expectedStopMoveToolTip, $"the tooltip for stop number '{stopNumber}' doesn't match the expected value.");
            };
        }

        //Checks that the Stop Move Source Route Scheduled Delivery value matches the Scheduled Delivery for the stop
        public void VerifyStopMoveSourceRouteScheduledDeliveryMatchesExpectedScheduledDelivery()
        {
            var routeId = ScenarioContext.Current["SourceRouteId"].ToString();
            var stopNumber = ScenarioContext.Current["SourceStopNumber"].ToString();
            //Get theExpected Scheduled Delivery value from the API
            var expectedScheduledDelivery = Convert.ToDateTime(DbHelper.GetStopInformationForGivenRouteId(routeId, stopNumber).ScheduledDeliveryDateTime);
            //Get the actual Scheduled Delivery value from the stop move popup
            var actualSourceStopScheduledDelivery = Convert.ToDateTime(SourceStopScheduledDeliveryDateTime.GetText());
            actualSourceStopScheduledDelivery.Should().Be(expectedScheduledDelivery);
        }

        public void VerifyQuickAddActivityLogShowsExectedComment(int stopNumber, string expectedComment, string expectedCategory)
        {
            //gets all the comment rows which are for the given stop number
            IList<IWebElement> allCommentsForStop = ActivityLogCommentRows.Where(commentRow => commentRow.GetAttribute("data-stop-number") == stopNumber.ToString()).ToList();
            //Selecting the first comment as it will be the most recent
            var firstComment = allCommentsForStop.FirstOrDefault();
            //Get the value from the comment column for the first comment row
            var commentValueForFirstComment = firstComment.FindElement(By.ClassName("stop-comment")).GetText();
            var categoryForFirstComment = firstComment.FindElement(By.ClassName("stop-comment-category")).GetText();
            commentValueForFirstComment.Should().Contain(expectedComment); //Verify the comment matches
            categoryForFirstComment.Should().Be(expectedCategory); //Verify the comment category matches
        }
    }
}
