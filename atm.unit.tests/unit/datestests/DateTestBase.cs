using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace atm.tests.unit.datestests
{
	public abstract class DateTestBase
	{
		public void DatesAreEqual(DateTime date, DateTime expected)
		{
			Assert.AreEqual(expected.Year, date.Year);
			Assert.AreEqual(expected.Month, date.Month);
			Assert.AreEqual(expected.Day, date.Day);
		}
	}
}