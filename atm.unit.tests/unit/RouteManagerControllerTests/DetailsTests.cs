using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using atm.Controllers;
using atm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using atm.services;
using System.Web;
using atm.services.models;
using atm.Models;
using System.IO;
using Newtonsoft.Json;

namespace atm.tests.unit.RouteManagerControllerTests
{
	[TestClass]
	public class DetailsTests
	{
        private IRouteService _routeService;
        private Mock<IRouteService> _routeServiceMock;
        private Mock<HttpRequestBase> _requestMock;
        private Mock<HttpResponseBase> _responseMock;
        private Mock<HttpContextBase> _contextMock;
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

            _requestMock = new Mock<HttpRequestBase>();
            _responseMock = new Mock<HttpResponseBase>();
            _contextMock = new Mock<HttpContextBase>();
            _contextMock.SetupGet(a => a.Request).Returns(_requestMock.Object);
            _contextMock.SetupGet(a => a.Response).Returns(_responseMock.Object);
            _contextMock.SetupGet(c => c.User.Identity.Name).Returns("na\\JSET0867");
        }

        [TestMethod]
        public async Task Details_ReturnsCorrectResult()
        {
            var routeId = 100483;
            _routeServiceMock = new Mock<IRouteService>();
            _routeServiceMock.Setup(r => r.GetByRouteIdAndCenterNumberAndRouteNumberAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(_searchableRoutes.First(r => r.RouteId == routeId)));
            _routeService = _routeServiceMock.Object;
            RouteManagerController controller = new RouteManagerController(_routeService, null, null, null)
            {
                ControllerContext = new ControllerContext() { HttpContext = _contextMock.Object }
            };
            var result = await controller.Details(routeId, 22, "103T") as JsonResult;
            var model = JsonConvert.DeserializeObject<dynamic>(result.Data.ToString()); 

            Assert.IsNotNull(model);
            Assert.AreEqual(routeId.ToString(), ((Newtonsoft.Json.Linq.JValue)Newtonsoft.Json.Linq.JObject.Parse(result.Data.ToString())["routeId"]).Value);
        }
    }
}