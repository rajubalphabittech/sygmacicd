using System;
using System.Web.Configuration;

namespace atm.Helpers
{
	public class Dates
	{
		public Dates() { }
		public static DateTime GetWeekending(DateTime dt)
		{
			if (dt.DayOfWeek != DayOfWeek.Saturday)
				return dt.AddDays(6 - (int)dt.DayOfWeek);
			return dt;
		}

		public static DateTime GetWeekending()
		{
			return GetWeekending(DateTime.Today);
		}

		public static DateTime GetPreviousSunday(DateTime date)
		{
			if (date.DayOfWeek == DayOfWeek.Sunday) return date.AddDays(-7).Date;

			int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
			return date.AddDays(-1 * diff).Date;
		}

		public static DateTime GetPreviousSunday()
		{
			return GetPreviousSunday(DateTime.Today);
		}
	}
}