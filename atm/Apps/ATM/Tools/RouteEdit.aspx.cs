using ATM.Payroll;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using atm;

public partial class Apps_ATM_Tools_RouteEdit : ATMPage
{
	private DataTable dtPayScales;
	private DataTable dtClassifications;
	private DataTable dtDomiciles;
	private DataTable dtDrivers;
	private DataTable dtHelpers;
	protected override void LoadATMPage()
	{
		setPageVariables();
		if (IsPostBack) return;
		RowCountBar1.PageSize = 25;
		loadCenters();
		var dsPayScales = ATMDB.GetDataSet("up_p_getPayScales");
		dtPayScales = dsPayScales.Tables[0];
	}

	protected void ddSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
	{
        CenterSelectionIndexChanged();
    }

    protected void CenterSelectionIndexChanged()
    {
        if (RouteSortExpression == string.Empty)
        {
            RouteSortExpression = "RouteNo";
            RouteSortDir = SortDirection.Ascending;
        }
        setRoutes();
    }


    protected void txtRouteSearch_TextChanged(object sender, EventArgs e)
	{
		setRoutes();
	}

	private void setPageVariables()
	{
		AddClientVariable("gUserName", UserName);
	}

	private void loadCenters()
	{
		DataView dv = ATMDB.GetDataView("up_getCenters", UserName);
		ddSygmaCenterNo.DataSource = dv;
		ddSygmaCenterNo.DataBind();
		ddSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
        if (ddSygmaCenterNo.Items.Count == 2)
        {
            ddSygmaCenterNo.SelectedIndex = 1;
            CenterSelectionIndexChanged();
        }
    }

	protected void btnRefreshRoutes_Click(object sender, EventArgs e)
	{
		setRoutes();
	}

	[Serializable]
	public class InvalidExcelValueException : Exception
	{
		public InvalidExcelValueException(int rowNumber, int columnNumber, string suppliedColumnName)
				: base("Invalid Excel Value")
		{
			RowNumber = rowNumber;
			ColumnNumber = columnNumber;
			SuppliedColumnName = suppliedColumnName;
		}
		public InvalidExcelValueException(string message) : base(message) { }
		public InvalidExcelValueException(string message, Exception inner) : base(message, inner) { }
		protected InvalidExcelValueException(
		SerializationInfo info,
		StreamingContext context)
				: base(info, context) { }

		public int RowNumber { get; private set; }
		public int ColumnNumber { get; private set; }
		public string SuppliedColumnName { get; private set; }
	}


	private void setRoutes()
	{
		if (ddSygmaCenterNo.SelectedIndex > 0)
		{
			DataSet dsPayScales = ATMDB.GetDataSet("up_p_getPayScales");
			dtPayScales = dsPayScales.Tables[0];
			DataSet dsDomiciles = ATMDB.GetDataSet("up_p_getDomiciles", ddSygmaCenterNo.SelectedValue);
			dtDomiciles = dsDomiciles.Tables[0];
			dtClassifications = dsDomiciles.Tables[1];
			DataSet dsDrivers = ATMDB.GetDataSet("up_p_getDrivers", ddSygmaCenterNo.SelectedValue);
			dtDrivers = dsDrivers.Tables[0];
			DataSet dsHelpers = ATMDB.GetDataSet("up_p_getDriverHelpers", ddSygmaCenterNo.SelectedValue);
			dtHelpers = dsHelpers.Tables[0];
			pnlProgression.Visible = true;
			//ddAddLocation.DataSource = dtDomiciles;
			//ddAddLocation.DataBind();
			if (dtDomiciles != null)
			{
				ddAddLocation.Items.Clear();
				ddAddLocation.Items.Add(new ListItem("Choose...", ""));
				foreach (DataRow rtd in dtDomiciles.Rows)
				{
					ListItem liRTD = new ListItem(rtd["LocationName"].ToString(), rtd["LocationId"].ToString());
					ddAddLocation.Items.Add(liRTD);
				}
			}
			if (dtClassifications != null)
			{
				ddAddClassification.Items.Clear();
				ddAddClassification.Items.Add(new ListItem("Choose...", ""));
				foreach (DataRow rtd in dtClassifications.Rows)
				{
					ListItem liRTD = new ListItem(rtd["ClassificationName"].ToString(), rtd["ClassificationId"].ToString());
					ddAddClassification.Items.Add(liRTD);
				}
			}
			if (dtDrivers != null)
			{
				ddAddDriver.Items.Clear();
				ddAddDriver.Items.Add(new ListItem("Choose...", ""));
				foreach (DataRow rtd in dtDrivers.Rows)
				{
					ListItem liRTD = new ListItem(rtd["DriverName"].ToString(), rtd["DriverId"].ToString());
					ddAddDriver.Items.Add(liRTD);
				}
			}
			if (dtHelpers != null)
			{
				ddAddDriverHelper.Items.Clear();
				ddAddDriverHelper.Items.Add(new ListItem("Choose...", ""));
				foreach (DataRow rtd in dtHelpers.Rows)
				{
					ListItem liRTD = new ListItem(rtd["DriverName"].ToString(), rtd["DriverId"].ToString());
					ddAddDriverHelper.Items.Add(liRTD);
				}
			}

			ddDepartDay.Items.Clear();
			ddDepartDay.Items.Add("Choose...");
			ddDepartDay.Items.Add("Sunday");
			ddDepartDay.Items.Add("Monday");
			ddDepartDay.Items.Add("Tuesday");
			ddDepartDay.Items.Add("Wednesday");
			ddDepartDay.Items.Add("Thursday");
			ddDepartDay.Items.Add("Friday");
			ddDepartDay.Items.Add("Saturday");

			ddAltDepartDay.Items.Clear();
			ddAltDepartDay.Items.Add("Choose...");
			ddAltDepartDay.Items.Add("Sunday");
			ddAltDepartDay.Items.Add("Monday");
			ddAltDepartDay.Items.Add("Tuesday");
			ddAltDepartDay.Items.Add("Wednesday");
			ddAltDepartDay.Items.Add("Thursday");
			ddAltDepartDay.Items.Add("Friday");
			ddAltDepartDay.Items.Add("Saturday");

			//ddAddDriver.Attributes.Add("onchange", string.Format("UpdateHelper({0}, {1});", ddAddDriver.SelectedValue, ddAddDriver.SelectedItem));
			//ddDriver.Attributes.Add("onchange", string.Format("UpdateRouteDetails(this, 5, {0});", row["RouteDetailId"]));

			//ddAddDriverPayScale.DataSource = dtPayScales;
			//ddAddDriverPayScale.DataBind();
			if (dtPayScales != null)
			{
				ddAddDriverPayScale.Items.Clear();
				ddAddDriverPayScale.Items.Add(new ListItem("Choose...", ""));
				foreach (DataRow rtd in dtPayScales.Rows)
				{
					ListItem liRTD = new ListItem(rtd["PayScaleDesignator"].ToString(), rtd["PayScaleId"].ToString());
					ddAddDriverPayScale.Items.Add(liRTD);
				}
			}

			//ddAddHelperPayScale.DataSource = dtPayScales;
			//ddAddHelperPayScale.DataBind();
			if (dtPayScales != null)
			{
				ddAddHelperPayScale.Items.Clear();
				ddAddHelperPayScale.Items.Add(new ListItem("Choose...", ""));
				foreach (DataRow rtd in dtPayScales.Rows)
				{
					ListItem liRTD = new ListItem(rtd["PayScaleDesignator"].ToString(), rtd["PayScaleId"].ToString());
					ddAddHelperPayScale.Items.Add(liRTD);
				}
			}

			//SetAddRouteControls(dtPayScales, dtDomiciles);
			DataView dv = ATMDB.GetDataView("up_getRoutes", UserName, ddSygmaCenterNo.SelectedValue);
			AddSort(dv);
			AddNameFilter(dv);
			gvRoutes.PageSize = RowCountBar1.PageSize;
			gvRoutes.DataSource = dv;
			gvRoutes.DataBind();
			pnlRoutes.Visible = true;
			//lblRouteCount.Text = dv.Count.ToString();
			RowCountBar1.ItemCount = dv.Count;
		}
		else
		{
			pnlProgression.Visible = false;
			pnlRoutes.Visible = false;
		}
	}

	protected void RowCountBar1_PageSizeChanged(object sender, EventArgs e)
	{
		gvRoutes.PageSize = RowCountBar1.PageSize;
		setRoutes();
	}

	private void AddSort(DataView dv)
	{
		if (RouteSortExpression != "")
			dv.Sort = string.Format("{0} {1}", RouteSortExpression, WebCommon.GetDBSortDirection(RouteSortDir));
	}
	private void AddNameFilter(DataView dv)
	{
		string[] names = txtRouteSearch.Text.Trim().Replace(".", "").Replace(",", "").Replace("'", "''").Replace("%", "").Replace("*", "").Split(' ');
		if (names.Length > 0)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string name in names)
			{
				if (sb.Length > 0)
					sb.Append(" OR ");
				sb.AppendFormat("RouteNo like '%{0}%'", name);
			}
			dv.RowFilter = sb.ToString();
		}
	}

	[WebMethod]
	public static string[] PM_ValidateRouteNo(int sygmaCenterNo, string routeNo)
	{
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_validateAddRouteNo", sygmaCenterNo, routeNo);
		string[] str = new string[2];
		str[0] = ds.Tables[0].Rows[0][0].ToString();
		if (ds.Tables[1].Rows.Count > 0)
		{
			DataRow row = ds.Tables[1].Rows[0];
			str[1] = (row["ZipCode"]).ToString();
		}
		else
		{
			str[1] = "No Match";
		}
		return str;
	}

	[WebMethod]
	public static string PM_RemoveRoute(string userName, int routeId)
	{
		RunNonQueryFromStatic("ATM", "up_p_removeRouteDetail", userName, routeId);
		return "";
	}

	[WebMethod]
	//public static string PM_AddRoute(string userName, int sygmaCenterNo, string routeNo, string routeName, int miles, int locationId, int classificationId, int driverPayScale, int helperPayScale, string zipCode, int driverId, int helperId, string departDay, string departTime, int duration, int isHolidayRoute, string altRouteNo, string altDepartDay, string altDepartTime, int altDuration)
	public static string PM_AddRoute(string userName, int sygmaCenterNo, string routeNo, string routeName, int miles, int locationId, int classificationId, int driverPayScale, int helperPayScale, string zipCode, int driverId, int helperId, string departDay, string departTime, int duration, int isHolidayRoute, string altDetail)
	{
		string altRouteNo = "";
		string altDepartDay = "";
		string altDepartTime = "";
		int altDuration = 0;
		if (isHolidayRoute == 1)
		{
			int RouteNoIndex = altDetail.IndexOf(",");
			altRouteNo = altDetail.Substring(0, RouteNoIndex);
			int DepartDayIndex = altDetail.IndexOf(",", RouteNoIndex + 1);
			altDepartDay = altDetail.Substring(RouteNoIndex + 1, DepartDayIndex - (RouteNoIndex + 1));
			int DepartTimeIndex = altDetail.IndexOf(",", DepartDayIndex + 1);
			altDepartTime = altDetail.Substring(DepartDayIndex + 1, DepartTimeIndex - (DepartDayIndex + 1));
			if (altDetail.Length - 1 != DepartTimeIndex)
			{
				altDuration = Convert.ToInt32(altDetail.Substring(DepartTimeIndex + 1));
			}
		}
		//return GetScalarFromStatic("ATM", "up_p_addRouteDetail", userName, sygmaCenterNo, routeNo, routeName, miles, locationId, classificationId, driverPayScale, helperPayScale, zipCode, driverId, helperId, departDay, departTime, duration, isHolidayRoute, "201F", "Monday", "10:20", 30).ToString();
		return GetScalarFromStatic("ATM", "up_p_addRouteDetail", userName, sygmaCenterNo, routeNo, routeName, miles, locationId, classificationId, driverPayScale, helperPayScale, zipCode, driverId, helperId, departDay, departTime, duration, isHolidayRoute, altRouteNo, altDepartDay, altDepartTime, altDuration).ToString();
	}

	[WebMethod]
	public static string PM_SaveRouteMiles(string userName, int routeId, int miles)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteMiles", userName, routeId, miles);
		return miles.ToString();
	}


	[WebMethod]
	public static string PM_SaveZipCode(string userName, int routeId, string ZipCOde)
	{
		return GetScalarFromStatic("ATM", "up_p_setRouteZip", userName, routeId, ZipCOde).ToString();
	}

	[WebMethod]
	public static void PM_SaveRouteName(string userName, int routeId, string routeName)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteName", userName, routeId, routeName);
	}

	[WebMethod]
	public static void PM_SaveAltRouteNo(string userName, int routeId, string altRouteNo)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteAltRouteNo", userName, routeId, altRouteNo);
	}

	[WebMethod]
	public static void PM_SaveRouteDepartDay(string userName, int routeId, int isAlt, string departDay)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteDepartDayOrTime", userName, routeId, isAlt, departDay, 1);
	}

	[WebMethod]
	public static string PM_SaveRouteDepartTime(string userName, int routeId, int isAlt, string departTime)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteDepartDayOrTime", userName, routeId, isAlt, departTime, 0);
		return departTime;
	}

	[WebMethod]
	public static void PM_SaveRouteDetails(string userName, int routeId, int detailId, int id)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteDetails", userName, routeId, detailId, id);
	}

	[WebMethod]
	public static int PM_SaveRouteDuration(string userName, int routeId, int detailId, int duration)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteDetails", userName, routeId, detailId, duration);
		return duration;
	}

	[WebMethod]
	public static int PM_SaveHolidayRouteFlag(string userName, int routeId, int IsHolidayRoute)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteDetails", userName, routeId, 9, IsHolidayRoute);
		return IsHolidayRoute;
	}

	[WebMethod]
	public static void PM_SaveActiveFlag(string userName, int routeId, int IsActive)
	{
		RunNonQueryFromStatic("ATM", "up_p_setRouteDetails", userName, routeId, 10, IsActive);
	}

	public string RouteSortExpression
	{
		get
		{
			if (ViewState["RouteSortExpression"] == null)
				ViewState.Add("RouteSortExpression", "");
			return (string)ViewState["RouteSortExpression"];
		}
		set { ViewState["RouteSortExpression"] = value; }
	}
	public SortDirection RouteSortDir
	{
		get
		{
			if (ViewState["RouteSortDir"] == null)
				ViewState.Add("RouteSortDir", SortDirection.Ascending);
			return (SortDirection)ViewState["RouteSortDir"];
		}
		set { ViewState["RouteSortDir"] = value; }
	}

	protected void gvRoutes_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		switch (e.Row.RowType)
		{
			case DataControlRowType.DataRow:
				DataRowView row = (DataRowView)e.Row.DataItem;
				TextBox txtMiles = (TextBox)e.Row.FindControl("txtMiles");
				TextBox txtRouteName = (TextBox)e.Row.FindControl("txtRouteName");
				TextBox txtZipCode = (TextBox)e.Row.FindControl("txtZipCode");
				Label lblCityState = (Label)e.Row.FindControl("lblCityState");
				DropDownList ddLocation = (DropDownList)e.Row.FindControl("ddLocation");
				DropDownList ddClassification = (DropDownList)e.Row.FindControl("ddClassification");
				DropDownList ddDriver = (DropDownList)e.Row.FindControl("ddDriver");
				DropDownList ddHelper = (DropDownList)e.Row.FindControl("ddHelper");
				DropDownList ddDriverPayScale = (DropDownList)e.Row.FindControl("ddDriverPayScale");
				DropDownList ddHelperPayScale = (DropDownList)e.Row.FindControl("ddHelperPayScale");
				DropDownList ddDepartDay = (DropDownList)e.Row.FindControl("ddDepartDay");
				TextBox txtDepartTime = (TextBox)e.Row.FindControl("txtDepartTime");
				TextBox txtDuration = (TextBox)e.Row.FindControl("txtDuration");
				CheckBox cbHolidayRoute = (CheckBox)e.Row.FindControl("cbHolidayRoute");
				TextBox txtAltRouteNo = (TextBox)e.Row.FindControl("txtAltRouteNo");
				DropDownList ddAltDepartDay = (DropDownList)e.Row.FindControl("ddAltDepartDay");
				TextBox txtAltDepartTime = (TextBox)e.Row.FindControl("txtAltDepartTime");
				TextBox txtAltDuration = (TextBox)e.Row.FindControl("txtAltDuration");
				CheckBox cbIsActive = (CheckBox)e.Row.FindControl("cbIsActive");

				cbHolidayRoute.InputAttributes.Add("data-id", row["RouteDetailId"].ToString());
				txtAltRouteNo.Attributes.Add("data-id", row["RouteDetailId"].ToString());
				ddAltDepartDay.Attributes.Add("data-id", row["RouteDetailId"].ToString());
				txtAltDuration.Attributes.Add("data-id", row["RouteDetailId"].ToString());
				txtAltDepartTime.Attributes.Add("data-id", row["RouteDetailId"].ToString());

				if (!Convert.IsDBNull(row["Miles"]))
				{
					txtMiles.Text = row["Miles"].ToString();
					txtMiles.Attributes.Add("OrigVal", row["Miles"].ToString());
				}
				else
				{
					txtMiles.Attributes.Add("OrigVal", "");
				}

				txtZipCode.Attributes.Add("data-id", row["RouteDetailId"].ToString());
				if (!Convert.IsDBNull(row["ZipCode"]))
				{
					txtZipCode.Text = row["ZipCode"].ToString();
					txtZipCode.Attributes.Add("OrigVal", row["ZipCode"].ToString());
				}
				else
				{
					txtZipCode.Attributes.Add("OrigVal", "");
				}

				lblCityState.Attributes.Add("data-id", row["RouteDetailId"].ToString());
				if (!Convert.IsDBNull(row["CityState"]))
				{
					lblCityState.Text = row["CityState"].ToString();
				}

				if (!Convert.IsDBNull(row["RouteName"]))
				{
					txtRouteName.Text = row["RouteName"].ToString();
				}

				if (!Convert.IsDBNull(row["DepartTime"]))
				{
					txtDepartTime.Text = row["DepartTime"].ToString();
					txtDepartTime.Attributes.Add("OrigVal", row["DepartTime"].ToString());
				}
				else
				{
					txtDepartTime.Attributes.Add("OrigVal", "");
				}

				if (!Convert.IsDBNull(row["Duration"]))
				{
					txtDuration.Text = row["Duration"].ToString();
					txtDuration.Attributes.Add("OrigVal", row["Duration"].ToString());
				}
				else
				{
					txtDuration.Attributes.Add("OrigVal", "");
				}

				if (!Convert.IsDBNull(row["AltRouteNo"]))
				{
					txtAltRouteNo.Text = row["AltRouteNo"].ToString();
					txtAltRouteNo.Attributes.Add("OrigVal", row["AltRouteNo"].ToString());
				}
				else
				{
					txtAltRouteNo.Attributes.Add("OrigVal", "");
				}

				if (!Convert.IsDBNull(row["AltDepartTime"]))
				{
					txtAltDepartTime.Text = row["AltDepartTime"].ToString();
					txtAltDepartTime.Attributes.Add("OrigVal", row["AltDepartTime"].ToString());
				}
				else
				{
					txtAltDepartTime.Attributes.Add("OrigVal", "");
				}

				if (!Convert.IsDBNull(row["AltDuration"]))
				{
					txtAltDuration.Text = row["AltDuration"].ToString();
					txtAltDuration.Attributes.Add("OrigVal", row["AltDuration"].ToString());
				}
				else
				{
					txtAltDuration.Attributes.Add("OrigVal", "");
				}

				if (!Convert.IsDBNull(row["DepartDay"]))
				{
					ddDepartDay.Items.Add("Choose...");
					ddDepartDay.Items.Add("Sunday");
					ddDepartDay.Items.Add("Monday");
					ddDepartDay.Items.Add("Tuesday");
					ddDepartDay.Items.Add("Wednesday");
					ddDepartDay.Items.Add("Thursday");
					ddDepartDay.Items.Add("Friday");
					ddDepartDay.Items.Add("Saturday");
					ddDepartDay.Items.FindByValue(row["DepartDay"].ToString()).Selected = true;
					for (int i = 0; i < 8; i++)
					{
						if (ddDepartDay.Items[i].Value == row["DepartDay"].ToString())
						{
							ddDepartDay.Items[i].Selected = true;
							break;
						}
					}
				}
				else
				{
					ddDepartDay.Items.Clear();
					ddDepartDay.Items.Add("Choose...");
					ddDepartDay.Items.Add("Sunday");
					ddDepartDay.Items.Add("Monday");
					ddDepartDay.Items.Add("Tuesday");
					ddDepartDay.Items.Add("Wednesday");
					ddDepartDay.Items.Add("Thursday");
					ddDepartDay.Items.Add("Friday");
					ddDepartDay.Items.Add("Saturday");
				}

				if (!Convert.IsDBNull(row["AltDepartDay"]))
				{
					ddAltDepartDay.Items.Add("Choose...");
					ddAltDepartDay.Items.Add("Sunday");
					ddAltDepartDay.Items.Add("Monday");
					ddAltDepartDay.Items.Add("Tuesday");
					ddAltDepartDay.Items.Add("Wednesday");
					ddAltDepartDay.Items.Add("Thursday");
					ddAltDepartDay.Items.Add("Friday");
					ddAltDepartDay.Items.Add("Saturday");
					ddAltDepartDay.Items.FindByValue(row["AltDepartDay"].ToString()).Selected = true;
					for (int i = 0; i < 8; i++)
					{
						if (ddAltDepartDay.Items[i].Value == row["AltDepartDay"].ToString())
						{
							ddAltDepartDay.Items[i].Selected = true;
							break;
						}
					}
				}
				else
				{
					ddAltDepartDay.Items.Clear();
					ddAltDepartDay.Items.Add("Choose...");
					ddAltDepartDay.Items.Add("Sunday");
					ddAltDepartDay.Items.Add("Monday");
					ddAltDepartDay.Items.Add("Tuesday");
					ddAltDepartDay.Items.Add("Wednesday");
					ddAltDepartDay.Items.Add("Thursday");
					ddAltDepartDay.Items.Add("Friday");
					ddAltDepartDay.Items.Add("Saturday");
				}



				if (!Convert.IsDBNull(row["LocationID"]))
				{
					ddLocation.DataSource = dtDomiciles;
					ddLocation.DataBind();
					ddLocation.Items.FindByValue(row["LocationID"].ToString()).Selected = true;
					for (int i = 0; i < dtDomiciles.Rows.Count; i++)
					{
						if (ddLocation.Items[i].Value == row["LocationID"].ToString())
						{
							ddLocation.Items[i].Selected = true;
							break;
						}
					}
				}
				if (!Convert.IsDBNull(row["ClassificationId"]))
				{
					ddClassification.DataSource = dtClassifications;
					ddClassification.DataBind();
					ddClassification.Items.FindByValue(row["ClassificationId"].ToString()).Selected = true;
					for (int i = 0; i < dtClassifications.Rows.Count; i++)
					{
						if (ddClassification.Items[i].Value == row["ClassificationId"].ToString())
						{
							ddClassification.Items[i].Selected = true;
							break;
						}
					}
				}
				if (!Convert.IsDBNull(row["DriverId"]))
				{
					long driverId = Convert.ToInt32(row["DriverId"]);
					ddDriver.DataSource = dtDrivers;
					ddDriver.DataBind();
					//ddDriver.Items.FindByValue(row["DriverId"].ToString()).Selected = true;
					for (int i = 0; i < dtDrivers.Rows.Count; i++)
					{
						if (ddDriver.Items[i].Value == row["DriverId"].ToString())
						{
							ddDriver.Items[i].Selected = true;
							break;
						}
					}
					ddDriver.Items.Insert(0, new ListItem("Choose...", "0"));

					if (ddDriver.SelectedItem.Text == "Choose...")
					{
						string driverName = (string)ATMDB.GetScalar("up_p_getDriversNameForInactive", driverId);
						ddDriver.Items.Insert(0, new ListItem("** " + driverName + " **", "999"));
						ddDriver.ClearSelection();
						ddDriver.Items.FindByValue("999").Selected = true;
                        ddDriver.BackColor = System.Drawing.Color.Gray;
                        ddDriver.ToolTip = driverName + " is currently marked as inactive. Please change the driver if needed!";
					}
				}
				else
				{
					ddDriver.Items.Clear();
					ddDriver.Items.Add(new ListItem("Choose...", "0"));
					foreach (DataRow rtd in dtDrivers.Rows)
					{
						ListItem liRTD = new ListItem(rtd["DriverName"].ToString(), rtd["DriverId"].ToString());
						ddDriver.Items.Add(liRTD);
					}
					//ddHelper.Items.FindByText("Choose...").Selected = true;
					ddHelper.Enabled = false;
                    ddHelper.CssClass = "disabled-drop-down";

                }
				if (!Convert.IsDBNull(row["DriverHelperId"]))
				{
					long helperId = Convert.ToInt32(row["DriverHelperId"]);
					ddHelper.DataSource = dtHelpers;
					ddHelper.DataBind();
					//ddHelper.Items.FindByValue(row["DriverHelperId"].ToString()).Selected = true;
					for (int i = 0; i < dtHelpers.Rows.Count; i++)
					{
						if (ddHelper.Items[i].Value == row["DriverHelperId"].ToString())
						{
							ddHelper.Items[i].Selected = true;
							break;
						}
					}
					ddHelper.Items.Insert(0, new ListItem("Choose...", "0"));
					if (ddHelper.SelectedItem.Text == "Choose...")
					{
						string helperName = (string)ATMDB.GetScalar("up_p_getDriversNameForInactive", helperId);
						ddHelper.Items.Insert(0, new ListItem("** " + helperName + " **", "999"));
						ddHelper.ClearSelection();
						ddHelper.Items.FindByValue("999").Selected = true;
                        ddHelper.BackColor = System.Drawing.Color.Gray;
                        ddHelper.ToolTip = helperName + " is currently marked as inactive. Please change the helper if needed!";
					}

					//if (Convert.ToInt32(ddDriver.SelectedValue) == 0)
					//{
					//    ddHelper.ClearSelection();
					//    ddHelper.Items.FindByText("Choose...").Selected = true;    
					//}
				}
				else
				{
					ddHelper.Items.Clear();
					ddHelper.Items.Add(new ListItem("Choose...", "0"));
					foreach (DataRow rtd in dtHelpers.Rows)
					{
						ListItem liRTD = new ListItem(rtd["DriverName"].ToString(), rtd["DriverId"].ToString());
						ddHelper.Items.Add(liRTD);
					}
					if (Convert.ToInt32(ddDriver.SelectedValue) == 0)
					{
						ddHelper.ClearSelection();
						ddHelper.Items.FindByText("Choose...").Selected = true;
						ddHelper.Enabled = false;
                        ddHelper.CssClass = "disabled-drop-down";
                    }
				}
				if (ddDriver.Items.Count > 0 && ddHelper.Items.Count > 0)
				{
					string DriverSel = ddDriver.SelectedItem.ToString();
					if (DriverSel.Equals("Choose...") == false)
					{
						if (ddHelper.Items.Contains(ddDriver.SelectedItem))
						{
							ddHelper.Items.Remove(ddDriver.SelectedItem);
						}
					}
					string HelperSel = ddHelper.SelectedItem.ToString();
					if (HelperSel.Equals("Choose...") == false)
					{
						if (ddDriver.Items.Contains(ddHelper.SelectedItem))
						{
							ddDriver.Items.Remove(ddHelper.SelectedItem);
						}
					}
				}
				if (!Convert.IsDBNull(row["DriverPayScaleId"]))
				{
					ddDriverPayScale.DataSource = dtPayScales;
					ddDriverPayScale.DataBind();
					ddDriverPayScale.Items.FindByValue(row["DriverPayScaleId"].ToString()).Selected = true;
					for (int i = 0; i < dtPayScales.Rows.Count; i++)
					{
						if (ddDriverPayScale.Items[i].Value == row["DriverPayScaleId"].ToString())
						{
							ddDriverPayScale.Items[i].Selected = true;
							break;
						}
					}
				}
				if (!Convert.IsDBNull(row["DriverHelperPayScaleId"]))
				{
					ddHelperPayScale.DataSource = dtPayScales;
					ddHelperPayScale.DataBind();
					ddHelperPayScale.Items.FindByValue(row["DriverHelperPayScaleId"].ToString()).Selected = true;
					for (int i = 0; i < dtPayScales.Rows.Count; i++)
					{
						if (ddHelperPayScale.Items[i].Value == row["DriverHelperPayScaleId"].ToString())
						{
							ddHelperPayScale.Items[i].Selected = true;
							break;
						}
					}
				}


				if (Convert.ToInt32(row["IsHolidayRoute"]) == 1)
				{
					cbHolidayRoute.Checked = true;
					txtAltRouteNo.Enabled = true;
					ddAltDepartDay.Enabled = true;
					txtAltDepartTime.Enabled = true;
					txtAltDuration.Enabled = true;
                }

                if (ddAltDepartDay.Enabled == false)
                {
                    ddAltDepartDay.CssClass = "disabled-drop-down";
                }

				if (Convert.ToInt32(row["IsActive"]) == 1)
				{
					cbIsActive.Checked = true;
				}
				if (Convert.ToInt16(row["IsDriverDuplicated"]) == 1)
				{
					ddDriver.BackColor = System.Drawing.Color.Yellow;
					ddDriver.ToolTip = "This driver is also assigned to other routes on the same day as driver/helper!!!";
				}
				if (Convert.ToInt16(row["IsHelperDuplicated"]) == 1)
				{
					ddHelper.BackColor = System.Drawing.Color.Yellow;
					ddHelper.ToolTip = "This helper is also assigned to other routes on the same day as driver/helper!!!";
				}


				txtMiles.Attributes.Add("onchange", string.Format("UpdateRouteMiles(this,{0});", row["RouteDetailId"]));
				txtZipCode.Attributes.Add("onchange", string.Format("UpdateZipCode(this,{0});", row["RouteDetailId"]));
				txtRouteName.Attributes.Add("onchange", string.Format("UpdateRouteRouteName(this,{0});", row["RouteDetailId"]));
				ddLocation.Attributes.Add("onchange", string.Format("UpdateRouteDetails(this, 1, {0});", row["RouteDetailId"]));
				ddClassification.Attributes.Add("onchange", string.Format("UpdateRouteDetails(this, 2, {0});", row["RouteDetailId"]));
				ddDriverPayScale.Attributes.Add("onchange", string.Format("UpdateRouteDetails(this, 3, {0});", row["RouteDetailId"]));
				ddHelperPayScale.Attributes.Add("onchange", string.Format("UpdateRouteDetails(this, 4, {0});", row["RouteDetailId"]));
				ddDriver.Attributes.Add("onchange", string.Format("UpdateRouteDetails(this, 5, {0});", row["RouteDetailId"]));
				ddHelper.Attributes.Add("onchange", string.Format("UpdateRouteDetails(this, 6, {0});", row["RouteDetailId"]));
				txtDuration.Attributes.Add("onchange", string.Format("UpdateRouteDuration(this, 7, {0});", row["RouteDetailId"]));
				txtDepartTime.Attributes.Add("onchange", string.Format("UpdateDepartTime(this, 0, {0});", row["RouteDetailId"]));
				ddDepartDay.Attributes.Add("onchange", string.Format("UpdateRouteDepartDay(this, 0, {0});", row["RouteDetailId"]));
				cbHolidayRoute.Attributes.Add("onclick", string.Format("UpdateHolidayRouteFlag(this, {0});", row["RouteDetailId"]));
				txtAltRouteNo.Attributes.Add("onchange", string.Format("UpdateRouteAltRouteNo(this, {0});", row["RouteDetailId"]));
				txtAltDuration.Attributes.Add("onchange", string.Format("UpdateRouteDuration(this, 8, {0});", row["RouteDetailId"]));
				txtAltDepartTime.Attributes.Add("onchange", string.Format("UpdateDepartTime(this, 1, {0});", row["RouteDetailId"]));
				ddAltDepartDay.Attributes.Add("onchange", string.Format("UpdateRouteDepartDay(this, 1, {0});", row["RouteDetailId"]));
				cbIsActive.Attributes.Add("onclick", string.Format("UpdateActiveFlag(this, {0});", row["RouteDetailId"]));
				break;
		}
	}

	protected void gvRoutes_Init(object sender, EventArgs e)
	{
		SetRoutesGridConfiguration();
	}

	protected void gvRoutes_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		gvRoutes.PageIndex = e.NewPageIndex;
		setRoutes();
	}

	protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName == "DeleteRow")
		{
			int RouteId = Convert.ToInt32(e.CommandArgument);
			RunNonQueryFromStatic("ATM", "up_p_removeRouteDetail", UserName, RouteId);
			setRoutes();
		}
	}

	protected void gvRoutes_Sorting(object sender, GridViewSortEventArgs e)
	{
		if (e.SortExpression == RouteSortExpression)
		{
			RouteSortDir = (RouteSortDir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
		}
		else
		{
			RouteSortExpression = e.SortExpression;
			RouteSortDir = SortDirection.Ascending;
		}
		setRoutes();
	}
	protected void ddAddDriver_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddAddDriverHelper.SelectedIndex = 0;
	}
	protected void ddAddDriverHelper_SelectedIndexChanged(object sender, EventArgs e)
	{
		ddAddDriver.SelectedIndex = 0;
	}
	protected void ddDriver_SelectedIndexChanged(object sender, EventArgs e)
	{
		DataView dv = ATMDB.GetDataView("up_getRoutes", UserName, ddSygmaCenterNo.SelectedValue);
		AddSort(dv);
		AddNameFilter(dv);
		gvRoutes.PageSize = RowCountBar1.PageSize;
		gvRoutes.DataSource = dv;
		setRoutes();
		gvRoutes.DataBind();
	}

	protected void ddHelper_SelectedIndexChanged(object sender, EventArgs e)
	{
		DataView dv = ATMDB.GetDataView("up_getRoutes", UserName, ddSygmaCenterNo.SelectedValue);
		AddSort(dv);
		AddNameFilter(dv);
		gvRoutes.PageSize = RowCountBar1.PageSize;
		gvRoutes.DataSource = dv;
		setRoutes();
		gvRoutes.DataBind();

	}

	protected void ddDepartDay_SelectedIndexChanged(object sender, EventArgs e)
	{
		DataView dv = ATMDB.GetDataView("up_getRoutes", UserName, ddSygmaCenterNo.SelectedValue);
		AddSort(dv);
		AddNameFilter(dv);
		gvRoutes.PageSize = RowCountBar1.PageSize;
		gvRoutes.DataSource = dv;
		setRoutes();
		gvRoutes.DataBind();

	}

	private void SetRoutesGridConfiguration()
	{
		DataSet dsSelectedColumns = ATMDB.GetDataSet("up_p_getProfileCustom", UserName, "MANAGEROUTE");

		if ((dsSelectedColumns.Tables[0].Rows != null) && (dsSelectedColumns.Tables[0].Rows.Count > 0))
		{
			DataTable dtSelectedCols = JsonConvert.DeserializeObject<DataSet>(dsSelectedColumns.Tables[0].Rows[0]["Value"].ToString()).Tables[0];

			// Start Grid column reordering
			List<string> columnOrder = CreateColumnOrder(dtSelectedCols);

			for (int columnOrderCounter = columnOrder.Count - 1; columnOrderCounter >= 0; columnOrderCounter--)
			{
				for (int columnCounter = 0; columnCounter <= gvRoutes.Columns.Count - 1; columnCounter++)
				{
					if (gvRoutes.Columns[columnCounter].HeaderText == columnOrder[columnOrderCounter])
					{
						var gridColumn = gvRoutes.Columns[columnCounter];
						gvRoutes.Columns.RemoveAt(columnCounter);
						gvRoutes.Columns.Insert(0, gridColumn);
					}
				}
			}
			// Stop Grid column reordering

			// Start Grid column hiding
			for (int columnCounter = 0; columnCounter <= gvRoutes.Columns.Count - 1; columnCounter++)
			{
				DataRow selectedDataTableRow = dtSelectedCols.Select("ColumnIdentifier='" + gvRoutes.Columns[columnCounter].HeaderText + "'").FirstOrDefault();

				if (selectedDataTableRow == null)
				{
					gvRoutes.Columns[columnCounter].HeaderStyle.CssClass = "hiddencol";
					gvRoutes.Columns[columnCounter].ItemStyle.CssClass = "hiddencol";
				}
			}
			// End Grid column hiding
		}
	}

	private List<string> CreateColumnOrder(DataTable dtSelectedCols)
	{
		List<string> finalColumn = new List<string>();
		string[,] columns = PayrollCommon.ManageRouteGridColumns;

		DataTable dtSelectedColsOrdered = dtSelectedCols.AsEnumerable()
																						.OrderBy(r => int.Parse(r.Field<String>("DisplayOrder")))
																						.CopyToDataTable();

		foreach (DataRow row in dtSelectedColsOrdered.Rows)
		{
			finalColumn.Add(Convert.ToString(row["ColumnIdentifier"]));
		}

		for (int i = 0; i < columns.GetLength(0); i++)
		{
			DataRow selectedRow = dtSelectedColsOrdered.Select("ColumnIdentifier='" + columns[i, 1] + "'").FirstOrDefault();

			if (selectedRow == null)
			{
				finalColumn.Add(Convert.ToString(columns[i, 1]));
			}
		}

		return finalColumn;
	}

	//protected void cbHolidayRoute_CheckedChanged(object sender, EventArgs e)
	//{
	//    int sygmaCenterNo = Convert.ToInt32(ddSygmaCenterNo.SelectedValue);
	//    foreach (GridViewRow gvRow in gvRoutes.Rows)
	//    {
	//        CheckBox cbHolidayRoute = gvRow.FindControl("cbHolidayRoute") as CheckBox;
	//        TextBox txtAltRouteNo = (TextBox)gvRow.FindControl("txtAltRouteNo");
	//        DropDownList ddAltDepartDay = (DropDownList)gvRow.FindControl("ddAltDepartDay");
	//        DropDownList ddAltDepartTimeHour = (DropDownList)gvRow.FindControl("ddAltDepartTimeHour");
	//        DropDownList ddAltDepartTimeMin = (DropDownList)gvRow.FindControl("ddAltDepartTimeMin");
	//        TextBox txtAltDuration = (TextBox)gvRow.FindControl("txtAltDuration");
	//        if (cbHolidayRoute.Checked)
	//        {
	//            txtAltRouteNo.Enabled = true;
	//            ddAltDepartDay.Enabled = true;
	//            ddAltDepartTimeHour.Enabled = true;
	//            ddAltDepartTimeMin.Enabled = true;
	//            txtAltDuration.Enabled = true;
	//        }
	//        else
	//        {
	//            string routeNo = gvRoutes.Rows[gvRow.RowIndex].Cells[0].Text;
	//            txtAltRouteNo.Text = "";
	//            ddAltDepartDay.ClearSelection();
	//            ddAltDepartTimeHour.ClearSelection();
	//            ddAltDepartTimeMin.ClearSelection();
	//            txtAltDuration.Text = "";
	//            txtAltRouteNo.Enabled = false;
	//            ddAltDepartDay.Enabled = false;
	//            ddAltDepartTimeHour.Enabled = false;
	//            ddAltDepartTimeMin.Enabled = false;
	//            txtAltDuration.Enabled = false;
	//            RemoveAltRouteDetails(UserName, sygmaCenterNo, routeNo);
	//        }
	//    }
	//}

	//protected void RemoveAltRouteDetails(string UserName, int Center, string RouteNo)
	//{
	//    RunNonQueryFromStatic("ATM", "up_p_removeAltRouteDetails", UserName, Center, RouteNo);
	//}
}