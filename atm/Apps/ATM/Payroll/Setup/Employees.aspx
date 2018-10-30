<%@ Page Title="Employees" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true"
	Inherits="Apps_ATM_Payroll_Setup_Employees" Codebehind="Employees.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<style type="text/css">
		div legend
		{
			font-weight: bold;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
	<div class="pageTitle" style="margin-bottom: 10px">
		Employees
	</div>
	<div class="inputPanel" style="width: 500px; height: 500px">
		<div class="header">
			Employee
		</div>
		<div class="body">
			<asp:DropDownList ID="ddEmployee" runat="server" DataTextField="WebDisplay" DataValueField="EmployeeId" AutoPostBack="true" 
				onselectedindexchanged="ddEmployee_SelectedIndexChanged">
			</asp:DropDownList>
			<asp:UpdatePanel ID="upSettings" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="ddEmployee" EventName="SelectedIndexChanged" />
				</Triggers>
				<ContentTemplate>
					<asp:Panel ID="pnlPayroll" runat="server" GroupingText="Payroll" Visible="false">
						<div class="inputHeader">
							Progression Rate
						</div>
						<asp:TextBox ID="txtProgressionRate" runat="server" Width=""></asp:TextBox>
					</asp:Panel>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
	</div>
</asp:Content>
