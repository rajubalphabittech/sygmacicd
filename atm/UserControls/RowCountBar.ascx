<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_RowCountBar" Codebehind="RowCountBar.ascx.cs" %>
<table id="tblMain" runat="server">
  <tr>
    <td style="text-align: left">
      <span class="inputHeader">Row Count: </span>
      <asp:Label ID="lblItemCount" runat="server"></asp:Label>
    </td>
    <td style="text-align: right">
      <span class="inputHeader">Rows Per Page: </span>
      <asp:DropDownList ID="ddPageSize" runat="server" AutoPostBack="true" onselectedindexchanged="ddPageSize_SelectedIndexChanged">
        <asp:ListItem Value="25"></asp:ListItem>
        <asp:ListItem Value="50"></asp:ListItem>
        <asp:ListItem Value="100"></asp:ListItem>
      </asp:DropDownList>
    </td>
  </tr>
</table>
