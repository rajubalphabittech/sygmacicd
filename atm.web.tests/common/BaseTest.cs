using atm.web.tests.pages;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace atm.web.tests.common
{
    public class BaseTest
    {
        private const string screenShotSuffix = ".png";

        public class ConstantsUtils
        {
            public static string PortNumber = "25812";
            public static string Url = $"http://localhost:{ PortNumber }";
            //public static string Url = $"http://ms084webqa";
            public static string ScreenShotLocation = "c:\\Temp\\";
        }

        public static void InitializeBrowserAndPages()
        {
            // start dotnet run and give it some time to init
            //StartStopWebApplication.StartIISExpress();
            Driver.StartBrowser(BrowserTypes.HeadlessChrome);
            // cache the pages so they can be reused
            Driver.Pages.Add("RouteManagerPage", new RouteManagerPage(Driver.Browser));
            Driver.Pages.Add("RouteTrackerPage", new RouteTrackerPage(Driver.Browser));
        }

        public static void TearDownTest()
        {
            Driver.StopBrowser();
            //StartStopWebApplication.StopIISExpress();
            Driver.Pages.Clear();
        }

        public static void TakeScreenshot()
        {
            try
            {
                string name = (ScenarioContext.Current.ScenarioInfo.Title).Replace(" ", "-");
                string cleanName = Regex.Replace(name, "[^A-Za-z]", "-");
                string filename = ConstantsUtils.ScreenShotLocation + cleanName + "_" + DateTime.Now.ToString("MM-dd-yyyy-hh-mmtt") + screenShotSuffix;
                Console.WriteLine("Take Screenshot - store in file: " + filename);
                Screenshot screenshot = ((ITakesScreenshot)Driver.Browser).GetScreenshot();
                screenshot.SaveAsFile(filename, ScreenshotImageFormat.Png);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to create screenshot");
                throw ex;
            }
        }
    }
}
