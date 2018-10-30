<%@ Page Title="Guaranteed Pay Tool" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Tools_GuaranteedPay" Codebehind="GuaranteedPay.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //        $(document).ready(function () {

        //            $("#dteWeekendingDate").datepicker({
        //                showButtonPanel: true
        //				, onSelect: function (dateText, inst) {
        //				    SetWeekending(dateText);
        //				}

        //            });
        //        });

        function ConfirmationWithIds(formIds) {
            var varFormIdsStr = formIds;
            //alert(varFormIdsStr.length);
            if (varFormIdsStr.length > 0) {
                if (confirm('Misc forms created with Guaranteed pay for all the eligible employees.  Form IDs are ' + formIds + '.Open these forms now?')) {
                    var varFormIds = varFormIdsStr.split(",");
                    for (var i in varFormIds) {
                        OpenWindow('../Payroll/Forms/AddUpdate.aspx?fid=' + varFormIds[i], 880, 650, 1, 1, 1, 1, 'form_' + varFormIds[i]);
                    }
                }
            }
            else {
                alert('All the eligible employees are already getting their guaranteed pay!');
            }
        }

        function SetWeekending(c) {
            var dd = new Date(c.value);
            if (dd.isValid()) {
                var di = dd.getDay();
                if (di != 6)
                    dd.setDate(dd.getDate() + (6 - di));
                //$get('lblWeekending').innerHTML = dd.format("M/d/yyyy");
                c.value = dd.format("M/d/yyyy");
                //$get('pnlWeekending').style.display = 'block';
            } else {
                //$get('lblWeekending').innerHTML = '';
                c.value = '';
                //$get('pnlWeekending').style.display = 'none';
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="container">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="RunNow" class="alert alert-danger" />
        <asp:UpdatePanel ID="upValidationSummary" runat="server" UpdateMode="Conditional">
        </asp:UpdatePanel>

        <div class="pageTitle">
            <div class="pageTitle">
                Guaranteed Pay Tool
            </div>
            <div class="pageSubtitle">
                View Employees by Minimum Weekly Guaranteed Pay
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="form-group">
                    <div class="row">
                        <div class="col-xs-6 col-sm-4 col-md-4 col-lg-3">
                            <label>
                                Center
                            </label>
                            <asp:DropDownList ID="ddSygmaCenterNo" runat="server"
                                DataValueField="SygmaCenterNo" DataTextField="CenterDisplay"
                                AppendDataBoundItems="false"
                                CausesValidation="false"
                                AutoPostBack="True" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-4 col-sm-3 col-md-3 col-lg-3">
                            <label for="txtWeekendingDate">Weekending</label>
                                    <asp:TextBox ID="txtWeekendingDate" ValidationGroup="RunNow" runat="server" CssClass="form-control" onchange="SetWeekending(this);">
                                    </asp:TextBox>
                            <cc1:CalendarExtender ID="txtWeekendingDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtWeekendingDate" Format="M/d/yyyy" PopupPosition="Right">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvWeekendingDate" runat="server"
                                ControlToValidate="txtWeekendingDate" ValidationGroup="RunNow"
                                ErrorMessage="'Weekending Date' is required!" Enabled="true">
                            </asp:RequiredFieldValidator>
                            <asp:RangeValidator CssClass="validator-message text-danger" ID="rngStartDate" ValidationGroup="RunNow" ControlToValidate="txtWeekendingDate" runat="server"
                                MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true" Type="date" Display="dynamic"
                                EnableClientScript="True" Enabled="True"
                                ErrorMessage="'Start Date' value must be between 1/1/1900 and 6/6/2079">
                            </asp:RangeValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2">
                            <div id="pnlGenerateButton" runat="server">
                                <asp:Button ID="btnRunNow" runat="server" Text="Run Now"
                                    ValidationGroup="RunNow" CssClass="btn btn-primary" OnClick="btnRunNow_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

