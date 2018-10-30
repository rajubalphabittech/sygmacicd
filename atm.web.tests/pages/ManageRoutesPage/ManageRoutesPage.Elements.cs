using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class ManageRoutesPage
    {
        public IWebElement ToolsButton { get { return this.driver.FindElement(By.LinkText("Tools")); } }
        public IWebElement ManageRoutesButton { get { return this.driver.FindElement(By.LinkText("Manage Routes")); } }
        public IWebElement ManageRoutesTable { get { return this.driver.FindElement(By.Id("body_gvRoutes")); } }

        //Search Fields
        public override IWebElement CenterDropDown => driver.FindElement(By.Id("body_ddSygmaCenterNo"));
        public IWebElement RowCount { get { return this.driver.FindElement(By.Id("body_RowCountBar1_lblItemCount")); } }
        public IWebElement RoutesTable { get { return this.driver.FindElement(By.Id("body_gvRoutes")); } }
        public IWebElement AddNewRoute { get { return driver.FindElement(By.Id("body_imgAddRoute")); } }
        public IWebElement SearchForRoute { get { return driver.FindElement(By.Id("body_txtRouteSearch")); } }

        //Fields in the create new route popup
        private IWebElement NewRouteNumber { get { return driver.FindElement(By.Id("txtRouteNo")); } }
        private IWebElement NewRouteLocation { get { return driver.FindElement(By.Id("body_ddAddLocation")); } }
        private IWebElement NewRouteClassification { get { return driver.FindElement(By.Id("body_ddAddClassification")); } }
        private IWebElement NewRouteMiles { get { return driver.FindElement(By.Id("txtAddMiles")); } }
        private IWebElement NewRouteDefaultDriver { get { return driver.FindElement(By.Id("body_ddAddDriver")); } }
        private IWebElement NewRouteDriverPayScale { get { return driver.FindElement(By.Id("body_ddAddDriverPayScale")); } }
        private IWebElement NewRouteDriverHelperPayScale { get { return driver.FindElement(By.Id("body_ddAddHelperPayScale")); } }
        private IWebElement NewRouteZipCode { get { return driver.FindElement(By.Id("txtAddZipCode")); } }
        private IWebElement NewRouteDepartDay { get { return driver.FindElement(By.Id("body_ddDepartDay")); } }
        private IWebElement NewRouteDepartTime { get { return driver.FindElement(By.Id("txtDepartTime")); } }
        private IWebElement NewRouteDuration { get { return driver.FindElement(By.Id("txtDuration")); } }
        private IWebElement NewRouteOkButton { get { return driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div/button[1]")); } }
    }
}
