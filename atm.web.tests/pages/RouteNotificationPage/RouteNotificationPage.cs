using atm.web.tests.common;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class RouteNotificationPage : BasePage
    {
        public RouteNotificationPage(IWebDriver driver) : base(driver) { }
        public override string Url => ConstantsUtils.Url + "/routenotification/index";
        public override string PageTitle => "ATM - Route Notification";

        public void SelectGivenStopsForRoute(List<string> stopNumbers, string routeId)
        {
            foreach (string stopNumber in stopNumbers)
            {
                SelectStopCheckbox(routeId, stopNumber).Check();
                Debug.WriteLine($"Selecting stop number: {stopNumber} from route id: {routeId}");
            }
        }

        public void SelectAllStopsForGivenRoute(string routeId)
        {
            SelectAllStopsInRouteCheckbox(routeId).Check();
            Debug.WriteLine($"Selecting all stops in route id: {routeId}");
        }

        public string GetIdForRandomRouteShown()
        {
            //get the Route Id for the first route shown
            Random rnd = new Random();
            //Randomly pick a route from the displayed list
            var randomRouteIndex = rnd.Next(1, RouteRows.Count() - 1);
            //Get the IWebElement for the route
            var randomRoute = RouteRows.ElementAt(randomRouteIndex);
            return randomRoute.GetAttribute("data-route-id"); //Get the Route ID and return it
        }

        public void SendNotifications()
        {
            SendNotificationsButton.Click();
            //Wait until the Save Notification message is shown or timeout after 20 seconds
            driver.WaitUntilElementIsPresent(By.ClassName("toast-is-shown"), 20);
        }
    }
}
