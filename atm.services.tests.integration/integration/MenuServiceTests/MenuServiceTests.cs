using Microsoft.VisualStudio.TestTools.UnitTesting;
using SygmaFramework;

namespace atm.services.tests.integration.menuservicetests
{
    [TestClass]
    public class GetByUserNameTests : AtmContextBase
    {
        [TestMethod]
        public void ICanGetAMenu()
        {
            var svc = new MenuService();
            var result = svc.GetByUserName("jset0867");

            Assert.IsTrue(result.Sections.Count > 0);
        }

        [TestMethod]
        public void ICannotGetAMenu()
        {
            var svc = new MenuService();
            var result = svc.GetByUserName("notauser");

            Assert.AreEqual(0, result.Sections.Count);
        }

        [TestMethod]
        public void ICanGetTheFirstPageURLAUserHasAccessTo()
        {
            var svc = new MenuService();
            var result = svc.GetFirstPageUrlForUserName("jset0867");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ICannotGetTheFirstPageURLAUserHasAccessTo()
        {
            var svc = new MenuService();
            var result = svc.GetFirstPageUrlForUserName("foo");

            Assert.AreEqual("", result);
        }
    }
}
