using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using atm.web.tests.pages;

namespace atm.web.tests.common
{
    public static class Driver
    {
        private static WebDriverWait browserWait;
        private static IWebDriver browser;
        private static Dictionary<string, BasePage> pages;
        private static string CurrentPage;

        public static IWebDriver Browser
        {
            get
            {
                if (browser == null)
                {
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method Start.");
                }
                return browser;
            }
            private set
            {
                browser = value;
            }
        }

        public static WebDriverWait BrowserWait
        {
            get
            {
                if (browserWait == null || browser == null)
                {
                    throw new NullReferenceException("The WebDriver browser wait instance was not initialized. You should first call the method Start.");
                }
                return browserWait;
            }
            private set
            {
                browserWait = value;
            }
        }

        public static void StartBrowser(BrowserTypes browserType = BrowserTypes.Chrome, int defaultTimeOut = 45)
        {
            //Save off start time so that we can use it later
            DisposeDriverService.TestRunStartTime = DateTime.Now;
            switch (browserType)
            {
                case BrowserTypes.Edge:
                    Browser = new EdgeDriver();
                    break;
                case BrowserTypes.Chrome:
                    Browser = new ChromeDriver();
                    break;
                case BrowserTypes.HeadlessChrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("headless");
                    chromeOptions.AddArguments("window-size=1920,1080");
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    Browser = new ChromeDriver(chromeDriverService, chromeOptions);
                    break;
                default:
                    Browser = new ChromeDriver();
                    break;
            }
            Browser.Manage().Window.Maximize();
            BrowserWait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(defaultTimeOut));
        }

        public static void StopBrowser()
        {
            Browser.Quit();
            Browser = null;
            BrowserWait = null;
        }

        public static Dictionary<string, BasePage> Pages
        {
            get
            {
                if (pages == null || pages.Count == 0)
                {
                    pages = new Dictionary<string, BasePage>();
                }
                return pages;
            }

            private set
            {
                pages = value;
            }
        }

        public static dynamic GetPage(string pageName)
        {
            pageName = pageName.Replace(" ", string.Empty); //Remove any spaces
            if (Driver.Pages.ContainsKey(pageName))
            {
                return Driver.Pages[pageName];
            }

            else
            {
                if (IsValidPageName(pageName))
                {
                    //If the PageName is valid, then instantiate the page and return it
                    try
                    {
                        Driver.Pages[pageName] = FindPageName(pageName);
                        return Driver.Pages[pageName];
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw new Exception($"{pageName} is not a valid page object name. Please add it to the Driver.GetValidPageNames method.");
                }
            }
        }

        private static List<string> GetValidPageNames()
        {
            //Add any newly created pages here
            List<string> ValidPages = new List<string>
            {
                "HomePage",
                "LoginPage",
                "ManageEmployeesPage",
                "ManageRoutesPage",
                "ManageVehiclesAndTrailersPage",
                "PayrollFormsPage",
                "PayrollFormPage",
                "UserMaintenancePage",
                "ViewEmployeePage",
                "ViewPayScalesPage",
                "RouteTrackerPage",
                "RouteManagerPage",
                "RouteNotificationPage"
            };
            return ValidPages;
        }

        private static bool IsValidPageName(string pageName)
        {
            return GetValidPageNames().Contains(pageName);
        }

        private static BasePage FindPageName(string PageName)
        {
            Type t = Type.GetType($"atm.web.tests.pages.{PageName}");
            dynamic pageObject = Activator.CreateInstance(t, Browser);
            return pageObject;
        }

        public static dynamic GetCurrentPage()
        {
            var CurrentPageName = GetPageNameFromUrl();
            CurrentPage = CurrentPageName;
            return GetPage(CurrentPageName);
        }

        public static string GetPageNameFromUrl()
        {
            var PageName = "";
            //Remove the atm version part of the title
            var formattedTitle = Browser.Title.Split('(')[0].Trim();
            switch (formattedTitle)
            {
                case "ATM - Manage Employees":
                    PageName = "ManageEmployeesPage";
                    break;
                case "ATM - View Employees":
                    PageName = "ViewEmployeesPage";
                    break;
                case "ATM - Manage Routes":
                    PageName = "ManageRoutesPage";
                    break;
                case "ATM - Manage Vehicles & Trailers":
                    PageName = "ManageVehiclesAndTrailersPage";
                    break;
                case "ATM - Payroll Form":
                    PageName = "PayrollFormPage";
                    break;
                case "ATM - Payroll Forms":
                    PageName = "PayrollFormsPage";
                    break;
                case "ATM - User Maintenance":
                    PageName = "UserMaintenancePage";
                    break;
                case "ATM - View Pay Rates":
                    PageName = "ViewPayScalesPage";
                    break;
                case "ATM - Route Manager":
                    PageName = "RouteManagerPage";
                    break;
                case "ATM - Route Tracker":
                    PageName = "RouteTrackerPage";
                    break;
                case "ATM - Route Notification":
                    PageName = "RouteNotificationPage";
                    break;
                default:
                    throw new Exception($"Please add {PageName} to the GetPageNameFromUrl method in the Driver.cs");
            }
            return PageName;
        }


    }
}
