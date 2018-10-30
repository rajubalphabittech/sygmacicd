using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using atm.services.models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;

namespace atm.services.tests.unit
{
    [TestClass]
    public class RouteServiceTests
    {
        private IRouteService _routeService;
        private static string routeJson;
        private static List<Route> _searchableRoutes;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            using (StreamReader r = new StreamReader("../../TestData/center_22_routes_only.json"))
            {
                routeJson = r.ReadToEnd();
            }
        }

        [TestInitialize()]
        public void Initialize()
        {
            _searchableRoutes = JsonConvert.DeserializeObject<List<Route>>(routeJson);
        }

        [TestMethod]
        public async Task SearchAsync_WithCenterNumber_ReturnsRoutesFromCenter()
        {
            // arrange
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_searchableRoutes.Where(r => r.SygmaCenterNo == 22).Take(5)))
            };

            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var httpClient = new HttpClient(fakeHandler);
            _routeService = new RouteService(httpClient);

            // act
            var result = await _routeService.SearchAsync(new SearchRoute { CenterNumber = 22 });

            // asserts
            Assert.IsFalse(result.Any(r => r.SygmaCenterNo != 22));
        }

        [TestMethod]
        public async Task SearchAsync_WithCenterNumberBilltoShipTo_ReturnsRoutesFromCenterWithBillToShipTo()
        {
            // arrange
            var billTo = 2712m;
            var shipTo = 15m;
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_searchableRoutes.Where(r => r.SygmaCenterNo == 22 && r.RouteStops.Any(s => s.BillTo == billTo && s.ShipTo == shipTo)).Take(5)))
            };

            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var httpClient = new HttpClient(fakeHandler);
            _routeService = new RouteService(httpClient);

            // act
            var result = await _routeService.SearchAsync(new SearchRoute { CenterNumber = 22, BillTo = 2712, ShipTo = 15 });

            // asserts
            Assert.IsFalse(result.Any(r => r.SygmaCenterNo != 22));
            Assert.IsTrue(result.All(r => r.RouteStops.Any(s => s.BillTo == billTo && s.ShipTo == shipTo)));
        }

        [TestMethod]
        public async Task GetByRouteIdAsync_ValidRouteId_ReturnsRoute()
        {
            // arrange
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_searchableRoutes.Where(r => r.SygmaCenterNo == 22).Single(r => r.RouteId == 100483)))
            };

            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var httpClient = new HttpClient(fakeHandler);
            _routeService = new RouteService(httpClient);

            // act
            var result = await _routeService.GetByRouteIdAsync(100482);

            // asserts
            Assert.AreEqual(100483, result.RouteId);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.Http.HttpRequestException))]
        public async Task GetByRouteIdAsync_InvalidRouteId_Returns404()
        {
            // arrange
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound
            };

            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var httpClient = new HttpClient(fakeHandler);
            _routeService = new RouteService(httpClient);

            // act
            var result = await _routeService.GetByRouteIdAsync(999999);
        }

        [TestMethod]
        public async Task UpdateRouteAsync_ReturnsNothing()
        {
            // arrange
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };

            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var httpClient = new HttpClient(fakeHandler);
            _routeService = new RouteService(httpClient);

            // act
            await _routeService.UpdateRouteAsync("102T", 22, new UpdateRouteWithStops { });
        }

        [TestMethod]
        public async Task MoveStopAsync_ReturnsNothing()
        {
            // arrange
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };

            var fakeHandler = new FakeHttpMessageHandler(fakeResponse);
            var httpClient = new HttpClient(fakeHandler);
            _routeService = new RouteService(httpClient);

            // act
            await _routeService.MoveStopAsync(new MoveStopTestModel { });
        }

        private class MoveStopTestModel : IMoveStopModel
        {
            public int CenterNumber { get; set; }
            public DateTime DestinationDeliveryDateTime { get; set; }
            public int DestinationRouteId { get; set; }
            public string DestinationRouteNumber { get; set; }
            public int DestinationStopNumber { get; set; }
            public int SourceRouteId { get; set; }
            public string SourceRouteNumber { get; set; }
            public int SourceRoutePlanId { get; set; }
            public int SourceStopNumber { get; set; }
            public string StopModificationComment { get; set; }
            public string CreatedBy { get; set; }
        }
    }
}