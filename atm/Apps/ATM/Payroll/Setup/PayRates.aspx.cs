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

public partial class Apps_ATM_Payroll_Setup_PayRates : ATMPage
{
    private bool gIsApsEnabledSygmaCenterNo;
    //private bool gSetFocusNextTo =  false;
    private bool gIsAddNewBand = false;
    private DataTable dtClassifications;
    protected override void LoadATMPage()
    {
        SetPageVariables();
        if (!IsPostBack)
        {
            LoadCenters();
            LoadPayScales();
            LoadCentersAndClass();
            //DataSet dsClassification = ATMDB.GetDataSet("up_p_getCentersAndClassifications");
            //dtClassifications = dsClassification.Tables[1];
        }
    }
    private void SetPageVariables()
    {
        AddClientVariable("gUserName", UserName);
        AddClientVariable("ddSygmaCenterNo", ddSygmaCenterNo.ClientID);
        AddClientVariable("ddPayScale", ddPayScale.ClientID);
        AddClientVariable("ddApsSygmaCenterNo", ddApsSygmaCenterNo.ClientID);
        AddClientVariable("ddHPSygmaCenterNo", ddHPSygmaCenterNo.ClientID);
        AddClientVariable("ddClassification", ddClassification.ClientID);
    }

    private void LoadCenters()
    {
        DataView dv = ATMDB.GetDataView("up_getCenters", UserName);

        ddSygmaCenterNo.DataSource = dv;
        ddSygmaCenterNo.DataBind();
        ddSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
        if (ddSygmaCenterNo.Items.Count == 2)
        {
            ddSygmaCenterNo.SelectedIndex = 1;
            SetPayRateTypes();
        }

        //ddProgSygmaCenterNo.DataSource = dv;
        //ddProgSygmaCenterNo.DataBind();
        //ddProgSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));

        ddApsSygmaCenterNo.DataSource = dv;
        ddApsSygmaCenterNo.DataBind();
        ddApsSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
        if (ddApsSygmaCenterNo.Items.Count == 2)
        {
            ddApsSygmaCenterNo.SelectedIndex = 1;
            SetPreviewRateAndSchedule();
        }

    }
    private void LoadCentersAndClass()
    {
        DataSet ds = ATMDB.GetDataSet("up_p_getCentersAndClassifications", UserName);

        ddHPSygmaCenterNo.DataSource = ds.Tables[0];
        ddHPSygmaCenterNo.DataBind();
        ddHPSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));

        if (ddHPSygmaCenterNo.Items.Count == 2)
        {
            ddHPSygmaCenterNo.SelectedIndex = 1;
            SetHourlyRate();
        }

        ddClassification.DataSource = ds.Tables[1];
        ddClassification.DataBind();
        ddClassification.Items.Insert(0, new ListItem("Choose...", ""));
    }

    private void LoadPayScales()
    {
        ddPayScale.DataSource = ATMDB.GetDataView("up_p_getPayScales");
        ddPayScale.DataBind();
        ddPayScale.Items.Insert(0, new ListItem("Choose...", ""));
    }

    private void SetPayRateTypes()
    {
        if (ddSygmaCenterNo.SelectedValue != "" && ddPayScale.SelectedValue != "")
        {
            pnlRates.Visible = true;
            gvPayRates.DataSource = ATMDB.GetDataView("up_p_getCenterPayScaleRates", ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue);
            gvPayRates.DataBind();

        }
        else
        {
            pnlRates.Visible = false;
        }
    }

    protected void ddSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetPayRateTypes();
    }

    private void SetPreviewRateAndSchedule()
    {
        if (ddApsSygmaCenterNo.SelectedValue != "")
        {

            cbAps.Visible = true;
            DataSet dsAps = ATMDB.GetDataSet("up_getApsFlagAndPreviewPay", ddApsSygmaCenterNo.SelectedValue);
            if (Convert.ToBoolean(dsAps.Tables[0].Rows[0][0]))
            {
                cbAps.Checked = true;
                txtPrevNetPay.Visible = true;
                lblPreview.Visible = true;
                pnlApsSchedule.Visible = true;
                if (dsAps.Tables[1].Rows.Count > 0 && !Convert.IsDBNull(dsAps.Tables[1].Rows[0][0]))
                {
                    txtPrevNetPay.Text = dsAps.Tables[1].Rows[0][0].ToString();
                }
                txtPrevNetPayCopy.Text = txtPrevNetPay.Text;
                SetAPSchedule();
            }
            else
            {
                cbAps.Checked = false;
                txtPrevNetPay.Visible = false;
                lblPreview.Visible = false;
                pnlApsSchedule.Visible = false;
            }
        }
        else
        {
            txtPrevNetPay.Visible = false;
            lblPreview.Visible = false;
            cbAps.Visible = false;
            pnlApsSchedule.Visible = false;
        }
    }

    private void SetHourlyRate()
    {
        if (ddHPSygmaCenterNo.SelectedValue != "" && ddClassification.SelectedValue != "")
        {
            decimal rate = Convert.ToDecimal(ATMDB.GetScalar("up_getHourlyRate", ddHPSygmaCenterNo.SelectedValue, ddClassification.SelectedValue));
            if (rate != 0)
            {
                txtHourlyRate.Enabled = true;
                txtHourlyRate.Text = Convert.ToString(rate);
                txtPrevHourlyPay.Text = txtHourlyRate.Text;
            }
            else
            {
                txtHourlyRate.Enabled = true;
                txtHourlyRate.Text = "";
                txtPrevHourlyPay.Text = txtHourlyRate.Text;
            }
        }
        else
        {
            txtHourlyRate.Text = "";
            txtPrevHourlyPay.Text = txtHourlyRate.Text;
            txtHourlyRate.Enabled = false;
        }
    }

    protected void ddApsSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetPreviewRateAndSchedule();
    }

    protected void ddHPSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetHourlyRate();
    }

    protected void ddClassification_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetHourlyRate();
    }

    protected void rptApsSchedule_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = (DataRowView)e.Item.DataItem;
                int seqNo = Convert.ToInt32(row["SeqNo"]);
                int nextToMonth = Convert.ToInt32(row["NextToMonth"]);
                int thisFromMonth = Convert.IsDBNull(row["FromMonth"]) ? 0 : Convert.ToInt32(row["FromMonth"]);
                int hasFromMonth = Convert.IsDBNull(row["FromMonth"]) ? 0 : 1;
                int nextSeqNo = Convert.IsDBNull(row["NextSeqNo"]) ? 0 : Convert.ToInt32(row["NextSeqNo"]);
                decimal nextProgRate = Convert.ToDecimal(row["NextProgRate"]);
                decimal prevProgRate = Convert.ToDecimal(row["PrevProgRate"]);
                decimal prevNetPay = Convert.ToDecimal(row["PrevNetPay"]);
                decimal nextNetPay = Convert.ToDecimal(row["NextNetPay"]);
                string payBandName = Convert.ToString(row["PayBandName"]);
                Label lblApsBandFrom = (Label)e.Item.FindControl("lblApsBandFrom");
                TextBox txtApsBandTo = (TextBox)e.Item.FindControl("txtApsBandTo");
                Label lblApsProgRate = (Label)e.Item.FindControl("lblApsProgRate");
                TextBox txtApsProgRate = (TextBox)e.Item.FindControl("txtApsProgRate");
                Label lblNetPay = (Label)e.Item.FindControl("lblNetPay");
                TextBox txtNetPay = (TextBox)e.Item.FindControl("txtNetPay");
                Label lblPayBandName = (Label)e.Item.FindControl("lblPayBandName");
                TextBox txtPayBandName = (TextBox)e.Item.FindControl("txtPayBandName");

                //txtApsBandTo
                //Repeater rptEmployeePayments = (Repeater)e.Item.FindControl("rptEmployeePayments");
                //DropDownList ddPayScale = null;

                //if (IsOpen)
                //{
                if (seqNo != 1)
                {
                    lblApsBandFrom.Text = Convert.IsDBNull(row["FromMonth"]) ? "" : row["FromMonth"].ToString();
                    txtApsBandTo.Text = Convert.IsDBNull(row["ToMonth"]) ? "" : row["ToMonth"].ToString();
                    txtApsBandTo.Attributes.Add("origVal", txtApsBandTo.Text);
                    lblApsProgRate.Text = Convert.IsDBNull(row["RatePercent"]) ? "" : row["RatePercent"].ToString();
                    txtApsProgRate.Text = Convert.IsDBNull(row["RatePercent"]) ? "" : row["RatePercent"].ToString();
                    txtApsProgRate.Attributes.Add("origVal", txtApsProgRate.Text);
                    lblApsProgRate.Style["display"] = "none";
                    lblNetPay.Text = Convert.IsDBNull(row["NetPay"]) ? "" : " " + row["NetPay"].ToString();
                    txtNetPay.Text = Convert.IsDBNull(row["NetPay"]) ? "" : row["NetPay"].ToString();
                    txtNetPay.Attributes.Add("origVal", txtNetPay.Text);
                    lblNetPay.Style["display"] = "none";
                    lblPayBandName.Text = Convert.IsDBNull(row["PayBandName"]) ? "" : row["PayBandName"].ToString();
                    txtPayBandName.Text = Convert.IsDBNull(row["PayBandName"]) ? "" : row["PayBandName"].ToString();
                    txtPayBandName.Attributes.Add("origVal", txtPayBandName.Text);
                    lblPayBandName.Style["display"] = "none";

                }
                else
                {
                    lblApsBandFrom.Text = Convert.IsDBNull(row["FromMonth"]) ? "" : row["FromMonth"].ToString();
                    txtApsBandTo.Text = Convert.IsDBNull(row["ToMonth"]) ? "" : row["ToMonth"].ToString();
                    txtApsBandTo.Attributes.Add("origVal", txtApsBandTo.Text);
                    lblApsProgRate.Text = Convert.IsDBNull(row["RatePercent"]) ? "" : row["RatePercent"].ToString();
                    txtApsProgRate.Text = Convert.IsDBNull(row["RatePercent"]) ? "" : row["RatePercent"].ToString();
                    txtApsProgRate.Attributes.Add("origVal", txtApsProgRate.Text);
                    txtApsProgRate.Style["display"] = "none";
                    lblNetPay.Text = Convert.IsDBNull(row["NetPay"]) ? "" : " " + row["NetPay"].ToString();
                    txtNetPay.Text = Convert.IsDBNull(row["NetPay"]) ? "" : row["NetPay"].ToString();
                    txtNetPay.Attributes.Add("origVal", txtNetPay.Text);
                    txtNetPay.Style["display"] = "none";
                    lblPayBandName.Text = Convert.IsDBNull(row["PayBandName"]) ? "" : row["PayBandName"].ToString();
                    txtPayBandName.Text = Convert.IsDBNull(row["PayBandName"]) ? "" : row["PayBandName"].ToString();
                    txtPayBandName.Attributes.Add("origVal", txtPayBandName.Text);
                    //txtPayBandName.Style["display"] = "none";
                    lblPayBandName.Style["display"] = "none";

                }
                if (gIsAddNewBand)
                {
                    txtApsBandTo.Focus();
                }
                //if (gSetFocusNextTo)
                //{
                //    txtApsBandTo.Focus();
                //    txtApsBandTo.Attributes.Add("onfocusin", " select();");
                //    gSetFocusNextTo = false;
                //}
                //if(HttpContext.Current.Session["CurrentSeqNo"] != null && HttpContext.Current.Session["CurrentFieldNo"] != null){
                //    if (seqNo == Convert.ToInt32(HttpContext.Current.Session["CurrentSeqNo"]))
                //    {
                //        if (Convert.ToInt32(HttpContext.Current.Session["CurrentFieldNo"]) == 1)
                //        {
                //            txtApsProgRate.Focus();
                //            txtApsProgRate.Attributes.Add("onfocusin", " select();");
                //        }
                //        else if(Convert.ToInt32(HttpContext.Current.Session["CurrentFieldNo"]) == 2)
                //        {
                //            gSetFocusNextTo = true;
                //        }
                //        else if(Convert.ToInt32(HttpContext.Current.Session["CurrentFieldNo"]) == 3)
                //        {
                //            gSetFocusNextTo = true;
                //        }
                //    }
                //    else if (Convert.ToInt32(HttpContext.Current.Session["CurrentSeqNo"]) == 1 && seqNo == 2)
                //    {
                //        txtApsBandTo.Focus();
                //        txtApsBandTo.Attributes.Add("onfocusin", " select();");
                //    }
                //}
                txtApsBandTo.Attributes.Add("onchange", string.Format("SetToMonth(this,{0}, {1}, {2}, {3});", nextToMonth, seqNo, thisFromMonth, hasFromMonth));
                txtApsProgRate.Attributes.Add("onchange", string.Format("SetApsProgRate(this,{0}, {1}, {2}, {3});", seqNo, prevProgRate, nextProgRate, nextSeqNo));
                txtNetPay.Attributes.Add("onchange", string.Format("SetApsNetPay(this,{0}, {1}, {2}, {3});", seqNo, prevNetPay, nextNetPay, nextSeqNo));
                txtPayBandName.Attributes.Add("onchange", string.Format("SetApsPayRateName(this,{0});", seqNo));

                //txtApsBandTo.Attributes.Add("onFocus", string.Format("SetCurrentFocus({0}, {1});", seqNo, 1));
                //txtApsProgRate.Attributes.Add("onFocus", string.Format("SetCurrentFocus({0}, {1});", seqNo, 2));
                //txtNetPay.Attributes.Add("onFocus", string.Format("SetCurrentFocus({0}, {1});", seqNo, 3));
                //ddPayScale = (DropDownList)e.Item.FindControl("ddPayScale");
                //ddPayScale.Visible = true;
                //ddPayScale.DataSource = row.DataView.Table.DataSet.Tables["PayScales"].DefaultView;
                //ddPayScale.DataBind();
                //if (ddPayScale.Items.Count > 0)
                //{
                //    if (!Convert.IsDBNull(row["PayScaleId"]))
                //    {
                //        bool selected = WebCommon.SelectListValue(ddPayScale, row["PayScaleId"].ToString());
                //    }
                //    ddPayScale.Attributes.Add("onChange", string.Format("ChangePayScale(this, {0}, false); return false;", employeeId));
                //    ddPayScale.Attributes.Add("prev", (ddPayScale.SelectedIndex == -1) ? "0" : ddPayScale.SelectedIndex.ToString());  //need to keep track of the previous selection
                //}
                //}
                //else
                //{
                //Label lblPayScale = (Label)e.Item.FindControl("lblPayScale");
                //lblPayScale.Text = row["PayScaleDisplay"].ToString();
                //lblPayScale.Visible = true;
                //}

                //SetPayments(employeeId, rptEmployeePayments, ddPayScale);
                break;
        }

    }

    protected void ddPayScale_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetPayRateTypes();
    }

    //protected void btnRefreshEmployees_Click(object sender, EventArgs e)
    //{
    //    SetEmployees();
    //}

    protected void btnRefreshSchedule_Click(object sender, EventArgs e)
    {
        gIsAddNewBand = false;
        SetPreviewRateAndSchedule();
    }

    protected void btnAddBandRefreshSchedule_Click(object sender, EventArgs e)
    {
        gIsAddNewBand = true;
        SetPreviewRateAndSchedule();
    }

    private void SetAPSchedule()
    {
        if (ddApsSygmaCenterNo.SelectedIndex > 0)
        {
            DataView dv = ATMDB.GetDataView("up_getAutoProgressionSchedule", ddApsSygmaCenterNo.SelectedValue);
            pnlApsSchedule.Visible = true;
            rptApsSchedule.DataSource = dv;
            rptApsSchedule.DataBind();
            hlAddProgressionBand.Style["display"] = "block";
            //hlAddProgressionBand.Attributes.Add("onmouseover",  "this.style.backgroundColor= 'FFF111'");
            //hlAddProgressionBand.Attributes.Add("onmouseout",  "this.style.backgroundColor= 'Navy'");
            if (dv.Table.Rows.Count > 0)
            {
                hlClearProgressionBand.Style["display"] = "block";
                lblClearAll.Style["display"] = "block";
                hlRemoveLastBand.Style["display"] = "block";
                lblRemove.Style["display"] = "block";
                hlUpdateNow.Style["display"] = "block";
                lblUpdateNow.Style["display"] = "block";
            }
            else
            {
                hlClearProgressionBand.Style["display"] = "none";
                lblClearAll.Style["display"] = "none";
                hlRemoveLastBand.Style["display"] = "none";
                lblRemove.Style["display"] = "none";
                hlUpdateNow.Style["display"] = "none";
                lblUpdateNow.Style["display"] = "none";
            }
        }
        else
        {
            pnlApsSchedule.Visible = false;
            hlAddProgressionBand.Style["display"] = "none";
            hlClearProgressionBand.Style["display"] = "none";
            lblClearAll.Style["display"] = "none";
            hlRemoveLastBand.Style["display"] = "none";
            lblRemove.Style["display"] = "none";
            hlUpdateNow.Style["display"] = "none";
            lblUpdateNow.Style["display"] = "none";
        }
    }

    //protected void ddProgSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ProgSortExpression == "")
    //    {
    //        ProgSortExpression = "HireDate";
    //        ProgSortDir = SortDirection.Descending;
    //    }
    //    SetEmployees();
    //}
    //private void SetEmployees()
    //{
    //    if (ddProgSygmaCenterNo.SelectedIndex > 0)
    //    {
    //        gIsApsEnabledSygmaCenterNo = Convert.ToBoolean(ATMDB.GetScalar("up_p_isApsEnabledCenter", ddProgSygmaCenterNo.SelectedValue));
    //        DataSet dsClassification = ATMDB.GetDataSet("up_p_getCentersAndClassifications");
    //        dtClassifications = dsClassification.Tables[1];
    //        DataView dv = ATMDB.GetDataView("up_getEmployees", UserName, ddProgSygmaCenterNo.SelectedValue);
    //        AddSort(dv);
    //        AddNameFilter(dv);
    //        gvEmployees.DataSource = dv;
    //        gvEmployees.DataBind();
    //        pnlProgression.Visible = true;
    //        lblEmployeeCount.Text = dv.Count.ToString();
    //    }
    //    else
    //    {
    //        pnlProgression.Visible = false;
    //    }
    //}
    //private void AddSort(DataView dv)
    //{
    //    if (ProgSortExpression != "")
    //        dv.Sort = string.Format("{0} {1}", ProgSortExpression, WebCommon.GetDBSortDirection(ProgSortDir));
    //}
    //private void AddNameFilter(DataView dv)
    //{
    //    string[] names = txtProgName.Text.Trim().Replace(".", "").Replace(",", "").Split(' ');
    //    if (names.Length > 0)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        foreach (string name in names)
    //        {
    //            if (sb.Length > 0)
    //                sb.Append(" OR ");
    //            sb.AppendFormat("WebDisplay like '%{0}%'", name);
    //        }
    //        dv.RowFilter = sb.ToString();
    //    }
    //}
    [WebMethod]
    public static void PM_SaveRate(string userName, int sygmaCenterNo, int payScaleId, int rateTypeId, decimal rate)
    {
        RunNonQueryFromStatic("ATM", "up_p_setCenterPayScaleRate", userName, sygmaCenterNo, payScaleId, rateTypeId, rate);
    }

    [WebMethod]
    public static void PM_SaveRateEnabled(string userName, int sygmaCenterNo, int payScaleId, int rateTypeId, bool enabled)
    {
        RunNonQueryFromStatic("ATM", "up_p_setCenterPayScaleOptions", userName, sygmaCenterNo, payScaleId, rateTypeId, enabled);
    }

    [WebMethod]
    public static void PM_SetIncludeInBase(string userName, int sygmaCenterNo, int payScaleId, int rateTypeId, bool includeInBase)
    {
        RunNonQueryFromStatic("ATM", "up_p_setCenterPayScaleOptions", userName, sygmaCenterNo, payScaleId, rateTypeId, null, includeInBase);
    }

    [WebMethod]
    public static void PM_SetIncludeInGuaranteedPay(string userName, int sygmaCenterNo, int payScaleId, int rateTypeId, bool includeInGuaranteedPay)
    {
        RunNonQueryFromStatic("ATM", "up_p_setCenterPayScaleOptions", userName, sygmaCenterNo, payScaleId, rateTypeId, null, null, null, includeInGuaranteedPay);
    }


    //[WebMethod]
    //public static string PM_SetToMonth(string userName, int sygmaCenterNo, int seqNo, int toMonth)
    //{
    //    RunNonQueryFromStatic("ATM", "up_p_SetApsToMonth", userName, sygmaCenterNo, seqNo, toMonth);
    //    return "0";
    //}

    [WebMethod]
    public static string[] PM_SetToMonth(string userName, int sygmaCenterNo, int seqNo, int toMonth)
    {
        string[] rv = new string[2];
        DataSet ds = GetDataSetFromStatic("ATM", "up_p_SetApsToMonth", userName, sygmaCenterNo, seqNo, toMonth);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            rv[0] = (row["FromMonth"]).ToString();
            rv[1] = (row["SeqNo"]).ToString();
        }
        else
        {
            rv[0] = "0";
            rv[1] = "0";
        }
        return rv;
    }

    [WebMethod]
    public static string[] PM_SetApsProgRateNetPay(string userName, int sygmaCenterNo, int seqNo, decimal progRate, int isProgRate)
    {
        string[] rv = new string[2];
        DataSet ds = GetDataSetFromStatic("ATM", "up_p_SetApsProgRateNetPay", userName, sygmaCenterNo, seqNo, progRate, isProgRate);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            rv[0] = (row[0]).ToString();
            rv[1] = (row[1]).ToString();
        }
        else
        {
            rv[0] = "0";
            rv[1] = "0";
        }
        return rv;
    }

    [WebMethod]
    public static void PM_SetPayRateName(string userName, int sygmaCenterNo, int seqNo, string payBandName)
    {
        RunNonQueryFromStatic("ATM", "up_p_SetApsPayBandName", userName, sygmaCenterNo, seqNo, payBandName);

    }


    [WebMethod]
    public static void PM_SetProgressionRateApplies(string userName, int sygmaCenterNo, int payScaleId, int rateTypeId, bool progRateApplies)
    {
        RunNonQueryFromStatic("ATM", "up_p_setCenterPayScaleOptions", userName, sygmaCenterNo, payScaleId, rateTypeId, null, null, progRateApplies);
    }

    [WebMethod]
    public static string PM_SaveProgressionRate(string userName, int employeeId, decimal rate)
    {
        RunNonQueryFromStatic("ATM", "up_p_setProgressionRate", userName, employeeId, (rate / 100));
        return "";
    }

    [WebMethod]
    public static string PM_SaveEmployeeClassification(string userName, int employeeId, int classificationId)
    {
        RunNonQueryFromStatic("ATM", "up_p_setEmployeeClassification", userName, employeeId, classificationId);
        return "";
    }

    /*[WebMethod]
    public static string PM_SaveVacationRate(string userName, int employeeId, decimal vacationRate)
    {
        RunNonQueryFromStatic("ATM", "up_p_setVacationRate", userName, employeeId, vacationRate);
        return "";
    }*/

    [WebMethod]
    public static string PM_SaveGuaranteedPay(string userName, int employeeId, decimal GPay)
    {
        RunNonQueryFromStatic("ATM", "up_p_setGuaranteedPay", userName, employeeId, GPay);
        return GPay.ToString("0.##");
    }

    [WebMethod]
    public static void PM_SetPreviewNetPay(string userName, int sygmaCenterNo, decimal previewNetPay)
    {
        RunNonQueryFromStatic("ATM", "up_p_SetApsPreviewNetPay", userName, sygmaCenterNo, previewNetPay);
    }

    [WebMethod]
    public static void PM_SetHourlyRate(string userName, int sygmaCenterNo, int classificationId, decimal rate)
    {
        RunNonQueryFromStatic("ATM", "up_p_SetHourlyRate", userName, sygmaCenterNo, classificationId, rate);
    }

    [WebMethod]
    public static void PM_SaveApsExceptionEnabled(string userName, int employeeId, bool ApsExceptionEnabled)
    {
        RunNonQueryFromStatic("ATM", "up_p_setEmployeeApsException", userName, employeeId, ApsExceptionEnabled);
    }

    [WebMethod]
    public static void PM_SaveGpExceptionEnabled(string userName, int employeeId, bool GpExceptionEnabled)
    {
        RunNonQueryFromStatic("ATM", "up_p_setEmployeeGpException", userName, employeeId, GpExceptionEnabled);
    }

    [WebMethod]
    public static void PM_ChangeProgressionBand(string userName, int SygmaCenterNo, int action, decimal PreviewNetPay)
    {
        RunNonQueryFromStatic("ATM", "up_p_ChangeAutoProgresionScheduleBand", userName, SygmaCenterNo, action, PreviewNetPay);
    }

    [WebMethod]
    public static void PM_SaveUseApsEnabled(string userName, int SygmaCenterNo, bool UseApsEnabled)
    {
        RunNonQueryFromStatic("ATM", "up_p_setCenterUseAps", userName, SygmaCenterNo, UseApsEnabled);
    }

    [WebMethod]
    public static void PM_UpdateProgressionRateNow(string userName, int SygmaCenterNo)
    {
        RunNonQueryFromStatic("ATM", "up_p_updateEmployeeProgression", userName, SygmaCenterNo);
    }

    [WebMethod(EnableSession = true)]
    public static void PM_SaveSessionValue(int seqNo, int fieldNo)
    {
        HttpContext.Current.Session.Remove("CurrentSeqNo");
        HttpContext.Current.Session.Remove("CurrentFieldNo");
        HttpContext.Current.Session["CurrentSeqNo"] = seqNo;
        HttpContext.Current.Session["CurrentFieldNo"] = fieldNo;
    }

    protected void gvPayRates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                DataRowView row = (DataRowView)e.Row.DataItem;
                bool isEnabled = Convert.ToBoolean(row["IsEnabled"]);
                string rate = row["rate"].ToString();
                string rateTypeId = row["RateTypeId"].ToString();

                CheckBox chkEnabled = (CheckBox)e.Row.FindControl("chkEnabled");
                chkEnabled.Checked = isEnabled;
                chkEnabled.Attributes.Add("onclick", string.Format("EnabledChanged(this, {0});", rateTypeId));

                CheckBox chkIncludeInBase = (CheckBox)e.Row.FindControl("chkIncludeInBase");
                chkIncludeInBase.Checked = Convert.ToBoolean(row["IncludeInBase"]);
                chkIncludeInBase.Attributes.Add("onclick", string.Format("IncludeInBaseChanged(this, {0});", rateTypeId));

                CheckBox chkApplyProgression = (CheckBox)e.Row.FindControl("chkApplyProgression");
                chkApplyProgression.Checked = Convert.ToBoolean(row["ProgressionRateApplies"]);
                chkApplyProgression.Attributes.Add("onclick", string.Format("ApplyProgChanged(this, {0});", rateTypeId));

                CheckBox chkIncludeInGuaranteedPay = (CheckBox)e.Row.FindControl("chkIncludeInGuaranteedPay");
                chkIncludeInGuaranteedPay.Checked = Convert.ToBoolean(row["IncludeInGuaranteedPay"]);
                chkIncludeInGuaranteedPay.Attributes.Add("onclick", string.Format("IncludeInGuaranteedPayChanged(this, {0});", rateTypeId));

                if (rate != "")
                {
                    TextBox txtRate = (TextBox)e.Row.FindControl("txtRate");
                    txtRate.Text = Convert.ToDecimal(rate).ToString("0.00##");
                    if (!isEnabled)
                    {
                        txtRate.Enabled = false;
                        chkIncludeInBase.Enabled = false;
                        chkApplyProgression.Enabled = false;
                        chkIncludeInGuaranteedPay.Enabled = false;
                    }
                }
                else
                {
                    chkEnabled.Enabled = false;
                    chkIncludeInBase.Enabled = false;
                    chkApplyProgression.Enabled = false;
                    chkIncludeInGuaranteedPay.Enabled = false;
                }

                break;
        }
    }

    //protected void gvEmployees_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    switch (e.Row.RowType)
    //    {
    //        case DataControlRowType.DataRow:
    //            DataRowView row = (DataRowView)e.Row.DataItem;
    //            TextBox txtProgRate = (TextBox)e.Row.FindControl("txtProgRate");
    //            TextBox txtGuaranteedPay = (TextBox)e.Row.FindControl("txtGuaranteedPay");
    //            Label lblGuaranteedPay = (Label)e.Row.FindControl("lblGuaranteedPay");
    //            Label lblProgRate = (Label)e.Row.FindControl("lblProgRate");
    //            CheckBox chkAPSException = (CheckBox)e.Row.FindControl("chkAPSExcptn");
    //            CheckBox chkGPExcptn = (CheckBox)e.Row.FindControl("chkGPExcptn");
    //            DropDownList ddClassification = (DropDownList)e.Row.FindControl("ddClassification");
    //            //TextBox txtVacationRate = (TextBox)e.Row.FindControl("txtVacationRate");
    //            if (!Convert.IsDBNull(row["ProgressionRate"]))
    //            {
    //                txtProgRate.Text = (Convert.ToDecimal(row["ProgressionRate"]) * 100).ToString("0.####");
    //                lblProgRate.Text = (Convert.ToDecimal(row["ProgressionRate"]) * 100).ToString("0.####");
    //            }
    //            if (!Convert.IsDBNull(row["GuaranteedPay"]))
    //            {
    //                txtGuaranteedPay.Text = Convert.ToDecimal(row["GuaranteedPay"]).ToString("0.##");
    //                txtGuaranteedPay.Attributes.Add("OrigVal", Convert.ToDecimal(row["GuaranteedPay"]).ToString("0.##"));
    //                lblGuaranteedPay.Text = Convert.ToDecimal(row["GuaranteedPay"]).ToString("0.##");
    //            }
    //            else
    //            {
    //                txtGuaranteedPay.Attributes.Add("OrigVal", "");
    //            }

    //            if (!Convert.IsDBNull(row["GPException"]))
    //            {
    //                chkGPExcptn.Checked = Convert.ToBoolean(row["GPException"]);
    //                if (Convert.ToBoolean(row["GPException"]))
    //                {
    //                    txtGuaranteedPay.Style["display"] = "block";
    //                    lblGuaranteedPay.Style["display"] = "none";
    //                }
    //                else
    //                {
    //                    txtGuaranteedPay.Style["display"] = "none";
    //                    lblGuaranteedPay.Style["display"] = "block";
    //                }
    //            }

    //            if (!Convert.IsDBNull(row["ApsException"]))
    //            {
    //                chkAPSException.Checked = Convert.ToBoolean(row["ApsException"]);
    //                if (Convert.ToBoolean(row["ApsException"]))
    //                {
    //                    txtProgRate.Style["display"] = "block";
    //                    lblProgRate.Style["display"] = "none";
    //                }
    //                else
    //                {
    //                    txtProgRate.Style["display"] = "none";
    //                    lblProgRate.Style["display"] = "block";
    //                }
    //            }
    //            if (!Convert.IsDBNull(row["ClassificationId"]))
    //            {
    //                ddClassification.DataSource = dtClassifications;
    //                ddClassification.DataBind();
    //                ddClassification.Items.FindByValue(row["ClassificationId"].ToString()).Selected = true;
    //                //for (int i = 0; i < dtClassifications.Rows.Count; i++)
    //                //{
    //                //    if (ddClassification.Items[i].Value == row["ClassificationId"].ToString())
    //                //    {
    //                //        ddClassification.Items[i].Selected = true;
    //                //        break;
    //                //    }
    //                //}
    //            }
    //            /*if (!Convert.IsDBNull(row["VacationRate"]))
    //            {
    //                txtVacationRate.Text = (Convert.ToDouble(row["VacationRate"])).ToString("0.00##");
    //            }*/
    //            txtProgRate.Attributes.Add("onchange", string.Format("UpdateProgressionRate(this,{0});", row["EmployeeId"]));
    //            txtGuaranteedPay.Attributes.Add("onchange", string.Format("UpdateGuaranteedPay(this,{0});", row["EmployeeId"]));

    //            chkAPSException.Attributes.Add("onclick", string.Format("ApsExceptionChanged(this,{0});", row["EmployeeId"]));
    //            chkGPExcptn.Attributes.Add("onclick", string.Format("GpExceptionChanged(this,{0});", row["EmployeeId"]));
    //            ddClassification.Attributes.Add("onchange", string.Format("UpdateEmployeeClassification(this,{0});", row["EmployeeId"]));
    //            //txtVacationRate.Attributes.Add("onchange", string.Format("UpdateVacationRate(this,{0});", row["EmployeeId"]));
    //            if (!gIsApsEnabledSygmaCenterNo)
    //            {
    //                txtProgRate.Style["display"] = "block";
    //                lblProgRate.Style["display"] = "none";
    //                chkAPSException.Enabled = false;
    //            }
    //            break;
    //    }
    //}
    //protected void gvEmployees_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    if (e.SortExpression == ProgSortExpression)
    //    {
    //        ProgSortDir = (ProgSortDir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
    //    }
    //    else
    //    {
    //        ProgSortExpression = e.SortExpression;
    //        ProgSortDir = SortDirection.Ascending;
    //    }
    //    SetEmployees();
    //}
    //protected void txtProgName_TextChanged(object sender, EventArgs e)
    //{
    //    SetEmployees();
    //}
    //public string ProgSortExpression
    //{
    //    get
    //    {
    //        if (ViewState["ProgSortExpression"] == null)
    //            ViewState.Add("ProgSortExpression", "");
    //        return (string)ViewState["ProgSortExpression"];
    //    }
    //    set { ViewState["ProgSortExpression"] = value; }
    //}
    //public SortDirection ProgSortDir
    //{
    //    get
    //    {
    //        if (ViewState["ProgSortDir"] == null)
    //            ViewState.Add("ProgSortDir", SortDirection.Ascending);
    //        return (SortDirection)ViewState["ProgSortDir"];
    //    }
    //    set { ViewState["ProgSortDir"] = value; }
    //}


}