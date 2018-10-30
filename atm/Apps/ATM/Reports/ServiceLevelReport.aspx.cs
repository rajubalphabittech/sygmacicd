using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;

public partial class Apps_ATM_Reports_ServiceLevelReport : ATMReportPage
{
    SygmaFramework.Log log = new SygmaFramework.Log("C:/Logs/ATMLog.log");
    protected override void LoadATMPage()
    {
        if (IsPostBack) return;
        
        DataView dv = ATMDB.GetDataView("up_getCenters", UserName);
        ddSygmaCenterNo.DataSource = dv;
        ddSygmaCenterNo.DataBind();
        ddSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
        ddSygmaCenterNo.Items.Insert(1, new ListItem("All", "0"));
        
        weeklystd();
        Quaterlystd();  
              
        txtWkyStartDate.Text = (DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.DayOfWeek) - 7)).ToShortDateString();
        txtWkyEndDate.Text = (DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.DayOfWeek) - 1)).ToShortDateString();

        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        string lastQuarter = GetLastQuarterDates(DateTime.Now.Month);
        SetLastQuarterDates(out startDate, out endDate, lastQuarter);
        txtQtystartDate.Text = Convert.ToDateTime(startDate).ToString("M/dd/yyyy");
        txtQtyEndDate.Text = Convert.ToDateTime(endDate).ToString("M/dd/yyyy");
       
       
    }


    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        
            try
            {
                
                if (tcWeeklyQuaterly.ActiveTab.Equals(tabWeekly) == true)
                {
                    List<string> selectedReasonCodesList = new List<string>();
                    foreach (ListItem item in lbWkyReasonCodes.Items)
                    {
                        if (item.Selected)
                        {
                            selectedReasonCodesList.Add(item.Value);
                        }
                    }
                    string strSelectedReasonCodes = string.Join(",", selectedReasonCodesList.ToArray());
                    var reportHasRecords = BuildExcelReport("13", Convert.ToDateTime(txtWkyStartDate.Text),
                    Convert.ToDateTime(txtWkyEndDate.Text), ddSygmaCenterNo.SelectedValue, strSelectedReasonCodes);
                    if (!reportHasRecords)
                    {
                        Javascript.Notify("No Records available for the selected input!!!");
                    }

                }
                else if (tcWeeklyQuaterly.ActiveTab.Equals(tabQuaterly) == true)
                {
                    List<string> selectedQReasonCodesList = new List<string>();
                    foreach (ListItem item in lbQtyReasonCodes.Items)
                    {
                        if (item.Selected)
                        {
                            selectedQReasonCodesList.Add(item.Value);
                        }
                    }
                    string strQSelectedReasonCodes = string.Join(",", selectedQReasonCodesList.ToArray());
                    var reportHasRecords = BuildExcelReport("13", Convert.ToDateTime(txtQtystartDate.Text),
                    Convert.ToDateTime(txtQtyEndDate.Text), ddSygmaCenterNo.SelectedValue, strQSelectedReasonCodes);
                    if (!reportHasRecords)
                    {
                        Javascript.Notify("No Records available for the selected input!!!");
                    }
                }

                                
            }
            catch (ThreadAbortException) { }
            catch (Exception exp)
            {
                throw new Exception("Error generating report", exp);
            }

        }
    

    //protected void lbReasonCodes_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    lblSelectedReasonCodes.Text = String.Empty;
    //    List<string> selectedReasonCodesList = new List<string>();
    //    foreach (ListItem item in lbReasonCodes.Items)
    //    {
    //        if (item.Selected)
    //        {
    //            selectedReasonCodesList.Add(item.Value);
    //        }
    //    }
    //    string strSelectedValues = string.Join(", ", selectedReasonCodesList.ToArray());
    //    lblSelectedReasonCodes.Text = strSelectedValues;
    //}

    protected void Wky_ReasonCode_CheckedChanged(Object sender, EventArgs e)
    {
        try
        {
            if (rbWkyStdReasonCodes.Checked)
            {
                weeklystd();
            }
            if (rbWkyCustReasonCodes.Checked)
            {
                lblWkyCustMessage.Visible = true;
                lbWkyReasonCodes.Enabled = true;
                DataView dvReasonCode = ATMDB.GetDataView("up_getReasonCode");
                lbWkyReasonCodes.DataSource = dvReasonCode.Table.DefaultView;
                lbWkyReasonCodes.DataBind();
                lbWkyReasonCodes.Height = 150;
                foreach (ListItem item in lbWkyReasonCodes.Items)
                {
                    if (item.Value == "18" || item.Value == "40" || item.Value == "41" || item.Value == "60" || item.Value == "61")
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        catch (Exception)
        {
            log.WriteEntryFormat("Error while loading weekly reason codes");
        }
    }
    protected void Qty_ReasonCode_CheckedChanged(Object sender, EventArgs e)
    {
        try
        {
            if (rbQtyStdReasoncodes.Checked)
            {
                Quaterlystd();
            }
            if (rbQtyCustReasoncodes.Checked)
            {
                lblQtyCustMessage.Visible = true;
                lbQtyReasonCodes.Enabled = true;
                DataView dvReasonCode = ATMDB.GetDataView("up_getReasonCode");
                lbQtyReasonCodes.DataSource = dvReasonCode.Table.DefaultView;
                lbQtyReasonCodes.DataBind();
                lbQtyReasonCodes.Height = 150;
                foreach (ListItem item in lbQtyReasonCodes.Items)
                {
                    if (item.Value == "18" || item.Value == "40" || item.Value == "41" || item.Value == "60" || item.Value == "61")
                    {
                        item.Selected = true;
                    }
                }
            }

        }
        catch (Exception)
        {
            log.WriteEntryFormat("Error while loading Quaterly reason codes");
        }

    }
    protected void weeklystd()
    {
        try
        {
            lblWkyCustMessage.Visible = false;
            lbWkyReasonCodes.Enabled = false;
            lbWkyReasonCodes.Height = 73;
            DataView dvReasonCode = ATMDB.GetDataView("up_getSelectedReasonCode");
            lbWkyReasonCodes.DataSource = dvReasonCode.Table.DefaultView;
            lbWkyReasonCodes.DataBind();


            foreach (ListItem item in lbWkyReasonCodes.Items)
            {
                if (item.Value == "18" || item.Value == "40" || item.Value == "41" || item.Value == "60" || item.Value == "61")
                {
                    item.Selected = true;
                }
            }
        }
        catch (Exception)
        {
            log.WriteEntryFormat("Error while loading weekly reason codes");
        }
    }

    protected void Quaterlystd()
    {
        try
        {
            lblQtyCustMessage.Visible = false;
            lbQtyReasonCodes.Enabled = false;
            lbQtyReasonCodes.Height = 73;
            DataView dvReasonCode = ATMDB.GetDataView("up_getSelectedReasonCode");
            lbQtyReasonCodes.DataSource = dvReasonCode.Table.DefaultView;
            lbQtyReasonCodes.DataBind();

            foreach (ListItem item in lbQtyReasonCodes.Items)
            {
                if (item.Value == "18" || item.Value == "40" || item.Value == "41" || item.Value == "60" || item.Value == "61")
                {
                    item.Selected = true;
                }
            }
        }
        catch (Exception)
        {
            log.WriteEntryFormat("Error while loading Quaterly reason codes");
        }
    }

    private void SetLastQuarterDates(out DateTime startDate, out DateTime endDate, string quarter)
    {
        try
        {
            int startMonth = 0;
            int startYear = DateTime.Now.Year;
            int startDay = 1;
            int endMonth = 0;
            int endYear = DateTime.Now.Year;
            int endDay = 31;

            switch (quarter)
            {
                case "Q1":
                    startMonth = 1;
                    endMonth = 3;
                    endDay = 31;
                    break;

                case "Q2":
                    startMonth = 4;
                    endMonth = 6;
                    endDay = 30;
                    break;

                case "Q3":
                    startMonth = 7;
                    endMonth = 9;
                    endDay = 30;
                    break;

                case "Q4":
                    startMonth = 10;
                    startYear = DateTime.Now.Year - 1;
                    endMonth = 12;
                    endYear = DateTime.Now.Year - 1;
                    break;
            }

            startDate = new DateTime(startYear, startMonth, startDay);
            endDate = new DateTime(endYear, endMonth, endDay);
        }
        catch (Exception exp)
        {
            throw new Exception("Error while loading quater date", exp);
        }
    }


    private string GetLastQuarterDates(int month)
    {
        try
        {
            string quarter = string.Empty;

            if (month >= 1 && month <= 3)
            {
                quarter = "Q4";
            }
            if (month >= 4 && month <= 6)
            {
                quarter = "Q1";
            }
            if (month >= 7 && month <= 9)
            {
                quarter = "Q2";
            }
            if (month >= 10 && month <= 12)
            {
                quarter = "Q3";
            }

            return quarter;
        }
        catch (Exception exp)
        {
            throw new Exception("Error while loading quater", exp);
        }
    }


    

        


}