using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class ManageEmployeesPageSteps
    {
        [Then(@"I should see a list of employees returned")]
        public void ThenIShouldSeeAListOfEmployeesreturned()
        {
            var manageEmployeesPage = Driver.GetPage("ManageEmployeesPage") as ManageEmployeesPage;
            manageEmployeesPage.VerifyEmployeesAreDisplayed();
        }
    }
}
