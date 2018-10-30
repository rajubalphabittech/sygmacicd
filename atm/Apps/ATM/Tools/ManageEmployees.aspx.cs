using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;
using atm;
using Newtonsoft.Json;
using ATM.Payroll;

public partial class Apps_ATM_Tools_ManageEmployees : ATMPage
{
    private bool gIsApsEnabledSygmaCenterNo;
    private DataTable dtClassifications;
    protected override void LoadATMPage()
    {
        SetPageVariables();
        if (!IsPostBack)
        {
            LoadCenters();
            DataSet dsClassification = ATMDB.GetDataSet("up_p_getCentersAndClassifications");
            dtClassifications = dsClassification.Tables[1];
        }
    }

    protected void ddProgSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        CenterSelectionIndexChanged();
    }

    protected void CenterSelectionIndexChanged()
    {
        if (ProgSortExpression == "")
        {
            ProgSortExpression = "HireDate";
            ProgSortDir = SortDirection.Descending;
        }
        SetEmployees();
    }

    protected void txtProgName_TextChanged(object sender, EventArgs e)
    {
        SetEmployees();
    }

    private void SetPageVariables()
    {
        AddClientVariable("gUserName", UserName);
    }

    private void LoadCenters()
    {
        DataView dv = ATMDB.GetDataView("up_getCenters", UserName);

        ddProgSygmaCenterNo.DataSource = dv;
        ddProgSygmaCenterNo.DataBind();
        ddProgSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
        if (ddProgSygmaCenterNo.Items.Count == 2)
        {
            ddProgSygmaCenterNo.SelectedIndex = 1;
            CenterSelectionIndexChanged();
        }
    }

    protected void btnRefreshEmployees_Click(object sender, EventArgs e)
    {
        SetEmployees();
    }

    private void SetEmployees()
    {
        if (ddProgSygmaCenterNo.SelectedIndex > 0)
        {
            gIsApsEnabledSygmaCenterNo = Convert.ToBoolean(ATMDB.GetScalar("up_p_isApsEnabledCenter", ddProgSygmaCenterNo.SelectedValue));
            DataSet dsClassification = ATMDB.GetDataSet("up_p_getCentersAndClassifications");
            dtClassifications = dsClassification.Tables[1];
            DataView dv = ATMDB.GetDataView("up_getEmployees", UserName, ddProgSygmaCenterNo.SelectedValue);
            AddSort(dv);
            AddNameFilter(dv);
            gvEmployees.DataSource = dv;
            gvEmployees.DataBind();
            pnlProgression.Visible = true;
            lblEmployeeCount.Text = dv.Count.ToString();
        }
        else
        {
            pnlProgression.Visible = false;
        }
    }

    private void AddSort(DataView dv)
    {
        if (ProgSortExpression != "")
            dv.Sort = string.Format("{0} {1}", ProgSortExpression, WebCommon.GetDBSortDirection(ProgSortDir));
    }
    private void AddNameFilter(DataView dv)
    {
        string[] names = txtProgName.Text.Trim().Replace(".", "").Replace(",", "").Replace("'", "''").Replace("%", "").Replace("*", "").Split(' ');
        if (names.Length > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in names)
            {
                if (sb.Length > 0)
                    sb.Append(" OR ");
                sb.AppendFormat("WebDisplay like '%{0}%'", name);
            }
            dv.RowFilter = sb.ToString();
        }
    }

    [WebMethod]
    public static string PM_SaveProgressionRate(string userName, int employeeId, decimal rate)
    {
        RunNonQueryFromStatic("ATM", "up_p_setProgressionRate", userName, employeeId, (rate / 100));
        return "";
    }

    [WebMethod]
    public static string PM_SaveGuaranteedPay(string userName, int employeeId, decimal GPay)
    {
        RunNonQueryFromStatic("ATM", "up_p_setGuaranteedPay", userName, employeeId, GPay);
        return GPay.ToString("0.##");
    }

    [WebMethod]
    public static string[] PM_SaveEffectiveHireDate(string userName, int employeeId, DateTime effHireDate)
    {
        DataSet ds = GetDataSetFromStatic("ATM", "up_p_setEffectiveHireDate", userName, employeeId, effHireDate);
        string[] hd = new string[3];
        if (ds.Tables.Count > 0)
        {
            hd[0] = effHireDate.ToShortDateString();
            hd[1] = ds.Tables[0].Rows[0][0].ToString();
            hd[2] = Convert.ToDecimal(ds.Tables[0].Rows[0][1]).ToString("0.####");
        }
        return hd;
    }

    [WebMethod]
    public static void PM_SaveApsExceptionEnabled(string userName, int employeeId, bool ApsExceptionEnabled)
    {
        RunNonQueryFromStatic("ATM", "up_p_setEmployeeApsException", userName, employeeId, ApsExceptionEnabled);
    }

    [WebMethod]
    public static string[] PM_SaveApplyEHDateChanged(string userName, int employeeId, bool isEHDChangable)
    {
        string[] rv = new string[3];
        DataSet ds = GetDataSetFromStatic("ATM", "up_p_setEmployeeEHDateChangable", userName, employeeId, isEHDChangable);
        if (ds.Tables.Count > 0)
        {
            rv[0] = (Convert.ToDateTime(ds.Tables[0].Rows[0][0])).ToShortDateString();
            rv[1] = ds.Tables[0].Rows[0][1].ToString();
            rv[2] = Convert.ToDecimal(ds.Tables[0].Rows[0][2]).ToString("0.####");
        }
        return rv;
    }

    [WebMethod]
    public static void PM_SaveGpExceptionEnabled(string userName, int employeeId, bool GpExceptionEnabled)
    {
        RunNonQueryFromStatic("ATM", "up_p_setEmployeeGpException", userName, employeeId, GpExceptionEnabled);
    }

    [WebMethod]
    public static string PM_SaveEmployeeClassification(string userName, int employeeId, int classificationId)
    {
        RunNonQueryFromStatic("ATM", "up_p_setEmployeeClassification", userName, employeeId, classificationId);
        return "";
    }

    //[WebMethod]
    //public static string PM_SaveVacationRate(string userName, int employeeId, decimal vacationRate)
    //{
    //    RunNonQueryFromStatic("ATM", "up_p_setVacationRate", userName, employeeId, vacationRate);
    //    return "";
    //}

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

    protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                DataRowView row = (DataRowView)e.Row.DataItem;
                TextBox txtProgRate = (TextBox)e.Row.FindControl("txtProgRate");
                TextBox txtGuaranteedPay = (TextBox)e.Row.FindControl("txtGuaranteedPay");
                TextBox txtEffHireDate = (TextBox)e.Row.FindControl("txtEffHireDate");
                //Label lblGuaranteedPay = (Label)e.Row.FindControl("lblGuaranteedPay");
                Label lblProgRate = (Label)e.Row.FindControl("lblProgRate");
                Label lblTenure = (Label)e.Row.FindControl("lblTenure");
                CheckBox chkAPSException = (CheckBox)e.Row.FindControl("chkAPSExcptn");
                CheckBox chkGPExcptn = (CheckBox)e.Row.FindControl("chkGPExcptn");
                CheckBox chkAllowHireDateChange = (CheckBox)e.Row.FindControl("chkAllowHireDateChange");
                DropDownList ddClassification = (DropDownList)e.Row.FindControl("ddClassification");
                //TextBox txtVacationRate = (TextBox)e.Row.FindControl("txtVacationRate");
                if (!Convert.IsDBNull(row["TenureDisplay"]))
                {
                    lblTenure.Text = row["TenureDisplay"].ToString();
                }
                if (!Convert.IsDBNull(row["ProgressionRate"]))
                {
                    txtProgRate.Text = (Convert.ToDecimal(row["ProgressionRate"]) * 100).ToString("0.####");
                    lblProgRate.Text = (Convert.ToDecimal(row["ProgressionRate"]) * 100).ToString("0.####");
                }
                if (!Convert.IsDBNull(row["GuaranteedPay"]))
                {
                    txtGuaranteedPay.Text = Convert.ToDecimal(row["GuaranteedPay"]).ToString("0.##");
                    txtGuaranteedPay.Attributes.Add("OrigVal", Convert.ToDecimal(row["GuaranteedPay"]).ToString("0.##"));
                    //lblGuaranteedPay.Text = Convert.ToDecimal(row["GuaranteedPay"]).ToString("0.##");
                }
                else
                {
                    txtGuaranteedPay.Attributes.Add("OrigVal", "");
                }

                if (!Convert.IsDBNull(row["EffectiveHireDate"]))
                {
                    txtEffHireDate.Text = Convert.ToDateTime(row["EffectiveHireDate"]).ToShortDateString();
                    txtEffHireDate.Attributes.Add("OrigVal", Convert.ToDateTime(row["EffectiveHireDate"]).ToShortDateString());
                }
                else
                {
                    txtEffHireDate.Attributes.Add("OrigVal", "");
                }

                if (!Convert.IsDBNull(row["IsEffHireDateChangable"]))
                {
                    chkAllowHireDateChange.Checked = Convert.ToBoolean(row["IsEffHireDateChangable"]);
                    if (Convert.ToBoolean(row["IsEffHireDateChangable"]))
                    {
                        txtEffHireDate.Enabled = true;
                    }
                    else
                    {
                        txtEffHireDate.Enabled = false;
                    }
                }

                if (!Convert.IsDBNull(row["GPException"]))
                {
                    chkGPExcptn.Checked = Convert.ToBoolean(row["GPException"]);
                    if (Convert.ToBoolean(row["GPException"]))
                    {
                        txtGuaranteedPay.Enabled = true;
                        txtGuaranteedPay.BackColor = System.Drawing.Color.White;
                        //lblGuaranteedPay.Style["display"] = "none";
                    }
                    else
                    {
                        txtGuaranteedPay.Enabled = false;
                        txtGuaranteedPay.BackColor = System.Drawing.Color.LightGray;
                        //lblGuaranteedPay.Style["display"] = "block";
                    }
                }

                if (!Convert.IsDBNull(row["ApsException"]))
                {
                    chkAPSException.Checked = Convert.ToBoolean(row["ApsException"]);
                    if (Convert.ToBoolean(row["ApsException"]))
                    {
                        txtProgRate.Style["display"] = "block";
                        lblProgRate.Style["display"] = "none";
                    }
                    else
                    {
                        txtProgRate.Style["display"] = "none";
                        lblProgRate.Style["display"] = "block";
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
                //if (!Convert.IsDBNull(row["VacationRate"]))
                //{
                //    txtVacationRate.Text = (Convert.ToDouble(row["VacationRate"])).ToString("0.00##");
                //}

                // set data attributes
                chkAllowHireDateChange.InputAttributes.Add("data-id", row["EmployeeId"].ToString());
                chkGPExcptn.InputAttributes.Add("data-id", row["EmployeeId"].ToString());
                chkAPSException.InputAttributes.Add("data-id", row["EmployeeId"].ToString());

                txtEffHireDate.Attributes.Add("data-id", row["EmployeeId"].ToString());
                txtGuaranteedPay.Attributes.Add("data-id", row["EmployeeId"].ToString());
                txtProgRate.Attributes.Add("data-id", row["EmployeeId"].ToString());
                lblProgRate.Attributes.Add("data-id", row["EmployeeId"].ToString());
                lblTenure.Attributes.Add("data-id", row["EmployeeId"].ToString());


                txtProgRate.Attributes.Add("onchange", string.Format("UpdateProgressionRate(this,{0});", row["EmployeeId"]));
                txtGuaranteedPay.Attributes.Add("onchange", string.Format("UpdateGuaranteedPay(this,{0});", row["EmployeeId"]));
                txtEffHireDate.Attributes.Add("onchange", string.Format("UpdateEffectiveHireDate(this,{0});", row["EmployeeId"]));
                chkAPSException.Attributes.Add("onclick", string.Format("ApsExceptionChanged(this,{0});", row["EmployeeId"]));
                chkAllowHireDateChange.Attributes.Add("onclick", string.Format("ApplyEHDateChanged(this,{0});", row["EmployeeId"]));
                chkGPExcptn.Attributes.Add("onclick", string.Format("GpExceptionChanged(this,{0});", row["EmployeeId"]));
                ddClassification.Attributes.Add("onchange", string.Format("UpdateEmployeeClassification(this,{0});", row["EmployeeId"]));
                //txtVacationRate.Attributes.Add("onchange", string.Format("UpdateVacationRate(this,{0});", row["EmployeeId"]));
                if (!gIsApsEnabledSygmaCenterNo)
                {
                    txtProgRate.Style["display"] = "block";
                    lblProgRate.Style["display"] = "none";
                    chkAPSException.Enabled = false;
                }
                break;
        }
    }


    protected void gvEmployees_Init(object sender, EventArgs e)
    {
        SetEmployeesGridConfiguration();
    }

    private void SetEmployeesGridConfiguration()
    {
        DataSet dsSelectedColumns = ATMDB.GetDataSet("up_p_getProfileCustom", UserName, "MANAGEEMPLOYEES");

        if ((dsSelectedColumns.Tables[0].Rows != null) && (dsSelectedColumns.Tables[0].Rows.Count > 0))
        {
            DataTable dtSelectedCols = JsonConvert.DeserializeObject<DataSet>(dsSelectedColumns.Tables[0].Rows[0]["Value"].ToString()).Tables[0];

            // Start Grid column reordering
            List<string> columnOrder = CreateColumnOrder(dtSelectedCols);

            for (int columnOrderCounter = columnOrder.Count - 1; columnOrderCounter >= 0; columnOrderCounter--)
            {
                for (int columnCounter = 0; columnCounter <= gvEmployees.Columns.Count - 1; columnCounter++)
                {
                    if (gvEmployees.Columns[columnCounter].HeaderText == columnOrder[columnOrderCounter])
                    {
                        var gridColumn = gvEmployees.Columns[columnCounter];
                        gvEmployees.Columns.RemoveAt(columnCounter);
                        gvEmployees.Columns.Insert(0, gridColumn);
                    }
                }
            }
            // Stop Grid column reordering

            // Start Grid column hiding
            for (int columnCounter = 0; columnCounter <= gvEmployees.Columns.Count - 1; columnCounter++)
            {
                DataRow selectedDataTableRow = dtSelectedCols.Select("ColumnIdentifier='" + gvEmployees.Columns[columnCounter].HeaderText + "'").FirstOrDefault();

                if (selectedDataTableRow == null)
                {
                    gvEmployees.Columns[columnCounter].HeaderStyle.CssClass = "hiddencol";
                    gvEmployees.Columns[columnCounter].ItemStyle.CssClass = "hiddencol";
                }
            }
            // End Grid column hiding
        }
    }


    private List<string> CreateColumnOrder(DataTable dtSelectedCols)
    {
        List<string> finalColumn = new List<string>();
        string[,] columns = PayrollCommon.ManageEmployeesColumns;

        DataTable dtSelectedColsOrdered = dtSelectedCols.AsEnumerable().OrderBy(r => int.Parse(r.Field<String>("DisplayOrder"))).CopyToDataTable();

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

    protected void gvEmployees_Sorting(object sender, GridViewSortEventArgs e)
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
        SetEmployees();
    }
}