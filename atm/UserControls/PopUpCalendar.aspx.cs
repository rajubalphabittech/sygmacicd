using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SygmaFramework;

namespace SygmaInternet.UserControls
{
	/// <summary>
	/// Summary description for PopUpCalendar.
	/// </summary>
	public partial class PopUpCalendar : System.Web.UI.Page
	{
		private string gControl;
		private DateTime gCurDate;
		private int gYearsBack = 6;
		private bool gSubmitParent;
			
		protected void Page_Load(object sender, System.EventArgs e)
		{
			SetPageVariables();
			if (!IsPostBack){
				SetYears();
				SetVisibleDate();
			}
		}
		private void SetPageVariables(){
			gControl = Request.QueryString.Get("control");
			gCurDate = (Common.IsDate(Request.QueryString.Get("curdate")))?Convert.ToDateTime(Request.QueryString.Get("curdate")):DateTime.Today;
			gSubmitParent = Convert.ToBoolean(Request.QueryString.Get("soc"));
			if (Request.QueryString.Get("yearsback")!=null)
				YearsBack = Convert.ToInt32(Request.QueryString.Get("yearsback"));
			if (Request.QueryString.Get("hidemys")!=null)
				HideMonthYearSelect = Convert.ToBoolean(Request.QueryString.Get("hidemys"));
		}
		private void SetVisibleDate(){
			Calendar1.VisibleDate = gCurDate;
			SetMonthYear(gCurDate.Month, gCurDate.Year);
		}
		private void SetYears(){
			int curYear = DateTime.Today.Year;
			for (int i=0;i<gYearsBack;i++){
				int year = curYear-i;
				ddYear.Items.Add(year.ToString());
			}
		}
		private void SetMonthYear(int month, int year){
			ddMonth.ClearSelection();
			ddYear.ClearSelection();
			ListItem liMonth = ddMonth.Items.FindByValue(month.ToString());
			if (liMonth!=null)liMonth.Selected = true;;
			ListItem liYear = ddYear.Items.FindByValue(year.ToString());
			if (liYear!=null)liYear.Selected = true;;
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Calendar1.VisibleMonthChanged += new System.Web.UI.WebControls.MonthChangedEventHandler(this.Calendar1_VisibleMonthChanged);

		}
		#endregion

		protected void Calendar1_SelectionChanged(object sender, System.EventArgs e) {
			SetParentControl();
		}
		private void SetParentControl(){
			string selectDate = Calendar1.SelectedDate.ToShortDateString();
			string script = string.Format("SetDate('{0}','{1}',{2})", gControl, selectDate, gSubmitParent.ToString().ToLower());
			this.ClientScript.RegisterStartupScript(this.GetType(), "Script_SetDate", script, true);
		}
		private void Calendar1_VisibleMonthChanged(object sender, System.Web.UI.WebControls.MonthChangedEventArgs e) {
			SetMonthYear(e.NewDate.Month, e.NewDate.Year);
		}

		protected void ddMonth_SelectedIndexChanged(object sender, System.EventArgs e) {
			Calendar1.VisibleDate = new DateTime(Convert.ToInt32(ddYear.SelectedValue), Convert.ToInt32(ddMonth.SelectedValue), 1);
		}

		protected void ddYear_SelectedIndexChanged(object sender, System.EventArgs e) {
			Calendar1.VisibleDate = new DateTime(Convert.ToInt32(ddYear.SelectedValue), Convert.ToInt32(ddMonth.SelectedValue), 1);
		}
		private int YearsBack{
			get{return gYearsBack;}
			set{gYearsBack = value;}
		}
		private bool HideMonthYearSelect{
			get{return !trMonthYearSelect.Visible;}
			set{trMonthYearSelect.Visible = !value;}
		}
		protected void btnToday_Click(object sender, EventArgs e) {
			Calendar1.VisibleDate = DateTime.Now.Date;
		}
}
}
