using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.services.tests.integration.authorizationservicetests
{

    [TestClass]
    public class IsApproverTests : AtmContextBase
    {
        [TestMethod]
        public void IsValidApprover()
        {
            var svc = new AuthorizationService();
            try
            {
                svc.IsApprover("jset0867");
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected Exception: " + e.ToString());
            }
        }

        [TestMethod]
        public void IsNOtValidApprover()
        {
            var svc = new AuthorizationService();
            var result = svc.IsApprover("notauser");

            Assert.IsFalse(result);
        }
    }
}