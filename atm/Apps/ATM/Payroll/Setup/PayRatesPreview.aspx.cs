using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;

public partial class Apps_ATM_Payroll_Setup_PayRatesPreview : ATMPage
{
    int gEmployeeId;
    protected override void LoadATMPage()
    {
        SetPageVariables();
        if (!IsPostBack)
        {
            SetEmployeeDetail();
            lblToday.Text = DateTime.Now.ToShortDateString();
        }
    }
    private void SetEmployeeDetail()
    {
        DataSet ds = ATMDB.GetDataSet("up_user_getPayRates", gEmployeeId);
        if (ds.Tables[3].Rows.Count > 0)
        {
            ds.Tables[0].TableName = "PayScales";
            ds.Tables[2].TableName = "PayRates";

            if (ds.Tables["PayRates"].Rows.Count > 0)
            {
                rptPayScales.DataSource = ds;
                rptRateTypeHeader.DataSource = new DataView(ds.Tables[1], "", "RateTypeDisplayOrder asc", DataViewRowState.CurrentRows);
                pnlEmployeeDetail.DataBind();
                pnlEmployeeDetail.Visible = true;
            }
            else
            {
                lblNoPayScales.Visible = true;
            }
            DataRow emplRow = ds.Tables[3].Rows[0];
            decimal progRate = Convert.ToDecimal(emplRow["ProgressionRate"]);
            lblEmployeeName.Text = emplRow["WebDisplay"].ToString();
            if (progRate != 1)
            {
                lblProgRateApplied.Text = string.Format("<span style='font-weight: normal;'>Progression rate of <b>{0:0.####%}</b> applied to values in bold</span>", progRate);
            }

        }
        else
        {
            pnlNoEmployee.Visible = true;
        }
    }
    private void SetPageVariables()
    {
        if (Request.QueryString.Get("eid") != null)
        {
            gEmployeeId = Convert.ToInt32(Request.QueryString.Get("eid"));
        }
        else
        {
            throw new Exception("gEmployeeId (qs: eid) is required in the querystring");
        }
    }

    protected void rptPayScales_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRowView dataItem = (DataRowView)e.Item.DataItem;
                Repeater rptRates = (Repeater)e.Item.FindControl("rptRates");
                int payScaleId = Convert.ToInt32(dataItem["PayScaleId"]);
                rptRates.DataSource = dataItem.DataView.Table.DataSet.Tables["PayRates"].Select(string.Format("PayScaleId = {0}", payScaleId), "RateTypeDisplayOrder asc");
                rptRates.DataBind();
                break;
        }
    }
    protected void rptRates_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.AlternatingItem:
            case ListItemType.Item:
                DataRow dataItem = (DataRow)e.Item.DataItem;
                Label rptRates = (Label)e.Item.FindControl("lblRate");
                if (Convert.ToBoolean(dataItem["ActiveRateType"]))
                {
                    if (!Convert.IsDBNull(dataItem["UserRate"]))
                    {
                        decimal userRate = Convert.ToDecimal(dataItem["UserRate"]);
                        decimal centerRate = Convert.ToDecimal(dataItem["CenterRate"]);
                        rptRates.Text = userRate.ToString("C4");
                        if (userRate != centerRate)
                        {
                            //rptRates.Text = string.Format("{0:C2}/{1:C2}", userRate, centerRate);
                            rptRates.Font.Bold = true;
                            rptRates.ToolTip = string.Format("Base Rate: {0:C4}", centerRate);
                        }
                    }
                    else
                    {
                        rptRates.Text = "--";
                    }
                }
                else
                {
                    rptRates.Text = "N/A";
                }

                break;
        }
    }
}