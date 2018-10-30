using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class UserMaintenancePage
    {
        private IWebElement SecurityTab { get { return this.driver.FindElement(By.LinkText("Security")); } }
        private IWebElement UserMaintenanceButton { get { return this.driver.FindElement(By.LinkText("User Maintenance")); } }

        private IWebElement Users { get { return this.driver.FindElement(By.Id("body_ddUser")); } }
        public IWebElement ViewPayRatesScalesPageCheckbox { get { return this.driver.FindElement(By.Id("body_dlSections_rptFunctions_0_24_2")); } }
    }
}
