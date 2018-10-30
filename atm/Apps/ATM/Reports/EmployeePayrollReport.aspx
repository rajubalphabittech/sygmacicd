<%@ Page Title="Employee Payroll Report" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="True" Inherits="Apps_ATM_Reports_EmployeePayrollReport" Codebehind="EmployeePayrollReport.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../../Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/AJAX.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SetWeekendingDate(c) {
            var val = c.val();
            var dd = new Date(val);
            if (dd.isValid()) {
                var di = dd.getDay();
                if (di != 6)
                    dd.setDate(dd.getDate() + (6 - di));
                //c.value = dd.format("MM/dd/yyyy");
                document.getElementById('<%=txtStartDate.ClientID%>').value = dd.format("MM/dd/yyyy");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:ValidationSummary ID="vsEmployeePayroll" runat="server" BorderColorValidationGroup="Generate" class="alert alert-danger"/>


<div class="panel panel-default report-panel">
        <div class="panel-title pageTitle">Employee Payroll Report</div>
        <div class="panel-heading">Please select the report period</div>
        <div class="panel-body">

            <asp:UpdatePanel ID="upEmployeePayroll" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlEmployeePayroll" runat="server">

                        <div class="form-group form-inline">
                        </div>

                        <div class="form-group form-inline">
                            <label for="ddSygmaCenterNo" class="col-sm-4">Center</label>
                            <asp:DropDownList ID="ddSygmaCenterNo" runat="server" DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" AutoPostBack="true" OnSelectedIndexChanged="ddSygmaCenterNo_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvCenter" runat="server"
                                ControlToValidate="ddSygmaCenterNo" ValidationGroup="Generate"
                                ErrorMessage="'Center' is required !">*</asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group form-inline">
                            <label for="lbEmployees" class="col-sm-4">Employees</label>
                            <asp:ListBox ID="lbEmployees" runat="server" DataValueField="EmployeeId" DataTextField="WebDisplay" Style="height: 250px;" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                            <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvEmployees" runat="server" ControlToValidate="lbEmployees" ErrorMessage="'Employee' is required!"
                                EnableClientScript="true" Text="*" ValidationGroup="Generate" Display="Dynamic"></asp:RequiredFieldValidator>

                        </div>

                        <div class="form-group form-inline">
                            <label for="ddRangeBy" class="col-sm-4">Date Range By</label>

                            <asp:DropDownList ID="ddRangeBy" runat="server" DataValueField="RangeByValue" DataTextField="RangeByDisplay" CssClass="form-control"></asp:DropDownList>

                        </div>

                        <div class="form-group form-inline">
                            <label for="txtStartDate" class="col-sm-4">Start Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" ValidationGroup="Generate" CssClass="form-control"></asp:TextBox>
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
                            <asp:TextBox ID="txtEndDate" ValidationGroup="Generate" runat="server" CssClass="form-control"></asp:TextBox>
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
                            <asp:CompareValidator CssClass="validator-message" ID="CompareValidator1" runat="server"
                                ControlToCompare="txtStartDate" ControlToValidate="txtEndDate" Display="dynamic"
                                ErrorMessage="'End Date' cannot be earlier than the 'Start Date'" class="reportsValidatorIndent"  Operator="GreaterThanEqual"
                                Type="Date" ValidationGroup="Generate"></asp:CompareValidator>
                        </div>


                        <div class="form-group form-inline">
                            <label for="cbGroupByWeekend">Group by Weekending Date</label>
                            <asp:CheckBox ID="cbGroupByWeekend" runat="server" />
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

