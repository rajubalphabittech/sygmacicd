using atm.web.tests.common;
using OpenQA.Selenium;
using System;
using System.Threading;
using TechTalk.SpecFlow;
using static atm.web.tests.common.BaseTest;

namespace atm.web.tests.pages
{
    public partial class PayrollFormPage : BasePage
    {
        public PayrollFormPage(IWebDriver driver) : base(driver)
        {
        }

        public override string Url => ConstantsUtils.Url + "/Apps/ATM/Payroll/Forms/AddUpdate.aspx";
        public override string PageTitle => "ATM - Payroll Form";

        public void InputPayrollFormDetails(string FormTypetoCreate, string CurrentOrClosedWeekending)
        {
            //Inputs all information needed to create a new payroll form
            FormType.SelectByText(FormTypetoCreate, "FormType");
            CenterDropDown.SelectByIndex(1, "Center");

            var routeNumberValue = GenerateRandomRouteNumber();
            //Save this route number value for use later
            ScenarioContext.Current["RouteNumber"] = routeNumberValue;
            RouteNumber.EnterText(routeNumberValue, "RouteNumber");
            RouteNumber.SendKeys(Keys.Tab);

            var WeekendingDateToSelect = DateTime.Now;
            if (CurrentOrClosedWeekending == "Current")
            {
                WeekendingDateToSelect = (DateTime.Now.AddDays(6 - (int)DateTime.Now.DayOfWeek));
            }
            else
            {
                DateTime StartOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                WeekendingDateToSelect = StartOfWeek.AddDays(-1);
            }
            var departDateValue = WeekendingDateToSelect.AddDays(-2).ToShortDateString();
            ScenarioContext.Current["DepartDate"] = departDateValue;
            DepartDate.EnterText(departDateValue, "DepartDate");
            DepartDate.SendKeys(Keys.Tab);
            WeekendingDate.EnterText(WeekendingDateToSelect.ToShortDateString(), "WeekendingDate");
            WeekendingDate.SendKeys(Keys.Tab);
            driver.RobustWait();

            if (FormTypetoCreate == "Regular")
            {
                Cases.EnterText("25", "Cases");
                Pounds.EnterText("4000", "Cases");
                Cubes.EnterText("25", "Cubes");
                Stops.EnterText("5", "Stops");
            }
        }

        public string GenerateRandomRouteNumber()
        {
            Random rnd = new Random();
            //Generate the first 3 digits of the route number
            var routeNumberValue = rnd.Next(100, 999).ToString();
            //Generate the int value representing a random value between a-z
            var charNumber = rnd.Next(0, 26); // Zero to 25
            routeNumberValue += (char)('a' + charNumber);
            return routeNumberValue;
        }

        /// <summary>
        /// Method to click Create button and then wait for the Payroll Form page to load 
        /// </summary>
        public void SubmitNewPayrollForm()
        {
            Create.Click();
            //Hard coding sleep to account for time Chrome is opening a new tab
            Thread.Sleep(5000);
            //Wait for the resulting page to load
            driver.RobustWait();
        }
    }
}
