<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_SelectWeek" Codebehind="SelectWeek.ascx.cs" %>
<asp:Calendar ID="calWeek" TabIndex="6" runat="server" SelectionMode="DayWeek"
    DayNameFormat="FirstTwoLetters" SelectWeekText="Select" OnSelectionChanged="calWeek_SelectionChanged"
    OnDayRender="calWeek_DayRender">
    <TodayDayStyle Font-Bold="True"></TodayDayStyle>
    <SelectorStyle Font-Bold="True"></SelectorStyle>
    <SelectedDayStyle Font-Bold="True"></SelectedDayStyle>
    <TitleStyle Font-Bold="True" ForeColor="White" BackColor="Teal"></TitleStyle>
</asp:Calendar>
