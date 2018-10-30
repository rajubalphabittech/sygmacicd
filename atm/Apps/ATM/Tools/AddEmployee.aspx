<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Tools_AddEmployee" CodeBehind="AddEmployee.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>ATM - Add Employee</title>
	<script type="text/javascript" src="/Scripts/jquery-1.9.1.min.js"></script>
	<link rel="stylesheet" href="/Content/bootstrap.min.css" />
	<script type="text/javascript" src="/Scripts/bootstrap.min.js"></script>
</head>
<body>
	<div class="container-fluid">
		<form id="form1" runat="server">
			<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
			</asp:ScriptManager>
			<div class="pageTitle" style="width: 100%">
				Add Employee
			</div>
			<div class="container-fluid">
				<div class="col-sm-5 col-md-4 col-lg-3">
					<div class="panel panel-default">
						<div class="panel-heading">
							Select a Center and Employee
						</div>
						<div class="panel-body">
							<div>
								<asp:UpdatePanel ID="upEmployee" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
									<Triggers>
										<asp:AsyncPostBackTrigger ControlID="ddSygmaCenterNo" EventName="SelectedIndexChanged" />
									</Triggers>
									<ContentTemplate>
										<div class="form-group">
											<label for="ddSygmaCenterNo">
												Center
											</label>
											<asp:DropDownList ID="ddSygmaCenterNo" CssClass="form-control" runat="server" DataTextField="CenterDisplay"
												DataValueField="SygmaCenterNo" AutoPostBack="True" OnSelectedIndexChanged="ddSygmaCenterNo_SelectedIndexChanged">
											</asp:DropDownList>
											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvSygmaCenterNo" runat="server" ControlToValidate="ddSygmaCenterNo"
												ValidationGroup="add" InitialValue="-1" Text="'Center' is required!"></asp:RequiredFieldValidator>
										</div>
										<div class="form-group">
											<label for="ddEmployee">
												Employee
											</label>
											<asp:DropDownList ID="ddEmployee" CssClass="form-control" runat="server" DataTextField="WebDisplay" DataValueField="File_nbr">
											</asp:DropDownList>
											<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvEmployee" runat="server" ControlToValidate="ddEmployee"
												ValidationGroup="add" InitialValue="-1" Text="'Employee' is required!"></asp:RequiredFieldValidator>
										</div>
									</ContentTemplate>
								</asp:UpdatePanel>

								<div>
									<asp:Button ID="btnAdd" CssClass="btn btn-primary" runat="server" Text="Add" ValidationGroup="add" OnClick="btnAdd_Click" />
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</form>
	</div>
</body>
</html>
