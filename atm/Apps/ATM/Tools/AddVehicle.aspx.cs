using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;
using SygmaFramework;

public partial class Apps_ATM_Tools_AddVehicle : BasePage
{
	protected bool IsRental
	{
		get
		{
			var isRentalAsString = Request.QueryString["IsRental"];
			return isRentalAsString == "true";
		}
	}

	private DataTable dtVehicleType;
	private DataTable dtVehicleMake;
	private DataTable dtVehicleStatus;
	private DataTable dtVehicleObjectType;
	private DataTable dtYear;
	private DataTable dtReplacedYear;
	private Database gATMDB;
	protected override void LoadBasePage()
	{
		SetPageVariables();
		if (!IsPostBack)
		{
			SetLabels();
			LoadCenters();
			LoadDropDownLists();
		}
	}

	private void SetLabels()
	{
		litTitle.Text = IsRental ? "Add Rental Vehicle" : "Add Vehicle";
		litPanelHeader.Text = IsRental ? "Add Rental Vehicle" : "Add Vehicle";
		litNameLabel.Text = IsRental ? "Rental Vehicle Name" : "Vehicle Name";
		litRentalIndicator.Text = IsRental ? "R" : "";
		litPageTitle.Text = IsRental ? "ATM - Add Rental Vehicle" : "ATM - Add Vehicle";
	}

	private void SetPageVariables()
	{
		AddClientVariable("gUserName", UserName);
		gATMDB = new Database("ATM");
	}

	private void LoadDropDownLists()
	{
		LoadYear();
		LoadReplacedYear();
		LoadMakeStatusObjectType();

		//Load Make Dropdown list
		ddMake.Items.Clear();
		ddMake.Items.Add(new ListItem("Choose...", "0"));
		foreach (DataRow vm in dtVehicleMake.Rows)
		{
			ListItem liVM = new ListItem(vm["VehicleMake"].ToString(), vm["MakeId"].ToString());
			ddMake.Items.Add(liVM);
		}

		//Load Replaced Year Dropdown list
		ddReplacedYear.DataSource = dtReplacedYear;
		ddReplacedYear.DataBind();

		//Load Cab Type Dropdown list
		ddType.Items.Add(new ListItem("Choose...", "0"));
		foreach (DataRow vs in dtVehicleType.Rows)
		{
			ListItem liVS = new ListItem(vs["VehicleType"].ToString(), vs["TypeId"].ToString());
			ddType.Items.Add(liVS);
		}

		//Load Status Dropdown list
		ddStatus.Items.Add(new ListItem("Choose...", "0"));
		foreach (DataRow vs in dtVehicleStatus.Rows)
		{
			ListItem liVS = new ListItem(vs["VehicleStatus"].ToString(), vs["StatusId"].ToString());
			ddStatus.Items.Add(liVS);
		}

		//Load Year Dropdown list
		ddYear.DataSource = dtYear;
		ddYear.DataBind();

		//Load Object type Dropdown list
		ddObjType.Items.Add(new ListItem("Choose...", "0"));
		foreach (DataRow vt in dtVehicleObjectType.Rows)
		{
			ListItem liVT = new ListItem(vt["VehicleObjectType"].ToString(), vt["ObjectTypeId"].ToString());
			ddObjType.Items.Add(liVT);
		}
	}

	private void LoadMakeStatusObjectType()
	{
		DataSet dsTypeMakeStatusObject = gATMDB.GetDataSet("up_getVehicle_Type_Make_Status_ObjectType");
		dtVehicleType = dsTypeMakeStatusObject.Tables[0];
		dtVehicleMake = dsTypeMakeStatusObject.Tables[1];
		dtVehicleStatus = dsTypeMakeStatusObject.Tables[2];
		dtVehicleObjectType = dsTypeMakeStatusObject.Tables[3];
	}

	private void LoadYear()
	{
		dtYear = new DataTable();
		dtYear.Columns.Add("Id", typeof(int));
		dtYear.Columns.Add("Year", typeof(string));
		DataRow drFirstRow = dtYear.NewRow();
		drFirstRow["Id"] = 0;
		drFirstRow["Year"] = "Choose...";
		dtYear.Rows.Add(drFirstRow);
		for (int i = DateTime.Now.AddYears(-25).Year; i <= DateTime.Now.AddYears(2).Year; i++)
		{
			DataRow drNewRow = dtYear.NewRow();
			drNewRow["Id"] = i;
			drNewRow["Year"] = i;
			dtYear.Rows.Add(drNewRow);
		}
	}

	private void LoadReplacedYear()
	{
		dtReplacedYear = new DataTable();
		dtReplacedYear.Columns.Add("ReplacedId", typeof(string));
		dtReplacedYear.Columns.Add("ReplacedYear", typeof(string));
		DataRow drFirstRow = dtReplacedYear.NewRow();
		drFirstRow["ReplacedId"] = 0;
		drFirstRow["ReplacedYear"] = "Choose...";
		dtReplacedYear.Rows.Add(drFirstRow);
		for (int i = DateTime.Now.AddYears(-25).Year; i <= DateTime.Now.AddYears(2).Year; i++)
		{
			DataRow drNewRow = dtReplacedYear.NewRow();
			drNewRow["ReplacedId"] = "FY" + i.ToString().Substring(2);
			drNewRow["ReplacedYear"] = "FY" + i.ToString().Substring(2);
			dtReplacedYear.Rows.Add(drNewRow);
		}
	}

	private void LoadCenters()
	{
		DataView dv = gATMDB.GetDataView("up_getCenters", UserName);
		ddSygmaCenterNo.DataSource = dv;
		ddSygmaCenterNo.DataBind();
		ddSygmaCenterNo.Items.Insert(0, new ListItem("Choose...", "0"));
	}

	protected void btnAdd_Click(object sender, EventArgs e)
	{
		object[] parms = new object[43];
		try
		{
			parms[0] = ddSygmaCenterNo.SelectedValue;

			if (IsRental)
			{
				parms[1] = "R" + txtRentalVehicleName.Text;
			}
			else
			{
				parms[1] = txtRentalVehicleName.Text;
			}

			if (!IsVehicleExist(parms[1].ToString(), Convert.ToInt32(parms[0])))
			{
				if (txtUnitNo.Text != "")
				{
					parms[2] = Convert.ToInt32(txtUnitNo.Text);
				}
				else
				{
					parms[2] = 9999999;
				}
				parms[3] = Convert.ToInt32(ddMake.SelectedValue);
				parms[4] = txtVIN.Text;
				parms[5] = ddReplacedYear.SelectedValue;
				parms[6] = txtModel.Text;
				parms[7] = Convert.ToInt32(ddType.SelectedValue);
				parms[8] = Convert.ToInt32(ddStatus.SelectedValue);
				parms[9] = Convert.ToInt32(ddYear.SelectedValue);
				parms[10] = txtDescription.Text;
				parms[11] = Convert.ToInt32(ddObjType.SelectedValue);
				parms[12] = Convert.ToInt32(chkActive.Checked);
				if (txtInServiceDate.Text != "")
				{
					parms[13] = Convert.ToDateTime(txtInServiceDate.Text).ToShortDateString();
				}
				if (txtDeprStartDate.Text != "")
				{
					parms[14] = Convert.ToDateTime(txtDeprStartDate.Text).ToShortDateString();
				}
				if (txtDeprEndDate.Text != "")
				{
					parms[15] = Convert.ToDateTime(txtDeprEndDate.Text).ToShortDateString();
				}
				if (txtDeprMonths.Text != "")
				{
					parms[16] = Convert.ToInt32(txtDeprMonths.Text);
				}
				if (txtCost.Text != "")
				{
					parms[17] = Convert.ToDouble(txtCost.Text);
				}
				if (txtMonthlyDepreciation.Text != "")
				{
					parms[18] = Convert.ToDouble(txtMonthlyDepreciation.Text);
				}
				parms[19] = txtLiftgate.Text;
				int rentalVehicleHeightFeet = 0;
				int rentalVehicleHeightInch = 0;
				int rentalVehicleLengthFeet = 0;
				int rentalVehicleLengthInch = 0;
				int rentalVehicleWidthFeet = 0;
				int rentalVehicleWidthInch = 0;
				if (txtRentalVehicleHeightFeet.Text != "")
				{
					rentalVehicleHeightFeet = Convert.ToInt32(txtRentalVehicleHeightFeet.Text);
				}
				if (txtRentalVehicleHeightInch.Text != "")
				{
					rentalVehicleHeightInch = Convert.ToInt32(txtRentalVehicleHeightInch.Text);
				}
				if (txtRentalVehicleLengthFeet.Text != "")
				{
					rentalVehicleLengthFeet = Convert.ToInt32(txtRentalVehicleLengthFeet.Text);
				}
				if (txtRentalVehicleLengthInch.Text != "")
				{
					rentalVehicleLengthInch = Convert.ToInt32(txtRentalVehicleLengthInch.Text);
				}
				if (txtRentalVehicleWidthFeet.Text != "")
				{
					rentalVehicleWidthFeet = Convert.ToInt32(txtRentalVehicleWidthFeet.Text);
				}
				if (txtRentalVehicleWidthInch.Text != "")
				{
					rentalVehicleWidthInch = Convert.ToInt32(txtRentalVehicleWidthInch.Text);
				}
				parms[20] = (rentalVehicleHeightFeet * 12) + rentalVehicleHeightInch;
				parms[21] = (rentalVehicleLengthFeet * 12) + rentalVehicleLengthInch;
				parms[22] = (rentalVehicleWidthFeet * 12) + rentalVehicleWidthInch;
				if (txtRoadsideDoorsNo.Text != "")
				{
					parms[23] = Convert.ToInt32(txtRoadsideDoorsNo.Text);
				}
				if (txtCurbsideDoorsNo.Text != "")
				{
					parms[24] = Convert.ToInt32(txtCurbsideDoorsNo.Text);
				}
				parms[25] = txtRearDoor.Text;
				parms[26] = txtRamp.Text;
				parms[27] = txtReeferUnit.Text;
				parms[28] = txtReeferSerialNo.Text;
				parms[29] = txtElectricStandby.Text;
				parms[30] = txtLinehaulEquipment.Text;
				parms[31] = txtCIPNo.Text;
				parms[32] = txtAssetNo.Text;
				parms[33] = txtTitleNo.Text;
				parms[34] = txtTitleState.Text;
				parms[35] = txtIRPBase.Text;
				parms[36] = txtPlateNo.Text;
				parms[37] = txtStateRegistered.Text;
				if (txtRegistrationExpires.Text != "")
				{
					parms[38] = Convert.ToDateTime(txtRegistrationExpires.Text).ToShortDateString();
				}
				parms[39] = txtPermitsRequired.Text;
				if (txtPermitExpiration.Text != "")
				{
					parms[40] = Convert.ToDateTime(txtPermitExpiration.Text).ToShortDateString();
				}
				parms[41] = txtNotes.Text;
				parms[42] = UserName;
				NullEmptyStrings(parms);
				gATMDB.RunNonQuery("up_p_AddATMRentalVehicle", parms);
				this.Javascript.Notify("Vehicle added to ATM successfully");
				ResetForm();
			}
			else
			{
				this.Javascript.Notify("Vehicle is already present in ATM!");
				ResetForm();
			}
		}
		catch (Exception exp)
		{
			throw new Exception("Error adding Vehicle to ATM import exception", exp);
		}
	}

	private bool IsVehicleExist(string vehicleName, int center)
	{
		bool exist = Convert.ToBoolean(gATMDB.GetScalar("up_p_IsVehicleExist", vehicleName, center));
		return exist;
	}

	/// <summary>
	///  This is to replace empty string values to null in parms
	/// </summary>
	/// <param name="parms">Param for replacing empty string to null</param>
	private void NullEmptyStrings(object[] parms)
	{
		for (int i = 0; i < parms.Length; i++)
		{
			if (parms[i] != null && parms[i].ToString() == "")
				parms[i] = null;
		}
	}

	public void ResetForm()
	{
		ddSygmaCenterNo.ClearSelection();
		txtRentalVehicleName.Text = null;
		txtUnitNo.Text = null;
		ddMake.ClearSelection();
		txtVIN.Text = null;
		ddReplacedYear.ClearSelection();
		txtModel.Text = null;
		ddType.ClearSelection();
		ddStatus.ClearSelection();
		ddYear.ClearSelection();
		txtDescription.Text = null;
		ddObjType.ClearSelection();
		chkActive.Checked = false;
		txtInServiceDate.Text = null;
		txtDeprStartDate.Text = null;
		txtDeprEndDate.Text = null;
		txtDeprMonths.Text = null;
		txtCost.Text = null;
		txtMonthlyDepreciation.Text = null;
		txtLiftgate.Text = null;
		txtRentalVehicleHeightFeet.Text = null;
		txtRentalVehicleHeightInch.Text = null;
		txtRentalVehicleLengthFeet.Text = null;
		txtRentalVehicleLengthInch.Text = null;
		txtRentalVehicleWidthFeet.Text = null;
		txtRentalVehicleWidthInch.Text = null;
		txtRoadsideDoorsNo.Text = null;
		txtCurbsideDoorsNo.Text = null;
		txtRearDoor.Text = null;
		txtRamp.Text = null;
		txtReeferUnit.Text = null;
		txtReeferSerialNo.Text = null;
		txtElectricStandby.Text = null;
		txtLinehaulEquipment.Text = null;
		txtCIPNo.Text = null;
		txtAssetNo.Text = null;
		txtTitleNo.Text = null;
		txtTitleState.Text = null;
		txtIRPBase.Text = null;
		txtPlateNo.Text = null;
		txtStateRegistered.Text = null;
		txtRegistrationExpires.Text = null;
		txtPermitsRequired.Text = null;
		txtPermitExpiration.Text = null;
		txtNotes.Text = null;
	}
}