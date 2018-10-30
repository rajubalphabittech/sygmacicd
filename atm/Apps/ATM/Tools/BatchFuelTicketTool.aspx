<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Tools_BatchFuelTicketTool" CodeBehind="BatchFuelTicketTool.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>ATM - Batch Fuel Ticket Tool</title>
	<script type="text/javascript" src="/Scripts/jquery-1.9.1.min.js"></script>

	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/AJAX.js" type="text/javascript"></script>

	<link rel="stylesheet" href="/Content/bootstrap.min.css" />
	<script type="text/javascript" src="/Scripts/bootstrap.min.js"></script>
	<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js" integrity="sha384-b/U6ypiBEHpOf/4+1nzFpr53nxSS+GLCkfwBdFNTxtclqqenISfwAzpKaMNFNmj4" crossorigin="anonymous"></script>
	<link rel="stylesheet" href="/Content/font-awesome.min.css" />
	<script type="text/javascript">
		function SetWeekending() {
			var val = $get('txtWeekendingDate').value;
			if (val != '') {
				var dd = new Date(val);
				var di = dd.getDay();
				if (di != 6) {
					dd.setDate(dd.getDate() + (6 - di));
					$get('txtWeekendingDate').value = dd.format("MM/dd/yyyy");
				}
			}

		}

		function Message(c) {
			alert(c);
		}

	</script>
</head>
<body>
	<form id="frmBFT" runat="server">
		<div class="container">
			<div class="row">
				<asp:ValidationSummary ID="FuelTicketValidationSummary" runat="server" EnableClientScript="true" ShowMessageBox="false" ShowSummary="true" CssClass="alert alert-danger" ValidationGroup="Create" />

				<div class="pageTitle">
					Batch Fuel Ticket Tool
				</div>
				<div class="pageSubtitle">
					Create Fuel Tickets by Vehicle/Trailer and Route
				</div>
				<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
				<asp:Panel ID="pnlChooseForms" runat="server" class="inlineBlock" Width="100%">
					<asp:UpdatePanel ID="upFormInfo" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<div class="panel panel-default">
								<div class="panel panel-body">
									<div class="row">
										<div class="col-sm-3 col-sm-3 col-med-3 col-lg-3">
											<asp:RadioButtonList CssClass="radioSpacing" ID="rblType" runat="server" RepeatDirection="Horizontal"
												AutoPostBack="True" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
												<asp:ListItem Text="&nbsp Vehicles &nbsp &nbsp &nbsp" Value="Vehicles"></asp:ListItem>

												<asp:ListItem Text="&nbsp Trailers" Value="Trailers"></asp:ListItem>
											</asp:RadioButtonList>
										</div>
									</div>
									<%--Import from Excel--%>
									<asp:CheckBox ID="cbImport" runat="server" AutoPostBack="true" OnCheckedChanged="cbImport_CheckedChanged" Visible="false"></asp:CheckBox>

									<asp:Panel ID="pnlUploadFile" runat="server" Visible="false">
										<div class="inputHeader">
											<asp:TextBox ID="txtFileLocation" runat="server" />
											<asp:Button ID="Button1" runat="server" Text="Browse"></asp:Button>
										</div>
									</asp:Panel>
									<asp:Panel ID="pnlMain" runat="server" Visible="false">
										<div class="row">
											<asp:Panel ID="pnlCenter" runat="server">
												<asp:UpdatePanel ID="upCenters" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<div class="col-sm-3 col-md-3 col-lg-3">
															<div>
																<label for="ddSygmaCenterNo">
																	Center
																</label>
																<asp:DropDownList ID="ddSygmaCenterNo" CssClass="form-control" runat="server" DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" AutoPostBack="true" OnSelectedIndexChanged="ddSygmaCenterNo_SelectedIndexChanged">
																</asp:DropDownList>
																<div>
																	<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvSygmaCenterNo" runat="server" ControlToValidate="ddSygmaCenterNo"
																		EnableClientScript="true" ErrorMessage="'Center' is required" Display="Dynamic" ValidationGroup="Create" AutoPostBack="true"></asp:RequiredFieldValidator>
																</div>
															</div>
															<div>
																<label for="txtWeekendingDate">
																	Weekending
																</label>
																<asp:TextBox ID="txtWeekendingDate" runat="server" CssClass="form-control date " AutoPostBack="true" OnTextChanged="txtWeekendingDate_TextChanged" onchange="SetWeekending();" AutoCompleteType="Disabled" Enabled="false" CausesValidation="false" MaxLength="10" ValidationGroup="Create"></asp:TextBox>
																<cc1:CalendarExtender
																	ID="CalendarExtenderWeekending" runat="server" Enabled="True" TargetControlID="txtWeekendingDate" Format="M/d/yyyy" PopupPosition="Right">
																</cc1:CalendarExtender>
																<div>
																	<asp:RegularExpressionValidator CssClass="validator-message" ID="revWeekendingDate" Display="Dynamic" runat="server" ErrorMessage="'Weekending Date' is in Invalid Format"
																		ValidationGroup="Create" ControlToValidate="txtWeekendingDate" ValidationExpression="^(0[1-9]|1[012]|[1-9])[\/](0[1-9]|[12][0-9]|3[01]|[1-9])[\/](19|20)\d\d$"></asp:RegularExpressionValidator>
																</div>
																<asp:Label ID="lblInvalidDate" runat="server" ForeColor="Red" Visible="false"></asp:Label>
															</div>
														</div>
													</ContentTemplate>
												</asp:UpdatePanel>
											</asp:Panel>
											<asp:Panel ID="pnlVehicles" runat="server">
												<div class="col-sm-2 col-md-2 col-lg-2">
													<label for="lbVehicles">
														Vehicles
													</label>
													<asp:ListBox ID="lbVehicles" runat="server" DataValueField="VehicleId" DataTextField="VehicleName" CssClass="form-control" Height="400px" SelectionMode="Multiple"></asp:ListBox>
													<div>
														<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvVehicles" runat="server" ControlToValidate="lbVehicles"
															EnableClientScript="true" ErrorMessage="'Vehicles' is required" Display="Dynamic" ValidationGroup="Create" Enabled="true" AutoPostBack="true">
														</asp:RequiredFieldValidator>
													</div>
												</div>
											</asp:Panel>
											<asp:Panel ID="pnlTrailers" runat="server">
												<div class="col-sm-2 col-md-2 col-lg-2">
													<label for="lbVehicles">
														Trailers
													</label>
													<asp:ListBox ID="lbTrailers" runat="server" DataValueField="TrailerId" DataTextField="TrailerName" Height="400px" CssClass="form-control" SelectionMode="Multiple"></asp:ListBox>
													<div>
														<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvTrailers" runat="server" ControlToValidate="lbTrailers"
															EnableClientScript="true" ErrorMessage="'Trailers' is required" Display="Dynamic" ValidationGroup="Create"></asp:RequiredFieldValidator>
													</div>
												</div>
											</asp:Panel>
											<div class="col-sm-2 col-md-2 col-lg-2" id="routes">
												<label for="lbRoutes">
													Routes - Depart Date
												</label>
												<asp:ListBox ID="lbRoutes" runat="server" DataValueField="FormID" DataTextField="RouteDisplay" Height="400px" CssClass="form-control" SelectionMode="Single"></asp:ListBox>
											</div>
											<div class="col-sm-2 col-md-2 col-lg-2" id="details">
												<label for="txtTicketNo">
													Ticket No:
												</label>
												<asp:TextBox ID="txtTicketNo" runat="server" MaxLength="15" CssClass="form-control"></asp:TextBox>
												<div>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvTicketNo" runat="server" ControlToValidate="txtTicketNo"
														EnableClientScript="true" ErrorMessage="'Ticket Number' is required" Display="Dynamic" ValidationGroup="Create">
													</asp:RequiredFieldValidator>
												</div>
												<label for="txtDatePurchased">
													Date Purchased:
												</label>
												<asp:TextBox ID="txtDatePurchased" runat="server" MaxLength="10" CssClass="date form-control"></asp:TextBox>
												<cc1:CalendarExtender
													ID="txtDatePurchased_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtDatePurchased" Format="M/d/yyyy" PopupPosition="Right">
												</cc1:CalendarExtender>
												<div>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvDatePurchased" runat="server" ControlToValidate="txtDatePurchased"
														EnableClientScript="true" ErrorMessage="'Date Purchased' is required" Display="Dynamic" ValidationGroup="Create">
													</asp:RequiredFieldValidator>
												</div>
												<div>
													<asp:RegularExpressionValidator CssClass="validator-message" ID="datePurchasedValidator" Display="Dynamic" runat="server" ErrorMessage="'Date Purchased' is in an Invalid Format"
														ValidationGroup="Create" ControlToValidate="txtDatePurchased" ValidationExpression="^(0[1-9]|1[012]|[1-9])[\/](0[1-9]|[12][0-9]|3[01]|[1-9])[\/](19|20)\d\d$"></asp:RegularExpressionValidator>
												</div>
												<div>
													<asp:CompareValidator CssClass="validator-message" ID="cvPurchaseDate" runat="server"
														ControlToCompare="txtDatePurchased" ControlToValidate="txtWeekendingDate" Display="dynamic"
														ErrorMessage="'Date purchased cannot be later than the Weekending Date'" Operator="GreaterThanEqual"
														Type="Date" ValidationGroup="Create"></asp:CompareValidator>
												</div>
												<label for="txtGallons">
													Gallons:
												</label>
												<asp:TextBox ID="txtGallons" runat="server" MaxLength="11" CssClass="form-control"></asp:TextBox>
												<div>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvGallons" runat="server" ControlToValidate="txtGallons"
														EnableClientScript="true" ErrorMessage="'Gallons' is required" Display="Dynamic" ValidationGroup="Create">
													</asp:RequiredFieldValidator>
												</div>
												<div>
													<asp:RangeValidator CssClass="validator-message" ID="rngGallons" ValidationGroup="Create" ControlToValidate="txtGallons" runat="server"
														MinimumValue="0" SetFocusOnError="true" Type="Double" Display="dynamic"
														EnableClientScript="True" Enabled="True" ErrorMessage="'Gallons' must be a positive value"></asp:RangeValidator>
												</div>
												<div>
													<asp:RegularExpressionValidator CssClass="validator-message" ID="gallonsDecimalValidator" Display="Dynamic" runat="server" ErrorMessage="'Gallons' can only contain up to 2 decimal places"
														ValidationGroup="Create" ControlToValidate="txtGallons" ValidationExpression="^(\\+|-)?[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
												</div>
												<label for="txtAmount">
													Amount:
												</label>
												<div class="input-group">
													<span class="input-group-addon" id="txtRouteNo-addon">$</span>
													<asp:TextBox ID="txtAmount" runat="server" MaxLength="7" CssClass="form-control"></asp:TextBox>
												</div>
												<div>
													<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvAmount" runat="server" ControlToValidate="txtAmount"
														EnableClientScript="true" ErrorMessage="'Amount' is required" Display="Dynamic" ValidationGroup="Create" Enabled="true">
													</asp:RequiredFieldValidator>
												</div>
												<div>
													<asp:RegularExpressionValidator CssClass="validator-message" ID="regexAmount" Display="Dynamic" runat="server" ErrorMessage="'Amount' can only contain up to 2 decimal places"
														ValidationGroup="Create" ControlToValidate="txtAmount" ValidationExpression="^(\\+|-)?[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
												</div>
												<div>
													<asp:RangeValidator CssClass="validator-message" ID="rngAmount" ValidationGroup="Create" ControlToValidate="txtAmount" runat="server"
														MinimumValue="0" SetFocusOnError="true" Type="Double" Display="dynamic"
														EnableClientScript="True" Enabled="True" ErrorMessage="'Amount' must be a positive value"></asp:RangeValidator>
												</div>
												<asp:Panel ID="pnlOdometer" runat="server">
													<label for="txtOdometer">
														Odometer:
													</label>
													<asp:TextBox ID="txtOdometer" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
													<div>
														<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvOdometer" runat="server" ControlToValidate="txtOdometer"
															EnableClientScript="true" ErrorMessage="'Odometer' is required" Display="Dynamic" ValidationGroup="Create">
														</asp:RequiredFieldValidator>
													</div>
													<div>
														<asp:RangeValidator CssClass="validator-message" ID="rngOdometer" ValidationGroup="Create" ControlToValidate="txtOdometer" runat="server"
															MinimumValue="0" SetFocusOnError="true" Type="Double" Display="dynamic"
															EnableClientScript="True" Enabled="True" ErrorMessage="'Odometer' must be a positive value"></asp:RangeValidator>
													</div>
												</asp:Panel>
												<asp:Panel ID="pnlHours" runat="server">
													<label for="txtHours">
														Hours Reading:
													</label>
													<asp:TextBox ID="txtHours" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
													<div>
														<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvHours" runat="server" ControlToValidate="txtHours"
															EnableClientScript="true" ErrorMessage="'Hours' is required" Display="Dynamic" ValidationGroup="Create">
														</asp:RequiredFieldValidator>
														<div>
															<asp:RangeValidator CssClass="validator-message" ID="rngHours" ValidationGroup="Create" ControlToValidate="txtHours" runat="server"
																MinimumValue="0" SetFocusOnError="true" Type="Double" Display="dynamic"
																EnableClientScript="True" Enabled="True" ErrorMessage="'Hours Reading' must be a positive value"></asp:RangeValidator>
														</div>
													</div>
												</asp:Panel>
											</div>
											<div class="col-sm-2 col-md-2 col-lg-2 pull-left" id="Div2">
												<label for="chkLocation">
													Location:
												</label>
												<asp:CheckBox ID="chkLocation" runat="server" AutoPostBack="true" OnCheckedChanged="chkLocation_CheckedChanged"></asp:CheckBox>
												<asp:Panel ID="pnlSellerDetails" runat="server">
													<label for="txtSellerName">
														Seller's Name:
													</label>
													<asp:TextBox ID="txtSellerName" runat="server" CssClass="form-control"></asp:TextBox>
													<label for="txtAddress1">
														Address Line 1:
													</label>
													<asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control"></asp:TextBox>
													<label for="txtAddess2">
														Address Line 2:
													</label>
													<asp:TextBox ID="txtAddress2" runat="server" CssClass="form-control"></asp:TextBox>
													<label for="txtZip">
														Zip: 
													</label>
													<asp:TextBox ID="txtZip" runat="server" MaxLength="6" CssClass="form-control"></asp:TextBox>
													<div>
														<%--compare validator --%>
														<asp:RegularExpressionValidator CssClass="validator-message" ID="regexZip" Display="Dynamic" runat="server" ErrorMessage="'Zip' is in Invalid Format"
															ValidationGroup="Create" ControlToValidate="txtZip" ValidationExpression="^[0-9]{0,6}$"></asp:RegularExpressionValidator>
													</div>
												</asp:Panel>
											</div>
										</div>
									</asp:Panel>
								</div>
								<div class="inputHeader" style="text-align: left">
									<asp:Label ID="lblMessage" runat="server" Visible="false" Style="padding-left: 5px; color: red"></asp:Label>
									<asp:Label ID="lblExisting" runat="server" Visible="false" Style="padding-left: 5px; color: red"></asp:Label>
									<asp:Label ID="lblAllExisting" runat="server" Visible="false" Style="padding-left: 5px; color: red"></asp:Label>
								</div>

							</div>
							<div class="row">
								<asp:Panel ID="pnlCreateButtons" runat="server" HorizontalAlign="Center">
									<asp:Button ID="btnAdd" CssClass="btn btn-primary" ValidationGroup="Create" runat="server" Text="Add" CausesValidation="true" OnClick="btnAdd_Click" Enabled="false"></asp:Button>
									<asp:Button ID="btnForms" CssClass="btn btn-primary" runat="server" Text="Go to Forms Page" OnClick="btnForms_Click" CausesValidation="false" />
								</asp:Panel>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
				</asp:Panel>

			</div>
			<asp:UpdateProgress ID="upProgress" runat="server" AssociatedUpdatePanelID="upFormInfo" DisplayAfter="100">
				<ProgressTemplate>
					<div class="disableBackground">
					</div>
					<div class="progressPane loading">
						<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
							Please Wait...
						</div>
						<asp:Image ID="imgGridProgress5" runat="server" ImageUrl="~/Images/animated_bar.gif" />
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>
		</div>
	</form>
</body>
</html>

