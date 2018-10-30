using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class ViewEmployeesPage : BasePage
    {
        public ViewEmployeesPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/Apps/ATM/Tools/ViewEmployees.aspx";
        public override string PageTitle => "ATM - View Employees";

        public override void NavigateTo()
        {
            ToolsButton.Click();
            ViewEmployeesButton.Click();
            //Verify the Manage Employees page actually loaded
            driver.Title.Should().Contain("View Employees", "the View Employees Page should have loaded");
        }
    }
}
