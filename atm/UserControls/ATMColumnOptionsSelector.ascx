<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ATMColumnOptionsSelector" Codebehind="ATMColumnOptionsSelector.ascx.cs" %>

<div>
    <div style="margin-bottom: 5px">
        <div class="inlineBlock" style="font-weight: bold; width: 200px">
            Available Columns
        </div>
        <div class="inlineBlock" style="width: 60px"></div>
        <div class="inlineBlock" style="font-weight: bold; width: 200px">
            Selected Columns
        </div>
    </div>
    <div style="margin-bottom: 5px; height: 200px">
        <div class="inlineBlock" style="width: 200px; vertical-align: top;">
            <asp:ListBox ID="lstBoxAvailableColumns" runat="server" Height="280px" Width="200px" SelectionMode="Multiple" ondblclick="lstBoxAvailableColumnsOnDblClick();" ClientIDMode="Static"></asp:ListBox>
        </div>
        <div class="inlineBlock" style="width: 60px; text-align: center; vertical-align: top;">
            <asp:HyperLink ID="btnSelectAvailableColumn" runat="server" Width="40px" Style="margin-bottom: 10px;" Alt="Select" Class="fa fa-angle-right fa-lg btn btn-default" title="Move Right" saria-hidden="true" NavigateUrl="javascript: btnSelectAvailableColumnOnClick();"></asp:HyperLink>
            <asp:HyperLink ID="btnSelectAllAvailableColumn" runat="server" Class="fa fa-angle-double-right fa-lg btn btn-default" Width="40px" aria-hidden="true" title="Move All Right" Style="margin-bottom: 10px;" Alt="Select All" NavigateUrl="javascript: btnSelectAllAvailableColumnOnClick();"></asp:HyperLink>
            <asp:HyperLink ID="btnUnSelectSelectedColumn" runat="server" Class="fa fa-angle-left fa-lg btn btn-default" Width="40px" aria-hidden="true" title="Move Left" Alt="UnSelect" NavigateUrl="javascript: btnUnSelectSelectedColumnOnClick();"></asp:HyperLink>
        </div>
        <div class="inlineBlock" style="width: 200px; vertical-align: top;">
            <asp:ListBox ID="lstBoxSelectedColumns" runat="server" Height="280px" Width="200px" SelectionMode="Multiple" ClientIDMode="Static" ondblclick="lstBoxSelectedColumnsOnDblClick();"></asp:ListBox>
        </div>
        <div class="inlineBlock" style="width: 60px; text-align: center; vertical-align: top">
            <asp:HyperLink ID="btnSelectedColumnMoveUp" runat="server" Width="40px" Style="margin-bottom: 10px;" Class="fa fa-angle-up fa-lg btn btn-default" aria-hidden="true" title="Move Up" Alt="Move Up" NavigateUrl="javascript: btnSelectedColumnMoveUpOnClick();"></asp:HyperLink>
            <asp:HyperLink ID="btnSelectedColumnMoveDown" runat="server" Width="40px" Alt="Move Down" Class="fa fa-angle-down fa-lg btn btn-default" aria-hidden="true" title="Move Down" NavigateUrl="javascript: btnSelectedColumnMoveDownOnClick();"></asp:HyperLink>
        </div>
    </div>
</div>
