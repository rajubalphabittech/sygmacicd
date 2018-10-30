using atm.web.tests.pages;
using System;
using TechTalk.SpecFlow;
using static atm.web.tests.common.Driver;
using atm.web.tests.common;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class RouteDetailsModalSteps
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        [When(@"I try to open the Route Details for a removed stop")]
        public void WhenITryToOpenTheRouteDetailsForARemovedStop()
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            //Select the current week to expand search results
            routeTrackerPage.SelectTextInDateRange("Current Week");
            int centerNumber = Int32.Parse(ScenarioContext.Current["CenterNumber"].ToString());
            DateTime startDate = DateTime.Parse(routeTrackerPage.FromDate.GetText()); //Get From Date value from the UI
            DateTime endDate = DateTime.Parse(routeTrackerPage.ToDate.GetText()); //Get To Date value from the UI
            //Use API to select a random route and stop number where that stop number is removed
            var randomRemovedStopInformation = DbHelper.FindRandomRouteAndStopNumberWhichHasRemovedStatus(centerNumber, startDate, endDate);
            ScenarioContext.Current["SelectedRouteId"] = randomRemovedStopInformation.Item1;
            ScenarioContext.Current["SelectedRouteNumber"] = randomRemovedStopInformation.Item2;
            ScenarioContext.Current["SelectedStopNumber"] = randomRemovedStopInformation.Item3;
            //Expand the stops table for the Route Id we found above
            routeTrackerPage.ExpandStopsTableForGivenRouteId(randomRemovedStopInformation.Item1);
            routeTrackerPage.OpenRouteDetailsModalForGivenRouteIdAndStopNumber(randomRemovedStopInformation.Item1, randomRemovedStopInformation.Item3);
        }
    }
}
