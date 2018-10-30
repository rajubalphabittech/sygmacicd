using FluentAssertions;

namespace atm.web.tests.pages
{
    public partial class ViewPayScalesPage
    {
        public void VerifyPayScaleRatesTableLoaded()
        {
            PayScaleRatesTable.Displayed.Should().BeTrue("because the Pay Scale Rates table should have been rendered.");
        }
    }
}
