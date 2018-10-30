using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;

public partial class Apps_ATM_Tools_BatchPayTool : ATMPage
{
    private string gFormIds = "";
    public DataSet gDsForms;
    protected override void LoadATMPage()
    {
        if (!IsPostBack)
        {
            SetCenters();
            SetPayScale();
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
    public void SetPayScale()
    {
        DataSet dsPayScale = ATMDB.GetDataSet("up_p_getPayScales");
        ddPayScale.DataSource = dsPayScale;
        ddPayScale.DataBind();
        if (dsPayScale.Tables[0].Rows.Count > 0)
        {
            DataSet dsRateTypes = ATMDB.GetDataSet("up_p_getPayRatesWithCategories", ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue);
            //DataView dvEnabled = new DataView(dsRateTypes.Tables[0], "IsEnabled = 1", "RateTypeId asc", DataViewRowState.CurrentRows);
            ddAddRateType.DataSource = dsRateTypes.Tables[0];
            ddAddRateType.DataBind();
            //string categoryFilter = string.Format("RateTypeId={0}", dsRateTypes.Tables[0].Rows[0][0]);
            if (dsRateTypes.Tables[1].Rows.Count > 0)
            {
                if (dsRateTypes.Tables[0].Rows.Count > 0)
                {
                    string categoryFilter = string.Format("RateTypeId={0}", dsRateTypes.Tables[0].Rows[0][0]);
                    rfvNotes.Visible = Convert.ToBoolean(dsRateTypes.Tables[0].Rows[0][2]);
                    //if (dsRateTypes.Tables[0].Rows[0][2].ToString() == "0")
                    //{
                    //    rfvNotes.Visible = false;
                    //}
                    //else
                    //{
                    //    rfvNotes.Visible = true;
                    //}
                    DataView dvCategories = new DataView(dsRateTypes.Tables[1], categoryFilter, "CategoryId asc", DataViewRowState.CurrentRows);
                    if (dvCategories.Count > 0)
                    {
                        pnlCategory.Visible = true;
                        ddAddCategory.DataSource = dvCategories;
                        ddAddCategory.DataBind();
                    }
                    else
                    {
                        pnlCategory.Visible = false;
                    }
                }
                else
                {
                    pnlCategory.Visible = false;
                }
            }
            else
            {
                pnlCategory.Visible = false;
            }
        }
    }

    public static void OpenNewWindow(System.Web.UI.Page page, string fullUrl, string key)
    {
        string script = "window.open('" + fullUrl + "', '" + key + "', 'status=1,location=1,menubar=1,resizable=1,toolbar=1,scrollbars=1,titlebar=1');";
        page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, script, true);
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            try
            {
                //DataSet dsForms;
                string strNotes;
                strNotes = txtNotes.Text.Length > 0 ? txtNotes.Text : null;
                if (pnlCategory.Visible)
                {
                    gDsForms = ATMDB.GetDataSet("up_p_createMiscFormsWithEmployees", UserName, 2, ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue, ddAddRateType.SelectedValue, txtQuantity.Text, getSelectedEmployees(), RouteNoEdit, dteDepartDate.Text, dteWeekendingDate.Text, ddAddCategory.SelectedValue, strNotes);
                }
                else
                {
                    gDsForms = ATMDB.GetDataSet("up_p_createMiscFormsWithEmployees", UserName, 2, ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue, ddAddRateType.SelectedValue, txtQuantity.Text, getSelectedEmployees(), RouteNoEdit, dteDepartDate.Text, dteWeekendingDate.Text, null, strNotes);
                }
                //gDsForms = ATMDB.GetDataSet("up_p_createMiscFormsWithEmployees", UserName, 2, ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue, ddAddRateType.SelectedValue, txtQuantity.Text, getSelectedEmployees(), RouteNoEdit, dteDepartDate.Text, dteWeekendingDate.Text);
                gFormIds = gDsForms.Tables[1].Rows[0][0].ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Javascript", "javascript:ConfirmationWithIds('" + gDsForms.Tables[1].Rows[0][0].ToString().Trim() + "');", true);
                //if (true)
                //{

                //}
                //foreach (DataRow drForm in gDsForms.Tables[0].Rows)
                //{
                //    i += 1;
                //    string URL = string.Format("http://localhost:9091/Apps/ATM/Payroll/Forms/AddUpdate.aspx?fid={0}", drForm[0].ToString());
                //    OpenNewWindow(this, URL, "Form" + i.ToString());

                //}
                //this.Javascript.Notify("Misc Forms Created Successfully. Form IDs are " + gFormIds);
                //Response.Write("<script>alert('Misc Forms Created Successfully. Form IDs are " + gFormIds + "');</script>");
                //RedirectToSelf("fid", gFormId.ToString());

                //SetFormLoaded();
                //SetInitialForm();
                //upStatus.Update();
                //SetFormInfoView();
            }
            catch (Exception exp)
            {
                throw new Exception("Error generating Misc Forms", exp);
            }
        }
    }


    protected void btnForms_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Apps/ATM/Payroll/Forms/Index.aspx");
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
        //refresh rate type drop down
        DataSet dsRateTypes = ATMDB.GetDataSet("up_p_getPayRatesWithCategories", ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue);
        //DataView dvEnabled = new DataView(dsRateTypes.Tables[0], "IsEnabled = 1", "RateTypeId asc", DataViewRowState.CurrentRows);\
        ddAddRateType.DataSource = dsRateTypes.Tables[0];
        ddAddRateType.DataBind();
        //refresh category dropm down
        if (dsRateTypes.Tables[0].Rows.Count > 0)
        {
            string categoryFilter = string.Format("RateTypeId={0}", dsRateTypes.Tables[0].Rows[0][0]);
            rfvNotes.Visible = Convert.ToBoolean(dsRateTypes.Tables[0].Rows[0][2]);
            if (dsRateTypes.Tables[1].Rows.Count > 0)
            {
                DataView dvCategories = new DataView(dsRateTypes.Tables[1], categoryFilter, "CategoryId asc", DataViewRowState.CurrentRows);
                if (dvCategories.Count > 0)
                {
                    pnlCategory.Visible = true;
                    ddAddCategory.DataSource = dvCategories;
                    ddAddCategory.DataBind();
                }
                else
                {
                    pnlCategory.Visible = false;
                }
            }
            else
            {
                pnlCategory.Visible = false;
            }
        }
        else
        {
            pnlCategory.Visible = false;
        }
    }
    protected void ddPayScale_SelectedIndexChanged(object sender, EventArgs e)
    {
        //refresh rate type drop down
        DataSet dsRateTypes = ATMDB.GetDataSet("up_p_getPayRatesWithCategories", ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue);
        //DataView dvEnabled = new DataView(dsRateTypes.Tables[0], "IsEnabled = 1", "RateTypeId asc", DataViewRowState.CurrentRows);
        ddAddRateType.DataSource = dsRateTypes.Tables[0];
        ddAddRateType.DataBind();
        if (dsRateTypes.Tables[0].Rows.Count > 0)
        {
            string categoryFilter = string.Format("RateTypeId={0}", dsRateTypes.Tables[0].Rows[0][0]);
            rfvNotes.Visible = Convert.ToBoolean(dsRateTypes.Tables[0].Rows[0][2]);
            if (dsRateTypes.Tables[1].Rows.Count > 0)
            {
                DataView dvCategories = new DataView(dsRateTypes.Tables[1], categoryFilter, "CategoryId asc", DataViewRowState.CurrentRows);
                if (dvCategories.Count > 0)
                {
                    pnlCategory.Visible = true;
                    ddAddCategory.DataSource = dvCategories;
                    ddAddCategory.DataBind();
                }
                else
                {
                    pnlCategory.Visible = false;
                }
            }
            else
            {
                pnlCategory.Visible = false;
            }
        }
        else
        {
            pnlCategory.Visible = false;
        }

    }

    protected void ddAddRateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet dsRateTypes = ATMDB.GetDataSet("up_p_getPayRatesWithCategories", ddSygmaCenterNo.SelectedValue, ddPayScale.SelectedValue);
        string categoryFilter = string.Format("RateTypeId={0}", ddAddRateType.SelectedValue);
        string NotesFilter = string.Format("RateTypeId={0}", ddAddRateType.SelectedValue);
        DataView dvNotes = new DataView(dsRateTypes.Tables[0], NotesFilter, "RateTypeId asc", DataViewRowState.CurrentRows);
        rfvNotes.Visible = Convert.ToBoolean(dvNotes[0][2]);
        if (dsRateTypes.Tables[1].Rows.Count > 0)
        {
            DataView dvCategories = new DataView(dsRateTypes.Tables[1], categoryFilter, "CategoryId asc", DataViewRowState.CurrentRows);
            if (dvCategories.Count > 0)
            {
                pnlCategory.Visible = true;
                ddAddCategory.DataSource = dvCategories;
                ddAddCategory.DataBind();
            }
            else
            {
                pnlCategory.Visible = false;
            }
        }
        else
        {
            pnlCategory.Visible = false;
        }
    }

    protected void cuvRouteNo_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (IsValid)
        {
            args.IsValid = (bool)ATMDB.GetScalar("up_p_isRouteNoValid", ddSygmaCenterNo.SelectedValue, RouteNoEdit, Convert.ToDateTime(dteWeekendingDate.Text));
            //cuvRouteNo.ErrorMessage = "Test";
            //cuvRouteNo.Text = "TEst";
            //if (!args.IsValid)
            //  FireValidationFailedMessage((CustomValidator)source);
        }
    }

    protected void cuvQuantity_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (IsValid)
        {
            txtQuantity.Text = Convert.ToString(ATMDB.GetScalar("up_p_getValidQuantity", ddAddRateType.SelectedValue, txtQuantity.Text));
            args.IsValid = true;
            //cuvQuantity.Text = "TEst";
            //if (!args.IsValid)
            //  FireValidationFailedMessage((CustomValidator)source);
        }
    }

    public string RouteNoEdit
    {
        get
        {

            return string.Concat("M", txtRouteNo.Text);
        }
    }
}