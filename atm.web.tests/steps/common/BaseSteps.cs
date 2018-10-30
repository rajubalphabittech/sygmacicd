using TechTalk.SpecFlow;
using atm.web.tests.common;
using FluentAssertions;
using static atm.web.tests.common.Driver;

namespace atm.web.tests
{
    [Binding]
    public class BaseSteps
    {
        [When(@"I click the '(.*)' button")]
        public void WhenIClickTheButton(string elementName)
        {
            dynamic currentPage = GetCurrentPage();
            //Retrieve the element from the current page you are on
            var element = currentPage.GetType().GetProperty(elementName.Replace(" ", string.Empty)).GetValue(currentPage, null);
            element.Click();
            Browser.RobustWait(); //Verify things have loaded up
        }

        [Given(@"I select a random center")]
        [When(@"I select a random center")]
        public void GivenISelectARandomCenter()
        {
            GetCurrentPage().SelectRandomCenter(); //Select a random center from the avaialble ones
        }

        [Given(@"I select '(.*)' for the Center")]
        [When(@"I select '(.*)' for the Center")]
        public void ISelectAGivenCenterByName(string centerName)
        {
            GetCurrentPage().SelectCenterByName(centerName); //Select the given center by name
        }

        [When(@"I select ""(.*)"" from the Center DropDown")]
        public void WhenISelectFromTheCenterDropDown(string targetText)
        {
            GetCurrentPage().SelectCenterByName(targetText);
        }
    }
}
