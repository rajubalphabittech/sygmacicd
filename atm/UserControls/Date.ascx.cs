

using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using SygmaFramework;
using Sygmanet;
using System.Web.UI;

public partial class UserControls_Date : System.Web.UI.UserControl {
	private const string DEFAULT_DATE_NAME = "Date";
	private const LabelPosition DEFAULT_NAME_POSITION = LabelPosition.Left;

	protected string AppPath;

	#region Event Handlers

	protected void Page_Load(object sender, System.EventArgs e) {
		AppPath = Application["AppPath"].ToString(); //((BasePage)Page).AppPath;
		SetValidDate();
		//put this in Page_Load b/c you have to get the value of the ControlToValidate from the postback before 
		SetCompareValidator();
		SetJavascript();
	}
	protected void Page_PreRender(object sender, System.EventArgs e) {
		//SetCalenderURL();
		SetDateName();
		SetValidation();
	}

	#endregion

	#region Private Members

	#region Methods
	private void SetValidDate() {
		if (IsValidDate)
			txtDate.Text = Convert.ToDateTime(ValueString).ToShortDateString();
	}
	//private void SetCalenderURL() {
	//  string script = string.Format("javascript:openDatePickerWindow('{0}', {1});", txtDate.ClientID, txtDate.AutoPostBack.ToString().ToLower());
	//  //hlCalender.NavigateUrl = script;
	//}
	private void SetDateName() {
		
		switch (NamePosition) {
			case LabelPosition.Top:
				lblName.Text = Name;
				pnlName.Visible = ShowName;
				pnlName.CssClass = "";
				pnlDate.CssClass = "";
				txtDate_TextBoxWatermarkExtender.Enabled = false;
				break;
			case LabelPosition.Left:
				lblName.Text = Name;
				pnlName.Visible = ShowName;
				pnlName.CssClass = "inlineBlock";
				pnlDate.CssClass = "inlineBlock";
				txtDate_TextBoxWatermarkExtender.Enabled = false;
				break;
			case LabelPosition.InBox:
				pnlName.Visible = false;
				txtDate_TextBoxWatermarkExtender.Enabled = true;
				txtDate_TextBoxWatermarkExtender.WatermarkText = Name;
				txtDate.ToolTip = Name;
				break;
		}

		//HtmlTableCell activeCell = null;
		//switch (NamePosition) {
		//  case LabelPosition.Left:
		//    activeCell = tblMainTable.Rows[1].Cells[0];
		//    activeCell.Visible = true;
		//    break;
		//  default:
		//    HtmlTableRow topNameRow = tblMainTable.Rows[0];
		//    topNameRow.Visible = true;
		//    activeCell = topNameRow.Cells[0];
		//    break;
		//}
		//activeCell.InnerText = Name;
		//if (NameStyle != "")
		//  activeCell.Attributes.Add("style", NameStyle);
		//if (NameClass != "")
		//  activeCell.Attributes.Add("class", NameClass);

	}
	private void SetValidation() {
		string displayName = (Name != "") ? string.Format("'{0}'", Name) : "Date";
		rfvDate.ErrorMessage = string.Format("{0} is required!", displayName);

		if (UseDBDates) {
			rngDBDate.Enabled = true;
			rngDBDate.ErrorMessage = string.Format("{0} value must be between {1} and {2}.", displayName, rngDBDate.MinimumValue, rngDBDate.MaximumValue);
		} else {
			cmpIsDate.Enabled = true;
			cmpIsDate.ErrorMessage = string.Format("{0} is an invalid date.", displayName);
		}
		if (this.Parent.FindControl("ValidationSummary1") != null || UseValidationSummary) {
			rfvDate.Text = "*";

			if (UseDBDates)
				rngDBDate.Text = "*";
			else
				cmpIsDate.Text = "*";

			if (cmpDates.Enabled)
				cmpDates.Text = "*";
		}
	}
	private void SetCompareValidator() {
		//if the control is not empty and compare validator isn't datatypecheck
		if (DateControlToCompare != "" && CompareOperator != ValidationCompareOperator.DataTypeCheck) {
			UserControls_Date compareControl = (UserControls_Date)this.Parent.FindControl(DateControlToCompare);
			string date = compareControl.ValueString;
			if (Common.IsDate(this.ValueString) && Common.IsDate(date) && EnableCompareDates) {
				cmpDates.Enabled = true;
				cmpDates.ValueToCompare = Convert.ToDateTime(date).ToShortDateString();
				cmpDates.Operator = CompareOperator;
				cmpDates.ErrorMessage = string.Format("'{0}' date must be {1} '{2}' date.", this.Name, GetCompareOperatorName(), compareControl.Name);
			} else {
				cmpDates.Enabled = false;
				cmpDates.ValueToCompare = DateTime.MinValue.ToShortDateString();
			}
		} else {
			cmpDates.Enabled = false;
			cmpDates.ValueToCompare = DateTime.MinValue.ToShortDateString();
		}
	}
	private string GetCompareOperatorName() {
		string retVal = "";
		switch (CompareOperator) {
			case ValidationCompareOperator.Equal:
				retVal = "equal to";
				break;
			case ValidationCompareOperator.GreaterThan:
				retVal = "greater than";
				break;
			case ValidationCompareOperator.GreaterThanEqual:
				retVal = "greater than or equal to";
				break;
			case ValidationCompareOperator.LessThan:
				retVal = "less than";
				break;
			case ValidationCompareOperator.LessThanEqual:
				retVal = "less than or equal to";
				break;
			case ValidationCompareOperator.NotEqual:
				retVal = "not equal to";
				break;
		}
		return retVal;
	}
	private void SetJavascript() {
		string scriptKey = "OPEN_DATE_PICKER";
		if (!Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType(), scriptKey)) {
			StringBuilder sb = new StringBuilder();
			sb.Append("function openDatePickerWindow(fieldName, setAutoPostBack) {");
			sb.AppendFormat("\r\nvar url='{0}UserControls/PopUpCalendar.aspx?curdate='+document.getElementById(fieldName).value+'&control=' + fieldName + '&soc=' + setAutoPostBack", AppPath);
			sb.Append("\r\nvar theHeight=202;");
			sb.Append("\r\nvar theWidth=260;");
			sb.Append("\r\nOpenWindow(url, theHeight, theWidth, 0, 0, 0, 0, 0, window.event);");
			sb.Append("\r\n}");
			Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), scriptKey, sb.ToString(), true);
		}

	}


	#endregion

	#endregion

	#region Public Members

	#region Events

	public event EventHandler DateChanged;

	protected void txtDate_TextChanged(object sender, EventArgs e) {
		if (DateChanged != null)
			DateChanged(sender, e);
	}

	#endregion

	#region Methods

	public void Validate() {
		rfvDate.Validate();

		if (UseDBDates)
			rngDBDate.Validate();
		else
			cmpIsDate.Validate();

		if (cmpDates.Enabled)
			cmpDates.Validate();
	}

	public void Clear() {
		txtDate.Text = "";
	}

	public override void Focus() {
		txtDate.Focus();
	}

	#endregion

	#region Properties

	public bool AutoPostBack {
		get { return txtDate.AutoPostBack; }
		set { txtDate.AutoPostBack = value; }
	}
	public short TabIndex {
		get { return txtDate.TabIndex; }
		set { txtDate.TabIndex = value; }
	}
	public DateTime Value {
		get { return (Common.IsDate(txtDate.Text)) ? Convert.ToDateTime(txtDate.Text) : DateTime.MinValue; }
		set { txtDate.Text = (value != DateTime.MinValue) ? value.ToShortDateString() : ""; }
	}
	public string ValueString {
		get { return txtDate.Text.Trim(); }
		//set { txtDate.Text = value; }
	}
	public string Name {
		get {
			if (ViewState["Name"] == null)
				ViewState.Add("Name", DEFAULT_DATE_NAME);
			return ViewState["Name"].ToString();
		}
		set { ViewState["Name"] = value; }

	}
	public string NameClass {
		get { return lblName.CssClass; }
		set { lblName.CssClass = value; }
	}
	public string NameStyle {
		get {
			if (ViewState["NameStyle"] == null)
				ViewState.Add("NameStyle", "");
			return ViewState["NameStyle"].ToString();
		}
		set { ViewState["NameStyle"] = value; }
	}
	public LabelPosition NamePosition {
		get {
			if (ViewState["NamePosition"] == null)
				ViewState.Add("NamePosition", DEFAULT_NAME_POSITION);
			return (LabelPosition)ViewState["NamePosition"];
		}
		set { ViewState["NamePosition"] = value; }
	}
	private bool gShowName = true;
	public bool ShowName {
		get { return gShowName; }
		set { gShowName = value; }
	}

	public bool Enabled {
		set {
			txtDate.Enabled = value;
			//hlCalender.Enabled = value;
		}
		get { return txtDate.Enabled; }
	}

	public bool ReadOnly {
		get { return txtDate.ReadOnly; }
		set {
			txtDate.ReadOnly = value;
			txtDate_CalendarExtender.Enabled = !value;
			//hlCalender.Enabled = !value;
		}
	}


	public bool UseValidationSummary {
		get {
			if (ViewState["UseValidationSummary"] == null)
				ViewState.Add("UseValidationSummary", false);
			return (bool)ViewState["UseValidationSummary"];
		}
		set { ViewState["UseValidationSummary"] = value; }

	}
	public string ValidationGroup {
		get { return rfvDate.ValidationGroup; }
		set {
			rfvDate.ValidationGroup = value;
			cmpDates.ValidationGroup = value;
			rngDBDate.ValidationGroup = value;
			cmpIsDate.ValidationGroup = value;
		}
	}
	public bool UseDBDates {
		get {
			if (ViewState["UseDBDates"] == null)
				ViewState.Add("UseDBDates", true);
			return (bool)ViewState["UseDBDates"];
		}
		set { ViewState["UseDBDates"] = value; }
	}
	public bool Required {
		set { rfvDate.Enabled = value; }
		get { return rfvDate.Enabled; }
	}
	public bool IsValid {
		get {
			bool retVal = false;
			if (rfvDate.IsValid)
				if ((UseDBDates) ? rngDBDate.IsValid : cmpIsDate.IsValid)
					retVal = (cmpDates.Enabled) ? cmpDates.IsValid : true;
			return retVal;
		}
	}
	public bool IsEmpty {
		get { return (this.ValueString.Trim() == ""); }
	}
	public string DateControlToCompare {
		get {
			if (ViewState["DateControlToCompare"] == null)
				ViewState.Add("DateControlToCompare", "");
			return ViewState["DateControlToCompare"].ToString();
		}
		set { ViewState["DateControlToCompare"] = value; }
	}
	public ValidationCompareOperator CompareOperator {
		get {
			if (ViewState["CompareOperator"] == null)
				ViewState.Add("CompareOperator", ValidationCompareOperator.LessThanEqual);
			return (ValidationCompareOperator)ViewState["CompareOperator"];
		}
		set { ViewState["CompareOperator"] = value; }
	}

	public string DateFieldClientID {
		get { return txtDate.ClientID; }
	}

	public string DateFieldUniqueID {
		get { return txtDate.UniqueID; }
	}

	public string CssClass {
		get { return pnlMain.CssClass; }
		set { pnlMain.CssClass = value; }
	}

	public string OnChange {
		get { return txtDate.Attributes["onChange"].ToString(); }
		set { txtDate.Attributes["onChange"] = value; }
	}

	public AjaxControlToolkit.CalendarPosition PopupPosition {
		get { return txtDate_CalendarExtender.PopupPosition; }
		set { txtDate_CalendarExtender.PopupPosition = value; }
	}

	public bool IsValidDate {
		get {
			DateTime temp;
			if (DateTime.TryParse(txtDate.Text, out temp)) {
				if (UseDBDates) {
					return ((Convert.ToDateTime(rngDBDate.MinimumValue) <= temp) && (temp <= Convert.ToDateTime(rngDBDate.MaximumValue)));
				}
				return true;
			}
			return false;
		}
	}

	public bool EnableClientScript {
		get { return rfvDate.EnableClientScript; }
		set {
			rfvDate.EnableClientScript = value;
			rngDBDate.EnableClientScript = value;
			cmpDates.EnableClientScript = value;
			cmpIsDate.EnableClientScript = value;
		}
	}

	

	public ValidatorDisplay ValidationDisplay {
		get { return rfvDate.Display; }
		set {
			rfvDate.Display = value;
			rngDBDate.Display = value;
			cmpDates.Display = value;
			cmpIsDate.Display = value;
		}
	}
	


	public bool EnableCompareDates {
		get {
			if (ViewState["EnableCompareDates"] == null)
				ViewState.Add("EnableCompareDates", true);
			return (bool)ViewState["EnableCompareDates"];
		}
		set { ViewState["EnableCompareDates"] = value; }
	}

	public string OnFocus {
		get { return txtDate.Attributes["onFocus"]; }
		set { txtDate.Attributes.Add("onFocus", value); }
	}

	public string ToolTip {
		get { return pnlMain.ToolTip; }
		set { pnlMain.ToolTip = value; }
	}


	#endregion

	#endregion
}

