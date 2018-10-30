<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Tools_DepreciationInfo" CodeBehind="DepreciationInfo.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ OutputCache Location="None" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Depreciation Info</title>
	<script src="/Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.hotkeys-0.7.9.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/AJAX.js" type="text/javascript"></script>
	<script src="/Scripts/ATMForm.js" type="text/javascript"></script>
	<script src="/Scripts/json2.js" type="text/javascript"></script>
	<script type="text/javascript">
		var thePath = location.pathname;

		function UpdateVehicleTrailerDeprDetails(c, fieldId, vid, isVehicle) {
			if (Page_ClientValidate()) {
				var val = c.value;
				PageMethods.set_path(thePath);
				PageMethods.PM_SaveVehicleTrailerDeprDetails(gUserName, vid, fieldId, val, isVehicle, onChangeSuccess, onFailed);
				if (fieldId == 2 && val == "") {
					document.getElementById('txtDeprEndDate').value = "";
				}
			}
		}

		function UpdateVehicleTrailerDeprCostDetails(c, fieldId, vid, isVehicle) {
			if (Page_ClientValidate()) {
				var val = c.value;
				PageMethods.set_path(thePath);
				PageMethods.PM_SaveVehicleTrailerDeprCostDetails(gUserName, vid, fieldId, val, isVehicle, onChangeSuccess, onFailed);
			}
		}

		function onChangeSuccess() {
			__doPostBack('lblCurrentBookValueValue', '');
			//location.reload();
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
			<asp:Panel ID="pnlDeprDetails" runat="server">
				<asp:Table ID="tblDeprDetails" runat="server">
					<asp:TableRow ID="trVehicleName" runat="server">
						<asp:TableCell ID="tcVehicleName" runat="server">
							<asp:Label ID="lblVTName" runat="server" Text="" Width="150px"></asp:Label>
							&nbsp;&nbsp;<asp:Label ID="lblVehicleTrailerName" runat="server"></asp:Label>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trInServiceDate" runat="server">
						<asp:TableCell ID="tcInServiceDate" runat="server">
							<asp:Label ID="lblInServiceDate" runat="server" Text="In-Service Date:" Width="150px"></asp:Label>
							&nbsp;&nbsp;<asp:TextBox ID="txtInServiceDate" runat="server" autocomplete="off"></asp:TextBox>
							<cc1:CalendarExtender ID="txtInServiceDate_CalendarExtender" runat="server" Enabled="True"
								TargetControlID="txtInServiceDate" Format="M/d/yyyy">
							</cc1:CalendarExtender>
							<asp:RangeValidator CssClass="validator-message" ID="rngInServiceDate" runat="server" ControlToValidate="txtInServiceDate"
								MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date"
								EnableClientScript="True" Enabled="True">'Invalid In-Service Date'!</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trDeprStartDate" runat="server">
						<asp:TableCell ID="tcDeprStartDate" runat="server">
							<asp:Label ID="lblDeprStartDate" runat="server" Text="Depr. Start Date:" Width="150px"></asp:Label>
							&nbsp;&nbsp;<asp:TextBox ID="txtDeprStartDate" runat="server" autocomplete="off"></asp:TextBox>
							<cc1:CalendarExtender ID="txtDeprStartDate_CalendarExtender" runat="server" Enabled="True"
								TargetControlID="txtDeprStartDate" Format="M/d/yyyy">
							</cc1:CalendarExtender>
							<asp:RangeValidator CssClass="validator-message" ID="rngDeprStartDate" runat="server" ControlToValidate="txtDeprStartDate"
								MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date"
								EnableClientScript="True" Enabled="True">'Invalid Depr. Start Date'!</asp:RangeValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trDeprEndDate" runat="server">
						<asp:TableCell ID="tcDeprEndDate" runat="server">
							<asp:Label ID="lblDeprEndDate" runat="server" Text="Depr. End Date:" Width="150px"></asp:Label>
							&nbsp;&nbsp;<asp:TextBox ID="txtDeprEndDate" runat="server" autocomplete="off"></asp:TextBox>
							<cc1:CalendarExtender ID="txtDeprEndDate_CalendarExtender" runat="server" Enabled="True"
								TargetControlID="txtDeprEndDate" Format="M/d/yyyy">
							</cc1:CalendarExtender>
							<asp:RangeValidator CssClass="validator-message" ID="rngDeprEndDate" runat="server" ControlToValidate="txtDeprEndDate"
								MinimumValue="1/1/1900" MaximumValue="6/6/2079" Display="dynamic" Type="date"
								EnableClientScript="True" Enabled="True">'Invalid Depr. End Date'!,</asp:RangeValidator>&nbsp;
                        <asp:CompareValidator CssClass="validator-message" ID="cvDeprEndDate" runat="server" ControlToCompare="txtDeprStartDate"
													ControlToValidate="txtDeprEndDate" Operator="GreaterThanEqual" Display="dynamic"
													Type="Date">'End date' cannot less than 'Start date'!</asp:CompareValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trDeprMonths" runat="server">
						<asp:TableCell ID="tcDeprMonths" runat="server">
							<asp:Label ID="lblDeprMonths" runat="server" Text="Depr. Months:" Width="150px"></asp:Label>
							&nbsp;&nbsp;<asp:TextBox ID="txtDeprMonths" runat="server" autocomplete="off"></asp:TextBox>
							<asp:CompareValidator CssClass="validator-message" ID="cvDeprMonths" runat="server" ControlToValidate="txtDeprMonths" Display="dynamic"
								Operator="DataTypeCheck" Type="Integer" ErrorMessage="Enter whole number for 'Depr. Months'!" ForeColor="Red">
							</asp:CompareValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trCost" runat="server">
						<asp:TableCell ID="tcCost" runat="server">
							<asp:Label ID="lblCost" runat="server" Text="Cost:" Width="150px"></asp:Label>
							$<asp:TextBox ID="txtCost" runat="server" autocomplete="off"></asp:TextBox>
							<asp:CompareValidator CssClass="validator-message" ID="cvCost" runat="server" ControlToValidate="txtCost" Display="dynamic"
								Operator="DataTypeCheck" Type="Double" ErrorMessage="Enter correct value for 'Cost'!" ForeColor="Red">
							</asp:CompareValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trMonthlyDepreciation" runat="server">
						<asp:TableCell ID="tcMonthlyDepreciation" runat="server">
							<asp:Label ID="lblMonthlyDepreciation" runat="server" Text="Monthly Depreciation:" Width="150px"></asp:Label>
							$<asp:TextBox ID="txtMonthlyDepreciation" runat="server" autocomplete="off"></asp:TextBox>
							<asp:CompareValidator CssClass="validator-message" ID="cvMonthlyDepreciation" runat="server" ControlToValidate="txtMonthlyDepreciation" Display="dynamic"
								Operator="DataTypeCheck" Type="Double" ErrorMessage="Enter correct value for 'Monthly Deprieciation'!" ForeColor="Red">
							</asp:CompareValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trCurrentBookValue" runat="server">
						<asp:TableCell ID="tcCurrentBookValue" runat="server">
							<asp:UpdatePanel ID="upCurrentBookValue" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<asp:Label ID="lblCurrentBookValue" runat="server" Text="Current Book Value:" Width="150px"></asp:Label>
									$<asp:Label ID="lblCurrentBookValueValue" runat="server" Text=""></asp:Label>
								</ContentTemplate>
							</asp:UpdatePanel>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:Panel>
			<asp:Label ID="lblNoRecordExist" runat="server" Text="" Visible="false"></asp:Label>
		</div>
	</form>
</body>
</html>
