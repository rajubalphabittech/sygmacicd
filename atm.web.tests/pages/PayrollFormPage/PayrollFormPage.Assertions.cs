using FluentAssertions;

namespace atm.web.tests.pages
{
    public partial class PayrollFormPage
    {
        public void VerifyPayrollFormCreatedForGivenRoute(string routeNumber, string departDate)
        {
            RouteNumberLabel.Text.Should().ContainEquivalentOf(routeNumber);
            DepartDateLabel.Text.Should().ContainEquivalentOf(departDate);
        }
    }
}
