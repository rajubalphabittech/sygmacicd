using atm.web.tests.common;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class ManageRoutesPage : BasePage
    {
        public ManageRoutesPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/Apps/ATM/Tools/RouteEdit.aspx";
        public override string PageTitle => "ATM - Manage Routes";

        public override void NavigateTo()
        {
            ToolsButton.Click();
            ManageRoutesButton.Click();
        }

        public void CreateNewRoute()
        {
            //Delete the test route if it exists
            DeleteRouteIfExists("Test");
            AddNewRoute.Click();
            driver.WaitUntilElementIsPresent(By.ClassName("ui-dialog-title")); //Wait until the dialog opens
            //Begin filling out the new route form
            NewRouteNumber.EnterText("Test", "NewRouteNumber");

            //The following will select a location
            NewRouteLocation.SelectByIndex(1, "NewRouteLocation");
            //The following will select a clasification
            NewRouteClassification.SelectByText("Drive Only", "NewRouteClassification");
            //The following will select a Default Driver
            NewRouteDefaultDriver.SelectByIndex(1, "NewRouteDefaultDriver");
            //The following will select a Driver Pay Scale
            NewRouteDriverPayScale.SelectByText("T", "NewRouteDriverPayScale");
            NewRouteDriverHelperPayScale.SelectByText("T", "NewRouteDriverHelperPayScale");

            //The following will select a Depart Day
            NewRouteDepartDay.SelectByIndex(1, "NewRouteDepartDay");
            NewRouteMiles.EnterText("275", "NewRouteMiles");
            NewRouteZipCode.EnterText("43212", "NewRouteZipCode");
            NewRouteDepartTime.EnterText("05:00", "NewRouteDepartTime");
            NewRouteDuration.EnterText("5", "NewRouteDuration");
            //Save this new route
            NewRouteOkButton.Click();
            //Need to manually wait for the alert to popup
            Thread.Sleep(2000);
        }

        public void DeleteRouteIfExists(string routeId)
        {
            SearchForRoute.EnterText(routeId, "SearchForRoute");
            //trigger the search with an Enter press
            SearchForRoute.SendKeys(Keys.Enter);
            //Waits until the Routes table is rendered
            driver.WaitUntilElementIsPresent(By.Id("body_pnlRoutes"), 30);
            List<IWebElement> RoutesFound = ManageRoutesTable.FindElements(By.TagName("tr")).ToList();

            //If routes are found with this search delete them, otherwise do nothing
            if (RoutesFound.Count > 1)
            {
                //Delete the first row
                IWebElement deleteButton = ManageRoutesTable.FindElements(By.LinkText("Delete")).First();
                deleteButton.Click();
                Thread.Sleep(2000);
                driver.SwitchTo().Alert().Accept();
                Thread.Sleep(2000);
            }
            //Clear the search box
            SearchForRoute.Clear();
            SearchForRoute.SendKeys(Keys.Enter);
        }
    }
}