using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Threading;

namespace atm.web.tests.common
{
    public static class WebDriverWaitExtensions
    {
        /// <summary>
        /// Method to account for js events, loading icons, and js alerts
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static bool RobustWait(this IWebDriver driver)
        {
            WaitForDocumentReady(driver);
            bool ajaxReady = WaitForAjaxReady(driver);
            WaitForDocumentReady(driver);

            return ajaxReady;
        }

        private static bool WaitForAjaxReady(this IWebDriver driver)
        {
            Thread.Sleep(1000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            return wait.Until<bool>((d) =>
            {
                try
                {
                    //Debug.WriteLine("Waiting to load page...");
                    //Must have a 'loading' class on the loading indicators
                    IWebElement element = d.FindElement(By.ClassName("loading"));
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    // If the find fails, the element exists, and
                    // by definition, cannot then be visible.
                    return true;
                }
                catch (UnhandledAlertException)
                {
                    //Debug.WriteLine("Unexpected Alert present.");
                    return true;
                }
            });
        }

        private static void WaitForDocumentReady(this IWebDriver driver)
        {
            var timeout = new TimeSpan(0, 0, 75);
            var wait = new WebDriverWait(driver, timeout);

            wait.Until((d) =>
            {
                try
                {
                    Thread.Sleep(500);
                    var jquery = (bool)(driver as IJavaScriptExecutor)
                    .ExecuteScript("return window.jQuery == undefined");
                    if (jquery)
                    {
                        return true;
                    }
                    var ajaxIsComplete = (bool)(driver as IJavaScriptExecutor)
                        .ExecuteScript("return window.jQuery.active == 0");
                    if (ajaxIsComplete)
                    {
                        return true;
                    }
                    return false;
                }
                catch (InvalidOperationException e)
                {
                    //Window is no longer available
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (UnhandledAlertException)
                {
                    Debug.WriteLine("Unexpected Alert present.");
                    return true;
                }
                catch (WebDriverException e)
                {
                    //Browser is no longer available
                    return e.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public static Boolean IsAlertPresent(this IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void WaitUntilAlertIsPresent(this IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
        }

        /// <summary>
        /// Method to wait until a given element is present or a timeout is thrown.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool WaitUntilElementIsPresent(this IWebDriver driver, By by, int timeout = 15)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.ElementIsPresent(by));
        }
    }
}
