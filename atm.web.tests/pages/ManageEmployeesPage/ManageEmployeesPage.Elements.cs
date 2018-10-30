using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class ManageEmployeesPage
    {
        public IWebElement ToolsButton { get { return this.driver.FindElement(By.LinkText("Tools")); } }
        public IWebElement ManageEmployeesButton { get { return this.driver.FindElement(By.LinkText("Manage Employees")); } }

        public override IWebElement CenterDropDown => driver.FindElement(By.Id("body_ddProgSygmaCenterNo"));

        public IWebElement EmployeeCount { get { return this.driver.FindElement(By.Id("body_lblEmployeeCount")); } }
    }
}
