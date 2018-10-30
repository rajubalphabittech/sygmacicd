using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

public partial class Apps_ATM_Reports_4PointsReport : ATMReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void LoadATMPage()
    {
        if (IsPostBack) return;
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            var reportHasRecords = BuildExcelReport("16", Convert.ToDateTime(txtStartDate.Text),
                Convert.ToDateTime(txtEndDate.Text));
            if (!reportHasRecords)
            {
                Javascript.Notify("No Records available for the selected input!!!");
            }
        }
        catch (ThreadAbortException) { }
        catch (Exception exp)
        {
            throw new Exception("Error generating 4 points report", exp);
        }

    }
}