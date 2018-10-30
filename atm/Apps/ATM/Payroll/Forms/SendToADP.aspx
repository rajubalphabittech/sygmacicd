<%@ Page Title="Send To ADP" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Payroll_Forms_SendToADP" Codebehind="SendToADP.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script type="text/javascript">
	    function UserSendConfirmation() {
//	        if (document.getElementById("ddSygmaCenterNo") == null)
//	        var ddlCenter = document.getElementById("ddSygmaCenterNo");
//	        var value = ddlCenter.getAttribute('value');
//	        var ddlCenterIndex = ddlCenter.selectedIndex;
	        //	        var ddlCenterValue = ddlCenterIndex.value;
	        var selCenter = jQuery('#<%=ddSygmaCenterNo.ClientID %> option:selected').text();
	        var selDate = jQuery('#<%=ddlWeekending.ClientID %> option:selected').text();
	        document.getElementById('<%=btnHidden.ClientID%>').enabled = false;
	        document.getElementById('<%=btnSend.ClientID%>').enabled = false;
	        if (confirm("Are you sure you want to Send ADP Report for center (" + selCenter + ") and weekend (" + selDate + ")?")) {
	            document.getElementById('<%=btnHidden.ClientID%>').click();
	            //	            alert("ADP report has been sent to ADP successfully");
	            //setTimeout(function () { window.location.replace(window.location) }, 3000);
	            //window.location.replace(window.location);
	        }
	        else {
	            document.getElementById('<%=btnSend.ClientID%>').enabled = true;
	        }
            return false;
        }
        function ReportSent() {
            alert("ADP Report Mailed Successfully to ");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
	<div class="pageTitle inlineBlock" style="width: 75%">
		<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
		Send To ADP
	</div>
	
    	<div>
		<asp:UpdatePanel ID="upValidationSummary" runat="server" UpdateMode="Conditional">
			
			<ContentTemplate>
				
         
                <div class="inputPanel" style="width: 100%;">
							
							<div class="body" style="padding-bottom: 5px; height: 75px">
                            <div class="header">
								Send To ADP Info
							</div>
                            <div class="inlineBlock" style="width: 170px; margin-right: 0px;">
									<div class="inputHeader">
										Center
									</div>
									<asp:DropDownList ID="ddSygmaCenterNo" runat="server" 
                                        DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" 
                                        AppendDataBoundItems="false" 
                                        onselectedindexchanged="ddSygmaCenterNo_SelectedIndexChanged" CausesValidation="false" 
                                        AutoPostBack="True">
									    <%--<asp:ListItem Value="-1">--Select Center--</asp:ListItem>--%>
									</asp:DropDownList>
									<%--<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvSygmaCenterNo" runat="server" 
                                        ControlToValidate="ddSygmaCenterNo"  
										 Text="'Center' is required!" ValidationGroup="send" InitialValue="-1"></asp:RequiredFieldValidator>--%>
									
								</div>
								<div class="inlineBlock" style="width: 170px">
									<div class="inputHeader">
										Weekending
									</div>
                                      <asp:DropDownList ID="ddlWeekending" runat="server" 
                                    DataTextField="FiscalWeekending" DataValueField="FieldValue" Width="146px" CausesValidation="false">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvWeekending" runat="server" 
                                    ControlToValidate="ddlWeekending"  ValidationGroup="send" 
                                    InitialValue="-1" Text="'Weekending' is required!"
                                    ></asp:RequiredFieldValidator> --%> 
								</div>
							</div>
							<asp:Panel ID="pnlSendButton" runat="server" Width="100%" HorizontalAlign="Center" Style="padding: 5px	0px 5px 0px">
                                <asp:Button ID="btnSend" runat="server" Text="Send"  ValidationGroup="send" 
                                    OnClientClick="if ( ! UserSendConfirmation()) return false;" Enabled="False" />
                                    <asp:Button ID="btnHidden" runat="server" style="visibility:hidden" OnClick="btnSend_Click" />
		                    </asp:Panel>
						</div>
	


           		</ContentTemplate>
		</asp:UpdatePanel>
            
        </div>
        <asp:UpdateProgress ID="prgSendReport" runat="server" AssociatedUpdatePanelID="upValidationSummary" DisplayAfter="600">
		<ProgressTemplate>
			<div class="disableBackground">
			</div>
			<div class="progressPane loading">
				<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
					Sending ADP Report...</div>
				<asp:Image ID="imgSearchProgress" runat="server" ImageUrl="~/Images/animated_bar.gif" />
			</div>
		</ProgressTemplate>
	</asp:UpdateProgress>
</asp:Content>

