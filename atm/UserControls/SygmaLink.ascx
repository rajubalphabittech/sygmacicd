<%@ Control Language="c#" Inherits="UserControls_SygmaLink" Codebehind="SygmaLink.ascx.cs" %>
<%@ Register Src="DocumentIcon.ascx" TagName="DocumentIcon" TagPrefix="uc1" %>
<asp:HyperLink ID="hlSygmaImg" runat="server" Width="16" Height="18">
        <uc1:DocumentIcon ID="diIcon" runat="server" />
</asp:HyperLink><asp:Literal ID="litSpace" runat="server" Text="&nbsp;&nbsp;"></asp:Literal>
<asp:HyperLink ID="hlLink" runat="server" CssClass="main_content_link"></asp:HyperLink>
