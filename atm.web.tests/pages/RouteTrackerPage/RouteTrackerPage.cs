using atm.web.tests.common;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class RouteTrackerPage : BasePage
    {
        public RouteTrackerPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/RouteTracker";
        public override string PageTitle => "ATM - Route Tracker";

        public override void NavigateTo()
        {
            NavbarRouteLink.Click();
            RouteTrackerLink.Click();
            //Verify the Route Tracker page actually loaded
            driver.Title.Should().Contain("- Route Tracker", "the Route Tracker Page should have loaded");
            Debug.WriteLine("Navigated to the Route Tracker Page");
        }
        //Search and Filter methods
        public void EnterCustomDateRange(DateTime fromDateValue, DateTime toDateValue)
        {
            DateRange.SelectByText("Custom", "DateRange");
            //Save the from date for use in assertions
            ScenarioContext.Current["FromDateValue"] = fromDateValue.ToShortDateString();
            ScenarioContext.Current["ToDateValue"] = toDateValue.ToShortDateString();
            //Enter this date in the from date field
            FromDate.EnterTextWithJS(fromDateValue.ToString("MM-dd-yyyy"), "From Date");
            ToDate.EnterTextWithJS(toDateValue.ToString("MM-dd-yyyy"), "To Date");
            driver.RobustWait();
        }
        public void EnterSearchBoxValue(string searchValue)
        {
            //Enter in the search criteria into the search box
            SearchBox.EnterTextWithJS(searchValue, "Route Tracker Search Box");
        }
        public void SelectTextInDateRange(string textToSelect)
        {
            //Some DateRange options require us to add the depart day number so we do it here
            var formattedDateRangeText = TestDataHelper.GetFormattedDateRangeTextForValue(textToSelect);
            DateRange.SelectByText(formattedDateRangeText, "Date Range");
        }
        public void GetRandomRouteFromRoutesShown()
        {
            Random rnd = new Random();
            var randomIndex = rnd.Next(0, DisplayedRoutes.Count() - 1);
            var routeId = DisplayedRoutes.ElementAt(randomIndex).GetAttribute("data-route-id");
            var routeNumber = DisplayedRoutes.ElementAt(randomIndex).GetAttribute("data-number").Trim();
            ScenarioContext.Current["SelectedRouteId"] = routeId;
            ScenarioContext.Current["SelectedRouteNumber"] = routeNumber;
        }
        public int GetNumberOfRoutesShown()
        {
            string numRoutes = RowCountIndicator.GetText().Remove(0, 11);
            return Int32.Parse(numRoutes);
        }
        // Retrieves the first route displayed in the Route Tracker page.
        // Then it clicks the + to load stops on that route
        public void LoadStopsForTheFirstRouteListed()
        {
            IWebElement firstRouteShown = driver.FindDisplayedElements(By.Name("route-row")).FirstOrDefault();
            //If no routes are shown then we expand the Date Range to current week
            if (firstRouteShown == null && DateRange.GetSelectedOptionText() != "Current Week")
            {
                SelectTextInDateRange("Current Week");
            }
            //If there are still no routes found using current week then there is a problem.
            firstRouteShown.Should().NotBeNull("there should have been routes shown in the Route Tracker. Please verify why there are no routes shown.");
            var firstRouteShownRouteId = Convert.ToInt32(firstRouteShown.GetAttribute("data-route-id").Trim());
            //Save the route id selected for use later
            ScenarioContext.Current["SelectedRouteId"] = firstRouteShownRouteId;
            Debug.WriteLine($"Expanding stops table for Route Id '{firstRouteShownRouteId}'");
            //Retrieve the number of stops for this route
            ScenarioContext.Current["SelectedRouteNumberOfStops"] = firstRouteShown.GetAttribute("data-route-number-of-stops").Trim();
            ShowStopsTable(firstRouteShownRouteId).Click();
            driver.RobustWait();
        }

        public void ExpandStopsTableForGivenRouteId(int routeId)
        {
            //Verify that the Route Row for the given routeid exists
            IWebElement routeRowToSelect = RouteRow(routeId);
            //Expand the stops table for the given route
            ShowStopsTable(routeId).Click();
            Debug.WriteLine($"Expanding stops table for Route Id: '{routeId}'");
            driver.RobustWait(); //Wait while the stops table renders
        }

        public void CloseAndReopenStopsTableForGivenRoute(int routeId)
        {
            driver.RobustWait(); //Waiting to ensure the stops table is ready to be closed
            HideStopsTable(routeId).Click();
            Debug.WriteLine($"Collapsing stops table for Route Id '{routeId}'");
            ShowStopsTable(routeId).Click();
            driver.WaitUntilElementIsPresent(By.Id($"route-{routeId}")); //Wait until the stops table loads
            Debug.WriteLine($"Expanding stops table for Route Id '{routeId}'");
        }

        /// <summary>
        /// Returns the stop number shown in Route Stops for the physical stop number.
        /// This is b/c the Route Tracker counts each stop in incriments of 5.
        /// </summary>
        /// <param name="stopIndex"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public int GetAdjustedStopNumberGivenStopIndex(int routeId, int stopIndex)
        {
            IWebElement stopsTable = StopsTable(routeId);
            //Get the given row in the stopsTable
            IWebElement stopRow = stopsTable.FindElements(By.TagName("tr"))[stopIndex];
            //Get the first cell from that row which is the stop row
            var adjustedStopNumber = Convert.ToInt32(stopRow.GetAttribute("data-adjusted-stop-number"));

            return adjustedStopNumber;
        }

        public void EnterGivenOffsetHoursForGivenStop(string offsetHoursValue, int stopNumber, int routeId, string scheduledOrAdjusted)
        {
            //Locate either the Adjusted Offset Hours or the Scheduled Offset Hours 
            IWebElement offsetHours = scheduledOrAdjusted.ToLower() == "adjusted" ? AdjustedOffsetHours(routeId, stopNumber) : ScheduledOffsetHours(routeId, stopNumber);
            //Adjust the Offset Hours field by the specified amount 
            Debug.WriteLine($"Entering {offsetHoursValue} in the {scheduledOrAdjusted} Offset Hours Column for stop index {stopNumber}");
            //Using enter with js because just entering text natively will not clear text our correctly
            offsetHours.EnterTextWithJS(offsetHoursValue, $"{scheduledOrAdjusted} Offset Hours for stop index {stopNumber}");
            //Save the StopNumber Updated for use later
            ScenarioContext.Current["StopNumUpdated"] = stopNumber;
        }
        public void ClickCascadeButtonForGivenStopNumber(int stopNumber, int routeId, string scheduledOrAdjusted)
        {
            //Locate either the Adjusted Delivery Cascade button or the Scheduled Delivery Cascade button
            IWebElement cascadeButton = scheduledOrAdjusted.ToLower() == "adjusted" ? AdjustedCascadeButton(routeId, stopNumber) : ScheduledCascadeButton(routeId, stopNumber);
            cascadeButton.Click();
            Debug.WriteLine($"Clicking {scheduledOrAdjusted} Cascade Button for stop number {stopNumber} in routeid {routeId}");
        }
        public void EnterGivenCommentValueForAGivenStopNumber(string commentValue, int routeId, int stopNumber)
        {
            IWebElement comment = Comment(routeId, stopNumber);
            //Retrieve Scheduled Delivery value and save off for verification later
            ScenarioContext.Current["ExpectedComment"] = commentValue;
            comment.EnterText(commentValue, $"Comment", true);
            comment.SendKeys(Keys.Tab);
            Debug.WriteLine($"Entering '{commentValue}' in the Comment column for stop number {stopNumber}");
        }
        public void EnterGivenAdjustedDeliveryValueForAGivenStopNumber(string adjustedDeliveryDateValue, string adjustedDeliveryTimeValue, int routeId, int stopNumber)
        {
            string[] rawDate = adjustedDeliveryDateValue.Split('/');

            var formattedAdjustedDeliveryDateValue = string.Join("-", rawDate.Reverse());
            IWebElement adjustedDeliveryDate = AdjustedDeliveryDate(routeId, stopNumber);
            IWebElement adjustedDeliveryTime = AdjustedDeliveryTime(routeId, stopNumber);
            //Retrieve Scheduled Delivery value and save off for verification later
            DateTime currentAdjustedDeliveryValue = DateTime.Parse($"{adjustedDeliveryDate.GetText()} {adjustedDeliveryTime.GetText()}");
            ScenarioContext.Current[$"InitialStopNumber{stopNumber}AdjustedDeliveryValue"] = currentAdjustedDeliveryValue;
            adjustedDeliveryDate.EnterTextWithJS(formattedAdjustedDeliveryDateValue, $"Adjusted Delivery Date for stop {stopNumber}");
            adjustedDeliveryTime.EnterTextWithJS(adjustedDeliveryTimeValue, $"Adjusted Delivery Time for stop {stopNumber}");
            //Using tab to ensure we fire an onblur event   
            adjustedDeliveryDate.SendKeys(Keys.Tab);
            driver.RobustWait();
        }
        /// <summary>
        /// Adds or Removes specified number of minutes from the given stop.
        /// </summary>
        /// <param name="amountToIncrimentBy">Number of minutes to add or remove from the adjusted delivery</param>
        /// <param name="routeId">int value for the Route Id</param>
        /// <param name="stopNumber">int value for the Stop Number</param>
        public void IncrimentAdjustedDeliveryForGivenRouteIdAndStopNumber(string amountToIncrimentBy, int routeId, int stopNumber)
        {
            //Retrieve the Adjusted Delivery Date and Scheduled Delivery Time fields
            IWebElement adjustedDeliveryDate = AdjustedDeliveryDate(routeId, stopNumber);
            IWebElement adjustedDeliveryTime = AdjustedDeliveryTime(routeId, stopNumber);
            DateTime currentAdjustedDeliveryValue = DateTime.Parse($"{adjustedDeliveryDate.GetText()} {adjustedDeliveryTime.GetText()}");
            //Save off current adjusted delivery value for assertions later
            ScenarioContext.Current[$"InitialStopNumber{stopNumber}AdjustedDeliveryValue"] = currentAdjustedDeliveryValue;
            //Add value to the currentAdjustedDeliveryValue then convert to a string in the correct format
            var newAdjustedDeliveryValue = currentAdjustedDeliveryValue.AddMinutes(double.Parse(amountToIncrimentBy));
            //Have to have the seconds in the expected time to match verification later
            ScenarioContext.Current[$"ExpectedStopNumber{stopNumber}AdjustedDeliveryValue"] = newAdjustedDeliveryValue;
            adjustedDeliveryDate.EnterTextWithJS(newAdjustedDeliveryValue.ToString("yyyy-MM-dd"), "Adjusted Delivery Date");
            adjustedDeliveryTime.EnterTextWithJS(newAdjustedDeliveryValue.ToString("hh:mm tt"), "Adjusted Delivery Date");
            adjustedDeliveryTime.SendKeys(Keys.Tab);
            driver.RobustWait();
        }
        public void IncrimentScheduledDeliveryForGivenRouteIdAndStopNumber(string amountToIncrimentBy, int routeId, int stopNumber)
        {
            //Retrieve the Scheduled Delivery Date and Scheduled Delivery Time fields
            IWebElement scheduledDeliveryDate = ScheduledDeliveryDate(routeId, stopNumber);
            IWebElement scheduledDeliveryTime = ScheduledDeliveryTime(routeId, stopNumber);
            //Get the current full Scheduled date and time displayed 
            DateTime currentScheduledDelivery = DateTime.Parse($"{scheduledDeliveryDate.GetText()} {scheduledDeliveryTime.GetText()}");
            //Add value to the currentAdjustedDeliveryValue then convert to a string in the correct format
            var newScheduledDeliveryValue = currentScheduledDelivery.AddMinutes(double.Parse(amountToIncrimentBy));
            ScenarioContext.Current[$"InitialStopNumber{stopNumber}ScheduledDeliveryValue"] = currentScheduledDelivery;
            //Save off the newAdjustedDeliveryValue for verification later
            ScenarioContext.Current[$"ExpectedStopNumber{stopNumber}ScheduledDeliveryValue"] = newScheduledDeliveryValue;
            //Have to set the date as yyyy-MM-dd when using JS
            scheduledDeliveryDate.EnterTextWithJS(newScheduledDeliveryValue.ToString("yyyy-MM-dd"), $"Scheduled Delivery Date for stop {stopNumber}");
            scheduledDeliveryTime.EnterTextWithJS(newScheduledDeliveryValue.ToString("hh:mm tt"), $"Scheduled Delivery Time for stop {stopNumber}");
            scheduledDeliveryDate.SendKeys(Keys.Tab);
            driver.RobustWait();
        }

        /// <summary>
        /// Selects a random stop index for a given route. NOTE: the given routeid must have the stops list displayed to work
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public void GetRandomStopNumberForGivenRouteId(int routeId)
        {
            IWebElement stopsTable = StopsTable(routeId);
            //Get number of rows in the table
            int numberOfStops = AllStopRowsForRoute(routeId).Count();
            Random rnd = new Random();
            int randomStopIndex = rnd.Next(1, numberOfStops);
            //Save off the stop number
            ScenarioContext.Current["SelectedStopNumber"] = GetAdjustedStopNumberGivenStopIndex(routeId, randomStopIndex);
            ScenarioContext.Current["SelectedStopIndex"] = randomStopIndex; //Save off the stop index
        }

        public string RetrieveAdjustedDeliveryValueForGivenRouteAndStopNumber(int routeId, int stopNumber)
        {
            var adjustedDeliveryDateValue = AdjustedDeliveryDate(routeId, stopNumber).GetText();
            var adjustedDeliveryTimeValue = AdjustedDeliveryTime(routeId, stopNumber).GetText();
            return $"{adjustedDeliveryDateValue} {adjustedDeliveryTimeValue}";
        }
        public string RetrieveScheduledDeliveryValueForGivenRouteAndStopNumber(int routeId, int stopNumber)
        {
            //Retrieve the Scheduled Delivery Date and Scheduled Delivery Time values
            var scheduledDeliveryDateValue = ScheduledDeliveryDate(routeId, stopNumber).GetText();
            var scheduledDeliveryTimeValue = ScheduledDeliveryTime(routeId, stopNumber).GetText();
            //Combine these two fields into 1 datetime and return it
            return $"{scheduledDeliveryDateValue} {scheduledDeliveryTimeValue}";
        }
        public string RetrieveOffsetHoursValueForGivenRouteAndStop(int routeId, int stopNumber)
        {
            return AdjustedOffsetHours(routeId, stopNumber).GetText();
        }
        public string RetrieveCommentValueForGivenRouteAndStop(int routeId, int stopNumber)
        {
            return Comment(routeId, stopNumber).GetText();
        }
        //Clicks the 'Submit' button for in the Route Stops section for the given route id
        public void SubmitChangesForRouteStops(int routeId)
        {
            //Locate the Submit button for given routeid
            IWebElement submitButton = driver.FindElement(By.Id("submit-route-" + routeId + "-changes"));
            submitButton.Click();
            Debug.WriteLine("Clicking Submit button to save stop changes.");
            driver.WaitUntilElementIsPresent(By.ClassName("toast-is-shown"), 60); //Wait until the confirmation message appears
            VerifyStopChangesSavedSuccessfully();
            driver.RobustWait();//wait until the toast window disappears
        }
        public void OpenRouteDetailsModalForGivenRouteIdAndStopNumber(int routeId, int stopNumber)
        {
            RouteDetailsLink(routeId, stopNumber).Click();
            //Wait until the Title loads in the Route Details Modal
            driver.WaitUntilElementIsPresent(By.Id("MovedStopRouteDialogTitle"), 45);
        }

        //***Begin Column Options Methods***
        public void OpenColumnOptionsPopup()
        {
            ColumnOptions.Click();
            driver.WaitUntilElementIsPresent(By.Id("columnOptionsDialog")); //Wait until the Column Options popup appears
            driver.RobustWait(); //Wait while fields within the popup render
        }
        //Used to select a given column name and then save changes
        public void AddGivenColumnNameToSelectedColumnsListAndSaveChanges(List<string> columnNames, string stopsOrRoutesList)
        {
            OpenColumnOptionsPopup();
            List<String> currentlySelectedColumnsForGivenList = stopsOrRoutesList == "Stops" ? StopsSelectedColumns.GetOptionsText().ToList() : RoutesSelectedColumns.GetOptionsText().ToList();
            //If all specified columns are already selected
            if (columnNames.All(value => currentlySelectedColumnsForGivenList.Contains(value)))
            {
                //Close the column options popup
                ColumnOptionsCancelButton.Click();
            }
            //Need to select some columns below
            else
            {
                foreach (string columnName in columnNames)
                {
                    if (IsGivenColumnNameSelected(columnName, stopsOrRoutesList) == false)
                    {
                        //If the given column is not already in the selected list, then move it there
                        MoveGivenColumnNameToSelectedColumnsList(columnName, stopsOrRoutesList);
                    }
                }
                SaveColumnOptions(); //Save the column options popup
            }
        }
        public void SelectGivenNumberColumnsFromColumnOptionsRoutesSelectedList(int numberColumns)
        {
            RoutesMoveAllColumnsRight.Click(); //Move all columns to Selected list to make sure we have options to work with
            var numberOfColumns = RoutesSelectedColumns.GetOptions().Count();
            var currentOption = numberOfColumns / 2;
            var counter = 0;
            //Start selecting columns from the middle index and then increase
            while (counter < numberColumns)
            {
                RoutesSelectedColumns.SelectByIndex(currentOption, "Routes - Selected Columns"); //Select the 3rd column in the list
                currentOption++;
                counter++;
            }
            ScenarioContext.Current["ExpectedSelectedColumns"] = RoutesSelectedColumns.GetSelectedOptionsText(); //Save off the selected columns for use later
        }
        public void SelectGivenNumberColumnsFromColumnOptionsStopsSelectedList(int numberColumns)
        {
            StopsMoveAllColumnsRight.Click(); //Move all columns to Selected list to make sure we have options to work with
            var numberOfColumns = StopsSelectedColumns.GetOptions().Count();
            var currentOption = numberOfColumns / 2;
            var counter = 0;
            //Start selecting columns from the middle index and then increase
            while (counter < numberColumns)
            {
                StopsSelectedColumns.SelectByIndex(currentOption, "Stops - Selected Columns"); //Select the 3rd column in the list
                currentOption++;
                counter++;
            }

            ScenarioContext.Current["ExpectedSelectedColumns"] = StopsSelectedColumns.GetSelectedOptionsText(); //Save off the selected columns for use later
        }
        public void DoubleLeftClickOnGivenStopsAvailableColumnName(string columnName)
        {
            StopsSelectedColumns.DoubleLeftClickOnSelectListOption(columnName);
        }
        public void MoveGivenColumnNameToSelectedColumnsList(string columnName, string stopsOrRoutesList)
        {
            //Either use the Stops Selected Columns List or Routes Selected Column list to look at
            var availableColumnList = stopsOrRoutesList == "Stops" ? StopsAvailableColumns : RoutesAvailableColumns;
            //Find button to move column to selected list
            var moveColumnRight = stopsOrRoutesList == "Stops" ? StopsMoveColumnRight : RoutesMoveColumnRight;
            availableColumnList.SelectByText(columnName, $"{stopsOrRoutesList}s - Available Columns List"); //Click on given column name
            moveColumnRight.Click(); //Move the column to the selected list
            IsGivenColumnNameSelected(columnName, stopsOrRoutesList).Should().BeTrue(); //Verify the column actually moved
        }
        public void SaveColumnOptions()
        {
            ColumnOptionsOkButton.Click();
            driver.RobustWait(); //Wait for the page loading spinner to disappear
            driver.RobustWait(); //Wait for the page loading spinner to disappear
        }
        //***End Column Options Methods***

        //***START Activity Log Methods***
        public void OpenActivityLogForGivenRouteIdAndStopNumber(int routeId, int stopNumber)
        {
            StopActivityLogLink(routeId, stopNumber).ScrollToAndClick(); //Find and click the activity log link for the given stop
            driver.WaitUntilElementIsPresent(By.ClassName("comment-table"), 30); //Wait until the Activity Log popup appears
            //Verify the Activity Log loaded for the correct stop
            ActivityLogTitle.GetText().Should().Contain($"ACTIVITY LOG FROM STOP {stopNumber}", $"The Activity Log should have been displayed for stop {stopNumber}");
        }
        public void OpenAggregateActivityLogViewerForGivenRouteId(int routeId)
        {
            RouteActivityLogLink(routeId).Click(); //Find and click the activity log link for the given route
            driver.WaitUntilElementIsPresent(By.ClassName("comment-table"), 45); //Wait until the Activity Log popup appears and then loads the comments
            Debug.WriteLine($"Opened Aggregate Activity Log for Route Id: {routeId}");
        }
        public void CreateActivityLogCommentForGivenStopNumber(int routeId, int stopNumber, string commentValue, string commentCategory, bool commentIsInternal)
        {
            OpenActivityLogForGivenRouteIdAndStopNumber(routeId, stopNumber);
            ActivityLogAddComment.Click();
            ScenarioContext.Current["ExpectedComment"] = commentValue; //Save for use later
            EnterDetailsForActivityLogComment(commentValue, commentCategory, commentIsInternal);
            ActivityLogSaveComment.Click(); //Click Save
            driver.WaitUntilElementIsPresent(By.ClassName("comment-table"), 30); //Wait until the Comments table loads
            ActivityLogTable.Displayed.Should().BeTrue("Activity Log Comment should have saved, but it failed to save."); //Verify comment saved successfully
            Debug.WriteLine($"Created an activity Log for stop index {stopNumber} for RouteId {routeId}");
            ActivityLogClose.Click();
        }
        public void CreateQuickActivityLogCommentForRandomRouteAndGivenStopNumber(int? stopNumber, string commentCategory, bool commentIsInternal)
        {
            OpenQuickActivityLogModal();
            //Select a random option from the Route select list and then save off the Route Id selected
            var selectedRouteId = ActivityLogCommentRouteNumber.SelectByRandomIndex("Activity Log Route Number");
            //Save off the route number for assertions later
            ScenarioContext.Current["CommentedOnRouteNumber"] = ActivityLogCommentRouteNumber.GetSelectedOptionText();
            ScenarioContext.Current["CommentedOnRouteId"] = selectedRouteId; //Save off the route id for assertions later
            var selectedStopNumber = "";
            if (stopNumber == null)
            {
                //Assuming we randomly select a stop number if the param is null
                selectedStopNumber = ActivityLogCommentStopNumber.SelectByRandomIndex("Activity Log Stop Number");
            }
            else
            {
                selectedStopNumber = stopNumber.ToString();
                ActivityLogCommentStopNumber.SelectByText(stopNumber.ToString(), "Activity Log Stop Number");
            }
            ScenarioContext.Current["CommentedOnStopNumber"] = selectedStopNumber; //Save off the stop number for assertion later
            ActivityLogCommentCategory.SelectByText(commentCategory, "Activity Log Comment Category"); //Select a category for the comment
            var commentValue = $"Attempting to make Quick Activity Log comment on {DateTime.Now.ToString("MM/dd/yy HH:mm:ss")} for route id: {selectedRouteId} stop number: {selectedStopNumber}.";
            ScenarioContext.Current["ExpectedComment"] = commentValue; //Save off expected comment
            ActivityLogComment.EnterText(commentValue, "Activity Log Comment");
            ActivityLogSaveComment.Click();
            driver.WaitUntilElementIsPresent(By.ClassName("comment-table"), 30);
            Debug.WriteLine($"Created activity log comment for stop number {stopNumber}");
            Thread.Sleep(2000); //Waiting for the API to process adding the comment. Otherwise we won't see it when we make an assertion.
            ActivityLogClose.Click(); //Close the Quick Activity Log popup
        }
        public void OpenQuickActivityLogModal()
        {
            ActivityLogAddCommentButton.Click();
            driver.WaitUntilElementIsPresent(By.Id("dialog-window"), 30); //Wait until the modal renders
            driver.RobustWait(); //Wait for the activity log to finish rendering
        }
        private void EnterDetailsForActivityLogComment(string comment, string commentCategory, bool commentIsInternal)
        {
            ActivityLogComment.EnterText(comment, "Activity Log Comment");
            ActivityLogCommentCategory.SelectByText(commentCategory, "Activity Log Comment Category");
            if (commentIsInternal)
            {
                ActivityLogCommentIsInteral.Check();
            }
            else
            {
                ActivityLogCommentIsInteral.UnCheck();
            }
        }
        public void GetLatestCommentFromStopLevelActivityLog()
        {
            //Comments are place with the newest one at the bottom of the list so we get the latest comment
            IWebElement comment = ActivityLogComments.Last();
            //Using the data attributes of the Edit icon within the first comment to get the following data
            ScenarioContext.Current["ActualComment"] = comment.GetAttribute("data-comment");
            ScenarioContext.Current["ActualCommentCategory"] = comment.GetAttribute("data-category");
            ScenarioContext.Current["ActualCommentIsInternal"] = bool.Parse(comment.GetAttribute("data-is-internal").ToString());
        }
        /// <summary>
        /// Retrieves a list of all comments for the given stop number
        /// </summary>
        /// <param name="stopNumber"></param>
        /// <returns>List of IWebElements for each comment row</returns>
        public List<Dictionary<string, string>> GetAggregateActivityLogCommentsForGivenStopNumber(int stopNumber)
        {
            IList<IWebElement> allActivityLogCommentRowsForGivenStopNumber = AggregateActivityLogComments.Where(stop => stop.GetAttribute("data-stop-number") == stopNumber.ToString()).ToList();
            List<Dictionary<string, string>> activityLogEntriesForStopNumber = new List<Dictionary<string, string>>();
            Dictionary<string, string> currentActivityLogEntry = new Dictionary<string, string>();
            foreach (var currentRow in allActivityLogCommentRowsForGivenStopNumber)
            {
                //gets and then saves off the value of comment column for the current row
                currentActivityLogEntry["Comment"] = currentRow.FindElement(By.ClassName("stop-comment")).GetText();
                //gets and then saves off the value of comment category column for the current row
                currentActivityLogEntry["Comment Category"] = currentRow.FindElement(By.ClassName("stop-comment-category")).GetText();
                //Add to the list of activityLogEntries
                activityLogEntriesForStopNumber.Add(currentActivityLogEntry);
                //Clear out the current Activity Log Entry
            }
            return activityLogEntriesForStopNumber;
        }
        //***END Activity Log Methods***
    }
}
