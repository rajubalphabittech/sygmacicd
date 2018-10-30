using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SygmaFramework;

namespace atm
{

	public class DateRange
	{
		public DateTime Begin { get; set; }
		public DateTime End { get; set; }
	}

	public enum DateRanges
	{

		LastWeek,
		ThisPeriod,
		LastPeriod,
		LastMonth,
		Custom,
		ThisWeek
	}

	public class DateRangeCalculator
	{

		public static Dictionary<string, string> GetValidDateRanges()
		{
			return new Dictionary<string, string>
			{
				{"Current Period",  DateRanges.ThisPeriod.ToString()},
				{"Current Week",    DateRanges.ThisWeek.ToString() },
				{"Last Month",      DateRanges.LastMonth.ToString() },
				{"Last Week",       DateRanges.LastWeek.ToString()},
				{"Previous Period", DateRanges.LastPeriod.ToString()},
				{"Custom",          DateRanges.Custom.ToString() }
			};
		}

		public static DateRange Calculate(DateTime reference, string value)
		{
			var result = new DateRange();

			if (value == DateRanges.LastWeek.ToString())
			{
				result.Begin = reference.AddDays(-(int)reference.DayOfWeek - 7);
				result.End = result.Begin.AddDays(6);
				return result;
			}

			if (value == DateRanges.ThisWeek.ToString())
			{
				result.Begin = reference.AddDays(-(int)reference.DayOfWeek);
				result.End = result.Begin.AddDays(6);
				return result;
			}

			if (value == DateRanges.LastMonth.ToString())
			{
				var lastMonth = reference.AddMonths(-1);
				result.Begin = new DateTime(lastMonth.Year, lastMonth.Month, 1);
				result.End = result.Begin.AddMonths(1).AddDays(-1);
				return result;
			}

			if (value == DateRanges.ThisPeriod.ToString())
			{
				using (var db = new Database("Intranet"))
				{
					var dv = db.GetDataView("up_getPeriod");
					if (dv.Count <= 0) return null;

					result.Begin = Convert.ToDateTime(dv[0]["StartDate"]);
					result.End = Convert.ToDateTime(dv[0]["EndDate"]);

					return result;
				}
			}

			if (value == DateRanges.LastPeriod.ToString())
			{
				using (var db = new Database("Intranet"))
				{
					var dv = db.GetDataView("up_getPeriod");
					if (dv.Count <= 0) return null;

					var firstDayCurrentPeriod = Convert.ToDateTime(dv[0]["StartDate"]);
					var lastDayLastPeriod = firstDayCurrentPeriod.AddDays(-1);


					dv = db.GetDataView("up_getPeriod", lastDayLastPeriod);
					if (dv.Count <= 0) return null;

					result.Begin = Convert.ToDateTime(dv[0]["StartDate"]);
					result.End = Convert.ToDateTime(dv[0]["EndDate"]);

					return result;
				}
			}

			return null;
		}


	}
}