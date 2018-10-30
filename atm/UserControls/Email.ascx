<%@ Control Language="c#" Inherits="UserControls_Email" Codebehind="Email.ascx.cs" %>
<asp:textbox id="txtValue" Runat="server" Width="200" MaxLength="50"></asp:textbox>&nbsp;
<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvValue" runat="server" Display="Dynamic"
	EnableClientScript="False" ControlToValidate="txtValue"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator CssClass="validator-message" ID="revValue" runat="server" Display="Dynamic"
	EnableClientScript="False" ControlToValidate="txtValue"></asp:RegularExpressionValidator>
<span style="FONT-SIZE: 9px" id="multiFootNote" runat="server" >
	<br>
	Separate emails with ;'s</span>
    