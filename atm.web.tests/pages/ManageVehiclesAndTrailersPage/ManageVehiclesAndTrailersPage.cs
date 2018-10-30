using atm.web.tests.common;
using OpenQA.Selenium;
using System;
using FluentAssertions;
using System.Threading;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class ManageVehiclesAndTrailersPage : BasePage
    {
        public ManageVehiclesAndTrailersPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/Apps/ATM/Tools/ManageVehicleTrailers.aspx";
        public override string PageTitle => "ATM - Manage Vehicles & Trailers";

        public override void NavigateTo()
        {
            ToolsButton.Click();
            ManageVehiclesAndTrailersButton.Click();
            //Verify the Manage Employees page actually loaded
            driver.Title.Should().Be("ATM - Manage Vehicles & Trailers", "the Manage Vehicles & Trailers Page should have loaded");
        }

        public void SelectACenter(string centerText)
        {
            Center.SelectByText(centerText, "CenterSelect");
            //Wait until the table of Vehicles & Trailers loads
            driver.WaitUntilElementIsPresent(By.Id("body_gvVT"), 45);
        }
    }
}
