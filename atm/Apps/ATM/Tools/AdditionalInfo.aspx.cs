using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Globalization;
using atm;

public partial class Apps_ATM_Tools_AdditionalInfo : ATMPage
{
	private string gVehicleTrailerId;
	private bool gIsVehicle = false;
	protected override void LoadATMPage()
	{
		SetPageVariables();
	}

	private void SetPageVariables()
	{
		AddClientVariable("gUserName", UserName);
		if (!string.IsNullOrEmpty(Request.QueryString.Get("vid")) && !string.IsNullOrEmpty(Request.QueryString.Get("isv")))
		{
			gVehicleTrailerId = Request.QueryString.Get("vid").ToString();
			gIsVehicle = Convert.ToBoolean(Request.QueryString.Get("isv"));
			lblVTName.Text = gIsVehicle ? "Vehicle Name:" : "Trailer Name:";
			lblTrailerHeight.Text = gIsVehicle ? "Vehicle Height:" : "Trailer Height:";
			lblTrailerLength.Text = gIsVehicle ? "Vehicle Length:" : "Trailer Length:";
			lblTrailerWidth.Text = gIsVehicle ? "Vehicle Width:" : "Trailer Width:";
			SetVehicleTrailerAdditionalInfo(gVehicleTrailerId, gIsVehicle);
			AddAttributesToFields();
		}
		else
		{
			pnlAdditionalInfo.Visible = false;
			lblNoRecordExist.Text = "No Record exist!";
			lblNoRecordExist.Visible = true;
		}
	}

	private void SetVehicleTrailerAdditionalInfo(string gVId, bool gIsVehicle)
	{
		DataSet dsVehicleTrailerAdditionalInfo = ATMDB.GetDataSet("up_p_getVehicleTrailerAdditionalInfo", gVId, gIsVehicle);
		if (dsVehicleTrailerAdditionalInfo.Tables.Count > 0)
		{
			DataView dvVehicleTrailerAdditionalInfo = new DataView(dsVehicleTrailerAdditionalInfo.Tables[0]);
			DataView dvVehicleTrailerName = new DataView(dsVehicleTrailerAdditionalInfo.Tables[1]);
			lblVehicleTrailerName.Text = dvVehicleTrailerName[0]["VehicleName"].ToString();
			if (dvVehicleTrailerAdditionalInfo.Count > 0)
			{
				txtLiftgate.Text = dvVehicleTrailerAdditionalInfo[0]["Liftgate"].ToString();
				if (!Convert.IsDBNull(dvVehicleTrailerAdditionalInfo[0]["TrailerHeight"]))
				{
					txtTrailerHeightFeet.Text = (Convert.ToInt32(dvVehicleTrailerAdditionalInfo[0]["TrailerHeight"]) / 12).ToString();
					txtTrailerHeightInch.Text = (Convert.ToInt32(dvVehicleTrailerAdditionalInfo[0]["TrailerHeight"]) % 12).ToString();
				}
				if (!Convert.IsDBNull(dvVehicleTrailerAdditionalInfo[0]["TrailerLength"]))
				{
					txtTrailerLengthFeet.Text = (Convert.ToInt32(dvVehicleTrailerAdditionalInfo[0]["TrailerLength"]) / 12).ToString();
					txtTrailerLengthInch.Text = (Convert.ToInt32(dvVehicleTrailerAdditionalInfo[0]["TrailerLength"]) % 12).ToString();
				}
				if (!Convert.IsDBNull(dvVehicleTrailerAdditionalInfo[0]["TrailerWidth"]))
				{
					txtTrailerWidthFeet.Text = (Convert.ToInt32(dvVehicleTrailerAdditionalInfo[0]["TrailerWidth"]) / 12).ToString();
					txtTrailerWidthInch.Text = (Convert.ToInt32(dvVehicleTrailerAdditionalInfo[0]["TrailerWidth"]) % 12).ToString();
				}
				txtRoadsideDoorsNo.Text = dvVehicleTrailerAdditionalInfo[0]["RoadsideDoorsNo"].ToString();
				txtCurbsideDoorsNo.Text = dvVehicleTrailerAdditionalInfo[0]["CurbsideDoorsNo"].ToString();
				txtRearDoor.Text = dvVehicleTrailerAdditionalInfo[0]["RearDoor"].ToString();
				txtRamp.Text = dvVehicleTrailerAdditionalInfo[0]["Ramp"].ToString();
				txtReeferUnit.Text = dvVehicleTrailerAdditionalInfo[0]["ReeferUnit"].ToString();
				txtReeferSerialNo.Text = dvVehicleTrailerAdditionalInfo[0]["ReeferSerialNo"].ToString();
				txtElectricStandby.Text = dvVehicleTrailerAdditionalInfo[0]["ElectricStandby"].ToString();
				txtLinehaulEquipment.Text = dvVehicleTrailerAdditionalInfo[0]["LinehaulEquipment"].ToString();
				txtCIPNo.Text = dvVehicleTrailerAdditionalInfo[0]["CIPNo"].ToString();
				txtAssetNo.Text = dvVehicleTrailerAdditionalInfo[0]["AssetNo"].ToString();
				txtTitleNo.Text = dvVehicleTrailerAdditionalInfo[0]["TitleNo"].ToString();
				txtTitleState.Text = dvVehicleTrailerAdditionalInfo[0]["TitleState"].ToString();
				txtIRPBase.Text = dvVehicleTrailerAdditionalInfo[0]["IRPBase"].ToString();
				txtPlateNo.Text = dvVehicleTrailerAdditionalInfo[0]["PlateNo"].ToString();
				txtStateRegistered.Text = dvVehicleTrailerAdditionalInfo[0]["StateRegistered"].ToString();
				if (!Convert.IsDBNull(dvVehicleTrailerAdditionalInfo[0]["RegistrationExpires"]))
				{
					txtRegistrationExpires.Text = Convert.ToDateTime(dvVehicleTrailerAdditionalInfo[0]["RegistrationExpires"]).ToShortDateString();
				}
				txtPermitsRequired.Text = dvVehicleTrailerAdditionalInfo[0]["PermitsRequired"].ToString();
				if (!Convert.IsDBNull(dvVehicleTrailerAdditionalInfo[0]["PermitExpiration"]))
				{
					txtPermitExpiration.Text = Convert.ToDateTime(dvVehicleTrailerAdditionalInfo[0]["PermitExpiration"]).ToShortDateString();
				}
				txtNotes.Text = dvVehicleTrailerAdditionalInfo[0]["Notes"].ToString();
			}
		}
		else
		{
			pnlAdditionalInfo.Visible = false;
			lblNoRecordExist.Text = "No Record exist!";
			lblNoRecordExist.Visible = true;
		}
	}

	private void AddAttributesToFields()
	{
		//txtTrailerHeightFeet.Attributes.Add
		txtTrailerHeightFeet.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 1, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtTrailerHeightInch.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 2, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtTrailerLengthFeet.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 3, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtTrailerLengthInch.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 4, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtTrailerWidthFeet.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 5, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtTrailerWidthInch.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 6, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtRoadsideDoorsNo.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 7, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtCurbsideDoorsNo.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerHeightLengthWidthDoors(this, 8, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));

		txtRegistrationExpires.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerRegistrationPermitExpireDate(this, 1, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtPermitExpiration.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerRegistrationPermitExpireDate(this, 2, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));

		txtLiftgate.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 1, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtRearDoor.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 2, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtRamp.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 3, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtReeferUnit.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 4, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtReeferSerialNo.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 5, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtElectricStandby.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 6, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtLinehaulEquipment.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 7, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtCIPNo.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 8, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtAssetNo.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 9, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtTitleNo.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 10, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtTitleState.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 11, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtIRPBase.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 12, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtPlateNo.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 13, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtStateRegistered.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 14, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtPermitsRequired.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 15, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
		txtNotes.Attributes.Add("onchange", string.Format("UpdateVehicleTrailerAdditionalInfo(this, 16, {0}, {1});", gVehicleTrailerId, Convert.ToInt32(gIsVehicle)));
	}

	[WebMethod]
	public static void PM_SaveVehicleTrailerHeightLengthWidthDoors(string userName, string vid, int fieldId, string value, int isVehicle)
	{
		uint val;
		if (UInt32.TryParse(value, out val))
		{
			RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerHeightLengthWidthDoors", userName, vid, fieldId, val, isVehicle);
		}
		else
		{
			if ((fieldId != 7 || fieldId != 8) && value == "")
			{
				RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerHeightLengthWidthDoors", userName, vid, fieldId, 0, isVehicle);
			}
		}
	}

	[WebMethod]
	public static void PM_SaveVehicleTrailerRegistrationPermitExpireDate(string userName, string vid, int fieldId, string value, int isVehicle)
	{
		if (value != "")
		{
			DateTime date;
			if (DateTime.TryParseExact(value, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
			{
				RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerRegistrationPermitExpireDate2", userName, vid, fieldId, date.ToShortDateString(), isVehicle);
			}
		}
		else
		{
			RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerRegistrationPermitExpireDate2", userName, vid, fieldId, value, isVehicle);
		}
	}

	[WebMethod]
	public static void PM_SaveVehicleTrailerAdditionalInfo(string userName, string vid, int fieldId, string value, int isVehicle)
	{
		ValidateValueBasedOnFieldId(fieldId, value);

		RunNonQueryFromStatic("ATM", "up_p_setVehicleTrailerAdditionalInfo", userName, vid, fieldId, value, isVehicle);
	}

	private static void ValidateValueBasedOnFieldId(int fieldId, string value)
	{/* sproc handles all updating based on fieldId where
		 01 = Liftgate, 
		 02 = RearDoor, 
		 03 = Ramp, 
		 04 = ReeferUnit, 
		 05 = ReeferSerialNo, 
		 06 = ElectricStandby,
		 07 = LinehaulEquipment,
		 08 = CIPNo,
		 09 = AssetNo,
		 10 = TitleNo,
		 11 = TitleState
		 12 = IRPBase,
		 13 = PlateNo,
		 14 = StateRegistered,
		 15 = PermitsRequired,
		 16 = Notes
		 */
		if (fieldId == 1 && value.Length > 5)
			throw new ArgumentException("Liftgate must be less than or equal to five characters in length.");

		return;
	}
}