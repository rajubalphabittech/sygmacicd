using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using atm.web.tests.common;
using System.Diagnostics;

namespace atm.web.tests.pages
{
    [Binding]
    public sealed class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            BaseTest.InitializeBrowserAndPages();
            //If we need a test payroll form to be created in the db before running the scenario
            if (ScenarioContext.Current.ScenarioInfo.Tags.Contains("CreatePayrollForm"))
            {
                DbHelper.CreatePayrollForm(); //Create the payroll form
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                if (ScenarioContext.Current.TestError != null)
                {
                    BaseTest.TakeScreenshot(); //Create a screen shot
                }
                //If we need created a test payroll form then we need to delete it
                if (ScenarioContext.Current.ScenarioInfo.Tags.Contains("CreatePayrollForm"))
                {
                    DbHelper.DeletePayrollForm(); //Create the payroll form
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Encountered Error in AfterScenario method: {ex}");
            }
            finally
            {
                BaseTest.TearDownTest();
                //Kills any process that escaped the above TearDownTest
            }
            //DisposeDriverService.FinishHim();
        }



    }
}
