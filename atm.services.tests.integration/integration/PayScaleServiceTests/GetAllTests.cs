using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.services.tests.integration.PayScaleServiceTests
{
	[TestClass]
	public class GetAllTests : AtmContextBase
	{
		[TestMethod]
		public async Task ICanGetCenters()
		{
			var svc = new PayScaleService();
			var result = await svc.GetAll();

			Assert.IsTrue(result.Count > 0);
		}
	}
}
