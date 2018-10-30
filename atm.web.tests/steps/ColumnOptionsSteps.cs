using atm.web.tests.common;
using atm.web.tests.pages;
using FluentAssertions;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace atm.web.tests.steps
{
    [Binding]
    public sealed class ColumnOptionsSteps
    {
        [Given(@"the following (Stops|Routes) columns are selected in Column Options:")]
        public void GivenIVerifyThatTheFolowingStopsColumnsAreSelected(string stopsOrRoutesColumns, Table table)
        {
            List<string> columnNames = new List<string>();
            foreach (var row in table.Rows)
            {
                //Add each of the column names to the columnNames list
                columnNames.Add(row["ColumnName"]);
            }
            //Find the current page
            var currentPage = Driver.GetCurrentPage();
            //Verify the desired columns are selected and save changes to column options
            currentPage.AddGivenColumnNameToSelectedColumnsListAndSaveChanges(columnNames, stopsOrRoutesColumns);
        }

        [Given(@"I select (.*) column\(s\) from the (Routes|Stops) - Selected Columns table")]
        public void GivenISelectColumnSFromTheRoutesSelectedColumnsTable(int numberColumns, string useRoutesOrStopsList)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            if (useRoutesOrStopsList == "Routes")
            {
                routeTrackerPage.SelectGivenNumberColumnsFromColumnOptionsRoutesSelectedList(numberColumns);
            }
            else
            {
                routeTrackerPage.SelectGivenNumberColumnsFromColumnOptionsStopsSelectedList(numberColumns);
            }
        }

        [Given(@"I view the Column Options popup")]
        public void GivenIViewTheColumnOptionsPopup()
        {
            var currentPage = Driver.GetCurrentPage();
            currentPage.OpenColumnOptionsPopup();
        }

        [When(@"I try to move the Selected (Routes|Stops) columns past the (top|bottom) of the list")]
        public void WhenITryToMoveTheSelectedRoutesColumnsPastTheTopOrBottomOfTheList(string routeOrStopsList, string topOrBottom)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            for (int i = 0; i < 15; i++) //Choose the number 5 assuming that this would ensure we hit the top of the list 
            {
                if (routeOrStopsList == "Routes")
                {
                    if (topOrBottom == "top")
                    {
                        routeTrackerPage.RoutesMoveColumnUp.Click();
                    }
                    else
                    {
                        routeTrackerPage.RoutesMoveColumnDown.Click();
                    }
                }
                else
                {
                    if (topOrBottom == "top")
                    {
                        routeTrackerPage.StopsMoveColumnUp.Click();
                    }
                    else
                    {
                        routeTrackerPage.StopsMoveColumnDown.Click();
                    }
                }
            }
        }

        [Then(@"I should still see the moved columns in the list in (Routes|Stops) Selected List")]
        public void ThenIShouldSeeTheMovedColumnsInRoutesSelectedTable(string routeOrStopsList)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            //Retrieve either the Routes Selected Column List option or the Stops Selected Column List
            IList<string> actualColumns = routeOrStopsList == "Routes" ? routeTrackerPage.RoutesSelectedColumns.GetOptionsText() : routeTrackerPage.StopsSelectedColumns.GetOptionsText();
            IList<string> movedColumns = (IList<string>)ScenarioContext.Current["ExpectedSelectedColumns"];

            actualColumns.Should().Contain(movedColumns);
        }

        [When(@"I double left click on the '(.*)' column")]
        public void WhenIDoubleLeftClickOnTheGivenColumn(string targetColumnName)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            ScenarioContext.Current["TargetColumnName"] = targetColumnName; //Save off the column name for use later
            routeTrackerPage.StopsMoveAllColumnsRight.Click(); //Move all columns to Selected to make sure we have columns available
            routeTrackerPage.DoubleLeftClickOnGivenStopsAvailableColumnName(targetColumnName);
        }

        [Then(@"I (should not|should) see the '(.*)' column in the (Stops|Routes) Available list")]
        public void ThenNotSeeTheColumnInTheStopsAvailableList(string shouldOrShouldNotSee, string targetColumn, string routesOrStopsList)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            IList<string> actualAvailableColumns = new List<string>();
            //Retrieve all options from either the Stops or Route available columns element
            actualAvailableColumns = (routesOrStopsList == "Stops") ? routeTrackerPage.StopsAvailableColumns.GetOptionsText() : routeTrackerPage.RoutesAvailableColumns.GetOptionsText();

            if (shouldOrShouldNotSee == "should")
            {
                actualAvailableColumns.Should().Contain(targetColumn);
            }
            else
            {
                actualAvailableColumns.Should().NotContain(targetColumn);
            }
        }

        [Given(@"All (Routes|Stops|Routes and Stops) columns are selected in Column Options")]
        public void GivenAllRoutesColumnsAreSelectedInColumnOptions(string columnTypeToSelect)
        {
            var routeTrackerPage = Driver.GetPage("RouteTrackerPage") as RouteTrackerPage;
            routeTrackerPage.OpenColumnOptionsPopup(); //Open the Column Options popup
            //Reset the Column Options to the default so that we always begin with the same columns
            routeTrackerPage.ResetToDefault.Click();
            switch (columnTypeToSelect)
            {
                case "Routes":
                    //Select all the Routes Columns
                    routeTrackerPage.RoutesMoveAllColumnsRight.Click();
                    break;
                case "Stops":
                    //Select all the Stops Columns
                    routeTrackerPage.StopsMoveAllColumnsRight.Click();
                    break;
                case "Routes and Stops":
                    //Select all the Routes Columns
                    routeTrackerPage.RoutesMoveAllColumnsRight.Click();
                    //Select all the Stops Columns
                    routeTrackerPage.StopsMoveAllColumnsRight.Click();
                    break;
            }
            routeTrackerPage.SaveColumnOptions(); //Click Ok button and wait for page to load
        }

    }
}
