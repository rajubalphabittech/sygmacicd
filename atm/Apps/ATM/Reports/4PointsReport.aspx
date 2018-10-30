<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Reports_4PointsReport" Codebehind="4PointsReport.aspx.cs" %>
<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="pageTitle inlineBlock" style="width: 75%">
    		<asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
		    4 Points Report
	    </div>
	
    <asp:ValidationSummary ID="vs4PointsReport" runat="server" ValidationGroup="Generate" />

    <div class="inputPanel" style="width: 50%;">
        <div class="header" style="width: 600px;">
            Please select the report period
        </div>
    
        <div class="inlineBlock" style="width: 240px; margin-right: 0px;">
    
            <asp:UpdatePanel ID="upInputs" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlInputs" runat="server" style="width: 350px; margin-right: 0px;">
                        <table>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtStartDate" ValidationGroup="Generate" runat="server" autocomplete="off" Width="81px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True" 
                                        TargetControlID="txtStartDate" Format="M/d/yyyy" PopupPosition="Right"></cc1:CalendarExtender>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" 
                                        ValidationGroup="Generate" ErrorMessage="'Start Date' is required !">*
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="cdvStartDate" runat="server" ControlToValidate="txtStartDate" 
                                        ErrorMessage="'Start Date' is invalid." Operator="DataTypeCheck"  ValidationGroup="Generate"
                                        Type="Date">*</asp:CompareValidator>
                                    <%--<asp:RangeValidator runat="server" ID="rngStartDate" ControlToValidate="txtStartDate" Type="Date" 
                                        Minimumvalue="01/01/1900" Maximumvalue="12/31/2200" 
                                        ErrorMessage="'Start Date' is invalid." ValidationGroup="Generate">*</asp:RangeValidator>--%>
                                    <%--<asp:RegularExpressionValidator CssClass="validator-message" ID="revStartDate" runat="server" ControlToValidate="txtStartDate"
                                        ValidationGroup="Generate" ErrorMessage="Please select a valid 'Start Date'."
                                        ValidationExpression="^(0?[1-9]|1[0-2])/(0?[1-9]|1[0-9]|2[0-9]|3[01])/\d{4}$">*
                                    </asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblEndDate" runat="server" Text="End Date"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtEndDate" ValidationGroup="Generate" runat="server" autocomplete="off" Width="81px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True" 
                                        TargetControlID="txtEndDate" Format="M/d/yyyy" PopupPosition="Right"></cc1:CalendarExtender>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" 
                                        ValidationGroup="Generate" ErrorMessage="'End Date' is required !">*
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="cdvEndDate" runat="server" ControlToValidate="txtEndDate" 
                                        ErrorMessage="'End Date' is invalid." Operator="DataTypeCheck"  ValidationGroup="Generate"
                                        Type="Date">*</asp:CompareValidator>
                                    <%--<asp:RangeValidator runat="server" id="rngEndDate" ControlToValidate="txtEndDate" Type="Date" 
                                        Minimumvalue="01/01/1900" Maximumvalue="12/31/2200" 
                                        ErrorMessage="'End Date' is invalid." ValidationGroup="Generate">*</asp:RangeValidator>--%>
                                    <%--<asp:RegularExpressionValidator CssClass="validator-message" ID="revEndDate" runat="server" ControlToValidate="txtEndDate"
                                        ValidationGroup="Generate" ErrorMessage="Please select a valid 'End Date'."
                                        ValidationExpression="^(0?[1-9]|1[0-2])/(0?[1-9]|1[0-9]|2[0-9]|3[01])/\d{4}$">*
                                    </asp:RegularExpressionValidator>--%>
                                    <asp:CompareValidator CssClass="validator-message" ID="cvEndDate" runat="server" ControlToCompare="txtStartDate" 
                                        ControlToValidate="txtEndDate" ErrorMessage="'End date' cannot be less than 'Start date'" 
                                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="Generate">*</asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="pnlGenerateButton" runat="server" Width="100%" HorizontalAlign="Center" Style="padding: 5px	0px 5px 0px">
			    <asp:Button ID="btnGenerate" runat="server" Text="Generate" ValidationGroup="Generate" onclick="btnGenerate_Click" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>

