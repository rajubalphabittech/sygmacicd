using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using atm;
using Sygma.Framework.ClosedXML.Excel;
using SygmaFramework;
using ATM.Payroll;
using Newtonsoft.Json;

public partial class Apps_ATM_Tools_ManageVehicleTrailers : ATMPage
{
	private DataTable dtVehicleType;
	private DataTable dtVehicleMake;
	private DataTable dtVehicleStatus;
	private DataTable dtVehicleObjectType;
	private DataTable dtYear;
	private DataTable dtReplacedYear;
	private DataTable dtSygmaCenter;
	protected override void LoadATMPage()
	{
		SetPageVariables();
		if (!IsPostBack)
		{
			VehicleTrailerRowCountBar1.PageSize = 50;
			LoadCenters();
		}

	}

	private void LoadCenters()
	{
		DataView dv = ATMDB.GetDataView("up_getCenters", UserName);
		ddProgSygmaCenterNo.DataSource = dv;
		ddProgSygmaCenterNo.DataBind();
		ddProgSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
		ddProgSygmaCenterNo.Items.Insert(1, new ListItem("All", "0"));
		if (ddProgSygmaCenterNo.Items.Count == 3)
		{
			ddProgSygmaCenterNo.SelectedIndex = 2;
			CenterSelectionIndexChanged();
		}
	}

	private void LoadYear()
	{
		dtYear = new DataTable();
		dtYear.Columns.Add("Id", typeof(int));
		dtYear.Columns.Add("Year", typeof(string));
		DataRow drFirstRow = dtYear.NewRow();
		drFirstRow["Id"] = 0;
		drFirstRow["Year"] = "Choose...";
		dtYear.Rows.Add(drFirstRow);
		for (int i = DateTime.Now.AddYears(-25).Year; i <= DateTime.Now.AddYears(2).Year; i++)
		{
			DataRow drNewRow = dtYear.NewRow();
			drNewRow["Id"] = i;
			drNewRow["Year"] = i;
			dtYear.Rows.Add(drNewRow);
		}
	}

	private void LoadReplacedYear()
	{
		dtReplacedYear = new DataTable();
		dtReplacedYear.Columns.Add("ReplacedId", typeof(string));
		dtReplacedYear.Columns.Add("ReplacedYear", typeof(string));
		DataRow drFirstRow = dtReplacedYear.NewRow();
		drFirstRow["ReplacedId"] = 0;
		drFirstRow["ReplacedYear"] = "Choose...";
		dtReplacedYear.Rows.Add(drFirstRow);
		for (int i = DateTime.Now.AddYears(-25).Year; i <= DateTime.Now.AddYears(2).Year; i++)
		{
			DataRow drNewRow = dtReplacedYear.NewRow();
			drNewRow["ReplacedId"] = "FY" + i.ToString().Substring(2);
			drNewRow["ReplacedYear"] = "FY" + i.ToString().Substring(2);
			dtReplacedYear.Rows.Add(drNewRow);
		}
	}

	private void SetPageVariables()
	{
		AddClientVariable("gUserName", UserName);
		CanManageCenter = Convert.ToBoolean(ATMDB.GetScalar("up_sec_CanManageCenter", UserName));
	}

	protected void ddProgSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
	{
		CenterSelectionIndexChanged();
	}

	protected void CenterSelectionIndexChanged()
	{
		if (ProgSortExpression == "")
		{
			ProgSortExpression = "VehicleName";
			ProgSortDir = SortDirection.Ascending;
		}
		SetVehicles();
	}

	private void SetVehicles()
	{
		if (ddProgSygmaCenterNo.SelectedIndex > 0)
		{
			DataView dvVehicles = ATMDB.GetDataView("up_getVehicles", UserName, ddProgSygmaCenterNo.SelectedValue);
			DataView dvTrailers = ATMDB.GetDataView("up_getTrailers", UserName, ddProgSygmaCenterNo.SelectedValue);
			DataTable dtVehicles = dvVehicles.ToTable();
			DataTable dtTrailers = dvTrailers.ToTable();
			dtVehicles.Merge(dtTrailers);
			dtVehicles.AcceptChanges();
			DataView dvVehicleTrailer = new DataView(dtVehicles);
			DataSet dsTypeMakeStatusObject = ATMDB.GetDataSet("up_getVehicle_Type_Make_Status_ObjectType");
			dtVehicleType = dsTypeMakeStatusObject.Tables[0];
			dtVehicleMake = dsTypeMakeStatusObject.Tables[1];
			dtVehicleStatus = dsTypeMakeStatusObject.Tables[2];
			dtVehicleObjectType = dsTypeMakeStatusObject.Tables[3];
			DataView dvSygmaCenter = ATMDB.GetDataView("up_getCenters");
			dtSygmaCenter = dvSygmaCenter.ToTable();
			LoadYear();
			LoadReplacedYear();
			AddSort(dvVehicleTrailer);
			AddNameFilter(dvVehicleTrailer);
			AddNameFilter(dvVehicles);
			AddNameFilter(dvTrailers);
			gvVT.PageSize = (VehicleTrailerRowCountBar1.PageSize > 0) ? VehicleTrailerRowCountBar1.PageSize : 50;
			gvVT.DataSource = dvVehicleTrailer;
			gvVT.DataBind();
			pnlProgression.Visible = true;
			VehicleTrailerRowCountBar1.VehiclesCount = dvVehicles.Count;
			VehicleTrailerRowCountBar1.TrailersCount = dvTrailers.Count;
			VehicleTrailerRowCountBar1.ItemCount = dvVehicles.Count + dvTrailers.Count;
		}
		else
		{
			pnlProgression.Visible = false;
		}
	}

	protected void VehicleTrailerRowCountBar1_PageSizeChanged(object sender, EventArgs e)
	{
		gvVT.PageSize = VehicleTrailerRowCountBar1.PageSize;
		SetVehicles();
	}

	private void AddSort(DataView dv)
	{
		if (ProgSortExpression != "")
			dv.Sort = string.Format("{0} {1}", ProgSortExpression, WebCommon.GetDBSortDirection(ProgSortDir));
	}

	private void AddNameFilter(DataView dv)
	{
		string[] vehicle = txtProgName.Text.Trim().Replace(".", "").Replace(",", "").Replace("'", "''").Replace("%", "").Replace("*", "").Split(' ');
		if (vehicle.Length > 0)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string id in vehicle)
			{
				if (sb.Length > 0)
					sb.Append(" OR ");
				sb.AppendFormat("VehicleName like '%{0}%'", id);
			}
			dv.RowFilter = sb.ToString();
		}
	}

	[WebMethod]
	public static void PM_SaveVehicleTrailerDetails(string userName, string vid, int fieldId, string value, int isVehicle)
	{
		RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerDetails", userName, vid, fieldId, value, isVehicle);
	}

	[WebMethod]
	public static void PM_SaveVehicleTrailerMake_Type_Status_ObjType(string userName, string vid, int fieldId, int id, int isVehicle)
	{
		RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerMake_Type_Status_ObjType", userName, vid, fieldId, id, isVehicle);
	}

	[WebMethod]
	public static void PM_SaveVehicleTrailerCenter(string userName, string vid, int id, int isVehicle)
	{
		DataSet dsVTCenterInfo = new DataSet();
		dsVTCenterInfo = GetDataSetFromStatic("ATM", "up_p_setVehicleTrailerCenter", userName, vid, id, isVehicle);
		//RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerCenter", userName, vid, id, isVehicle);
		InformUserCenterChanged(dsVTCenterInfo, isVehicle);
	}

	[WebMethod]
	public static void PM_SaveVehicleTrailerActiveChanged(string userName, string vid, bool isActive, int isVehicle)
	{
		RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerActiveChanged", userName, vid, isActive, isVehicle);
	}

	protected void txtProgName_TextChanged(object sender, EventArgs e)
	{
		SetVehicles();
	}

	protected void gvVT_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		switch (e.Row.RowType)
		{
			case DataControlRowType.DataRow:
				DataRowView row = (DataRowView)e.Row.DataItem;
				TextBox txtUnitAttached = (TextBox)e.Row.FindControl("txtUnitAttached");
				Label lblUnitAttached = (Label)e.Row.FindControl("lblUnitAttached");
				TextBox txtVIN = (TextBox)e.Row.FindControl("txtVIN");
				DropDownList ddReplacedYear = (DropDownList)e.Row.FindControl("ddReplacedYear");
				TextBox txtModel = (TextBox)e.Row.FindControl("txtModel");
				DropDownList ddYear = (DropDownList)e.Row.FindControl("ddYear");
				TextBox txtDescription = (TextBox)e.Row.FindControl("txtDescription");
				DropDownList ddObjType = (DropDownList)e.Row.FindControl("ddObjType");
				CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
				DropDownList ddType = (DropDownList)e.Row.FindControl("ddType");
				Label lblType = (Label)e.Row.FindControl("lblType");
				DropDownList ddSygmaCenter = (DropDownList)e.Row.FindControl("ddSygmaCenter");
				Label lblCenterNumber = (Label)e.Row.FindControl("lblCenterNumber");
				DropDownList ddMake = (DropDownList)e.Row.FindControl("ddMake");
				DropDownList ddStatus = (DropDownList)e.Row.FindControl("ddStatus");
				int isVehicle = Convert.ToInt32(row["IsVehicle"]);
				if (isVehicle != 1)
				{
					if (!Convert.IsDBNull(row["UnitAttachedId"]))
					{
						txtUnitAttached.Text = row["UnitAttachedId"].ToString();
					}
				}
				else
				{
					txtUnitAttached.Visible = false;
					lblUnitAttached.Visible = true;
				}


				ddSygmaCenter.Attributes.Add("data-id", row["VehicleId"].ToString());
				lblCenterNumber.Attributes.Add("data-id", row["VehicleId"].ToString());

				if (!Convert.IsDBNull(row["SygmaCenterNo"]))
				{
					ddSygmaCenter.DataSource = dtSygmaCenter;
					ddSygmaCenter.DataBind();


					var center = ddSygmaCenter.Items.FindByValue(row["SygmaCenterNo"].ToString());

					if (center != null) center.Selected = true;


					for (int i = 0; i < dtSygmaCenter.Rows.Count; i++)
					{
						if (ddSygmaCenter.Items[i].Value == row["SygmaCenterNo"].ToString())
						{
							ddSygmaCenter.Items[i].Selected = true;
							break;
						}
					}
				}
				else
				{
					ddSygmaCenter.Items.Clear();
					foreach (DataRow vm in dtSygmaCenter.Rows)
					{
						ListItem liVM = new ListItem(vm["Center"].ToString(), vm["SygmaCenterNo"].ToString());
						ddSygmaCenter.Items.Add(liVM);
					}
				}

				if (!Convert.IsDBNull(row["SygmaCenterNo"]))
				{
					lblCenterNumber.Text = row["SygmaCenterNo"].ToString();
				}
				else
				{
					lblCenterNumber.Text = "NA";
				}

				if (CanManageCenter)
				{
					e.Row.Cells[2].Enabled = true;
				}
				else
				{
					e.Row.Cells[2].Enabled = false;
				}

				if (!Convert.IsDBNull(row["Make"]))
				{
					ddMake.DataSource = dtVehicleMake;
					ddMake.DataBind();
					ddMake.Items.FindByValue(row["Make"].ToString()).Selected = true;
					for (int i = 0; i < dtVehicleMake.Rows.Count; i++)
					{
						if (ddMake.Items[i].Value == row["Make"].ToString())
						{
							ddMake.Items[i].Selected = true;
							break;
						}
					}
					ddMake.Items.Insert(0, new ListItem("Choose...", "0"));
				}
				else
				{
					ddMake.Items.Clear();
					ddMake.Items.Add(new ListItem("Choose...", "0"));
					foreach (DataRow vm in dtVehicleMake.Rows)
					{
						ListItem liVM = new ListItem(vm["VehicleMake"].ToString(), vm["MakeId"].ToString());
						ddMake.Items.Add(liVM);
					}
				}
				if (!Convert.IsDBNull(row["VIN"]))
				{
					txtVIN.Text = row["VIN"].ToString();
				}
				if (!Convert.IsDBNull(row["ReplacedYear"]))
				{
					ddReplacedYear.DataSource = dtReplacedYear;
					ddReplacedYear.DataBind();
					ddReplacedYear.Items.FindByValue(row["ReplacedYear"].ToString()).Selected = true;
					for (int i = 0; i < dtReplacedYear.Rows.Count; i++)
					{
						if (ddReplacedYear.Items[i].Value == row["ReplacedYear"].ToString())
						{
							ddReplacedYear.Items[i].Selected = true;
							break;
						}
					}
				}
				else
				{
					ddReplacedYear.DataSource = dtReplacedYear;
					ddReplacedYear.DataBind();
				}
				if (!Convert.IsDBNull(row["Model"]))
				{
					txtModel.Text = row["Model"].ToString();
				}
				if (isVehicle == 1)
				{
					if (!Convert.IsDBNull(row["Type"]))
					{
						ddType.DataSource = dtVehicleType;
						ddType.DataBind();
						ddType.Items.FindByValue(row["Type"].ToString()).Selected = true;
						for (int i = 0; i < dtVehicleType.Rows.Count; i++)
						{
							if (ddType.Items[i].Value == row["Type"].ToString())
							{
								ddType.Items[i].Selected = true;
								break;
							}
						}
						ddType.Items.Insert(0, new ListItem("Choose...", "0"));
					}
					else
					{
						ddType.Items.Clear();
						ddType.Items.Add(new ListItem("Choose...", "0"));
						foreach (DataRow vt in dtVehicleType.Rows)
						{
							ListItem liVT = new ListItem(vt["VehicleType"].ToString(), vt["TypeId"].ToString());
							ddType.Items.Add(liVT);
						}
					}
				}
				else
				{
					ddType.Visible = false;
					lblType.Visible = true;
				}
				if (!Convert.IsDBNull(row["Status"]))
				{
					ddStatus.DataSource = dtVehicleStatus;
					ddStatus.DataBind();
					ddStatus.Items.FindByValue(row["Status"].ToString()).Selected = true;
					for (int i = 0; i < dtVehicleStatus.Rows.Count; i++)
					{
						if (ddStatus.Items[i].Value == row["Status"].ToString())
						{
							ddStatus.Items[i].Selected = true;
							break;
						}
					}
					ddStatus.Items.Insert(0, new ListItem("Choose...", "0"));
				}
				else
				{
					ddStatus.Items.Clear();
					ddStatus.Items.Add(new ListItem("Choose...", "0"));
					foreach (DataRow vs in dtVehicleStatus.Rows)
					{
						ListItem liVS = new ListItem(vs["VehicleStatus"].ToString(), vs["StatusId"].ToString());
						ddStatus.Items.Add(liVS);
					}
				}
				if (!Convert.IsDBNull(row["Year"]))
				{
					ddYear.DataSource = dtYear;
					ddYear.DataBind();
					ddYear.Items.FindByValue(row["Year"].ToString()).Selected = true;
					for (int i = 0; i < dtYear.Rows.Count; i++)
					{
						if (ddYear.Items[i].Value == row["Year"].ToString())
						{
							ddYear.Items[i].Selected = true;
							break;
						}
					}
				}
				else
				{
					ddYear.DataSource = dtYear;
					ddYear.DataBind();
				}
				if (isVehicle == 1)
				{
					txtDescription.MaxLength = 200;
				}
				else
				{
					txtDescription.MaxLength = 25;
				}
				if (!Convert.IsDBNull(row["Description"]))
				{
					txtDescription.Text = row["Description"].ToString();
				}
				if (!Convert.IsDBNull(row["ObjectType"]))
				{
					ddObjType.DataSource = dtVehicleObjectType;
					ddObjType.DataBind();
					ddObjType.Items.FindByValue(row["ObjectType"].ToString()).Selected = true;
					for (int i = 0; i < dtVehicleObjectType.Rows.Count; i++)
					{
						if (ddObjType.Items[i].Value == row["ObjectType"].ToString())
						{
							ddObjType.Items[i].Selected = true;
							break;
						}
					}
					ddObjType.Items.Insert(0, new ListItem("Choose...", "0"));
				}
				else
				{
					ddObjType.Items.Clear();
					ddObjType.Items.Add(new ListItem("Choose...", "0"));
					foreach (DataRow vt in dtVehicleObjectType.Rows)
					{
						ListItem liVT = new ListItem(vt["VehicleObjectType"].ToString(), vt["ObjectTypeId"].ToString());
						ddObjType.Items.Add(liVT);
					}
				}
				if (!Convert.IsDBNull(row["Active"]))
				{
					chkActive.Checked = Convert.ToBoolean(row["Active"]);
				}
				txtUnitAttached.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDetails(this, 1, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddReplacedYear.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDetails(this, 2, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				txtModel.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDetails(this, 3, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddYear.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDetails(this, 4, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				txtDescription.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDetails(this, 5, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				txtVIN.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDetails(this, 6, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddMake.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerMake_Type_Status_ObjType(this, 1, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddType.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerMake_Type_Status_ObjType(this, 2, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddStatus.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerMake_Type_Status_ObjType(this, 3, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddObjType.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerMake_Type_Status_ObjType(this, 4, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddSygmaCenter.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerCenter(this, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				ddSygmaCenter.Attributes.Add("onfocus", "SaveOriginalCenter(this);");
				chkActive.Attributes.Add("onclick", string.Format("UpdateVehicleTrailerActiveChanged(this, {0}, {1});", row["VehicleId"].ToString(), isVehicle));
				break;
		}
	}

	protected void gvVehiclesTrailers_Init(object sender, EventArgs e)
	{
		SetRoutesGridConfiguration();
	}

	protected void gvVT_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		gvVT.PageIndex = e.NewPageIndex;
		SetVehicles();
	}

	protected void gvVT_Sorting(object sender, GridViewSortEventArgs e)
	{
		if (e.SortExpression == ProgSortExpression)
		{
			ProgSortDir = (ProgSortDir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
		}
		else
		{
			ProgSortExpression = e.SortExpression;
			ProgSortDir = SortDirection.Ascending;
		}
		SetVehicles();
	}

	//View States
	public string ProgSortExpression
	{
		get
		{
			if (ViewState["ProgSortExpression"] == null)
				ViewState.Add("ProgSortExpression", "");
			return (string)ViewState["ProgSortExpression"];
		}
		set { ViewState["ProgSortExpression"] = value; }
	}

	public SortDirection ProgSortDir
	{
		get
		{
			if (ViewState["ProgSortDir"] == null)
				ViewState.Add("ProgSortDir", SortDirection.Ascending);
			return (SortDirection)ViewState["ProgSortDir"];
		}
		set { ViewState["ProgSortDir"] = value; }
	}

	public bool CanManageCenter
	{
		get
		{
			if (ViewState["CanManageCenter"] == null)
				ViewState.Add("CanManageCenter", false);
			return (bool)ViewState["CanManageCenter"];
		}
		set { ViewState["CanManageCenter"] = value; }
	}

	//Add Export Button
	private static List<int> getDateTimeColumns(DataTable dt)
	{
		var list = new List<int>(dt.Columns.Count);

		list.AddRange(from DataColumn col in dt.Columns where isColumnDateTime(col) select col.Ordinal);

		list.TrimExcess();
		return list;
	}

	private static bool isColumnDateTime(DataColumn col)
	{
		return col.ColumnName.EndsWith("DateTime");
	}


	protected void btnExport_Click(object sender, ImageClickEventArgs e)
	{

		//try
		//{
		DataView dvVehicleTrailer = ATMDB.GetDataView("up_exportVehicleTrailers", UserName, ddProgSygmaCenterNo.SelectedValue);
		AddNameFilter(dvVehicleTrailer);


		object val = null;
		var textColumnNumbers = new List<int>();


		XLWorkbook book = new XLWorkbook();
		book.AddWorksheet("VT");

		var sheet = book.Worksheet(1);

		var table = dvVehicleTrailer.ToTable();
		var columnCount = table.Columns.Count;
		var rowCount = table.Rows.Count;


		var header = sheet.Range(1, 1, 1, columnCount);
		for (var c = 1; c <= columnCount; c++)
		{
			header.Cell(1, c).Value = table.Columns[c - 1].ColumnName;
		}
		header.Style.Font.Bold = true;

		var rowNumber = 2;
		var columnNumber = 1;
		var dateTimeCols = getDateTimeColumns(table);

		for (var r = 0; r < rowCount; r++)
		{
			for (var j = 0; j < columnCount; j++)
			{
				val = table.Rows[r][j];
				if (val != null)
				{
					var cell = sheet.Cell(rowNumber, columnNumber);
					if (val is DateTime)
					{
						cell.DataType = XLCellValues.DateTime;

						var dateFormat = cell.Style.NumberFormat.Format;

						cell.Value = (DateTime)val;
						if (string.IsNullOrEmpty(dateFormat.Replace("General", string.Empty)))
							cell.Style.NumberFormat.Format = (dateTimeCols.Contains(j))
									? "M/d/yyyy HH:mm"
									: "M/d/yyyy";
						else
							cell.Style.NumberFormat.Format = dateFormat;
					}
					else if (val is short || val is int || val is double || val is decimal || val is byte)
					{
						cell.Value = Convert.ToDouble(val);
					}
					else
					{
						var textVal = val.ToString().Trim();
						if (textVal != string.Empty)
						{
							cell.SetValue(textVal);
							if (!textColumnNumbers.Contains(columnNumber))
								textColumnNumbers.Add(columnNumber);
						}
					}
				}
				columnNumber++; //incrememnt the excel column
			}
			rowNumber++; //increment the excel row
			columnNumber = 1;
		}
		sheet.Columns().AdjustToContents();

		textColumnNumbers.ForEach(num =>
				sheet.Column(num).AddIgnoredError(false, false, false, false, false, false, true, false, false));

		const char PAD_CHAR = '0';
		var fileName = string.Concat("ATM_Vehicle_Trailer_Report_" + ddProgSygmaCenterNo.SelectedValue + "_" + DateTime.Now.Year,
										DateTime.Now.Month.ToString().PadLeft(2, PAD_CHAR),
										DateTime.Now.Day.ToString().PadLeft(2, PAD_CHAR), DateTime.Now.Hour.ToString().PadLeft(2, PAD_CHAR),
										DateTime.Now.Minute.ToString().PadLeft(2, PAD_CHAR), DateTime.Now.Second.ToString().PadLeft(2, PAD_CHAR),
										".xlsx");

		Web.StreamExcelDocument(book, fileName, Response);

		Response.Flush();
		//Response.End();
		HttpContext.Current.ApplicationInstance.CompleteRequest();


		//catch (Exception ex)
		//{

		//}
	}

	protected static void InformUserCenterChanged(DataSet dsVTCenterInfo, int isVehicle)
	{
		if (dsVTCenterInfo.Tables.Count > 0)
		{
			DataTable dtVTCenterInfo = dsVTCenterInfo.Tables[0];
			DataTable dtUserEmailsInfo = dsVTCenterInfo.Tables[1];
			var toEmails = dtUserEmailsInfo.Rows[0]["ToEmails"].ToString();
			var fromEmail = dtUserEmailsInfo.Rows[0]["FromEmail"].ToString();
			var ccAddresses = dtUserEmailsInfo.Rows[0]["CCEmails"].ToString();
			var bccAddresses = dtUserEmailsInfo.Rows[0]["BCCEmails"].ToString();
			string subject = "";
			string vt = "";
			if (Convert.ToBoolean(isVehicle))
			{
				vt = "Vehicle";
			}
			else
			{
				vt = "Trailer";
			}
			subject = String.Format("ATM - Center changed for {0}", vt);
			StringBuilder sbBody = new StringBuilder("Hi All,<br /><br />");
			sbBody.AppendLine("Please be informed, Center has been changed for the below " + vt + ".<br /><br />");
			sbBody.AppendLine("<Table border='1' cellspacing='0' cellpadding='5' style='width:dynamic; border-collapse:collapse; border:black 1px solid; font-size:15; font-family:calibri;'>");
			sbBody.AppendLine("<tr style='color:#FFFFFF; background: #5DADE2;'>");
			sbBody.AppendLine("<th>" + vt + " Name</th><th>Old Center</th><th>New Center</th></tr>");
			sbBody.AppendLine("<tr><td>" + dtVTCenterInfo.Rows[0]["VTName"].ToString() + "</td>");
			sbBody.AppendLine("<td>" + dtVTCenterInfo.Rows[0]["OldCenterDisplay"].ToString() + "</td>");
			sbBody.AppendLine("<td>" + dtVTCenterInfo.Rows[0]["NewCenterDisplay"].ToString() + "</td></tr>");
			sbBody.AppendLine("</Table>");
			sbBody.AppendLine("<br /><br />Please check and confirm the above change.<br /><br />");
			sbBody.AppendLine("<p style='font-size:15; font-family:calibri; font-style:italic'>");
			sbBody.AppendLine("<b>Note</b>: For any further queries please send an email to " + dtUserEmailsInfo.Rows[0]["QueryEmail"].ToString() + ".");
			sbBody.AppendLine("<br />Please do not reply to this email, this will not be monitored.</p>");
			sbBody.AppendLine("Thanks");
			Email.Send(toEmails, fromEmail, subject, sbBody.ToString(), System.Net.Mail.MailPriority.Normal, true, null, bccAddresses, ccAddresses);
		}
	}



	private void SetRoutesGridConfiguration()
	{
		DataSet dsSelectedColumns = ATMDB.GetDataSet("up_p_getProfileCustom", UserName, "MANAGEVEHICLESTRAILERS");

		if ((dsSelectedColumns.Tables[0].Rows != null) && (dsSelectedColumns.Tables[0].Rows.Count > 0))
		{
			DataTable dtSelectedCols = JsonConvert.DeserializeObject<DataSet>(dsSelectedColumns.Tables[0].Rows[0]["Value"].ToString()).Tables[0];

			// Start Grid column reordering
			List<string> columnOrder = CreateColumnOrder(dtSelectedCols);

			for (int columnOrderCounter = columnOrder.Count - 1; columnOrderCounter >= 0; columnOrderCounter--)
			{
				for (int columnCounter = 0; columnCounter <= gvVT.Columns.Count - 1; columnCounter++)
				{
					if (gvVT.Columns[columnCounter].HeaderText == columnOrder[columnOrderCounter])
					{
						var gridColumn = gvVT.Columns[columnCounter];
						gvVT.Columns.RemoveAt(columnCounter);
						gvVT.Columns.Insert(0, gridColumn);
					}
				}
			}
			// Stop Grid column reordering

			// Start Grid column hiding
			for (int columnCounter = 0; columnCounter <= gvVT.Columns.Count - 1; columnCounter++)
			{
				DataRow selectedDataTableRow = dtSelectedCols.Select("ColumnIdentifier='" + gvVT.Columns[columnCounter].HeaderText + "'").FirstOrDefault();

				if (selectedDataTableRow == null)
				{
					gvVT.Columns[columnCounter].HeaderStyle.CssClass = "hiddencol";
					gvVT.Columns[columnCounter].ItemStyle.CssClass = "hiddencol";
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
}