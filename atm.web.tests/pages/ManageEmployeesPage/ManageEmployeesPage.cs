using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class ManageEmployeesPage : BasePage
    {
        public ManageEmployeesPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/Apps/ATM/Tools/ManageEmployees.aspx";
        public override string PageTitle => "ATM - Manage Employees";

        public override void NavigateTo()
        {
            ToolsButton.Click();
            ManageEmployeesButton.Click();
            //Verify the Manage Employees page actually loaded
            driver.Title.Should().Be("ATM - Manage Employees", "the Manage Employees Page should have loaded");
        }
    }
}
