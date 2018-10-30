using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class PayrollFormsPage : BasePage
    {
        public PayrollFormsPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/Apps/ATM/Payroll/Forms/Index.aspx";
        public override string PageTitle => "ATM - Payroll Forms";

        public override void NavigateTo()
        {
            PayrollButton.Click();
            PayrollFormsButton.Click();
            //Verify the Manage Employees page actually loaded
            driver.Title.Should().Contain("Payroll Forms", "the Payroll Forms Page should have loaded");
        }

        public void SaveCurrentColumnsToScenarioContext()
        {
            ScenarioContext.Current["PayrollFormsStartingColumns"] = GetColumnsCurrentlyDisplayed();
        }

        public List<string> GetColumnsCurrentlyDisplayed()
        {
            List<IWebElement> RawListOfColumns = PayrollFormsTable.FindElements(By.TagName("th")).ToList();
            List<string> ColumnsDisplayed = new List<string>();

            for (int i = 0; i < RawListOfColumns.Count; i++)
            {
                ColumnsDisplayed.Add(RawListOfColumns[i].Text);
            }
            return ColumnsDisplayed;
        }

        public void OpenCustomizeColumnsSection()
        {
            CustomizeColumnsButton.Click();
            driver.WaitUntilElementIsPresent(By.Id("columnOptionsDialog")); //Wait for the Column Options popup to appear
            driver.RobustWait(); //Wait while the columns in the Column Options popup loads
        }

        public void SaveCustomColumnSelection()
        {
            SaveColumnSelection.Click();
        }

        public void CancelAndCloseColumnOptionsWindow()
        {
            CancelColumnSelection.Click();
            ColumnOptionsPopupWindow.Displayed.Should().BeFalse("The Column Options Popup should have closed");
        }

        public void RemoveXNumberOfColumnsFromSelectedList(int numberOfColumns)
        {
            //TODO: add logic to add numberOfColumns from the Available list of columns and place them in the Selected Columns list
            for (var i = 0; i < numberOfColumns; i++)
            {
                SelectTopSelectedColumn();
            }
        }

        private void SelectTopSelectedColumn()
        {
            //Click the top available column from the available list
            SelectedColumnsList.FindElements(By.TagName("option"))[0].Click();
            RemoveColumn.Click();
        }

        //Search for the given payroll form id
        public void SearchForAGivenPayrollForm(string payrollFormId)
        {
            PayrollFormIdSearch.EnterText(payrollFormId, "PayrollFormIdSearch");
            //Trigger a blur event to initiate search
            PayrollFormIdSearch.SendKeys(Keys.Enter);
            Thread.Sleep(2000);
            driver.RobustWait();
            PayrollFormsTable.Text.Should().NotContain("No forms exist for this search criteria", "valid payroll forms should be displayed");
        }

        public void ClickOnGivenPayrollFormRow(int rowNumber)
        {
            var PayrollIdColumnNumber = driver.FindElements(By.TagName("th"));
            IWebElement givenRow = (driver.FindElement(By.Id("body_gvForms"))).FindElements(By.TagName("tr"))[rowNumber].FindElements(By.TagName("td"))[0];
            givenRow.Click();
        }

        public void CreateNewBlankPayrollForm()
        {
            CreateNewForm.Click();
            //Verify that a new tab has opened for Payroll Details
            driver.WindowHandles.Count().Should().BeGreaterThan(1);
            //Switch to this window
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.RobustWait();
        }
    }
}
