using static atm.web.tests.common.Driver;
using atm.web.tests.pages;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class ManageRoutesPageSteps
    {
        [Then(@"I should see a list of Routes")]
        public void ThenIShouldSeeAListOfRoutes()
        {
            var manageRoutesPage = GetPage("ManageRoutesPage") as ManageRoutesPage;
            manageRoutesPage.VerifyRoutesAreDisplayed();
        }

        [When(@"I complete the Form to add a new Route")]
        public void WhenICompleteTheFormToAddANewRoute()
        {
            var manageRoutesPage = GetPage("ManageRoutesPage") as ManageRoutesPage;
            //This step will open the new Route form. 
            //Then it will complete the form. Finally it will attempt to save the route.
            manageRoutesPage.CreateNewRoute();
        }

        [Then(@"The new route should be saved")]
        public void ThenTheNewRouteShouldBeSaved()
        {
            var manageRoutesPage = GetPage("ManageRoutesPage") as ManageRoutesPage;
            manageRoutesPage.VerifyNewRouteSavedSuccessfully();
            //TODO: Assert that no errors were thrown on the create route form.
        }

    }
}
