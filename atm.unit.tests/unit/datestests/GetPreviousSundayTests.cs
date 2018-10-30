using System;
using atm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.tests.unit.datestests
{
	[TestClass]
	public class GetPreviousSundayTests : DateTestBase
	{
		[TestMethod]
		public void WhenSendingADateReturnsTheCorrectDate()
		{
			var date = new DateTime(2018, 1, 31);
			var expected = new DateTime(2018, 1, 28);

			DatesAreEqual(Dates.GetPreviousSunday(date), expected);
		}

		[TestMethod]
		public void WhenTheDateIsSundayReturnsPreviousSunday()
		{
			var date = new DateTime(2018, 1, 28);
			var expected = new DateTime(2018, 1, 21);

			DatesAreEqual(Dates.GetPreviousSunday(date), expected);
		}
	}
}