using atm.web.tests.common;
using atm.web.tests.pages;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class PayrollFormPageSteps
    {
        [When(@"I enter valid information for a (Regular|Special|Line|miscellaneous) form with a (current|closed) weekending date")]
        public void WhenIEnterValidInformationForAFormWithAClosedWeekendingDate(string FormType, string CurrentOrClosedDate)
        {
            var payrollFormPage = Driver.GetPage("PayrollFormPage") as PayrollFormPage;
            payrollFormPage.InputPayrollFormDetails(FormType, CurrentOrClosedDate);
        }

        [When(@"I try to create the payroll form")]
        public void WhenITryToCreateThePayrollForm()
        {
            var payrollFormPage = Driver.GetPage("PayrollFormPage") as PayrollFormPage;
            payrollFormPage.SubmitNewPayrollForm();
        }

        [Then(@"I should see the newly created form")]
        public void ThenIShouldSeeTheNewlyCreatedForm()
        {
            var payrollFormPage = Driver.GetPage("PayrollFormPage") as PayrollFormPage;
            var routeNumber = ScenarioContext.Current["RouteNumber"].ToString();
            var departDate = ScenarioContext.Current["DepartDate"].ToString();

            payrollFormPage.VerifyPayrollFormCreatedForGivenRoute(routeNumber, departDate);
        }


    }
}
