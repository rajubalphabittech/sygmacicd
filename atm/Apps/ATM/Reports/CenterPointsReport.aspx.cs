using System;
using System.Data;
using System.Threading;
using System.Web.UI.WebControls;

public partial class Apps_ATM_Reports_CenterPointsReport : ATMReportPage
{
    protected override void LoadATMPage()
    {
        var ds = ATMDB.GetDataSet("up_p_getFormCriteria", UserName);
        var count = ds.Tables[2].Rows.Count;
        if (IsPostBack) return;

        DataView dv = ATMDB.GetDataView("up_getCenters", UserName);
        ddSygmaCenterNo.DataSource = dv;
        ddSygmaCenterNo.DataBind();
        ddSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", ""));
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            var reportHasRecords = BuildExcelReport("13", Convert.ToDateTime(txtStartDate.Text),
                Convert.ToDateTime(txtEndDate.Text), ddSygmaCenterNo.SelectedValue, cbValid.Checked); 
            if (!reportHasRecords)
            {
                Javascript.Notify("No Records available for the selected input!!!");
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception exp)
        {
            throw new Exception("Error generating report", exp);
        }

    }
}