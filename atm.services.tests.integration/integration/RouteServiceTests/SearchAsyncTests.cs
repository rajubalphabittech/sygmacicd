using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using atm.services.models;

namespace atm.services.tests.integration.RouteServiceTests
{
    [TestClass]
    public class SearchAsyncTests : AtmContextBase
    {
        [TestMethod]
        public async Task SearchAsync_WithCenterNumberFilterDates_ReturnsRoutesFromCenter()
        {
            var svc = new RouteService();
            var result = await svc.SearchAsync(new SearchRoute { CenterNumber = 22, FilterStartDate = System.DateTime.Today.AddDays(-6), FilterEndDate = System.DateTime.Today.AddDays(-3) });

            Assert.IsTrue(result.Any(r => r.SygmaCenterNo == 22));
        }
        [TestMethod]
        public async Task SearchAsync_WithCenterNumberBilltoShipTo_ReturnsRoutesFromCenterWithBillToShipTo()
        {
            var billTo = 2712m;
            var shipTo = 15m;
            var svc = new RouteService();
            var result = await svc.SearchAsync(new SearchRoute { CenterNumber = 22, BillTo = 2712, ShipTo = 15, DeepSearch = true });

            Assert.IsTrue(result.All(r => r.SygmaCenterNo == 22 && r.RouteStops.Any(s => s.BillTo == billTo && s.ShipTo == shipTo)));
        }

    }
}