using atm.web.tests.common;
using TechTalk.SpecFlow;
using FluentAssertions;
using static atm.web.tests.common.Driver;
using static atm.web.tests.common.WebDriverElementExtensions;
using atm.web.tests.pages;
using OpenQA.Selenium;
using System.Linq;
using System.Collections.Generic;

namespace atm.web.tests.steps.common
{
    [Binding]
    public sealed class VerificationSteps
    {
        [Then(@"I should see an alert saying '(.*)'")]
        public void ThenIShouldSeeAnAlertSaying(string ExpectedAlertMessage)
        {
            Browser.WaitUntilAlertIsPresent();
            var alertMessage = Browser.SwitchTo().Alert().Text;
            alertMessage.Should().Contain(ExpectedAlertMessage);
        }

        [Then(@"the '(.*)' button should be (enabled|disabled)")]
        public void ThenTheSaveButtonShouldBeDisabled(string elementName, string enabledOrDisabled)
        {
            bool shouldBeEnabled = enabledOrDisabled == "enabled" ? true : false;
            dynamic currentPage = GetCurrentPage();
            //Retrieve the element from the current page you are on
            var element = currentPage.GetType().GetProperty(elementName.Replace(" ", string.Empty)).GetValue(currentPage, null);
            bool isElementEnabled = element.Enabled;
            isElementEnabled.Should().Be(shouldBeEnabled); //Verify if the element is enabled or disabled
        }

        [Then(@"I should see the following options in the '(.*)' select list")]
        public void ThenIShouldSeeTheFollowingOptionsInTheSelectList(string selectListName, Table table)
        {
            var currentPage = Driver.GetCurrentPage();
            //Retrieve the given select list element from the current page or throw an error
            IWebElement selectListElement = currentPage.GetType().GetProperty(selectListName.Replace(" ", string.Empty)).GetValue(currentPage, null);
            //Get a list of the all the columns displayed in the select list
            IList<string> actualColumns = selectListElement.GetOptionsText();
            //Make a list of all the column names we supplied
            List<string> expectedColumns = table.Rows.Select(r => r["Column"]).ToList();
            actualColumns.Should().Equal(expectedColumns);
        }


    }
}
