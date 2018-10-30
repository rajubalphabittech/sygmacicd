<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Payroll_Forms_AddUpdate" CodeBehind="AddUpdate.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ OutputCache Location="None" %>
<%@ Import Namespace="atm.Helpers" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>ATM - Payroll Form</title>
	<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.hotkeys-0.7.9.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/AJAX.js" type="text/javascript"></script>
	<script src="/Scripts/ATMForm.js" type="text/javascript"></script>
	<script src="/Scripts/json2.js" type="text/javascript"></script>

	<style type="text/css">
		div legend {
			font-weight: bold;
			font-size: 10PX;
		}

		.separatorLine {
			border-bottom: 1px solid gray;
			margin-bottom: 5px;
			margin-top: 5px;
			width: 99%;
		}

		.entityArea {
			position: relative;
			background-color: White;
			border: 1px solid grey;
			padding: 0px 0px 5px 0px;
			overflow: hidden;
			margin-bottom: 5px;
		}

		.entity {
			padding: 0px 0px 5px 5px;
		}

		.entityHeader {
			background-color: #00008B;
			color: White;
			font-weight: bold;
			padding-left: 2px;
			overflow: hidden;
			margin-bottom: 4px;
		}

		.entityDeleteIcon {
			font-weight: bold;
			color: white;
			text-decoration: none;
		}

		.entitySubHeader {
			background-color: maroon;
			color: White;
			font-weight: bold;
		}

		.mhHeaderColumn {
			width: 100px;
			font-weight: bold;
			vertical-align: top;
		}

		.mhRow {
			margin-bottom: 3px;
		}

		.toggle {
			cursor: pointer;
			border: 0px 0px 0px 0px;
		}

		.inputfocus {
			border: 2px solid #AA88FF;
			background-color: #FFEEAA;
		}

		.linkfocus {
			border: 2px solid #AA88FF;
		}

		a {
			text-decoration: none;
		}

			a:hover {
				text-decoration: underline;
			}

		.lefttAlign {
			float: left;
		}

		.rightAlign {
			float: right;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<div style="z-index: 1; width: 950px">

			<div style="margin-bottom: 10px; width: 100%">
				<div class="pageTitle inlineBlock" style="width: 550px">
					Payroll Form
				<asp:Label ID="lblFormId" runat="server"></asp:Label>
				</div>
				<asp:Panel ID="pnlMessages" runat="server">
					<div class="inlineBlock" style="width: 1000px; padding-top: 3px">


						<div class="inlineBlock" style="vertical-align: top; text-align: center; padding-top: 3px; margin-left: 5px">
							<asp:Panel ID="pnlMessageInside" runat="server" CssClass="inlineBlock">
								<asp:UpdatePanel ID="upMessages" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:TextBox ID="txtMessages" runat="server" Style="display: none"
											AutoPostBack="true" OnTextChanged="txtMessages_TextChanged"></asp:TextBox>
										<asp:Panel ID="pnlLabelMessages" runat="server" Visible="false">
											<div class="inlineBlock">
												<asp:Label ID="lblMessages" runat="server" Text="" ForeColor="#942C29"></asp:Label>
											</div>
										</asp:Panel>


									</ContentTemplate>
								</asp:UpdatePanel>
							</asp:Panel>
						</div>

					</div>
				</asp:Panel>
				<asp:UpdatePanel ID="upStatusPanel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
					<ContentTemplate>
						<asp:Panel ID="pnlActions" runat="server" CssClass="inlineBlock" Style="width: 890px; text-align: right">
							<div style="text-align: right;">
								<asp:Panel ID="pnlChangeStatus" runat="server" CssClass="inlineBlock">
									<asp:HyperLink ID="hlChangeStatus" runat="server" NavigateUrl="javascript: void(0);"></asp:HyperLink>
									&nbsp;|&nbsp;
								</asp:Panel>
								<asp:Panel ID="pnlChangeStatusAppr" runat="server" CssClass="inlineBlock">
									<asp:HyperLink ID="hlApprove" runat="server" NavigateUrl="javascript: void(0);" Text="Approve" onclick="ChangeStatus(3);return false;"></asp:HyperLink>
									&nbsp;|&nbsp;
								</asp:Panel>
                                <asp:Panel ID="pnlNext" runat="server" CssClass="inlineBlock">
									<asp:LinkButton ID="btnNext" runat="server" Text="Next Form" onclick="btnNext_Click"></asp:LinkButton>
									&nbsp;|&nbsp;
								</asp:Panel>
								<asp:Panel ID="pnlReject" runat="server" CssClass="inlineBlock">
									<asp:HyperLink ID="hlReject" runat="server" NavigateUrl="javascript: void(0);" Text="Reject" onclick="ChangeStatus(4);return false;"></asp:HyperLink>
									&nbsp;|&nbsp;
								</asp:Panel>
								<asp:Panel ID="pnlDelete" runat="server" CssClass="inlineBlock">
									<asp:HyperLink ID="hlDelete" runat="server" NavigateUrl="javascript: void(0);" Text="Delete" onclick="DeleteForm(); return false;"></asp:HyperLink>
									&nbsp;|&nbsp;
								</asp:Panel>
								<asp:HyperLink ID="hlClose" runat="server" NavigateUrl="javascript: void(0);" Text="Close" onclick="window.close();"></asp:HyperLink>
							</div>
						</asp:Panel>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
			<asp:Panel ID="pnlStatusLine" runat="server">
				<div class="inlineBlock" style="width: 570px; padding-top: 3px">
					<div class="inlineBlock inputHeader" style="vertical-align: top; text-align: left;">
						Last Updated:
					</div>
					<asp:Panel ID="pnlLastUpdated" runat="server" Style="text-align: left; margin-left: 5px" CssClass="inlineBlock">
						<asp:UpdatePanel ID="upLastUpdated" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
							<ContentTemplate>
								<asp:Label ID="lblLastUpdated" runat="server"></asp:Label>
								<asp:Label ID="lblRouteStarted" runat="server" Text="Route has started" Visible="false"></asp:Label>

							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>
				</div>

				<div class="inlineBlock" style="vertical-align: top; text-align: right; width: 350px">
					<asp:Panel ID="pnlStatus" runat="server" HorizontalAlign="Right" Width="350px">
						<div style="text-align: right;">
							<asp:UpdatePanel ID="upStatus" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
								<ContentTemplate>
									<div class="inlineBlock inputHeader" style="vertical-align: top; text-align: right; width: 100px; padding-top: 3px">
										Status:
									</div>
									<div class="inlineBlock" style="vertical-align: top; text-align: left; padding-top: 3px; margin-left: 5px">
										<asp:Label ID="lblStatus" runat="server" Style="vertical-align: top;"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 16px; margin-left: 3px">
										<asp:Image ID="imgStatus" runat="server" ImageAlign="Bottom" />
									</div>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</asp:Panel>
				</div>
			</asp:Panel>

			<div style="width: 100%; margin-top: 5px">
				<asp:Panel ID="pnlFormInfo" runat="server" class="inlineBlock" Width="100%">
					<asp:UpdatePanel ID="upFormInfo" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true" ShowMessageBox="true" ShowSummary="false" ValidationGroup="Create" />
							<div class="inputPanel" style="width: 100%;">
								<div class="header">
									Form Info
								</div>
								<div class="body" style="padding-bottom: 5px; height: 42px">
									<div class="inlineBlock" style="width: 140px">
										<div class="inputHeader">
											Form Type
										</div>
										<asp:DropDownList ID="ddFormType" runat="server" AutoPostBack="true" DataValueField="FormTypeId" DataTextField="FormTypeDescription" OnSelectedIndexChanged="ddFormType_SelectedIndexChanged">
										</asp:DropDownList>
										<asp:Label ID="lblFormType" runat="server"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 170px">
										<div class="inputHeader">
											Center
										</div>
										<asp:DropDownList ID="ddSygmaCenterNo" runat="server" DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" onchange="pullRouteInfo();">
										</asp:DropDownList>
										<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvSygmaCenterNo" runat="server" ControlToValidate="ddSygmaCenterNo" ErrorMessage="'Center' is required!"
											EnableClientScript="true" Text="*" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
										<asp:Label ID="lblSygmaCenter" runat="server"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 100px" id="routeNo" runat="server" title=''>
										<div class="inputHeader">
											Route
										</div>
										<asp:Panel ID="pnlRouteNoEdit" runat="server">
											<div class="inlineBlock" id="lblRouteNoPrefix" runat="server" style="width: 10px">
											</div>
											<asp:TextBox ID="txtRouteNo" runat="server" MaxLength="4" Width="45px" onchange="pullRouteInfo();"></asp:TextBox>
											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvRouteNo" runat="server" ControlToValidate="txtRouteNo" ErrorMessage="'Route	#' is required!" EnableClientScript="true"
												Text="*" ValidationGroup="Create" Display="Dynamic"></asp:RequiredFieldValidator>
											<asp:CustomValidator CssClass="validator-message" ID="cuvRouteNo" runat="server" ControlToValidate="txtRouteNo" ErrorMessage="This 'Route #' already exists! Please try a different one."
												EnableClientScript="true" Text="*" ValidationGroup="Create" OnServerValidate="cuvRouteNo_ServerValidate" Display="Dynamic"></asp:CustomValidator>
										</asp:Panel>
										<asp:Label ID="lblRouteNo" runat="server"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 120px; text-align: left">
										<div class="inputHeader">
											Depart Date
										</div>
										<asp:TextBox ID="dteDepartDate" runat="server" class="date" Width="70px" onchange="pullRouteInfo();" AutoCompleteType="Disabled"></asp:TextBox>
										<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvDepartDate" runat="server" ErrorMessage="'Depart Date'	is required!" Text="*" ValidationGroup="Create"
											ControlToValidate="dteDepartDate" Display="Dynamic"></asp:RequiredFieldValidator>
										<asp:RegularExpressionValidator CssClass="validator-message" ID="revDepartDate" runat="server"
											ErrorMessage="'Depart Date' is invalid!" Text="*" ValidationGroup="Create" ControlToValidate="dteDepartDate"
											ValidationExpression="^([1-9]|0[1-9]|1[012])[/]([1-9]|0[1-9]|[12][0-9]|3[01])[/](19|20)\d\d$" Display="Dynamic"></asp:RegularExpressionValidator>
										<asp:Label ID="lblDepartDate" runat="server"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 120px; text-align: left; vertical-align: top">
										<asp:Panel ID="pnlWeekending" runat="server">
											<div class="inputHeader" style="vertical-align: top; text-align: left">
												Weekending
											</div>
											<asp:TextBox ID="dteWeekendingDate" runat="server" class="date" Width="70px" onchange="SetWeekendingDate($(this));" AutoCompleteType="Disabled"></asp:TextBox>

											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvWeekendingDate" runat="server" ErrorMessage="'Weekending Date'	is required!" Text="*" ValidationGroup="Create"
												ControlToValidate="dteWeekendingDate" Display="Dynamic"></asp:RequiredFieldValidator>
											
											<asp:RangeValidator CssClass="validator-message" ID="rngLastSunday" runat="server" ControlToValidate="dteWeekendingDate" 
											                    MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true"
																					ErrorMessage="'Weekending' must not be in a closed week."
											                    Display="dynamic" Type="date" EnableClientScript="True" Enabled="True" ValidationGroup="Create">*</asp:RangeValidator>

											<asp:CompareValidator CssClass="validator-message" ID="cvWeekendingDate" runat="server" ControlToCompare="dteDepartDate" ControlToValidate="dteWeekendingDate"
												ErrorMessage="'Weekending Date' cannot be less than 'Depart Date'!" Operator="GreaterThanEqual"
												Type="Date" ValidationGroup="Create" Display="Dynamic" Text="*"></asp:CompareValidator>



											<asp:TextBox ID="dteFormWeekendingDate" runat="server" class="date" Width="70px" onchange="ValidateFormWeekending($(this));" ReadOnly="True" AutoCompleteType="Disabled"></asp:TextBox>
											
											<asp:RangeValidator CssClass="validator-message" ID="rngFormWeekendingDateLastSunday" runat="server" ControlToValidate="dteFormWeekendingDate" 
											                    MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true"
											                    ErrorMessage="'Weekending' must not be in a closed week."
											                    Display="dynamic" Type="date" EnableClientScript="True" Enabled="True" ValidationGroup="Create">*</asp:RangeValidator>

											<asp:Panel ID="pnlWeekendingDate" runat="server" Style="padding-top: 2px; text-align: left">
												<asp:Label ID="lblWeekending" runat="server"></asp:Label>
											</asp:Panel>
										</asp:Panel>
									</div>
									<div class="inlineBlock" style="width: 120px; text-align: left; vertical-align: top">
										<asp:Panel ID="pnlRouteCategory" runat="server">
											<div class="inputHeader" style="vertical-align: top; text-align: left">
												Route Category
											</div>
											<%--<asp:TextBox ID="txtRoutrcategory" runat="server" Width="70px"></asp:TextBox>--%>
											<%--<asp:DropDownList ID = "ddRouteCategory" runat="server" DataTextField="CategoryName" DataValueField="CategoryId" Width="90px" Visible="false"></asp:DropDownList>--%>
											<asp:Label ID="lblRouteCategory" runat="server" Text="NA"></asp:Label>
										</asp:Panel>
									</div>
									<%--<div class="inlineBlock" style="width: 140px; text-align: left">
									<asp:Panel ID="pnlRouteStatus" runat="server">
																				<div class="inputHeader" style="vertical-align: top; text-align: left">
											Route Status
										</div>
										<asp:Label ID="lblRouteStatus" runat="server" Text="Route Not Started"></asp:Label>
									</asp:Panel>
									</div>--%>
								</div>
								<asp:Panel ID="pnlPlanInfo" runat="server">
									<div class="body" style="padding-bottom: 5px; height: 42px">
										<div class="inlineBlock" style="width: 140px" id="Cases" title=''>
											<div class="inputHeader">
												Cases
											</div>
											<asp:TextBox ID="txtCasesOnCreate" runat="server" Width="50px"></asp:TextBox>
											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvCasesOnCreate" runat="server" ControlToValidate="txtCasesOnCreate"
												ErrorMessage="'Cases' is required!" EnableClientScript="true" Text="*" ValidationGroup="Create"
												Display="Dynamic"></asp:RequiredFieldValidator>
											<asp:CompareValidator CssClass="validator-message" ID="cvCasesOnCreate" runat="server" ControlToValidate="txtCasesOnCreate" Type="Integer" Operator="DataTypeCheck"
												ErrorMessage="Please enter valid 'Cases'!" EnableClientScript="true" Text="*" ValidationGroup="Create"></asp:CompareValidator>
										</div>
										<div class="inlineBlock" style="width: 170px" id="Pounds" title=''>
											<div class="inputHeader">
												Pounds
											</div>
											<asp:TextBox ID="txtPoundsOnCreate" runat="server" Width="50px"></asp:TextBox>
											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvPoundsOnCreate" runat="server" ControlToValidate="txtPoundsOnCreate"
												ErrorMessage="'Pounds' is required!" EnableClientScript="true" Text="*" ValidationGroup="Create"
												Display="Dynamic"></asp:RequiredFieldValidator>
											<asp:CompareValidator CssClass="validator-message" ID="cvPoundsOnCreate" runat="server" ControlToValidate="txtPoundsOnCreate" Type="Integer" Operator="DataTypeCheck"
												ErrorMessage="Please enter valid 'Pounds'!" EnableClientScript="true" Text="*" ValidationGroup="Create"></asp:CompareValidator>
										</div>
										<div class="inlineBlock" style="width: 100px" id="Cubes" title=''>
											<div class="inputHeader">
												Cubes
											</div>
											<asp:TextBox ID="txtCubesOnCreate" runat="server" Width="50px"></asp:TextBox>
											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvCubesOnCreate" runat="server" ControlToValidate="txtCubesOnCreate"
												ErrorMessage="'Cubes' is required!" EnableClientScript="true" Text="*" ValidationGroup="Create"
												Display="Dynamic"></asp:RequiredFieldValidator>
											<asp:CompareValidator CssClass="validator-message" ID="cvCubesOnCreate" runat="server" ControlToValidate="txtCubesOnCreate" Type="Integer" Operator="DataTypeCheck"
												ErrorMessage="Please enter valid 'Cubes'!" EnableClientScript="true" Text="*" ValidationGroup="Create"></asp:CompareValidator>
										</div>
										<div class="inlineBlock" style="width: 120px" id="Stops" title=''>
											<div class="inputHeader">
												Stops
											</div>
											<asp:TextBox ID="txtStopsOnCreate" runat="server" Width="50px"></asp:TextBox>
											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvStopsOnCreate" runat="server" ControlToValidate="txtStopsOnCreate"
												ErrorMessage="'Stops' is required!" EnableClientScript="true" Text="*" ValidationGroup="Create"
												Display="Dynamic"></asp:RequiredFieldValidator>
											<asp:CompareValidator CssClass="validator-message" ID="cvStopsOnCreate" runat="server" ControlToValidate="txtStopsOnCreate" Type="Integer" Operator="DataTypeCheck"
												ErrorMessage="Please enter valid 'Stops'!" EnableClientScript="true" Text="*" ValidationGroup="Create"></asp:CompareValidator>
										</div>
									</div>
								</asp:Panel>
								<asp:Panel ID="pnlCreateButtons" runat="server" Width="100%" HorizontalAlign="Center" Style="padding: 5px	0px 5px 0px">
									<asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" ValidationGroup="Create" />
									<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="window.close();" CausesValidation="false" />
								</asp:Panel>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
				</asp:Panel>



			</div>

			<div style="margin-top: 10px">
				<asp:Panel ID="pnlDrivers" runat="server" class="inputPanel" Visible="false" Style="width: 100%">
					<div class="header">
						<div class="inlineBlock" style="width: 30%; vertical-align: top">
							<u>P</u>re-Route Assignment
						</div>
					</div>
					<asp:UpdatePanel ID="upDrivers" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ddDriver" EventName="SelectedIndexChanged" />
							<asp:AsyncPostBackTrigger ControlID="ddDriverHelper" EventName="SelectedIndexChanged" />
						</Triggers>
						<ContentTemplate>
							<div class="body" id="Drivers">
								<asp:Panel ID="Driver" runat="server">
									<div class="inlineBlock" style="width: 200px">
										<div class="inputHeader">
											ATM Driver
										</div>
										<div class="inlineBlock" style="width: 25%;">
											<asp:DropDownList ID="ddDriver" runat="server" DataTextField="DriverName" DataValueField="DriverId" OnSelectedIndexChanged="ddDriver_SelectedIndexChanged" AutoPostBack="true" Width="150px" Style="text-align: right; margin-bottom: 4px;"></asp:DropDownList>
										</div>

										<div class="inputHeader" style="vertical-align: top; text-align: left">
											</br>                                               
																								Actual Route Start Time
										</div>
										<asp:Label ID="lblRouteStartTime" runat="server" Text="-"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 200px">
										<div class="inputHeader">
											ATM Team Driver/Helper
										</div>
										<div class="inlineBlock" style="width: 25%;">
											<asp:DropDownList ID="ddDriverHelper" runat="server" DataTextField="DriverName" DataValueField="DriverId" OnSelectedIndexChanged="ddDriverHelper_SelectedIndexChanged" AutoPostBack="true" Width="150px" Style="text-align: right; margin-top: 2px;"></asp:DropDownList>
										</div>

										<div class="inputHeader" style="vertical-align: top; text-align: left">
											</br>                                              
																								Actual Route End Time
										</div>
										<asp:Label ID="lblRouteEndTime" runat="server" Text="-"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 200px; text-align: left">
										<div class="inputHeader">
											Telogis Primary Driver                                                                                      
										</div>
										<div class="inlineBlock" style="width: 25%;">
											<asp:Label ID="lblTPDriver" runat="server" DataTextField="DriverName" DataValueField="DriverId" AutoPostBack="true" Width="150px" Style="text-align: left; margin-bottom: 4px;"></asp:Label>
										</div>
										<div class="inputHeader" style="vertical-align: top; text-align: left">
											</br>
															Ontime Recording status
										</div>
										<asp:Label ID="lblOntimeStatus" runat="server" Text="-"></asp:Label>
									</div>
									<div class="inlineBlock" style="width: 200px; text-align: left">
										<div class="inputHeader">
											Telogis Secondary Driver                                      
										</div>
										<div class="inlineBlock" style="width: 25%;">
											<asp:Label ID="lblTSDriver" runat="server" DataTextField="DriverName" DataValueField="DriverId" AutoPostBack="true" Width="150px" Style="text-align: left; margin-bottom: 4px;"></asp:Label>
										</div>
										<asp:Panel ID="pnlRouteStatus" runat="server">
											<div class="inputHeader" style="vertical-align: top; text-align: left">
												</br>
															Route Status
											</div>
											<asp:Label ID="lblRouteStatus" runat="server" Text="Route Not Started"></asp:Label>
										</asp:Panel>
									</div>
									<div class="inlineBlock" style="width: 140px; text-align: left; color: Red">
										<asp:Panel ID="Panel1" runat="server">
											<asp:Label ID="lblActualDriverUpdated" runat="server" Text="Actual Driver Details updated from Telogis" Visible="false"></asp:Label>
										</asp:Panel>
									</div>
								</asp:Panel>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
				</asp:Panel>
			</div>

			<div style="margin-top: 10px;">
				<asp:Panel ID="pnlPlan" runat="server" class="inputPanel inlineBlock" Style="width: 100%; vertical-align: top" Visible="false">
					<div class="header">
						Route Info
					</div>
					<div class="body" style="padding-bottom: 5px; vertical-align: top">
						<div class="inlineBlock" style="width: 190px">
							<div class="inputHeader">
								Cases
							</div>
							<asp:TextBox ID="txtCases" runat="server" Width="50px" onChange="SavePlanInfo(this, 'Cases');"></asp:TextBox>
							<asp:Label ID="lblCases" runat="server"></asp:Label>
						</div>
						<div class="inlineBlock" style="width: 190px">
							<div class="inputHeader">
								Pounds
							</div>
							<asp:TextBox ID="txtPounds" runat="server" Width="50px" onChange="SavePlanInfo(this, 'Pounds');"></asp:TextBox>
							<asp:Label ID="lblPounds" runat="server"></asp:Label>
						</div>
						<div class="inlineBlock" style="width: 190px">
							<div class="inputHeader">
								Cubes
							</div>
							<asp:TextBox ID="txtCubes" runat="server" Width="50px" onChange="SavePlanInfo(this, 'Cubes');"></asp:TextBox>
							<asp:Label ID="lblCubes" runat="server"></asp:Label>
						</div>
						<div class="inlineBlock" style="width: 160px">
							<div class="inputHeader">
								Stops
							</div>
							<asp:TextBox ID="txtStops" runat="server" Width="50px" onChange="SavePlanInfo(this,'Stops');"></asp:TextBox>
							<asp:Label ID="lblStops" runat="server"></asp:Label>
						</div>
						<div class="inlineBlock" style="width: 145px">
							<div class="inputHeader">
								Miles
							</div>
							<asp:UpdatePanel ID="upMiles" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
								<ContentTemplate>
									<asp:Label ID="lblMiles" runat="server"></asp:Label>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</div>
				</asp:Panel>
			</div>
			<asp:Panel ID="pnlAssets" runat="server" Style="margin-top: 10px;" Visible="false">
				<div class="inlineBlock" style="vertical-align: top; width: 390px">
					<asp:Panel ID="pnlEmployees" runat="server" Style="width: 100%;" CssClass="inputPanel">
						<div class="header">
							<div class="inlineBlock" style="width: 30%; vertical-align: top">
								<u>E</u>mployees
							</div>
						</div>
						<asp:UpdatePanel ID="upEmployees" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
							<Triggers>
								<%--<asp:AsyncPostBackTrigger ControlID="ddAddEmployee" EventName="SelectedIndexChanged" />--%>
								<asp:AsyncPostBackTrigger ControlID="btnAddEmployee" EventName="Click" />
								<asp:AsyncPostBackTrigger ControlID="btnRefreshEmployees" EventName="Click" />
								<asp:AsyncPostBackTrigger ControlID="btnRefreshAddEmployees" EventName="Click" />
								<asp:AsyncPostBackTrigger ControlID="btnRefreshBackhauls" EventName="Click" />
							</Triggers>
							<ContentTemplate>
								<asp:Button ID="btnRefreshEmployees" runat="server" Text="Refresh" OnClick="btnRefreshEmployees_Click" Style="display: none" />
								<asp:Button ID="btnRefreshAddEmployees" runat="server" Text="Refresh Add Employee" OnClick="btnRefreshAddEmployees_Click" Style="display: none" />
								<div class="body" id="employees">
									<asp:Panel ID="pnlAddEmployee" runat="server" Style="margin-bottom: 10px">
										<div class="inlineBlock" style="width: 35%">
											<%--<asp:DropDownList ID="ddAddEmployee" runat="server" DataTextField="WebDisplay" DataValueField="EmployeeId" DataMember="AvailableEmployees"
											OnSelectedIndexChanged="ddAddEmployee_SelectedIndexChanged" AutoPostBack="true">
										</asp:DropDownList>--%>
											<asp:TextBox ID="txtAddEmployee" runat="server" Style="width: 130px;" autocomplete="off" ToolTip="Start typing the Employee's Name to be added..." MaxLength="100"></asp:TextBox>
											<asp:HiddenField ID="hfAddEmployee" runat="server" />
											<asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" Style="display: none" OnClick="btnAddEmployee_Click" />
										</div>
										<div class="inlineBlock" style="width: 35%;">
											<div id="AddEmployeeFocus" style="display: none; color: Gray; font-size: 80%">Type Employee Name</div>
										</div>
										<div class="inlineBlock" style="width: 25%; text-align: right">
											<asp:HyperLink ID="hlAddRateTypeDialog" runat="server" NavigateUrl="javascript: void(0);" onclick="OpenPaymentDialog();">Add <u>P</u>ayment
											</asp:HyperLink>
										</div>
									</asp:Panel>
									<div>
										<asp:HyperLink ID="hlEmployeesLimit" runat="server" Style="text-align: center; display: none; background-color: yellow; color: black; font-weight: bold; font-size: 10px;">Form already has 50 Employees!</asp:HyperLink>
									</div>
									<asp:Label ID="lblEmptyEmployees" runat="server" Text="No employees have been added."></asp:Label>
									<asp:Repeater ID="rptEmployees" runat="server" OnItemDataBound="rptEmployees_ItemDataBound" DataMember="Employees">
										<ItemTemplate>
											<div class="entityArea" style="width: 96%;">
												<div class="entityHeader">
													<div class="inlineBlock" style="width: 200px">
														<%#Container.ItemIndex + 1 %>.&nbsp;Name
													</div>
													<div class="inlineBlock" style="width: 143px">
														Pay Scale
													</div>
													<div class="inlineBlock" style="width: 16px; text-align: right">
														<asp:LinkButton ID="btnRemoveEmployee" runat="server" OnClientClick="return confirm('Are you sure you want to remove this employee?');"
															OnClick="btnRemoveEmployee_Click" CommandArgument='<%# Eval("EmployeeId") + "|" + Eval("WebDisplay") %>' Visible="<%# IsOpen	%>">
															<span class="entityDeleteIcon">X</span>
														</asp:LinkButton>
													</div>
												</div>
												<div id="testDiv" class="entity">
													<div id="testDiv2" class="inlineBlock" runat="server" style="width: 200px; vertical-align: top;">
														<%# Eval("WebDisplay") %>
													</div>
													<div class="inlineBlock" style="width: 143px; vertical-align: top; text-align: left">
														<asp:DropDownList ID="ddPayScale" runat="server" DataValueField="PayScaleId" DataTextField="PayScaleDisplay" DataMember="Employees"
															ToolTip="'Pay Scale' that is used to calculate the employee's payments." Visible="false">
														</asp:DropDownList>
														<asp:Label ID="lblPayScale" runat="server" Visible="false"></asp:Label>
													</div>
												</div>
												<asp:UpdatePanel ID="upPayments" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
													<ContentTemplate>
														<asp:Button ID="btnUpdatePayScale" runat="server" Text="Refresh" OnClick="btnUpdatePayScale_Click" Style="display: none;" CommandArgument='<%# Eval("EmployeeId") %>' />
														<fieldset style="width: 360px; margin-bottom: 5px">
															<legend>Payments</legend>
															<asp:Panel ID="pnlEmptyPayments" runat="server" Style="margin-bottom: 3px; margin-top: 3px">
																No payments have been added.
															</asp:Panel>
															<asp:Repeater ID="rptEmployeePayments" runat="server" OnItemDataBound="rptEmployeePayments_ItemDataBound">
																<HeaderTemplate>
																	<div class="entitySubHeader" style="margin-top: 5px; width: 353px">
																		<div class="inlineBlock" style="width: 100px">
																			Type
																		</div>
																		<div class="inlineBlock" style="width: 55px; text-align: right">
																			Quantity
																		</div>
																		<div class="inlineBlock" style="width: 80px; text-align: right">
																			Rate
																		</div>
																		<div class="inlineBlock" style="width: 88px; text-align: right">
																			Total
																		</div>
																	</div>
																</HeaderTemplate>
																<ItemTemplate>
																	<div style="margin-top: 2px">
																		<div class="inlineBlock" style="width: 100px" title='<%# Eval("PaymentNotes") %>'>
																			<asp:Label ID="lblRateTypeDesc" runat="server"></asp:Label>
																		</div>
																		<div class="inlineBlock" style="width: 55px; text-align: right">
																			<asp:TextBox ID="txtQty" runat="server" Width="55px" Style="text-align: right"></asp:TextBox>
																		</div>
																		<div class="inlineBlock" style="width: 80px; text-align: right">
																			$<asp:Label ID="lblRate" runat="server"></asp:Label>
																		</div>
																		<div class="inlineBlock" style="width: 88px; text-align: right">
																			$<asp:Label ID="lblTotal" runat="server"></asp:Label>
																		</div>
																		<div class="inlineBlock">
																			<asp:ImageButton ID="btnRemovePayment" runat="server" class="imagebutton" OnClientClick="return confirm('Are you sure you want to delete this payment?');"
																				OnClick="btnRemovePayment_Click" TabIndex="-1" ImageUrl="~/Images/Icons/delete-icon3.png" ImageAlign="Left"></asp:ImageButton>
																		</div>
																	</div>
																	<asp:Label ID="lblPaymentNotes" runat="server"></asp:Label>
																</ItemTemplate>
															</asp:Repeater>
															<asp:Panel ID="pnlGrandTotal" runat="server" Style="width: 323px; border-top: 1px dotted gray; text-align: right; margin-top: 5px; padding-top: 2px;">
																<div class="inputHeader inlineBlock" style="text-align: left; font-size: 11px;">
																	<%# Eval("WebDisplay") %>
																</div>
																<div class="inputHeader inlineBlock" style="width: 91px; text-align: right; font-size: 11px;">
																	$<asp:Label ID="lblEmployeeTotal" runat="server"></asp:Label>
																</div>
															</asp:Panel>
															<asp:Panel ID="pnlHolidayPay" runat="server" Style="width: 338px; text-align: left; margin-top: 5px; padding-top: 2px;">
																<asp:Label ID="lblHolidayPay" runat="server" Text="* - Holiday Pay" Style="text-align: left; font-weight: bold; font-size: 11px;">
																</asp:Label>
															</asp:Panel>
														</fieldset>
													</ContentTemplate>
												</asp:UpdatePanel>
												<asp:Panel ID="pnlTimeLogs" runat="server" Style="margin-bottom: 3px; margin-top: 3px">
													<asp:UpdatePanel ID="upTimeLogs" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Button ID="btnRefreshEmployeeTL" runat="server" OnClick="btnRefreshEmployeeTL_Click" Style="display: none;" CommandName="Employee" CommandArgument='<%# Eval("EmployeeId") %>' />
															<fieldset style="width: 360px; margin-bottom: 5px">
																<legend>Time Logs
																	<asp:HyperLink ID="hlAddEtl" runat="server" NavigateUrl="javascript: void(0);" onclick='<%# string.Format("OpenTLDialog(&#39;{0}&#39;, {1});", Container.ClientID, Eval("EmployeeId"))%>' Style="margin-left: 5px" Visible="<%# IsOpen %>">
																		<asp:Image ID="imgAddETL" runat="server" ImageUrl="~/Images/Icons/document_add_16.png" />
																	</asp:HyperLink>
																</legend>
																<div style="width: 100%">
																	<asp:Panel ID="pnlEmptyEmployeesTimeLogs" runat="server" Style="margin-bottom: 3px; margin-top: 3px">
																		No time logs have been added
																	</asp:Panel>
																	<asp:Repeater ID="rptEmployeesTimeLogs" runat="server">
																		<HeaderTemplate>
																			<div style="margin-bottom: 5px; width: 100%">
																				<div style="margin-top: 5px; width: 353px" class="entitySubHeader">
																					<div class="inlineBlock" style="width: 122px">
																						Start
																					</div>
																					<div class="inlineBlock" style="width: 122px">
																						End
																					</div>
																					<div class="inlineBlock" style="width: 49px; text-align: right">
																						Total
																					</div>
																				</div>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<div style="margin-top: 2px">
																				<div class="inlineBlock" style="width: 122px">
																					<%# Eval("StartDateTime")%>
																				</div>
																				<div class="inlineBlock" style="width: 122px">
																					<%# Eval("EndDateTime")%>
																				</div>
																				<div class="inlineBlock" style="width: 49px; text-align: right">
																					<%# Eval("HoursAndMinutes")%>
																				</div>
																				<div class="inlineBlock" style="width: 20px; text-align: left">
																					<asp:HyperLink ID="hlUpdateETL" runat="server" NavigateUrl="javascript:	void(0);"
																						onclick='<%# string.Format("OpenTLUpdateDialog(&#39;{0}&#39;, {1}, &#39;{2}&#39;, &#39;{3}&#39;, &#39;{4}&#39;);", Container.NamingContainer.NamingContainer.ClientID, Eval("TimeLogId"), Eval("StartDateTime"), Eval("EndDateTime"), Eval("HoursAndMinutes"))%>'
																						Visible="<%# IsOpen %>">
																						<asp:Image ID="imgUpdateETL" runat="server" ImageUrl="../../../../Images/Icons/Edit1.png" AlternateText="E" ToolTip="Edit Employee Time Log"
																							ImageAlign="Bottom" />
																					</asp:HyperLink>
																				</div>
																				<div class="inlineBlock" style="width: 30px; text-align: left">
																					<asp:HyperLink ID="hlRemoveETL" runat="server" NavigateUrl="javascript:	void(0);" onclick='<%# string.Format("OpenTLDeleteDialog(&#39;{0}&#39;, {1});", Container.NamingContainer.NamingContainer.ClientID, Eval("TimeLogId"))%>'
																						Visible="<%# IsOpen %>">
																						<asp:Image ID="imgRemoveETL" runat="server" ImageUrl="../../../../Images/Icons/delete-icon3.png" AlternateText="X" ToolTip="Remove Employee Time Log"
																							ImageAlign="Bottom" />
																					</asp:HyperLink>
																				</div>
																			</div>
																		</ItemTemplate>
																		<FooterTemplate>
																			</div>
																		</FooterTemplate>
																	</asp:Repeater>
																	<div>
																		<asp:Label ID="lblTimeLoglOverlap" runat="server" Text="Last Time log is overlapping with other time logs" Style="text-align: center; display: none; background-color: yellow; color: black; font-weight: bold; font-size: 10px;"></asp:Label>
																	</div>
																</div>
															</fieldset>
														</ContentTemplate>
													</asp:UpdatePanel>
												</asp:Panel>
											</div>
										</ItemTemplate>
									</asp:Repeater>
								</div>
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>
					<asp:Panel ID="pnlBackhauls" runat="server" CssClass="inputPanel inlineBlock" Width="100%" Visible="false" Style="margin-top: 10px;">
						<div class="header">
							<div class="inlineBlock" style="width: 30%; vertical-align: top">
								<u>B</u>ackhauls
							</div>
						</div>
						<div class="body" id="backhauls">
							<asp:Image ID="imgAddBackhaul" runat="server" ImageUrl="~/Images/Icons/document_add_16.png" Style="margin-bottom: 5px; cursor: pointer" />
							<asp:UpdatePanel ID="upBackhauls" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="btnRefreshBackhauls" EventName="Click" />
								</Triggers>
								<ContentTemplate>
									<asp:Button ID="btnRefreshBackhauls" runat="server" Style="display: none;" OnClick="btnRefreshBackhauls_Click" />
									<asp:HiddenField ID="hfOpenAddPayment" runat="server" />
									<asp:Repeater ID="rptBackhauls" runat="server" Visible="false">
										<ItemTemplate>
											<div class="entityArea" style="width: 96%;">
												<div class="entityHeader">
													<div class="inlineBlock" style="width: 150px;">
														<%#Container.ItemIndex + 1 %>.&nbsp;PO #
													</div>
													<div class="inlineBlock" style="width: 195px">
														Revenue
													</div>
													<div class="inlineBlock" style="width: 16px; text-align: right;">
														<asp:LinkButton ID="btnRemoveBackhaul" runat="server" OnClientClick="return confirm('Are you sure you want to delete this backhaul?  \'Backhaul\' payments can not exceed the number of backhauls.  The payments will adjust to accommodate this change.');"
															OnClick="btnRemoveBackhaul_Click" TabIndex="-1" CommandArgument='<%# Eval("BackhaulId") %>' Visible="<%# IsOpen %>">
													<span class="entityDeleteIcon">X</span>
														</asp:LinkButton>
													</div>
												</div>
												<div class="entity">
													<div class="inlineBlock" style="width: 140px">
														<input type="text" id="txtBHPoNo" value="<%# Eval("PONo") %>" style="width: 90px" maxlength="20" onchange="SaveBackhaul(this, <%# Eval("BackhaulId") %>);"
															<%# (IsOpen) ? "" : "readonly" %> />
													</div>
													<div class="inlineBlock">
														$<input type="text" id="txtBHRevenue" value="<%# Eval("Revenue", "{0:N2}") %>" style="width: 60px" maxlength="10" onchange="SaveBackhaul(this, <%# Eval("BackhaulId") %>);"
															<%# (IsOpen) ? "" : "readonly" %> />
													</div>
													<div class="inlineBlock">
													</div>
												</div>
											</div>
										</ItemTemplate>
									</asp:Repeater>
									<asp:Label ID="lblEmptyBackhauls" runat="server" Visible="false" Text="No backhauls have been added."></asp:Label>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</asp:Panel>
					<asp:Panel ID="pnlStatusLog" runat="server" CssClass="inputPanel inlineBlock" Width="100%" Visible="false" Style="margin-top: 10px;">
						<div class="header">
							<div class="inlineBlock" style="width: 60%; vertical-align: top">
								<u>F</u>orm Status Change Log
							</div>
						</div>
						<div class="body" id="statusChange">
							<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
								<ContentTemplate>
									<asp:GridView ID="gvStatusLog" runat="server" AutoGenerateColumns="false" Width="97.5%" RowStyle-VerticalAlign="Top">
										<Columns>
											<asp:BoundField HeaderText="Action" DataField="ActionDescription" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="80px" />
											<asp:BoundField HeaderText="Action By" DataField="UserName" HeaderStyle-HorizontalAlign="Left" HtmlEncode="false" ItemStyle-Width="100px" />
											<asp:BoundField HeaderText="Date Time" DataField="ActionDate" DataFormatString="{0:MM/dd/yy h:mm tt}" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="90px" />
										</Columns>
									</asp:GridView>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</asp:Panel>

				</div>
				<div class="inlineBlock" style="vertical-align: top; width: 545px; margin-left: 10px">
					<div>
						<asp:Panel ID="pnlVehicles" runat="server" class="inputPanel" Style="width: 100%; vertical-align: top" Visible="false">
							<div class="header">
								<div class="inlineBlock" style="width: 30%; vertical-align: top">
									<u>V</u>ehicles
								</div>
							</div>
							<asp:UpdatePanel ID="upVehicles" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
								<Triggers>
									<%--<asp:AsyncPostBackTrigger ControlID="ddAddVehicle" EventName="SelectedIndexChanged" />--%>
									<asp:AsyncPostBackTrigger ControlID="btnAddVehicle" EventName="Click" />
								</Triggers>
								<ContentTemplate>
									<div class="body" id="vehicles">
										<asp:Panel ID="pnlAddVehicle" runat="server">
											<%--<asp:DropDownList ID="ddAddVehicle" runat="server" DataValueField="VehicleId" DataTextField="WebDisplay" OnSelectedIndexChanged="ddAddVehicle_SelectedIndexChanged"
											AutoPostBack="true" Style="margin-bottom: 10px">
										</asp:DropDownList>--%>
											<div class="inlineBlock" style="width: 25%;">
												<asp:TextBox ID="txtAddVehicle" runat="server" MaxLength="50" Style="margin-bottom: 10px; width: 130px;"></asp:TextBox>
												<asp:HiddenField ID="hfAddVehicle" runat="server" />
												<asp:HiddenField ID="hfVFTDialogBoxCall" runat="server" />
												<asp:Button ID="btnAddVehicle" runat="server" Text="Add Employee" Style="display: none" OnClick="btnAddVehicle_Click" />
											</div>
											<div class="inlineBlock" style="width: 40%; text-align: left">
												<div id="AddVehicleFocus" style="display: none; color: Gray; font-size: 80%">Enter first 2 char of vehicle#</div>
											</div>
										</asp:Panel>
										<div>
											<asp:Label ID="lblEmptyVehicles" runat="server" Text="No vehicles have been added."></asp:Label>
											<asp:Repeater ID="rptVehicles" runat="server" OnItemDataBound="rptVehicles_ItemDataBound">
												<ItemTemplate>
													<div class="entityArea" style="width: 97%;">
														<div class="entityHeader">
															<div class="inlineBlock" style="width: 135px;">
																<%#Container.ItemIndex + 1 %>.&nbsp;Vehicle #
															</div>
															<div class="inlineBlock" style="width: 120px">
																Begin Odometer
															</div>
															<div class="inlineBlock" style="width: 120px">
																End Odometer
															</div>
															<div class="inlineBlock" style="width: 110px">
																<asp:Label ID="lblIDName" runat="server" Visible='<%# !Convert.IsDBNull(Eval("IDName")) %>'><%# Eval("IDName") %></asp:Label>
															</div>
															<div class="inlineBlock" style="width: 21px; text-align: right">
																<asp:LinkButton ID="btnRemoveVehicle" runat="server" OnClientClick="return confirm('Are you sure you want to remove this vehicle?');"
																	OnClick="btnRemoveVehicle_Click" CommandArgument='<%#	Eval("FormVehicleId")  + "|" + Eval("VehicleId") + "|" + Eval("WebDisplay")%>' Visible="<%# IsOpen %>">
																<span class="entityDeleteIcon">X</span>
																</asp:LinkButton>
															</div>
														</div>
														<div class="entity">
															<div class="inlineBlock" style="width: 135px;">
																<%# Eval("WebDisplay") %>
															</div>
															<div class="inlineBlock" style="width: 120px">
																<input type="text" id="txtBeginOdometer" class="FocusBeginOdometer" style="width: 80px" onchange="SaveOdometer(this,'<%# Eval("FormVehicleId") %>','begin')" value="<%# Eval("BeginOdometer",	"{0:#,##0.##}") %>"
																	origval="<%# Eval("BeginOdometer", "{0:#,##0.##}") %>" />
															</div>
															<div class="inlineBlock" style="width: 120px">
																<input type="text" id="txtEndOdometer" name="<%#Container.ItemIndex%>" style="width: 80px" onchange="SaveOdometer(this, '<%# Eval("FormVehicleId") %>', 'end')" value="<%# Eval("EndOdometer",	"{0:#,##0.##}") %>"
																	origval="<%# Eval("EndOdometer", "{0:#,##0.##}") %>" />
															</div>
															<div class="inlineBlock" style="width: 110px">
																<asp:Panel ID="pnlVehicleId" runat="server" Visible='<%# !Convert.IsDBNull(Eval("IDName")) %>'>
																	<input type="text" id="txtVehicleID" style="width: 80px" value='<%# Eval("ID") %>' onchange="SaveExternalVehicleId(this, '<%# Eval("FormVehicleId") %>', 'end')" maxlength="30"
																		<%# (IsOpen) ? "" : "readonly" %> />
																</asp:Panel>
															</div>
														</div>
														<div>
															<asp:HyperLink ID="hlOverlap" runat="server" Style="text-align: center; display: none; background-color: yellow; color: black; font-weight: bold; font-size: 10px;">HyperLink</asp:HyperLink>
														</div>
														<asp:UpdatePanel ID="upFuelTickets" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Button ID="btnRefreshVehicleFT" runat="server" OnClick="btnRefreshFT_Click" Style="display: none;" CommandName="Vehicle" CommandArgument='<%# Eval("FormVehicleId") %>' />
																<asp:Button ID="btnRefreshAddVehicleFT" runat="server" OnClick="btnRefreshAddFT_Click" Style="display: none;" CommandName="Vehicle" CommandArgument='<%# Eval("FormVehicleId") %>' />
																<asp:HiddenField ID="hfValues" runat="server" />
																<fieldset style="width: 515px; margin-bottom: 5px">
																	<legend><u>F</u>uel Tickets
																	<a href="javascript: void(0);" class="vehicleFTFocus" onclick="OpenFTDialog('<%# Container.ClientID %>',<%# Eval("VehicleId") %>,<%# Eval("FormVehicleId") %>, 'Vehicle');"
																		style="margin-left: 5px">
																		<asp:Image ID="imgAddVFT" runat="server" ImageUrl="~/Images/Icons/document_add_16.png" />
																	</a>
																	</legend>
																	<div style="width: 100%">
																		<asp:Panel ID="pnlEmptyVehicleFuelTickets" runat="server" Style="margin-bottom: 3px; margin-top: 3px">
																			No fuel tickets have been added
																		</asp:Panel>
																		<asp:Repeater ID="rptVehicleFuelTickets" runat="server">
																			<HeaderTemplate>
																				<div style="margin-bottom: 5px; width: 100%">
																					<div style="margin-top: 5px; width: 512px" class="entitySubHeader">
																						<div class="inlineBlock" style="width: 90px">
																							Ticket #
																						</div>
																						<div class="inlineBlock" style="width: 75px">
																							Date
																						</div>
																						<div class="inlineBlock" style="width: 75px; text-align: right">
																							Odometer
																						</div>
																						<div class="inlineBlock" style="width: 65px; text-align: right">
																							Gallons
																						</div>
																						<div class="inlineBlock" style="width: 50px; text-align: right">
																							Price
																						</div>
																						<div class="inlineBlock" style="width: 65px; text-align: right">
																							Total
																						</div>
																					</div>
																			</HeaderTemplate>
																			<ItemTemplate>
																				<div style="margin-top: 2px">
																					<div class="inlineBlock" style="width: 90px">
																						<%# Eval("FuelTicketNo") %>
																					</div>
																					<div class="inlineBlock" style="width: 75px">
																						<%# Eval("DatePurchased", "{0:M/d/yyyy}")
																						%>
																					</div>
																					<div class="inlineBlock" style="width: 75px; text-align: right">
																						<%# Eval("Miles", "{0:#,##0.##}")%>
																					</div>
																					<div class="inlineBlock" style="width: 65px; text-align: right">
																						<%# Eval("Gallons", "{0:#,##0.##}")%>
																					</div>
																					<div class="inlineBlock" style="width: 50px; text-align: right">
																						$<%# Eval("Price", "{0:N2}")%>
																					</div>
																					<div class="inlineBlock" style="width: 65px; text-align: right">
																						$<%# Eval("Amount",	"{0:N2}")%>
																					</div>
																					<div class="inlineBlock" style="width: 30px; text-align: left">
																						<asp:HyperLink ID="hlUpdateTFT" runat="server" NavigateUrl="javascript:	void(0);" onclick='<%# string.Format("OpenFTUpdateDialog(&#39;{0}&#39;, {1}, &#39;Vehicle&#39;, &#39;{2}&#39;, &#39;{3}&#39;, {4}, {5}, {6}, {7});", Container.NamingContainer.NamingContainer.ClientID, Eval("FuelTicketId"), 
																																												Eval("FuelTicketNo"), Eval("DatePurchased", "{0:M/d/yyyy}"), Eval("Gallons"), Eval("Amount"), Eval("Miles"), Eval("VehicleId"))%>'>
																							<asp:Image ID="imgUpdateVFT" runat="server" ImageUrl="../../../../Images/Icons/Edit1.png" AlternateText="E" ToolTip="Update Vehicle Fuel Ticket"
																								ImageAlign="Bottom" />
																						</asp:HyperLink>
																					</div>
																					<div class="inlineBlock" style="width: 30px; text-align: left">
																						<asp:HyperLink ID="hlRemoveTFT" runat="server" NavigateUrl="javascript:	void(0);" onclick='<%# string.Format("OpenFTDeleteDialog(&#39;{0}&#39;, {1}, &#39;Vehicle&#39;, {2});", Container.NamingContainer.NamingContainer.ClientID, Eval("FuelTicketId"), Eval("VehicleId"))%>'>
																							<asp:Image ID="imgRemoveVFT" runat="server" ImageUrl="../../../../Images/Icons/delete-icon3.png" AlternateText="X" ToolTip="Remove Vehicle Fuel Ticket"
																								ImageAlign="Bottom" />
																						</asp:HyperLink>
																					</div>
																				</div>
																			</ItemTemplate>
																			<FooterTemplate>
																				</div>
																			</FooterTemplate>
																		</asp:Repeater>
																	</div>
																</fieldset>
															</ContentTemplate>
														</asp:UpdatePanel>
													</div>
												</ItemTemplate>
											</asp:Repeater>
										</div>
									</div>
								</ContentTemplate>
							</asp:UpdatePanel>
						</asp:Panel>
					</div>
					<div style="margin-top: 10px">
						<asp:Panel ID="pnlTrailers" runat="server" class="inputPanel" Visible="false" Style="width: 100%">
							<div class="header">
								<div class="inlineBlock" style="width: 30%; vertical-align: top">
									<u>T</u>railers
								</div>
							</div>
							<asp:UpdatePanel ID="upTrailers" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
								<Triggers>
									<%--<asp:AsyncPostBackTrigger ControlID="ddAddTrailer" EventName="SelectedIndexChanged" />--%>
									<asp:AsyncPostBackTrigger ControlID="btnAddTrailer" EventName="Click" />
								</Triggers>
								<ContentTemplate>
									<div class="body" id="trailers">
										<asp:Panel ID="pnlAddTrailer" runat="server">
											<%--<asp:DropDownList ID="ddAddTrailer" runat="server" DataValueField="TrailerId" DataTextField="WebDisplay" OnSelectedIndexChanged="ddAddTrailer_SelectedIndexChanged"
											AutoPostBack="true" Style="margin-bottom: 10px">
										</asp:DropDownList>--%>
											<div class="inlineBlock" style="width: 25%">
												<div id="AddTrailerFocus" style="display: none; color: Gray; font-size: 80%">Enter first 2 char of trailer#</div>
												<asp:TextBox ID="txtAddTrailer" runat="server" MaxLength="50" Style="margin-bottom: 10px; width: 130px;"></asp:TextBox>
												<asp:HiddenField ID="hfAddTrailer" runat="server" />
												<asp:HiddenField ID="hfTFTDialogBoxCall" runat="server" />
												<asp:Button ID="btnAddTrailer" runat="server" Text="Add Employee" Style="display: none" OnClick="btnAddTrailer_Click" />
											</div>
											<%--<div class="inlineBlock" style="width: 40%; text-align: left">
																						<div id="AddTrailerFocus" style="display: none; color:Gray; font-size:80%">Enter first 2 char of trailer#</div>
																				</div>--%>
											<div class="inlineBlock" style="width: 32%">
												<asp:DropDownList ID="ddlCenterList" runat="server" DataTextField="CenterDisplay" DataValueField="SygmaCenterNo"
													AutoPostBack="false" Height="20px" Width="170px">
												</asp:DropDownList>
											</div>
										</asp:Panel>
										<%--<asp:Label ID="lblEmptyTrailers" runat="server" Text="No trailers have been added."></asp:Label>--%>
										<div style="text-align: left">
											<asp:Label ID="lblEmptyTrailers" runat="server" Text="No trailers have been added."></asp:Label>
										</div>


										<asp:Repeater ID="rptTrailers" runat="server" OnItemDataBound="rptTrailers_ItemDataBound">
											<ItemTemplate>
												<div class="entityArea" style="width: 97%;">
													<div class="entityHeader">
														<div class="inlineBlock" style="width: 140px">
															<%#Container.ItemIndex + 1 %>.&nbsp;Trailer #
														</div>
														<div class="inlineBlock" style="width: 120px">
															Begin Hours
														</div>
														<div class="inlineBlock" style="width: 120px">
															End Hours
														</div>
														<div class="inlineBlock" style="width: 131px; text-align: right">
															<asp:LinkButton ID="btnRemoveTrailer" runat="server" OnClientClick="return confirm('Are you sure you want to remove this trailer?');"
																OnClick="btnRemoveTrailer_Click" CommandArgument='<%# Eval("FormTrailerId")   + "|" + Eval("TrailerId") + "|" + Eval("WebDisplay") %>' Visible="<%#	IsOpen %>">
															<span class="entityDeleteIcon">X</span>
															</asp:LinkButton>
														</div>
													</div>
													<div class="entity">
														<div class="inlineBlock" style="width: 140px;">
															<%# Eval("WebDisplay") %>
														</div>
														<div class="inlineBlock" style="width: 120px">
															<input type="text" id="txtBeginHours" class="FocusBeginHour" style="width: 80px" onchange="SaveHours(this,'<%# Eval("FormTrailerId") %>','begin')" value="<%# Eval("BeginHours", "{0:#,##0.#}") %>"
																origval="<%# Eval("BeginHours", "{0:#,##0.#}") %>"></input>
														</div>
														<div class="inlineBlock" style="width: 120px">
															<input type="text" id="txtEndHours" name="<%#Container.ItemIndex%>" style="width: 80px" onchange="SaveHours(this,'<%# Eval("FormTrailerId") %>','end')" value="<%# Eval("EndHours", "{0:#,##0.#}") %>"
																origval="<%# Eval("EndHours", "{0:#,##0.#}") %>"></input>
														</div>
													</div>
													<asp:UpdatePanel ID="upFuelTickets" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Button ID="btnRefreshTrailerFT" runat="server" OnClick="btnRefreshFT_Click" Style="display: none;" CommandName="Trailer" CommandArgument='<%# Eval("FormTrailerId") %>' />
															<asp:Button ID="btnRefreshAddTrailerFT" runat="server" OnClick="btnRefreshAddFT_Click" Style="display: none;" CommandName="Trailer" CommandArgument='<%# Eval("FormTrailerId") %>' />
															<fieldset style="width: 515px">
																<legend><u>F</u>uel Tickets <a href="javascript: void(0);" class="trailerFTFocus" onclick="OpenFTDialog('<%# Container.ClientID %>',<%# Eval("TrailerId") %>,<%# Eval("FormTrailerId") %>, 'Trailer');"
																	style="margin-left: 5px">
																	<asp:Image ID="imgAddTFT" runat="server" ImageUrl="~/Images/Icons/document_add_16.png" />
																</a></legend>
																<div style="width: 100%">
																	<asp:Panel ID="pnlEmptyTrailerFuelTickets" runat="server" Style="margin-bottom: 3px; margin-top: 3px">
																		No fuel tickets have been added
																	</asp:Panel>
																	<asp:Repeater ID="rptTrailerFuelTickets" runat="server">
																		<HeaderTemplate>
																			<div style="margin-bottom: 5px; width: 100%">
																				<div style="margin-top: 5px; width: 512px" class="entitySubHeader">
																					<div class="inlineBlock" style="width: 80px">
																						Ticket #
																					</div>
																					<div class="inlineBlock" style="width: 75px">
																						Date
																					</div>
																					<div class="inlineBlock" style="width: 85px; text-align: right">
																						Hour Reading
																					</div>
																					<div class="inlineBlock" style="width: 60px; text-align: right">
																						Gallons
																					</div>
																					<div class="inlineBlock" style="width: 50px; text-align: right">
																						Price
																					</div>
																					<div class="inlineBlock" style="width: 65px; text-align: right">
																						Total
																					</div>
																				</div>
																		</HeaderTemplate>
																		<ItemTemplate>
																			<div style="margin-top: 2px">
																				<div class="inlineBlock" style="width: 80px">
																					<%# Eval("FuelTicketNo")
																					%>
																				</div>
																				<div class="inlineBlock" style="width: 75px">
																					<%# Eval("DatePurchased", "{0:M/d/yyyy}") %>
																				</div>
																				<div class="inlineBlock" style="width: 85px; text-align: right">
																					<%# Eval("FuelTicketHours", "{0:#,##0.##}")%>
																				</div>
																				<div class="inlineBlock" style="width: 60px; text-align: right">
																					<%# Eval("Gallons", "{0:#,##0.##}")%>
																				</div>
																				<div class="inlineBlock" style="width: 50px; text-align: right">
																					$<%# Eval("Price", "{0:N2}")%>
																				</div>
																				<div class="inlineBlock" style="width: 65px; text-align: right">
																					$<%# Eval("Amount", "{0:N2}")%>
																				</div>
																				<div class="inlineBlock" style="width: 30px; text-align: left">
																					<a href="javascript: void(0);" onclick='<%# string.Format("OpenFTUpdateDialog(&#39;{0}&#39;, {1}, &#39;Trailer&#39;, &#39;{2}&#39;, &#39;{3}&#39;, {4}, {5}, {6}, {7});", Container.NamingContainer.NamingContainer.ClientID, Eval("FuelTicketId"), 
																																												Eval("FuelTicketNo"), Eval("DatePurchased", "{0:M/d/yyyy}"), Eval("Gallons"), Eval("Amount"), Eval("FuelTicketHours"), Eval("TrailerId"))%>'>
																						<asp:Image ID="imgUpdateVFT" runat="server" ImageUrl="../../../../Images/Icons/Edit1.png" AlternateText="E" ToolTip="Update Trailer Fuel Ticket"
																							ImageAlign="Bottom" />
																					</a>
																				</div>
																				<div class="inlineBlock" style="width: 30px; text-align: left">
																					<a href="javascript: void(0);" onclick="OpenFTDeleteDialog('<%# Container.NamingContainer.NamingContainer.ClientID %>', <%# Eval("FuelTicketId") %>, 'Trailer', <%# Eval("TrailerId") %>);">
																						<asp:Image ID="imgRemoveVFT" runat="server" ImageUrl="../../../../Images/Icons/delete-icon3.png" AlternateText="X" ToolTip="Remove Trailer Fuel Ticket"
																							ImageAlign="Bottom" />
																					</a>
																				</div>
																			</div>
																		</ItemTemplate>
																		<FooterTemplate>
																			</div>
																		</FooterTemplate>
																	</asp:Repeater>
																</div>
															</fieldset>
														</ContentTemplate>
													</asp:UpdatePanel>
												</div>
											</ItemTemplate>
										</asp:Repeater>
									</div>
								</ContentTemplate>
							</asp:UpdatePanel>
						</asp:Panel>
					</div>

					<%--   chris--%>
				</div>
			</asp:Panel>

			<asp:Panel ID="pnlNotes" runat="server" CssClass="inputPanel inlineBlock" Width="40%" Style="margin-top: 10px; margin-bottom: 3px; vertical-align: top;">
				<div class="header">
					<div class="inlineBlock" style="width: 30%; vertical-align: top">
						Notes
					</div>
				</div>
				<div class="body" style="overflow: auto; display: block; position: relative;">

					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
						<ContentTemplate>
							<asp:Repeater runat="server" ID="rptNotes">
								<ItemTemplate>
									<asp:Label ID="lbuserName" runat="server" Text='<%# Eval("Title") %>' Font-Bold="true"></asp:Label>

									<br></br>
									Notes:
									<asp:Label ID="Notes" runat="server" Text='<%# Eval("Notes") %>'></asp:Label>
									<br></br>


								</ItemTemplate>
							</asp:Repeater>
						</ContentTemplate>
					</asp:UpdatePanel>

				</div>
			</asp:Panel>

		</div>
		<div id="paymentDialog">
			<div class="mhRow">
				<div class="inlineBlock mhHeaderColumn">
					Rate Type:
				</div>
				<div class="inlineBlock">
					<asp:UpdatePanel ID="upAddRateType" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:DropDownList ID="ddAddRateType" runat="server" DataValueField="RateTypeId" DataTextField="RateTypeDescription" onchange="RateTypeChanged(this);">
							</asp:DropDownList>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</div>
			<div id="mhcategory" class="mhRow" style="display: none;">
				<div class="inlineBlock mhHeaderColumn">
					Category:
				</div>
				<div class="inlineBlock">
					<asp:UpdatePanel ID="upRateTypeCategory" runat="server" UpdateMode="Conditional" RenderMode="Inline" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:DropDownList ID="ddAddRateTypeCategory" runat="server">
							</asp:DropDownList>
							<asp:Button ID="btnCategoryRefresh" runat="server" Text="CatRefresh" Style="display: none" OnClick="btnCategoryRefresh_Click" />
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="btnCategoryRefresh" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
			</div>
			<div class="mhRow">
				<div class="inlineBlock mhHeaderColumn">
					Quantity:
				</div>
				<div class="inlineBlock">
					<input type="text" id="txtAddRateQuantity" maxlength="12" style="width: 75px" onchange="ValidateQuantity($(this), GetOption($('#ddAddRateType')).attr('allowQuarters'), false);" runat="server" />
				</div>
			</div>
			<div class="mhRow">
				<div class="inlineBlock mhHeaderColumn">
					Holiday Pay?
				</div>
				<div class="inlineBlock">
					<input type="checkbox" id="cbHolidayPay" onclick="$('#btnRatePreview').click();" runat="server" />
				</div>
			</div>
			<div class="mhRow">
				<div class="inlineBlock mhHeaderColumn">
					<asp:Label ID="lblAddRateTypeNotes" Text="Notes:" runat="server"></asp:Label>
				</div>
				<div class="inlineBlock">
					<input type="text" id="txtAddRateNotes" maxlength="50" style="width: 250px" />
				</div>
			</div>
			<div class="mhRow">
				<div class="inlineBlock mhHeaderColumn">
					Apply To:
				</div>
				<div class="inlineBlock">
					<asp:UpdatePanel ID="upAddRateApplyTos" runat="server" UpdateMode="Conditional" RenderMode="Inline" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:ListBox ID="lbAddRateApplyTos" runat="server" DataValueField="EmployeeId" DataTextField="WebDisplay" SelectionMode="Multiple"
								Width="250px" Height="60px"></asp:ListBox>
							<asp:Button ID="btnRatePreview" runat="server" Text="Preview" Style="display: none" OnClick="btnRatePreview_Click" />
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="btnRatePreview" />
							<asp:AsyncPostBackTrigger ControlID="btnCategoryRefresh" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
			</div>
			<div class="mhRow">
				<div class="inlineBlock">
					NA - Rate type not applicable for Pay Scale of the Employee
				</div>
			</div>
		</div>

		<div id="tldialog">
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 200px">
					Start Date:
				</div>
				<input type="text" id="txtAddTLStartDate" style="width: 60px" onchange="PreviewHoursMinutes();" maxlength="10" />
				<input type="hidden" id="txtStartDate" style="width: 60px" />
				<asp:HyperLink ID="hlCalender" runat="server" ImageUrl="~/images/calendarIcon.gif" CssClass="ImageAsLink">
				</asp:HyperLink>
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 200px">
					Start Time:
				</div>
				<asp:DropDownList ID="ddAddStartHour" runat="server" DataValueField="HoursId" DataTextField="HoursDisplay" onchange="PreviewHoursMinutes();">
				</asp:DropDownList>
				<asp:DropDownList ID="ddAddStartMin" runat="server" DataValueField="MinutesId" DataTextField="MinutesDisplay" onchange="PreviewHoursMinutes();">
				</asp:DropDownList>
				<asp:DropDownList ID="ddAddStartAmPm" runat="server" DataValueField="AmPmId" DataTextField="AmPmDisplay" onchange="PreviewHoursMinutes();">
				</asp:DropDownList>
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 200px">
					End Date:
				</div>
				<input type="text" id="txtAddTLEndDate" style="width: 60px" onfocus="SetCursorPosition();" onchange="PreviewHoursMinutes();" maxlength="10" />
				<input type="hidden" id="txtEndDate" style="width: 60px" />
				<asp:HyperLink ID="hlEndCalender" runat="server" ImageUrl="~/images/calendarIcon.gif" CssClass="ImageAsLink">
				</asp:HyperLink>
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 200px">
					End Time:
				</div>
				<asp:DropDownList ID="ddAddEndHour" runat="server" DataValueField="HoursId" DataTextField="HoursDisplay" onchange="PreviewHoursMinutes();">
				</asp:DropDownList>
				<asp:DropDownList ID="ddAddEndMin" runat="server" DataValueField="MinutesId" DataTextField="MinutesDisplay" onchange="PreviewHoursMinutes();">
				</asp:DropDownList>
				<asp:DropDownList ID="ddAddEndAmPm" runat="server" DataValueField="AmPmId" DataTextField="AmPmDisplay" onchange="PreviewHoursMinutes();">
				</asp:DropDownList>
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 200px">
					Hours : Minutes :
				</div>
				<asp:UpdatePanel ID="upTotalHoursMin" runat="server" UpdateMode="Conditional" RenderMode="Inline" ChildrenAsTriggers="false">
					<ContentTemplate>
						<asp:Label ID="lblTotalHoursMin" runat="server"></asp:Label>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
		</div>
		<div id="addbackhaulDialog">
			<div style="margin-bottom: 5px">
				<div class="inputHeader inlineBlock" style="width: 60px">
					PO #:
				</div>
				<input type="text" id="txtAddBackhaulPoNo" value="<%# Eval("PONo") %>" style="width: 90px" maxlength="20" />
			</div>
			<div>
				<div class="inputHeader inlineBlock" style="width: 60px">
					Revenue:
				</div>
				$<input type="text" id="txtAddBackhaulRevenue" value="<%# Eval("Revenue", "{0:N2}") %>" style="width: 60px" maxlength="10" />
			</div>
			<div>
				<div class="inputHeader inlineBlock" style="width: 60px">
					Add Payment ?
				</div>
				<input type="checkbox" id="cbAddPayment" runat="server" />
			</div>
		</div>
		<div id="ftdeletedialog">
			This will remove the fuel ticket from the form. Do you want to permanently delete the ticket as well?
		</div>
		<div id="tldeletedialog">
			This will remove the Time Log from the form. Do you want to remove it?
		</div>
		<div id="ftdialog">
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 100px;">
					Ticket #:
				</div>
				<input type="text" id="txtAddFTNo" style="width: 100px" maxlength="15" class="email" />
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 100px">
					Date Purchased:
				</div>
				<input type="text" id="txtAddFTDate" style="width: 60px" maxlength="10" />
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 100px">
					Gallons:
				</div>
				<input type="text" id="txtAddFTGallons" style="width: 40px" maxlength="11" />
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 100px">
					Amount:
				</div>
				$<input type="text" id="txtAddFTAmount" style="width: 50px" maxlength="7" />
			</div>
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 100px">
					<span id="ftQtyType"></span>:
				</div>
				<input type="text" id="txtAddFTQty" style="width: 60px" maxlength="10" />
			</div>
		</div>
		<div id="ndialog">
			<div style="margin-bottom: 5px">
				<div class="inlineBlock" style="font-weight: bold; width: 100px;">
					Notes :
				</div>
				<asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="36px"
					Width="95%" MaxLength="500"></asp:TextBox>
			</div>
		</div>

		<div id="selFTdialog">
			<div class="mhRow">
				<div class="inlineBlock mhHeaderColumn">
					Fuel Tickets:
				</div>
				<div class="inlineBlock">
					<asp:HiddenField ID="hfVTId" runat="server" />
					<asp:HiddenField ID="hfType" runat="server" />
					<asp:HiddenField ID="hfOpen" runat="server" />
					<asp:HiddenField ID="hfCallFrom" runat="server" />
					<asp:UpdatePanel ID="upFuelTickets" runat="server" UpdateMode="Conditional" RenderMode="Inline" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:ListBox ID="lbSelectFuelTickets" runat="server" class="FocusSelFT" DataValueField="FuelTicketId" DataTextField="FTDisplay" SelectionMode="Multiple"
								Width="600px" Height="60px"></asp:ListBox>
							<%--Width="600px" Height="60px" DataSourceID="SqlDataSource1"></asp:ListBox>--%>
							<asp:Button ID="btnLoadExisting" runat="server" OnClick="btnLoadExisting_Click" Style="display: none" />
							<asp:Button ID="btnRefreshVFT" runat="server" OnClick="btnRefreshVFT_Click" Style="display: none;" />
							<asp:Button ID="btnRefreshTFT" runat="server" OnClick="btnRefreshTFT_Click" Style="display: none;" />
							<%--<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
														ConnectionString="<%$ ConnectionStrings:ATM %>" ProviderName="<%$ ConnectionStrings:ATM.ProviderName %>"
									SelectCommand="up_p_getExisitingFuelTickets" SelectCommandType="StoredProcedure" 
														CancelSelectOnNullParameter="False">
									<SelectParameters>
										<asp:ControlParameter ControlID="hfVTId" Name="VTID" PropertyName="Value" Type="String" />
																<asp:ControlParameter ControlID="hfType" Name="ftType" PropertyName="Value" Type="String" />
									</SelectParameters>
								</asp:SqlDataSource>--%>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="btnLoadExisting" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
			</div>
		</div>

		<asp:UpdatePanel ID="upWarning" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
			<ContentTemplate>
				<asp:Panel ID="pnlWarning" runat="server" Text="Odometer Overlap" Width="750px" Style="text-align: center; display: none; background-color: yellow; color: black; font-weight: bold; font-size: 12px; padding: 5px 0px 5px 0px;">
					Warning!
					<asp:Label ID="lblWarning" Text="test" runat="server"></asp:Label>
				</asp:Panel>
			</ContentTemplate>
		</asp:UpdatePanel>
		<asp:Button ID="btnNotesRefresh" runat="server" Text="Button" Visible="false"
			OnClick="btnNotesRefresh_Click" />


		<asp:UpdateProgress ID="upDriverUpdate" runat="server" AssociatedUpdatePanelID="upDrivers" DisplayAfter="100">
			<ProgressTemplate>
				<div class="disableBackground">
				</div>
				<div class="progressPane loading">
					<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
						Updating Drivers...
					</div>
					<asp:Image ID="imgGridProgress" runat="server" ImageUrl="~/Images/animated_bar.gif" />
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>

		<asp:UpdateProgress ID="upEmployeesProgress" runat="server" AssociatedUpdatePanelID="upEmployees" DisplayAfter="100">
			<ProgressTemplate>
				<div class="disableBackground">
				</div>
				<div class="progressPane loading">
					<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
						Updating Employee, Please Wait...
					</div>
					<asp:Image ID="imgGridProgress2" runat="server" ImageUrl="~/Images/animated_bar.gif" />
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>

		<asp:UpdateProgress ID="upVehiclesProgress" runat="server" AssociatedUpdatePanelID="upVehicles" DisplayAfter="100">
			<ProgressTemplate>
				<div class="disableBackground">
				</div>
				<div class="progressPane loading">
					<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
						Updating Vehicle, Please Wait...
					</div>
					<asp:Image ID="imgGridProgress3" runat="server" ImageUrl="~/Images/animated_bar.gif" />
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>
		<asp:UpdateProgress ID="upTrailersProgress" runat="server" AssociatedUpdatePanelID="upTrailers" DisplayAfter="100">
			<ProgressTemplate>
				<div class="disableBackground">
				</div>
				<div class="progressPane loading">
					<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
						Updating Trailer, Please Wait...
					</div>
					<asp:Image ID="imgGridProgress4" runat="server" ImageUrl="~/Images/animated_bar.gif" />
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>
		<asp:UpdateProgress ID="upBackhaulsProgress" runat="server" AssociatedUpdatePanelID="upBackhauls" DisplayAfter="100">
			<ProgressTemplate>
				<div class="disableBackground">
				</div>
				<div class="progressPane loading">
					<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
						Updating Backhaul, Please Wait...
					</div>
					<asp:Image ID="imgGridProgress5" runat="server" ImageUrl="~/Images/animated_bar.gif" />
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>





	</form>

</body>
</html>
