<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Tools_AdditionalInfo" CodeBehind="AdditionalInfo.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ OutputCache Location="None" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Additional Info</title>
	<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.hotkeys-0.7.9.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/AJAX.js" type="text/javascript"></script>
	<script src="/Scripts/ATMForm.js" type="text/javascript"></script>
	<script src="/Scripts/json2.js" type="text/javascript"></script>
	<script type="text/javascript">

		var thePath = location.pathname;

		function UpdateVehicleTrailerHeightLengthWidthDoors(c, fieldId, vid, isVehicle) {
			var val;
			var valFeet = 0;
			var valInch = 0;
			if (fieldId == 1) {
				valFeet = c.value;
				valInch = document.getElementById('txtTrailerHeightInch').value;
				if (valFeet >= 0 && valInch >= 0 && valInch < 13 && valFeet < 1000) {
					val = +(valFeet * 12) + +valInch;
				}
				else {
					val = -1;
				}
			}
			else if (fieldId == 2) {
				valFeet = document.getElementById('txtTrailerHeightFeet').value;
				valInch = c.value;
				if (valFeet >= 0 && valInch >= 0 && valInch < 13 && valFeet < 1000) {
					val = +(valFeet * 12) + +valInch;
				}
				else {
					val = -1;
				}
			}
			else if (fieldId == 3) {
				valFeet = c.value;
				valInch = document.getElementById('txtTrailerLengthInch').value;
				if (valFeet >= 0 && valInch >= 0 && valInch < 13 && valFeet < 1000) {
					val = +(valFeet * 12) + +valInch;
				}
				else {
					val = -1;
				}
			}
			else if (fieldId == 4) {
				valFeet = document.getElementById('txtTrailerLengthFeet').value;
				valInch = c.value;
				if (valFeet >= 0 && valInch >= 0 && valInch < 13 && valFeet < 1000) {
					val = +(valFeet * 12) + +valInch;
				}
				else {
					val = -1;
				}
			}
			else if (fieldId == 5) {
				valFeet = c.value;
				valInch = document.getElementById('txtTrailerWidthInch').value;
				if (valFeet >= 0 && valInch >= 0 && valInch < 13 && valFeet < 1000) {
					val = +(valFeet * 12) + +valInch;
				}
				else {
					val = -1;
				}
			}
			else if (fieldId == 6) {
				valFeet = document.getElementById('txtTrailerWidthFeet').value;
				valInch = c.value;
				if (valFeet >= 0 && valInch >= 0 && valInch < 13) {
					val = +(valFeet * 12) + +valInch;
				}
				else {
					val = -1;
				}
			}
			else if (fieldId == 7) {
				val = c.value;
				if (val > 50) {
					val = -1;
				}
			}
			else if (fieldId == 8) {
				val = c.value;
				if (val > 50) {
					val = -1;
				}
			}
			if (val > 12000) {
				val = -1;
			}
			PageMethods.set_path(thePath);
			PageMethods.PM_SaveVehicleTrailerHeightLengthWidthDoors(gUserName, vid, fieldId, val, isVehicle, onChangeSuccess, onFailed);
		}

		function UpdateVehicleTrailerRegistrationPermitExpireDate(c, fieldId, vid, isVehicle) {
			var val = c.value;
			PageMethods.set_path(thePath);
			PageMethods.PM_SaveVehicleTrailerRegistrationPermitExpireDate(gUserName, vid, fieldId, val, isVehicle, onChangeSuccess, onFailed);
		}

		function UpdateVehicleTrailerAdditionalInfo(c, fieldId, vid, isVehicle) {
			var val = c.value;
			PageMethods.set_path(thePath);
			PageMethods.PM_SaveVehicleTrailerAdditionalInfo(gUserName, vid, fieldId, val, isVehicle, onChangeSuccess, onFailed);
		}

		function onChangeSuccess() {
		}

		function onFailed(error) {
			alert('There was an error saving the field.  Please refresh the page and try again.');
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<div class="inputPanel" style="width: 400px;">
			<asp:Panel ID="pnlAdditionalInfo" runat="server">
				<asp:Table ID="tblAdditionalInfo" runat="server">
					<asp:TableRow ID="trVehicleTrailerName" runat="server">
						<asp:TableCell ID="tcVehicleTrailerName" runat="server">
							<asp:Label ID="lblVTName" runat="server" Text="Vehicle/Trailer Name:" Width="150px"></asp:Label>
							<asp:Label ID="lblVehicleTrailerName" runat="server"></asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trLiftgate" runat="server">
						<asp:TableCell ID="tcLiftgate" runat="server">
							<asp:Label ID="lblLiftgate" runat="server" Text="Liftgate:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtLiftgate" runat="server" MaxLength="5" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trTrailerHeight" runat="server">
						<asp:TableCell ID="tcTrailerHeight" runat="server">
							<asp:Label ID="lblTrailerHeight" runat="server" Text="" Width="150px"></asp:Label>
							Feet&nbsp;<asp:TextBox ID="txtTrailerHeightFeet" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
							Inches&nbsp;<asp:TextBox ID="txtTrailerHeightInch" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
							<asp:RangeValidator CssClass="validator-message" ID="rngTrailerHeightFeet" runat="server" ControlToValidate="txtTrailerHeightFeet" MaximumValue="999"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Feet must be a whole number between 0 and 999!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
							<asp:RangeValidator CssClass="validator-message" ID="rngTrailerHeightInch" runat="server" ControlToValidate="txtTrailerHeightInch" MaximumValue="12"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Inches must be a whole number between 0 and 12!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trTrailerLength" runat="server">
						<asp:TableCell ID="tcTrailerLength" runat="server">
							<asp:Label ID="lblTrailerLength" runat="server" Text="" Width="150px"></asp:Label>
							Feet&nbsp;<asp:TextBox ID="txtTrailerLengthFeet" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
							Inches&nbsp;<asp:TextBox ID="txtTrailerLengthInch" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
							<asp:RangeValidator CssClass="validator-message" ID="rngTrailerLengthFeet" runat="server" ControlToValidate="txtTrailerLengthFeet" MaximumValue="999"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Feet must be a whole number between 0 and 999!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
							<asp:RangeValidator CssClass="validator-message" ID="rngTrailerLengthInch" runat="server" ControlToValidate="txtTrailerLengthInch" MaximumValue="12"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Inches must be a whole number between 0 and 12!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trTrailerWidth" runat="server">
						<asp:TableCell ID="tcTrailerWidth" runat="server">
							<asp:Label ID="lblTrailerWidth" runat="server" Text="" Width="150px"></asp:Label>
							Feet&nbsp;<asp:TextBox ID="txtTrailerWidthFeet" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
							Inches&nbsp;<asp:TextBox ID="txtTrailerWidthInch" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
							<asp:RangeValidator CssClass="validator-message" ID="rngTrailerWidthFeet" runat="server" ControlToValidate="txtTrailerWidthFeet" MaximumValue="999"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Feet must be a whole number between 0 and 999!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
							<asp:RangeValidator CssClass="validator-message" ID="rngTrailerWidthInch" runat="server" ControlToValidate="txtTrailerWidthInch" MaximumValue="12"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Inches must be a whole number between 0 and 12!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trRoadsideDoorsNo" runat="server">
						<asp:TableCell ID="tcRoadsideDoorsNo" runat="server">
							<asp:Label ID="lblRoadsideDoorsNo" runat="server" Text="# Roadside Doors:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtRoadsideDoorsNo" runat="server" autocomplete="off"></asp:TextBox>
							<asp:RangeValidator CssClass="validator-message" ID="rngRoadsideDoorsNo" runat="server" ControlToValidate="txtRoadsideDoorsNo" MaximumValue="50"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Value must be a whole number between 0 and 50!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trCurbsideDoorsNo" runat="server">
						<asp:TableCell ID="tcCurbsideDoorsNo" runat="server">
							<asp:Label ID="lblCurbsideDoorsNo" runat="server" Text="# Curbside Doors:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtCurbsideDoorsNo" runat="server" autocomplete="off"></asp:TextBox>
							<asp:RangeValidator CssClass="validator-message" ID="rngCurbsideDoorsNo" runat="server" ControlToValidate="txtCurbsideDoorsNo" MaximumValue="50"
								MinimumValue="0" Type="Integer" SetFocusOnError="true" ErrorMessage="Value must be a whole number between 0 and 50!"
								ForeColor="Red" Display="Dynamic">
							</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trRearDoor" runat="server">
						<asp:TableCell ID="tcRearDoor" runat="server">
							<asp:Label ID="lblRearDoor" runat="server" Text="Rear Door:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtRearDoor" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trRamp" runat="server">
						<asp:TableCell ID="tcRamp" runat="server">
							<asp:Label ID="lblRamp" runat="server" Text="Ramp:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtRamp" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trReeferUnit" runat="server">
						<asp:TableCell ID="tcReeferUnit" runat="server">
							<asp:Label ID="lblReeferUnit" runat="server" Text="Reefer Unit:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtReeferUnit" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trReeferSerialNo" runat="server">
						<asp:TableCell ID="tcReeferSerialNo" runat="server">
							<asp:Label ID="lblReeferSerialNo" runat="server" Text="Reefer Serial #:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtReeferSerialNo" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trElectricStandby" runat="server">
						<asp:TableCell ID="tcElectricStandby" runat="server">
							<asp:Label ID="lblElectricStandby" runat="server" Text="Electric Standby:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtElectricStandby" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trLinehaulEquipment" runat="server">
						<asp:TableCell ID="tcLinehaulEquipment" runat="server">
							<asp:Label ID="lblLinehaulEquipment" runat="server" Text="Linehaul Equipment:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtLinehaulEquipment" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trCIPNo" runat="server">
						<asp:TableCell ID="tcCIPNo" runat="server">
							<asp:Label ID="lblCIPNo" runat="server" Text="CIP #:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtCIPNo" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trAssetNo" runat="server">
						<asp:TableCell ID="tcAssetNo" runat="server">
							<asp:Label ID="lblAssetNo" runat="server" Text="Asset #:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtAssetNo" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trTitleNo" runat="server">
						<asp:TableCell ID="tcTitleNo" runat="server">
							<asp:Label ID="lblTitleNo" runat="server" Text="Title #:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtTitleNo" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trTitleState" runat="server">
						<asp:TableCell ID="tcTitleState" runat="server">
							<asp:Label ID="lblTitleState" runat="server" Text="Title State:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtTitleState" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trIRPBase" runat="server">
						<asp:TableCell ID="tcIRPBase" runat="server">
							<asp:Label ID="lblIRPBase" runat="server" Text="IRP/Base:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtIRPBase" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trPlateNo" runat="server">
						<asp:TableCell ID="tcPlateNo" runat="server">
							<asp:Label ID="lblPlateNo" runat="server" Text="Plate No:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtPlateNo" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trStateRegistered" runat="server">
						<asp:TableCell ID="tcStateRegistered" runat="server">
							<asp:Label ID="lblStateRegistered" runat="server" Text="State Registered:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtStateRegistered" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trRegistrationExpires" runat="server">
						<asp:TableCell ID="tcRegistrationExpires" runat="server">
							<asp:Label ID="lblRegistrationExpires" runat="server" Text="Registration Expires:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtRegistrationExpires" runat="server" autocomplete="off"></asp:TextBox>
							<cc1:CalendarExtender ID="txtRegistrationExpires_CalendarExtender" runat="server" Enabled="True"
								TargetControlID="txtRegistrationExpires" Format="M/d/yyyy">
							</cc1:CalendarExtender>
							<asp:RangeValidator CssClass="validator-message" ID="rngRegistrationExpires" runat="server" ControlToValidate="txtRegistrationExpires"
								MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true" Display="dynamic" Type="date"
								EnableClientScript="True" Enabled="True">'Invalid Registration Expires Date!'</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trPermitsRequired" runat="server">
						<asp:TableCell ID="tcPermitsRequired" runat="server">
							<asp:Label ID="lblPermitsRequired" runat="server" Text="Permits Required:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtPermitsRequired" runat="server" MaxLength="20" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trPermitExpiration" runat="server">
						<asp:TableCell ID="tcPermitExpiration" runat="server">
							<asp:Label ID="lblPermitExpiration" runat="server" Text="Permit Expiration:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtPermitExpiration" runat="server" autocomplete="off"></asp:TextBox>
							<cc1:CalendarExtender ID="txtPermitExpiration_CalendarExtender" runat="server" Enabled="True"
								TargetControlID="txtPermitExpiration" Format="M/d/yyyy">
							</cc1:CalendarExtender>
							<asp:RangeValidator CssClass="validator-message" ID="rngPermitExpiration" runat="server" ControlToValidate="txtPermitExpiration"
								MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true" Display="dynamic" Type="date"
								EnableClientScript="True" Enabled="True">'Invalid Permit Expiration Date!'</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trNotes" runat="server">
						<asp:TableCell ID="tcNotes" runat="server">
							<asp:Label ID="lblNotes" runat="server" Text="Notes:" Width="150px"></asp:Label>
							<asp:TextBox ID="txtNotes" runat="server" MaxLength="200" autocomplete="off"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:Panel>
			<asp:Label ID="lblNoRecordExist" runat="server" Text="" Visible="false"></asp:Label>
		</div>
	</form>
</body>
</html>
