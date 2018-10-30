using System;
using System.Diagnostics;
using System.Threading.Tasks;
using atm.services.models.payroll;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.services.tests.integration.driverservicetests
{
	[TestClass]
    public class GetFormsAsyncTests : AtmContextBase
    {
        [TestMethod]
        public async Task CanGetPayrollForms()
        {
            var svc = new DriverService();

            var request = new FormsQueryRequest
            {
                UserName = "swal0197",
                WeekEnding = new DateTime(2017, 1, 1),
                SygmaCenterNo = 22
            };

            var result = await svc.GetFormsAsync(request);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CanGetDrivers()
        {
            var svc = new DriverService();
            
            var result = await svc.GetDriversAsync();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CanGetFormCriteria()
        {
            var svc = new DriverService();
            var result = await svc.GetFormCriteriaAsync("swal0197");

            foreach (var item in result.Status)
            {
                Debug.WriteLine(item.StatusDescription);
            }

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CanGetDriverHelpers()
        {
            var svc = new DriverService();

            var result = await svc.GetDriverHelpersAsync();

            Assert.IsNotNull(result);
        }
    }
}