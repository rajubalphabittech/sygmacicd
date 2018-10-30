<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_LocationList" Codebehind="LocationList.ascx.cs" %>
<asp:DropDownList ID="ddLocation" runat="server" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged">
    </asp:DropDownList>
<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvLocation" runat="server" Text="*" EnableClientScript="false" Visible="false" ControlToValidate="ddLocation"></asp:RequiredFieldValidator>
