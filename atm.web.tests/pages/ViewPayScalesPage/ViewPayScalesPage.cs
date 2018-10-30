using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class ViewPayScalesPage : BasePage
    {
        public ViewPayScalesPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/payscale/readonly";
        public override string PageTitle => "ATM - View Pay Rates";

        public override void NavigateTo()
        {
            PayrollButton.Click();
            ViewPayScales.Click();

            //Verify the Manage Employees page actually loaded
            driver.Title.Should().Contain("View Pay Rates", "the View Pay Rates Page should have loaded");
        }

        public void SelectARandomPayScale()
        {
            SelectAGivenPayScaleByName("Driver w/ Helper  - D");
        }

        public void SelectAGivenPayScaleByName(string PayScaleName)
        {
            PayScales.SelectByText(PayScaleName, "PayScales");
        }
    }
}