<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Reports_DisplayEmployeePayroll" CodeBehind="DisplayEmployeePayroll.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>ATM - Display Employee Payroll Report</title>
	<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/AJAX.js" type="text/javascript"></script>
	<script type="text/javascript">

</script>
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

		a {
			text-decoration: none;
		}

			a:hover {
				text-decoration: underline;
			}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<div style="z-index: 1; width: 850px">
			<asp:Label ID="lblInvalidInput" runat="server" class="pageTitle inlineBlock" Text="Invalid Input!!! Please select the input properly!!!"></asp:Label>
			<asp:Label ID="lblNoRecords" runat="server" class="pageTitle inlineBlock" Text="No records found for the selected input!!!"></asp:Label>
			<asp:Repeater ID="rptEmployees" runat="server" OnItemDataBound="rptEmployees_ItemDataBound">
				<ItemTemplate>
					<div style="margin-bottom: 10px; width: 100%">
						<div class="pageTitle inlineBlock" style="width: 750px">
							<%# Eval("WebDisplay")%> -
							<asp:Label ID="lblEmployeeDates" runat="server"></asp:Label>
						</div>
						<div class="inlineBlock" style="width: 750px">
							<asp:Label ID="lblProgressionRate" Text="test" runat="server"></asp:Label>
						</div>
					</div>
					<div style="z-index: 1; width: 850px">

						<asp:Panel ID="pnlAssets" runat="server" Style="margin-top: 10px;" Visible="true">
							<div class="inlineBlock" style="vertical-align: top; width: 340px">
								<asp:Panel ID="pnlPayments" runat="server" Style="width: 100%;" CssClass="inputPanel">
									<div class="header">
										<div class="inlineBlock" style="width: 30%; vertical-align: top">
											Payments
										</div>
										<%--<div class="inlineBlock" style="width: 66%; text-align: right">
							<img id="toggleEmployees" class="toggle" entity="#employees" src="../../../../Images/Icons/hide.png" alt="hide" title="Hide Area" />
						</div>--%>
									</div>
									<div class="body" id="Routes">
										<asp:Label ID="lblEmptyPayments" runat="server" Text="No Payments have been added."></asp:Label>
										<asp:Repeater ID="rptRoutes" runat="server" OnItemDataBound="rptRoutes_ItemDataBound">
											<ItemTemplate>
												<div class="entityArea" style="width: 96%;">
													<div class="entityHeader">
														<div class="inlineBlock" style="width: 20px">
															<%#Container.ItemIndex + 1 %>.&nbsp;
														</div>
														<div class="inlineBlock" style="width: 100px">
															Route
														</div>
														<div class="inlineBlock" style="width: 173px">
															Depart Date
														</div>
														<div class="inlineBlock" style="width: 16px; text-align: right">
														</div>
													</div>
													<div class="entity">
														<div class="inlineBlock" style="width: 19px; vertical-align: top;">
														</div>
														<div class="inlineBlock" style="width: 100px; vertical-align: top;">
															<asp:Label runat="server" ID="lblRoute"></asp:Label>
														</div>
														<div class="inlineBlock" style="width: 176px; vertical-align: top; text-align: left">
															<asp:Label runat="server" ID="lblDepartDate"></asp:Label>
														</div>
													</div>
													<fieldset style="width: 310px; margin-bottom: 5px">
														<legend>Payments</legend>
														<asp:Panel ID="pnlEmptyPayments" runat="server" Style="margin-bottom: 3px; margin-top: 3px" Visible="false">
															No payments have been added.
														</asp:Panel>
														<asp:Repeater ID="rptEmployeePayments" runat="server" OnItemDataBound="rptEmployeePayments_ItemDataBound">
															<HeaderTemplate>
																<div class="entitySubHeader" style="margin-top: 5px; width: 303px">
																	<div class="inlineBlock" style="width: 85px">
																		Type
																	</div>
																	<div class="inlineBlock" style="width: 55px; text-align: right">
																		Quantity
																	</div>
																	<div class="inlineBlock" style="width: 70px; text-align: right">
																		Rate
																	</div>
																	<div class="inlineBlock" style="width: 78px; text-align: right">
																		Total
																	</div>
																</div>
															</HeaderTemplate>
															<ItemTemplate>
																<div>
																	<div class="inlineBlock" style="width: 85px" title='<%# Eval("PaymentNotes") %>'>
																		<asp:Label ID="lblRateTypeDesc" runat="server"></asp:Label>
																	</div>
																	<div class="inlineBlock" style="width: 55px; text-align: right">
																		<asp:Label ID="lblQty" runat="server" Style="text-align: right"></asp:Label>
																	</div>
																	<div class="inlineBlock" style="width: 70px; text-align: right">
																		$<asp:Label ID="lblRate" runat="server"></asp:Label>
																	</div>
																	<div class="inlineBlock" style="width: 78px; text-align: right">
																		$<asp:Label ID="lblTotal" runat="server"></asp:Label>
																	</div>
																	<div class="inlineBlock">
																	</div>
																</div>
																<%--<div id="dPaymentNotes" class="inlineBlock">--%>
																<%--&nbsp;&nbsp;&nbsp;--%>
																<asp:Label ID="lblPaymentNotes" runat="server"></asp:Label>
																<%--</div>--%>
															</ItemTemplate>
														</asp:Repeater>
														<asp:Panel ID="pnlGrandTotal" runat="server" Style="width: 288px; border-top: 1px dotted gray; text-align: right; margin-top: 5px; padding-top: 2px;">
															<div class="inputHeader inlineBlock" style="text-align: left; font-size: 11px;">
																Total
															</div>
															<div class="inputHeader inlineBlock" style="width: 91px; text-align: right; font-size: 11px;">
																$<asp:Label ID="lblEmployeeTotal" runat="server"></asp:Label>
															</div>
														</asp:Panel>
														<asp:Panel ID="pnlHolidayPay" runat="server" Style="width: 288px; text-align: left; margin-top: 5px; padding-top: 2px;">
															<div class="inputHeader inlineBlock" style="text-align: left;">
																* - Holiday Pay
															</div>
														</asp:Panel>
													</fieldset>
												</div>
											</ItemTemplate>
										</asp:Repeater>
									</div>
								</asp:Panel>
							</div>
							<div class="inlineBlock" style="vertical-align: top; width: 495px; margin-left: 10px">
								<div>
									<asp:Panel ID="pnlVehicles" runat="server" class="inputPanel" Style="width: 100%; vertical-align: top" Visible="true">
										<div class="header">
											<div class="inlineBlock" style="width: 30%; vertical-align: top">
												Vehicles
											</div>
											<%--<div class="inlineBlock" style="width: 67%; text-align: right">
								<img id="toggleVehicles" class="toggle" entity="#vehicles" src="../../../../Images/Icons/hide.png" alt="hide" title="Hide Area" />
							</div>--%>
										</div>
										<div class="body" id="vehicles">
											<div>
												<%--height: 150px; --%>
												<asp:Label ID="lblEmptyVehicles" runat="server" Text="No Vehicles have been added."></asp:Label>
												<asp:Repeater ID="rptEmployeeVehicles" runat="server" OnItemDataBound="rptEmployeeVehicles_ItemDataBound">
													<ItemTemplate>
														<div class="entityArea" style="width: 96%;">
															<div class="entityHeader">
																<div class="inlineBlock" style="width: 20px">
																	<%#Container.ItemIndex + 1 %>.&nbsp;
																</div>
																<div class="inlineBlock" style="width: 100px">
																	Route
																</div>
																<div class="inlineBlock" style="width: 176px; text-align: left;">
																	Depart Date
																</div>
															</div>
															<div class="entity">
																<div class="inlineBlock" style="width: 19px; vertical-align: top;">
																</div>
																<div class="inlineBlock" style="width: 100px; vertical-align: top;">
																	<asp:Label runat="server" ID="lblRoute"></asp:Label>
																</div>
																<div class="inlineBlock" style="width: 176px; vertical-align: top; text-align: left;">
																	<asp:Label runat="server" ID="lblDepartDate"></asp:Label>
																</div>
															</div>
															<fieldset style="width: 465px; margin-bottom: 5px">
																<legend>Route Vehicles</legend>
																<asp:Label ID="lblEmptyVehicles" runat="server" Text="No vehicles have been added."></asp:Label>
																<asp:Repeater ID="rptVehicles" runat="server" OnItemDataBound="rptVehicles_ItemDataBound">
																	<ItemTemplate>
																		<div class="entityArea" style="width: 97%;">
																			<div class="entitySubHeader" style="margin-top: 5px; width: 460px">
																				<div class="inlineBlock" style="width: 120px;">
																					<%#Container.ItemIndex + 1 %>.&nbsp;Vehicle #
																				</div>
																				<div class="inlineBlock" style="width: 110px">
																					Begin Odometer
																				</div>
																				<div class="inlineBlock" style="width: 110px">
																					End Odometer
																				</div>
																				<div class="inlineBlock" style="width: 105px">
																					Total Miles
																				</div>
																			</div>
																			<div class="entity">
																				<div class="inlineBlock" style="width: 115px;">
																					<%# Eval("WebDisplay") %>
																				</div>
																				<div class="inlineBlock" style="width: 110px">
																					<%# Eval("BeginOdometer", "{0:#,##0.##}") %>
																				</div>
																				<div class="inlineBlock" style="width: 110px">
																					<%# Eval("EndOdometer", "{0:#,##0.##}") %>
																				</div>
																				<div class="inlineBlock" style="width: 105px">
																					<%# Eval("TotalMiles", "{0:#,##0.##}")%>
																				</div>
																			</div>
																			<div>
																			</div>
																		</div>
																	</ItemTemplate>
																</asp:Repeater>
															</fieldset>
														</div>
													</ItemTemplate>
												</asp:Repeater>
											</div>
										</div>
									</asp:Panel>
								</div>
							</div>
						</asp:Panel>
						<asp:Panel ID="pnlTotal" runat="server" Style="margin-top: 10px;" Visible="true">
							<div class="inlineBlock" style="vertical-align: top; width: 340px">
								<asp:Panel ID="pnlGrandTotal" runat="server" Style="width: 100%;" CssClass="inputPanel">
									<div class="header">
										<div class="inlineBlock" style="width: 30%; vertical-align: top">
											<asp:Label ID="lblTotalHeader" runat="server" Text="Total"></asp:Label>
										</div>
										<%--<div class="inlineBlock" style="width: 66%; text-align: right">
							<img id="toggleEmployees" class="toggle" entity="#employees" src="../../../../Images/Icons/hide.png" alt="hide" title="Hide Area" />
						</div>--%>
									</div>
									<div class="body" id="Div1">
										<asp:Panel ID="pnlEmptyComponentPay" runat="server" Style="margin-bottom: 3px; margin-top: 3px">
											No component pay have been added
										</asp:Panel>
										<asp:Repeater ID="rptTotal" runat="server" OnItemDataBound="rptTotal_ItemDataBound">
											<HeaderTemplate>
												<div class="entitySubHeader" style="margin-top: 5px; width: 330px">
													<div class="inlineBlock" style="width: 155px">
														Type
													</div>
													<div class="inlineBlock" style="width: 80px; text-align: right">
														Quantity
													</div>
													<div class="inlineBlock" style="width: 80px; text-align: right">
														Total
													</div>
												</div>
											</HeaderTemplate>
											<ItemTemplate>
												<div>
													<div class="inlineBlock" style="width: 155px">
														<asp:Label ID="lblRateTypeDesc" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 80px; text-align: right">
														<asp:Label ID="lblQty" runat="server" Style="text-align: right"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 80px; text-align: right">
														$<asp:Label ID="lblTotal" runat="server"></asp:Label>
													</div>
												</div>
												<asp:Label ID="lblPaymentNotes" runat="server"></asp:Label>
											</ItemTemplate>
										</asp:Repeater>
										<asp:Panel ID="pnlEmployeeGrandTotal" runat="server" Visible="false" Style="width: 315px; border-top: 1px dotted gray; text-align: right; margin-top: 5px; padding-top: 2px;">
											<div class="inputHeader inlineBlock" style="text-align: left; font-size: 11px;">
												Total
											</div>
											<div class="inputHeader inlineBlock" style="width: 91px; text-align: right; font-size: 11px;">
												$<asp:Label ID="lblEmployeeGrandTotal" runat="server"></asp:Label>
											</div>
										</asp:Panel>
										<asp:Panel ID="pnlTotalHolidayPay" runat="server" Style="width: 315px; text-align: left; margin-top: 5px; padding-top: 2px;">
											<div class="inputHeader inlineBlock" style="text-align: left;">
												* - Holiday Pay
											</div>
										</asp:Panel>
									</div>
								</asp:Panel>
							</div>
						</asp:Panel>
						<asp:Panel ID="pnlEmployeeTimelog" runat="server" Style="margin-top: 10px;" Visible="false">
							<div class="inlineBlock" style="vertical-align: top; width: 340px">
								<asp:Panel ID="pnlTimelog" runat="server" Style="width: 100%;" CssClass="inputPanel">
									<div class="header">
										<div class="inlineBlock" style="width: 30%; vertical-align: top">
											Elapsed Hours
										</div>
										<%--<div class="inlineBlock" style="width: 66%; text-align: right">
							<img id="toggleEmployees" class="toggle" entity="#employees" src="../../../../Images/Icons/hide.png" alt="hide" title="Hide Area" />
						</div>--%>
									</div>
									<div class="body" id="Div5">
										<asp:Panel ID="pnlEmptyEmployeesTimeLogs" runat="server" Style="margin-bottom: 3px; margin-top: 3px">
											No time logs have been added
										</asp:Panel>
										<asp:Repeater ID="rptEmployeesTimeLogs" runat="server" OnItemDataBound="rptEmployeesTimeLogs_ItemDataBound">
											<HeaderTemplate>
												<div style="margin-top: 5px; width: 330px" class="entitySubHeader">
													<div class="inlineBlock" style="width: 44px">
														Route
													</div>
													<div class="inlineBlock" style="width: 118px">
														Start
													</div>
													<div class="inlineBlock" style="width: 118px">
														End
													</div>
													<div class="inlineBlock" style="width: 35px; text-align: right">
														Total
													</div>
												</div>
											</HeaderTemplate>
											<ItemTemplate>
												<div style="margin-top: 2px">
													<div class="inlineBlock" style="width: 44px">
														<asp:Label ID="lblRoute" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 118px">
														<asp:Label ID="lblStart" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 118px">
														<asp:Label ID="lblEnd" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 35px; text-align: right">
														<asp:Label ID="lblHours" runat="server"></asp:Label>
													</div>
												</div>
											</ItemTemplate>
										</asp:Repeater>
										<asp:Panel ID="pnlTimelogTotal" runat="server" Visible="false" Style="width: 315px; border-top: 1px dotted gray; text-align: right; margin-top: 5px; padding-top: 2px;">
											<div class="inputHeader inlineBlock" style="text-align: left; font-size: 11px;">
												Total Hours
											</div>
											<div class="inputHeader inlineBlock" style="width: 91px; text-align: right; font-size: 11px;">
												<asp:Label ID="lblTotalHours" runat="server"></asp:Label>
											</div>
										</asp:Panel>
									</div>
								</asp:Panel>
							</div>
						</asp:Panel>
						<asp:Panel ID="pnlComponentHourly" runat="server" Style="margin-top: 10px;" Visible="false">
							<div class="inlineBlock" style="vertical-align: top; width: 340px">
								<asp:Panel ID="pnlComponent" runat="server" Style="width: 100%;" CssClass="inputPanel">
									<div class="header">
										<div class="inlineBlock" style="width: 100%; vertical-align: top">
											Component Pay True-up Calculation
										</div>
										<%--<div class="inlineBlock" style="width: 66%; text-align: right">
							<img id="toggleEmployees" class="toggle" entity="#employees" src="../../../../Images/Icons/hide.png" alt="hide" title="Hide Area" />
						</div>--%>
									</div>
									<div class="body" id="Div3">
										<asp:Repeater ID="rptComponent" runat="server" OnItemDataBound="rptComponent_ItemDataBound">
											<HeaderTemplate>
												<div class="entitySubHeader" style="margin-top: 5px; width: 330px">
													<div class="inlineBlock" style="width: 145px">
														Type
													</div>
													<div class="inlineBlock" style="width: 40px; text-align: right">
														Hours
													</div>
													<div class="inlineBlock" style="width: 50px; text-align: right">
														Rate
													</div>
													<div class="inlineBlock" style="width: 80px; text-align: right">
														Total
													</div>
												</div>
											</HeaderTemplate>
											<ItemTemplate>
												<div>
													<div class="inlineBlock" style="width: 145px">
														<asp:Label ID="lblType" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 40px; text-align: right">
														<asp:Label ID="lblHours" runat="server" Style="text-align: right"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 50px; text-align: right">
														<asp:Label ID="lblRate" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 80px; text-align: right">
														<asp:Label ID="lblTotal" runat="server"></asp:Label>
													</div>
												</div>
											</ItemTemplate>
										</asp:Repeater>
										<asp:Panel ID="pnlCompTrueUp" runat="server" Style="width: 315px; border-top: 1px dotted gray; text-align: right; margin-top: 5px; padding-top: 2px;">
											<div class="inputHeader inlineBlock" style="text-align: left; font-size: 11px;">
												<asp:Label ID="lblComponentPayTrueUp" runat="server" Text="Component Pay True-up"></asp:Label>
											</div>
											<div class="inputHeader inlineBlock" style="width: 91px; text-align: right; font-size: 11px;">
												$<asp:Label ID="lblCompTrueUp" runat="server"></asp:Label>
											</div>
										</asp:Panel>
										<asp:Panel ID="pnlNegCompTrueUp" runat="server" Style="width: 315px;" Visible="false">
											<asp:Label ID="lblNegCompMsg" runat="server"></asp:Label>
										</asp:Panel>
									</div>
								</asp:Panel>
							</div>
						</asp:Panel>
						<asp:Panel ID="pnlPayStubPreview" runat="server" Style="margin-top: 10px;" Visible="false">
							<div class="inlineBlock" style="vertical-align: top; width: 340px">
								<asp:Panel ID="pnlPayStub" runat="server" Style="width: 100%;" CssClass="inputPanel">
									<div class="header">
										<div class="inlineBlock" style="width: 100%; vertical-align: top">
											Pay Stub Preview 
										</div>
										<%--<div class="inlineBlock" style="width: 66%; text-align: right">
							<img id="toggleEmployees" class="toggle" entity="#employees" src="../../../../Images/Icons/hide.png" alt="hide" title="Hide Area" />
						</div>--%>
									</div>
									<div class="body" id="Div4">
										<asp:Repeater ID="rptPayStub" runat="server" OnItemDataBound="rptPayStub_ItemDataBound">
											<HeaderTemplate>
												<div class="entitySubHeader" style="margin-top: 5px; width: 330px">
													<div class="inlineBlock" style="width: 145px">
														Type
													</div>
													<div class="inlineBlock" style="width: 40px; text-align: right">
														Hours
													</div>
													<div class="inlineBlock" style="width: 70px; text-align: right">
														Rate
													</div>
													<div class="inlineBlock" style="width: 60px; text-align: right">
														Total
													</div>
												</div>
											</HeaderTemplate>
											<ItemTemplate>
												<div>
													<div class="inlineBlock" style="width: 145px">
														<asp:Label ID="lblType" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 40px; text-align: right">
														<asp:Label ID="lblHours" runat="server" Style="text-align: right"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 70px; text-align: right">
														<asp:Label ID="lblRate" runat="server"></asp:Label>
													</div>
													<div class="inlineBlock" style="width: 60px; text-align: right">
														<asp:Label ID="lblTotal" runat="server"></asp:Label>
													</div>
												</div>
											</ItemTemplate>
										</asp:Repeater>
										<asp:Panel ID="pnlPayStubTotal" runat="server" Style="width: 317px; border-top: 1px dotted gray; text-align: right; margin-top: 5px; padding-top: 2px;">
											<div class="inputHeader inlineBlock" style="text-align: left; font-size: 11px;">
												Total
											</div>
											<div class="inputHeader inlineBlock" style="width: 91px; text-align: right; font-size: 11px;">
												$<asp:Label ID="lblPayStubTotal" runat="server"></asp:Label>
											</div>
										</asp:Panel>
									</div>
								</asp:Panel>
							</div>
						</asp:Panel>
						<asp:Panel ID="pnlWarning" runat="server" Style="margin-top: 10px;" Visible="false">
							<div class="inlineBlock" style="vertical-align: top; width: 340px">
								<asp:Panel ID="Panel2" runat="server" Style="width: 100%;" CssClass="inputPanel">
									<div class="header">
										<div class="inlineBlock" style="width: 100%; vertical-align: top">
											Warning 
										</div>
									</div>
									<div class="body" id="Div7">
										<div class="inlineBlock" style="text-align: left; font-size: 11px;">
											Hourly Pay is greater than Component Pay. Please run Send to ADP Report and try again!!!
										</div>
									</div>
								</asp:Panel>
							</div>
						</asp:Panel>
					</div>
				</ItemTemplate>
				<SeparatorTemplate>
					<div id="div6" runat="server" style="page-break-before: always">
						&nbsp;
					</div>
				</SeparatorTemplate>
			</asp:Repeater>
		</div>
	</form>

</body>
</html>
