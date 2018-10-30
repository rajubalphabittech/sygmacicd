<%@ Control Language="c#" Inherits="UserControls_Date" Codebehind="Date.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<uc1:JScript ID="JScript1" runat="server" FileName="Common.js" />
<%-- not sure why this is here but it's causing ajax to freak: <a name='<% Response.Write(Name.Replace(" ", "_")); %>date'></a>--%>
<asp:Panel ID="pnlMain" runat="server">
	<asp:Panel ID="pnlName" runat="server">
		<asp:Label ID="lblName" runat="server"></asp:Label>
	</asp:Panel>
	<asp:Panel ID="pnlDate" runat="server" style="vertical-align: top">
		<div style="display: inline-block; position: relative; zoom: 1; *display: inline;">
			<asp:TextBox ID="txtDate" runat="server" Columns="9" MaxLength="10" OnTextChanged="txtDate_TextChanged" AutoCompleteType="None" onChange="this.value = this.value.toString().trim();" autocomplete="off"></asp:TextBox>
			
			<cc1:TextBoxWatermarkExtender ID="txtDate_TextBoxWatermarkExtender" runat="server" Enabled="True" TargetControlID="txtDate" WatermarkCssClass="textboxWatermark" >
			</cc1:TextBoxWatermarkExtender>
			
			<cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtDate" Format="M/d/yyyy" PopupPosition="Right">
			</cc1:CalendarExtender>
		</div>
		<div style="display: inline-block; position: relative; zoom: 1; *display: inline;">
			<asp:RequiredFieldValidator CssClass="validator-message" ID="rfvDate" runat="server" Font-Bold="True" ControlToValidate="txtDate" Display="Dynamic" EnableClientScript="False"></asp:RequiredFieldValidator>
			<asp:CompareValidator CssClass="validator-message" ID="cmpIsDate" runat="server" ControlToValidate="txtDate" SetFocusOnError="true" Operator="DataTypeCheck" Type="date"
				Display="dynamic" EnableClientScript="False" Enabled="false"></asp:CompareValidator>
			<asp:RangeValidator CssClass="validator-message" ID="rngDBDate" runat="server" ControlToValidate="txtDate" MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true"
				Display="dynamic" Type="date" EnableClientScript="False" Enabled="false"></asp:RangeValidator>
			<asp:CompareValidator CssClass="validator-message" ID="cmpDates" runat="server" ControlToValidate="txtDate" SetFocusOnError="true" Type="date" Display="dynamic"
				EnableClientScript="false" Enabled="false"></asp:CompareValidator>
		</div>
	</asp:Panel>
</asp:Panel>
