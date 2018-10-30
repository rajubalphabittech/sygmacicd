using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;

public partial class Apps_ATM_Reports_EmployeePayrollReport : ATMPage
{
    protected override void LoadATMPage()
    {
        if (!IsPostBack)
        {
            SetCenters();
            ddRangeBy.Items.Add("Week Ending");
            ddRangeBy.Items.Add("Depart Date");
            txtStartDate.Text = (DateTime.Now.AddDays(-Convert.ToInt32(DateTime.Now.DayOfWeek))).ToShortDateString();
            txtEndDate.Text = (DateTime.Now.AddDays(6 - Convert.ToInt32(DateTime.Now.DayOfWeek))).ToShortDateString();
        }
    }

    public void SetCenters()
    {
        DataSet dsCenters = ATMDB.GetDataSet("up_getCenters", UserName);
        ddSygmaCenterNo.DataSource = dsCenters;
        ddSygmaCenterNo.DataBind();
        if (dsCenters.Tables[0].Rows.Count > 0)
        {
            DataSet dsEmployees = ATMDB.GetDataSet("up_getEmployees", UserName, ddSygmaCenterNo.SelectedValue);
            lbEmployees.DataSource = dsEmployees;
            lbEmployees.DataBind();
            for (int i = 0; i < lbEmployees.Items.Count; i++)
            {
                lbEmployees.Items[i].Selected = true;
            }
        }
    }

    public string getSelectedEmployees()
    {
        string Employees = null;
        for (int i = 0; i < lbEmployees.Items.Count; i++)
        {
            if (lbEmployees.Items[i].Selected)
            {
                if (Employees == null)
                {
                    Employees = lbEmployees.Items[i].Value;
                }
                else
                {
                    Employees = Employees + "," + lbEmployees.Items[i].Value;
                }
            }
        }
        return Employees;
    }

    protected void ddSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Refersh Employees list box
        DataSet dsEmployees = ATMDB.GetDataSet("up_getEmployees", UserName, ddSygmaCenterNo.SelectedValue);
        lbEmployees.DataSource = dsEmployees;
        lbEmployees.DataBind();
        for (int i = 0; i < lbEmployees.Items.Count; i++)
        {
            lbEmployees.Items[i].Selected = true;
        }
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        Session["eprEmployeeIds"] = getSelectedEmployees();
        Session["eprCenterNo"] = ddSygmaCenterNo.SelectedValue;
        Session["eprRangeBy"] = ddRangeBy.SelectedItem.Text;
        Session["eprFromDate"] = txtStartDate.Text;
        Session["eprToDate"] = txtEndDate.Text;
        Session["eprGroupByWeekend"] = cbGroupByWeekend.Checked;
        //Response.Redirect("~/Apps/ATM/Reports/DisplayEmployeePayroll.aspx");
        //if (Session["NextPageNumber"] != null)
        //{
        //ScriptManager.RegisterStartupScript(this, typeof(string), "DisplayEmployeeReport" + Session["NextPageNumber"].ToString(), "window.open( 'DisplayEmployeePayroll.aspx', null, 'height=800,width=1280,status=yes,toolbar=yes,menubar=yes,location=yes, scrollbars=yes' );", true);
        //Session["NextPageNumber"] = Convert.ToInt32(Session["NextPageNumber"]) + 1;
        //}
        //else
        //{
        ScriptManager.RegisterStartupScript(this, typeof(string), "DisplayEmployeeReport1", "window.open( 'DisplayEmployeePayroll.aspx', null, 'height=800,width=1000,status=yes,toolbar=yes,menubar=yes,location=yes, scrollbars=yes, resizable=yes' );", true);
        //Session["NextPageNumber"] = 2;
        //}
    }

    protected void txtWeekendingDate_OnTextChanged(object sender, EventArgs e)
    {
        DateTime selectedDate = Convert.ToDateTime(txtStartDate.Text);
        txtEndDate.Text = selectedDate.AddDays(7 - Convert.ToInt32(selectedDate.DayOfWeek)).ToString("MM/dd/yyyy");
    }
}