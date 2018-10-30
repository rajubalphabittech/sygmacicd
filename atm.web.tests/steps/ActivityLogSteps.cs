using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using System;
using TechTalk.SpecFlow;
using static atm.web.tests.common.Driver;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class ActivityLogSteps
    {
        [When(@"I create an activity log comment with a category of (Customer Communication|Driver Communication|Stop Adjustment) for a random stop")]
        public void WhenICreateAnActivityLogCommentGivenCategoryForARandomStop(string commentCategory)
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            int selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            //Randomly select one of the stops for the given route
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(selectedRouteId);
            var selectedStopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            var commentValue = $"Activity Log added comment on {DateTime.Now.ToString("MM/dd/yy HH:mm:ss")}";
            ScenarioContext.Current["ExpectedCommentCategory"] = commentCategory; //Save off expected Comment Category
            ScenarioContext.Current["ExpectedCommentIsInternal"] = true; //Save off expected Is Internal Checkbox value
            //Enter an activity log comment with given comment category and make it internal
            routeTrackerPage.CreateActivityLogCommentForGivenStopNumber(selectedRouteId, selectedStopNumber, commentValue, commentCategory, true);
        }

        [When(@"I use the Quick Add activity log to create a comment with a category of (Customer Communication|Driver Communication|Stop Adjustment) for stop number ""(.*)"" and random route")]
        public void WhenIUseTheQuickAddActivityLogToCreateACommentWithGivenCategoryForGivenStopAndRandomRoute(string commentCategory, int stopNumber)
        {
            var currentPage = GetCurrentPage();
            //Currently only added the following method to the Route Tracker and Route Manager pages
            currentPage.CreateQuickActivityLogCommentForRandomRouteAndGivenStopNumber(stopNumber, commentCategory, true);
        }

        [When(@"I use the Quick Add activity log to create a comment with a category of (Customer Communication|Driver Communication|Stop Adjustment) for a random stop number and route")]
        public void WhenIUseTheQuickAddActivityLogToCreateACommentWithGivenCategoryForRandomStopAndRoute(string commentCategory)
        {
            var currentPage = GetCurrentPage();
            //Currently only added the following method to the Route Tracker and Route Manager pages
            currentPage.CreateQuickActivityLogCommentForRandomRouteAndGivenStopNumber(null, commentCategory, true);
        }

        [Then(@"I (should|should not) see the comment column show the activity log comment I entered")]
        public void ThenIShouldOrShouldNotSeeTheCommentColumnShowTheActivityLogCommentIEntered(string shouldOrShouldNot)
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            int selectedRouteid = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            int selectedStopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            var actualComment = routeTrackerPage.RetrieveCommentValueForGivenRouteAndStop(selectedRouteid, selectedStopNumber);
            var expectedComment = ScenarioContext.Current["ExpectedComment"].ToString();
            if (shouldOrShouldNot == "should")
            {
                actualComment.Should().Be(expectedComment, $"the most recent activity log comment should be displayed in the comment column for stop number {selectedStopNumber}");
            }
            else
            {
                actualComment.Should().NotBe(expectedComment, $"the most recent activity log comment should not be displayed comment column for stop number {selectedStopNumber}");
            }
        }

        [When(@"I view the activity log for the selected stop")]
        public void WhenIViewTheActivityLogForTheSelectedStop()
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            int selectedRouteId = Int32.Parse(ScenarioContext.Current["SelectedRouteId"].ToString());
            int selectedStopNumber = Int32.Parse(ScenarioContext.Current["SelectedStopNumber"].ToString());
            routeTrackerPage.OpenActivityLogForGivenRouteIdAndStopNumber(selectedRouteId, selectedStopNumber); //Open the Activity Log
        }

        [Then(@"I should see an activity log comment created for the comment I just entered")]
        public void ThenIShouldSeeAnActivityLogCommentCreatedForTheCommentIJustEntered()
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            var expectedCommentText = ScenarioContext.Current["ExpectedComment"];
            var expectedCommentCategory = ScenarioContext.Current["ExpectedCommentCategory"];
            bool expectedCommentIsInternal = bool.Parse(ScenarioContext.Current["ExpectedCommentIsInternal"].ToString());
            routeTrackerPage.GetLatestCommentFromStopLevelActivityLog();
            var actualCommentText = ScenarioContext.Current["ActualComment"];
            var actualCommentCategory = ScenarioContext.Current["ActualCommentCategory"];
            bool actualCommentIsInternal = bool.Parse(ScenarioContext.Current["ActualCommentIsInternal"].ToString());

            actualCommentText.Should().Be(expectedCommentText);
            actualCommentCategory.Should().Be(expectedCommentCategory);
            actualCommentIsInternal.Should().Be(expectedCommentIsInternal);
        }

        [When(@"I load the activity log for a randomly selected route and stop")]
        public void WhenILoadTheActivityLogForARandomlySelectedRouteAndStop()
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            routeTrackerPage.LoadStopsForTheFirstRouteListed(); //Load stops list
            var selectedRouteId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            routeTrackerPage.GetRandomStopNumberForGivenRouteId(selectedRouteId); //Randomly pick a stop index
            var selectedStopNumber = Convert.ToInt32(ScenarioContext.Current["SelectedStopNumber"]);
            routeTrackerPage.OpenActivityLogForGivenRouteIdAndStopNumber(selectedRouteId, selectedStopNumber); //Open the activity log
        }

        [Then(@"I should see a Call Logging button")]
        public void ThenIShouldSeeACallLoggingButtonWhichPointsTo()
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            routeTrackerPage.ActivityLogCallLogging.Displayed.Should().BeTrue("The Call Logging button should have been displayed");
        }

        [When(@"I open the Activity Log Aggregator for (a random route|the selected route)")]
        public void WhenIOpenTheActivityLogAggregatorForARandomRoute(string useRandomOrSelectedRoute)
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            //Selects a random route if you specified in that you need one. Otherwise you should already have a routeid.
            if (useRandomOrSelectedRoute == "a random route")
            {
                routeTrackerPage.GetRandomRouteFromRoutesShown();
            }
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            //Open the Route Activity Log
            routeTrackerPage.OpenAggregateActivityLogViewerForGivenRouteId(routeId);
        }

        [Then(@"I should see the Activity Log popup loaded for the that route")]
        public void ThenIShouldSeeTheActivityLogPopupLoadedForTheThatRoute()
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            var routeNumber = ScenarioContext.Current["SelectedRouteNumber"].ToString();
            //Verify the Route Activity Log loaded for the correct route number
            routeTrackerPage.RouteActivityLogTitle.GetText().Should().Contain($"AGGREGATE ACTIVITY VIEWER FOR ROUTE {routeNumber.ToUpper()}",
                $"The Activity Log should have been displayed for route number {routeNumber.ToUpper()}");
        }

        [Then(@"I should see the comment I just created in the Aggregate Activity Viewer for the randomly selected route")]
        public void ThenIShouldSeeTheCommentIJustCreatedInTheAggregateActivityViewerForTheRandomlySelectedRoute()
        {
            var routeTrackerPage = GetPage("RouteTrackerPage") as RouteTrackerPage;
            var routeId = Convert.ToInt32(ScenarioContext.Current["CommentedOnRouteId"]);
            var stopNumber = Convert.ToInt32(ScenarioContext.Current["CommentedOnStopNumber"]);
            var expectedCommentValue = ScenarioContext.Current["ExpectedComment"].ToString();
            var expectedCommentCategory = "Customer Communication";
            //Opens the Aggregate Activity Viewer for the the route Id
            routeTrackerPage.OpenAggregateActivityLogViewerForGivenRouteId(routeId);
            //Verifies the expected comment is present
            routeTrackerPage.VerifyAggregateActivityLogContainsGivenComment(routeId, stopNumber, expectedCommentValue, expectedCommentCategory);
        }

        [Then(@"I should see that the Quick Add Activity Log comment saved correctly")]
        public void ThenIShouldSeeThatTheQuickAddActivityLogCommentSavedCorrectly()
        {
            var currentPage = GetCurrentPage();
            var stopNumber = Convert.ToInt32(ScenarioContext.Current["CommentedOnStopNumber"]);
            var expectedCommentValue = ScenarioContext.Current["ExpectedComment"].ToString();
            var expectedCommentCategory = ScenarioContext.Current["ExpectedCommentCategory"].ToString();
            currentPage.VerifyQuickAddActivityLogShowsExectedComment(stopNumber, expectedCommentValue, expectedCommentCategory);
        }
        [When(@"I open the Quick Add Activity Log Comment popup")]
        public void WhenIOpenTheQuickAddActivityLogCommentPopup()
        {
            var currentPage = GetCurrentPage();
            currentPage.OpenQuickActivityLogModal();
        }

        [Then(@"I should see a Time Adjustment comment for the updated stop which shows the (Adjusted|Scheduled) Delivery time change")]
        public void ThenIShouldSeeATimeAdjustmentCommentForTheUpdatedStopWhichShowsTheTimeChange(string adjustedOrScheduledDelivery)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            var routeId = Convert.ToInt32(ScenarioContext.Current["SelectedRouteId"]);
            var stopNumber = Convert.ToInt32(ScenarioContext.Current["SelectedStopNumber"]);
            //Retrieve the initial Adjusted Delivery or Scheduled Delivery value
            var initialStopDeliveryValue = ScenarioContext.Current[$"InitialStopNumber{stopNumber}{adjustedOrScheduledDelivery.Replace(" ", "")}DeliveryValue"].ToString();
            //Retrieve the expected Adjusted Delivery or Scheduled Delivery value
            var expectedStopDelivery = ScenarioContext.Current[$"ExpectedStopNumber{stopNumber}{adjustedOrScheduledDelivery.Replace(" ", "")}DeliveryValue"].ToString();
            //Format the updatedStopDelivery correctly
            var formattedExpectedStopDelivery = Convert.ToDateTime(expectedStopDelivery).ToString("M/d/yyyy h:mm:ss tt");
            var expectedComment = $"Stop {stopNumber} {adjustedOrScheduledDelivery.ToLower()} delivery was changed from {initialStopDeliveryValue} to {expectedStopDelivery}";
            //Verify the latest comment for the specified stop number contains the expected comment and comment category
            routeTrackerPage.VerifyAggregateActivityLogContainsGivenComment(routeId, stopNumber, expectedComment, "Time Adjustment");
        }
    }
}
