<%@ Page Title="View Employees" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Tools_ViewEmployees" Codebehind="ViewEmployees.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function OpenPreview(eid) {
            OpenWindow('../Payroll/Setup/PayRatesPreview.aspx?eid=' + eid, 925, 300, 1, 1, 1, 1, 'atM_emp_preview');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div class="container">
        <div class="pageTitle">
            View Employees
        </div>
        <div class="pageSubtitle">
            View Employee Details
        </div>

        <div class="row">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div class="row form-group">
                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
                            <label for="ddProgSygmaCenterNo">Center</label>
                            <asp:DropDownList ID="ddProgSygmaCenterNo" runat="server" DataTextField="CenterDisplay" DataValueField="SygmaCenterNo" AutoPostBack="True"
                                OnSelectedIndexChanged="ddProgSygmaCenterNo_SelectedIndexChanged" CssClass="form-control form-control-sm">
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
                            <label for="txtProgName">Name Search</label>
                            <asp:TextBox ID="txtProgName" runat="server" OnTextChanged="txtProgName_TextChanged" CssClass="form-control form-control-sm" AutoPostBack="True"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row table-responsive">
                    <asp:Panel ID="pnlProgression" class="panel-body" runat="server" Visible="false">
                        <asp:UpdatePanel ID="upSettings" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                Employees <span style="font-weight: normal">(<asp:Label ID="lblEmployeeCount" runat="server"></asp:Label>)</span>
                                <asp:GridView ID="gvEmployees" runat="server" AllowSorting="true" AutoGenerateColumns="false" OnRowDataBound="gvEmployees_RowDataBound"
                                    OnSorting="gvEmployees_Sorting" CssClass="table-responsive" EmptyDataText="No employees exist for this search.">
                                    <Columns>
                                        <asp:BoundField HeaderText="Employee" DataField="WebDisplay" SortExpression="WebDisplay" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderText="Qualified Driver" SortExpression="IsQualifiedDriver">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <%# Convert.ToBoolean(Eval("IsQualifiedDriver"))? "Qualified" : "Not Qualified" %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Hire Date" DataField="HireDate" DataFormatString="{0:M/d/yyyy}" SortExpression="HireDate" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="EffectiveHireDate" DataField="EffectiveHireDate" DataFormatString="{0:M/d/yyyy}" SortExpression="EffectiveHireDate" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField HeaderText="Tenure" DataField="TenureDisplay" SortExpression="HireDate" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField SortExpression="ProgressionRate" HeaderText="Rate (%)">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblProgRate" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ApsException" HeaderText="APS Exception">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAPSExcptn" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="GuaranteedPay" HeaderText="Guaranteed Pay ($)">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGuaranteedPay" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="ClassificationId" HeaderText="Classification">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblClassification" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="RTP">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <a href="javascript: void(0);" onclick='OpenPreview(<%#Eval("EmployeeId") %>);'>
                                                    <i class="fa fa-play-circle fa-lg"></i>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

