using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Text;
using ATM;
using Newtonsoft.Json;
using ATM.Payroll;
using System.Web.Script.Services;
using atm;

public partial class Apps_ATM_Payroll_Forms_Index : ATMPage
{
    private int isApprover;
    private bool isUserSelectedDateRange;
    private static DataTable dtDrivers;
    private static DataTable dtFileredDriver = null;
    private static DataTable dtHelper;
    private static DataTable dtFileredHelper = null;
    public enum FormStatus
    {
        Open,
        Closed,
        Sent
    }
    private double gvFormsWidth;
    public double GVFormsWidth
    {
        get { return gvFormsWidth; }
        set { gvFormsWidth = value; }
    }

    private List<string> activeFormIDs = new List<string>();

    protected void ddlDateRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateDateRanges(ddlDateRange.SelectedValue);
        if (ddlDateRange.SelectedValue != DateRanges.Custom.ToString())
        {
            isUserSelectedDateRange = true;
        }
    }

    protected override void LoadATMPage()
    {
        isUserSelectedDateRange = false;
        DataSet dsIsApprover = ATMDB.GetDataSet("up_user_isApprover", UserName);
        isApprover = Convert.ToInt32(dsIsApprover.Tables[0].Rows[0][0].ToString());
        if (!IsPostBack)
        {
            Session["sUserName"] = UserName;
            SetSearchCriteria();
            RegisterSelectableGridView(gvForms);
            SetSelectedActions();

            InitializeDateRanges();
        }

        if (Session["sUserName"] == null)
        {
            Session["sUserName"] = UserName;
        }

        gvForms.PageSize = RowCountBar1.PageSize;
        DataSet dsDrivers = ATMDB.GetDataSet("up_p_getDrivers");
        dtDrivers = dsDrivers.Tables[0];
        DataSet dsHelpers = ATMDB.GetDataSet("up_p_getDriverHelpers");
        dtHelper = dsHelpers.Tables[0];
        AddClientVariable("gUserName", UserName);
    }

    private void InitializeDateRanges()
    {
        var dateRanges = DateRangeCalculator.GetValidDateRanges();
        ddlDateRange.Items.Add("- select date range -");
        foreach (var item in dateRanges)
        {
            ddlDateRange.Items.Add(new ListItem(item.Key, item.Value.ToString()));
        }
        ddlDateRange.SelectedIndex = 0;
        CalculateDateRanges(ddlDateRange.SelectedValue);
    }

    private void SetSearchCriteria()
    {
        DataSet ds = ATMDB.GetDataSet("up_p_getFormCriteria", UserName);

        ddStatus.DataSource = ds.Tables[0].DefaultView;
        if (isApprover == 1)
        {
            hlCreateNew.Visible = false;
        }

        ddFormType.DataSource = ds.Tables[1].DefaultView;
        ddSygmaCenterNo.DataSource = ds.Tables[2].DefaultView;
        pnlSearch.DataBind();

        ddStatus.Items.Insert(0, new ListItem("All", ""));

        ddFormType.Items.Insert(0, new ListItem("All", ""));
        ddSygmaCenterNo.Items.Insert(0, new ListItem("All", ""));
        if(ddSygmaCenterNo.Items.Count == 2)
        {
            ddSygmaCenterNo.SelectedIndex = 1;
            Session["ATM_Center"] = ddSygmaCenterNo.SelectedValue;
        }

        if (Session["ATM_FormId"] != null)
        {
            txtFormId.Text = Session["ATM_FormId"].ToString();
        }
        else
        {
            if (Session["ATM_Status"] != null)
                WebCommon.SelectListValue(ddStatus, Session["ATM_Status"].ToString());
            if (Session["ATM_FormType"] != null)
                WebCommon.SelectListValue(ddFormType, Session["ATM_FormType"].ToString());
            if (Session["ATM_Center"] != null)
                WebCommon.SelectListValue(ddSygmaCenterNo, Session["ATM_Center"].ToString());
            if (Session["ATM_RouteNo"] != null)
                txtRouteNo.Text = Session["ATM_RouteNo"].ToString();
            if (Session["ATM_Weekending"] != null)
            {
                if (Session["ATM_Weekending"].ToString() != "") //the user has cleared the box, do don't default
                    dteWeekending.Value = (DateTime)Session["ATM_Weekending"];
            }
            else
            { //no user interaction yet so default to weekending
                dteWeekending.Value = Dates.GetWeekending();
            }
            if (Session["ATM_FromDate"] != null)
                txtFromDate.Text = Session["ATM_FromDate"].ToString();
            if (Session["ATM_ToDate"] != null)
                txtToDate.Text = Session["ATM_ToDate"].ToString();
            if (Session["ATM_Employee"] != null)
                txtEmployee.Text = Session["ATM_Employee"].ToString();
            if (Session["ATM_Tractor"] != null)
                txtTractor.Text = Session["ATM_Tractor"].ToString();
            if (Session["ATM_Trailer"] != null)
                txtTrailer.Text = Session["ATM_Trailer"].ToString();
            if (Session["ATM_ActualsUpdated"] != null)
                WebCommon.SelectListValue(ddActualsUpdated, Session["ATM_ActualsUpdated"].ToString());
        }
    }

    private void ClearSearch()
    {
        txtFormId.Text = "";
        ddStatus.ClearSelection();
        ddFormType.ClearSelection();
        ddSygmaCenterNo.ClearSelection();
        txtRouteNo.Text = "";
        ClearDateRangeValues();
        dteWeekending.Value = Dates.GetWeekending();
        txtEmployee.Text = "";
        txtTrailer.Text = "";

        Session["ATM_FormId"] = null;
        Session["ATM_Status"] = null;
        Session["ATM_FormType"] = null;
        Session["ATM_Center"] = null;
        Session["ATM_RouteNo"] = null;
        Session["ATM_Weekeending"] = null;
        Session["ATM_Employee"] = null;
        Session["ATM_Tractor"] = null;
        Session["ATM_Trailer"] = null;
        Session["ATM_OnlyXata"] = null;
    }

    private void SetSelectedActions()
    {
        ddSelectedAction.Items.Clear();
        switch (ddStatus.SelectedValue)
        {
            case "":
                ddSelectedAction.Visible = false;
                break;
            case "0":

                if (isApprover != 1)
                {
                    //ddSelectedAction.Items.Add(new ListItem("Check In", "I"));

                }

                goto default;
            case "1":
                if (isApprover != 1)
                {

                    ddSelectedAction.Items.Add(new ListItem("Check Out", "O"));

                }
                else
                {
                    ddSelectedAction.Items.Add(new ListItem("Approve", "A"));
                    ddSelectedAction.Items.Add(new ListItem("Reject", "R"));
                }
                goto default;
            case "2":
                ddSelectedAction.Visible = false;
                break;
            case "3":
                ddSelectedAction.Visible = false;
                break;
            case "4":
                if (isApprover != 1)
                {
                    ddSelectedAction.Items.Add(new ListItem("Check Out", "O"));
                    goto default;
                }

                ddSelectedAction.Visible = false;
                break;
            default:
                ddSelectedAction.Visible = true;
                if (isApprover != 1)
                {
                    ddSelectedAction.Items.Insert(0, new ListItem("Delete", "D"));
                }
                ddSelectedAction.Items.Insert(0, new ListItem("Selected Actions...", ""));
                break;
        }
    }

    private string gChangeCheckScript;

    private void SetFormGridConfiguration()
    {
        DataSet dsSelectedColumns = ATMDB.GetDataSet("up_p_getProfileCustom", UserName, "PROFILEFORMGRID");

        if ((dsSelectedColumns.Tables[0].Rows != null) && (dsSelectedColumns.Tables[0].Rows.Count > 0))
        {
            DataTable dtSelectedCols = JsonConvert.DeserializeObject<DataSet>(dsSelectedColumns.Tables[0].Rows[0]["Value"].ToString()).Tables[0];

            // Start Grid column reordering
            List<string> columnOrder = CreateColumnOrder(dtSelectedCols);

            for (int columnOrderCounter = columnOrder.Count - 1; columnOrderCounter >= 0; columnOrderCounter--)
            {
                for (int columnCounter = 0; columnCounter <= gvForms.Columns.Count - 1; columnCounter++)
                {
                    if (gvForms.Columns[columnCounter].HeaderText == columnOrder[columnOrderCounter])
                    {
                        var gridColumn = gvForms.Columns[columnCounter];
                        gvForms.Columns.RemoveAt(columnCounter);
                        gvForms.Columns.Insert(0, gridColumn);
                    }
                }
            }
            // Stop Grid column reordering

            // Start Grid column hiding
            for (int columnCounter = 0; columnCounter <= gvForms.Columns.Count - 2; columnCounter++)
            {
                DataRow selectedDataTableRow = dtSelectedCols.Select("ColumnIdentifier='" + gvForms.Columns[columnCounter].HeaderText + "'").FirstOrDefault();

                if (selectedDataTableRow == null)
                {
                    gvForms.Columns[columnCounter].HeaderStyle.CssClass = "hiddencol";
                    gvForms.Columns[columnCounter].ItemStyle.CssClass = "hiddencol";
                }
            }
            // End Grid column hiding
        }
    }

    private List<string> CreateColumnOrder(DataTable dtSelectedCols)
    {
        List<string> finalColumn = new List<string>();
        string[,] columns = PayrollCommon.FormPageGridColumns;

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

    protected void gvForms_Init(object sender, EventArgs e)
    {
        Cache.Remove("CachedPayrollFormList");
        activeFormIDs.Clear();
        SetFormGridConfiguration();
    }

    protected void gvForms_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        SygmaFramework.Log log = new SygmaFramework.Log("C:/Logs/ATMLog.log");
        string FormId = "";
        try
        {
            switch (e.Row.RowType)
            {

                case DataControlRowType.Header:
                    UserControls_ChangeChecks cc = (UserControls_ChangeChecks)e.Row.FindControl("ChangeChecks1");
                    if (cc != null)
                    {
                        gChangeCheckScript = cc.ChangeCheckAllBoxScript;
                    }

                    break;
                case DataControlRowType.DataRow:
                    DataRowView row = (DataRowView)e.Row.DataItem;
                    FormId = row["FormId"].ToString();
                    WebCommon.SetRowScript(string.Format("javascript: OpenForm({0});", row["FormId"]), e);
                    CheckBox chkSelected = (CheckBox)e.Row.FindControl("chkSelected");
                    DropDownList ddDriver = (DropDownList)e.Row.FindControl("ddDriver");
                    DropDownList ddHelper = (DropDownList)e.Row.FindControl("ddHelper");
                    if (gChangeCheckScript != null)
                    {
                        chkSelected.Attributes.Add("onClick", gChangeCheckScript);
                    }

                    FilterDrivers(row["SygmacenterNo"].ToString());

                    if (!Convert.IsDBNull(row["DriverId"]))
                    {
                        long DriverId = Convert.ToInt64(row["DriverId"]);
                        ddDriver.DataSource = dtFileredDriver;
                        ddDriver.DataBind();
                        //ddDriver.Items.FindByValue(row["DriverId"].ToString()).Selected = true;
                        for (int i = 0; i < dtFileredDriver.Rows.Count; i++)
                        {
                            if (ddDriver.Items[i].Value == row["DriverId"].ToString())
                            {
                                ddDriver.Items[i].Selected = true;
                                break;
                            }
                        }
                        ddDriver.Items.Insert(0, new ListItem("Choose...", "0"));

                        // Handle Inactive Drivers
                        if (ddDriver.SelectedItem.Text == "Choose...")
                        {
                            string driverName = (string)ATMDB.GetScalar("up_p_getDriversNameForInactive", DriverId);
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
                        foreach (DataRow rtd in dtFileredDriver.Rows)
                        {
                            ListItem liRTD = new ListItem(rtd["DriverName"].ToString(), rtd["DriverId"].ToString());
                            ddDriver.Items.Add(liRTD);
                        }
                        ddHelper.Enabled = false;
                        ddHelper.CssClass = "disabled-drop-down";
                    }

                    if (!Convert.IsDBNull(row["DriverHelperId"]))
                    {
                        long HelperId = Convert.ToInt64(row["DriverHelperId"]);
                        //int val = Convert.ToInt32(row["DriverHelperId"]);
                        ddHelper.DataSource = dtFileredHelper;
                        ddHelper.DataBind();
                        for (int i = 0; i < dtFileredHelper.Rows.Count; i++)
                        {
                            if (ddHelper.Items[i].Value == row["DriverHelperId"].ToString())
                            {
                                ddHelper.Items[i].Selected = true;
                                break;
                            }
                        }
                        ddHelper.Enabled = true;
                        ddHelper.Items.Insert(0, new ListItem("Choose...", "0"));
                        // Handle Inactive Helpers
                        if (ddHelper.SelectedItem.Text == "Choose...")
                        {
                            string helperName = (string)ATMDB.GetScalar("up_p_getDriversNameForInactive", HelperId);
                            ddHelper.Items.Insert(0, new ListItem("** " + helperName + " **", "999"));
                            ddHelper.ClearSelection();
                            ddHelper.Items.FindByValue("999").Selected = true;
                            ddHelper.BackColor = System.Drawing.Color.Gray;
                            ddHelper.ToolTip = helperName + " is currently marked as inactive. Please change the helper if needed!";
                        }
                    }
                    else
                    {
                        ddHelper.Items.Clear();
                        ddHelper.Items.Add(new ListItem("Choose...", "0"));
                        foreach (DataRow rtd in dtFileredHelper.Rows)
                        {
                            ListItem liRTD = new ListItem(rtd["DriverName"].ToString(), rtd["DriverId"].ToString());
                            ddHelper.Items.Add(liRTD);
                        }
                        //ddHelper.Items.FindByText("Choose...").Selected = true;
                        //ddHelper.Enabled = false;
                        //ddHelper.CssClass = "disabled-drop-down";
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

                    if (Convert.ToBoolean(ATMDB.GetScalar("up_p_isRouteStarted", row["FormId"])))
                    {
                        ddDriver.Enabled = false;
                        ddHelper.Enabled = false;
                        ddDriver.CssClass = "disabled-drop-down";
                        ddHelper.CssClass = "disabled-drop-down";
                    }
                    if (Convert.ToInt16(row["StatusId"]) != 0)
                    {
                        ddDriver.Enabled = false;
                        ddHelper.Enabled = false;
                        ddDriver.CssClass = "disabled-drop-down";
                        ddHelper.CssClass = "disabled-drop-down";
                    }
                    if (Convert.ToInt16(row["FormTypeId"]) == 2)
                    {
                        ddDriver.Visible = false;
                        ddHelper.Visible = false;
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

                    ddDriver.Attributes.Add("onchange", string.Format("UpdateDriverDetails(this, 1, {0}, {1});", row["FormId"], e.Row.RowIndex));
                    ddHelper.Attributes.Add("onchange", string.Format("UpdateDriverDetails(this, 2, {0}, {1});", row["FormId"], e.Row.RowIndex));

                    //On each line item add it's ID to a list to be cached
                    activeFormIDs.Add(FormId);

                    break;
            case DataControlRowType.Footer:
                    //when you hit the footer you've exhausted the page, so we can cache the list
                    Cache.Insert("CachedPayrollFormList", activeFormIDs, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 60, 0));
                    break;
            }
        }
        catch (Exception ex)
        {
            log.WriteEntryFormat("FormId = {0}", FormId);
            log.WriteEntryFormat("Exception = {0}", ex.ToString());

        }
    }

    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid)
        {
            e.Cancel = true;
        }
    }

    protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        RowCountBar1.ItemCount = e.AffectedRows;
        pnlButtons.Visible = (e.AffectedRows > 0);
        if (!Convert.IsDBNull(e.Command.Parameters["@weekending"].Value))
        {
            DateTime we = Convert.ToDateTime(e.Command.Parameters["@weekending"].Value);
            if (dteWeekending.Value != we)
            {
                dteWeekending.Value = we;
                Session["ATM_Weekending"] = dteWeekending.Value;
                upWeekending.Update();
            }
        }
    }

    protected void txtFormId_TextChanged(object sender, EventArgs e)
    {
        Session["ATM_FormId"] = txtFormId.Text;
        SetSelectedActions();
        if (txtFormId.Text.Trim() == "")
        {
            dteWeekending.Value = Dates.GetWeekending();
            upSearch.Update();
        }
    }

    protected void ddStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ATM_Status"] = ddStatus.SelectedValue;
        SetSelectedActions();
    }

    private string GetSelectedForms()
    {
        StringBuilder sb = new StringBuilder();
        foreach (GridViewRow row in gvForms.Rows)
        {
            CheckBox chkSelected = (CheckBox)row.FindControl("chkSelected");
            if (chkSelected.Checked)
            {
                sb.AppendFormat(",{0}", gvForms.DataKeys[row.DataItemIndex].Value);
            }
        }
        return sb.ToString();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {

        BindGVFormsData();
    }
    protected void ddFormType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ATM_FormType"] = ddFormType.SelectedValue;
    }
    protected void ddSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ATM_Center"] = ddSygmaCenterNo.SelectedValue;
    }
    protected void txtRouteNo_TextChanged(object sender, EventArgs e)
    {
        Session["ATM_RouteNo"] = txtRouteNo.Text;
    }
    protected void dteWeekEnding_DateChanged(object sender, EventArgs e)
    {
        if (dteWeekending.IsEmpty)
        {
            Session["ATM_Weekending"] = "";
        }
        else
        {
            ClearDateRangeValues();
            Session["ATM_Weekending"] = dteWeekending.Value;
        }
    }
    protected void dteFromDate_DateChanged(object sender, EventArgs e)
    {
        if (txtFromDate.Text != "")
        {
            if (isUserSelectedDateRange == false)
            {
                SetDateRangeToCustom();
                isUserSelectedDateRange = false;
            }
            Session["ATM_FromDate"] = txtFromDate.Text;
        }
        else
        {
            Session["ATM_FromDate"] = "";
        }
    }

    protected void dteToDate_DateChanged(object sender, EventArgs e)
    {
        if (txtToDate.Text != "")
        {
            if (isUserSelectedDateRange == false)
            {
                SetDateRangeToCustom();
                isUserSelectedDateRange = false;
            }
            Session["ATM_ToDate"] = txtToDate.Text;
        }
        else
        {
            Session["ATM_ToDate"] = "";
        }
    }
    protected void txtEmployee_TextChanged(object sender, EventArgs e)
    {
        Session["ATM_Employee"] = txtEmployee.Text;
    }
    protected void txtTractor_TextChanged(object sender, EventArgs e)
    {
        Session["ATM_Tractor"] = txtTractor.Text;
    }
    protected void txtTrailer_TextChanged(object sender, EventArgs e)
    {
        Session["ATM_Trailer"] = txtTrailer.Text;
    }
    protected void ddActualsUpdated_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ATM_ActualsUpdated"] = ddActualsUpdated.SelectedValue;
    }
    protected void btnResetSearch_Click(object sender, EventArgs e)
    {
        ClearSearch();
        BindGVFormsData();
    }



    protected void btnRejected_Click(object sender, EventArgs e)
    {
        ClearSearch();
        ddStatus.SelectedValue = "4";
        ddSelectedAction.Visible = true;
        SetSelectedActions();

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script", "gridviewupdate();", true);
    }
    protected void btnUnApproved_Click(object sender, EventArgs e)
    {
        ClearSearch();
        ddStatus.SelectedValue = "1";
        ddSelectedAction.Visible = true;
        SetSelectedActions();

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script1", "gridviewupdate();", true);
    }
    [WebMethod]
    public static void PM_ChangeStatusAR(string userName, int formId, string newStatus, string notes)
    {
        RunNonQueryFromStatic("ATM", "up_p_ChangeStatus", formId, userName, newStatus, notes);
    }

    /// <summary>
    /// For event validation while clicking Unapproved and rejected
    /// </summary>
    /// <param name="writer"></param>
    protected override void Render(HtmlTextWriter writer)
    {
        ClientScript.RegisterForEventValidation(ddSelectedAction.UniqueID);

        base.Render(writer);
    }
    protected void btnInvisibleNotes_Click(object sender, EventArgs e)
    {
        string formIds = GetSelectedForms();
        if (formIds != "")
        {
            switch (ddSelectedAction.SelectedValue)
            {
                case "D":
                    ATMDB.RunNonQuery("up_p_deleteForms", formIds, UserName);
                    break;
                case "I": //Check In
                    ATMDB.RunNonQuery("up_p_changeStatus", formIds, UserName, 1);
                    break;
                case "O": //Check Out
                    ATMDB.RunNonQuery("up_p_changeStatus", formIds, UserName, 0);
                    break;
                case "A"://Approve
                    ATMDB.RunNonQuery("up_p_changeStatus", formIds, UserName, 3);
                    break;
                case "R": //Reject
                    ATMDB.RunNonQuery("up_p_changeStatus", formIds, UserName, 4, txtNotes.Text);

                    break;
                default:

                    break;
            }

            BindGVFormsData();
            ddSelectedAction.ClearSelection();
        }
    }

    [WebMethod]
    public static int PM_SaveDriverDetails(string userName, int formId, int detailId, int driverid)
    {
        return (int)GetScalarFromStatic("ATM", "up_p_updateDriverDetail", userName, formId, detailId, driverid);
    }

    protected void FilterDrivers(string SygmaCenterNo)
    {
        string filter = "SygmaCenterNo = " + SygmaCenterNo;
        //filter drivers based on center no
        DataRow[] drFiltered = dtDrivers.Select(filter);
        dtFileredDriver = null;
        int var = drFiltered.Length;
        if (drFiltered.Length > 0)
        {
            dtFileredDriver = drFiltered.CopyToDataTable();
        }
        //Filter helpers based on center no
        DataRow[] drFiltered2 = dtHelper.Select(filter);
        dtFileredHelper = null;
        if (drFiltered2.Length > 0)
        {
            dtFileredHelper = drFiltered2.CopyToDataTable();
        }
    }

    private void CalculateDateRanges(string selectedValue)
    {
        if (selectedValue == DateRanges.Custom.ToString())
        {
            selectedValue = DateRanges.ThisWeek.ToString();
        }

        var range = DateRangeCalculator.Calculate(DateTime.Today, selectedValue);

        if (range != null)
        {
            txtFromDate.Text = range.Begin.ToShortDateString();
            txtToDate.Text = range.End.ToShortDateString();
            ClearWeekendingDate();

            return;
        }

        ClearDateRangeValues();
    }

    private void ClearWeekendingDate()
    {
        dteWeekending.Value = DateTime.MinValue;
        upWeekending.Update();
    }

    private void ClearDateRangeValues()
    {
        ddlDateRange.SelectedIndex = 0;
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }

    private void SetDateRangeToCustom()
    {
        ddlDateRange.SelectedValue = DateRanges.Custom.ToString();
    }

    private void BindGVFormsData()
    {
        Cache.Remove("CachedPayrollFormList");
        activeFormIDs.Clear();

        gvForms.DataBind();

        Cache.Insert("CachedPayrollFormList", activeFormIDs, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 60, 0));
    }
}
