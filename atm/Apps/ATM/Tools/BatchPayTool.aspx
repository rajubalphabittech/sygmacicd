<%@ Page Title="Batch Pay Tool" Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Tools_BatchPayTool" CodeBehind="BatchPayTool.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc2" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.30930.28736, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ OutputCache Location="None" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>ATM - Batch Pay Tool</title>
	<link href="/Content/bootstrap.min.css" rel="stylesheet" />
	<script src="/Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
	<script src="/Scripts/bootstrap.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/AJAX.js" type="text/javascript"></script>
	<script type="text/javascript">


		function ConfirmationWithIds(formIds) {
			//            var Ids = formIds;
			//            if (true) {
			//                //$("#btnHidden").click();
			//                OpenWindow('AddUpdate.aspx?fid=25000', 880, 650, 1, 1, 1, 1, 'atmp_25000');
			//                OpenWindow('AddUpdate.aspx?fid=25002', 880, 650, 1, 1, 1, 1, 'atmp_25002');
			//            }
			var varFormIdsStr = formIds;
			if (confirm('Misc forms created successfully.  Form IDs are ' + formIds + '.Open these forms now?')) {
				var varFormIds = varFormIdsStr.split(",");
				for (var i in varFormIds) {
					OpenWindow('../Payroll/Forms/AddUpdate.aspx?fid=' + varFormIds[i], 880, 650, 1, 1, 1, 1, 'form_' + varFormIds[i]);
				}
			}
		}

		function SetWeekending() {
			var val = $get('dteWeekendingDate').value;
			if (val != '') {
				var dd = new Date(val);
				var di = dd.getDay();
				if (di != 6) {
					dd.setDate(dd.getDate() + (6 - di));
					$get('dteWeekendingDate').value = dd.format("MM/dd/yyyy");
				}
			}
		}

		function SetDepartDate() {
			var val = $get('dteDepartDate').value;
			if (val != '') {
				var dd = new Date(val);
				var di = dd.getDay();
				if (di != 6) {
					dd.setDate(dd.getDate() + (6 - di));
					$get('dteWeekendingDate').value = dd.format("MM/dd/yyyy");
				}
			}
		}

	</script>
</head>
<body>
	<form id="form1" runat="server">
		<div class="container">
			<div class="row">
				<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
				</asp:ScriptManager>
				<asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" ShowMessageBox="false" ShowSummary="true" CssClass="alert alert-danger" ValidationGroup="Create" />
				<div class="pageTitle">
					Batch Pay Tool
				</div>
				<div class="pageSubtitle">
					Process Employee Payroll By Group
				</div>
				<asp:Panel ID="pnlFormInfo" runat="server">
					<asp:UpdatePanel ID="upFormInfo" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<div class="panel panel-default">
								<div class="panel-body">
									<div class="row">
										<div class="col-xs-2 col-sm-2 col-md-2 col-lg-1">
											<strong>Form Type</strong>
											<asp:Label ID="lblFormType" Text="Miscellaneous" runat="server"></asp:Label>
										</div>
										<div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
											<div class="form-group">
												<div>
													<label for="ddSygmaCenterNo">Center</label>
													<asp:DropDownList ID="ddSygmaCenterNo" runat="server" CssClass="form-control" DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" AutoPostBack="true" OnSelectedIndexChanged="ddSygmaCenterNo_SelectedIndexChanged">
													</asp:DropDownList>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvSygmaCenterNo" runat="server" ControlToValidate="ddSygmaCenterNo" ErrorMessage="'Center' is required!"
														EnableClientScript="true" Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
												</div>
												<div>
													<label for="ddPayScale">Pay Scale</label>
													<asp:DropDownList ID="ddPayScale" runat="server" DataTextField="PayScaleDisplay" DataValueField="PayScaleId" OnSelectedIndexChanged="ddPayScale_SelectedIndexChanged"
														AutoPostBack="True" CssClass="form-control">
													</asp:DropDownList>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvPayScale" runat="server" ControlToValidate="ddPayScale" ErrorMessage="'Pay Scale' is required!"
														EnableClientScript="true" Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
												</div>
												<div>
													<label for="ddAddRateType">Rate Type</label>
													<asp:DropDownList ID="ddAddRateType" runat="server" DataValueField="RateTypeId" DataTextField="RateTypeDescription" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddAddRateType_SelectedIndexChanged">
													</asp:DropDownList>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvRateType" runat="server" ControlToValidate="ddAddRateType" ErrorMessage="'Rate Type' is required!"
														EnableClientScript="true" Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
												</div>
												<asp:Panel ID="pnlCategory" runat="server" Visible="false">
													<label for="ddAddCategory">Category</label>
													<asp:DropDownList ID="ddAddCategory" runat="server" DataValueField="CategoryId" DataTextField="CategoryDescription" CssClass="form-control">
													</asp:DropDownList>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvCategory" runat="server" ControlToValidate="ddAddCategory" ErrorMessage="'Category' is required!"
														EnableClientScript="true" Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
												</asp:Panel>
												<div>
													<label for="txtQuantity">Quantity</label>
													<asp:TextBox ID="txtQuantity" runat="server" CausesValidation="False" CssClass="form-control"></asp:TextBox>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="'Quantity' is required!" EnableClientScript="true"
														Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
													<asp:RegularExpressionValidator CssClass="validator-message" ID="revQuantity" runat="server" Display="Dynamic" ControlToValidate="txtQuantity" ValidationGroup="Create" ErrorMessage="'Quantity' should be a valid number" ValidationExpression="^[1-9]\d*(\.\d+)?$" Text="*"></asp:RegularExpressionValidator>
													<asp:CustomValidator CssClass="validator-message" ID="cuvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="'Quantity' Should be a whole numer"
														EnableClientScript="true" Text="" ValidationGroup="Create" OnServerValidate="cuvQuantity_ServerValidate" Display="Dynamic"></asp:CustomValidator>
												</div>
												<div>
													<label for="txtNotes">Notes</label>
													<asp:TextBox ID="txtNotes" Rows="3" Style="overflow: auto" CssClass="form-control" runat="server" TextMode="MultiLine" Wrap="true" CausesValidation="False"></asp:TextBox>
													<div>
														<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvNotes" runat="server" ControlToValidate="txtNotes" ErrorMessage="'Notes' is required!" EnableClientScript="true"
															Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
													</div>
												</div>
											</div>
										</div>
										<div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
											<div class="form-group">
												<label for="lbEmployees">Employees</label>
												<asp:ListBox ID="lbEmployees" runat="server" DataValueField="EmployeeId" DataTextField="WebDisplay" Height="400px" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
												<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvEmployees" runat="server" ControlToValidate="lbEmployees" ErrorMessage="'Employee' is required!"
													EnableClientScript="true" Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
											</div>
										</div>
										<div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
											<asp:Panel ID="pnlRouteNoEdit" runat="server">
											</asp:Panel>
											<div class="form-group">
												<label for="txtRouteNo">Route #</label>
												<div class="input-group">
													<span class="input-group-addon" id="txtRouteNo-addon">M</span>
													<asp:TextBox aria-label="Username" aria-describedby="txtRouteNo-addon" ID="txtRouteNo" runat="server" CssClass="form-control" MaxLength="3"></asp:TextBox>
												</div>
												<div>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvRouteNo" runat="server" ControlToValidate="txtRouteNo" ErrorMessage="'Route	#' is required!" EnableClientScript="true"
														Text="" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
													<asp:CustomValidator CssClass="validator-message" ID="cuvRouteNo" runat="server" ControlToValidate="txtRouteNo" ErrorMessage="This 'Route #' already exists! Please try a different one."
														EnableClientScript="true" Text="This 'Route #' already exists! Please try a different one." ValidationGroup="Create" OnServerValidate="cuvRouteNo_ServerValidate" Display="Dynamic"></asp:CustomValidator>
													<asp:RangeValidator CssClass="validator-message" ID="rvRouteNo" runat="server" ControlToValidate="txtRouteNo" Text="" ValidationGroup="Create" ErrorMessage="Route# - Value entered should be between 0 and 999" Type="Integer" MinimumValue="0" Display="Dynamic" MaximumValue="999"></asp:RangeValidator>
												</div>
												<div>
													<label for="dteDepartDate">
														Depart Date
													</label>
													<asp:TextBox ID="dteDepartDate" runat="server" class="date form-control " onchange="SetDepartDate();"></asp:TextBox>
													<cc1:CalendarExtender
														ID="CalendarExtenderWeekending" runat="server" Enabled="True" TargetControlID="dteDepartDate" Format="M/d/yyyy" PopupPosition="Right">
													</cc1:CalendarExtender>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvDepartDate" runat="server" ErrorMessage="'Depart Date' is required!" Text="" ValidationGroup="Create"
														ControlToValidate="dteDepartDate" Display="Dynamic"></asp:RequiredFieldValidator>
													<asp:RegularExpressionValidator CssClass="validator-message" ID="revDepartDate" Display="Dynamic" runat="server" ErrorMessage="'Depart Date' is in Invalid Format!" Text="" ValidationGroup="Create" ControlToValidate="dteDepartDate" ValidationExpression="^(0[1-9]|1[012]|[1-9])[\/](0[1-9]|[12][0-9]|3[01]|[1-9])[\/](19|20)\d\d$">
													</asp:RegularExpressionValidator>
												</div>
												<div>
													<label for="dteWeekendingDate">
														Weekending
													</label>
													<asp:TextBox ID="dteWeekendingDate" runat="server" class="date form-control" onchange="SetWeekending();"></asp:TextBox>
													<cc1:CalendarExtender
														ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="dteWeekendingDate" Format="M/d/yyyy" PopupPosition="Right">
													</cc1:CalendarExtender>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvWeekendingDate" runat="server" ErrorMessage="'Weekending Date' is required!" Text="" ValidationGroup="Create"
														ControlToValidate="dteWeekendingDate" Display="Dynamic"></asp:RequiredFieldValidator>
													<asp:RegularExpressionValidator CssClass="validator-message" ID="revWeekendingDate" Display="Dynamic" runat="server" ErrorMessage="'Weekending Date' is in Invalid Format!" Text="*" ValidationGroup="Create" ControlToValidate="dteWeekendingDate" ValidationExpression="^(0[1-9]|1[012]|[1-9])[\/](0[1-9]|[12][0-9]|3[01]|[1-9])[\/](19|20)\d\d$">
													</asp:RegularExpressionValidator>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
					<div class="row">
						<asp:Panel ID="pnlCreateButtons" runat="server" HorizontalAlign="Center">
							<asp:Button ID="btnCreate" class="btn btn-primary" runat="server" Text="Create" OnClick="btnCreate_Click" ValidationGroup="Create" />
							<%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="window.close();" CausesValidation="false" />--%>
							<asp:Button ID="btnForms" class="btn btn-primary" runat="server" Text="Go to Forms Page" OnClick="btnForms_Click" CausesValidation="false" />
						</asp:Panel>
					</div>
				</asp:Panel>
			</div>
		</div>
	</form>
</body>
</html>

