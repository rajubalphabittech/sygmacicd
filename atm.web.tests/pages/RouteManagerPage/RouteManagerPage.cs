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
    public partial class RouteManagerPage : BasePage
    {
        public RouteManagerPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/RouteManager";
        public override string PageTitle => "ATM - Route Manager";

        public override void NavigateTo()
        {
            RouteButton.Click();
            RouteManagerButton.Click();
            driver.Title.Should().Contain("R.O.U.T.E.", "the R.O.U.T.E. Page should have loaded");
        }

        /// <summary>
        /// Locates and returns a Stop Card Div given a Stop Number
        /// </summary>
        /// <param name="stopNumber"></param>
        /// <param name="LeftOrRightPanel"></param>
        /// <returns>IWebElement for a StopCard</returns>
        private IWebElement StopCard(string stopNumber, string LeftOrRightPanel)
        {
            if (LeftOrRightPanel == "Left")
            {
                return LeftStopsContainerContent.FindElement(Selectors.SelectorByTagAndAttributeValue("Div", "data-stopnumber", stopNumber));
            }
            else
            {
                return RightStopsContainerContent.FindElement(Selectors.SelectorByTagAndAttributeValue("Div", "data-stopnumber", stopNumber));
            }
        }

        // ***END Methods to locate elements ***


        // ***Methods to Filter or Sort the Route Manager ***
        /// <summary>
        /// Select a value from Date Range in the left panel and save off the start and end dates to ScenarioContext
        /// </summary>
        /// <param name="optionText"></param>
        public void SelectLeftPanelDateRange(string optionText, int? optionIndex)
        {
            //If you don't specify text to select, then use the index
            if (string.IsNullOrEmpty(optionText))
            {
                var optionTargetIndex = new Int32();
                Int32.TryParse(optionIndex.ToString(), out optionTargetIndex);
                LeftDateRange.SelectByIndex(optionTargetIndex, "Left Date Range");
            }
            else
            {
                LeftDateRange.SelectByText(optionText, "LeftDateRange");
            }
            ScenarioContext.Current["FromDateLeftPanel"] = LeftRoutesPanelStartDate.GetText();
            ScenarioContext.Current["ToDateLeftPanel"] = LeftRoutesPanelEndDate.GetText();
        }
        /// <summary>
        /// Used to clear all the filters and then broaden search results to make finding test routes easier
        /// </summary>
        public void ClearFiltersAndBroadenSearchResults()
        {
            LeftRoutesFilterWeightCheckbox.UnCheck();
            //Select 'Tomorrow's Dispatch' from the left panel date range
            //SelectLeftPanelDateRange(null, 1);
            SelectLeftPanelDateRange("Current Week", null);
        }
        public void SortLeftRoutesPanelByNumberOfStops()
        {
            LeftRouteSortOption.SelectByText("Number of Stops", "Route Sort Dropdown");
        }
        public void FilterLeftRoutesPanelByWeight(string targetWeight, string selectedOperator)
        {
            LeftRoutesFilterWeight.EnterText(targetWeight, "Left Routes Filter Weight");
            LeftRoutesFilterWeightOperator.SelectByText(selectedOperator, "Left Routes Filter Weight Operator");
            //Will check the Weight Filter unless it is already checked
            LeftRoutesFilterWeight.Check();
        }
        // *** END of Methods to Filter and Sort the Route Manager ***

        public void ExpandStopsListForGivenRouteId(string routeId, string leftOrRightRoutesPanel)
        {
            IWebElement SelectedRouteCard = RouteCard(routeId, leftOrRightRoutesPanel);
            //Find the Toggle Routes Chevron for the selected route
            IWebElement toggleStopsList = SelectedRouteCard.FindElement(By.ClassName("stop-list-toggle"));
            toggleStopsList.Click();
            //Waits until 1 stop is displayed in the stops table
            driver.WaitUntilElementIsPresent(By.ClassName("stop-item"));
            driver.RobustWait(); //Wait for things to finish loading in the stops table
        }

        public void SearchForGivenRouteNumber(string routeNumber, string leftOrRightRoutePanel)
        {
            leftOrRightRoutePanel = leftOrRightRoutePanel.ToLower(); //Make sure we use a lower case value
            //Find the Search Box in either the left or right routes panel then enter given route #
            SearchBox(leftOrRightRoutePanel).EnterText(routeNumber, $"{leftOrRightRoutePanel} Routes Panel Search Box");
        }

        /// <summary>
        /// Locates and selects a route from the Left Routes panel by the Route Number
        /// </summary>
        public void SelectRouteByRouteNumberFromGivenRoutesPanel(string routeNumber, string leftOrRightPanel)
        {
            IWebElement targetRoute = LeftRoutesList.FindElement(Selectors.SelectorByTagAndAttributeValue("Div", "data-routenumber", routeNumber));
            Debug.WriteLine($"Selecting Route# {routeNumber} from the {leftOrRightPanel} routes panel.");
            SelectRouteCheckbox(targetRoute).Check();
            Thread.Sleep(3000); //Sleep while the route stop markers load
        }

        public void DeSelectRouteFromTheLeftRoutesPanelGivenRouteId(string routeId)
        {
            SelectRouteCheckbox(RouteCard(routeId, "left")).UnCheck(); //Uncheck to deselect the route
        }

        /// <summary>
        /// Selects a route from the Left or Right Routes panel based on the routeId provided
        /// </summary>
        public void SelectRouteToShowInGMapsByGivenRouteIdAndTargetPanel(int routeId, string leftOrRightPanel)
        {
            //Find the target route card from either the left or right routes panel
            IWebElement targetRoute = RouteCard(routeId.ToString(), leftOrRightPanel);
            //Get data for given routes and save it to ScenarioContext
            TestDataHelper.SaveExpectedValuesForRouteId(routeId);
            //Get data for the selected route
            Dictionary<string, string> selectedRoute = ((Dictionary<string, Dictionary<string, string>>)ScenarioContext.Current["SelectedRoutes"])[routeId.ToString()]; ;
            var routeNumber = selectedRoute["RouteNumber"];
            //Search for the route number so that it's in view when we click the select box
            SearchForGivenRouteNumber(routeNumber, leftOrRightPanel);
            //Find the Checkbox to select the route with and check it
            SelectRouteCheckbox(targetRoute).Check();
            Debug.WriteLine($"Selected Route Id {routeId} from the {leftOrRightPanel} routes panel.");
            //Have to hard code sleep as the google maps renders markers individually
            Thread.Sleep(1500); //Wait for the markers to load
        }

        //public void FindAndSelectRouteWithCapacityData()
        //{
        //    FilterLeftRoutesPanelByWeight("20000", ">");
        //    //Safe to select the first route becuase we know the route has capacity and stops
        //    SelectRouteByIndexFromTheLeftRoutesPanel(0);
        //}

        /// <summary>
        /// Saves the Total Weight, Total Cubes, and Total Cases values to ScenarioContext for given Source routeId
        /// </summary>
        /// <param name="routeId"></param>
        public void SaveSourceRouteTotalCapacityTotalsForGivenRouteId(string routeId)
        {
            IWebElement sourceRoute = RouteCard(routeId, "Left");
            GetAndSaveRouteHeaderCapacityValues(sourceRoute, "Source");
        }

        /// <summary>
        /// Saves the Total Weight, Total Cubes, and Total Cases values to ScenarioContext for given Target routeId
        /// </summary>
        /// <param name="routeId"></param>
        public void SaveTargetRouteTotalCapacityTotalsForGivenRouteId(string routeId)
        {
            IWebElement targetRoute = RouteCard(routeId, "Right");
            GetAndSaveRouteHeaderCapacityValues(targetRoute, "Target");
        }

        private void GetAndSaveRouteHeaderCapacityValues(IWebElement route, string sourceOrTarget)
        {
            ScenarioContext.Current[$"{sourceOrTarget}RouteInitialTotalWeight"] = route.GetAttribute("data-totalweight");
            ScenarioContext.Current[$"{sourceOrTarget}RouteInitialTotalCubes"] = route.GetAttribute("data-totalcubes");
            ScenarioContext.Current[$"{sourceOrTarget}RouteInitialTotalCases"] = route.GetAttribute("data-totalcases");
        }

        public void OpenMoveStopPromptForGivenStopNumber(int sourceStopNumber)
        {
            ClickOnMoveStopForGivenStopNumber(sourceStopNumber);
        }

        public void OpenMoveStopForStopWithCapacityValues()
        {
            //Expand the stops table for the last route we selected in the left routes panel
            ExpandStopsListForGivenRouteId(ScenarioContext.Current["LastSelectedRouteId"].ToString(), "left");
            var numberOfStops = LeftStopCards.Count();
            numberOfStops.Should().BeGreaterOrEqualTo(3, "Need to have at least 5 stops for the selected route to continue");
            var stopIndex = 0;
            //While current stop is not removed and the current stop has a value for the total stop weight
            while ((LeftStopCards[stopIndex].FindElements(By.ClassName("bg-danger")).Count() == 1) || (float.Parse(LeftStopCards[stopIndex].GetAttribute("data-weight")) + float.Parse(LeftStopCards[stopIndex].GetAttribute("data-cubes")) + float.Parse(LeftStopCards[stopIndex].GetAttribute("data-cases")) > 0.0))
            {
                stopIndex.Should().BeLessThan(LeftStopCards.Count() - 1, $"All stops in route {ScenarioContext.Current["LastSelectedRouteId"]} have been removed or don't have any capacity values. Failing test for this reason.");
                stopIndex++;
            }
            IWebElement targetStop = LeftStopCards[stopIndex];
            var stopNumber = Convert.ToInt32(targetStop.GetAttribute("data-stopnumber"));
            ScenarioContext.Current["SourceStop"] = stopNumber;
            ClickOnMoveStopForGivenStopNumber(stopNumber);
        }

        private void ClickOnStopProximitySearchForGivenStopCard(IWebElement stopCard)
        {

            ProximitySearch(stopCard).Click();
            driver.WaitUntilElementIsPresent(By.ClassName("row-right"), 60); //Wait until 1 route is shown in the right routes panel
        }

        private void ClickOnMoveStopForGivenStopNumber(int stopNumber)
        {
            IWebElement moveStopButton = MoveStopButton(stopNumber);
            //Move stop button will be disabled by js viewing the data-canmove attribute
            bool isMoveStopButtonEnabled = moveStopButton.GetAttribute("data-canmove") == "True";
            isMoveStopButtonEnabled.Should().BeTrue($"Move stop button should have been enabled. Please verify stop: {stopNumber} should be able to be moved");
            moveStopButton.Click();
            driver.WaitUntilElementIsPresent(By.ClassName("stop-list-is-displayed"), 30); //Wait until move stop popup shown
            driver.RobustWait(); //Wait for the move stop popup to finish loading
            MoveStopsDialog.Displayed.Should().BeTrue("Because the Move Stops popup should have been present");
        }

        public void PopulateStopMovePopup(string commentValue, string targetRouteNumber, string targetStopNumber = null, string sourceRouteNumber = null, string sourceStopNumber = null, string deliveryDateTimeValue = null)
        {
            //If you specified a source route number then populate the source route and stop
            if (sourceRouteNumber != null)
            {
                FromRouteNumber.SelectByText(sourceRouteNumber, "From Route Number");
                FromStopNumber.SelectByText(sourceStopNumber, "From Stop Number");
            }
            ToNewRouteNumber.SelectByText(targetRouteNumber, "New Route Number");
            NewComment.EnterText(commentValue, "Move Stop Comment");
            ToNewStopNumber.EnterText(targetStopNumber, "NEW STOP NUMBER");
            ToNewStopNumber.SendKeys(Keys.Tab); //Using tab to fire blur event b/c otherwise js never fires to allow user to click save.
            //If you specified a delivery date/time then use it. Otherwise leave the default value.
            if (deliveryDateTimeValue != null)
            {
                NewDeliveryDateTime.EnterText(deliveryDateTimeValue, "New Delivery Date Time");
            }
            Debug.WriteLine($"Attempting to move Route {FromRouteNumber.GetSelectedOptionText()} Stop {FromStopNumber.GetSelectedOptionText()} to Route {targetRouteNumber} Stop {targetStopNumber} with a new Delivery Date of {NewDeliveryDateTime.GetText()}");
        }

        public List<string> GetRouteNumbersDisplayedInLeftRoutesPanel()
        {
            //get and return the route number for each route shown in the left routes panel
            return RouteCards("left").Select(route => route.GetAttribute("data-routenumber").ToString()).ToList();
        }

        public IList<string> GetStopMarkerLabelsFromGoogleMaps()
        {
            IList<string> stopMarkerLabelsText = new List<string>();
            IList<IWebElement> markerIWebElements = GoogleMapMarkers();
            stopMarkerLabelsText = markerIWebElements.Select(marker => marker.GetText()).ToList();
            return stopMarkerLabelsText;
        }

        public void OpenQuickActivityLogModal()
        {
            ActivityLogAddCommentButton.Click();
            driver.WaitUntilElementIsPresent(By.Id("dialog-window"), 30); //Wait until the modal renders
            driver.RobustWait(); //Wait for the activity log to finish rendering
        }
        public void OpenQuickStopMoveModal()
        {
            QuickStopMove.Click();
            driver.WaitUntilElementIsPresent(By.ClassName("stop-move-container"), 30);
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
                ActivityLogCommentStopNumber.SelectByRandomIndex("Activity Log Stop Number");
            }
            else
            {
                selectedStopNumber = stopNumber.ToString();
                ActivityLogCommentStopNumber.SelectByText(stopNumber.ToString(), "Activity Log Stop Number");
            }
            ScenarioContext.Current["CommentedOnStopNumber"] = ActivityLogCommentStopNumber.GetSelectedOptionText(); //Save off the stop number for assertion later
            ActivityLogCommentCategory.SelectByText(commentCategory, "Activity Log Comment Category"); //Select a category for the comment
            var commentValue = $"Attempting to make Quick Activity Log comment on {DateTime.Now.ToString("MM/dd/yy HH:mm:ss")} for route id: {selectedRouteId} stop number: {selectedStopNumber}.";
            ScenarioContext.Current["ExpectedComment"] = commentValue; //Save off expected comment
            ScenarioContext.Current["ExpectedCommentCategory"] = commentCategory; //Save off expected comment category
            ActivityLogComment.EnterText(commentValue, "Activity Log Comment");
            ActivityLogSaveComment.Click();
            driver.WaitUntilElementIsPresent(By.ClassName("comment-table"), 30);
            Debug.WriteLine($"Created activity log comment for stop number {stopNumber}");
        }
        public void SaveStopMove()
        {
            SaveStopMoveButton.Click();
        }
        public void SaveStopMoveAndVerifySuccessfulSave()
        {
            SaveStopMove();
            driver.WaitUntilElementIsPresent(By.Id("toast-window"), 60);
            //Retrieve the save result notification shown to the user
            var saveResult = driver.FindElement(By.Id("toast-window")).GetText();
            //Verify the stop move was successful
            saveResult.Should().Contain("Stop move is successful", "The should move should have been successful");
            Debug.WriteLine("Stop Move Saved Successfully.");
            driver.RobustWait(); //Wait for the page to reload
        }
    }
}
