using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Globalization;
using atm;

public partial class Apps_ATM_Tools_DepreciationInfo : ATMPage
{
    private string gVId;
    private bool gIsVehicle = false;
    protected override void LoadATMPage()
    {
        SetPageVariables();
    }

    private void SetPageVariables()
    {
        AddClientVariable("gUserName", UserName);
        if (!string.IsNullOrEmpty(Request.QueryString.Get("vid")) && !string.IsNullOrEmpty(Request.QueryString.Get("isv")))
        {
            gVId = Request.QueryString.Get("vid").ToString();
            gIsVehicle = Convert.ToBoolean(Request.QueryString.Get("isv"));
            lblVTName.Text = gIsVehicle ? "Vehicle Name:" : "Trailer Name:";
            SetVehicleTrailerDeprDetails(gVId, gIsVehicle);
            AddAttributesToFields();
        }
        else
        {
            pnlDeprDetails.Visible = false;
            lblNoRecordExist.Text = "No Record exist!";
            lblNoRecordExist.Visible = true;
        }
    }

    [WebMethod]
    public static void PM_SaveVehicleTrailerDeprDetails(string userName, string vid, int fieldId, string value, int isVehicle)
    {
        if ((fieldId == 1 || fieldId == 2 || fieldId == 3) && value != "")
        {
            DateTime date;
            if (DateTime.TryParseExact(value, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerDeprDetails", userName, vid, fieldId, date.ToShortDateString(), isVehicle);
            }
        }
        else if ((fieldId == 4) && value != "")
        {
            uint months;
            if (UInt32.TryParse(value, out months))
            {
                RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerDeprDetails", userName, vid, fieldId, months, isVehicle);
            }
        }
        else
        {
            RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerDeprDetails", userName, vid, fieldId, value, isVehicle);
        }
    }

    [WebMethod]
    public static void PM_SaveVehicleTrailerDeprCostDetails(string userName, string vid, int fieldId, string value, int isVehicle)
    {
        if (value != "")
        {
            double val;
            if (Double.TryParse(value, out val) && val > 0)
            {
                RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerDeprCostDetails", userName, vid, fieldId, val, isVehicle);
            }
        }
        else
        {
            RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerDeprCostDetails", userName, vid, fieldId, 0, isVehicle);
        }
    }

    private void SetVehicleTrailerDeprDetails(string gVId, bool gIsVehicle)
    {
        DataView dvVehicleTrailerDeprDetails = ATMDB.GetDataView("up_p_getVehicleTrailerDeprDetails", gVId, gIsVehicle);
        if (dvVehicleTrailerDeprDetails.Count > 0)
        {
            lblVehicleTrailerName.Text = dvVehicleTrailerDeprDetails[0]["VehicleName"].ToString();
            if (!Convert.IsDBNull(dvVehicleTrailerDeprDetails[0]["InServiceDate"]))
            {
                txtInServiceDate.Text = Convert.ToDateTime(dvVehicleTrailerDeprDetails[0]["InServiceDate"]).ToShortDateString();
            }
            if (!Convert.IsDBNull(dvVehicleTrailerDeprDetails[0]["DeprStartDate"]))
            {
                txtDeprStartDate.Text = Convert.ToDateTime(dvVehicleTrailerDeprDetails[0]["DeprStartDate"]).ToShortDateString();
            }
            if (!Convert.IsDBNull(dvVehicleTrailerDeprDetails[0]["DeprEndDate"]))
            {
                txtDeprEndDate.Text = Convert.ToDateTime(dvVehicleTrailerDeprDetails[0]["DeprEndDate"]).ToShortDateString();
            }
            txtDeprMonths.Text = dvVehicleTrailerDeprDetails[0]["DeprMonths"].ToString();
            txtCost.Text = dvVehicleTrailerDeprDetails[0]["Cost"].ToString();
            txtMonthlyDepreciation.Text = dvVehicleTrailerDeprDetails[0]["MonthlyDepreciation"].ToString();
            double currentBookValue = 0;
            if (!Convert.IsDBNull(dvVehicleTrailerDeprDetails[0]["InServiceDate"]) && dvVehicleTrailerDeprDetails[0]["MonthlyDepreciation"].ToString() != "")
            {
                DateTime InServiceDate = Convert.ToDateTime(dvVehicleTrailerDeprDetails[0]["InServiceDate"]);
                DateTime currentDate = DateTime.Now;
                double monthsFromInSerDate = (currentDate.Year - InServiceDate.Year) * 12 + currentDate.Month - InServiceDate.Month + (currentDate.Day >= InServiceDate.Day ? 0 : -1);
                double monthlyDepreciation = Convert.ToDouble(dvVehicleTrailerDeprDetails[0]["MonthlyDepreciation"]);
                currentBookValue = monthlyDepreciation * monthsFromInSerDate;
                lblCurrentBookValueValue.Text = currentBookValue.ToString("N2");
            }
            else
            {
                lblCurrentBookValueValue.Text = currentBookValue.ToString("N2");
            }
        }
        else
        {
            pnlDeprDetails.Visible = false;
            lblNoRecordExist.Text = "No Record exist!";
            lblNoRecordExist.Visible = true;
        }
    }

    private void AddAttributesToFields()
    {
        txtInServiceDate.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDeprDetails(this, 1, {0}, {1});", gVId, Convert.ToInt32(gIsVehicle)));
        txtDeprStartDate.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDeprDetails(this, 2, {0}, {1});", gVId, Convert.ToInt32(gIsVehicle)));
        txtDeprEndDate.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDeprDetails(this, 3, {0}, {1});", gVId, Convert.ToInt32(gIsVehicle)));
        txtDeprMonths.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDeprDetails(this, 4, {0}, {1});", gVId, Convert.ToInt32(gIsVehicle)));
        txtCost.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDeprCostDetails(this, 1, {0}, {1});", gVId, Convert.ToInt32(gIsVehicle)));
        txtMonthlyDepreciation.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerDeprCostDetails(this, 2, {0}, {1});", gVId, Convert.ToInt32(gIsVehicle)));
    }
}