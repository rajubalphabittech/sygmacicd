using FluentAssertions;
using System;
using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class ManageRoutesPage
    {
        public void VerifyRoutesAreDisplayed()
        {
            int DisplayedRowCount = Int32.Parse(RowCount.Text);
            //Verify that the Row Count displayed above the Routes table shows a value greater than 1
            DisplayedRowCount.Should().BeGreaterOrEqualTo(1);

            //Verify that more than 1 row is in the Routes table
            int NumberOfRowsInRoutesTable = RoutesTable.FindElements(By.TagName("tr")).Count;
            //Verify the the number of rows in the Routes table is at least 2 because the
            NumberOfRowsInRoutesTable.Should().BeGreaterOrEqualTo(2);
        }

        public void VerifyNewRouteSavedSuccessfully()
        {
            //Verify no alerts are present
            (driver.SwitchTo().Alert().Text).Should().Be("Route added successfully");
            driver.SwitchTo().Alert().Accept();
        }
    }
}
