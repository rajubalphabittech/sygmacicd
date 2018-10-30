<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ItemSearch" Codebehind="ItemSearch.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:TextBox ID="txtItem" runat="server" Width="200px" AutoComplete="off" MaxLength="200" OnTextChanged="txtItem_TextChanged"></asp:TextBox>
<cc1:AutoCompleteExtender ID="txtItem_AutoCompleteExtender" runat="server" Enabled="True" ServicePath="~/WebServices/Items.asmx" ServiceMethod="GetItemList" TargetControlID="txtItem"
  UseContextKey="True" MinimumPrefixLength="2" CompletionSetCount="20" CompletionInterval="100">
</cc1:AutoCompleteExtender>
<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvItem" runat="server" EnableClientScript="false" ErrorMessage="Must supply a valid item number" ControlToValidate="txtItem" Text="*" Visible="false"></asp:RequiredFieldValidator>
