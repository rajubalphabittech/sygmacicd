using TechTalk.SpecFlow;
using static atm.web.tests.common.Driver;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class ManageVehiclesAndTrailersPageSteps
    {
        [When(@"I select ""(.*)"" from the center search field")]
        public void WhenISelectFromTheCenterSearchField(string TextToSelect)
        {
            var manageVehiclesAndTrailersPage = GetPage("ManageVehiclesAndTrailersPage");
            manageVehiclesAndTrailersPage.SelectACenter(TextToSelect);
        }

        [Then(@"I should see a list of all Routes returned")]
        public void ThenIShouldSeeAListOfAllRoutesReturned()
        {
            var manageVehiclesAndTrailersPage = GetPage("ManageVehiclesAndTrailersPage");
            manageVehiclesAndTrailersPage.VerifyVehicleTrailerRowsAreDisplayed();
        }

    }
}
