<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_IndexList" Codebehind="IndexList.ascx.cs" %>
<%@ Register Src="LimitedAccess.ascx" TagName="LimitedAccess" TagPrefix="uc1" %>
<table cellspacing="2" cellpadding="2" border="0" align="left">
    <tr>
        <td>
            <asp:SiteMapDataSource ID="siteMap" runat="server" />
            <asp:TreeView ID="tvList" runat="server" DataSourceID="siteMap" ShowExpandCollapse="false"
                Font-Size="12px" NodeIndent="8" OnDataBound="tvList_DataBound">
                <RootNodeStyle CssClass="main_content_header" ChildNodesPadding="2" ForeColor="#000000" />
                <ParentNodeStyle CssClass="main_content_subheader" ChildNodesPadding="2" NodeSpacing="4"/>
                <LeafNodeStyle CssClass="main_content_link" Font-Size="12px" ImageUrl="~/images/sygma_small.gif"
                    VerticalPadding="6" HorizontalPadding="4" />
            </asp:TreeView>
           
        </td>
    </tr>
    <tr id="trLimitedAccess" runat="server">
        <td>
            <uc1:LimitedAccess ID="LimitedAccess1" runat="server" />
        </td>
    </tr>
</table>