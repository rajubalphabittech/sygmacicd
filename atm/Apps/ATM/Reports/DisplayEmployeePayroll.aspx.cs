using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;
using SygmaFramework;

public partial class Apps_ATM_Reports_DisplayEmployeePayroll : ATMPage
{
    public DataSet dsEmployeesPayment;
    public double routeTotal, employeeTotal;
    public int SygmaCenterNo, isWeekendingRange;
    public string EmployeeIds, FromDate, ToDate, RangeBy;
    public Boolean hasHolidayPay, isGroupByWeekending, hasTotalHolidayPay;
    protected override void LoadATMPage()
    {
        SetEmployeesPayroll();
    }

    private void SetEmployeesPayroll()
    {
        if (Session["eprEmployeeIds"] != null && Session["eprCenterNo"] != null && Session["eprFromDate"] != null && Session["eprToDate"] != null && Session["eprRangeBy"] != null && Session["eprGroupByWeekend"] != null)
        {
            lblInvalidInput.Visible = false;
            SygmaCenterNo = Convert.ToInt32(Session["eprCenterNo"]);
            EmployeeIds = Session["eprEmployeeIds"].ToString();
            RangeBy = Session["eprRangeBy"].ToString();
            FromDate = Session["eprFromDate"].ToString();
            ToDate = Session["eprToDate"].ToString();
            isGroupByWeekending = Convert.ToBoolean(Session["eprGroupByWeekend"]);
            isWeekendingRange = (RangeBy == "Week Ending") ? 1 : 0;
            //dsEmployeesPayment = ATMDB.GetDataSet("up_p_EmployeesPaymentReport", 30, "538,2798,525,1593", "2014-08-09");
            DataSet dsConfig = ATMDB.GetDataSet("up_p_getReportConfig", 11);
            string storedproc = dsConfig.Tables[0].Rows[0][0].ToString();
            dsEmployeesPayment = ATMDB.GetDataSet(storedproc, SygmaCenterNo, EmployeeIds, FromDate, ToDate, isWeekendingRange);
            if (dsEmployeesPayment.Tables[0].Rows.Count > 0)
            {
                lblNoRecords.Visible = false;
                DataSetHelper dsh = new DataSetHelper(dsEmployeesPayment);
                if (isGroupByWeekending)
                {
                    dsh.AddDistinctTable("Employees", 0, "WebDisplay", "EmployeeId", "WeekendingDate", "HireDate", "EffectiveHireDate", "ProgressionRate", "PayBandName");
                    dsh.AddDistinctTable("Routes", 0, "EmployeeId", "RouteNo", "DepartDate", "WeekendingDate");
                }
                else
                {
                    dsh.AddDistinctTable("Employees", 0, "WebDisplay", "EmployeeId", "HireDate", "EffectiveHireDate", "ProgressionRate", "PayBandName");
                    dsh.AddDistinctTable("Routes", 0, "EmployeeId", "RouteNo", "DepartDate");
                }
                rptEmployees.DataSource = dsEmployeesPayment.Tables["Employees"];
                rptEmployees.DataBind();
            }
        }
        else
        {
            lblInvalidInput.Visible = true;
            lblNoRecords.Visible = false;
        }
    }

    protected void rptEmployeePayments_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Header:
                hasHolidayPay = false;
                break;
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                Label lblRateTypeDesc = (Label)e.Item.FindControl("lblRateTypeDesc");
                Label lblQty = (Label)e.Item.FindControl("lblQty");
                Label lblRate = (Label)e.Item.FindControl("lblRate");
                Label lblTotal = (Label)e.Item.FindControl("lblTotal");
                Label lblPaymentNotes = (Label)e.Item.FindControl("lblPaymentNotes");
                DataRowView row = (DataRowView)e.Item.DataItem;
                string strRate;
                lblRateTypeDesc.Text = row["RateTypeDescription"].ToString();
                lblQty.Text = Convert.ToDouble(row["Quantity"]).ToString("#,##0.##");
                //lblRate.Text = Convert.ToDouble(row["Rate"]).ToString();
                strRate = Convert.ToDouble(row["Rate"]).ToString("0.00##");
                lblRate.Text = strRate.Substring(strRate.IndexOf(".") + 1).Length > 2 ? Convert.ToDouble(row["Rate"]).ToString("0.0000") : Convert.ToDouble(row["Rate"]).ToString("0.00");
                lblTotal.Text = row["Total"].ToString();
                routeTotal = routeTotal + Convert.ToDouble(row["Total"]);
                lblPaymentNotes.Text = "&nbsp;&nbsp;&nbsp;" + row["PaymentNotes"].ToString();
                if (row["PaymentNotes"].ToString() == "(No Notes)")
                {
                    lblPaymentNotes.Visible = false;
                }

                if (Convert.ToInt32(row["IsHolidayPay"]) == 1)
                {
                    hasHolidayPay = true;
                    hasTotalHolidayPay = true;
                }
                break;
        }
    }

    protected void rptEmployees_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Header:
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = (DataRowView)e.Item.DataItem;
                Label lblEmployeeDates = (Label)e.Item.FindControl("lblEmployeeDates");
                Label lblProgressionRate = (Label)e.Item.FindControl("lblProgressionRate");
                Label lblEmployeeGrandTotal = (Label)e.Item.FindControl("lblEmployeeGrandTotal");
                Label lblCompTrueUp = (Label)e.Item.FindControl("lblCompTrueUp");
                Label lblPayStubTotal = (Label)e.Item.FindControl("lblPayStubTotal");
                Label lblTotalHours = (Label)e.Item.FindControl("lblTotalHours");
                Label lblTotalHeader = (Label)e.Item.FindControl("lblTotalHeader");
                Label lblEmptyPayments = (Label)e.Item.FindControl("lblEmptyPayments");
                Label lblEmptyVehicles = (Label)e.Item.FindControl("lblEmptyVehicles");
                Label lblComponentPayTrueUp = (Label)e.Item.FindControl("lblComponentPayTrueUp");
                Label lblNegCompMsg = (Label)e.Item.FindControl("lblNegCompMsg");
                Panel pnlTotalHolidayPay = (Panel)e.Item.FindControl("pnlTotalHolidayPay");
                Panel pnlComponentHourly = (Panel)e.Item.FindControl("pnlComponentHourly");
                Panel pnlPayStubPreview = (Panel)e.Item.FindControl("pnlPayStubPreview");
                Panel pnlEmployeeTimelog = (Panel)e.Item.FindControl("pnlEmployeeTimelog");
                Panel pnlEmptyEmployeesTimeLogs = (Panel)e.Item.FindControl("pnlEmptyEmployeesTimeLogs");
                Panel pnlWarning = (Panel)e.Item.FindControl("pnlWarning");
                Panel pnlTimelogTotal = (Panel)e.Item.FindControl("pnlTimelogTotal");
                Panel pnlEmptyComponentPay = (Panel)e.Item.FindControl("pnlEmptyComponentPay");
                Panel pnlEmployeeGrandTotal = (Panel)e.Item.FindControl("pnlEmployeeGrandTotal");
                Panel pnlNegCompTrueUp = (Panel)e.Item.FindControl("pnlNegCompTrueUp");
                Repeater rptRoutes = (Repeater)e.Item.FindControl("rptRoutes");
                Repeater rptEmployeeVehicles = (Repeater)e.Item.FindControl("rptEmployeeVehicles");
                Repeater rptTotal = (Repeater)e.Item.FindControl("rptTotal");
                Repeater rptComponent = (Repeater)e.Item.FindControl("rptComponent");
                Repeater rptPayStub = (Repeater)e.Item.FindControl("rptPayStub");
                Repeater rptEmployeesTimeLogs = (Repeater)e.Item.FindControl("rptEmployeesTimeLogs");
                string EmployeeFilter, EmployeeRouteFilter;
                hasTotalHolidayPay = false;
                bool isComponentPayTrueUpNeg = false;
                DataTable dtTotal;
                DataTable dtComponent;
                DataTable dtPayStub;
                DataTable dtTimelog;
                bool isHourlyPayAllowed;
                isHourlyPayAllowed = Convert.ToBoolean(row.DataView.Table.DataSet.Tables[4].Rows[0][0]);
                if (isGroupByWeekending)
                {
                    EmployeeFilter = string.Format("EmployeeId={0} and WeekendingDate='{1}'", row["EmployeeId"], row["WeekendingDate"]);
                    EmployeeRouteFilter = string.Format("EmployeeId={0} and WeekendingDate='{1}' and RouteNo <> 'NA'", row["EmployeeId"], row["WeekendingDate"]);
                    lblEmployeeDates.Text = "Weekending " + row["WeekendingDate"].ToString();
                    lblProgressionRate.Text = (Convert.ToDateTime(row["HireDate"]) == Convert.ToDateTime(row["EffectiveHireDate"])) ?
                                                    "Hire Date: " + Convert.ToDateTime(row["HireDate"]).ToShortDateString() + " | Progression Rate: " + row["ProgressionRate"].ToString() + "%" + " | Pay Band: " + row["PayBandName"].ToString()
                                                    : "Hire Date: " + Convert.ToDateTime(row["HireDate"]).ToShortDateString() + " | Effective Hire Date: " + Convert.ToDateTime(row["EffectiveHireDate"]).ToShortDateString()
                                                    + " | Progression Rate: " + row["ProgressionRate"].ToString() + "%"
                                                    + " | Pay Band: " + row["PayBandName"].ToString();
                    dtTotal = row.DataView.Table.DataSet.Tables[2].Select(EmployeeFilter, "RateTypeDescription").Count() > 0 ? row.DataView.Table.DataSet.Tables[2].Select(EmployeeFilter, "RateTypeDescription").CopyToDataTable() : new DataTable();

                    if (isWeekendingRange == 1 && isHourlyPayAllowed)
                    {
                        dtTimelog = row.DataView.Table.DataSet.Tables[5].Select(EmployeeFilter, "StartDateTime").Count() > 0 ? row.DataView.Table.DataSet.Tables[5].Select(EmployeeFilter, "StartDateTime").CopyToDataTable() : new DataTable();
                        dtComponent = row.DataView.Table.DataSet.Tables[6].Select(EmployeeFilter, "RateTypeDescription").Count() > 0 ? row.DataView.Table.DataSet.Tables[6].Select(EmployeeFilter, "RateTypeDescription").CopyToDataTable() : new DataTable();
                        dtPayStub = row.DataView.Table.DataSet.Tables[7].Select(EmployeeFilter, "RateTypeDescription").Count() > 0 ? row.DataView.Table.DataSet.Tables[7].Select(EmployeeFilter, "RateTypeDescription").CopyToDataTable() : new DataTable();
                    }
                    else
                    {
                        dtTimelog = new DataTable();
                        dtComponent = new DataTable();
                        dtPayStub = new DataTable();
                    }
                }
                else
                {
                    EmployeeFilter = string.Format("EmployeeId={0}", row["EmployeeId"]);
                    EmployeeRouteFilter = string.Format("EmployeeId={0} and RouteNo <> 'NA'", row["EmployeeId"]);
                    if (Session["eprRangeBy"].ToString() == "Week Ending")
                    {
                        lblEmployeeDates.Text = "Weekending " + Session["eprFromDate"].ToString() + " - " + Session["eprToDate"].ToString();
                        lblProgressionRate.Text = (Convert.ToDateTime(row["HireDate"]) == Convert.ToDateTime(row["EffectiveHireDate"])) ?
                                                    "Hire Date: " + Convert.ToDateTime(row["HireDate"]).ToShortDateString() + " | Progression Rate: " + row["ProgressionRate"].ToString() + "%" + " | Pay Band: " + row["PayBandName"].ToString() 
                                                    : "Hire Date: " + Convert.ToDateTime(row["HireDate"]).ToShortDateString() + " | Effective Hire Date: " + Convert.ToDateTime(row["EffectiveHireDate"]).ToShortDateString()
                                                    + " | Progression Rate: " + row["ProgressionRate"].ToString() + "%"
                                                    + " | Pay Band: " + row["PayBandName"].ToString();
                    }
                    else
                    {
                        lblEmployeeDates.Text = "Depart dates " + Session["eprFromDate"].ToString() + " - " + Session["eprToDate"].ToString();
                        lblProgressionRate.Text = (Convert.ToDateTime(row["HireDate"]) == Convert.ToDateTime(row["EffectiveHireDate"])) ?
                                                    "Hire Date: " + Convert.ToDateTime(row["HireDate"]).ToShortDateString() + " | Progression Rate: " + row["ProgressionRate"].ToString() + "%" + " | Pay Band: " + row["PayBandName"].ToString()
                                                    : "Hire Date: " + Convert.ToDateTime(row["HireDate"]).ToShortDateString() + " | Effective Hire Date: " + Convert.ToDateTime(row["EffectiveHireDate"]).ToShortDateString()
                                                    + " | Progression Rate: " + row["ProgressionRate"].ToString() + "%"
                                                    + " | Pay Band: " + row["PayBandName"].ToString();
                    }
                    dtTotal = row.DataView.Table.DataSet.Tables[3].Select(EmployeeFilter, "RateTypeDescription").Count() > 0 ? row.DataView.Table.DataSet.Tables[3].Select(EmployeeFilter, "RateTypeDescription").CopyToDataTable() : new DataTable();
                    dtTimelog = new DataTable();
                    dtComponent = new DataTable();
                    dtPayStub = new DataTable();
                }

                DataTable dtRoutes = row.DataView.Table.DataSet.Tables["Routes"].Select(EmployeeRouteFilter, "DepartDate").Count() > 0 ? row.DataView.Table.DataSet.Tables["Routes"].Select(EmployeeRouteFilter, "DepartDate").CopyToDataTable() : new DataTable();
                DataTable dtPayments = row.DataView.Table.DataSet.Tables[0].Select(EmployeeRouteFilter, "RouteNo, RateTypeDescription").Count() > 0 ? row.DataView.Table.DataSet.Tables[0].Select(EmployeeRouteFilter, "RouteNo, RateTypeDescription").CopyToDataTable() : new DataTable();
                DataTable dtVehicles = row.DataView.Table.DataSet.Tables[1].Select(EmployeeFilter, "WebDisplay").Count() > 0 ? row.DataView.Table.DataSet.Tables[1].Select(EmployeeFilter, "WebDisplay").CopyToDataTable() : new DataTable();
                //dtTotal = row.DataView.Table.DataSet.Tables[2].Select(EmployeeFilter, "RateTypeDescription").Count() > 0 ? row.DataView.Table.DataSet.Tables[2].Select(EmployeeFilter, "RateTypeDescription").CopyToDataTable() : new DataTable();
                DataSet dsRoutes = new DataSet();
                dsRoutes.Tables.Add(dtPayments);
                dsRoutes.Tables.Add(dtRoutes);
                dsRoutes.Tables.Add(dtVehicles);
                //DataRow[] drRoutes = row.DataView.Table.DataSet.Tables["Routes"].Select(EmployeeFilter, "RouteNo");
                if (dsRoutes.Tables[1].Rows.Count > 0)
                {
                    lblEmptyPayments.Visible = false;
                    lblEmptyVehicles.Visible = false;
                    rptRoutes.DataSource = dsRoutes.Tables[1];
                    rptRoutes.DataBind();
                    rptEmployeeVehicles.DataSource = dsRoutes.Tables[1];
                    rptEmployeeVehicles.DataBind();
                }
                //if (dtVehicles.Rows.Count > 0)
                //{
                //    Label lblEmptyVehicles = (Label)e.Item.FindControl("lblEmptyVehicles");
                //    lblEmptyVehicles.Visible = false;
                //    Repeater rptVehicles = (Repeater)e.Item.FindControl("rptVehicles");
                //    rptVehicles.DataSource = dtVehicles;
                //    rptVehicles.DataBind();
                //}
                employeeTotal = 0;
                if (dtTotal.Rows.Count > 0)
                {
                    rptTotal.DataSource = dtTotal;
                    rptTotal.DataBind();
                    pnlEmptyComponentPay.Visible = false;
                    pnlEmployeeGrandTotal.Visible = true;
                    lblEmployeeGrandTotal.Text = employeeTotal.ToString("N2");
                }
                if (isGroupByWeekending && isWeekendingRange == 1 && isHourlyPayAllowed)
                {
                    lblTotalHeader.Text = "Component Pay";
                }
                employeeTotal = 0;
                if (isGroupByWeekending && isWeekendingRange == 1 && isHourlyPayAllowed && dtTimelog.Rows.Count > 0)
                {
                    pnlEmployeeTimelog.Visible = true;
                    pnlEmptyEmployeesTimeLogs.Visible = false;
                    rptEmployeesTimeLogs.DataSource = dtTimelog;
                    rptEmployeesTimeLogs.DataBind();
                    pnlTimelogTotal.Visible = true;
                    lblTotalHours.Text = employeeTotal.ToString("N2");
                }
                employeeTotal = 0;
                if (isGroupByWeekending && isWeekendingRange == 1 && isHourlyPayAllowed && dtComponent.Rows.Count > 0)
                {
                    pnlComponentHourly.Visible = true;
                    rptComponent.DataSource = dtComponent;
                    rptComponent.DataBind();
                    if (employeeTotal < 0)
                    {
                        lblCompTrueUp.Text = "0*";
                        lblComponentPayTrueUp.Text = lblComponentPayTrueUp.Text + "*";
                        pnlNegCompTrueUp.Visible = true;
                        string strMessage = "*  Elapsed Hours Pay is greater than Component Pay, so <b>no Component Pay True-up is required</b>. "
                                                + "However, when the Send to ADP report runs, an <b>Elapsed Hours True-up payment</b> of <b>$" + Convert.ToDouble(employeeTotal * -1).ToString("0.00")
                                                + "</b> will be added to Component Pay.";
                        lblNegCompMsg.Text = strMessage;
                        isComponentPayTrueUpNeg = true;
                    }
                    else
                    {
                        lblCompTrueUp.Text = employeeTotal.ToString("N2");
                    }
                    //lblCompTrueUp.Text = employeeTotal < 0 ? "N/A" : employeeTotal.ToString("N2");
                }
                employeeTotal = 0;
                if (isGroupByWeekending && isWeekendingRange == 1 && isHourlyPayAllowed && dtPayStub.Rows.Count > 0)
                {
                    pnlPayStubPreview.Visible = true;
                    rptPayStub.DataSource = dtPayStub;
                    rptPayStub.DataBind();
                    lblPayStubTotal.Text = employeeTotal.ToString("N2");
                }
                if (!hasTotalHolidayPay)
                {
                    pnlTotalHolidayPay.Visible = false;
                }
                if (isComponentPayTrueUpNeg)
                {
                    //pnlWarning.Visible = true;
                    //pnlComponentHourly.Visible = false;
                    pnlPayStubPreview.Visible = false;
                }
                break;
        }
    }

    protected void rptRoutes_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Header:
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                Repeater rptEmployeePayments = (Repeater)e.Item.FindControl("rptEmployeePayments");
                Label lblRoute = (Label)e.Item.FindControl("lblRoute");
                Label lblDepartDate = (Label)e.Item.FindControl("lblDepartDate");
                Label lblEmployeeTotal = (Label)e.Item.FindControl("lblEmployeeTotal");
                Panel pnlHolidayPay = (Panel)e.Item.FindControl("pnlHolidayPay");
                DataRowView row = (DataRowView)e.Item.DataItem;
                string EmployeeFilter = string.Format("EmployeeId={0} And RouteNo = '{1}' And DepartDate = '{2}'", row["EmployeeId"], row["RouteNo"], row["DepartDate"]);
                lblRoute.Text = row["RouteNo"].ToString();
                lblDepartDate.Text = Convert.ToDateTime(row["DepartDate"]).ToString("MM/dd/yyyy");
                if (row.DataView.Table.DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable dtPayments = row.DataView.Table.DataSet.Tables[0].Select(EmployeeFilter, "RateTypeDescription").Count() > 0 ? row.DataView.Table.DataSet.Tables[0].Select(EmployeeFilter, "RateTypeDescription").CopyToDataTable() : new DataTable();
                    if (dtPayments.Rows.Count > 0)
                    {
                        routeTotal = 0;
                        rptEmployeePayments.DataSource = dtPayments;
                        rptEmployeePayments.DataBind();
                        lblEmployeeTotal.Text = routeTotal.ToString("N2");
                        if (!hasHolidayPay)
                        {
                            pnlHolidayPay.Visible = false;
                        }
                    }
                }
                break;
        }
    }


    protected void rptEmployeeVehicles_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                Repeater rptVehicles = (Repeater)e.Item.FindControl("rptVehicles");
                Label lblRoute = (Label)e.Item.FindControl("lblRoute");
                Label lblDepartDate = (Label)e.Item.FindControl("lblDepartDate");
                Label lblEmptyVehicles = (Label)e.Item.FindControl("lblEmptyVehicles");
                DataRowView row = (DataRowView)e.Item.DataItem;
                string EmployeeFilter = string.Format("EmployeeId={0} And RouteNo = '{1}' And DepartDate = '{2}'", row["EmployeeId"], row["RouteNo"], row["DepartDate"]);
                lblRoute.Text = row["RouteNo"].ToString();
                lblDepartDate.Text = Convert.ToDateTime(row["DepartDate"]).ToString("MM/dd/yyyy");
                if (row.DataView.Table.DataSet.Tables[2].Rows.Count > 0)
                {
                    DataTable dtVehicles = row.DataView.Table.DataSet.Tables[2].Select(EmployeeFilter, "WebDisplay").Count() > 0 ? row.DataView.Table.DataSet.Tables[2].Select(EmployeeFilter, "WebDisplay").CopyToDataTable() : new DataTable();
                    if (dtVehicles.Rows.Count > 0)
                    {
                        rptVehicles.DataSource = dtVehicles;
                        rptVehicles.DataBind();
                        lblEmptyVehicles.Visible = false;
                    }
                }
                break;
        }
    }

    protected void rptVehicles_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:

                break;
        }
    }

    protected void rptTotal_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = (DataRowView)e.Item.DataItem;
                Label lblRateTypeDesc = (Label)e.Item.FindControl("lblRateTypeDesc");
                Label lblQty = (Label)e.Item.FindControl("lblQty");
                Label lblTotal = (Label)e.Item.FindControl("lblTotal");
                Label lblPaymentNotes = (Label)e.Item.FindControl("lblPaymentNotes");
                //lblPaymentNotes.Text = "&nbsp;&nbsp;&nbsp;" + row["CategoryDescription"].ToString();
                if (!Convert.IsDBNull(row["CategoryDescription"]))
                {
                    lblPaymentNotes.Text = "&nbsp;&nbsp;&nbsp;" + row["CategoryDescription"].ToString();
                }
                lblRateTypeDesc.Text = row["RateTypeDescription"].ToString();
                lblQty.Text = Convert.ToDouble(row["Quantity"]).ToString("#,##0.##");
                lblTotal.Text = row["Total"].ToString();
                employeeTotal += Convert.ToDouble(row["Total"]);
                break;
        }
    }

    protected void rptEmployeesTimeLogs_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = (DataRowView)e.Item.DataItem;
                Label lblRoute = (Label)e.Item.FindControl("lblRoute");
                Label lblStart = (Label)e.Item.FindControl("lblStart");
                Label lblEnd = (Label)e.Item.FindControl("lblEnd");
                Label lblHours = (Label)e.Item.FindControl("lblHours");
                lblRoute.Text = row["RouteNo"].ToString();
                lblStart.Text = row["StartDateTime"].ToString();
                lblEnd.Text = row["EndDateTime"].ToString();
                lblHours.Text = row["HoursAndMinutes"].ToString();
                employeeTotal += Convert.ToDouble(row["HoursAndMinutes"]);
                break;
        }
    }

    protected void rptComponent_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = (DataRowView)e.Item.DataItem;
                Label lblType = (Label)e.Item.FindControl("lblType");
                Label lblHours = (Label)e.Item.FindControl("lblHours");
                Label lblRate = (Label)e.Item.FindControl("lblRate");
                Label lblTotal = (Label)e.Item.FindControl("lblTotal");
                string strRate;

                if (row["Hours"].ToString() == "-1.00")
                {
                    lblHours.Text = "&nbsp;&nbsp;&nbsp;";
                }
                else
                {
                    lblHours.Text = row["Hours"].ToString();
                }

                if (row["Rate"].ToString() == "-1.0000")
                {
                    lblRate.Text = "&nbsp;&nbsp;&nbsp;";
                }
                else
                {
                    strRate = Convert.ToDouble(row["Rate"]).ToString("0.00##");
                    lblRate.Text = strRate.Substring(strRate.IndexOf(".") + 1).Length > 2 ? Convert.ToDouble(row["Rate"]).ToString("$#,##0.0000") : Convert.ToDouble(row["Rate"]).ToString("$#,##0.00");
                }
                lblType.Text = row["RateTypeDescription"].ToString();
                lblTotal.Text = Convert.ToDouble(row["Total"]).ToString("$#,##0.00");
                employeeTotal += Convert.ToDouble(row["Total"]);
                break;
        }
    }


    protected void rptPayStub_ItemDataBound(object source, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView row = (DataRowView)e.Item.DataItem;
                Label lblType = (Label)e.Item.FindControl("lblType");
                Label lblHours = (Label)e.Item.FindControl("lblHours");
                Label lblRate = (Label)e.Item.FindControl("lblRate");
                Label lblTotal = (Label)e.Item.FindControl("lblTotal");
                string strRate;

                if (row["Hours"].ToString() == "-1.00")
                {
                    lblHours.Text = "&nbsp;&nbsp;&nbsp;";
                }
                else
                {
                    lblHours.Text = row["Hours"].ToString();
                }

                if (row["Rate"].ToString() == "-1.0000")
                {
                    lblRate.Text = "&nbsp;&nbsp;&nbsp;";
                }
                else
                {
                    strRate = Convert.ToDouble(row["Rate"]).ToString("0.00##");
                    lblRate.Text = strRate.Substring(strRate.IndexOf(".") + 1).Length > 2 ? Convert.ToDouble(row["Rate"]).ToString("$#,##0.0000") : Convert.ToDouble(row["Rate"]).ToString("$#,##0.00");
                }
                lblType.Text = row["RateTypeDescription"].ToString();
                lblTotal.Text = Convert.ToDouble(row["Total"]).ToString("$#,##0.00");
                employeeTotal += Convert.ToDouble(row["Total"]);
                break;
        }
    }
}