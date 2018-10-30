<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UserControls_ChangeChecks" Codebehind="ChangeChecks.ascx.cs" %>
<%@ Register Src="JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<asp:CheckBox ID="chkCheckAll" runat="server" Visible="false"/>
<uc1:JScript ID="JScript1" runat="server" FileNames="UserControls/ChangeChecks.js" />
<span id="viewLinks" runat="server" visible="false">
    <asp:HyperLink ID="hlCheckAll" runat="server" Text="Check All" AccessKey="A" onfocus="if (navigator.appName == 'Microsoft Internet Explorer')this.click();" ></asp:HyperLink>&nbsp;
    <asp:HyperLink ID="hlUnCheckAll" runat="server" Text="Uncheck All" AccessKey="U" onfocus="if (navigator.appName == 'Microsoft Internet Explorer')this.click();" ></asp:HyperLink>
</span>
