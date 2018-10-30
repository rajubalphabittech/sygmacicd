using System;
using System.Threading;
using System.Web.UI.WebControls;
using atm;
using SygmaIntranet;

public partial class Apps_ATM_Reports_PlannedVsActualReport : ATMReportPage
{
    protected override void LoadATMPage()
    {
        var ds = ATMDB.GetDataSet("up_p_getFormCriteria", UserName);
        var count = ds.Tables[2].Rows.Count;
        if (IsPostBack) return;

        lbSygmaCenterNo.DataSource = ds.Tables[2].DefaultView;
        lbSygmaCenterNo.DataBind();
        if (count > 1)
        {
            lbSygmaCenterNo.Items.Insert(0, new ListItem("All", "0"));
            lbSygmaCenterNo.ClearSelection();
            lbSygmaCenterNo.Height = 15 * (count + 2);
            lbSygmaCenterNo.SelectedIndex = 0;
        }
        else if (count == 1)
        {
            lbSygmaCenterNo.SelectedIndex = 0;
            lbSygmaCenterNo.Height = 45;
        }
        else
        {
            lbSygmaCenterNo.Width = 100;
        }

        if (!Page.IsPostBack)
        {
            var today = DateTime.Today;
            var lastMonday = today.AddDays(-(int)today.DayOfWeek - 7);

            txtStartDate.Text = lastMonday.ToShortDateString();
            txtEndDate.Text = lastMonday.AddDays(6).ToShortDateString();

            var x = DateRangeCalculator.GetValidDateRanges();
            foreach (var item in x)
            {
                ddlDateRange.Items.Add(new ListItem(item.Key, item.Value.ToString()));
            }
            ddlDateRange.SelectedIndex = 0;
            CalculateDateRanges();
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < lbSygmaCenterNo.Items.Count; i++)
            {
                string centerValue;
                if (lbSygmaCenterNo.Items[0].Value == "0" && lbSygmaCenterNo.Items[0].Selected && i < (lbSygmaCenterNo.Items.Count - 1))
                {
                    centerValue = lbSygmaCenterNo.Items[i + 1].Value;
                    GetSelectedCenters(centerValue);
                }
                else if (lbSygmaCenterNo.Items[i].Selected)
                {

                    centerValue = lbSygmaCenterNo.Items[i].Value;
                    GetSelectedCenters(centerValue);

                }
            }

            var reportHasRecords = BuildExcelReport("4", SygmaCenterNo, Convert.ToDateTime(txtStartDate.Text),
                Convert.ToDateTime(txtEndDate.Text)); // 4 – Report Id, Startdate, Enddate
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

    protected void ddlDateRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateDateRanges();
    }

    private void CalculateDateRanges()
    {
        var range = DateRangeCalculator.Calculate(DateTime.Today, ddlDateRange.SelectedValue);

        if (range != null)
        {
            txtStartDate.Text = range.Begin.ToShortDateString();
            txtEndDate.Text = range.End.ToShortDateString();
            return;
        }

        txtStartDate.Text = "";
        txtEndDate.Text = "";
    }
}