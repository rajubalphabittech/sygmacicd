<%@ Page Language="c#" Inherits="SygmaInternet.UserControls.PopUpCalendar"
    Theme="default" Codebehind="PopUpCalendar.aspx.cs" %>
<html>
<head runat="server">
    <title>Select Date</title>

    <script language="javascript">
			function SetDate(control, date, parentPostBack){
				
				eval("window.opener.document.forms[0]."+control).value = date;
				if (parentPostBack){
					window.opener.document.forms[0].submit();
				}
				window.close();
			}
    </script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="mainForm" method="post" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%" height="100%" bgcolor="#cccccc">
            <tr>
                <td colspan="2">
                    <asp:Calendar ID="Calendar1" runat="server" DayNameFormat="FirstTwoLetters" BorderColor="Black"
                        Font-Names="Verdana" Font-Size="9pt" CellSpacing="1" ForeColor="Black" BackColor="White"
                        BorderStyle="Solid" Height="100%" Width="100%" OnSelectionChanged="Calendar1_SelectionChanged">
                        <TodayDayStyle ForeColor="White" BackColor="#999999"></TodayDayStyle>
                        <DayStyle BackColor="#CCCCCC"></DayStyle>
                        <NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="White"></NextPrevStyle>
                        <DayHeaderStyle Font-Size="8pt" Font-Bold="True" Height="8pt" ForeColor="#333333"></DayHeaderStyle>
                        <SelectedDayStyle ForeColor="White" BackColor="#333399"></SelectedDayStyle>
                        <TitleStyle Font-Size="10pt" Font-Bold="True" Height="12pt" ForeColor="White" BackColor="#333399">
                        </TitleStyle>
                        <OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
                    </asp:Calendar>
                </td>
            </tr>
            <tr id="trMonthYearSelect" runat="server" height="15">
                <td style="text-align: left; width: 55%">
                    <asp:DropDownList ID="ddMonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddMonth_SelectedIndexChanged">
                        <asp:ListItem Value="1">January</asp:ListItem>
                        <asp:ListItem Value="2">February</asp:ListItem>
                        <asp:ListItem Value="3">March</asp:ListItem>
                        <asp:ListItem Value="4">April</asp:ListItem>
                        <asp:ListItem Value="5">May</asp:ListItem>
                        <asp:ListItem Value="6">June</asp:ListItem>
                        <asp:ListItem Value="7">July</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="text-align: center">
                  <asp:LinkButton ID="btnToday" runat="server" Text="Today" onclick="btnToday_Click"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
