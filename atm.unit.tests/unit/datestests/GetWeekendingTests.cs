using System;
using atm.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.tests.unit.datestests
{
	[TestClass]
	public class GetWeekendingTests : DateTestBase
	{
		[TestMethod]
		public void ValidWeekEndingFromStartOfWeek()
		{
			var date = new DateTime(2017, 1, 1);
			var expected = new DateTime(2017, 1, 7);

			DatesAreEqual(Dates.GetWeekending(date), expected);
		}

		[TestMethod]
		public void ValidWeekEndingFromMiddleOfWeek()
		{
			var date = new DateTime(2017, 1, 5);
			var expected = new DateTime(2017, 1, 7);

			DatesAreEqual(Dates.GetWeekending(date), expected);
		}

		[TestMethod]
		public void ValidWeekEndingFromEndOfWeek()
		{
			var date = new DateTime(2017, 1, 7);
			var expected = new DateTime(2017, 1, 7);

			DatesAreEqual(Dates.GetWeekending(date), expected);
		}
	}
}
