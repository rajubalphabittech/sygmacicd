<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Reports_EmployeePointsHistoryReport" Codebehind="EmployeePointsHistoryReport.aspx.cs" %>
<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="pageTitle inlineBlock" style="width: 75%">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        Employee's Points History Report
    </div>
    
    <asp:ValidationSummary ID="vsEmployeePointsHistoryReport" runat="server" ValidationGroup="Generate" />
    
    <div class="inputPanel" style="width: 50%;">
        <div class="header" style="width: 600px;">
            Please enter the Employee ID
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
                                        <asp:Label ID="lblEmployeeID" runat="server" Text="Employee ID"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtEmplID" runat="server" DataTextField="EmplID" DataValueField="EmplID" 
                                        AutoPostBack="True" OnTextChanged="txtEmplID_TextChanged" ></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="txtEmplID_AutoCompleteExtender" runat="server" Enabled="True" 
                                        ServicePath="~/Apps/ATM/WebServices/FileNBR.asmx" ServiceMethod="GetFileNBR" 
                                        UseContextKey="True" TargetControlID="txtEmplID" MinimumPrefixLength="2" 
                                        CompletionSetCount="20" CompletionInterval="100">
                                    </cc1:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="txtEmplID_Validator" runat="server" ControlToValidate="txtEmplID" 
                                        ValidationGroup="Generate" ErrorMessage="'Employee ID' is a required field !">*
                                    </asp:RequiredFieldValidator>
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

