<%@ Control Language="c#" Inherits="UserControls_ADUserDropDown" Codebehind="ADUserDropDown.ascx.cs" %>
<div ID="lblDDName" Visible="False" runat="server"></div><asp:DropDownList ID="ddUsers" runat="server"
  OnSelectedIndexChanged="ddUsers_SelectedIndexChanged">
</asp:DropDownList>
<asp:Label ID="lblDisplayName" runat="server"></asp:Label>
<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvUsers" Visible="False" runat="server" ErrorMessage="Name is Required." EnableClientScript="False"
  Display="Dynamic" ControlToValidate="ddUsers">*</asp:RequiredFieldValidator>
