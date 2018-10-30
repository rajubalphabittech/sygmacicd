using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;

public partial class Apps_ATM_Tools_AddEmployee : BasePage
{
    protected override void LoadBasePage()
    {
        if (!Convert.ToBoolean(ATMDB.GetScalar("up_sec_isValidUser", UserName, "~/Apps/ATM/Tools/ManageEmployees.aspx")))
        {
            GotoInvalidUserPage();
        }
        else
        {
            if (!IsPostBack)
            {
                LoadCenters();
            }
        }
    }

    private void LoadCenters()
    {
        DataView dv = ATMDB.GetDataView("up_getCenters", UserName);

        ddSygmaCenterNo.DataSource = dv;
        ddSygmaCenterNo.DataBind();
        ddSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", "-1"));

        ddEmployee.Items.Insert(0, new ListItem("Choose...", "-1"));
    }

    private void LoadEmployees()
    {
        DataView dv = ATMDB.GetDataView("up_getNonATMEmployees", ddSygmaCenterNo.SelectedValue);
        ddEmployee.DataSource = dv;
        ddEmployee.DataBind();
        ddEmployee.Items.Insert(0, new ListItem("Choose...", "-1"));
    }

    protected void ddSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadEmployees();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            ATMDB.GetScalar("up_p_addEmployeeException", UserName, ddEmployee.SelectedValue);
            LoadEmployees();
            this.Javascript.Notify("Employee added to ATM successfully");
            //Session["AddSuccess"] = true;
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Employee added", "window.location.replace(window.location);", true);
        }
        catch (Exception exp)
        {
            throw new Exception("Error adding Employee to ATM import exception", exp);
        }
    }
}