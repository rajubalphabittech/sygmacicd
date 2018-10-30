<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Tools_AddVehicle" CodeBehind="AddVehicle.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>
		<asp:Literal ID="litPageTitle" runat="server"></asp:Literal>
	</title>
	<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<div class="pageTitle" style="width: 100%">
			<asp:Literal ID="litTitle" runat="server"></asp:Literal>
		</div>
		<asp:ValidationSummary ID="vsAddRentalVehicle" runat="server"
			ValidationGroup="Add" />
		<div style="margin-top: 10px;">
			<div class="inputPanel inlineBlock" style="height: auto; width: 450px; vertical-align: top">
				<div class="header">
					<asp:Literal ID="litPanelHeader" runat="server"></asp:Literal>
				</div>
				<div class="body">
					<div id="Center" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Center:<span style="color: red">*</span>
						</div>
						<asp:DropDownList ID="ddSygmaCenterNo" runat="server" DataTextField="Center"
							DataValueField="SygmaCenterNo" Width="155px">
						</asp:DropDownList>
						<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvSygmaCenter" runat="server" ControlToValidate="ddSygmaCenterNo"
							ValidationGroup="Add" InitialValue="0" ErrorMessage="'Center' is required !">*</asp:RequiredFieldValidator>
					</div>
					<div id="RentalVehicleName" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							<asp:Literal ID="litNameLabel" runat="server"></asp:Literal>
							<span style="color: red">*</span>
						</div>
						<asp:Literal ID="litRentalIndicator" runat="server"></asp:Literal>
						<asp:TextBox ID="txtRentalVehicleName" runat="server" Width="142px" MaxLength="49" autocomplete="off"></asp:TextBox>
						<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvRentalVehicleName" runat="server" ControlToValidate="txtRentalVehicleName"
							ValidationGroup="Add" ErrorMessage="'Rental Vehicle Name' is required!">*</asp:RequiredFieldValidator>
					</div>
					<div id="UnitNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Unit No:
						</div>
						<asp:TextBox ID="txtUnitNo" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<asp:CompareValidator CssClass="validator-message" ID="cvUnitNo" runat="server" ControlToValidate="txtUnitNo" Display="dynamic" ValidationGroup="Add"
							Operator="DataTypeCheck" Type="Integer" ErrorMessage="Enter whole number for 'Unit No'!" ForeColor="Red">*
						</asp:CompareValidator>
					</div>
					<div id="Make" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Make:
						</div>
						<asp:DropDownList ID="ddMake" runat="server" DataTextField="VehicleMake" DataValueField="MakeId" Style="text-align: left" Width="155px"></asp:DropDownList>
					</div>
					<div id="VIN" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							VIN:
						</div>
						<asp:TextBox ID="txtVIN" runat="server" Width="150px" MaxLength="50" autocomplete="off"></asp:TextBox>
					</div>
					<div id="ReplacedYear" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Replaced Year:
						</div>
						<asp:DropDownList ID="ddReplacedYear" runat="server" DataTextField="ReplacedYear" DataValueField="ReplacedId" Style="text-align: left" Width="155px"></asp:DropDownList>
					</div>
					<div id="Model" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Model:
						</div>
						<asp:TextBox ID="txtModel" runat="server" Width="150px" MaxLength="50" autocomplete="off"></asp:TextBox>
					</div>
					<div id="CabType" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Cab Type:
						</div>
						<asp:DropDownList ID="ddType" runat="server" DataTextField="VehicleType" DataValueField="TypeId" Style="text-align: left" Width="155px"></asp:DropDownList>
					</div>
					<div id="Status" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Status:
						</div>
						<asp:DropDownList ID="ddStatus" runat="server" DataTextField="VehicleStatus" DataValueField="StatusId" Style="text-align: left" Width="155px"></asp:DropDownList>
					</div>
					<div id="Year" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Year:
						</div>
						<asp:DropDownList ID="ddYear" runat="server" DataTextField="Year" DataValueField="Id" Style="text-align: left" Width="155px"></asp:DropDownList>
					</div>
					<div id="Description" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Description:
						</div>
						<asp:TextBox ID="txtDescription" runat="server" Width="150px" MaxLength="200" autocomplete="off"></asp:TextBox>
					</div>
					<div id="ObjectType" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Object Type:
						</div>
						<asp:DropDownList ID="ddObjType" runat="server" DataTextField="VehicleObjectType" DataValueField="ObjectTypeId" Style="text-align: left" Width="155px"></asp:DropDownList>
					</div>
					<div id="Active" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Active:
						</div>
						<asp:CheckBox ID="chkActive" runat="server" Enabled="true"></asp:CheckBox>
					</div>
					<div id="InServiceDate" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							In-Service Date:
						</div>
						<asp:TextBox ID="txtInServiceDate" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<cc1:CalendarExtender ID="txtInServiceDate_CalendarExtender" runat="server" Enabled="True"
							TargetControlID="txtInServiceDate" Format="M/d/yyyy" PopupPosition="Right">
						</cc1:CalendarExtender>
						<asp:RangeValidator CssClass="validator-message" ID="rngInServiceDate" runat="server" ControlToValidate="txtInServiceDate" ValidationGroup="Add"
							MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date" ErrorMessage="Invalid 'In-Service Date'!"
							EnableClientScript="True" Enabled="True">*</asp:RangeValidator>
					</div>
					<div id="DeprStartDate" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Depr. Start Date:
						</div>
						<asp:TextBox ID="txtDeprStartDate" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<cc1:CalendarExtender ID="txtDeprStartDate_CalendarExtender" runat="server" Enabled="True"
							TargetControlID="txtDeprStartDate" Format="M/d/yyyy" PopupPosition="Right">
						</cc1:CalendarExtender>
						<asp:RangeValidator CssClass="validator-message" ID="rngDeprStartDate" runat="server" ControlToValidate="txtDeprStartDate" ValidationGroup="Add"
							MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date" ErrorMessage="Invalid 'Depr. Start Date'!"
							EnableClientScript="True" Enabled="True">*</asp:RangeValidator>
					</div>
					<div id="DeprEndDate" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Depr. End Date:
						</div>
						<asp:TextBox ID="txtDeprEndDate" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<cc1:CalendarExtender ID="txtDeprEndDate_CalendarExtender" runat="server" Enabled="True"
							TargetControlID="txtDeprEndDate" Format="M/d/yyyy" PopupPosition="Right">
						</cc1:CalendarExtender>
						<asp:RangeValidator CssClass="validator-message" ID="rngDeprEndDate" runat="server" ControlToValidate="txtDeprEndDate" ValidationGroup="Add"
							MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date" ErrorMessage="Invalid 'Depr. End Date'!"
							EnableClientScript="True" Enabled="True">*</asp:RangeValidator>&nbsp;
                        <asp:CompareValidator CssClass="validator-message" ID="cvDeprEndDate" runat="server" ControlToCompare="txtDeprStartDate" ValidationGroup="Add"
													ControlToValidate="txtDeprEndDate" Operator="GreaterThanEqual" Display="dynamic" ErrorMessage="'Depr. End date' cannot less than 'Depr. Start date'!"
													Type="Date">*</asp:CompareValidator>
					</div>
					<div id="DeprMonths" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Depr. Months:
						</div>
						<asp:TextBox ID="txtDeprMonths" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<asp:CompareValidator CssClass="validator-message" ID="cvDeprMonths" runat="server" ControlToValidate="txtDeprMonths" Display="dynamic" ValidationGroup="Add"
							Operator="DataTypeCheck" Type="Integer" ErrorMessage="Enter whole number for 'Depr. Months'!" ForeColor="Red">*
						</asp:CompareValidator>
					</div>
					<div id="Cost" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Cost:
						</div>
						<asp:TextBox ID="txtCost" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<asp:CompareValidator CssClass="validator-message" ID="cvCost" runat="server" ControlToValidate="txtCost" Display="dynamic" ValidationGroup="Add"
							Operator="DataTypeCheck" Type="Double" ErrorMessage="Enter correct value for 'Cost'!" ForeColor="Red">*
						</asp:CompareValidator>
					</div>
					<div id="MonthlyDepreciation" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Monthly Depreciation:
						</div>
						<asp:TextBox ID="txtMonthlyDepreciation" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<asp:CompareValidator CssClass="validator-message" ID="cvMonthlyDepreciation" runat="server" ControlToValidate="txtMonthlyDepreciation" Display="dynamic" ValidationGroup="Add"
							Operator="DataTypeCheck" Type="Double" ErrorMessage="Enter correct value for 'Monthly Deprieciation'!" ForeColor="Red">*
						</asp:CompareValidator>
					</div>
					<div id="Liftgate" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Liftgate:
						</div>
						<asp:TextBox ID="txtLiftgate" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="RentalVehicleHeight" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Vehicle Height:
						</div>
						Feet&nbsp;<asp:TextBox ID="txtRentalVehicleHeightFeet" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
						Inches&nbsp;<asp:TextBox ID="txtRentalVehicleHeightInch" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
						<asp:RangeValidator CssClass="validator-message" ID="rngRentalVehicleHeightFeet" runat="server" ControlToValidate="txtRentalVehicleHeightFeet" MaximumValue="999"
							MinimumValue="0" Type="Integer" ErrorMessage="'RentalVehicle Height Feet' must be a whole number between 0 and 999!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
						<asp:RangeValidator CssClass="validator-message" ID="rngRentalVehicleHeightInch" runat="server" ControlToValidate="txtRentalVehicleHeightInch" MaximumValue="12"
							MinimumValue="0" Type="Integer" ErrorMessage="'RentalVehicle Height Inches' must be a whole number between 0 and 12!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
					</div>
					<div id="RentalVehicleLength" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Vehicle Length:
						</div>
						Feet&nbsp;<asp:TextBox ID="txtRentalVehicleLengthFeet" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
						Inches&nbsp;<asp:TextBox ID="txtRentalVehicleLengthInch" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
						<asp:RangeValidator CssClass="validator-message" ID="rngRentalVehicleLengthFeet" runat="server" ControlToValidate="txtRentalVehicleLengthFeet" MaximumValue="999"
							MinimumValue="0" Type="Integer" ErrorMessage="'RentalVehicle Length Feet' must be a whole number between 0 and 999!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
						<asp:RangeValidator CssClass="validator-message" ID="rngRentalVehicleLengthInch" runat="server" ControlToValidate="txtRentalVehicleLengthInch" MaximumValue="12"
							MinimumValue="0" Type="Integer" ErrorMessage="'RentalVehicle Length Inches' must be a whole number between 0 and 12!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
					</div>
					<div id="RentalVehicleWidth" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Vehicle Width:
						</div>
						Feet&nbsp;<asp:TextBox ID="txtRentalVehicleWidthFeet" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
						Inches&nbsp;<asp:TextBox ID="txtRentalVehicleWidthInch" runat="server" Width="36px" autocomplete="off"></asp:TextBox>
						<asp:RangeValidator CssClass="validator-message" ID="rngRentalVehicleWidthFeet" runat="server" ControlToValidate="txtRentalVehicleWidthFeet" MaximumValue="999"
							MinimumValue="0" Type="Integer" ErrorMessage="'RentalVehicle Width Feet' must be a whole number between 0 and 999!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
						<asp:RangeValidator CssClass="validator-message" ID="rngRentalVehicleWidthInch" runat="server" ControlToValidate="txtRentalVehicleWidthInch" MaximumValue="12"
							MinimumValue="0" Type="Integer" ErrorMessage="'RentalVehicle Width Inches' must be a whole number between 0 and 12!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
					</div>
					<div id="RoadsideDoorsNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							# Roadside Doors:
						</div>
						<asp:TextBox ID="txtRoadsideDoorsNo" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<asp:RangeValidator CssClass="validator-message" ID="rngRoadsideDoorsNo" runat="server" ControlToValidate="txtRoadsideDoorsNo" MaximumValue="50"
							MinimumValue="0" Type="Integer" ErrorMessage="'Roadside Doors No' must be a whole number between 0 and 50!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
					</div>
					<div id="CurbsideDoorsNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							# Curbside Doors:
						</div>
						<asp:TextBox ID="txtCurbsideDoorsNo" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<asp:RangeValidator CssClass="validator-message" ID="rngCurbsideDoorsNo" runat="server" ControlToValidate="txtCurbsideDoorsNo" MaximumValue="50"
							MinimumValue="0" Type="Integer" ErrorMessage="'Curbside Doors No' must be a whole number between 0 and 50!"
							ValidationGroup="Add" ForeColor="Red" Display="Dynamic">*
						</asp:RangeValidator>
					</div>
					<div id="RearDoor" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Rear Door:
						</div>
						<asp:TextBox ID="txtRearDoor" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="Ramp" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Ramp:
						</div>
						<asp:TextBox ID="txtRamp" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="ReeferUnit" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Reefer Unit:
						</div>
						<asp:TextBox ID="txtReeferUnit" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="ReeferSerialNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Reefer Serial #:
						</div>
						<asp:TextBox ID="txtReeferSerialNo" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="ElectricStandby" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Electric Standby:
						</div>
						<asp:TextBox ID="txtElectricStandby" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="LinehaulEquipment" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Linehaul Equipment:
						</div>
						<asp:TextBox ID="txtLinehaulEquipment" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="CIPNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							CIP #:
						</div>
						<asp:TextBox ID="txtCIPNo" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="AssetNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Asset #:
						</div>
						<asp:TextBox ID="txtAssetNo" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="TitleNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Title #:
						</div>
						<asp:TextBox ID="txtTitleNo" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="TitleState" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Title State:
						</div>
						<asp:TextBox ID="txtTitleState" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="IRPBase" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							IRP/Base:
						</div>
						<asp:TextBox ID="txtIRPBase" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="PlateNo" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Plate No:
						</div>
						<asp:TextBox ID="txtPlateNo" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="StateRegistered" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							State Registered:
						</div>
						<asp:TextBox ID="txtStateRegistered" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="RegistrationExpires" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Registration Expires:
						</div>
						<asp:TextBox ID="txtRegistrationExpires" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<cc1:CalendarExtender ID="txtRegistrationExpires_CalendarExtender" runat="server" Enabled="True"
							TargetControlID="txtRegistrationExpires" Format="M/d/yyyy" PopupPosition="Right">
						</cc1:CalendarExtender>
						<asp:RangeValidator CssClass="validator-message" ID="rngRegistrationExpires" runat="server" ControlToValidate="txtRegistrationExpires"
							MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date" ErrorMessage="Invalid 'Registration Expires' Date!"
							ValidationGroup="Add" EnableClientScript="True" Enabled="True">*</asp:RangeValidator>
					</div>
					<div id="PermitsRequired" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Permits Required:
						</div>
						<asp:TextBox ID="txtPermitsRequired" runat="server" MaxLength="20" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div id="PermitExpiration" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Permit Expiration:
						</div>
						<asp:TextBox ID="txtPermitExpiration" runat="server" Width="150px" autocomplete="off"></asp:TextBox>
						<cc1:CalendarExtender ID="txtPermitExpiration_CalendarExtender" runat="server" Enabled="True"
							TargetControlID="txtPermitExpiration" Format="M/d/yyyy" PopupPosition="Right">
						</cc1:CalendarExtender>
						<asp:RangeValidator CssClass="validator-message" ID="rngPermitExpiration" runat="server" ControlToValidate="txtPermitExpiration"
							MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date" ErrorMessage="Invalid 'Permit Expiration' Date!"
							ValidationGroup="Add" EnableClientScript="True" Enabled="True">*</asp:RangeValidator>
					</div>
					<div id="Notes" style="margin-bottom: 5px">
						<div class="inlineBlock" style="font-weight: bold; width: 150px">
							Notes:
						</div>
						<asp:TextBox ID="txtNotes" runat="server" MaxLength="200" Width="150px" autocomplete="off"></asp:TextBox>
					</div>
					<div style="margin-bottom: 5px">
						<asp:Button ID="btnAdd" runat="server" Text="Add Vehicle" ValidationGroup="Add" OnClick="btnAdd_Click" />
					</div>
					<div style="margin-bottom: 5px">
						<asp:Label ID="lblValidation" runat="server" Text="* - Fields are mandatory!" ForeColor="Red"></asp:Label>
					</div>
				</div>
			</div>
		</div>
	</form>
</body>
</html>
