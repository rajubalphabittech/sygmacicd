using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;

public partial class Apps_ATM_Tools_GuaranteedPay : ATMPage
{
    protected override void LoadATMPage()
    {
        if (!IsPostBack)
        {
            DataSet dsCenter = ATMDB.GetDataSet("up_getCenters ", UserName);
            ddSygmaCenterNo.DataSource = dsCenter;
            ddSygmaCenterNo.DataBind();
            txtWeekendingDate.Text = (DateTime.Now.AddDays(6 - Convert.ToInt32(DateTime.Now.DayOfWeek))).ToShortDateString();
        }
    }

    protected void btnRunNow_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet dsForms = ATMDB.GetDataSet("up_p_GuaranteedPay", UserName, ddSygmaCenterNo.SelectedValue, txtWeekendingDate.Text);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:ConfirmationWithIds('" + dsForms.Tables[1].Rows[0][0].ToString() + "');", true);
        }
        catch (Exception exp)
        {
            throw new Exception("Error generating Misc Forms", exp);
        }
    }
}