<%@ Page Language="C#" Inherits="Apps_ATM_ATMChanges" Codebehind="ATMChanges.aspx.cs" %>

<html>
<head id="HEAD1" runat="server">
	<title>ATMChanges</title>
</head>
<body>
	<form id="Form1" method="post" runat="server">
	<table cellspacing="0" cellpadding="2" width="80%" align="center" border="0">
		<tr style="height: 30px">
			<th colspan="2" style="font-size: 15px">
				ATM Update List
			</th>
		</tr>
		<tr>
			<td>
				<table cellpadding="2" cellspacing="0" border="0">
					<tr style="background-color: #008A8C; color: #ffffff; text-align: Left; font-weight: bold">
						<td style="width: 8%">
							Date
						</td>
						<td style="width: 2%">
							&nbsp;
						</td>
						<td style="width: 90%">
							Description
						</td>
					</tr>
					<asp:XmlDataSource ID="xmlChanges" runat="server" XPath="//Changes/Change"></asp:XmlDataSource>
					<asp:Repeater ID="rptChangeList" runat="server" DataSourceID="xmlChanges">
						<ItemTemplate>
							<tr>
								<td style="text-align: left; vertical-align: top; font-weight: bold">
									<%# XPath("date") %>
								</td>
								<td style="text-align: center; vertical-align: top;" nowrap>
									--
								</td>
								<td style="text-align: left">
									<ul>
										<asp:Repeater ID="rptDescs" runat="server" DataSource='<%# XPathSelect("Descriptions/description") %>'>
											<ItemTemplate>
												<li>
													<%# Eval("InnerText") %>
												</li>
											</ItemTemplate>
										</asp:Repeater>
									</ul>
									<%--<asp:BulletedList ID="blDescriptions" runat="server" DataSource='<%# XPathSelect("Descriptions/description") %>' DataTextField="InnerText">
									</asp:BulletedList>--%>
								</td>
							</tr>
						</ItemTemplate>
					</asp:Repeater>
				</table>
			</td>
		</tr>
	</table>
	</form>
</body>
</html>
