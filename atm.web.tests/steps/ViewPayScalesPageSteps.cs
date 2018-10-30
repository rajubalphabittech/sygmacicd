using atm.web.tests.common;
using atm.web.tests.pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class ViewPayScalesPageSteps
    {
        [When(@"I select a Pay Scale")]
        public void WhenISelectAPayScale()
        {
            var viewPayScalesPage = Driver.GetPage("ViewPayScalesPage") as ViewPayScalesPage;
            viewPayScalesPage.SelectARandomPayScale();
        }

        [Then(@"I should see a Pay Scale Rates table")]
        public void ThenIShouldSeeAPayScaleRatesTable()
        {
            var viewPayScalesPage = Driver.GetPage("ViewPayScalesPage") as ViewPayScalesPage;
            viewPayScalesPage.VerifyPayScaleRatesTableLoaded();
        }

    }
}
