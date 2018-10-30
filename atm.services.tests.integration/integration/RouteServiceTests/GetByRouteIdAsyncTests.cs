using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using atm.services.models;


namespace atm.services.tests.integration.RouteServiceTests
{
    [TestClass]
    public class GetByRouteIdAsyncTests : AtmContextBase
    {
        [TestMethod]
        public async Task GetRoute_WithValidRouteId_ReturnsRoute()
        {
            var svc = new RouteService();
            var routes = await svc.SearchAsync(new SearchRoute { CenterNumber = 22, FilterStartDate = System.DateTime.Today.AddDays(-6), FilterEndDate = System.DateTime.Today.AddDays(-3) });

            var result = await svc.GetByRouteIdAsync(routes.First().RouteId);

            Assert.AreEqual(routes.First().RouteId, result.RouteId);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.Http.HttpRequestException))]
        public async Task GetRoute_WithInvalidRouteId_Returns404()
        {
            var svc = new RouteService();
            var result = await svc.GetByRouteIdAsync(999999);
        }
    }
}
