using FluentAssertions;
using OpenQA.Selenium;
using System;

namespace atm.web.tests.pages
{
    public partial class ManageVehiclesAndTrailersPage
    {
        public void VerifyVehicleTrailerRowsAreDisplayed()
        {
            int DisplayedVehicleRowCount = Int32.Parse(VehicleRowCount.Text);
            int DisplayedTrailerRowCount = Int32.Parse(TrailerRowCount.Text);

            DisplayedVehicleRowCount.Should().BeGreaterOrEqualTo(1);
            DisplayedTrailerRowCount.Should().BeGreaterOrEqualTo(1);

            //Verify that more than 1 row is in the Vehicles Trailers table
            int NumberOfRowsInVehiclesTrailersTable = VehiclesTrailersTable.FindElements(By.TagName("tr")).Count;
            NumberOfRowsInVehiclesTrailersTable.Should().BeGreaterOrEqualTo(2);
        }
    }
}
