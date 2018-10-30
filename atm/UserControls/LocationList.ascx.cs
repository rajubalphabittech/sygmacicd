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
using atm;
using Sygmanet;
using SygmaFramework;

public partial class UserControls_LocationList : System.Web.UI.UserControl {
	public enum IDColumn {
		SyscoHouseNo,
		SygmaCenterNo,
		LocationId
	}

	protected void Page_Load(object sender, EventArgs e) {

	}

	protected void Page_PreRender(object sender, EventArgs e) {
		rfvLocation.ErrorMessage = string.Format("'{0}' is required!", Name);
	}
	[Obsolete("Use the overrides that use the IDColumn.  This is for legacy only")]
	public void SetLocations(bool useSygmaCenterNo) {
		SetLocations(useSygmaCenterNo, null);
	}
	[Obsolete("Use the overrides that use the IDColumn.  This is for legacy only")]
	public void SetLocations(bool useSygmaCenterNo, string[] specificLocations) {
		IDColumn idColumn = (useSygmaCenterNo) ? IDColumn.SygmaCenterNo : IDColumn.SyscoHouseNo;
		SetLocations(idColumn, specificLocations);
		UseSygmaCenterNo = useSygmaCenterNo;
	}

	public void SetLocations() {
		SetLocations(IDColumnUsed, null);
	}
	public void SetLocations(string[] specificLocations) {
		SetLocations(IDColumnUsed, specificLocations);
	}
	public void SetLocations(IDColumn idColumn) {
		SetLocations(idColumn, null);
	}
	public void SetLocations(IDColumn idColumn, string[] specificLocations) {
		string colName = GetIDColumn(idColumn);

		DataView dv = WebCommon.GetCenterList(BasePage.IntranetDB, Type);
		if (dv.Count > 0) {
			if (specificLocations != null && specificLocations.Length > 0)
				dv.RowFilter = string.Format("{0} in ({1})", colName, string.Join(",", specificLocations));
			ddLocation.DataTextField = "Description";
			ddLocation.DataValueField = colName;
			ddLocation.DataSource = dv;
			ddLocation.DataBind();
			if (InsertInitialItem)
				ddLocation.Items.Insert(0, new ListItem(gInitialItemText, InitialItemValue));
		}
		IDColumnUsed = idColumn;
	}
	public override void Focus() {
		ddLocation.Focus();
	}

	private string GetIDColumn(IDColumn idColumn) {
		switch (idColumn) {
			case IDColumn.SyscoHouseNo:
				return "SyscoHouseNo";
			case IDColumn.SygmaCenterNo:
				return "SygmaCenterNo";
			case IDColumn.LocationId:
				return "LocationID";
			default:
				return "SyscoHouseNo";
		}
	}

	public void ClearSelection() {
		ddLocation.ClearSelection();
	}

	private LocationType gLocationType = LocationType.DistributionCenter;

	public LocationType Type {
		get { return gLocationType; }
		set { gLocationType = value; }
	}

	public bool AutoPostBack {
		get { return ddLocation.AutoPostBack; }
		set { ddLocation.AutoPostBack = value; }
	}

	public string SelectedValue {
		get { return ddLocation.SelectedValue; }
		set {
			ListItem li = ddLocation.Items.FindByValue(value);
			if (li != null) {
				ddLocation.ClearSelection();
				li.Selected = true;
			}
		}
	}

	public ListItemCollection Items {
		get { return ddLocation.Items; }
	}

	public bool IsRequired {
		get { return rfvLocation.Visible; }
		set { rfvLocation.Visible = value; }
	}

	private string gInitialItemText = "Choose...";

	public string InitialItemText {
		get { return gInitialItemText; }
		set { gInitialItemText = value; }
	}

	public string InitialItemValue {
		get {
			if (ViewState["InitialItemValue"] == null)
				ViewState.Add("InitialItemValue", "");
			return (string)ViewState["InitialItemValue"];
		}
		set { ViewState["InitialItemValue"] = value; }
	}

	public string Name {
		get {
			if (ViewState["Name"] == null)
				ViewState.Add("Name", "Location");
			return (string)ViewState["Name"];
		}
		set { ViewState["Name"] = value; }
	}

	[Obsolete("Use the IDColumnUsed.  This is for legacy only")]
	public bool UseSygmaCenterNo {
		get { return (IDColumnUsed == IDColumn.SygmaCenterNo); }
		set {
			if (value)
				IDColumnUsed = IDColumn.SygmaCenterNo;
			else
				IDColumnUsed = IDColumn.SyscoHouseNo;
		}
	}

	public IDColumn IDColumnUsed {
		get {
			if (ViewState["IDColumnUsed"] == null)
				ViewState.Add("IDColumnUsed", IDColumn.SyscoHouseNo);
			return (IDColumn)ViewState["IDColumnUsed"];
		}
		set { ViewState["IDColumnUsed"] = value; }
	}

	private LocationInfo gLocationInfo;
	private bool gLocationInfoSet;
	public LocationInfo SelectedLocationInfo {
		get {
			if (!gLocationInfoSet) {
				if (SelectedValue != "") {
					gLocationInfo = new LocationInfo(BasePage.IntranetDB, (IDColumnUsed == IDColumn.SygmaCenterNo), SelectedValue);
					gLocationInfoSet = true;
				}
			}
			return gLocationInfo;
		}
	}

	private BasePage gBasePage;
	public BasePage BasePage {
		get {
			try {
				if (gBasePage == null)
					gBasePage = (BasePage)this.Page;
				return gBasePage;
			} catch (Exception exp) {
				throw new InvalidBasePageException(exp);
			}
		}
	}

	public bool Enabled {
		get { return ddLocation.Enabled; }
		set { ddLocation.Enabled = value; }
	}

	private bool gRemoveInitialOnPostBack = true;

	public bool RemoveInitialOnPostBack {
		get { return gRemoveInitialOnPostBack; }
		set { gRemoveInitialOnPostBack = value; }
	}

	public bool InsertInitialItem {
		get {
			if (ViewState["InsertInitialItem"] == null)
				ViewState.Add("InsertInitialItem", true);
			return (bool)ViewState["InsertInitialItem"];
		}
		set { ViewState["InsertInitialItem"] = value; }
	}


	public event EventHandler SelectedIndexChanged;
	protected void ddLocation_SelectedIndexChanged(object sender, EventArgs e) {
		if (gRemoveInitialOnPostBack) {
			ListItem li = ddLocation.Items.FindByValue(InitialItemValue);
			if (li != null)
				ddLocation.Items.Remove(li);
		}
		if (SelectedIndexChanged != null)
			SelectedIndexChanged(sender, e);
	}
}
