using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class UserMaintenancePageSteps
    {
        [When(@"I select a user from the Users dropdown")]
        public void WhenISelectAUserFromTheUsersDropdown()
        {
            var userMaintenancePage = Driver.GetPage("UserMaintenancePage") as UserMaintenancePage;
            userMaintenancePage.ViewATestUserPermissions();
        }

        [Then(@"I should see the View Pay Rates / Scales page")]
        public void ThenIShouldSeeTheViewPayRatesScalesPage()
        {
            var userMaintenancePage = Driver.GetPage("UserMaintenancePage") as UserMaintenancePage;
            userMaintenancePage.ViewPayRatesScalesPageCheckbox.Displayed.Should().BeTrue("because we expect the View Pay Rates Scales Checkbox to be displayed.");
        }


    }
}
