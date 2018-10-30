using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class PayrollFormsPageSteps
    {

        [Given(@"I select some new columns from the customize columns menu")]
        public void GivenISelectSomeNewColumnsFromTheCustomizeColumnsMenu()
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            payrollFormsPage.OpenCustomizeColumnsSection();
            payrollFormsPage.RemoveXNumberOfColumnsFromSelectedList(1);
        }

        [Given(@"I open the Customize Columns window")]
        public void GivenIOpenTheCustomizeColumnsWindow()
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            //Saves the current list of columns to compare against later
            payrollFormsPage.SaveCurrentColumnsToScenarioContext();
            //Click the column options link
            payrollFormsPage.OpenCustomizeColumnsSection();
        }

        [When(@"I remove (.*) columns? from the Selected Column list")]
        public void WhenIRemoveXColumnsFromTheSelectedColumnList(int numberOfColumns)
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            payrollFormsPage.RemoveXNumberOfColumnsFromSelectedList(numberOfColumns);
        }

        [Then(@"the columns being displayed should not have changed")]
        public void ThenTheColumnsBeingDisplayedShouldNotHaveChanged()
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            var actualList = payrollFormsPage.GetColumnsCurrentlyDisplayed();
            actualList.Should().BeEquivalentTo(ScenarioContext.Current["PayrollFormsStartingColumns"]);
        }
        
        [Given(@"I select a weekending date with valid results")]
        public void GivenISelectAWeekendingDateWithValidResults()
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            //Search for the payroll form we created in the database
            var payrollFormId = ScenarioContext.Current["PayrollFormId"].ToString();
            payrollFormsPage.SearchForAGivenPayrollForm(payrollFormId);
        }

        [When(@"I try to view details for a payroll form")]
        public void WhenITryToViewDetailsForAPayrollForm()
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            //Search for the payroll form we created in the database
            var payrollFormId = ScenarioContext.Current["PayrollFormId"].ToString();
            payrollFormsPage.SearchForAGivenPayrollForm(payrollFormId);
            //Click on the first payroll form in the table
            payrollFormsPage.ClickOnGivenPayrollFormRow(1);
        }

        [Then(@"I should see details for the given payroll form")]
        public void ThenIShouldSeeDetailsForTheGivenPayrollForm()
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            payrollFormsPage.VerifyDetailsDisplayedForPayrollForm();
        }

        [Given(@"I open a blank payroll form")]
        public void GivenIOpenABlankPayrollForm()
        {
            var payrollFormsPage = Driver.GetPage("PayrollFormsPage") as PayrollFormsPage;
            payrollFormsPage.CreateNewBlankPayrollForm();   
        }



    }
}
