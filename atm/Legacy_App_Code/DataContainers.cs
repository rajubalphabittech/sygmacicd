using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using SygmaIntranet.Reports.Cadec.Fix_Lat_Longs;

/// <summary>
/// Summary description for DataContainers
/// </summary>
namespace DataContainers {

	public class GroupFileList {
		private string gHeader;
		private DataView gDataSource;
		public GroupFileList(string header, DataView dataSource) {
			gHeader = header;
			gDataSource = dataSource;
		}
		public string Header {
			get { return gHeader; }
		}
		public DataView DataSource {
			get { return gDataSource; }
		}
	}

	#region Absenteeism Containers

	public class CenterData {
		string gCenter;
		double gYTDScore;
		List<PeriodData> gPeriodData;
		public CenterData(string center, double ytdScore, List<PeriodData> periodData) {
			gCenter = center;
			gYTDScore = ytdScore;
			gPeriodData = periodData;
		}
		public string Center {
			get { return gCenter; }
		}
		public double YTDScore {
			get { return gYTDScore; }
		}
		public List<PeriodData> PeriodData {
			get { return gPeriodData; }
		}
	}
	public class PeriodData {
		double gPeriodScore;
		DataView gWeekData;
		public PeriodData(double periodScore, DataView weekData) {
			gPeriodScore = periodScore;
			gWeekData = weekData;
		}
		public double PeriodScore {
			get { return gPeriodScore; }
		}
		public DataView WeekData {
			get { return gWeekData; }
		}
	}
	public class PeriodWeeks {
		int gPeriod;
		List<string> gWeeks;
		public PeriodWeeks(int period, List<string> weeks) {
			gPeriod = period;
			gWeeks = weeks;
		}
		public int Period {
			get { return gPeriod; }
		}
		public List<string> Weeks {
			get { return gWeeks; }
		}
	}

	#endregion

	#region Cadec Containers

	[Serializable]
	public struct SearchCriteria {
		private SearchType gST;
		private string[] gParms;
		public SearchCriteria(SearchType type, params string[] parms) {
			gST = type;
			gParms = parms;
		}
		public SearchType Type {
			get { return gST; }
		}
		public string[] Parameters {
			get { return gParms; }
		}
	}

	#endregion
}
