using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using TechTalk.SpecFlow;
using static atm.web.tests.common.Driver;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class RouteNavigationPageSteps
    {
        [Given(@"I select all stops for a randomly selected route")]
        public void WhenISelectAllStopsForARandomlySelectedRoute()
        {
            var routeNotificationPage = GetPage("RouteNotificationPage") as RouteNotificationPage;
            var targetRouteId = routeNotificationPage.GetIdForRandomRouteShown();
            routeNotificationPage.SelectAllStopsForGivenRoute(targetRouteId);
        }

        [When(@"I try to send notifications")]
        public void WhenITryToSendNotifications()
        {
            var routeNotificationPage = GetPage("RouteNotificationPage") as RouteNotificationPage;
            routeNotificationPage.SendNotifications();
        }

        [Then(@"I should see confirmation that Notifications are sent successfully")]
        public void ThenIShouldSeeConfirmationThatNotificationsAreSentSuccessfully()
        {
            var routeNotificationPage = GetPage("RouteNotificationPage") as RouteNotificationPage;
            //Retrieve the notification the user recieved after attempting to send notification
            var actualResult = routeNotificationPage.SendNotificationsResultsMessage.GetText();
            actualResult.Should().Contain("Notifications are sent successfully", "Notifications should have been sent successfully but failed");
        }
    }
}
