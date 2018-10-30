using OpenQA.Selenium;

namespace atm.web.tests.pages
{
    public partial class PayrollFormPage
    {
        //Create New Payroll Form Fields
        public IWebElement FormType { get { return this.driver.FindElement(By.Id("ddFormType")); } }
        public override IWebElement CenterDropDown => driver.FindElement(By.Id("ddSygmaCenterNo"));
        public IWebElement RouteNumber { get { return this.driver.FindElement(By.Id("txtRouteNo")); } }
        public IWebElement DepartDate { get { return this.driver.FindElement(By.Id("dteDepartDate")); } }
        public IWebElement WeekendingDate { get { return this.driver.FindElement(By.Id("dteWeekendingDate")); } }
        public IWebElement Cases { get { return this.driver.FindElement(By.Id("txtCasesOnCreate")); } }
        public IWebElement Pounds { get { return this.driver.FindElement(By.Id("txtPoundsOnCreate")); } }
        public IWebElement Cubes { get { return this.driver.FindElement(By.Id("txtCubesOnCreate")); } }
        public IWebElement Stops { get { return this.driver.FindElement(By.Id("txtStopsOnCreate")); } }
        public IWebElement Create { get { return this.driver.FindElement(By.Id("btnCreate")); } }
        public IWebElement Cancel { get { return this.driver.FindElement(By.Id("btnCancel")); } }

        //Payroll Form Details Fields
        public IWebElement FormNumber { get { return this.driver.FindElement(By.Id("lblFormId")); } }
        public IWebElement RouteNumberLabel { get { return this.driver.FindElement(By.Id("lblRouteNo")); } }
        public IWebElement DepartDateLabel { get { return this.driver.FindElement(By.Id("lblDepartDate")); } }
    }
}
