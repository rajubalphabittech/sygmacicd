<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_VehicleTrailerRowCountBar" Codebehind="VehicleTrailerRowCountBar.ascx.cs" %>
<table id="tblMain" runat="server">
  <tr>
    <td style="text-align: left">
        <span class="inputHeader">Vehicles</span>(<asp:Label ID="lblVehiclesCount" runat="server"></asp:Label>)
        <span class="inputHeader">Trailers</span>(<asp:Label ID="lblTrailersCount" runat="server"></asp:Label>)
        <asp:Label ID="lblItemCount" runat="server" Visible="false"></asp:Label>
    </td>
    <td style="text-align: right">
      <span class="inputHeader">Rows Per Page: </span>
      <asp:DropDownList ID="ddPageSize" runat="server" AutoPostBack="true" onselectedindexchanged="ddPageSize_SelectedIndexChanged">
        <asp:ListItem Value="50"></asp:ListItem>
        <asp:ListItem Value="100"></asp:ListItem>
      </asp:DropDownList>
    </td>
  </tr>
</table>
