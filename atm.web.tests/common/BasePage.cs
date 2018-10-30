using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace atm.web.tests.common
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait browserWait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            this.browserWait = Driver.BrowserWait;
        }

        public virtual string Url => string.Empty;
        public virtual string PageTitle => string.Empty;

        public virtual IWebElement CenterDropDown => null;

        public virtual void Open(string part = "")
        {
            if (string.IsNullOrEmpty(this.Url))
            {
                throw new ArgumentException("The main URL can't be null or empty.");
            }
            driver.Navigate().GoToUrl(string.Concat(this.Url, part));
            driver.RobustWait();
            VerifyAtmIsAvailable();
            //Verify the page actually loaded
            driver.Title.Should().Contain(PageTitle);
            Debug.WriteLine($"Navigated to the {GetType().Name} page successfully");
        }

        public virtual void SelectRandomCenter()
        {
            IList<IWebElement> options = CenterDropDown.GetOptions();
            Random rnd = new Random();
            //Pick a option from the center list exclusing the first option as it is 'Select Center...'
            var randomCenterIndex = rnd.Next(1, options.Count() - 1);
            //If Denver2 would be selected, then use the next center instead
            if (options[randomCenterIndex].Text == "Denver2")
            {
                randomCenterIndex++;
            }
            CenterDropDown.SelectByIndex(randomCenterIndex, "Center Selector");
            driver.RobustWait(); //Wait for any js to fire
            ScenarioContext.Current["CenterNumber"] = CenterDropDown.GetText();
            Debug.WriteLine($"Selected Center '{CenterDropDown.GetSelectedOptionText()}'");
        }

        //Select a center from dd list. Has optional CenterName param.
        public virtual void SelectCenterByName(string CenterName)
        {
            CenterDropDown.SelectByText(CenterName, "Center");
            ScenarioContext.Current["CenterNumber"] = CenterDropDown.GetText();
            Debug.WriteLine($"Selected Center Number: {CenterDropDown.GetText()}");
            driver.RobustWait();
        }

        public virtual void NavigateTo()
        { 
}
        private void VerifyAtmIsAvailable()
        {
            switch (driver.Title)
            {
                case "Service Unavailable":
                    throw new Exception("ATM is currently unavailable.");
                case "localhost":
                    throw new Exception("ATM is not currently running. Please start ATM without debugging.");
                case "Timeout expired.  The timeout period elapsed prior to obtaining a connection from the pool.  This may have occurred because all pooled connections were in use and max pool size was reached.":
                    throw new Exception("ATM is unavailable due to the timeout pool elapsing before a connection was made.");
            }
        }
    }
}
