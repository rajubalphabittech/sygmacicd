<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Reports_ServiceLevelReport" Codebehind="ServiceLevelReport.aspx.cs" %>
<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        
       
        function ConfirmReasonCodes() {
            if (Page_ClientValidate()) {
                title: "Notification";
                var tabIndex = $find('<%=tcWeeklyQuaterly.ClientID%>');
                var i = tabIndex._activeTabIndex;              
                if (i == 0) {
                    var reasonCodes = document.getElementById('<%=lbWkyReasonCodes.ClientID%>');
                    var selectedReasonCodes = "";
                    for (var i = 0; i < reasonCodes.options.length; i++) {
                        if (reasonCodes.options[i].selected) {
                            selectedReasonCodes += reasonCodes.options[i].value + ", ";
                        }
                    }
                    selectedReasonCodes = selectedReasonCodes.substring(0, selectedReasonCodes.length - 2);

                    if (selectedReasonCodes == "") {
                        alert("'Reason Code' is required !"); return false
                    }
                    return confirm(String.format("Please confirm selected Reason Codes: - {0} \n \n If you want to proceed press 'OK' button", selectedReasonCodes));
                }

                else if (i == 1) {
                  
                    var reasonCodes = document.getElementById('<%=lbQtyReasonCodes.ClientID %>');
                    var selectedReasonCodes = "";
                    for (var i = 0; i < reasonCodes.options.length; i++) {
                        if (reasonCodes.options[i].selected) {
                            selectedReasonCodes += reasonCodes.options[i].value + ", ";
                        }
                    }
                    selectedReasonCodes = selectedReasonCodes.substring(0, selectedReasonCodes.length - 2);

                    if (selectedReasonCodes == "") {
                        alert("'Reason Code' is required !"); return false
                    }
                    return confirm(String.format("Please confirm selected Reason Codes: - {0}", selectedReasonCodes));
                }
            }
        }
    function clientActiveTabChanged(sender, args) {

        return sender.get_activeTabIndex();
    }

        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="pageTitle inlineBlock" style="width: 75%">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        Service Level Report
    </div>
    <asp:ValidationSummary ID="vsServiceLevelReport" runat="server" ValidationGroup="Generate" />
    <div class="inputPanel" style="width: 350px;">
        <div class="header" style="width: 600px;">
            Please select the report period
        </div>
        <div class="inlineBlock" style="width: 350px; margin-right: 0px;">
            <asp:UpdatePanel ID="upInputs" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlInputs" runat="server" Style="width: 350px; margin-right: 0px;">
                        <div>
                                &nbsp;
                                &nbsp;
                                &nbsp;
                                &nbsp;
                           </div>
                          <div class="inputHeader">
                               &nbsp;
                                        <asp:Label ID="lblCenter" runat="server" Text=" Center"></asp:Label>
                            &nbsp;&nbsp;&nbsp;                            
                                    <asp:DropDownList ID="ddSygmaCenterNo" runat="server" DataTextField="CenterDisplay" DataValueField="SygmaCenterNo">
                                    </asp:DropDownList>                          
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvCenter" runat="server" ControlToValidate="ddSygmaCenterNo" 
                                        ValidationGroup="Generate" InitialValue="" ErrorMessage="'Center' is required !">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                &nbsp;
                                &nbsp;
                                &nbsp;
                                &nbsp;
                           </div>
                         
                            <cc1:TabContainer ID="tcWeeklyQuaterly" runat="server"  Width="350px" OnClientActiveTabChanged="clientActiveTabChanged">
			                <cc1:TabPanel ID="tabWeekly" runat="server" HeaderText="Weekly" AutoPostBack="true"  TabIndex="0">
                                <ContentTemplate>                                   
                                    <table>
                            <tr>
                                <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblWkyStartDate" runat="server" Text="Start Date" Width="65px"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtWkyStartDate" ValidationGroup="Generate" autocomplete="off" runat="server"
                                        Width="81px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtWkyStartDate_CalendarExtender" runat="server" Enabled="True" 
                                        TargetControlID="txtWkyStartDate" Format="M/dd/yyyy" PopupPosition="Right">
                                    </cc1:CalendarExtender>                              
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvWkyStartDate" runat="server" ControlToValidate="txtWkyStartDate" 
                                        ValidationGroup="Generate" ErrorMessage="'Start Date' is required !">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="cdvWkyStartDate" runat="server" ControlToValidate="txtWkyStartDate"
                                        ErrorMessage="'Start Date' is invalid." Operator="DataTypeCheck" ValidationGroup="Generate" 
                                        Type="Date">*</asp:CompareValidator>
                                </td>
                            </tr>  
                                        <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>                        
                                <tr>
                                        <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblWkyEndDate" runat="server" Text="End Date"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtWkyEndDate" ValidationGroup="Generate" autocomplete="off" runat="server" Width="81px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtWkyEndDate_CalendarExtender" runat="server" Enabled="True" 
                                        TargetControlID="txtWkyEndDate" Format="M/dd/yyyy" PopupPosition="Right">
                                    </cc1:CalendarExtender>                             
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvWkyEndDate" ValidationGroup="Generate" ControlToValidate="txtWkyEndDate" 
                                        runat="server" ErrorMessage="'End Date' is required !">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="cdvWkyEndDate" runat="server" ControlToValidate="txtWkyEndDate"
                                        ErrorMessage="'End Date' is invalid." Operator="DataTypeCheck" ValidationGroup="Generate" 
                                        Type="Date">*</asp:CompareValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="cvWkyEndDate" runat="server" ControlToCompare="txtWkyStartDate" 
                                        ControlToValidate="txtWkyEndDate" ErrorMessage="'End date' cannot be less than 'Start date'" 
                                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="Generate">*</asp:CompareValidator>
                                </td>
                            </tr>
                             <tr>
                                <td>&nbsp;</td>
                                 </tr>  
                            <tr>                              
                                <td colspan="3">
                               <div class="inputHeader">                                                                                                      
                                <asp:RadioButton ID="rbWkyStdReasonCodes" AutoPostBack="True" runat="server" GroupName="ReasonCodes" Text="Standard" Checked="True" OnCheckedChanged="Wky_ReasonCode_CheckedChanged" />
                                <asp:RadioButton ID="rbWkyCustReasonCodes" AutoPostBack="True" runat="server" GroupName="ReasonCodes" Text="Customized" OnCheckedChanged="Wky_ReasonCode_CheckedChanged" />                            
                                </div>    
                                </td>                          
                            </tr>                            
                              <tr>
                                <td>&nbsp;</td>
                              </tr>   
                            <tr>
                                <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblWeeklyReasonCodes" runat="server" Text="Reason Codes"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:ListBox ID="lbWkyReasonCodes" runat="server" AutoPostBack="True" DataTextField="ReasonCode" DataValueField="ReasonCodeId" Enabled="False" Height="73px" SelectionMode="Multiple" Width="240px"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvWkyReasonCodes" runat="server" ControlToValidate="lbWkyReasonCodes" ErrorMessage="'Reason Code' is required !" ValidationGroup="Generate">*</asp:RequiredFieldValidator>
                                </td>                  
                               </tr>
                                 <tr>
                                <td>&nbsp;</td>
                                 </tr>
                                <tr>
                            <td colspan="3">                       
                                <asp:Label ID="lblWkyCustMessage" runat="server" Visible="false" Text="Standard codes are pre-selected, Please press Ctrl to select more codes" Font-Italic="true"></asp:Label>
                            </td>
                            </tr> 
                                 <tr>
                                <td>&nbsp;</td>
                                 </tr>                      
                          </table>                        
                       </ContentTemplate>
                           </cc1:TabPanel>
                                <cc1:TabPanel ID="tabQuaterly" runat="server" HeaderText="Quaterly" AutoPostBack="true"  TabIndex="1">
                                    <ContentTemplate>
                                        <table>
                                        <tr>
                                        <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblQtystartDate" runat="server" Text="Start Date" Width="65px"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtQtystartDate" ValidationGroup="Generate" autocomplete="off" runat="server"
                                        Width="81px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtQtystartDate_CalendarExtender" runat="server" Enabled="True" 
                                        TargetControlID="txtQtystartDate" Format="M/dd/yyyy" PopupPosition="Right">
                                    </cc1:CalendarExtender>                            
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtQtyStartDate" 
                                        ValidationGroup="Generate" ErrorMessage="'Start Date' is required !">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="CompareValidator1" runat="server" ControlToValidate="txtQtystartDate"
                                        ErrorMessage="'Start Date' is invalid." Operator="DataTypeCheck" ValidationGroup="Generate" 
                                        Type="Date">*</asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblQtyEndDate" runat="server" Text="End Date"></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtQtyEndDate" ValidationGroup="Generate" autocomplete="off" runat="server" Width="81px"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtQtyEndDate_CalendarExtender" runat="server" Enabled="True" 
                                        TargetControlID="txtQtyEndDate" Format="M/dd/yyyy" PopupPosition="Right">
                                    </cc1:CalendarExtender>                             
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="RequiredFieldValidator3" ValidationGroup="Generate" ControlToValidate="txtQtyEndDate" 
                                        runat="server" ErrorMessage="'End Date' is required !">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="CompareValidator2" runat="server" ControlToValidate="txtQtyEndDate"
                                        ErrorMessage="'End Date' is invalid." Operator="DataTypeCheck" ValidationGroup="Generate" 
                                        Type="Date">*</asp:CompareValidator>
                                    <asp:CompareValidator CssClass="validator-message" ID="CompareValidator3" runat="server" ControlToCompare="txtQtyStartDate" 
                                        ControlToValidate="txtQtyEndDate" ErrorMessage="'End date' cannot be less than 'Start date'" 
                                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="Generate">*</asp:CompareValidator>
                                </td>
                            </tr>
                             <tr>
                                <td>&nbsp;</td>                    
                            <tr>
                            <tr>
                                <td colspan="3">
                                <div class="inputHeader">                                     
                                <asp:RadioButton ID="rbQtyStdReasoncodes" AutoPostBack="true" runat="server" GroupName="QtyReasonCodes" Text="Standard" Checked="true" OnCheckedChanged="Qty_ReasonCode_CheckedChanged" />
                                <asp:RadioButton ID="rbQtyCustReasoncodes" AutoPostBack="true" runat="server" GroupName="QtyReasonCodes" Text="Customized" OnCheckedChanged="Qty_ReasonCode_CheckedChanged" />  
                                </div>
                                 </td>                                                       
                            </tr>
                                <tr>
                                <td>&nbsp;</td>                              
                            <tr>
                            <tr>
                                <td>
                                    <div class="inputHeader">
                                        <asp:Label ID="lblQtyReasonCodes" runat="server" Text="Reason Codes" ></asp:Label>
                                    </div>
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                <asp:ListBox ID="lbQtyReasonCodes" runat="server" DataValueField="ReasonCodeId" DataTextField="ReasonCode" 
                                    AutoPostBack="true" SelectionMode="Multiple" Width="240px" Height="73px" Enabled="false">                    
                                    </asp:ListBox>                                   
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator CssClass="validator-message" ID="rfvQtyReasonCodes" runat="server" ControlToValidate="lbQtyReasonCodes" 
                                        ValidationGroup="Generate" ErrorMessage="'Reason Code' is required !">*</asp:RequiredFieldValidator>
                                </td>                               
                            </tr>
                                <tr>
                                <td>&nbsp;</td>
                                 </tr>
                                <tr>                        
                            <td colspan="3">                             
                                <asp:Label ID="lblQtyCustMessage" runat="server" Visible="false" Text="Standard codes are pre-selected, Please press Ctrl to select more codes" Font-Italic="true"></asp:Label>
                            </td>
                        </tr>  
                                <tr>
                                <td>&nbsp;</td>
                                 </tr>                   
                           </table>
                        
                                        </ContentTemplate>
                                </cc1:TabPanel>
                                </cc1:TabContainer>
                        
                        </table>                       
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>&nbsp;</div>
            
            <asp:Panel ID="pnlGenerateButton" runat="server" Width="100%" HorizontalAlign="Center" Style="padding: 5px	0px 5px 0px">
                <asp:Button ID="btnGenerate" runat="server" Text="Generate"
                    ValidationGroup="Generate" OnClick="btnGenerate_Click" OnClientClick="return ConfirmReasonCodes();" />                          
            </asp:Panel>
            <div>&nbsp;</div>
        </div>
        <%--<asp:UpdateProgress ID="upGrid" runat="server" AssociatedUpdatePanelID="upInputs" DisplayAfter="10">
        <ProgressTemplate>
            <div class="disableBackground">
            </div>
            <div class="progressPane loading">
                <div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
                    Loading, please Wait...
                </div>
                <asp:Image ID="imgGridProgress" runat="server" ImageUrl="~/Images/animated_bar.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
            
    </div>
</asp:Content>