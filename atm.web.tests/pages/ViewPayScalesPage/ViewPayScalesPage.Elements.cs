using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class ViewPayScalesPage
    {
        public IWebElement PayrollButton { get { return driver.FindElement(By.LinkText("Payroll")); } }
        public IWebElement ViewPayScales { get { return driver.FindElement(By.LinkText("Pay Scales")); } }

        public override IWebElement CenterDropDown => driver.FindElement(By.Id("sygmaCenterNo"));
        public IWebElement PayScales { get { return driver.FindElement(By.Id("payScale")); } }
        public IWebElement PayScaleRatesTable { get { return driver.FindElement(By.Id("ratesTable")); } }
    }
}
