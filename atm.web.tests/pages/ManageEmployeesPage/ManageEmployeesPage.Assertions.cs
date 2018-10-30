using FluentAssertions;
using OpenQA.Selenium;
using System;

namespace atm.web.tests.pages
{
    public partial class ManageEmployeesPage
    {
        public void VerifyEmployeesAreDisplayed()
        {
            //Assert that the employees table has loaded
            var EmployeesTableHasLoaded = driver.FindElements(By.Id("body_pnlProgression")).Count > 0;
            EmployeesTableHasLoaded.Should().BeTrue("The Employees Table didn't load properly");
            //Assert that the employee count is 1 or larger
            Int32.Parse(EmployeeCount.Text).Should().BeGreaterThan(0);
        }
    }
}
