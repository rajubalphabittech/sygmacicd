using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class ViewEmployeesPage
    {
        public IWebElement ToolsButton { get { return this.driver.FindElement(By.LinkText("Tools")); } }
        public IWebElement ViewEmployeesButton { get { return this.driver.FindElement(By.LinkText("View Employees")); } }

        public override IWebElement CenterDropDown => driver.FindElement(By.Id("ctl00_body_ddProgSygmaCenterNo"));

        public IWebElement EmployeeCount { get { return this.driver.FindElement(By.Id("ctl00_body_lblEmployeeCount")); } }
    }
}
