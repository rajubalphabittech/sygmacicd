<%@ Page Title="Audit Report" MasterPageFile="~/Masters/ATM.master" Language="C#" AutoEventWireup="true" CodeBehind="AuditReport.aspx.cs" Inherits="atm.Apps.ATM.Reports.AuditReport" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript">

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>

	<asp:ValidationSummary ID="vsAuditReport" runat="server" ValidationGroup="Generate" class="alert alert-danger" />

	<div class="panel panel-default report-panel ">
		<div class="panel-title pageTitle">ATM Audit Report</div>
		<div class="panel-heading">Please select the report period</div>
		<div class="panel-body">
			<asp:UpdatePanel ID="upAuditReport" runat="server">
				<ContentTemplate>
					<asp:Panel ID="pnlpAuditReport" runat="server">

						<div class="form-group form-inline">
							<label for="lbSygmaCenterNo" class="col-sm-4">Access To</label>
							<asp:ListBox ID="lbSygmaCenterNo" runat="server" DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
							<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvCenter" runat="server"
								ControlToValidate="lbSygmaCenterNo" ValidationGroup="Generate"
								ErrorMessage="'Center' is required!">*</asp:RequiredFieldValidator>
						</div>
						
						<div class="form-group form-inline">
							<label for="ddPageNames" class="col-sm-4">Filter Pages<br/>(Optional)</label>
							<asp:ListBox ID="ddPageNames" runat="server" DataValueField="FunctionId" DataTextField="FunctionDescription" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
						</div>

						<div class="form-group form-inline">
							<label for="ddlEmployeeStatus" class="col-sm-4">Employee Status</label>
							<asp:DropDownList ID="ddlEmployeeStatus" runat="server" CssClass="form-control" AutoPostBack="True" >
							</asp:DropDownList>
						</div>

					</asp:Panel>
				</ContentTemplate>


			</asp:UpdatePanel>
			<asp:Panel ID="pnlGenerateButton" runat="server" Width="100%" HorizontalAlign="Center" Style="padding: 5px	0px 5px 0px">

				<asp:Button ID="btnGenerate" runat="server" Text="Generate"
					ValidationGroup="Generate" OnClick="btnGenerate_Click" CssClass="btn btn-primary" />
			</asp:Panel>
		</div>
	</div>
</asp:Content>


