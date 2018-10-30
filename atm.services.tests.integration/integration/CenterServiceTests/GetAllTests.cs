using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.services.tests.integration.CenterServiceTests
{
	[TestClass]
	public class GetAllTests : AtmContextBase
	{
		[TestMethod]
		public async Task ICanGetCenters()
		{
			var svc = new CenterService();
			var result = await svc.GetAll("jset0867");

			Assert.IsTrue(result.Count > 0);
		}

		[TestMethod]
		public async Task ICannotGetCenters()
		{
			var svc = new CenterService();
			var result = await svc.GetAll("notauser");

            if (result.Count > 0)
            {
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(99, result[0].SygmaCenterNo);
            } 
		}
	}
}
