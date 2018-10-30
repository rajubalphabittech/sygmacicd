using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Safelite.Commercial.SelfService.Web.Specs.Tests.Common
{
	public class ConstantsUtils
	{
		public static string PortNumber = "5000";
		public static string Url = $"http://localhost:{ PortNumber }/home";
		public static string ScreenShotLocation = "c:\\temp\\";
		public static string WebProjectName = "Safelite.Commercial.SelfService.Web";
	}

	public class Browser : IDisposable
	{
		private const string screenShotSuffix = ".png";
		private static IWebDriver webDriver;
		private static TimeSpan timeSpan = TimeSpan.FromSeconds(15);
		private static Process _iisExpressProcess;

		public Browser()
		{
			StartIISExpress();

			webDriver = new ChromeDriver();
			//Maximizing window to ensure all fields are rendered within a responsive webpage.
			webDriver.Manage().Window.Maximize();
			webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
		}

		public IWebDriver WebDriver
		{
			get { return webDriver; }
		}

		public string PageSource
		{
			get { return webDriver.PageSource; }
		}

		private static void StartIISExpress()
		{
			var applicationPath = GetApplicationPath(ConstantsUtils.WebProjectName);
			var arguments = string.Format(CultureInfo.InvariantCulture, "/path:\"{0}\" /port:{1}", applicationPath, ConstantsUtils.PortNumber);
			var startInfo = new ProcessStartInfo("dotnet.exe ")
			{
				WorkingDirectory = applicationPath,
				WindowStyle = ProcessWindowStyle.Hidden,
				ErrorDialog = true,
				LoadUserProfile = true,
				CreateNoWindow = true,
				UseShellExecute = false,
				Arguments = " run"
			};

			_iisExpressProcess = Process.Start(startInfo);
		}

		private static void StopIISExpress()
		{
			IEnumerable<Process> processes = Process.GetProcesses().Where(p => p.ProcessName == "dotnet" && p.HasExited == false).AsEnumerable();

			foreach (Process process in processes) process.Kill();

			if (_iisExpressProcess != null)
			{
				if (!_iisExpressProcess.HasExited)
				{
					_iisExpressProcess.Kill();
					_iisExpressProcess.Dispose();
				}
				_iisExpressProcess = null;
			}
		}

		private static string GetApplicationPath(string applicationName)
		{
			var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
			return Path.Combine(solutionFolder, "src", applicationName);
		}

		/// <summary>
		/// Reads from the App.config file the value of the specified parameter key.
		/// The value is splitted (separater is ':'). The first part contains the 
		/// locator type (e.g. xpath or id) and the second part contains the value of the locator.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private By GetBy(string key)
		{
			string element = ConfigurationManager.AppSettings[key];
			By by = null;
			if (element != null)
			{
				string[] elements = element.Split(':');
				switch (elements[0].ToLower())
				{
					case "xpath":
						by = By.XPath(elements[1]);
						break;
					case "tagname":
						by = By.TagName(elements[1]);
						break;
					case "name":
						by = By.Name(elements[1]);
						break;
					case "cssselector":
						by = By.CssSelector(elements[1]);
						break;
					default:
						by = By.Id(elements[1]);
						break;
				}
			}
			return by;
		}

		public void TakeScreenshot(string name)
		{
			string filename = ConstantsUtils.ScreenShotLocation + name + "_" + DateTime.Now.Ticks + screenShotSuffix;
			Console.WriteLine("Take Screenshot - store in file: " + filename);
			Screenshot screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
			screenshot.SaveAsFile(filename, ScreenshotImageFormat.Png);
		}

		public void GoTo(string url)
		{
			webDriver.Url = url;
		}

		public void EnterValueInField(string value, string field)
		{
			By by = GetBy(field);
			if (by != null)
			{
				IWebElement e = webDriver.FindElement(by);
				e.Click();
				e.Clear();
				e.SendKeys(value);
			}
		}

		public void ClickButton(string buttonName)
		{
			By by = GetBy(buttonName);
			if (by != null)
			{
				webDriver.FindElement(by).Click();
				System.Threading.Thread.Sleep(1000);
			}
		}

		public void ClickLink(string linkText)
		{
			webDriver.FindElement(By.LinkText(linkText)).Click();
		}

		public void Dispose()
		{
			webDriver.Quit();
			StopIISExpress();
		}
	}
}
