using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class UserMaintenancePage : BasePage
    {
        public UserMaintenancePage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/Apps/ATM/Security/Users.aspx";
        public override string PageTitle => "ATM - User Maintenance";

        public override void NavigateTo()
        {
            SecurityTab.Click();
            UserMaintenanceButton.Click();
            //Verify the Manage Employees page actually loaded
            driver.Title.Should().Contain("User Maintenance", "the User Maintenance Page should have loaded");
        }

        public void ViewATestUserPermissions()
        {
            Users.SelectByIndex(3, "Users");
            //Wait until the panels appear to change the user's permissions
            driver.WaitUntilElementIsPresent(By.Id("body_pnlSelectedUser"));
        }
    }
}
