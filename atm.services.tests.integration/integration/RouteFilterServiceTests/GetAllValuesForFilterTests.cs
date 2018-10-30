using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using atm.services.models;

namespace atm.services.tests.integration.RouteFilterTests
{
    [TestClass]
    public class GetAllValuesForFilterTests : AtmContextBase
    {
        [TestMethod]
        public async Task ICanGetAllDropDownValuesForRouteNumbers()
        {
            var svc = new LookUpService();
            var result = await svc.GetAllRouteFilters((int)RouteFilterTypes.RouteNumbers);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public async Task ICanGetAllDropDownValuesForRouteStatus()
        {
            var svc = new LookUpService();
            var result = await svc.GetAllRouteFilters((int)RouteFilterTypes.RouteStatus);

            Assert.IsTrue(result.Count > 0);
        }
    }
}
