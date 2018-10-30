using atm.web.tests.common;
using atm.web.tests.pages;
using TechTalk.SpecFlow;

namespace atm.web.tests
{
    [Binding]
    public sealed class NavigationSteps
    {
        [Given(@"I am an authenticated ATM user on the ""(.*)"" page")]
        [Given(@"I'm an authenticated ATM user on the ""(.*)"" page")]
        public void GivenIAmAnATMUserOnTheGivenPage(string targetPage)
        {
            //Load the url of the targetpage
            Driver.GetPage($"{targetPage}Page").Open();
        }

        [Given(@"I navigate to the ""(.*)"" page")]
        public void GivenINavigateToThePage(string targetPage)
        {
            //Load ATM Landing page
            Driver.GetPage("HomePage").Open();
            //Navigate to the target page using the Navbar
            Driver.GetPage($"{targetPage}Page").NavigateTo();
        }

    }
}