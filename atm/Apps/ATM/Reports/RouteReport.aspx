<%@ Page Title="Route Report" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Reports_RouteReport" Codebehind="RouteReport.aspx.cs" %>
<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:ValidationSummary ID="vsDirectorReport" runat="server" ValidationGroup="Generate" class="alert alert-danger"/>

<div class="panel panel-default report-panel">
        <div class="panel-title pageTitle">Route Report</div>
        <div class="panel-heading">Please select the report period</div>
        <div class="panel-body">
            <asp:UpdatePanel ID="upTotalFuelReport" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlpTotalFuelReport" runat="server" >

                        <div class="form-group form-inline">
                            <label for="lbSygmaCenterNo" class="col-sm-4">Center</label>
                            <asp:ListBox ID="lbSygmaCenterNo" runat="server" DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                            <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvCenter" runat="server"
                                                        ControlToValidate="lbSygmaCenterNo" ValidationGroup="Generate"
                                                        ErrorMessage="'Center' is required !">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-inline">
                            <label for="ddlDateRange" class="col-sm-4">Date Range</label>
                            <asp:DropDownList ID="ddlDateRange" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group form-inline">
                            <label for="txtStartDate" class="col-sm-4">Start Date</label>
                            <asp:TextBox ID="txtStartDate" ValidationGroup="Generate" runat="server"
                                         CssClass="form-control"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtStartDate" Format="M/d/yyyy" PopupPosition="Right">
                            </cc1:CalendarExtender>
                        </div>

                        <div class="form-group form-inline">
                            <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvStartDate" ValidationGroup="Generate"
                                ControlToValidate="txtStartDate" runat="server" Display="dynamic"
                                ErrorMessage="'Start Date' is required" class="reportsValidatorIndent"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group form-inline">
                            <asp:RangeValidator CssClass="validator-message" ID="rngStartDate" ValidationGroup="Generate" ControlToValidate="txtStartDate" runat="server"
                                MinimumValue="1/1/1900" MaximumValue="6/5/2079" SetFocusOnError="true" Type="date" Display="dynamic"
                                EnableClientScript="True" Enabled="True" class="reportsValidatorIndent" ErrorMessage="'Start Date' value must be between 1/1/1900 and 6/5/2079"></asp:RangeValidator>
                        </div>

                        <div class="form-group form-inline">
                            <label for="txtEndDate" class="col-sm-4">End Date</label>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtEndDate" Format="M/d/yyyy" PopupPosition="Right">
                            </cc1:CalendarExtender>
                        </div>

                        <div class="form-group form-inline">
                            <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvEndDate" ValidationGroup="Generate"
                                ControlToValidate="txtEndDate" runat="server" Display="dynamic"
                                ErrorMessage="'End Date' is required" class="reportsValidatorIndent"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group form-inline">
                            <asp:RangeValidator CssClass="validator-message" ID="rngEndDate" ValidationGroup="Generate" ControlToValidate="txtEndDate" runat="server"
                                MinimumValue="1/2/1900" MaximumValue="6/6/2079" SetFocusOnError="true" Type="date" Display="dynamic"
                                EnableClientScript="True" Enabled="True" class="reportsValidatorIndent" ErrorMessage="'End Date' value must be between 1/2/1900 and 6/6/2079"></asp:RangeValidator>
                        </div>
                        
                        <div class="form-group form-inline">
                            <asp:CompareValidator CssClass="validator-message" ID="cvEndDate" runat="server"
                                ControlToCompare="txtStartDate" ControlToValidate="txtEndDate" Display="dynamic"
                                ErrorMessage="'End Date' cannot be earlier than the 'Start Date'" class="reportsValidatorIndent"  Operator="GreaterThanEqual"
                                Type="Date" ValidationGroup="Generate"></asp:CompareValidator>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="pnlGenerateButton" runat="server" Width="100%" HorizontalAlign="Center" Style="padding: 5px	0px 5px 0px">
								
                <asp:Button ID="btnGenerate" runat="server" Text="Generate" 
                            ValidationGroup="Generate" onclick="btnGenerate_Click" CssClass="btn btn-primary" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>

