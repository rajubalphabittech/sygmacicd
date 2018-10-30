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

namespace atm.tests.unit.RouteManagerControllerTests
{
	[TestClass]
	public class IndexTests
	{
        private ICenterService _centerService;
        private Mock<ICenterService> _centerServiceMock;
        private Mock<HttpRequestBase> _requestMock;
        private Mock<HttpResponseBase> _responseMock;
        private Mock<HttpContextBase> _contextMock;

        [TestInitialize]
        public void Setup()
        {
            _requestMock = new Mock<HttpRequestBase>();
            _responseMock = new Mock<HttpResponseBase>();
            _contextMock = new Mock<HttpContextBase>();
            _contextMock.SetupGet(a => a.Request).Returns(_requestMock.Object);
            _contextMock.SetupGet(a => a.Response).Returns(_responseMock.Object);
            _contextMock.SetupGet(c => c.User.Identity.Name).Returns("na\\JSET0867");
        }

        [TestMethod]
        public async Task Index_ReturnsCorrectView()
        {
            var list = new List<BasicCenter>() { new BasicCenter { Center = "Columbus", SygmaCenterNo = 22 }, new BasicCenter { Center = "Denver", SygmaCenterNo = 8 } }.Cast<BasicCenter>().ToList();

            _centerServiceMock = new Mock<ICenterService>();
            _centerServiceMock.Setup(r => r.GetAll(It.IsAny<string>())).Returns(Task.FromResult(list));
            _centerServiceMock.Setup(r => r.GetLocationByNoAsync(5)).Throws(new KeyNotFoundException());
            _centerServiceMock.Setup(r => r.GetLocationByNoAsync(22)).Returns(Task.FromResult(new CenterLocation { SygmaCenterNo = 22, Description = "Columbus" }));
            _centerService = _centerServiceMock.Object;
            RouteManagerController controller = new RouteManagerController(null, _centerService, null, null)
            {
                ControllerContext = new ControllerContext() { HttpContext = _contextMock.Object }
            };

            ViewResult result = await controller.Index() as ViewResult;
            var model = result.ViewData.Model as RouteManagerViewModel;

            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));
            Assert.IsNotNull(model);
            Assert.IsTrue(model.CenterSelectList.Count() == 2);
        }
    }
}