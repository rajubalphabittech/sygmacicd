<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_LimitedTextBox" Codebehind="LimitedTextBox.ascx.cs" %>
<asp:Panel ID="pnlMain" runat="server">
	<asp:Panel ID="pnlHeader" runat="server" Style="position: relative; display: block; vertical-align: middle" Height="17px">
		<asp:Panel ID="pnlName" runat="server" CssClass="inlineBlock" HorizontalAlign="Left" Width="69%" Style="vertical-align: top">
			<asp:Label ID="lblName" runat="server"></asp:Label>
		</asp:Panel>
		<asp:Panel ID="pnlCount" runat="server" CssClass="inlineBlock" HorizontalAlign="Right" Width="27%" Style="vertical-align: top;">
			(<asp:Label ID="lblCount" runat="server" Font-Bold="true"></asp:Label>
			<span style="font-weight: normal; font-size: 9px">left</span>)
		</asp:Panel>
	</asp:Panel>
	<asp:Panel ID="pnlBody" runat="server">
		<asp:TextBox ID="txtText" runat="server" TextMode="MultiLine" OnTextChanged="txtText_TextChanged"></asp:TextBox>
	</asp:Panel>
</asp:Panel>
