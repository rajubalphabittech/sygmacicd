<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Payroll_Setup_PayRatesPreview" CodeBehind="PayRatesPreview.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link href="/Content/bootstrap.min.css" rel="stylesheet" />
	<script src="/Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
	<script src="/Scripts/bootstrap.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/AJAX.js" type="text/javascript"></script>

	<title>ATM - Employee Pay Rates Preview</title>
</head>
<body>
	<div class="container">
		<form id="form1" runat="server">
			<div class="pageTitle">
				Employee Pay Rates Preview
			</div>
			<div class="row">
				<div class="panel panel-default">
					<div class="panel-heading">
						<div class="row form-row">
							<div class="col-lg-4 text-left">
								<h3 class="panel-title">
									<asp:Label ID="lblEmployeeName" runat="server"></asp:Label></h3>
							</div>
							<div class="col-lg-4 text-center">
								<h3 class="panel-title">
									<asp:Label ID="lblToday" runat="server"></asp:Label>
								</h3>
							</div>
							<div class="col-lg-4 text-right">
								<h3 class="panel-title">
									<asp:Label ID="lblProgRateApplied" runat="server"></asp:Label>
								</h3>
							</div>
						</div>
					</div>
					<div class="row">
						<div class="panel-body table-responsive">
							<asp:Panel ID="pnlEmployeeDetail" CssClass="table-responsive" runat="server" Visible="false">
								<table cellspacing="0" class="table table-bordered table-striped table-hover" rules="all">
									<tr>
										<th scope="col" class="text-center" style="vertical-align: middle;">Pay Scales
										</th>
										<asp:Repeater ID="rptRateTypeHeader" runat="server">
											<ItemTemplate>
												<th scope="col" class="text-center" style="vertical-align: middle;">
													<%# Eval("RateTypeDescription") %>
												</th>
											</ItemTemplate>
										</asp:Repeater>
									</tr>
									<asp:Repeater ID="rptPayScales" runat="server" OnItemDataBound="rptPayScales_ItemDataBound" DataMember="PayScales">
										<ItemTemplate>
											<tr>
												<td>
													<div>
														<%# Eval("PayScaleDescription") %>
													</div>
												</td>
												<asp:Repeater ID="rptRates" runat="server" OnItemDataBound="rptRates_ItemDataBound">
													<ItemTemplate>
														<td style="text-align: center">
															<asp:Label ID="lblRate" runat="server" Style="cursor: default"></asp:Label>
														</td>
													</ItemTemplate>
												</asp:Repeater>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</table>

							</asp:Panel>
						</div>
					</div>
					<div class="row">
						<div class="col-lg-3">
							<asp:Panel ID="lblNoPayScales" runat="server" Visible="false">
								No pay scales have been configured for this employee's center.
							</asp:Panel>
							<asp:Panel ID="pnlNoEmployee" runat="server" Visible="false">
								No employee exists for this employee id.
							</asp:Panel>
						</div>
					</div>
				</div>
			</div>
		</form>
	</div>
</body>
</html>
