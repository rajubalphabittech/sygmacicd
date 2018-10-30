using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.services.tests.integration.authorizationservicetests
{
    [TestClass]
    public class IsValidUserTests : AtmContextBase
    {
        [TestMethod]
        public void IsAuthorizedForRoute()
        {
            var svc = new AuthorizationService();
            var result = svc.IsValiduser("jset0867", "~/Apps/ATM/Payroll/Forms/Index.aspx");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNOTAuthorizedForRoute()
        {
            var svc = new AuthorizationService();
            var result = svc.IsValiduser("notauser", "~/Apps/ATM/Payroll/Forms/Index.aspx");

            Assert.IsFalse(result);
        }
    }
}