using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;

public partial class Apps_ATM_Reports_EmployeePointsHistoryReport : ATMReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void LoadATMPage()
    {
        SetPageVariables();
        if (!IsPostBack)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "focus",
                    "document.getElementById('" + this.txtEmplID.ClientID + "').focus();", true);
        }
    }
    private void SetPageVariables()
    {
        AddClientVariable("gUserName", UserName);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            var reportHasRecords = BuildExcelReport("15", txtEmplID.Text);
            if (!reportHasRecords)
            {
                Javascript.Notify("No Records available for the selected input!!!");
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception exp)
        {
            throw new Exception("Error generating Employee's points history report", exp);
        }

    }

    protected void txtEmplID_TextChanged(object sender, EventArgs e)
    {
        if (txtEmplID.Text != "" && txtEmplID.Text.Length == 8)
        {
            DataView dvEmplName = ATMDB.GetDataView("up_checkEmplId", txtEmplID.Text);
            if (dvEmplName.Count < 1)
            {
                txtEmplID.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "focus",
                    "document.getElementById('" + this.txtEmplID.ClientID + "').focus();", true);                
            }
        }
        else
        {
            txtEmplID.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "focus",
                    "document.getElementById('" + this.txtEmplID.ClientID + "').focus();", true);
        }
    }
}