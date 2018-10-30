using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class ManageVehiclesAndTrailersPage
    {
        public IWebElement ToolsButton { get { return driver.FindElement(By.LinkText("Tools")); } }
        public IWebElement ManageVehiclesAndTrailersButton { get { return driver.FindElement(By.LinkText("Manage Vehicles & Trailers")); } }
        public IWebElement VehiclesTrailersTable { get { return driver.FindElement(By.Id("body_gvVT")); } }

        public IWebElement Center { get { return driver.FindElement(By.Id("body_ddProgSygmaCenterNo")); } }
        public IWebElement VehicleRowCount { get { return driver.FindElement(By.Id("body_VehicleTrailerRowCountBar1_lblVehiclesCount")); } }
        public IWebElement TrailerRowCount { get { return driver.FindElement(By.Id("body_VehicleTrailerRowCountBar1_lblTrailersCount")); } }
    }
}
