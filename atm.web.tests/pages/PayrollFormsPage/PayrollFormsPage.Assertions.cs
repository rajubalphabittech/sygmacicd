using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using System.Linq;

namespace atm.web.tests.pages
{
    public partial class PayrollFormsPage
    {
        public void VerifyDetailsDisplayedForPayrollForm()
        {
            //Verify that a new tab has opened for Payroll Details
            driver.WindowHandles.Count().Should().BeGreaterThan(1);
            //Switch to this window
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.RobustWait();
            var StatusOfForm = (driver.FindElement(By.Id("lblStatus"))).Text;
            //Verify the status field has been populated
            StatusOfForm.Should().NotBeNullOrWhiteSpace();
        }

    }
}
