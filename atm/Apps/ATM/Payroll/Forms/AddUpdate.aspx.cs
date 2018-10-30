using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using ATM;
using ATM.Payroll;
using System.Text;
using System.IO;
using System.Web.Script.Services;
using atm;

public partial class Apps_ATM_Payroll_Forms_AddUpdate : ATMPage
{
	private int gFormId = 0;
	private int gNewlyAddedVehicle = 0;
	private bool gFormLoaded = false;
	private string gRouteDepartDate;
	private bool gHasHolidayPay;
	private const string DATETIME_FORMAT = "M/d/yyyy h:mm tt";
	private int isApprover = 0;
	private DataTable dtDrivers;
	private DataTable dtHelpers;
	//private DataTable dtEmployees;
	//private DataTable dtVehicles;
	//private DataTable dtTrailers;
	private int gSygmaCenterNo;
	private List<string> activeFormIDs;
	private string nextFormID;


	protected override void LoadATMPage()
	{
		//*For displaying the confirmation messages
		if (Session["message"] != null)
		{
			lblMessages.Text = Session["message"].ToString();
			Session["message"] = null;
			pnlLabelMessages.Visible = true;
		}
		else
		{
			lblMessages.Text = string.Empty;
		}
		//*
		SetPageVariables();
		DataSet dsIsApprover = ATMDB.GetDataSet("up_user_isApprover", UserName);
		isApprover = Convert.ToInt32(dsIsApprover.Tables[0].Rows[0][0].ToString());
		if (isApprover == 1 && Request.QueryString.Get("fid") == null)
		{
			Page.Response.Redirect("../../CustomErrors/401.aspx", true);

		}

		if (!IsPostBack)
		{
			SetPayScales();
			SetInitialForm();
		}

	}

	private void SetPayScales()
	{

	}

	private void SetPageVariables()
	{
		AddClientVariable("gUserName", UserName);
		if (Request.QueryString.Get("fid") != null)
		{
			gFormId = Convert.ToInt32(Request.QueryString.Get("fid"));

			setNextButton(false);
			SetFormLoaded();
		}
		else
		{
			setNextButton(true);
			lblRouteStatus.Text = "New";
		}

	}

	private void setNextButton(bool isNew)
	{
		//set up cached values
		activeFormIDs = (List<string>)Cache.Get("CachedPayrollFormList");
		if (activeFormIDs != null)
		{
			//determine if there is a next form and show or hide the next button depending
			int formIndex = activeFormIDs.IndexOf(gFormId.ToString());
			if (isNew || (formIndex == -1) || (formIndex >= activeFormIDs.Count - 1))
			{
				pnlNext.Visible = false;
				btnNext.Visible = false;
			}
			else
			{
				pnlNext.Visible = true;
				btnNext.Visible = true;
				nextFormID = activeFormIDs[formIndex + 1];
			}
		}
		else
		{
			pnlNext.Visible = false;
			btnNext.Visible = false;
		}

	}

	private void SetFormLoaded()
	{
		AddClientVariable("gFormId", gFormId.ToString());
		gFormLoaded = true;
		if (!IsPostBack && (gFormId != 0))
			lblFormId.Text = string.Format("#{0}", gFormId);
	}
	public void SettingsForApprover()
	{
		pnlPlan.Enabled = false;
		pnlEmployees.Enabled = false;
		pnlVehicles.Enabled = false;
		pnlBackhauls.Enabled = false;
		pnlTrailers.Enabled = false;

	}
	public void DisableAllControls()
	{

		foreach (Control ctrl in Page.Controls)
		{
			if (ctrl is TextBox) ((TextBox)(ctrl)).ReadOnly = true;
			else if (ctrl is Button) ((Button)(ctrl)).Visible = false;
			else if (ctrl is DropDownList) ((DropDownList)(ctrl)).Visible = false;
			else if (ctrl is ListBox) ((ListBox)(ctrl)).Visible = false;
			else if (ctrl is CheckBox) ((CheckBox)(ctrl)).Visible = false;
			else if (ctrl is CheckBoxList) ((CheckBoxList)(ctrl)).Visible = false;
			else if (ctrl is RadioButton) ((RadioButton)(ctrl)).Visible = false;
			else if (ctrl is RadioButtonList) ((RadioButtonList)(ctrl)).Visible = false;

		}
		ddAddRateType.Visible = false;
		txtAddTrailer.Visible = false;
		txtAddVehicle.Visible = false;
		txtAddEmployee.Visible = false;

		Image imgAddBackhaul = (Image)pnlBackhauls.FindControl("imgAddBackhaul");

		if (imgAddBackhaul != null)
		{
			imgAddBackhaul.Visible = false;
		}
		Image imgAddTFT = (Image)pnlAddTrailer.FindControl("imgAddTFT");
		if (imgAddTFT != null)
		{
			imgAddTFT.Visible = false;
		}
		Image imgAddVFT = (Image)pnlAddVehicle.FindControl("imgAddVFT");
		if (imgAddVFT != null)
		{
			imgAddVFT.Visible = false;
		}
		Image imgRemoveVFT = (Image)pnlAddVehicle.FindControl("imgRemoveVFT");
		if (imgRemoveVFT != null)
		{
			imgRemoveVFT.Visible = false;
		}
		HyperLink hlRemoveTFT = (HyperLink)pnlTrailers.FindControl("hlRemoveTFT");
		if (hlRemoveTFT != null)
		{
			hlRemoveTFT.Visible = false;
		}
		HyperLink hlAddRateTypeDialog = (HyperLink)upEmployees.FindControl("hlAddRateTypeDialog");
		if (hlAddRateTypeDialog != null)
		{
			hlAddRateTypeDialog.Visible = false;
		}
	}
	protected void Page_PreRender(object sender, EventArgs e)
	{
		if (IsPostBack) //if this is a post back then check to see if the valdiation summary has an server side failed validations that need displayed
			RegisterValidationSummaryMessageBox(ValidationSummary1);
	}

	private void SetInitialForm()
	{
		if (!gFormLoaded)
		{
			SetNewLists();
		}
		else
		{

			DataSet dsForm = ATMDB.GetDataSet("up_p_getForm", gFormId);
			dsForm.Tables[0].TableName = "Forms";
			dsForm.Tables[1].TableName = "Employees";
			dsForm.Tables[2].TableName = "Vehicles";
			dsForm.Tables[3].TableName = "Trailers";
			dsForm.Tables[4].TableName = "PayScales";
			dsForm.Tables[5].TableName = "Backhauls";
			dsForm.Tables[6].TableName = "CenterRateTypes";
			dsForm.Tables[7].TableName = "RateTypeCats";

			DataView dvForm = dsForm.Tables[0].DefaultView;
			if (dvForm.Count > 0)
			{
				DataRowView formRow = dvForm[0];
				Status = (FormStatus)Enum.Parse(typeof(FormStatus), formRow["StatusId"].ToString());
				gRouteDepartDate = Convert.ToDateTime(formRow["RouteDepartDate"]).ToShortDateString();
				gSygmaCenterNo = Convert.ToInt32(formRow["SygmaCenterNo"]);
				AddClientVariable("gRouteDepartDate", gRouteDepartDate);
				SetFormInfo(formRow);
				pnlAssets.Visible = true;
				SetActions();
				switch (this.Type)
				{
					case FormType.Regular:
						SetPlanInfo(formRow, false);
						goto default;
					case FormType.Special:
					case FormType.Line:
						SetPlanInfo(formRow, IsOpen);
						goto default;
					case FormType.Miscellaneous:
						pnlEmployees.Visible = true;
						SetFormList("Employee", dsForm.Tables["Employees"], rptEmployees, txtAddEmployee, lblEmptyEmployees);
						SetAddRateType(dsForm);
						break;

					default:
						pnlEmployees.Visible = true;
						pnlVehicles.Visible = true;
						pnlTrailers.Visible = true;
						pnlDrivers.Visible = true;

						SetFormList("Employee", dsForm.Tables["Employees"], rptEmployees, txtAddEmployee, lblEmptyEmployees);
						SetFormList("Vehicle", dsForm.Tables["Vehicles"], rptVehicles, txtAddVehicle, lblEmptyVehicles, "AddedToForm = 0 or IsExternal = 1", "IsExternal desc, WebDisplay asc");
						SetFormList("Trailer", dsForm.Tables["Trailers"], rptTrailers, txtAddTrailer, lblEmptyTrailers);
						SetBackhauls(dsForm.Tables["Backhauls"]);
						SetAddRateType(dsForm);
						SetHoursMinutes();
						SetDriverHelpers(formRow);
						break;
				}
				DataSet dsStatuslog = ATMDB.GetDataSet("up_getStatusLogs", gFormId);
				SetStatusLog(dsStatuslog);
				ddDriver.Attributes.Add("onchange", string.Format("UpdateDriverDetails(this, 1, {0});", gFormId));
				ddDriverHelper.Attributes.Add("onchange", string.Format("UpdateDriverDetails(this, 2, {0});", gFormId));
				lblRouteStarted.Visible = Convert.ToBoolean(ATMDB.GetScalar("up_p_isRouteStarted", gFormId));
				SetRouteStatus(formRow);
				DataSet dsRouteCategory = ATMDB.GetDataSet("up_p_getRouteCategory", formRow["LockedRouteCategory"]);
				if (dsRouteCategory.Tables[0].Rows.Count > 0)
				{
					lblRouteCategory.Text = dsRouteCategory.Tables[0].Rows[0][0].ToString();
				}
				else
				{
					lblRouteCategory.Text = "NA";
				}
				if (!Convert.IsDBNull(formRow["ActualDriverUpdateDate"]))
				{
					//lblActualDriverUpdated.Visible = true;
				}
				DataView dv = ATMDB.GetDataView("up_getCenters");
				ddlCenterList.DataSource = dv;
				ddlCenterList.DataBind();
				ddlCenterList.SelectedValue = gSygmaCenterNo.ToString();
				AddClientVariable("gCenter", gSygmaCenterNo.ToString());

				DataSet dsTelogisDetails = ATMDB.GetDataSet("up_p_getTelogisDetails", formRow["RouteNo"].ToString(), ((DateTime)formRow["RouteDepartDate"]).ToShortDateString(), formRow["SygmaCenterNo"]);
				DataTable dtTelogis = dsTelogisDetails.Tables[0];
				DataTable dtTSDriver = dsTelogisDetails.Tables[1];
				if (dtTelogis.Rows.Count > 0)
				{
					lblTPDriver.Text = dtTelogis.Rows[0][0].ToString();
					if (!Convert.IsDBNull(dtTelogis.Rows[0][1]))
					{
						lblRouteStartTime.Text = dtTelogis.Rows[0][1].ToString();
					}
					else
					{
						lblRouteStartTime.Text = "-NA-";
					}
					if (!Convert.IsDBNull(dtTelogis.Rows[0][2]))
					{
						lblRouteEndTime.Text = dtTelogis.Rows[0][2].ToString();
					}
					else
					{
						lblRouteEndTime.Text = "-NA-";
					}
					if (dtTSDriver.Rows.Count > 0)
					{
						lblTSDriver.Text = dtTSDriver.Rows[0][0].ToString();
					}
					else
					{
						lblTSDriver.Text = "-NA-";
					}
				}
				else
				{
					lblTPDriver.Text = "-NA-";
					lblTSDriver.Text = "-NA-";
					lblRouteStartTime.Text = "-NA-";
					lblRouteEndTime.Text = "-NA-";
				}

				DataSet dsOntime = ATMDB.GetDataSet("up_p_getOntimeRecordings", formRow["RouteNo"].ToString(), ((DateTime)formRow["RouteDepartDate"]).ToShortDateString(), formRow["SygmaCenterNo"]);
				DataTable dtOntime = dsOntime.Tables[0];

				if (Convert.ToInt32(dtOntime.Rows[0][1]) != 0)
				{
					lblOntimeStatus.Text = dtOntime.Rows[0][0].ToString();
					lblOntimeStatus.ToolTip = "Total Stops - " + dtOntime.Rows[0][1].ToString() + ", Completed Stops - " + dtOntime.Rows[0][2].ToString() + ", Pending Stops - " + dtOntime.Rows[0][3].ToString();
				}
				else
				{
					lblOntimeStatus.Text = "Not Recorded";
					lblOntimeStatus.ToolTip = "Route is not assigned in telogis";
				}
				//chris
				//DataSet dsAllDetails = ATMDB.GetDataSet("up_p_getAllDetailsForCenterByForm", gFormId);
				//dtEmployees = dsAllDetails.Tables[0];
				//dtVehicles = dsAllDetails.Tables[1];
				//dtTrailers = dsAllDetails.Tables[2];
				//DataTable dtTest = FilterExistingEmployees();
			}
		}
		SetFormInfoView();
	}



	private void SetRouteStatus(DataRowView formRow)
	{
		DataSet dsRouteStatus = ATMDB.GetDataSet("up_p_isRouteStarted", formRow["FormId"]);
		DataTable dtRouteStatus = dsRouteStatus.Tables[1];

		int Dispatched = !Convert.IsDBNull(dtRouteStatus.Rows[0]["IsDispatch"]) ? Convert.ToInt16(dtRouteStatus.Rows[0]["IsDispatch"]) : 0;
		int Active = !Convert.IsDBNull(dtRouteStatus.Rows[0]["IsActive"]) ? Convert.ToInt16(dtRouteStatus.Rows[0]["IsActive"]) : 0;
		int Complete = !Convert.IsDBNull(dtRouteStatus.Rows[0]["IsComplete"]) ? Convert.ToInt16(dtRouteStatus.Rows[0]["IsComplete"]) : 0;
		int Archived = !Convert.IsDBNull(dtRouteStatus.Rows[0]["IsArchived"]) ? Convert.ToInt16(dtRouteStatus.Rows[0]["IsArchived"]) : 0;
		if (Dispatched == 1)
		{
			lblRouteStatus.Text = "Route Not Started";
		}
		if (Active == 1)
		{
			lblRouteStatus.Text = "Route In Progress";
		}
		if (Complete == 1 || Archived == 1)
		{
			lblRouteStatus.Text = "Route Completed";
		}

	}

	private void SetDriverHelpers(DataRowView formRow)
	{

		DataSet dsDrivers = ATMDB.GetDataSet("up_p_getDrivers", formRow["SygmaCenterNo"]);
		dtDrivers = dsDrivers.Tables[0];
		DataSet dsHelpers = ATMDB.GetDataSet("up_p_getDriverHelpers", formRow["SygmaCenterNo"]);
		dtHelpers = dsHelpers.Tables[0];

		//lblStatus.Text = formRow["StatusDescription"].ToString();
		ddDriver.DataSource = dtDrivers;
		ddDriver.DataBind();
		ddDriver.Items.Insert(0, new ListItem("Choose...", "0"));
		ddDriverHelper.DataSource = dtHelpers;
		ddDriverHelper.DataBind();
		ddDriverHelper.Items.Insert(0, new ListItem("Choose...", "0"));


		bool routeStarted = Convert.ToBoolean(ATMDB.GetScalar("up_p_isRouteStarted", formRow["FormId"]));
		if (!Convert.IsDBNull(formRow["DriverId"]))
		{
			long driverId = Convert.ToInt64(formRow["DriverId"]);
			for (int i = 0; i < dtDrivers.Rows.Count; i++)
			{
				if (ddDriver.Items[i].Value == formRow["DriverId"].ToString())
				{
					ddDriver.Items[i].Selected = true;
				}
			}
			if (ddDriver.SelectedItem.Text == "Choose...")
			{
				string driverName = (string)ATMDB.GetScalar("up_p_getDriversNameForInactive", driverId);
				ddDriver.Items.Insert(0, new ListItem("** " + driverName + " **", "999"));
				ddDriver.ClearSelection();
				ddDriver.Items.FindByValue("999").Selected = true;
				ddDriver.BackColor = System.Drawing.Color.Gray;
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
			ddDriverHelper.Enabled = false;
		}
		if (!Convert.IsDBNull(formRow["DriverHelperId"]))
		{
			long helperId = Convert.ToInt64(formRow["DriverHelperId"]);
			for (int i = 0; i < dtHelpers.Rows.Count; i++)
			{
				if (ddDriverHelper.Items[i].Value == formRow["DriverHelperId"].ToString())
				{
					ddDriverHelper.Items[i].Selected = true;
				}
			}
			if (ddDriverHelper.SelectedItem.Text == "Choose...")
			{
				string helperName = (string)ATMDB.GetScalar("up_p_getDriversNameForInactive", helperId);
				ddDriverHelper.Items.Insert(0, new ListItem("** " + helperName + " **", "999"));
				ddDriverHelper.ClearSelection();
				ddDriverHelper.Items.FindByValue("999").Selected = true;
				ddDriverHelper.BackColor = System.Drawing.Color.Gray;
			}
		}
		else
		{
			ddDriverHelper.Items.Clear();
			ddDriverHelper.Items.Add(new ListItem("Choose...", "0"));
			foreach (DataRow rtd in dtHelpers.Rows)
			{
				ListItem liRTD = new ListItem(rtd["DriverName"].ToString(), rtd["DriverId"].ToString());
				ddDriverHelper.Items.Add(liRTD);
			}
			//ddDriverHelper.Enabled = false;
		}

		if (ddDriver.Items.Count > 0 && ddDriverHelper.Items.Count > 0)
		{
			string DriverSel = ddDriver.SelectedItem.ToString();
			if (DriverSel.Equals("Choose...") == false)
			{
				if (ddDriverHelper.Items.Contains(ddDriver.SelectedItem))
				{
					ddDriverHelper.Items.Remove(ddDriver.SelectedItem);
				}
			}
			string HelperSel = ddDriverHelper.SelectedItem.ToString();
			if (HelperSel.Equals("Choose...") == false)
			{
				if (ddDriver.Items.Contains(ddDriverHelper.SelectedItem))
				{
					ddDriver.Items.Remove(ddDriverHelper.SelectedItem);
				}
			}
		}
		//int a1 = (!Convert.IsDBNull(formRow["DriverId"])) ? Convert.ToInt32(formRow["DriverId"]) : 0;
		//int b1 = (!Convert.IsDBNull(formRow["DriverHelperId"])) ? Convert.ToInt32(formRow["DriverId"]) : 0;
		//if (((!Convert.IsDBNull(formRow["DriverId"])) ? Convert.ToInt32(formRow["DriverId"]) : 0) > 0)
		//{
		//    //if (ddDriver.SelectedItem.Value != "Choose...")
		//    //{
		//    //    if (ddDriverHelper.Items.Contains(ddDriver.SelectedItem))
		//    //    {
		//            ddDriverHelper.Items.Remove(ddDriver.SelectedItem);
		//    //    }
		//    //}
		//}
		//if (((!Convert.IsDBNull(formRow["DriverHelperId"])) ? Convert.ToInt32(formRow["DriverHelperId"]) : 0) > 0)
		//{
		//    //if (ddDriverHelper.SelectedItem.Value != "Choose...")
		//    //{
		//    //    if (ddDriver.Items.Contains(ddDriverHelper.SelectedItem))
		//    //    {
		//            ddDriver.Items.Remove(ddDriverHelper.SelectedItem);
		//    //    }
		//    //}
		//}
		if (!routeStarted)
		{
			switch (Status)
			{
				case FormStatus.Out:
					ddDriver.Enabled = true;
					if (Convert.IsDBNull(formRow["DriverId"]))
					{
						ddDriverHelper.Enabled = false;
					}
					else
					{
						ddDriverHelper.Enabled = true;
					}
					break;
				default:
					ddDriver.Enabled = false;
					ddDriverHelper.Enabled = false;
					break;

			}
		}
		else
		{
			ddDriver.Enabled = false;
			ddDriverHelper.Enabled = false;
		}
		if (Convert.ToBoolean(ATMDB.GetScalar("up_p_IsADPUser", UserName)))
		{
			ddDriver.Enabled = false;
			ddDriverHelper.Enabled = false;
		}

	}

	private void SetStatusLog(DataSet ds)
	{
		DataView dvStatusLog = ds.Tables[0].DefaultView;
		if (dvStatusLog.Count > 0)
		{
			gvStatusLog.DataSource = dvStatusLog;
			gvStatusLog.DataBind();
			pnlStatusLog.Visible = true;
		}
	}


	private void SetActions()
	{

	}
	/// <summary>
	/// To Set the image for status
	/// </summary>
	/// <param name="formRow"></param>
	private void SetFormInfo(DataRowView formRow)
	{

		switch (Status)
		{
			case FormStatus.Out:
				((Panel)(pnlActions.FindControl("pnlChangeStatusAppr"))).Visible = false;
				((Panel)(pnlActions.FindControl("pnlReject"))).Visible = false;
				if (isApprover == 1)
				{

					((Panel)(pnlActions.FindControl("pnlDelete"))).Visible = false;
					pnlChangeStatus.Visible = false;
					lblStatus.Text = formRow["StatusDescription"].ToString();
					imgStatus.ImageUrl = string.Format("~/Images/Icons/{0}", "smallredcheck.png");
					Status = FormStatus.In;
				}
				else
				{
					SetStatusDisplay(formRow["StatusDescription"].ToString(), FormStatus.In, "smallredcheck.png");

				}
				break;
			case FormStatus.In:
				if (isApprover == 1)
				{
					pnlChangeStatus.Visible = false;
					((Panel)(pnlActions.FindControl("pnlChangeStatusAppr"))).Visible = true;
					((Panel)(pnlActions.FindControl("pnlReject"))).Visible = true;
					lblStatus.Text = formRow["StatusDescription"].ToString();
					imgStatus.ImageUrl = string.Format("~/Images/Icons/{0}", "smalllock.png");
					((Panel)(pnlActions.FindControl("pnlDelete"))).Visible = false;
				}
				else
				{
					SetStatusDisplay(formRow["StatusDescription"].ToString(), FormStatus.Out, "smalllock.png");
					((Panel)(pnlActions.FindControl("pnlChangeStatusAppr"))).Visible = false;
					((Panel)(pnlActions.FindControl("pnlReject"))).Visible = false;
				}
				break;
			case FormStatus.Submitted:
				SetStatusDisplay(formRow["StatusDescription"].ToString(), FormStatus.Submitted, "smalllock.png");
				break;
			case FormStatus.Approve:
				pnlChangeStatus.Visible = false;
				((Panel)(pnlActions.FindControl("pnlChangeStatusAppr"))).Visible = false;
				((Panel)(pnlActions.FindControl("pnlDelete"))).Visible = false;
				if (isApprover == 1 && Convert.ToInt32(formRow["isAdpRan"]) == 0)
				{
					((Panel)(pnlActions.FindControl("pnlReject"))).Visible = true;
				}
				else
				{
					((Panel)(pnlActions.FindControl("pnlReject"))).Visible = false;
				}
				pnlChangeStatus.Visible = false;
				lblStatus.Text = formRow["StatusDescription"].ToString();
				imgStatus.ImageUrl = string.Format("~/Images/Icons/{0}", "Approved.png");
				break;
			case FormStatus.Reject:
				if (isApprover == 1)
				{
					((Panel)(pnlActions.FindControl("pnlDelete"))).Visible = false;
					lblStatus.Text = formRow["StatusDescription"].ToString();
					imgStatus.ImageUrl = string.Format("~/Images/Icons/{0}", "Rejected.png");
					pnlChangeStatus.Visible = false;
				}
				else
				{
					SetStatusDisplay(formRow["StatusDescription"].ToString(), FormStatus.Out, "Rejected.png");
					((Panel)(pnlActions.FindControl("pnlDelete"))).Visible = true;
				}
						((Panel)(pnlActions.FindControl("pnlChangeStatusAppr"))).Visible = false;
				((Panel)(pnlActions.FindControl("pnlReject"))).Visible = false;
				break;
			default:

				break;
		}


		lblFormType.Text = formRow["FormTypeDescription"].ToString();
		Type = (FormType)Enum.Parse(typeof(FormType), formRow["FormTypeId"].ToString());
		lblSygmaCenter.Text = formRow["CenterDisplay"].ToString();
		lblRouteNo.Text = formRow["RouteNo"].ToString();
		lblDepartDate.Text = ((DateTime)formRow["RouteDepartDate"]).ToShortDateString();
		lblWeekending.Text = ((DateTime)formRow["FiscalWeekending"]).ToShortDateString();
		dteFormWeekendingDate.Text = ((DateTime)formRow["FiscalWeekending"]).ToShortDateString();
		DateTime lastUpdated = (!Convert.IsDBNull(formRow["LastUpdatedDate"])) ? (DateTime)formRow["LastUpdatedDate"] : (DateTime)formRow["AddedDate"];
		lblLastUpdated.Text = lastUpdated.ToString(DATETIME_FORMAT);

		txtNotes.Text = formRow["Notes"].ToString();
		gvNotesDataBind();
	}

	private void SetStatusDisplay(string desc, FormStatus newStatus, string imageFile)
	{
		lblStatus.Text = desc;
		imgStatus.ImageUrl = string.Format("~/Images/Icons/{0}", imageFile);
		if (newStatus != Status)
		{
			pnlChangeStatus.Visible = true;

			string changeScript = string.Format("ChangeStatus({0});return false;", (int)newStatus);
			hlChangeStatus.Attributes.Add("onclick", changeScript);
			string newstat = newStatus.ToString();
			if (((int)newStatus) < 2)
			{
				newstat = "Check " + newStatus.ToString();
			}
			hlChangeStatus.Text = string.Format("{0}", newstat);
		}
		else
		{
			pnlChangeStatus.Visible = false;
		}
	}

	private void SetPlanInfo(DataRowView formRow, bool isEditable)
	{
		pnlPlan.Visible = true;
		SetMetricField(txtCases, lblCases, formRow["Cases"], isEditable);
		SetMetricField(txtPounds, lblPounds, formRow["Pounds"], isEditable);
		SetMetricField(txtCubes, lblCubes, formRow["Cubes"], isEditable);
		SetMetricField(txtStops, lblStops, formRow["Stops"], isEditable);
		lblMiles.Text = (formRow["Miles"]).ToString();
	}

	private void SetMetricField(TextBox txt, Label lbl, object dbVal, bool isEditable)
	{
		if (isEditable)
		{
			txt.Text = Convert.ToInt32(dbVal).ToString("N0");
			txt.Visible = true;
			lbl.Visible = false;
		}
		else
		{
			lbl.Text = Convert.ToInt32(dbVal).ToString("N0");
			txt.Visible = false;
			lbl.Visible = true;
		}
	}

	private void SetMetricField(TextBox txt, Label lbl, object dbActualVal, object dbPlannedVal, object dbAUVal)
	{
		string actual = (Convert.ToInt32(dbActualVal) == 0 && Convert.ToInt32(dbAUVal) == 0) ? "-" : Convert.ToInt32(dbActualVal).ToString("N0");
		string planned = Convert.IsDBNull(dbPlannedVal) ? "-" : Convert.ToInt32(dbPlannedVal).ToString("N0");
		lbl.Text = planned + "/" + actual;
		txt.Visible = false;
		lbl.Visible = true;
	}

	private void SetBackhauls()
	{
		DataSet ds = ATMDB.GetDataSet("up_p_getBackhauls", gFormId);
		SetBackhauls(ds.Tables[0]);
	}

	private void SetBackhauls(DataTable dt)
	{
		pnlBackhauls.Visible = true;
		if (dt.Rows.Count > 0)
		{
			lblEmptyBackhauls.Visible = false;
			rptBackhauls.Visible = true;
			rptBackhauls.DataSource = dt.DefaultView;
			rptBackhauls.DataBind();
		}
		else
		{
			lblEmptyBackhauls.Visible = true;
			rptBackhauls.Visible = false;
		}
		imgAddBackhaul.Visible = IsOpen;
	}
	private void SetEmployees()
	{
		DataSet ds = ATMDB.GetDataSet("up_p_getEmployees", gFormId);
		ds.Tables[0].TableName = "Employees";
		ds.Tables[1].TableName = "PayScales";
		ds.Tables[2].TableName = "CenterRateTypes";
		SetFormList("Employee", ds.Tables["Employees"], rptEmployees, txtAddEmployee, lblEmptyEmployees);
		SetAddRateType(ds);
	}

	private void SetVehicles()
	{
		DataSet ds = ATMDB.GetDataSet("up_p_getVehicles", gFormId);
		ds.Tables[0].TableName = "Vehicles";
		SetFormList("Vehicle", ds.Tables["Vehicles"], rptVehicles, txtAddVehicle, lblEmptyVehicles, "AddedToForm = 0 or IsExternal = 1", "IsExternal desc, WebDisplay asc");
	}

	private void SetTrailers()
	{
		DataSet ds = ATMDB.GetDataSet("up_p_getTrailers", gFormId);
		ds.Tables[0].TableName = "Trailers";
		SetFormList("Trailers", ds.Tables["Trailers"], rptTrailers, txtAddTrailer, lblEmptyTrailers);
	}
	private void SetFormList(string entityName, DataTable dt, Repeater rpt, TextBox txt, Label lblEmpty)
	{
		SetFormList(entityName, dt, rpt, txt, lblEmpty, "AddedToForm = 0", "WebDisplay");
	}

	private void SetFormList(string entityName, DataTable dt, Repeater rpt, TextBox txt, Label lblEmpty, string newFilter, string newSort)
	{
		DataView dvAdded = new DataView(dt, "AddedToForm = 1", "SortDate asc", DataViewRowState.CurrentRows);
		if (dvAdded.Count > 0)
		{
			rpt.DataSource = dvAdded;
			rpt.DataBind();
			lblEmpty.Visible = false;
			rpt.Visible = true;
		}
		else
		{
			rpt.DataSource = null;
			rpt.DataBind();
			lblEmpty.Visible = true;
			rpt.Visible = false;
		}
		if (IsOpen)
		{
			txt.Visible = true;
		}
		else
		{
			txt.Parent.Visible = false;
		}
	}

	private void SetHoursMinutes()
	{

		Dictionary<int, string> Hours = new Dictionary<int, string>();
		for (int i = 1; i < 13; i++)
		{
			Hours.Add(i, i.ToString());
		}
		ddAddStartHour.DataSource = Hours;
		ddAddStartHour.DataValueField = "Key";
		ddAddStartHour.DataTextField = "Value";
		ddAddStartHour.DataBind();
		ddAddEndHour.DataSource = Hours;
		ddAddEndHour.DataValueField = "Key";
		ddAddEndHour.DataTextField = "Value";
		ddAddEndHour.DataBind();
		Dictionary<int, string> Minutes = new Dictionary<int, string>();
		for (int i = 0; i < 60; i++)
		{
			Minutes.Add(i, i.ToString().PadLeft(2, '0'));
		}
		ddAddStartMin.DataSource = Minutes;
		ddAddStartMin.DataValueField = "Key";
		ddAddStartMin.DataTextField = "Value";
		ddAddStartMin.DataBind();
		ddAddEndMin.DataSource = Minutes;
		ddAddEndMin.DataValueField = "Key";
		ddAddEndMin.DataTextField = "Value";
		ddAddEndMin.DataBind();
		Dictionary<string, string> AMPM = new Dictionary<string, string>();
		AMPM.Add("AM", "AM");
		AMPM.Add("PM", "PM");
		ddAddStartAmPm.DataSource = AMPM;
		ddAddStartAmPm.DataValueField = "Key";
		ddAddStartAmPm.DataTextField = "Value";
		ddAddStartAmPm.DataBind();
		ddAddEndAmPm.DataSource = AMPM;
		ddAddEndAmPm.DataValueField = "Key";
		ddAddEndAmPm.DataTextField = "Value";
		ddAddEndAmPm.DataBind();
	}

	private void SetAddRateType(DataSet ds)
	{
		DataView dvEmployees = new DataView(ds.Tables["Employees"], "AddedToForm = 1", "SortDate desc", DataViewRowState.CurrentRows);
		if (dvEmployees.Count > 0)
		{
			hlAddRateTypeDialog.Visible = true;
			lbAddRateApplyTos.DataSource = dvEmployees;
			lbAddRateApplyTos.DataBind();
			if (dvEmployees.Count > 1)
			{
				ListItem liAll = new ListItem("All that Apply", "");
				liAll.Selected = true;
				lbAddRateApplyTos.Items.Insert(0, liAll);

			}
		}
		else
		{
			hlAddRateTypeDialog.Visible = false;
		}
		if (ds.Tables["CenterRateTypes"] != null)
		{
			ddAddRateType.Items.Clear();
			ddAddRateType.Items.Add(new ListItem("Choose...", ""));
			ddAddRateType.Items[0].Attributes.Add("hasCategories", "false");
			ddAddRateType.Items[0].Attributes.Add("requireNote", "false");
			ddAddRateType.Items[0].Attributes.Add("allowQuarters", "1");
			ddAddRateType.Items[0].Attributes.Add("max", "");
			foreach (DataRow rt in ds.Tables["CenterRateTypes"].Rows)
			{
				ListItem li = new ListItem(rt["RateTypeDescription"].ToString(), rt["RateTypeId"].ToString());
				li.Attributes.Add("hasCategories", Convert.ToBoolean(rt["hasCategories"]).ToString().ToLower());
				li.Attributes.Add("requireNote", Convert.ToBoolean(rt["RequireNote"]).ToString().ToLower());
				li.Attributes.Add("allowQuarters", rt["AllowQuarters"].ToString());
				li.Attributes.Add("max", rt["max"].ToString());
				ddAddRateType.Items.Add(li);
			}

			if (ds.Tables["RateTypeCats"] != null)
			{
				ddAddRateTypeCategory.Items.Add(new ListItem("Choose...", ""));
				foreach (DataRow rtc in ds.Tables["RateTypeCats"].Rows)
				{
					ListItem liRTC = new ListItem(rtc["CategoryDescription"].ToString(), rtc["CategoryId"].ToString());
					liRTC.Attributes.Add("rateTypeId", rtc["RateTypeId"].ToString());
					ddAddRateTypeCategory.Items.Add(liRTC);
				}
			}
		}
		if (ScriptManager1.IsInAsyncPostBack)
		{
			upAddRateType.Update();
			upAddRateApplyTos.Update();
		}
	}


	private void SetFormInfoView()
	{
		pnlStatus.Visible = gFormLoaded;
		lblFormType.Visible = gFormLoaded;
		lblSygmaCenter.Visible = gFormLoaded;
		lblRouteNo.Visible = gFormLoaded;
		lblDepartDate.Visible = gFormLoaded;
		dteFormWeekendingDate.Visible = gFormLoaded;

		ddFormType.Visible = !gFormLoaded;
		ddSygmaCenterNo.Visible = !gFormLoaded;
		pnlRouteNoEdit.Visible = !gFormLoaded;
		pnlPlanInfo.Visible = !gFormLoaded;
		dteDepartDate.Visible = !gFormLoaded;
		dteWeekendingDate.Visible = !gFormLoaded;

		if (gFormLoaded && IsOpen)
		{
			lblWeekending.Style["display"] = "none";
			dteFormWeekendingDate.Visible = true;
		}
		else if (gFormLoaded && !IsOpen)
		{
			lblWeekending.Style["display"] = "block";
			dteFormWeekendingDate.Visible = false;
		}

		if (!gFormLoaded)
		{
			pnlWeekendingDate.Style.Add(HtmlTextWriterStyle.PaddingTop, "2px");
		}
		else
		{
			pnlWeekending.Style.Add(HtmlTextWriterStyle.Display, "block");
			pnlWeekendingDate.Style.Add(HtmlTextWriterStyle.PaddingTop, "0px");
			pnlCreateButtons.Visible = false;
		}
	}

	//should only be called when it is a "new form"
	private void SetNewLists()
	{
		DataSet ds = ATMDB.GetDataSet("up_p_getFormCriteria", UserName);

		DataView dvFT = ds.Tables[1].DefaultView;
		dvFT.RowFilter = "Editable = 1";
		ddFormType.DataSource = dvFT;
		ddFormType.DataBind();

		ddFormType.Visible = true;
		lblFormType.Visible = false;

		ddSygmaCenterNo.DataSource = ds.Tables[2].DefaultView;
		ddSygmaCenterNo.DataBind();
		if (ds.Tables[2].Rows.Count == 1)
		{
			ddSygmaCenterNo.Items[0].Selected = true;
		}
		else
		{
			ddSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
		}
		ddSygmaCenterNo.Visible = true;
		lblSygmaCenter.Visible = false;
		pnlChangeStatus.Visible = false;
		pnlNotes.Visible = false;
		pnlActions.Visible = false;
		pnlStatusLine.Visible = false;
	}

	public FormType Type
	{
		get
		{
			if (ViewState["Type"] == null)
				ViewState.Add("Type", FormType.Regular);
			return (FormType)ViewState["Type"];
		}
		set { ViewState["Type"] = value; }
	}
	public FormStatus Status
	{
		get
		{
			if (ViewState["Status"] == null)
				ViewState.Add("Status", FormStatus.Out);
			return (FormStatus)ViewState["Status"];
		}
		set { ViewState["Status"] = value; }
	}

	public bool IsOpen
	{
		get
		{

			return (Status == FormStatus.Out);
		}
	}


	public string RouteNoEdit
	{
		get
		{
			if (ddFormType.SelectedItem.Text.Substring(0, 1) == "R")
			{
				return txtRouteNo.Text;
			}
			else if (ddFormType.SelectedItem.Text.Substring(0, 1) == "L")
			{
				return string.Concat("H", txtRouteNo.Text);
			}
			else
			{
				return string.Concat(ddFormType.SelectedItem.Text.Substring(0, 1), txtRouteNo.Text);
			}
		}
	}


	protected void btnCreate_Click(object sender, EventArgs e)
	{
		if (IsValid)
		{
			if (Convert.ToInt32(ddFormType.SelectedValue) == 0 || Convert.ToInt32(ddFormType.SelectedValue) == 1)
			{
				gFormId = Convert.ToInt32(ATMDB.GetScalar("up_p_createForm", UserName, ddFormType.SelectedValue, ddSygmaCenterNo.SelectedValue, RouteNoEdit, dteDepartDate.Text, dteWeekendingDate.Text,
						Convert.ToInt32(txtCubesOnCreate.Text), Convert.ToInt32(txtPoundsOnCreate.Text), Convert.ToInt32(txtCasesOnCreate.Text), Convert.ToInt32(txtStopsOnCreate.Text)));
			}
			else
			{
				gFormId = Convert.ToInt32(ATMDB.GetScalar("up_p_createForm", UserName, ddFormType.SelectedValue, ddSygmaCenterNo.SelectedValue, RouteNoEdit, dteDepartDate.Text, dteWeekendingDate.Text));
			}
			RedirectToSelf("fid", gFormId.ToString());
		}
		else
		{
			if (lblWeekending.Text == "")
			{
				pnlWeekending.Style.Add(HtmlTextWriterStyle.Display, "block"); //put this here so that the weekending date doesn't dissapear
				if (dteWeekendingDate.Text != "")
					lblWeekending.Text = Convert.ToDateTime(dteWeekendingDate.Text).ToShortDateString();
				dteFormWeekendingDate.Text = Convert.ToDateTime(dteWeekendingDate.Text).ToShortDateString();
			}
		}
	}

	protected void cuvRouteNo_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (IsValid)
		{
			args.IsValid = (bool)ATMDB.GetScalar("up_p_isRouteNoValid", ddSygmaCenterNo.SelectedValue, RouteNoEdit, Convert.ToDateTime(dteWeekendingDate.Text));
		}
	}

	protected void btnRefreshEmployees_Click(object sender, EventArgs e)
	{
		SetEmployees();
		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusAddPaymentOnPayScaleChange();", true);
	}

	protected void btnRefreshAddEmployees_Click(object sender, EventArgs e)
	{
		SetEmployees();
		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "OpenPaymentDialog();", true);
	}

	protected void btnRatePreview_Click(object sender, EventArgs e)
	{
		DataView dvEmployeePreview;
		if (ddAddRateType.SelectedValue != "" && txtAddRateQuantity.Value != "" && txtAddRateQuantity.Value != "0")
		{
			dvEmployeePreview = ATMDB.GetDataView("up_p_getEmployeePayRatePreview", gFormId, ddAddRateType.SelectedValue, txtAddRateQuantity.Value, Convert.ToInt16(cbHolidayPay.Checked));
		}
		else
		{
			dvEmployeePreview = ATMDB.GetDataView("up_p_getEmployeePayRatePreview", gFormId, 0, 0, 0);
		}
		lbAddRateApplyTos.DataSource = dvEmployeePreview;
		lbAddRateApplyTos.DataBind();
		if (dvEmployeePreview.Count > 1)
		{
			ListItem liAll = new ListItem("All that Apply", "");
			liAll.Selected = true;
			lbAddRateApplyTos.Items.Insert(0, liAll);
		}
		if (ddAddRateType.SelectedValue != "")
		{
			//if (txtAddRateQuantity.Value != "")
			//{
			//ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusHolidayCheckbox();", true);
			//ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusAddRateQuantity();", true);
			//}
			//else
			//{
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusAddRateQuantity();", true);
			//}
		}
		else
		{
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusAddRateType();", true);
		}
	}

	protected void btnNext_Click(object sender, EventArgs e)
	{
		//If there are forms in your next queue and the current form is in that list
		if (activeFormIDs != null && nextFormID != null)
		{
			//Create the Url and load the next form
			string url = "";
			url = "?fid=" + nextFormID;
			string current = Request.Url.ToString();
			int indexToCut = current.IndexOf("?fid=");
			string newUrl = current.Substring(0, indexToCut);
			newUrl = newUrl + url;
			Response.Redirect(newUrl);
		}
		//Otherwise reload the current form b/c there isn't a valid next form
		else
		{
			Response.Redirect(Request.Url.ToString());
		}
	}


	protected void btnCategoryRefresh_Click(object sender, EventArgs e)
	{
		DataSet dsEmployeePreview;
		if (ddAddRateType.SelectedValue != "")
		{
			dsEmployeePreview = ATMDB.GetDataSet("up_p_getRtCategoriesAndPreview", gFormId, ddAddRateType.SelectedValue);
			ddAddRateTypeCategory.Items.Clear();
			ddAddRateTypeCategory.Items.Add(new ListItem("Choose...", ""));
			if (dsEmployeePreview.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow rtc in dsEmployeePreview.Tables[0].Rows)
				{
					ListItem liRTC = new ListItem(rtc["CategoryDescription"].ToString(), rtc["CategoryId"].ToString());
					liRTC.Attributes.Add("rateTypeId", rtc["RateTypeId"].ToString());
					ddAddRateTypeCategory.Items.Add(liRTC);
				}
			}
			lbAddRateApplyTos.DataSource = dsEmployeePreview.Tables[1];
			lbAddRateApplyTos.DataBind();
			if (dsEmployeePreview.Tables[1].Rows.Count > 1)
			{
				ListItem liAll = new ListItem("All that Apply", "");
				liAll.Selected = true;
				lbAddRateApplyTos.Items.Insert(0, liAll);
			}
		}
	}

	protected void btnRefreshBackhauls_Click(object sender, EventArgs e)
	{
		SetBackhauls();
		SetEmployees(); //the ratetypes can change
		if (hfOpenAddPayment.Value == "1")
		{
			hfOpenAddPayment.Value = "0";
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "OpenBackhaulPaymentDialog();", true);
		}
	}

	protected void btnAddEmployee_Click(object sender, EventArgs e)
	{
		if (hfAddEmployee.Value != "")
		{
			DataSet ds = ATMDB.GetDataSet("up_p_addEmployee", UserName, gFormId, hfAddEmployee.Value);
			string employeeId = hfAddEmployee.Value;
			hfAddEmployee.Value = "";
			txtAddEmployee.Text = "";
			if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
			{
				if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
				{
					lblLastUpdated.Text = ((DateTime)ds.Tables[1].Rows[0]["LastUpdatedDate"]).ToString(DATETIME_FORMAT);
					upLastUpdated.Update();
					if (!Convert.IsDBNull(ds.Tables[1].Rows[0]["Miles"]))
					{
						lblMiles.Text = (ds.Tables[1].Rows[0]["Miles"]).ToString();
						upMiles.Update();
					}
				}
				//Remove the added employee from Cache
				//DataRow[] drAddedEmp = dtEmployees.Select(String.Format("EmployeeId = {0}",employeeId));
				//dtEmployees.Rows.Remove(drAddedEmp[0]);

				txtAddEmployee.Focus();
				SetEmployees();
			}
			else
			{
				ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Employee50", "<script type='text/javascript'> alert('This form has reached the maximum of 50 employees.'); </script>", false);
			}
		}
		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusAddPayment();", true);
	}

	//protected void ddAddEmployee_SelectedIndexChanged(object sender, EventArgs e)
	//{
	//    if (ddAddEmployee.SelectedValue != "")
	//    {
	//        DataSet ds = ATMDB.GetDataSet("up_p_addEmployee", UserName, gFormId, ddAddEmployee.SelectedValue);
	//        if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
	//        {
	//            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
	//            {
	//                lblLastUpdated.Text = ((DateTime)ds.Tables[1].Rows[0]["LastUpdatedDate"]).ToString(DATETIME_FORMAT);
	//                upLastUpdated.Update();
	//                if (!Convert.IsDBNull(ds.Tables[1].Rows[0]["Miles"]))
	//                {
	//                    lblMiles.Text = (ds.Tables[1].Rows[0]["Miles"]).ToString();
	//                    upMiles.Update();
	//                }
	//            }
	//            SetEmployees();
	//        }
	//        else
	//        {
	//            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Employee50", "<script type='text/javascript'> alert('This form has reached the maximum of 50 employees.'); </script>", false);
	//        }
	//    }
	//}

	private void RunUpdateSP(string sp, params object[] parms)
	{
		DataView dv = ATMDB.GetDataView(sp, parms);
		if (dv.Count > 0)
		{
			lblLastUpdated.Text = ((DateTime)dv[0]["LastUpdatedDate"]).ToString(DATETIME_FORMAT);
			upLastUpdated.Update();
			if (!Convert.IsDBNull(dv[0]["Miles"]))
			{
				lblMiles.Text = (dv[0]["Miles"]).ToString();
				upMiles.Update();
			}
		}
	}



	protected void rptEmployees_ItemDataBound(object source, RepeaterItemEventArgs e)
	{
		switch (e.Item.ItemType)
		{
			case ListItemType.AlternatingItem:
			case ListItemType.Item:
				DataRowView row = (DataRowView)e.Item.DataItem;
				int employeeId = Convert.ToInt32(row["EmployeeId"]);

				Repeater rptEmployeePayments = (Repeater)e.Item.FindControl("rptEmployeePayments");
				DropDownList ddPayScale = null;

				if (IsOpen)
				{
					ddPayScale = (DropDownList)e.Item.FindControl("ddPayScale");
					ddPayScale.Visible = true;
					ddPayScale.DataSource = row.DataView.Table.DataSet.Tables["PayScales"].DefaultView;
					ddPayScale.DataBind();
					if (ddPayScale.Items.Count > 0)
					{
						if (!Convert.IsDBNull(row["PayScaleId"]))
						{
							bool selected = WebCommon.SelectListValue(ddPayScale, row["PayScaleId"].ToString());
						}
						ddPayScale.Attributes.Add("onChange", string.Format("ChangePayScale(this, {0}, false); return false;", employeeId));
						ddPayScale.Attributes.Add("prev", (ddPayScale.SelectedIndex == -1) ? "0" : ddPayScale.SelectedIndex.ToString());  //need to keep track of the previous selection
					}
				}
				else
				{
					Label lblPayScale = (Label)e.Item.FindControl("lblPayScale");
					lblPayScale.Text = row["PayScaleDisplay"].ToString();
					lblPayScale.Visible = true;
				}

				SetPayments(employeeId, rptEmployeePayments, ddPayScale);
				SetTimeLogs(employeeId, (Repeater)e.Item.FindControl("rptEmployeesTimeLogs"), (Panel)e.Item.FindControl("pnlEmptyEmployeesTimeLogs"), (Panel)e.Item.FindControl("pnlTimeLogs"), (Label)e.Item.FindControl("lblTimeLoglOverlap"));
				break;
		}

	}

	protected void btnUpdatePayScale_Click(object source, EventArgs e)
	{
		Button btnUpdatePayScale = (Button)source;

		int employeeId = Convert.ToInt32(btnUpdatePayScale.CommandArgument);
		DropDownList ddPayScale = (DropDownList)btnUpdatePayScale.NamingContainer.FindControl("ddPayScale");
		Repeater rptEmployeePayments = (Repeater)ddPayScale.Parent.FindControl("rptEmployeePayments");
		SetPayments(employeeId, rptEmployeePayments, ddPayScale);
	}
	private void SetPayments(int employeeId, Repeater rptEmployeePayments)
	{
		SetPayments(employeeId, rptEmployeePayments, null);
	}

	private void SetPayments(int employeeId, Repeater rptEmployeePayments, DropDownList ddPayScale)
	{
		string payScaleId = null;
		if (ddPayScale != null)
			payScaleId = ddPayScale.SelectedValue;
		if (payScaleId != "")
		{
			DataSet ds = ATMDB.GetDataSet("up_p_getEmployeePayments", gFormId, employeeId, payScaleId);
			DataView dvPay = new DataView(ds.Tables[0], "IsAssigned = 1", "SortDate asc", DataViewRowState.CurrentRows);
			if (dvPay.Count > 0)
			{
				rptEmployeePayments.DataSource = dvPay;
				rptEmployeePayments.DataBind();

				Label lblHolidayPay = (Label)rptEmployeePayments.NamingContainer.FindControl("lblHolidayPay");
				if (!gHasHolidayPay)
				{
					lblHolidayPay.Visible = false;
				}
				rptEmployeePayments.Visible = true;
				rptEmployeePayments.NamingContainer.FindControl("pnlEmptyPayments").Visible = false;
				rptEmployeePayments.NamingContainer.FindControl("pnlGrandTotal").Visible = true;
				rptEmployeePayments.NamingContainer.FindControl("pnlHolidayPay").Visible = true;
			}
			else
			{
				rptEmployeePayments.DataSource = null;
				rptEmployeePayments.DataBind();
				rptEmployeePayments.Visible = false;
				rptEmployeePayments.NamingContainer.FindControl("pnlEmptyPayments").Visible = true;
				rptEmployeePayments.NamingContainer.FindControl("pnlGrandTotal").Visible = false;
				rptEmployeePayments.NamingContainer.FindControl("pnlHolidayPay").Visible = false;
			}
			Label lblEmployeeTotal = (Label)rptEmployeePayments.NamingContainer.FindControl("lblEmployeeTotal");
			object gt = ds.Tables[0].Compute("SUM(LineTotal)", "IsAssigned = 1");

			decimal grandTotal = (!Convert.IsDBNull(gt)) ? Convert.ToDecimal(gt) : (decimal)0;
			lblEmployeeTotal.Text = grandTotal.ToString("N2");
		}
	}
	protected void rptEmployeePayments_ItemDataBound(object source, RepeaterItemEventArgs e)
	{
		switch (e.Item.ItemType)
		{
			case ListItemType.Header:
				gHasHolidayPay = false;
				break;
			case ListItemType.AlternatingItem:
			case ListItemType.Item:
				DataRowView row = (DataRowView)e.Item.DataItem;
				Panel pnlHolidayPay = (Panel)e.Item.FindControl("pnlHolidayPay");
				decimal qty = 0;
				if (!Convert.IsDBNull(row["Quantity"]))
					qty = Convert.ToDecimal(row["Quantity"]);
				decimal rate = Convert.ToDecimal(row["Rate"]);
				string strRate = Convert.ToDouble(row["Rate"]).ToString("0.00##");
				string paymentId = row["PaymentId"].ToString();
				string empId = row["EmployeeId"].ToString();

				Label lblRateTypeDesc = (Label)e.Item.FindControl("lblRateTypeDesc");
				lblRateTypeDesc.Text = row["RateTypeDescription"].ToString();

				Label lblPaymentNotes = (Label)e.Item.FindControl("lblPaymentNotes");
				lblPaymentNotes.Text = "&nbsp;&nbsp;&nbsp;" + row["PaymentNotes"].ToString();
				if (row["PaymentNotes"].ToString() == "(No Notes)")
				{
					lblPaymentNotes.Visible = false;
				}
				if (Convert.ToInt32(row["IsHolidayPay"]) == 1)
				{
					gHasHolidayPay = true;
				}
				TextBox txtQty = (TextBox)e.Item.FindControl("txtQty");
				txtQty.Text = qty.ToString("0.####");
				txtQty.Attributes.Add("onchange", string.Format("SetPaymentQty(this,{0});", paymentId));
				txtQty.Attributes.Add("allowQuarters", row["AllowQuarters"].ToString());
				txtQty.Attributes.Add("rateTypeId", row["RateTypeId"].ToString());
				txtQty.Attributes.Add("max", row["Max"].ToString());
				txtQty.Attributes.Add("origVal", qty.ToString("0.####"));

				Label lblRate = (Label)e.Item.FindControl("lblRate");
				lblRate.Text = strRate.Substring(strRate.IndexOf(".") + 1).Length > 2 ? Convert.ToDouble(row["Rate"]).ToString("0.0000") : Convert.ToDouble(row["Rate"]).ToString("0.00");
				Label lblTotal = (Label)e.Item.FindControl("lblTotal");
				if (row["RateTypeId"].ToString() != "5" && row["RateTypeId"].ToString() != "54")
				{
					lblTotal.Text = (rate * qty).ToString("N2");
				}
				else
				{
					lblTotal.Text = (rate * (qty / 1000)).ToString("N2");
				}
				ImageButton btnRemovePayment = (ImageButton)e.Item.FindControl("btnRemovePayment");
				if (IsOpen)
				{
					txtQty.ReadOnly = false;
					btnRemovePayment.Visible = true;
					btnRemovePayment.CommandName = empId;
					btnRemovePayment.CommandArgument = paymentId;
				}
				else
				{
					txtQty.ReadOnly = true;
					btnRemovePayment.Visible = false;
				}
				break;
		}
	}

	protected void btnRemovePayment_Click(object sender, EventArgs e)
	{
		ImageButton btnRemovePayment = (ImageButton)sender;
		int empId = Convert.ToInt32(btnRemovePayment.CommandName);
		int paymentId = Convert.ToInt32(btnRemovePayment.CommandArgument);
		RunUpdateSP("up_p_removeEmployeePayment", UserName, gFormId, paymentId);

		Repeater rptEmployeePayments = (Repeater)btnRemovePayment.NamingContainer.NamingContainer.NamingContainer.FindControl("rptEmployeePayments");
		DropDownList ddPayScale = (DropDownList)rptEmployeePayments.NamingContainer.FindControl("ddPayScale");
		SetPayments(empId, rptEmployeePayments, ddPayScale);
	}


	protected void rptVehicles_ItemDataBound(object source, RepeaterItemEventArgs e)
	{
		switch (e.Item.ItemType)
		{
			case ListItemType.AlternatingItem:
			case ListItemType.Item:
				DataRowView row = (DataRowView)e.Item.DataItem;
				int vehicleId = Convert.ToInt32(row["FormVehicleId"]);
				int actualVehicleId = Convert.ToInt32(row["VehicleId"]);
				SetFuelTickets(vehicleId, (Repeater)e.Item.FindControl("rptVehicleFuelTickets"), (Panel)e.Item.FindControl("pnlEmptyVehicleFuelTickets"), FuelTicketType.Vehicle);
				HyperLink hlOverlap = (HyperLink)e.Item.FindControl("hlOverlap");
				hlOverlap.CssClass = "hlOverlap" + Convert.ToString(vehicleId);
				if (Convert.ToString(row["OverlapForm"]) != "")
				{
					//lblOverlap.Style["display"] = "block";
					hlOverlap.Style["display"] = "block";
					hlOverlap.Style["text-align"] = "left";
					hlOverlap.NavigateUrl = Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.Query, string.Empty) + "?fid=" + Convert.ToString(row["OverlapForm"]);
					hlOverlap.Text = "Warning: odometer reading overlaps with Form " + Convert.ToString(row["OverlapForm"]);
				}
				var btnAddRefresh = (Button)e.Item.FindControl("btnRefreshAddVehicleFT");
				btnAddRefresh.Attributes.Add("data-refreshTrigger", "add_refresh_trigger_vehicle_" + actualVehicleId.ToString());

				var btnUpdateRefresh = (Button)e.Item.FindControl("btnRefreshVehicleFT");
				btnUpdateRefresh.Attributes.Add("data-refreshTrigger", "update_refresh_trigger_vehicle_" + actualVehicleId.ToString());

				//if (gNewlyAddedVehicle != 0)
				//{
				//    if (actualVehicleId == gNewlyAddedVehicle)
				//    {
				//        System.Web.UI.HtmlControls.HtmlInputText txtBeginOdometer = (System.Web.UI.HtmlControls.HtmlInputText)e.Item.FindControl("txtBeginOdometer");
				//        txtBeginOdometer.Focus();
				//        gNewlyAddedVehicle = 0;    
				//    }
				//}
				break;
		}
	}

	protected void btnRemoveEmployee_Click(object sender, EventArgs e)
	{
		LinkButton btnRemoveEmployee = (LinkButton)sender;

		//Add removed Employee to cache
		string[] commandArgs = btnRemoveEmployee.CommandArgument.ToString().Split(new char[] { '|' });
		string employeeId = commandArgs[0];
		string employeeName = commandArgs[1];
		//dtEmployees.Rows.Add(employeeId, employeeName);

		RunUpdateSP("up_p_removeEmployee", UserName, gFormId, employeeId);
		//RunUpdateSP("up_p_removeEmployee", UserName, gFormId, btnRemoveEmployee.CommandArgument);
		SetEmployees();
		upEmployees.Update(); //do this here b/c we can't add this button as trigger since it's nested in a repeater
	}

	protected void btnAddVehicle_Click(object sender, EventArgs e)
	{
		if (hfAddVehicle.Value != "")
		{
			RunUpdateSP("up_p_addVehicle", UserName, gFormId, hfAddVehicle.Value);
			gNewlyAddedVehicle = Convert.ToInt32(hfAddVehicle.Value);
			hfAddVehicle.Value = "";
			txtAddVehicle.Text = "";
			SetVehicles();

			//Remove the added vehicle from Cache
			//DataRow[] drAddedVeh = dtVehicles.Select(String.Format("VehicleId = {0}", gNewlyAddedVehicle));
			//dtVehicles.Rows.Remove(drAddedVeh[0]);

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusLastInputElement('body .FocusBeginOdometer');", true);
			//txtAddVehicle.Focus();
		}
	}

	//protected void ddAddVehicle_SelectedIndexChanged(object sender, EventArgs e)
	//{
	//    RunUpdateSP("up_p_addVehicle", UserName, gFormId, ddAddVehicle.SelectedValue);
	//    gNewlyAddedVehicle = Convert.ToInt32(ddAddVehicle.SelectedValue);
	//    SetVehicles();
	//}

	protected void btnRemoveVehicle_Click(object sender, EventArgs e)
	{
		LinkButton btnRemoveVehicle = (LinkButton)sender;

		//Add removed vehichle to cache
		string[] commandArgs = btnRemoveVehicle.CommandArgument.ToString().Split(new char[] { '|' });
		string formVehicleId = commandArgs[0];
		string vehicleId = commandArgs[1];
		string vehicleName = commandArgs[2];
		// dtVehicles.Rows.Add(vehicleId, vehicleName);

		RunUpdateSP("up_p_removeVehicle", UserName, gFormId, formVehicleId);
		//RunUpdateSP("up_p_removeVehicle", UserName, gFormId, btnRemoveVehicle.CommandArgument);
		SetVehicles();
		upVehicles.Update(); //do this here b/c we can't add this button as trigger since it's nested in a repeater
	}

	protected void btnRefreshFT_Click(object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		Repeater rpt = (Repeater)btn.NamingContainer.FindControl(string.Format("rpt{0}FuelTickets", btn.CommandName));
		Panel pnl = (Panel)btn.NamingContainer.FindControl(string.Format("pnlEmpty{0}FuelTickets", btn.CommandName));
		SetFuelTickets(Convert.ToInt32(btn.CommandArgument), rpt, pnl, (FuelTicketType)Enum.Parse(typeof(FuelTicketType), btn.CommandName));
	}

	protected void btnRefreshAddFT_Click(object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		Repeater rpt = (Repeater)btn.NamingContainer.FindControl(string.Format("rpt{0}FuelTickets", btn.CommandName));
		Panel pnl = (Panel)btn.NamingContainer.FindControl(string.Format("pnlEmpty{0}FuelTickets", btn.CommandName));
		SetFuelTickets(Convert.ToInt32(btn.CommandArgument), rpt, pnl, (FuelTicketType)Enum.Parse(typeof(FuelTicketType), btn.CommandName));
		if (hfVFTDialogBoxCall.Value != "" && !Convert.IsDBNull(hfVFTDialogBoxCall.Value))
		{
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", hfVFTDialogBoxCall.Value, true);
			hfVFTDialogBoxCall.Value = "";
		}
		else if (hfTFTDialogBoxCall.Value != "" && !Convert.IsDBNull(hfTFTDialogBoxCall.Value))
		{
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", hfTFTDialogBoxCall.Value, true);
			hfTFTDialogBoxCall.Value = "";
		}
	}

	protected void btnRefreshEmployeeTL_Click(object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		Repeater rpt = (Repeater)btn.NamingContainer.FindControl("rptEmployeesTimeLogs");
		Panel pnl = (Panel)btn.NamingContainer.FindControl("pnlEmptyEmployeesTimeLogs");
		Panel pnlTimeLog = (Panel)btn.NamingContainer.FindControl("pnlTimeLogs");
		Label lblTimelogOverlap = (Label)btn.NamingContainer.FindControl("lblTimeLoglOverlap");
		SetTimeLogs(Convert.ToInt32(btn.CommandArgument), rpt, pnl, pnlTimeLog, lblTimelogOverlap);
	}
	protected void ddFormType_SelectedIndexChanged(object sender, EventArgs e)
	{
		string pre;
		if (Convert.ToInt32(ddFormType.SelectedValue) == 0)
		{
			pnlPlanInfo.Visible = true;
			pre = "";
			routeNo.Attributes.Add("title", "");
			txtRouteNo.Text = "";
			txtRouteNo.MaxLength = 4;
			lblRouteNoPrefix.InnerHtml = "";
		}
		else if (Convert.ToInt32(ddFormType.SelectedValue) == 1)
		{
			pnlPlanInfo.Visible = true;
			pre = "S";
			routeNo.Attributes.Add("title", ddFormType.SelectedItem.ToString() + " forms must contain a route # that begins with an '" + pre + "'");
			txtRouteNo.Text = "";
			txtRouteNo.MaxLength = 3;
			lblRouteNoPrefix.InnerHtml = pre + " ";
		}
		else
		{
			pnlPlanInfo.Visible = false;
			pre = ddFormType.SelectedItem.ToString().Substring(0, 1);
			if (pre == "L")
			{
				pre = "H";
			}
			routeNo.Attributes.Add("title", ddFormType.SelectedItem.ToString() + " forms must contain a route # that begins with an '" + pre + "'");
			txtRouteNo.Text = "";
			txtRouteNo.MaxLength = 3;
			lblRouteNoPrefix.InnerHtml = pre + " ";
		}
		txtCasesOnCreate.Text = "";
		txtPoundsOnCreate.Text = "";
		txtCubesOnCreate.Text = "";
		txtStopsOnCreate.Text = "";
		dteDepartDate.Attributes.Add("AutoComplete", "Off");
	}

	protected void btnAddTrailer_Click(object sender, EventArgs e)
	{
		if (hfAddTrailer.Value != "")
		{
			RunUpdateSP("up_p_addTrailer", UserName, gFormId, hfAddTrailer.Value);
			string addedTrailer = hfAddTrailer.Value;
			hfAddTrailer.Value = "";
			txtAddTrailer.Text = "";

			//Remove the added trailer from Cache
			//DataRow[] drAddedTrailer = dtTrailers.Select(String.Format("TrailerId = {0}", addedTrailer));
			//dtTrailers.Rows.Remove(drAddedTrailer[0]);

			SetTrailers();
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusLastInputElement('body .FocusBeginHour');", true);
			//txtAddTrailer.Focus();
		}
	}

	//protected void ddAddTrailer_SelectedIndexChanged(object sender, EventArgs e)
	//{
	//    RunUpdateSP("up_p_addTrailer", UserName, gFormId, ddAddTrailer.SelectedValue);
	//    SetTrailers();
	//}
	protected void rptTrailers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		switch (e.Item.ItemType)
		{
			case ListItemType.AlternatingItem:
			case ListItemType.Item:
				DataRowView row = (DataRowView)e.Item.DataItem;
				int trailerId = Convert.ToInt32(row["FormTrailerId"]);
				int actualTrailerId = Convert.ToInt32(row["TrailerId"]);
				SetFuelTickets(trailerId, (Repeater)e.Item.FindControl("rptTrailerFuelTickets"), (Panel)e.Item.FindControl("pnlEmptyTrailerFuelTickets"), FuelTicketType.Trailer);

				var btnAddRefresh = (Button)e.Item.FindControl("btnRefreshAddTrailerFT");
				btnAddRefresh.Attributes.Add("data-refreshTrigger", "add_refresh_trigger_trailer_" + actualTrailerId.ToString());

				var btnUpdateRefresh = (Button)e.Item.FindControl("btnRefreshTrailerFT");
				btnUpdateRefresh.Attributes.Add("data-refreshTrigger", "update_refresh_trigger_trailer_" + actualTrailerId.ToString());
				break;
		}
	}
	protected void btnRemoveTrailer_Click(object sender, EventArgs e)
	{
		LinkButton btnRemoveTrailer = (LinkButton)sender;

		//Add removed trailer to cache
		string[] commandArgs = btnRemoveTrailer.CommandArgument.ToString().Split(new char[] { '|' });
		string formTrailerId = commandArgs[0];
		string trailerId = commandArgs[1];
		string trailerName = commandArgs[2];
		//dtTrailers.Rows.Add(trailerId, trailerName);

		RunUpdateSP("up_p_removeTrailer", UserName, gFormId, formTrailerId);
		//RunUpdateSP("up_p_removeTrailer", UserName, gFormId, btnRemoveTrailer.CommandArgument);
		SetTrailers();
		upTrailers.Update(); //do this here b/c we can't add this button as trigger since it's nested in a repeater
	}

	protected void btnRemoveBackhaul_Click(object sender, EventArgs e)
	{
		LinkButton btnRemoveBackhaul = (LinkButton)sender;
		DataSet ds = ATMDB.GetDataSet("up_p_removeBackhaul", UserName, gFormId, btnRemoveBackhaul.CommandArgument);
		SetEmployees();
		upEmployees.Update();
		SetBackhauls();
		upBackhauls.Update();

		if (ds.Tables[1].Rows.Count > 0)
		{
			lblLastUpdated.Text = ((DateTime)ds.Tables[1].Rows[0]["LastUpdatedDate"]).ToString(DATETIME_FORMAT);
			upLastUpdated.Update();
		}
	}

	enum FuelTicketType
	{
		Vehicle,
		Trailer
	}
	private void SetFuelTickets(int id, Repeater rptFuelTickets, Panel pnlEmptyFuelTickets, FuelTicketType type)
	{
		DataView dv = ATMDB.GetDataView((type == FuelTicketType.Trailer) ? "up_p_getTrailerFuelTickets" : "up_p_getVehicleFuelTickets", id);
		rptFuelTickets.DataSource = dv;
		rptFuelTickets.DataBind();
		if (dv.Count > 0)
		{
			rptFuelTickets.Visible = true;
			pnlEmptyFuelTickets.Visible = false;
		}
		else
		{
			rptFuelTickets.Visible = false;
			pnlEmptyFuelTickets.Visible = true;
		}
	}

	private void SetTimeLogs(int empId, Repeater rptEmployeeTimeLogs, Panel pnlEmptyEmployeeTimeLogs, Panel pnlTimeLog, Label lblTimelogOverlap)
	{
		DataSet ds = ATMDB.GetDataSet("up_p_getEmployeeTimeLogs", gFormId, empId);
		if (Convert.ToInt32(ds.Tables[1].Rows[0][0]) == 1 && Convert.ToInt32(ds.Tables[1].Rows[0][1]) != 2)
		{
			rptEmployeeTimeLogs.DataSource = ds.Tables[0];
			rptEmployeeTimeLogs.DataBind();
			if (ds.Tables[0].Rows.Count > 0)
			{
				rptEmployeeTimeLogs.Visible = true;
				pnlEmptyEmployeeTimeLogs.Visible = false;
				if (Convert.ToInt32(ds.Tables[2].Rows[0][0]) > 0)
				{
					lblTimelogOverlap.Style["display"] = "block";
				}
				else
				{
					lblTimelogOverlap.Style["display"] = "none";
				}
			}
			else
			{
				rptEmployeeTimeLogs.Visible = false;
				pnlEmptyEmployeeTimeLogs.Visible = true;
			}
		}
		else
		{
			pnlTimeLog.Visible = false;
		}
	}

	[WebMethod]
	public static string[] PM_GetRouteInfo(string routeNo, int sygmaCenterNo, DateTime departDate)
	{
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_getRouteInfo", routeNo, sygmaCenterNo, departDate);
		string[] routeInfo = new string[5];
		if (ds.Tables[0].Rows.Count > 0)
		{
			routeInfo[0] = "1";
			routeInfo[1] = ds.Tables[0].Rows[0][0].ToString();
			routeInfo[2] = ds.Tables[0].Rows[0][1].ToString();
			routeInfo[3] = ds.Tables[0].Rows[0][2].ToString();
			routeInfo[4] = ds.Tables[0].Rows[0][3].ToString();
		}
		else
		{
			routeInfo[0] = "0";
		}
		return routeInfo;
	}

	[WebMethod]
	public static string PM_SavePlanInfo(string userName, int formId, string valType, int val)
	{
		object[] parms = new object[7];
		parms[0] = userName;
		parms[1] = formId;
		switch (valType.ToLower())
		{
			case "cases":
				parms[2] = val;
				break;
			case "pounds":
				parms[3] = val;
				break;
			case "cubes":
				parms[4] = val;
				break;
			case "stops":
				parms[5] = val;
				break;
			case "miles":
				parms[6] = val;
				break;
		}
		return RunStaticUpdateSP("up_p_saveFormPlanInfo", parms);
	}
	[WebMethod]
	public static object[] PM_ChangePayScale(string userName, int formid, int employeeId, int payScaleId, bool deleteExisting)
	{
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_setEmployeePayScale", userName, formid, employeeId, payScaleId, deleteExisting);
		int count = ds.Tables[0].Rows.Count;
		if (count > 0)
		{
			StringBuilder sb = new StringBuilder("The following \'Rate Types\' are invalid for the selected \'Pay Scale\':\n");
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				sb.AppendFormat("\n{0}", row["RateTypeDescription"]);
			}
			sb.AppendFormat("\n\nClick \'OK\' to delete the invalid payment.\nClick \'Cancel\' to cancel the selection.", (count > 1) ? "s" : "");
			return new object[] { false, sb.ToString() };
		}
		if (ds.Tables.Count > 1)
		{
			return new object[] { true, ((DateTime)ds.Tables[1].Rows[0]["LastUpdatedDate"]).ToString(DATETIME_FORMAT) };
		}
		return new string[0];
	}
	[WebMethod]
	public static string[] PM_SetPaymentQty(string userName, int formid, int paymentId, decimal qty)
	{
		string[] rv = new string[3];
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_setEmployeePayment", userName, formid, paymentId, qty);
		if (ds.Tables[0].Rows.Count > 0)
		{
			DataRow row = ds.Tables[0].Rows[0];
			rv[0] = row["LineTotal"].ToString();
			rv[1] = ((DateTime)row["LastUpdatedDate"]).ToString(DATETIME_FORMAT);
			rv[2] = row["IsMilesQtyGreater"].ToString();
		}
		return rv;
	}

	[WebMethod]
	public static string[] PM_AddFuelTicket(string userName, int formEntityId, string fuelTicketNo, DateTime datePurchased, decimal gallons, decimal quantity, decimal price, int formid, string fttype, string item)
	{
		string[] retValues = new string[5];
		retValues[0] = RunStaticUpdateSP((fttype.ToLower() == "vehicle") ? "up_f_addVehicleFuelTicket" : "up_f_addTrailerFuelTicket", userName, formEntityId, fuelTicketNo, datePurchased, gallons, quantity, price, formid);
		retValues[1] = item;
		retValues[2] = Convert.ToString(formEntityId);
		retValues[3] = Convert.ToString(formid);
		retValues[4] = fttype;
		return retValues;
	}

	[WebMethod]
	public static string PM_UpdateFuelTicket(string userName, string fuelTicketNo, DateTime datePurchased, decimal gallons, decimal quantity, decimal price, int fuelTicketid, string fttype)
	{
		return RunStaticUpdateSP((fttype.ToLower() == "vehicle") ? "up_f_UpdateVehicleFuelTicket" : "up_f_UpdateTrailerFuelTicket", userName, fuelTicketNo, datePurchased, gallons, quantity, price, fuelTicketid);
	}

	[WebMethod]
	public static string PM_RemoveFuelTicket(string userName, int formId, int fuelTicketId, bool deleteTicket, string fttype)
	{
		return RunStaticUpdateSP((fttype.ToLower() == "vehicle") ? "up_p_removeVehicleFuelTicket" : "up_p_removeTrailerFuelTicket", userName, formId, fuelTicketId, deleteTicket);
	}

	[WebMethod]
	public static int PM_CheckFuelTicketsOdometer(int vehicleOrFTId, int miles, string addUpd)
	{
		if (addUpd == "ADD")
		{
			return Convert.ToInt32(RunStaticGetSP("up_f_checkOdometerVehicleFuelTicket", vehicleOrFTId, miles, 'A'));
		}
		else
		{
			return Convert.ToInt32(RunStaticGetSP("up_f_checkOdometerVehicleFuelTicket", vehicleOrFTId, miles, 'U'));
		}
	}

	[WebMethod]
	public static string PM_AddTimeLog(string userName, int EmployeeId, int formId, DateTime startDate, DateTime endDate, int startHour, int endHour, int startMin, int endMin)
	{
		return RunStaticUpdateSP("up_p_addEmployeeTimeLog", userName, EmployeeId, formId, startDate, endDate, startHour, endHour, startMin, endMin);
	}

	[WebMethod]
	public static string PM_UpdateTimeLog(string userName, int timeLogId, DateTime startDate, DateTime endDate, int startHour, int endHour, int startMin, int endMin)
	{
		return RunStaticUpdateSP("up_p_updateEmployeeTimeLog", userName, timeLogId, startDate, endDate, startHour, endHour, startMin, endMin);
	}

	[WebMethod]
	public static string PM_RemoveTimeLog(string userName, int formId, int timelogid)
	{
		return RunStaticUpdateSP("up_p_removeEmployeeTimeLog", userName, formId, timelogid);
	}

	[WebMethod]
	public static string[] PM_SetVehicleOdometer(string userName, int formId, int formVehicleId, decimal odometer, string type)
	{
		object[] parms = new object[5];
		parms[0] = userName;
		parms[1] = formId;
		parms[2] = formVehicleId;
		switch (type)
		{
			case "begin":
				parms[3] = odometer;
				break;
			case "end":
				parms[4] = odometer;
				break;
		}
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_setVehicleOptions", parms);
		string[] rv = new string[6];

		if (ds.Tables[0].Rows.Count > 0)
		{
			DataRow row = ds.Tables[0].Rows[0];
			rv[0] = (row["Miles"]).ToString();
			rv[1] = ((DateTime)row["LastUpdatedDate"]).ToString(DATETIME_FORMAT);
		}
		rv[2] = string.Empty;
		if (ds.Tables[1].Rows.Count > 0)
		{
			rv[2] = ds.Tables[1].Rows[0][0].ToString();
			//Is Payment Added flag - need to refresh employee panel
			rv[4] = ds.Tables[1].Rows[0][1].ToString();
			//Is Confirmation needed flag - get the confirmation before updating/adding miles payment
			rv[5] = ds.Tables[1].Rows[0][2].ToString();
		}
		rv[3] = Convert.ToString(formVehicleId);
		return rv;
	}

	[WebMethod]
	public static string[] PM_UpdateEmployeesMilesPayment(string userName, int formId)
	{
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_addUpdateMilesPayment", userName, formId);
		string[] str = new string[2];
		if (ds.Tables.Count > 0)
		{
			str[0] = ds.Tables[0].Rows[0][0].ToString();
			str[1] = "1";
		}
		else
		{
			str[0] = string.Empty;
			str[1] = "0";
		}
		return str;
	}

	[WebMethod]
	public static string PM_SetExternalVehicleID(string userName, int formId, int vehicleId, string id)
	{
		object[] parms = new object[6];
		parms[0] = userName;
		parms[1] = formId;
		parms[2] = vehicleId;
		parms[5] = id;
		return RunStaticUpdateSP("up_p_setVehicleOptions", parms);
	}

	[WebMethod]
	public static string PM_SetTrailerHours(string userName, int formId, int formTrailerId, decimal hours, string type)
	{
		object[] parms = new object[5];
		parms[0] = userName;
		parms[1] = formId;
		parms[2] = formTrailerId;
		switch (type)
		{
			case "begin":
				parms[3] = hours;
				break;
			case "end":
				parms[4] = hours;
				break;
		}
		return RunStaticUpdateSP("up_p_setTrailerHours", parms);
	}
	[WebMethod]
	public static string PM_SetNotes(string userName, int formId, string notes)
	{
		return RunStaticUpdateSP("up_p_setNotes", userName, formId, notes);

	}
	[WebMethod]
	public static string PM_SaveBackhaul(string userName, int backhaulId, string poNo, decimal? revenue)
	{
		return RunStaticUpdateSP("up_p_setBackhaul", userName, backhaulId, poNo, revenue);
	}

	[WebMethod]
	public static int PM_UpdateWeekendingDate(string userName, int formId, DateTime weekendingDate)
	{
		//RunNonQueryFromStatic("ATM", "up_p_updateFormWeekendingDate", userName, formId, weekendingDate);
		int rv = Convert.ToInt32(GetScalarFromStatic("ATM", "up_p_updateFormWeekendingDate", userName, formId, weekendingDate));
		return rv;
	}

	[WebMethod]
	public static string[] PM_ChangeStatus(string userName, int formId, string newStatus)
	{
		string isUpdateSuccess = Convert.ToString(GetScalarFromStatic("ATM", "up_p_changeStatus", formId, userName, newStatus));
		string[] ret = new string[2];
		ret[0] = newStatus;
		ret[1] = isUpdateSuccess;
		return ret;
	}
	[WebMethod]
	public static string[] PM_ChangeStatusReject(string userName, int formId, string newStatus, string notes)
	{
		string isUpdateSuccess = Convert.ToString(GetScalarFromStatic("ATM", "up_p_changeStatus", formId, userName, newStatus, notes));
		string[] ret = new string[2];
		ret[0] = newStatus;
		ret[1] = isUpdateSuccess;
		return ret;
	}
	[WebMethod]
	public static void PM_DeleteForm(string userName, int formId)
	{
		RunNonQueryFromStatic("ATM", "up_p_deleteForms", formId, userName);
	}
	[WebMethod]
	public static string[] PM_AddPayment(string userName, int formId, int rateTypeId, int? categoryId, decimal quantity, string notes, int isHolidayPay, string[] applyTos)
	{
		string[] rv = new string[3];
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_addEmployeePayments", userName, formId, rateTypeId, categoryId, quantity, notes, string.Join(",", applyTos), isHolidayPay);
		if (ds.Tables.Count > 2)
		{
			DataRow row = ds.Tables[0].Rows[0];
			rv[0] = ((DateTime)row[0]).ToString(DATETIME_FORMAT);
			if (ds.Tables[1].Rows.Count > 0)
			{
				DataRow rowEmp;
				rv[1] = "";
				for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
				{
					rowEmp = ds.Tables[1].Rows[i];
					if (i == 0)
						rv[1] = rowEmp[1].ToString();
					else if (i != ds.Tables[0].Rows.Count - 1)
						rv[1] = rv[1] + ", " + rowEmp[1].ToString();
					else
						rv[1] = rv[1] + " and " + rowEmp[1].ToString();
				}
			}
			else
			{
				rv[1] = "0";
			}
			if (ds.Tables[2].Rows.Count > 0)
			{
				rv[2] = ds.Tables[2].Rows[0][0].ToString();
			}
			else
			{
				rv[2] = "0";
			}
		}
		else
		{
			rv[0] = "";
			if (ds.Tables[0].Rows.Count > 0)
			{
				DataRow rowEmp;
				rv[1] = "";
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					rowEmp = ds.Tables[0].Rows[i];
					if (i == 0)
						rv[1] = rowEmp[1].ToString();
					else if (i != ds.Tables[0].Rows.Count - 1)
						rv[1] = rv[1] + ", " + rowEmp[1].ToString();
					else
						rv[1] = rv[1] + " and " + rowEmp[1].ToString();
				}
			}
			else
			{
				rv[1] = "0";
			}
			if (ds.Tables[1].Rows.Count > 0)
			{
				rv[2] = ds.Tables[1].Rows[0][0].ToString();
			}
			else
			{
				rv[2] = "0";
			}
		}
		return rv;
	}
	[WebMethod]
	public static string[] PM_AddBackhaul(string userName, int formId, string poNo, decimal revenue, string isAddPayment)
	{
		string[] bk = new string[2];
		bk[0] = Convert.ToString(RunStaticUpdateSP("up_p_addBackhaul", userName, formId, poNo, revenue));
		bk[1] = isAddPayment;
		return bk;
	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static string[] GetEmployees(string prefixText, string formId)
	{
		DataView dv = GetDataViewFromStatic("ATM", "up_p_getFormEmployees", prefixText, Convert.ToInt32(formId));
		//DataTable dt = FilterExistingEmployees();
		//List<string> employees = new List<string>(dv.Count);
		//foreach (DataRowView row in dv)
		//{
		//    employees.Add(string.Format("{0}-{1}", row["EmployeeId"].ToString(), row["EmployeeFullName"].ToString()));
		//}
		//return employees.ToArray();
		DataTable dtEmployees = dv.Table;
		//DataRow[] empFound = dtEmployees.Select(string.Format("EmployeeFullName like'%{0}%'", prefixText));
		List<string> employees = new List<string>(dtEmployees.Rows.Count);
		foreach (DataRowView row in dtEmployees.DefaultView)
		{
			employees.Add(string.Format("{0}-{1}", row["EmployeeId"].ToString(), row["EmployeeFullName"].ToString()));
		}
		return employees.ToArray();

	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static string[] GetVehicles(string prefixText, string formId)
	{
		DataView dv = GetDataViewFromStatic("ATM", "up_p_getFormVehicles", prefixText, Convert.ToInt32(formId));
		//List<string> vehicles = new List<string>(dv.Count);
		//foreach (DataRowView row in dv)
		//{
		//    vehicles.Add(string.Format("{0}-{1}", row["VehicleId"].ToString(), row["VehicleName"].ToString()));
		//}
		//return vehicles.ToArray();
		DataTable dtVehicles = dv.Table;
		//DataRow[] vehicleFound = dtVehicles.Select(string.Format("VehicleName like'%{0}%'", prefixText));
		List<string> vehicles = new List<string>(dtVehicles.Rows.Count);
		foreach (DataRow row in dtVehicles.Rows)
		{
			vehicles.Add(string.Format("{0}-{1}", row["VehicleId"].ToString(), row["VehicleName"].ToString()));
		}
		return vehicles.ToArray();

	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	//public static string[] GetTrailers(string prefixText, string formId)
	public static string[] GetTrailers(string prefixText, string formId, string centerNo)
	{
		//DataView dv = GetDataViewFromStatic("ATM", "up_p_getFormTrailers", prefixText, Convert.ToInt32(formId));
		DataView dv = GetDataViewFromStatic("ATM", "up_p_getFormTrailers", prefixText, Convert.ToInt32(formId), Convert.ToInt32(centerNo));
		//List<string> trailers = new List<string>(dv.Count);
		//foreach (DataRowView row in dv)
		//{
		//    trailers.Add(string.Format("{0}-{1}", row["TrailerId"].ToString(), row["TrailerName"].ToString()));
		//}
		//return trailers.ToArray();
		DataTable dtTrailers = dv.Table;
		//DataRow[] trailerFound = dtTrailers.Select(string.Format("TrailerName like'%{0}%'", prefixText));
		//List<string> trailers = new List<string>(trailerFound.Count());
		//foreach (DataRow row in trailerFound)
		List<string> trailers = new List<string>(dtTrailers.Rows.Count);
		foreach (DataRow row in dtTrailers.Rows)
		{
			trailers.Add(string.Format("{0}-{1}", row["TrailerId"].ToString(), row["TrailerName"].ToString()));
		}
		return trailers.ToArray();
	}

	//[WebMethod]
	//[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	//public static string[] GetDrivers(string prefixText, string formId)
	//{
	//    DataView dv = GetDataViewFromStatic("ATM", "up_p_getFormDrivers", prefixText, Convert.ToInt32(formId));
	//    List<string> employees = new List<string>(dv.Count);
	//    foreach (DataRowView row in dv)
	//    {
	//        employees.Add(string.Format("{0}-{1}", row["EmployeeId"].ToString(), row["EmployeeFullName"].ToString()));
	//    }
	//    return employees.ToArray();
	//}

	[WebMethod]
	public static string PM_GetFormPlanInfo(int formId, int rateType)
	{
		if (rateType != 0)
		{
			return RunStaticGetSP("up_p_getPlanInfo", formId, rateType);
		}
		else
		{
			return "0";
		}
	}

	[WebMethod]
	public static string[] PM_MapFuelTickets(string userName, int formVTId, string ftType, string[] selectedFT)
	{
		string[] rv = new string[2];
		if (selectedFT.Count() <= 1 && selectedFT[0] == "0")
		{
			rv[0] = "0";
		}
		else
		{
			foreach (string FTId in selectedFT)
			{
				RunNonQueryFromStatic("ATM", "up_p_mapFuelTickets", userName, formVTId, FTId, ftType);
			}
			rv[0] = "1";
		}
		rv[1] = ftType;
		return rv;
	}

	public void btnLoadExisting_Click(object sender, EventArgs e)
	{
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_getExisitingFuelTickets", hfVTId.Value, hfType.Value, gFormId);
		setSelectFTDialog(ds, hfType.Value);
	}

	public void btnRefreshVFT_Click(object sender, EventArgs e)
	{
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_getAddedFormVehicles", gFormId);
		SetFormList("Vehicle", ds.Tables[0], rptVehicles, txtAddVehicle, lblEmptyVehicles, "AddedToForm = 0 or IsExternal = 1", "IsExternal desc, WebDisplay asc");
		upVehicles.Update();
	}
	public void btnRefreshTFT_Click(object sender, EventArgs e)
	{
		DataSet ds = GetDataSetFromStatic("ATM", "up_p_getAddedFormTrailers", gFormId);
		SetFormList("Trailer", ds.Tables[0], rptTrailers, txtAddTrailer, lblEmptyTrailers);
		upTrailers.Update();
	}

	private void setSelectFTDialog(DataSet dsFuelTickets, string FTType)
	{
		lbSelectFuelTickets.Items.Clear();

		if (dsFuelTickets.Tables[0].Rows.Count > 0)
		{
			lbSelectFuelTickets.DataSource = dsFuelTickets.Tables[0];
			lbSelectFuelTickets.DataBind();
			ListItem liChoose = new ListItem("Choose..", "0");
			liChoose.Selected = true;
			lbSelectFuelTickets.Items.Insert(0, liChoose);
			upFuelTickets.Update();
			//ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "FocusLastInputElement('body .FocusSelFT');", true);
			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "openSelFTDialog();", true);
		}
		else
		{

			ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "disableAndCloseSelFTDialog();", true);
		}
	}

	private static string RunStaticGetSP(string sp, params object[] parms)
	{
		object quantity = GetScalarFromStatic("ATM", sp, parms);
		if (quantity != null)
			return ((int)quantity).ToString();
		return "";
	}
	private static string RunStaticUpdateSP(string sp, params object[] parms)
	{
		object date = GetScalarFromStatic("ATM", sp, parms);
		if (date != null)
			return ((DateTime)date).ToString(DATETIME_FORMAT);
		return "";
	}

	public void gvNotesDataBind()
	{
		DataView dvNotes = ATMDB.GetDataView("up_p_getNotesLog", Convert.ToInt32(Request.QueryString.Get("fid")));
		if (dvNotes.Table.Rows.Count == 0)
		{
			pnlNotes.Visible = false;
		}
		Repeater rptNotes = (Repeater)pnlNotes.FindControl("rptNotes");
		rptNotes.DataSource = dvNotes;
		rptNotes.DataBind();
	}

	protected void btnNotesRefresh_Click(object sender, EventArgs e)
	{
		gvNotesDataBind();
	}


	protected void txtMessages_TextChanged(object sender, EventArgs e)
	{
		Session["message"] = txtMessages.Text;
		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Srcipt", "window.location.replace(window.location);", true);

	}

	protected void ddDriver_SelectedIndexChanged(object sender, EventArgs e)
	{
		DriverRefresh(ddDriver.SelectedItem.ToString(), "ddDriver");
	}

	protected void ddDriverHelper_SelectedIndexChanged(object sender, EventArgs e)
	{
		DriverRefresh(ddDriverHelper.SelectedItem.ToString(), "ddDriverHelper");
	}

	protected void DriverRefresh(string Selection, string DropDownControl)
	{
		string sygmaCenterLabel = lblSygmaCenter.Text;
		int centerNo = Convert.ToInt32(sygmaCenterLabel.Substring(sygmaCenterLabel.IndexOf("-") + 1));
		DataSet dsDrivers = ATMDB.GetDataSet("up_p_getDrivers", centerNo);
		dtDrivers = dsDrivers.Tables[0];
		DataSet dsHelpers = ATMDB.GetDataSet("up_p_getDriverHelpers", centerNo);
		dtHelpers = dsHelpers.Tables[0];

		if (DropDownControl.Equals("ddDriver"))
		{
			ListItem InitialSelection = ddDriverHelper.SelectedItem;
			ddDriverHelper.Items.Clear();
			ddDriverHelper.DataSource = dtHelpers;
			ddDriverHelper.DataBind();
			ddDriverHelper.Items.Insert(0, new ListItem("Choose...", "0"));
			if (Selection.Equals("Choose...") == false)
			{
				ddDriverHelper.ClearSelection();
				if (ddDriverHelper.Items.Contains(ddDriver.SelectedItem))
				{
					ddDriverHelper.Items.Remove(ddDriver.SelectedItem);
				}

				if (InitialSelection.Value == "999")
				{
					ddDriverHelper.Items.Insert(0, InitialSelection);
					ddDriverHelper.ClearSelection();
					ddDriverHelper.Items.FindByValue("999").Selected = true;
				}
				else
				{
					ddDriverHelper.Items.FindByText(InitialSelection.Text).Selected = true;
				}
				if (ddDriver.Items.Contains(ddDriver.Items.FindByValue("999")))
				{
					ddDriver.Items.Remove(ddDriver.Items.FindByValue("999"));
					ddDriver.BackColor = System.Drawing.Color.White;
				}
				//ddDriverHelper.Items.Clear();
				ddDriverHelper.Enabled = true;
			}
			else
			{
				ddDriver.Items.Clear();
				ddDriver.DataSource = dtDrivers;
				ddDriver.DataBind();
				ddDriver.Items.Insert(0, new ListItem("Choose...", "0"));
				ddDriver.BackColor = System.Drawing.Color.White;

				ddDriverHelper.ClearSelection();
				ddDriverHelper.SelectedIndex = 0;
				ddDriverHelper.Enabled = false;
				ddDriverHelper.BackColor = System.Drawing.Color.White;
				RemoveHelper(UserName, gFormId, gSygmaCenterNo);
			}

		}
		else
		{
			ListItem InitialSelection = ddDriver.SelectedItem;
			ddDriver.Items.Clear();
			ddDriver.DataSource = dtDrivers;
			ddDriver.DataBind();
			ddDriver.Items.Insert(0, new ListItem("Choose...", "0"));

			//string driverSel = ddDriverHelper.SelectedItem.ToString();
			if (Selection.Equals("Choose...") == false)
			{
				ddDriver.ClearSelection();
				if (ddDriver.Items.Contains(ddDriverHelper.SelectedItem))
				{
					ddDriver.Items.Remove(ddDriverHelper.SelectedItem);
				}
				if (InitialSelection.Value == "999")
				{
					ddDriver.Items.Insert(0, InitialSelection);
					ddDriver.ClearSelection();
					ddDriver.Items.FindByValue("999").Selected = true;
				}
				else
				{
					ddDriver.Items.FindByText(InitialSelection.Text).Selected = true;
				}
				if (ddDriverHelper.Items.Contains(ddDriverHelper.Items.FindByValue("999")))
				{
					ddDriverHelper.Items.Remove(ddDriverHelper.Items.FindByValue("999"));
					ddDriverHelper.BackColor = System.Drawing.Color.White;
				}
			}
			else
			{
				if (InitialSelection.Value == "999")
				{
					ddDriver.Items.Insert(0, InitialSelection);
					ddDriver.ClearSelection();
					ddDriver.Items.FindByValue("999").Selected = true;
				}
				else
				{
					ddDriver.Items.FindByText(InitialSelection.Text).Selected = true;
				}
				if (ddDriverHelper.Items.Contains(ddDriverHelper.Items.FindByValue("999")))
				{
					ddDriverHelper.Items.Remove(ddDriverHelper.Items.FindByValue("999"));
					ddDriverHelper.BackColor = System.Drawing.Color.White;
				}
			}
		}

	}
	[WebMethod]
	public static int PM_SaveDriverDetails(string userName, int formId, int detailId, int driverid)
	{
		return (int)GetScalarFromStatic("ATM", "up_p_updateDriverDetail", userName, formId, detailId, driverid);
	}

	protected void RemoveHelper(string UserName, int FormId, int Center)
	{

		RunNonQueryFromStatic("ATM", "up_p_removeHelperDetail", UserName, Center, 2, FormId);

	}
	//[WebMethod]
	//public static void PM_SaveRouteCategory(string userName, int formId, int routeCategory)
	//{
	//    RunNonQueryFromStatic("ATM", "up_p_updateRouteCategory", userName, formId, routeCategory);
	//}

	//private void FilterExistingEmployees()
	//{
	//    DataTable dtEmpl = dtEmployees.Copy();
	//    foreach (RepeaterItem ri in rptEmployees.Items)
	//    {
	//        System.Web.UI.HtmlControls.HtmlContainerControl a1 = ri.FindControl("testDiv2") as System.Web.UI.HtmlControls.HtmlContainerControl;
	//        string b = a1.InnerHtml;
	//        string c = b.ToString();
	//        string d = System.Text.RegularExpressions.Regex.Replace(b, @"\t|\n|\r", "");

	//        DataRow[] empFound = dtEmpl.Select(string.Format("EmployeeFullName='{0}'",d ));
	//        dtEmpl.Rows.Remove(empFound[0]);



	//    }
	//    //return dtEmpl;
	//}
	[WebMethod]
	public static string[] GetDetails(string formId)
	{
		DataSet dv = GetDataSetFromStatic("ATM", "up_p_getAllDetailsForCenterByForm", Convert.ToInt32(formId));

		List<string> emp = new List<string>(dv.Tables[0].Rows.Count);
		foreach (DataRow row in dv.Tables[0].Rows)
		{
			emp.Add(string.Format("{0}-{1}", row["EmployeeId"].ToString(), row["EmployeeFullName"].ToString()));
		}
		return emp.ToArray();
	}

}