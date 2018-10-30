using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using atm.web.tests.common;
using FluentAssertions;

namespace atm.web.tests.pages
{
    public partial class RouteManagerPage
    {
        //Navbar
        public IWebElement RouteButton { get { return driver.FindElement(By.LinkText("Route")); } }
        public IWebElement RouteManagerButton { get { return driver.FindElement(By.LinkText("Manager")); } }
        public override IWebElement CenterDropDown => driver.FindElement(By.Id("RouteCenterSelector"));
        public IWebElement ActivityLogAddCommentButton => driver.FindElement(By.Id("managerAddCommentButton"));
        public IList<IWebElement> GoogleMapMarkers() => driver.FindElements(By.ClassName("pin-wrap"));

        //***BEGIN Left Routes table
        public IWebElement LeftRouteLink => driver.FindElement(By.Id("leftRouteTab"));
        //Left Routes Search / Filter fields
        public IWebElement LeftDateRange => driver.FindElement(By.Id("RouteTimingTypeSelectorleft"));
        public IWebElement LeftRoutesPanelStartDate => driver.FindElement(By.Id("RouteDispatchStartDateleft"));
        public IWebElement LeftRoutesPanelEndDate => driver.FindElement(By.Id("RouteDispatchEndDateleft"));
        public IWebElement LeftBillTo { get { return driver.FindElement(By.Id("RouteStopBillToleft")); } }
        public IWebElement LeftShipTo { get { return driver.FindElement(By.Id("RouteStopShipToleft")); } }
        public IWebElement LeftSearchButton { get { return driver.FindElement(By.Id("RouteSearchButtonleft")); } }
        public IWebElement LeftRoutesFilterWeightCheckbox { get { return driver.FindElement(By.Id("RouteFilterWeightCheckboxleft")); } }
        public IWebElement LeftRoutesFilterWeight { get { return driver.FindElement(By.Id("RouteFilterWeightleft")); } }
        public IWebElement LeftRoutesFilterWeightOperator { get { return driver.FindElement(By.Id("RouteFilterWeightOperatorleft")); } }


        //public IWebElement LeftSearchBox { get { return driver.FindElement(By.Id("RouteNameNumberleft")); } }
        //Private b/c you need to use the OpenQuickStopMove method instead
        private IWebElement QuickStopMove => driver.FindElement(By.Id("managerMoveStopButton"));
        //Left Routes Table fields
        public IWebElement LeftRouteSortOption { get { return driver.FindElement(By.Id("RouteSorterleft")); } }
        public IWebElement LeftRoutesList { get { return driver.FindElement(By.Id("routeListleft")); } }
        //***END Left Routes Table

        //*** Begin Route Card Fields For Left and Right Routes Panel ***
        //The following fields require you to pass in a route card IWebElement and can therefore be used for the left or right routes panels
        private IWebElement ProximitySearch(IWebElement routeCard) => routeCard.FindElement(By.ClassName("primary-stop-toggle"));
        private IWebElement SelectRouteCheckbox(IWebElement routeCard) => routeCard.FindElement(By.ClassName("route-in-map"));
        //Retrieves the specified Route Card from the Left or the Right Routes panel
        public IWebElement RouteCard(string routeId, string leftOrRightPanel) => RouteList(leftOrRightPanel).FindElement(Selectors.SelectorByTagAndAttributeValue("div", "data-routeid", routeId));
        public IList<IWebElement> RouteCards(string leftOrRightPanel)
        {
            //Returns all displayed Route Cards from the Left or the Right Routes panel
            return RouteList(leftOrRightPanel).FindElements(By.ClassName("route-item")).Where(route => route.Displayed == true).ToList();
        }
        //Returns the Route List from the Left or the Right Routes panel
        public IWebElement RouteList(string leftOrRightPanel)
        {
            string targetPanel = leftOrRightPanel.ToLower() == "left" ? "left" : "right";
            //Find the RouteList
            return driver.FindElement(By.Id($"routeList{targetPanel}"));
        }
        //Returns the Search Box from the Left or the Right Routes panel
        private IWebElement SearchBox(string leftOrRightPanel) => driver.FindElement(By.Id($"RouteNameNumber{leftOrRightPanel}"));
        //*** END Route Card Fields For Left and Right Routes Panel ***

        //Right Routes table
        public IWebElement RightRouteLink { get { return driver.FindElement(By.Id("rightRouteTab")); } }
        public IWebElement RightStopsContainerContent { get { return driver.FindElement(By.XPath("//*[@class='stop-list-content']//*[@data-position='right']")); } }


        //***BEGIN Left Stops Table ***
        public IList<IWebElement> LeftStopCards => LeftStopsContainer.FindElements(By.ClassName("stop-item"));
        //Shows all the Stop Card Headers which contain html attributes we can use to find details about each stop 
        public IList<IWebElement> LeftStopCardsHeaders => LeftStopsContainer.FindElements(By.ClassName("stop-header"));
        //Get the Stop Card for a given stop number in the Left Routes Stops table
        private IWebElement LeftStopCard(int stopNumber) => LeftStopsContainer.FindElement(Selectors.SelectorByAttributeValue("data-stopnumber", stopNumber.ToString()));
        public IWebElement LeftStopsContainer { get { return driver.FindElements(By.ClassName("stop-list")).FirstOrDefault(); } }
        public IWebElement LeftStopsContainerContent { get { return driver.FindElement(By.XPath("//*[@class='stop-list-content']//*[@data-position='left']")); } }
        private IWebElement MoveStopButton(int stopNumber) => LeftStopCard(stopNumber).FindElement(By.ClassName("stop-move-btn"));
        //***End Left Stops Table ***

        //*** Stop Move Details ***
        public IWebElement MoveStopsDialog => driver.FindElement(By.ClassName("stop-move-container"));
        public IWebElement FromRouteNumber => driver.FindElement(By.Id("SourceRouteNumberDDL"));
        public IWebElement FromStopNumber => driver.FindElement(By.Id("SourceStopNumberDDL"));
        public IWebElement ToNewRouteNumber => driver.FindElement(By.Id("DestinationRouteNumberDDL"));
        public IWebElement ToNewStopNumber => driver.FindElement(By.Id("destinationStopNumber"));
        public IWebElement NewDeliveryDateTime => driver.FindElement(By.Id("destinationDeliveryDateTime"));
        public IWebElement NewComment => driver.FindElement(By.Id("stopModificationComment"));
        public IWebElement SaveStopMoveButton => driver.FindElement(By.XPath("//button[contains(text(),'Save')]"));
        //Target Route Section
        private IWebElement TargetRouteTotalWeight => driver.FindElement(By.Id("DestinationTotalWeight"));
        private IWebElement TargetRouteTotalWeightUpdated => driver.FindElement(By.Id("DestinationTotalWeightUpdated"));
        private IWebElement TargetRouteTotalWeightDifference => driver.FindElement(By.Id("DestinationTotalWeightUpdated"));
        private IWebElement TargetRouteTotalCubes => driver.FindElement(By.Id("DestinationTotalCubes"));
        private IWebElement TargetRouteTotalCubesUpdated => driver.FindElement(By.Id("DestinationTotalCubesUpdated"));
        private IWebElement TargetRouteTotalCubesDifference => driver.FindElement(By.Id("DestinationTotalCubesDifference"));
        private IWebElement TargetRouteTotalCases => driver.FindElement(By.Id("DestinationTotalCases"));
        private IWebElement TargetRouteTotalCasesUpdated => driver.FindElement(By.Id("DestinationTotalCasesUpdated"));
        private IWebElement TargetRouteTotalCasesDifference => driver.FindElement(By.Id("DestinationTotalCasesDifference"));
        //Source Route Section
        public IWebElement SourceStopScheduledDeliveryDateTime => driver.FindElement(By.Id("SourceScheduledDeliveryDateTime"));
        private IWebElement SourceRouteTotalWeightDifference => driver.FindElement(By.Id("SourceTotalWeightDifference"));
        private IWebElement SourceRouteTotalWeightUpdated => driver.FindElement(By.Id("SourceTotalWeightUpdated"));
        private IWebElement SourceRouteTotalCubesDifference => driver.FindElement(By.Id("SourceTotalCubesDifference"));
        private IWebElement SourceRouteTotalCubesUpdated => driver.FindElement(By.Id("SourceTotalCubesUpdated"));
        private IWebElement SourceRouteTotalCasesDifference => driver.FindElement(By.Id("SourceTotalCasesDifference"));
        private IWebElement SourceRouteTotalCasesUpdated => driver.FindElement(By.Id("SourceTotalCasesUpdated"));
        //*** END Stop Move Details ***

        //*** BEGIN QUICK STOP MOVE DETAILS ***
        //*** End QUICK STOP MOVE DETAILS ***

        //***BEGIN Activity Log Popup Fields***
        public IWebElement ActivityLogCommentRouteNumber => driver.FindElement(By.Id("commentRouteNumber"));
        public IWebElement ActivityLogCommentStopNumber => driver.FindElement(By.Id("commentStopNumber"));
        private IWebElement ActivityLogCommentCategory => driver.FindElement(By.Id("comment-category"));
        private IWebElement ActivityLogCommentIsInteral => driver.FindElement(By.Id("comment-is-internal"));
        private IWebElement ActivityLogComment => driver.FindElement(By.Id("comment-box"));
        public IWebElement ActivityLogSaveComment => driver.FindElement(By.XPath("//button[.='Save']"));
        public IWebElement ActivityLogOk => driver.FindElement(By.XPath("//button[.='OK']"));
        public IWebElement ActivityLogCallLogging => driver.FindElement(By.XPath("//button[.='Call Logging']"));
        public IWebElement ActivityLogClose => driver.FindElement(By.XPath("//button[.='Close']"));
        //Comments rows in the Activity Log for a given stop number
        private IList<IWebElement> ActivityLogComments => driver.FindElements(By.ClassName("comment-edit"));
        //Comments rows in the Activity Log popup
        private IList<IWebElement> ActivityLogCommentRows => driver.FindElement(By.ClassName("comment-table")).FindElements(By.ClassName("stop-comment-row"));
        //***END Activity Log Popup Fields ***

    }
}
