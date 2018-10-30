using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm.services.tests.integration.PayScaleServiceTests
{
    [TestClass]
    public class GetRatesForCenterAndPayScaleTests : AtmContextBase
    {
        [TestMethod]
        public async Task ICanGetPayScaleRates()
        {
            var svc = new PayScaleService();
            var result = await svc.GetRatesForCenterAndPayScale(22, 11);

            Assert.IsTrue(result.Count > 0);
        }
    }
}
