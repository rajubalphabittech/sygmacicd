using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UserControls_SelectWeek : System.Web.UI.UserControl {
	
	private const int WEEK_LENGTH = 7;

	protected void Page_Load(object sender, EventArgs e) {
				
	}


	public DateTime SelectedDate {
		get { return calWeek.SelectedDates[(int)WeekEndDay].Date; }
	}
	public void SelectWeek() {
		SelectWeek(DateTime.Now);
	}
	public void SelectWeek(DateTime curDay) {
		while (curDay.DayOfWeek != WeekEndDay) {
			curDay = curDay.AddDays(1);
		}
		for (int i = 0; i < WEEK_LENGTH; i++) {
			calWeek.SelectedDates.Add(curDay);
			curDay = curDay.AddDays(-1);
		}
		calWeek.VisibleDate = curDay;
	}

	public Unit Width {
		get { return calWeek.Width; }
		set { calWeek.Width = value; }
	}

	public Unit Height {
		get { return calWeek.Height; }
		set { calWeek.Height = value; }
	}


	private DayOfWeek gWeekEndDay = DayOfWeek.Saturday;

	public DayOfWeek WeekEndDay {
		get { return gWeekEndDay; }
		set { gWeekEndDay = value; }
	}
	
	public TableItemStyle TodayDayStyle {
		get { return calWeek.TodayDayStyle; }
	}

	public TableItemStyle SelectorStyle {
		get { return calWeek.SelectorStyle; }
	}

	public TableItemStyle SelectedDayStyle {
		get { return calWeek.SelectedDayStyle; }
	}

	public TableItemStyle TitleStyle {
		get { return calWeek.TitleStyle; }
	}

	public event EventHandler SelectionChanged;
	protected void calWeek_SelectionChanged(object sender, EventArgs e) {
		if (SelectionChanged != null)
			SelectionChanged(sender, e);
	}
	protected void calWeek_DayRender(object sender, DayRenderEventArgs args) {
		args.Day.IsSelectable = false;
	}
}
