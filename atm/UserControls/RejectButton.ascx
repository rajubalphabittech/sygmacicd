<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_RejectButton" Codebehind="RejectButton.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">
	function GetReason(hid) {
		var text = GetRequiredPrompt('Why are you rejecting this form?', 'A reason is required!');
		if (text != null) {
			document.getElementById(hid).value = text;
			return true;
		}
		return false;
	}
</script>
<asp:Button ID="btnReject" runat="server" Text="Reject"  OnClick="btnOK_Click"/>
<asp:HiddenField ID="hidRejectReason" runat="server" />
<%--<cc1:ModalPopupExtender ID="btnReject_ModalPopupExtender" runat="server" Enabled="True" TargetControlID="btnReject" PopupControlID="pnlRejectReason"
  DropShadow="true" OkControlID="btnOK" CancelControlID="btnCancel" BackgroundCssClass="modalBG" PopupDragHandleControlID="header">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlRejectReason" runat="server" BackColor="White" Width="300px" Height="175px" CssClass="modalPopup" HorizontalAlign="Center"
  Style="top: -1000; left: -1000;">
  <div id="header" class="modalTitle">
    <asp:Label ID="lblTitle" runat="server" Text="Why are you rejecting this form?"></asp:Label>
  </div>
  <br />
  <asp:TextBox ID="txtRejectReason" runat="server" MaxLength="1000" TextMode="MultiLine" Height="100px" Width="250px"></asp:TextBox><br />
  <br />
  <asp:Button ID="btnOK" runat="server" Text="OK" CssClass="button1" OnClick="btnOK_Click" CausesValidation="false" OnClientClick="mpClickOK(this.name,'')" />
  <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button1" CausesValidation="false" />
  <div id="irr" style="display: none;">
    Please enter a reason for rejecting
  </div>
</asp:Panel>--%>
