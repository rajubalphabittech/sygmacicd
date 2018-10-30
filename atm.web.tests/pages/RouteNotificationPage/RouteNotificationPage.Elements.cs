using atm.web.tests.common;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace atm.web.tests.pages
{
    public partial class RouteNotificationPage
    {
        private IWebElement SendNotificationsButton { get { return driver.FindElement(By.Id("sendNotificationButton")); } }
        //Window that notifies user is send email was successful or failed
        public IWebElement SendNotificationsResultsMessage { get { return driver.FindElement(By.Id("toast-window")); } }

        //***Routes Fields***
        //Retrieves the Route header for each route shown
        private IList<IWebElement> RouteRows { get { return driver.FindElements(By.Name("route-row")); } }
        //Find the Route Row specified and then find the select all stops checkbox
        private IWebElement SelectAllStopsInRouteCheckbox(string routeId) { return driver.FindElements(Selectors.SelectorByTagAndAttributeValue("tr", "data-route-id", routeId)).FirstOrDefault().FindElement(By.ClassName("select-all-stops-in-route")); }
        //***Routes Fields***

        //***Stops Table Fields***
        private IWebElement StopsTable(string routeId) { return driver.FindElements(Selectors.SelectorByAttributeValue("data-route-id", routeId)).ElementAtOrDefault(1); }
        private IWebElement StopRow(string routeId, string stopNumber) { return driver.FindElement(By.Id($"{routeId}-row-{stopNumber}")); }
        //Locate the Stop Row specified and then find the select checkbox
        private IWebElement SelectStopCheckbox(string routeId, string stopNumber) { return driver.FindElement(By.Id($"{routeId}-row-{stopNumber}")).FindElement(By.ClassName("StopRow")); }
        //***END Stops Table Fields ***
    }
}
