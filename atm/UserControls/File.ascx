<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_File" Codebehind="File.ascx.cs" %>
<asp:FileUpload ID="fuFile" runat="server" BackColor="White"  />
<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvFile" runat="server" ControlToValidate="fuFile" EnableClientScript="false" Display="Dynamic" Text="&nbsp;&nbsp;*" ErrorMessage=""></asp:RequiredFieldValidator>
<asp:HyperLink ID="hlFile" runat="server" Visible="false" Target="_blank"></asp:HyperLink>&nbsp;
<asp:LinkButton ID="hlRemoveFile" runat="server" Text="Remove" Visible="false" onclick="hlRemoveFile_Click" ></asp:LinkButton>