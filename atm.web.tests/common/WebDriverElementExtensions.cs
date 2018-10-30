using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace atm.web.tests.common
{
    public static class WebDriverElementExtensions
    {
        /// <summary>
        /// Extension to check if an element can be found by given locator
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool ElementIsPresent(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Extension to robustly search for an IWebElement
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        /// <summary>
        /// Extension to robustly search for IWebElements
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
            }
            return driver.FindElements(by);
        }

        /// <summary>
        /// Extension to enter clear and then text into a textbox
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        /// <param name="elementName"></param>
        /// <param name="clearField"></param>
        public static void EnterText(this IWebElement element, string text, string elementName, bool clearField = true)
        {
            //Some situations we can't or don't want to call element.clear
            if (clearField)
            {
                element.Clear();
            }
            element.SendKeys(text);
            Driver.Browser.RobustWait();
            Debug.WriteLine($"'{text}' entered into the {elementName} field.");
        }

        /// <summary>
        /// Used to set the text of a field using JS diectly and therefore control when onblur events fire. 
        /// Please use EnterText method unless the JS on your page won't you to enter text as expected.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text">Text to enter</param>
        /// <param name="elementName">Name of the element</param>
        public static void EnterTextWithJS(this IWebElement element, string text, string elementName)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)Driver.Browser;
            //Set the value for desired element and then trigger a change event
            jse.ExecuteScript($"arguments[0].value='{text}';$(arguments[0]).trigger('change');", element);
            Driver.Browser.RobustWait();
            Debug.WriteLine($"'{text}' entered into the {elementName} field using JavaScript.");
        }

        /// <summary>
        /// Will Scroll to a given element and try to click it.
        /// </summary>
        /// <param name="element"></param>
        public static void ScrollToAndClick(this IWebElement element)
        {
            String scrollElementIntoMiddle = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                                            + "var elementTop = arguments[0].getBoundingClientRect().top;"
                                            + "window.scrollBy(0, elementTop-(viewPortHeight/2));";
            ((IJavaScriptExecutor)Driver.Browser).ExecuteScript(scrollElementIntoMiddle, element);

            //This eliminates errors when an element is not in view or is infront of our element
            element.Click();
            Driver.Browser.RobustWait();
        }

        public static void DoubleClick(this IWebElement element)
        {
            new Actions(Driver.Browser).DoubleClick(element).Perform();
        }

        /// <summary>
        /// Right clicks on a given IWebElement
        /// </summary>
        /// <param name="element"></param>
        public static void RightClick(this IWebElement element, IWebDriver driver)
        {
            Actions RClick = new Actions(driver);
            RClick.ContextClick(element).Perform();
        }

        public static void BootstrapMultiSelectByText(this IWebElement element, string optionTextToSelect, string elementName)
        {
            try
            {
                //Find the button to open the Bootstrap Multiselect
                IWebElement selectListButton = element.FindElement(By.XPath(".//*[@class='multiselect-native-select']/div/button"));
                selectListButton.Click(); //Click that button
                Debug.WriteLine($"Clicked on the button to expand the {elementName} Bootstrap Multi Select");
            }
            catch (WebDriverException)
            {
                throw new Exception($"Unable to locate the button to expand the Bootstrap Multi Select {elementName}. Please verify your ID used is for the container div and not the select list.");
            }
            //Finds all the list items within the dropdown
            IList<IWebElement> selectListOptions = element.FindElement(By.ClassName("dropdown-menu")).FindElements(By.ClassName("checkbox"));
            //Find the select list option which contains optionTextToSelect
            IWebElement targetOption = selectListOptions.First(opt => opt.Text == optionTextToSelect);
            targetOption.Click(); //Click on that option
            targetOption.SendKeys(Keys.Tab); //Close the bootstrap multi select
            Driver.Browser.RobustWait(); //Wait for the page to load
            Debug.WriteLine($"Selected {optionTextToSelect} from the {elementName} Bootstrap Multi Select List");
        }

        /// <summary>
        /// Extension to select value by its' Value from a select list. 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        /// <param name="elementName"></param>
        public static void SelectByValue(this IWebElement element, string text, string elementName)
        {
            SelectElement oSelect = new SelectElement(element);
            oSelect.SelectByValue(text);
            Driver.Browser.RobustWait();
            oSelect.SelectedOption.ToString();

            Debug.WriteLine(oSelect.SelectedOption.ToString() + " value selected on " + elementName + " select list.");
        }

        /// <summary>
        /// Extension to select value by its' Text from a select list.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        /// <param name="elementName"></param>
        public static void SelectByText(this IWebElement element, string text, string elementName)
        {
            if (element.GetAttribute("class").Contains("multiselect-ui"))
            {
                throw new WebDriverException($"The {elementName} is a Bootstrap MultiSelect List. Please use BootstrapMultiSelectByText method instead.");
            }
            SelectElement oSelect = new SelectElement(element);
            oSelect.SelectByText(text);
            Driver.Browser.RobustWait();
            Debug.WriteLine($"'{text}' selected in the {elementName} select list.");
        }

        /// <summary>
        /// Extension to select value by its' Index from a select list.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="index"></param>
        /// <param name="elementName">""</param>
        public static void SelectByIndex(this IWebElement element, int index, string elementName)
        {
            SelectElement oSelect = new SelectElement(element);
            oSelect.SelectByIndex(index);
            Driver.Browser.RobustWait();
            //Debug.WriteLine($"Index '{index}' selected in the {elementName} select list.");
        }

        public static string SelectByRandomIndex(this IWebElement element, string elementName)
        {
            Random rnd = new Random();
            var options = element.GetOptions();
            //If first option in Select One, then remove that option
            var startingIndex = 0;
            if (options.ElementAt(0).GetText().Contains("Select"))
            {
                startingIndex = 1; //If first element is select one then skip it when picking a random option
            }
            //Pick a option from the select list
            var randomIndex = rnd.Next(startingIndex, options.Count() - 1);
            var valueToSelect = options.ElementAt(randomIndex).GetAttribute("value"); //get the value of the option we want
            element.SelectByIndex(randomIndex, elementName); //Select that index
            return valueToSelect; //Return the value selected
        }

        /// <summary>
        /// Returns a list of IWebElements which are Displayed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchContext"></param>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static IList<IWebElement> FindDisplayedElements<T>(this T searchContext, By locator) where T : ISearchContext
        {
            IList<IWebElement> elements = searchContext.FindElements(locator);
            return elements.Where(e => e.Displayed).ToList();
        }

        /// <summary>
        /// Retrieves the Value attriute from an IWebElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetText(this IWebElement element)
        {
            var result = element.GetAttribute("value");
            //If the Value attribute is null then use the Text method
            result = result ?? element.Text;
            return result;
        }

        /// <summary>
        /// Checks a checkbox if it isn't already checked
        /// </summary>
        /// <param name="element"></param>
        public static void Check(this IWebElement element)
        {
            if (!element.Selected)
            {
                element.Click();
                Driver.Browser.RobustWait();
            }
        }

        /// <summary>
        /// UnChecks a checkbox unless is is already unchecked
        /// </summary>
        /// <param name="element"></param>
        public static void UnCheck(this IWebElement element)
        {
            if (element.Selected)
            {
                element.Click();
                Driver.Browser.RobustWait();
            }
        }

        /// <summary>
        /// Returns all options for a given select list
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IList<IWebElement> GetOptions(this IWebElement element)
        {
            SelectElement oSelect = new SelectElement(element);
            return oSelect.Options;
        }

        /// <summary>
        /// Returns all an IList of the text from each options for a given select list
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IList<string> GetOptionsText(this IWebElement element)
        {
            IList<IWebElement> OptionsIWebElements = element.GetOptions(); //Retrieve all Option IWebElements for select list
            IList<string> selectedColumnNames = new List<string>();
            for (var option = 0; option < OptionsIWebElements.Count; option++)
            {
                selectedColumnNames.Add(OptionsIWebElements.ElementAt(option).Text);
            }
            return selectedColumnNames;
        }

        /// <summary>
        /// Returns all Selected Options for a given multi select list
        /// </summary>
        /// <param name="element"></param>
        /// <returns>a list containing a WebElement for each selected option</returns>
        public static IList<IWebElement> GetSelectedOptions(this IWebElement element)
        {
            SelectElement oSelect = new SelectElement(element);
            return oSelect.AllSelectedOptions;
        }

        /// <summary>
        /// Returns a list of the text for all the selected elements in a given selct list
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IList<string> GetSelectedOptionsText(this IWebElement element)
        {
            IList<IWebElement> selectedColumnIWebElements = element.GetSelectedOptions(); //Retrieve the selected Options IWebElements
            IList<string> selectedColumnNames = new List<string>();
            for (var option = 0; option < selectedColumnIWebElements.Count; option++)
            {
                selectedColumnNames.Add(selectedColumnIWebElements.ElementAt(option).Text);
            }
            return selectedColumnNames;
        }

        /// <summary>
        /// Returns the text value of the selected options in a dropdown list
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetSelectedOptionText(this IWebElement element)
        {
            return element.GetSelectedOptions().FirstOrDefault().Text;
        }

        public static void DoubleLeftClickOnSelectListOption(this IWebElement element, string optionText)
        {
            Actions doubleClickAction = new Actions(Driver.Browser);
            IList<IWebElement> options = element.GetOptions();
            IEnumerable<IWebElement> targetOptions = options.Where(opt => opt.Text == optionText); //Find options with the given option text
            if (targetOptions.Count() == 0) //Throw an error if you don't find the option
            {
                throw new Exception($"Expected to find option: {optionText} within the select list.");
            }
            IWebElement targetOption = targetOptions.First(); //Take the first in the list b/c it's our target option
            targetOption.DoubleClick();
        }

        /// <summary>
        /// Checks in the given element has a data attribute of 'readonly' set to true
        /// </summary>
        /// <param name="element"></param>
        /// <returns>bool value</returns>
        public static bool IsReadOnly(this IWebElement element)
        {
            return element.GetAttribute("readonly") == "true" ? true : false;
        }

        /// <summary>
        /// Retrieves the tooltip for the given item and returns empty string if no tooltip is found
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetToolTip(this IWebElement element)
        {
            return element.GetAttribute("title");
        }
    }
}

