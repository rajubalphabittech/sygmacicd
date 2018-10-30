using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using atm;

public partial class Apps_ATM_Tools_BatchFuelTicketTool : ATMPage
{
    protected override void LoadATMPage()
    {
        if (!IsPostBack)
        {
            SetCenters();
            rblType.SelectedIndex = 0;
            cbImport.Checked = false;
            SetPanels();
            SetSellerDetails();
        }

    }
    public void SetCenters()
    {
        DataSet dsCenters = ATMDB.GetDataSet("up_getCenters", UserName);
        ddSygmaCenterNo.DataSource = dsCenters;
        ddSygmaCenterNo.DataBind();
        ddSygmaCenterNo.Items.Insert(0, "Choose...");
        ddSygmaCenterNo.SelectedIndex = 0;
        if (ddSygmaCenterNo.Items.Count == 2)
        {
            ddSygmaCenterNo.SelectedIndex = 1;
            CenterSelectionIndexChanged();
        }
    }

    public void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddSygmaCenterNo.SelectedIndex = 0;
        txtWeekendingDate.Text = "";
        txtWeekendingDate.Enabled = false;
        //ClearVehicleDetails();
        //ClearFTDetails();
        //ClearSellerDetails();
        ClearAllDetails();
        SetPanels();
        lblMessage.Text = "";
        lblInvalidDate.Visible = false;

        if (ddSygmaCenterNo.Items.Count == 2)
        {
            ddSygmaCenterNo.SelectedIndex = 1;
            CenterSelectionIndexChanged();
        }
    }
    private void SetPanels()
    {
        if (!cbImport.Checked)
        {
            pnlMain.Visible = true;
            pnlUploadFile.Visible = false;
            if (rblType.SelectedIndex == 0)
            {
                pnlVehicles.Visible = true;
                pnlTrailers.Visible = false;
                pnlOdometer.Visible = true;
                pnlHours.Visible = false;
            }
            else
            {
                pnlVehicles.Visible = false;
                pnlTrailers.Visible = true;
                pnlOdometer.Visible = false;
                pnlHours.Visible = true;
            }
            SetSellerDetails();
        }
        else
        {
            pnlMain.Visible = false;
            pnlUploadFile.Visible = true;
        }

    }
    protected void cbImport_CheckedChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        lblInvalidDate.Visible = false;
        ddSygmaCenterNo.SelectedIndex = 0;

        if (ddSygmaCenterNo.Items.Count == 2)
        {
            ddSygmaCenterNo.SelectedIndex = 1;
            CenterSelectionIndexChanged();
        }

        txtWeekendingDate.Text = "";
        ClearAllDetails();
        SetPanels();
    }
    protected void chkLocation_CheckedChanged(object sender, EventArgs e)
    {
        SetSellerDetails();
    }
    private void SetSellerDetails()
    {
        if (chkLocation.Checked)
        {
            pnlSellerDetails.Visible = true;
        }
        else
        {
            ClearSellerDetails();
            pnlSellerDetails.Visible = false;
        }
    }

    protected void txtWeekendingDate_TextChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        DateTime date;
        if (DateTime.TryParse(txtWeekendingDate.Text, out date))
        { 
            lblInvalidDate.Visible = false;
            if (date >= Convert.ToDateTime("01-01-2001") && date <= Convert.ToDateTime("01-01-2099"))
            {
                DataView dvForms = ATMDB.GetDataView("up_p_getFormsByWeekending", ddSygmaCenterNo.SelectedValue, txtWeekendingDate.Text);
                lbRoutes.DataSource = dvForms;
                lbRoutes.DataBind();
                lbRoutes.Items.Insert(0, "Choose...");
                upFormInfo.Update();
            }
            else
            {
                txtWeekendingDate.Text = "";
                lblInvalidDate.Text = "Invalid Date; Outside available Date Range! ";
                lblInvalidDate.Visible = true;
            }
        }
        else
        {
            lbRoutes.Items.Clear();
            if (txtWeekendingDate.Text != "")
            {
                lblInvalidDate.Text = "Weekending Date is invalid!";
                lblInvalidDate.Visible = true;
            }
            txtWeekendingDate.Text = "";
            upFormInfo.Update();
        }
        
    }
    protected void ddSygmaCenterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
       CenterSelectionIndexChanged();
    }

    protected void CenterSelectionIndexChanged()
    {
        ClearAllDetails();
        lblMessage.Text = "";
        lblInvalidDate.Visible = false;
        if (ddSygmaCenterNo.SelectedIndex > 0)
        {
            btnAdd.Enabled = true;
            txtWeekendingDate.Enabled = true;
            DataSet dsVehicles = ATMDB.GetDataSet("up_p_getVehiclesByCenter", ddSygmaCenterNo.SelectedValue);
            DataSet dsTrailers = ATMDB.GetDataSet("up_p_getTrailersByCenter", ddSygmaCenterNo.SelectedValue);
            lbVehicles.DataSource = dsVehicles.Tables[0];
            lbVehicles.DataBind();
            lbTrailers.DataSource = dsTrailers.Tables[0];
            lbTrailers.DataBind();
        }
        else
        {
            txtWeekendingDate.Enabled = false;
            btnAdd.Enabled = false;
        }
        upFormInfo.Update();
    }

    private void ClearFTDetails()
    {
        txtTicketNo.Text = "";
        txtDatePurchased.Text = "";
        txtGallons.Text = "";
        txtAmount.Text = "";
        txtOdometer.Text = "";
        txtHours.Text = "";
    }
    private void ClearVehicleDetails()
    {
        lbRoutes.Items.Clear();
        lbVehicles.Items.Clear();
        lbTrailers.Items.Clear();
    }
    private void ClearSellerDetails()
    {
        txtSellerName.Text = "";
        txtAddress1.Text = "";
        txtAddress2.Text = "";
        txtZip.Text = "";
    }
    private void ClearAllDetails()
    {
        txtWeekendingDate.Text = "";
        lblInvalidDate.Visible = false;
        ClearVehicleDetails();
        ClearFTDetails();
        ClearSellerDetails();
    }
    protected void btnForms_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Apps/ATM/Payroll/Forms/Index.aspx");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string vehicleId = "";
        string trailerId = "";
        string fuelTicketNo = txtTicketNo.Text;
        string datePurchased = txtDatePurchased.Text;
        string gallons = txtGallons.Text;
        string miles = txtOdometer.Text;
        string hours = txtHours.Text;
        string price = txtAmount.Text;
        string sellerName = txtSellerName.Text;
        string address1 = txtAddress1.Text;
        string address2 = txtAddress2.Text;
        string zip = txtZip.Text;
        string formId = null;
        if (lbRoutes.GetSelectedIndices().Count() > 0)
        {
            if (lbRoutes.SelectedIndex > 0)
            {
                formId = lbRoutes.SelectedItem.Value.ToString();
            }
            
        }

        if (rblType.SelectedIndex == 0)
        {
            try
            {
                int[] selectedVehicles = lbVehicles.GetSelectedIndices();
                int ret = 0;
                string message = "";
                string errorVehicles = "";
                string existingFTVehicles = "";
                string existingFTAllVehicles = "";
                lblMessage.Text = "";
                lblExisting.Text = "";
                lblAllExisting.Text = "";
                foreach (int index in selectedVehicles)
                {
                    vehicleId = lbVehicles.Items[index].Value.ToString();
                    string vehicleName = lbVehicles.Items[index].Text.ToString();
                    int added = Convert.ToInt32(GetScalarFromStatic("ATM", "up_f_addBulkVehicleFuelTicket", UserName, vehicleId, fuelTicketNo, datePurchased, gallons, miles, price, formId, sellerName, address1, address2, zip));
                    if (added == 1)
                    {
                        ret += added;
                    }
                    else if (added == -1)
                    {
                        if (errorVehicles == "")
                        {
                            errorVehicles += vehicleName;
                        }
                        else
                        {
                            //if(errorVehicles.Substring(errorVehicles.Length - 1) != ",")
                            errorVehicles += ", " + vehicleName;
                        }
                               
                    }
                    else if (added == -2)
                    {
                        if (existingFTVehicles == "")
                        {
                            existingFTVehicles += vehicleName;
                        }
                        else
                        {
                            //if(errorVehicles.Substring(errorVehicles.Length - 1) != ",")
                            existingFTVehicles += ", " + vehicleName;
                        }
                    }
                    //else
                    //{
                    //    existingFTAllVehicles += vehicleName + "; ";
                    //}

                }
                if (selectedVehicles.Count() == ret)
                {
                    if (formId != null)
                    {
                        //lblMessage.Text = 
                         message = String.Format("Fuel Ticket successfully added for all selected Vehicles on Form# {0}", formId);
                        //lblMessage.Visible = true;
                    }
                    else
                    {
                        //lblMessage.Text = String.Format("Fuel Ticket successfully added for all selected Vehicles");
                        //lblMessage.Visible = true;
                        message = String.Format("Fuel Ticket successfully added for all selected Vehicles");
                    }
                    ddSygmaCenterNo.SelectedIndex = 0;

                    if (ddSygmaCenterNo.Items.Count == 2)
                    {
                        ddSygmaCenterNo.SelectedIndex = 1;
                        CenterSelectionIndexChanged();
                    }

                    txtWeekendingDate.Text = "";
                    ClearAllDetails();
                }
                else
                {
                    message = String.Format("One or more Fuel Ticket not added for Vehicles");
                    if (errorVehicles != "")
                    {                      
                        lblMessage.Text = String.Format("Fuel Ticket not added for Vehicles - {0}", errorVehicles);
                        lblMessage.Visible = true;
                        errorVehicles = "";
                    }
                    if (existingFTVehicles != "")
                    {
                        lblExisting.Text = String.Format("Same Fuel Ticket or same odometer exists for Vehicles - {0}", existingFTVehicles);
                        lblExisting.Visible = true;
                        existingFTVehicles = "";
                    }
                    //if (existingFTAllVehicles != "")
                    //{
                    //    lblAllExisting.Text = String.Format("Same Fuel Ticket for same odometer exists for Vehicles - {0}", existingFTAllVehicles);
                    //    lblAllExisting.Visible = true;
                    //    existingFTAllVehicles = "";
                    //}
                }
                //RunNonQueryFromStatic("ATM", "up_f_addSellerDetails", UserName, fuelTicketNo, datePurchased, sellerName, address1, address2, zip, "Vehicle");
                //string script = String.Format("SuccessMessage({0});", message);  
                string script = String.Format("Message('{0}');", message);               
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script", script, true);
                
                
            }
            catch (Exception ex)
            {
                SygmaFramework.ErrorHandler eh = new SygmaFramework.ErrorHandler(ex);
                string[] parms = new string[3];
                parms[0] = String.Format("Error when adding Batch Fuel Ticket - Vehicles");
                parms[1] = String.Format("UserName: {0} VehicleId: {1}, FuelTicketNo: {2}, Date Purchased: {3}, Gallons: {4}, Miles: {5}, Price: {6}, FormId: {7}", UserName, vehicleId, fuelTicketNo, datePurchased, gallons, miles, price, formId);
                parms[2] = String.Format("Exception: {0}", ex.ToString());
                eh.ReportError("Batch Fuel Ticket Tool", parms);
            }
        }
        else
        {
            try
            {
                int[] selectedTrailers = lbTrailers.GetSelectedIndices();
                int ret = 0;
                string message = "";
                string errorTrailers = "";
                //string existingFTTrailers = "";
                //string existingFTAllTrailers = "";
                lblMessage.Text = "";
                lblExisting.Text = "";
                lblAllExisting.Text = "";
                foreach (int index in selectedTrailers)
                {
                    trailerId = lbTrailers.Items[index].Value.ToString();
                    string trailerName = lbTrailers.Items[index].Text.ToString();
                    int added = Convert.ToInt32(GetScalarFromStatic("ATM", "up_f_addBulkTrailerFuelTicket", UserName, trailerId, fuelTicketNo, datePurchased, gallons, hours, price, formId, sellerName, address1, address2, zip));
                    if (added == 1)
                    {
                        ret += added;
                    }
                    else if (added == -1)
                    {
                        //errorTrailers += trailerName + " ";
                        if (errorTrailers == "")
                        {
                            errorTrailers += trailerName;
                        }
                        else
                        {
                            //if(errorVehicles.Substring(errorVehicles.Length - 1) != ",")
                            errorTrailers += ", " + trailerName;
                        }
                          
                    }
                    //else if (added == -2)
                    //{
                    //    existingFTTrailers += trailerName + "; ";
                    //}
                }
                if (selectedTrailers.Count() == ret)
                {
                    if (formId != null)
                    {
                        //lblMessage.Text = String.Format("Fuel Ticket successfully added for all selected Trailers on Form# {0}", formId);
                        //lblMessage.Visible = true;
                        message = String.Format("Fuel Ticket successfully added for all selected Trailers on Form# {0}", formId);
                    }
                    else
                    {
                        //lblMessage.Text = String.Format("Fuel Ticket successfully added for all selected Trailers");
                        //lblMessage.Visible = true;
                        message = String.Format("Fuel Ticket successfully added for all selected Trailers");
                    }
                    ddSygmaCenterNo.SelectedIndex = 0;

                    if (ddSygmaCenterNo.Items.Count == 2)
                    {
                        ddSygmaCenterNo.SelectedIndex = 1;
                        CenterSelectionIndexChanged();
                    }

                    txtWeekendingDate.Text = "";
                    ClearAllDetails();
                }
                else
                {
                    message = String.Format("One or more Fuel Ticket not added for Trailers");
                    if (errorTrailers != "")
                    {
                        lblMessage.Text = String.Format("Fuel Ticket not added for Trailers - {0}", errorTrailers);
                        lblMessage.Visible = true;
                        errorTrailers = "";
                    }
                    //if (existingFTTrailers != "")
                    //{
                    //    lblExisting.Text = String.Format("Same Fuel Ticket exists for Trailers - {0}", existingFTTrailers);
                    //    lblExisting.Visible = true;
                    //    existingFTTrailers = "";
                    //}
                    //if (existingFTAllTrailers != "")
                    //{
                    //    lblAllExisting.Text = String.Format("Same Fuel Ticket for same odometer exists for Trailers - {0}", existingFTAllTrailers);
                    //    lblAllExisting.Visible = true;
                    //    existingFTAllTrailers = "";
                    //}
                }
                //RunNonQueryFromStatic("ATM", "up_f_addSellerDetails", UserName, fuelTicketNo, datePurchased, sellerName, address1, address2, zip, "Trailer");
                string script = String.Format("Message('{0}');", message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script", script, true);

                
            }
            catch (Exception ex)
            {
                SygmaFramework.ErrorHandler eh = new SygmaFramework.ErrorHandler(ex);
                string[] parms = new string[3];
                parms[0] = String.Format("Error when adding Batch Fuel Ticket - Trailers");
                parms[1] = String.Format("UserName: {0}, TrailerId: {1}, FuelTicketNo: {2}, Date Purchased: {3}, Gallons: {4}, Hours: {5}, Price: {6}, FormId: {7}", UserName, vehicleId, fuelTicketNo, datePurchased, gallons, miles, price, formId);
                parms[2] = String.Format("Exception: {0}", ex.ToString());
                eh.ReportError("Batch Fuel Ticket Tool", parms);
            }

        }
        
    }
}