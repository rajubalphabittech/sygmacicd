<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Payroll_Setup_PayRates" MasterPageFile="~/Masters/ATM.master"
    Title="Pay Rates" CodeBehind="PayRates.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function SetCurrentFocus(sid, fid) {
            AddAJAXRequest();
            PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
            PageMethods.PM_SaveSessionValue(sid, fid, onSuccess, onfocus);
        }

        function SetToMonth(c, nextToMonth, seqNo, thisFromMonth, hasFromMonth) {
            var val = c.value;
            var rowId = c.id;

            var commonRowId = rowId.toString().substring(0, rowId.lastIndexOf('_') + 1);
            // seqNo is 1 based instead of 0 based
            var currentSeq = seqNo - 1;
            var cPrevToMonth = commonRowId + (currentSeq - 1);
            var prevToMonthControl = $get(cPrevToMonth);
                        
            hasFromMonth = 0;
            thisFromMonth = prevToMonthControl.value;
            if (seqNo == 1) {
                hasFromMonth = 1;
                thisFromMonth = 0;
            }

            var cNextToMonth = commonRowId + (currentSeq + 1);
            var nextToMonthControl = $get(cNextToMonth);

            nextToMonth = 0;
            if (nextToMonthControl != null && nextToMonthControl.value != '') {
                nextToMonth = nextToMonthControl.value;
            }

            //alert('before prev to month');
            if (seqNo == 1 || thisFromMonth != '') {
                hasFromMonth = 1;
            }
            else {
                thisFromMonth = 0;
                hasFromMonth = 0;
            }
            //alert('NEXT tO mONTH: ' + nextToMonth);
            //alert('this fROM mONTH: ' + thisFromMonth);
            //alert('Has fROM mONTH: ' + hasFromMonth);
            if (val != "") {
                val = val.replace(/\,/g, '');
                if (val.isNumeric && (val > 0)) {
                    if (val.indexOf('.') == -1) {
                        if (hasFromMonth == 1) {
                            if (nextToMonth == 0) {
                                if (parseInt(val) > parseInt(thisFromMonth)) {
                                    c.value = FormatAsNumber(val);
                                    PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                                    PageMethods.PM_SetToMonth(gUserName, GetSelectedValue(ddApsSygmaCenterNo), seqNo, val, onSetToMonthSuccess, onFailed, c.id);
                                }
                                else {
                                    RollbackToOrigVal(c);
                                    alert('To Month should be greater than from month of the same band!');
                                }
                            }
                            else {
                                if (parseInt(val) < parseInt(nextToMonth)) {
                                    if (parseInt(val) > parseInt(thisFromMonth)) {
                                        PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                                        PageMethods.PM_SetToMonth(gUserName, GetSelectedValue(ddApsSygmaCenterNo), seqNo, val, onSetToMonthSuccess, onFailed, c.id);
                                    }
                                    else {
                                        RollbackToOrigVal(c);
                                        alert('To Month should be greater than from month of the same band!');
                                    }
                                }
                                else {
                                    RollbackToOrigVal(c);
                                    alert('To Month should be less than To Month of next band!');
                                }

                            }
                        }
                        else {
                            RollbackToOrigVal(c);
                            alert('Please complete filling previous band!!!');
                        }
                    }
                    else {
                        RollbackToOrigVal(c);
                        alert('To Month should be a whole number! Ex: 12');
                    }
                }
                else {
                    RollbackToOrigVal(c);
                    alert('To Month is not a valid positive number!');
                }
            }
            else {
                RollbackToOrigVal(c);
                alert('To Month can not be empty!');
            }
        }

        function SetApsProgRate(c, seqNo, prevProgRate, nextProgRate, nextSeqNo) {
            var val = c.value;
            var rowId = c.id;

            //alert('rowid: ' + rowId);
            var commonRowId = rowId.toString().substring(0, rowId.lastIndexOf('_') + 1);
            // seqNo is 1 based instead of 0 based
            var currentSeq = seqNo - 1;
            var cPrevProg = commonRowId + (currentSeq - 1);
            var cNextProg = commonRowId + (currentSeq + 1);
            var prevProgControl = $get(cPrevProg);
            var nextProgControl = $get(cNextProg);
            if (nextProgControl != null) {
                //alert(nextProgControl.value);
                nextProgRate = nextProgControl.value;
            }

            prevProgRate = prevProgControl.value;
            //alert('prevProgRate: ' + prevProgRate);
            //alert('nextProgRate: ' + nextProgRate);
            if (val != "") {
                val = val.replace(/\,/g, '');
                if (val.isNumeric && (val > 0)) {
                    if ((val.indexOf('.') == -1 && val.length < 4) || ((val.length - val.indexOf('.')) - 1 < 5 && val.indexOf('.') < 4)) {
                        if (prevProgRate != 0) {
                            if (nextProgRate == 0 || parseFloat(val) < parseFloat(nextProgRate)) {
                                if (parseFloat(val) > parseFloat(prevProgRate)) {
                                    c.value = FormatAsNumber(val);
                                    PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                                    PageMethods.PM_SetApsProgRateNetPay(gUserName, GetSelectedValue(ddApsSygmaCenterNo), seqNo, val, 1, onProgRateSuccess, onFailed, c.id);
                                }
                                else {
                                    RollbackToOrigVal(c);
                                    alert('Please enter Prog Rate higher than previous prog rate!!!');
                                }
                            }
                            else {
                                RollbackToOrigVal(c);
                                alert('Please enter Prog Rate less than next prog rate!!!');
                            }
                        }
                        else {
                            RollbackToOrigVal(c);
                            alert('Please complete filling previous band!!!');
                        }
                    }
                    else {
                        RollbackToOrigVal(c);
                        alert('Prog Rate should be less than 4 decimal! Ex: 120.2536');
                    }
                }
                else {
                    RollbackToOrigVal(c);
                    alert('Prog Rate is not a valid positive number!');
                }
            }
            else {
                RollbackToOrigVal(c);
                alert('Prog Rate can not be empty!');
            }
        }


        function SetApsNetPay(c, seqNo, prevNetPay, nextNetPay, nextSeqNo) {
            var val = c.value;
            var rowId = c.id;
            var previewnetRateVal = document.getElementById('<%=txtPrevNetPay.ClientID%>').value;
            previewnetRateVal = previewnetRateVal * 10;
            //alert(previewnetRateVal);
            //var commonRowId = rowId.toString().substring(0, (rowId.toString().indexOf('rptApsSchedule_ctl') + 18));
            var commonRowId = rowId.toString().substring(0, rowId.lastIndexOf('_') + 1);
            // seqNo is 1 based instead of 0 based
            var currentSeq = seqNo - 1;
            var cPrevNetPay = commonRowId + (currentSeq - 1);
            var cNextNetPay = commonRowId + (currentSeq + 1);

            //alert(cNextNetPay);
            var prevNetPayControl = $get(cPrevNetPay);
            var nextNetPayControl = $get(cNextNetPay);
            //alert('before next pay');
            if (nextNetPayControl != null) {
                //alert(nextNetPayControl.value);
                nextNetPay = nextNetPayControl.value;
            }

            prevNetPay = prevNetPayControl.value;
            //alert('nextNetPay: ' + nextNetPay);
            //alert('prevNetPay: ' + prevNetPay);
            if (val != "") {
                val = val.replace(/\,/g, '');
                if (val.isNumeric && (val > 0)) {
                    if ((val.indexOf('.') == -1 && val.length < 9) || ((val.length - val.indexOf('.')) - 1 < 5 && val.indexOf('.') < 9)) {
                        //alert(prevNetPay);
                        //                        if (seqNo == 2 && prevNetPay != 0) {
                        if (prevNetPay != 0) {
                            if (nextNetPay == 0 || parseFloat(val) < parseFloat(nextNetPay)) {
                                if (parseFloat(val) > parseFloat(prevNetPay)) {
                                    if (parseFloat(previewnetRateVal) > parseFloat(c.value)) {
                                        c.value = FormatAsNumber(val);
                                        PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                                        PageMethods.PM_SetApsProgRateNetPay(gUserName, GetSelectedValue(ddApsSygmaCenterNo), seqNo, val, 0, onNetPaySuccess, onFailed, c.id);
                                    }
                                    else {
                                        RollbackToOrigVal(c);
                                        alert('Please enter  Valid Net Pay!!! Maximum accepted Net Pay is ' + (previewnetRateVal - 0.01));
                                    }
                                }
                                else {
                                    RollbackToOrigVal(c);
                                    alert('Please enter Net Pay higher than previous Net Pay!!!');
                                }
                            }
                            else {
                                RollbackToOrigVal(c);
                                alert('Please enter Net Pay less than next Net Pay!!!');
                            }
                        }
                        else {
                            RollbackToOrigVal(c);
                            if (seqNo == 2)
                                alert('Please enter preview Net Pay!!!');
                            else
                                alert('Please complete filling previous band!!!');
                            //alert(prevNetPay);
                        }
                        //                        }
                        //                        else {
                        //                            RollbackToOrigVal(c);
                        //                            alert('Please enter preview Net Pay!!!');
                        //                        }
                    }
                    else {
                        RollbackToOrigVal(c);
                        alert('Net Pay should be less than 4 decimal! Ex: 4.2536');
                    }
                }
                else {
                    RollbackToOrigVal(c);
                    alert('Net Pay is not a valid positive number!');
                }
            }
            else {
                RollbackToOrigVal(c);
                alert('Net Pay can not be empty!');
            }
        }

        function SetApsPayRateName(c, seqNo) {
            var val = c.value;
            if (val.length < 50) {
                PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                PageMethods.PM_SetPayRateName(gUserName, GetSelectedValue(ddApsSygmaCenterNo), seqNo, val); //, onNetPaySuccess, onFailed, c.id);
            }
            else {
                RollbackToOrigVal(c);
                alert('Pay Rate Name can not be more than 50 characters');
            }
        }


        function RollbackToOrigVal(c) {
            if ($(c).attr('origVal') != null) {
                $(c).val($(c).attr('origVal'));
                return true;
            }
            return false;
        }

        function SavePreviewNetPay(c) {
            var val = c.value;
            if (val != "") {
                val = val.replace(/\,/g, '');
                if (val.isNumeric && (val > 0)) {
                    if ((val.indexOf('.') == -1 && val.length < 9) || ((val.length - val.indexOf('.')) - 1 < 5 && val.indexOf('.') < 9)) {
                        c.value = FormatAsNumber(val);
                        PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                        PageMethods.PM_SetPreviewNetPay(gUserName, GetSelectedValue(ddApsSygmaCenterNo), val, onPreviewNetPaySuccess, onFailed);
                    }
                    else {
                        RollbackField(c);
                        alert('Please enter a valid 4 decimal Preview Net Pay! (Example:1.2536)');
                    }
                } else {
                    RollbackField(c);
                    alert('Preview Net pay is not a valid positive number!');
                }
            }
            else {
                RollbackField(c);
                alert('Preview Net Pay can not be empty!');
            }
        }

        function RollbackField(c) {
            var oriVal = document.getElementById('<%=txtPrevNetPayCopy.ClientID%>').value
            if (oriVal != "") {
                $(c).val(oriVal);
                return true;
            }
            $(c).val("");
            return false;
        }

        function RollbackHourlyRate(c) {
            var oriVal = document.getElementById('<%=txtPrevHourlyPay.ClientID%>').value
            if (oriVal != "") {
                $(c).val(oriVal);
                return true;
            }
            $(c).val("");
            return false;
        }

        function SaveHourlyRate(c) {
            var val = c.value;
            if (val != "") {
                val = val.replace(/\,/g, '');
                if (val.isNumeric && (val > 0)) {
                    if ((val.indexOf('.') == -1 && val.length < 9) || ((val.length - val.indexOf('.')) - 1 < 5 && val.indexOf('.') < 9)) {
                        AddAJAXRequest();
                        PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                        PageMethods.PM_SetHourlyRate(gUserName, GetSelectedValue(ddHPSygmaCenterNo), GetSelectedValue(ddClassification), val, onHPSuccess, onFailed);
                    }
                    else {
                        RollbackHourlyRate(c);
                        alert('Please enter a valid 4 decimal Hourly Rate! (Example:1.1234)');
                    }
                } else {
                    RollbackHourlyRate(c);
                    alert('Hourly Rate is not a valid positive number!');
                }
            }
            else {
                RollbackHourlyRate(c);
                alert('Hourly Rate can not be empty!');
            }
        }

        function AddProgressionBand() {
            var previewnetRateVal = document.getElementById('<%=txtPrevNetPay.ClientID%>').value;
            if (previewnetRateVal != "") {
                previewnetRateVal = previewnetRateVal.replace(/\,/g, '');
                if (previewnetRateVal.isNumeric && (previewnetRateVal >= 0)) {
                    PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                    PageMethods.PM_ChangeProgressionBand(gUserName, GetSelectedValue(ddApsSygmaCenterNo), 1, previewnetRateVal, onAddBandSuccess, onfocus);
                }
                else {
                    alert('Please enter a Positive Preview Net Pay! (Example:2.2536)');
                }
            }
            else {
                alert('Preview Net Pay cannot be empty!');
            }

        }

        function RemoveProgressionBand() {
            if (confirm("Are you sure want to remove last progression band? Click on Ok to confirm")) {
                PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
            }
            PageMethods.PM_ChangeProgressionBand(gUserName, GetSelectedValue(ddApsSygmaCenterNo), 2, 0.00, onChangeBandSuccess, onfocus);
        }

        function ClearProgressionBand() {
            if (confirm("Are you sure want to clear all progression band? Click on Ok to confirm")) {
                PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
            }
            PageMethods.PM_ChangeProgressionBand(gUserName, GetSelectedValue(ddApsSygmaCenterNo), 3, 0.00, onChangeBandSuccess, onfocus);
        }

        function UpdateProgressionRateNow() {
            if (confirm("Are you sure want to Update all associates Progression rate now for " + jQuery('#<%=ddApsSygmaCenterNo.ClientID %> option:selected').text() + " ?")) {
                PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                PageMethods.PM_UpdateProgressionRateNow(gUserName, GetSelectedValue(ddApsSygmaCenterNo), onUpdateRateNowSuccess, onfocus);
            }
        }

        function UseApsChanged(c) {
            var enableDisable = 'disable';
            var enableDisableMsg;
            if (c.checked) {
                enableDisable = 'enable';
                enableDisableMsg = 'This will replace the progression rates for ALL ASSOCIATES with the rates you define below, unless they are set for “APS Exception” under the Progression tab. ';
            }
            else {
                enableDisableMsg = 'This will disable the Auto Progression Schedule for ALL ASSOCIATES. '
            }
            if (confirm(enableDisableMsg + "Are you sure you want to " + enableDisable + " Auto Progression Schedule for " + jQuery('#<%=ddApsSygmaCenterNo.ClientID %> option:selected').text() + " ?")) {
                PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                PageMethods.PM_SaveUseApsEnabled(gUserName, GetSelectedValue(ddApsSygmaCenterNo), c.checked, onUseApsChangeSuccess, onfocus);
            }
            else {
                c.checked = !c.checked;
            }
        }

        function EnabledChanged(c, rid) {
            var rate = $get(c.id.replace("chkEnabled", "txtRate"));
            var def = $get(c.id.replace("chkEnabled", "chkIncludeInBase"));
            var pr = $get(c.id.replace("chkEnabled", "chkApplyProgression"));
            var gp = $get(c.id.replace("chkEnabled", "chkIncludeInGuaranteedPay"));

            if (c.checked && (rate.value == '')) {
                rate.focus();
            } else {
                PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                PageMethods.PM_SaveRateEnabled(gUserName, GetSelectedValue(ddSygmaCenterNo), GetSelectedValue(ddPayScale), rid, c.checked, onSuccess, onfocus);
                if (!c.checked && (rate.value != '')) {
                    rate.disabled = true;
                    SetCBDisabled(def, true);
                    SetCBDisabled(pr, true);
                    SetCBDisabled(gp, true);
                } else {
                    rate.disabled = false;
                    SetCBDisabled(def, false);
                    SetCBDisabled(pr, false);
                    SetCBDisabled(gp, false);
                }
            }
        }

        function IncludeInBaseChanged(c, rid) {
            PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
            PageMethods.PM_SetIncludeInBase(gUserName, GetSelectedValue(ddSygmaCenterNo), GetSelectedValue(ddPayScale), rid, c.checked, onSuccess, onfocus);
        }

        function ApplyProgChanged(c, rid) {
            PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
            PageMethods.PM_SetProgressionRateApplies(gUserName, GetSelectedValue(ddSygmaCenterNo), GetSelectedValue(ddPayScale), rid, c.checked, onSuccess, onfocus);
        }

        function IncludeInGuaranteedPayChanged(c, rid) {
            PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
            PageMethods.PM_SetIncludeInGuaranteedPay(gUserName, GetSelectedValue(ddSygmaCenterNo), GetSelectedValue(ddPayScale), rid, c.checked, onSuccess, onfocus);
        }

        function UpdateRateType(c, rid, cb) {
            var val = c.value.trim();
            if (val == '')
                val = 0;
            if (!isNaN(c.value) && (c.value >= 0)) {
                AddAJAXRequest();
                PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
                PageMethods.PM_SaveRate(gUserName, GetSelectedValue(ddSygmaCenterNo), GetSelectedValue(ddPayScale), rid, val, onSuccess, onFailed, c.id);

                var rateTextBoxId = c.id;
                if (cb && (val > 0)) {
                    EnableRow(rateTextBoxId);
                } else {
                    ClearRow(rateTextBoxId);
                }
            } else {
                alert('\'Rate\' must be a positive number!');
                c.select();
            }
        }

        function EnableRow(rowId) {
            var enabled = $get(rowId.replace("txtRate", "chkEnabled"));
            enabled.checked = true;
            SetCBDisabled(enabled, false);

            var def = $get(rowId.replace("txtRate", "chkIncludeInBase"));
            var pr = $get(rowId.replace("txtRate", "chkApplyProgression"));
            var gp = $get(rowId.replace("txtRate", "chkIncludeInGuaranteedPay"));
            SetCBDisabled(def, false);
            SetCBDisabled(pr, false);
            SetCBDisabled(gp, false);
        }
        function ClearRow(rowId) {
            var enabled = $get(rowId.replace("txtRate", "chkEnabled"));
            enabled.checked = false;
            SetCBDisabled(enabled, true);

            var def = $get(rowId.replace("txtRate", "chkIncludeInBase"));
            var pr = $get(rowId.replace("txtRate", "chkApplyProgression"));
            var gp = $get(rowId.replace("txtRate", "chkIncludeInGuaranteedPay"));
            def.checked = false;
            pr.checked = false;
            gp.checked = false;
            SetCBDisabled(def, true);
            SetCBDisabled(pr, true);
            SetCBDisabled(gp, true);
        }
        function SetCBDisabled(cb, disabled) {
            cb.disabled = disabled;
            cb.parentNode.disabled = disabled;
        }
        function onSuccess() {
            RemoveAJAXRequest();
        }

        function onHPSuccess() {
            $("#<%=txtPrevHourlyPay.ClientID%>").val($("#<%=txtHourlyRate.ClientID%>").val());
            RemoveAJAXRequest();
        }

        function onSetToMonthSuccess(rv, cid) {
            RemoveAJAXRequest();
            var c = $get(cid);
            var rowId = c.id;
            var commonRowId = rowId.toString().substring(0, rowId.lastIndexOf('_') + 1);
            
            if (rv[1] != '0') {
                // rv[1] is 1 based instead of 0 based
                var nextSeq = parseInt(rv[1]) - 1;
                var cFromMonth = $get(commonRowId.replace('txtApsBandTo', 'lblApsBandFrom') + (nextSeq));
                cFromMonth.innerHTML = FormatAsNumber(rv[0]);
                c.origVal = rv[0];
            }
        }

        function onChangeBandSuccess() {
            RemoveAJAXRequest();
            var btnid = "#<%=btnRefreshScheduler.ClientID%>";
            var btn = $(btnid);
            btn.click();
        }

        function onAddBandSuccess() {
            RemoveAJAXRequest();
            var btnid = "#<%=btnAddBandRefreshScheduler.ClientID%>";
            var btn = $(btnid);
            btn.click();
        }

        function onPreviewNetPaySuccess() {
            RemoveAJAXRequest();
            var btnid = "#<%=btnRefreshScheduler.ClientID%>";
            var btn = $(btnid);
            btn.click();
        }

        function onNetPaySuccess(rv, cid) {
            RemoveAJAXRequest();
            var c = $get(cid);
            var rowId = c.id;
            var cProgRate;
            if (rv[0] != '0') {
                cProgRate = $get(rowId.replace("txtNetPay", "txtApsProgRate"));
                cProgRate.value = FormatAsNumber(rv[0]);
                cProgRate.origVal = rv[0];
                c.value = rv[1];
                c.origVal = rv[1];
            }
        }

        function onProgRateSuccess(rv, cid) {
            RemoveAJAXRequest();
            var c = $get(cid);
            var rowId = c.id;
            var cNetPay;
            if (rv[0] != '0') {
                cNetPay = $get(rowId.replace("txtApsProgRate", "txtNetPay"));
                cNetPay.value = rv[0];
                cNetPay.origVal = rv[0];
                c.value = rv[1];
                c.origVal = rv[1];
            }
        }

        function onUpdateRateNowSuccess() {
            RemoveAJAXRequest();
            alert('Progression rate of all the employees without APS Exception of the selected center are updated successfully');
        }

        function onUseApsChangeSuccess() {
            RemoveAJAXRequest();
            var btnid = "#<%=btnRefreshScheduler.ClientID%>";
            var btn = $(btnid);
            btn.click();
        }

        function onClassChangeSuccess(bl, c) {
            RemoveAJAXRequest();
        }

        function onVRateSuccess(bl, c) {
            RemoveAJAXRequest();
        }

        function onChangeSuccess(bl, c) {
            var lblRT = $get(GetQualifier(c) + 'lblProgRate');
            lblRT.innerText = $(c).val();
            RemoveAJAXRequest();
        }

        function onGPChangeSuccess(bl, c) {
            $(c).attr('OrigVal', bl);
            var lblRT = $get(GetQualifier(c) + 'lblGuaranteedPay');
            lblRT.innerText = $(c).val();
            RemoveAJAXRequest();
        }

        function onFailed(error) {
            RemoveAJAXRequest();
            alert('There was an error updating the rate.  Please close the window and try again.');
        }
    </script>
    <uc1:JScript ID="JScript1" runat="server" FileNames="AJAX.js" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div class="pageTitle" style="width: 100%">
        Set Pay Rates
    </div>
    <div class="pageSubtitle">
        View and Modify Pay Rates
    </div>
    <div class="container" style="margin-top: 10px">
        <div class="row">
            <div>
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#payscaleTab" aria-controls="payscaletab" role="tab" data-toggle="tab">Pay Scales</a></li>
                    <li role="presentation"><a href="#autoprogressionTab" aria-controls="autoprogressionTab" role="tab" data-toggle="tab">Auto Progression Scheduler</a></li>
                    <li role="presentation"><a href="#hourlyRateTab" aria-controls="hourlyRateTab" role="tab" data-toggle="tab">Hourly Rate</a></li>
                </ul>

                <div class="tab-content">
                    <div id="payscaleTab" role="tabpanel" class="tab-pane active">
                        <div class="row">
                            <div class="col-sm-5 col-md-4 col-lg-3">
                                <div class="panel panel-default">
                                    <div class="panel-heading">Select Pay Scale</div>
                                    <div class="panel-body">
                                        <div class="form-group">
                                            <label for="ddSygmaCenterNo">Center</label>
                                            <asp:DropDownList ID="ddSygmaCenterNo" runat="server" DataTextField="CenterDisplay" DataValueField="SygmaCenterNo" CssClass="form-control" OnSelectedIndexChanged="ddSygmaCenterNo_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <label for="ddPayScale">Pay Scale</label>
                                            <asp:DropDownList ID="ddPayScale" runat="server" DataTextField="PayScaleDisplay" DataValueField="PayScaleId" CssClass="form-control" OnSelectedIndexChanged="ddPayScale_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-8 col-sm-7 col-md-8 col-lg-8">
                                <asp:UpdatePanel ID="upRates" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddSygmaCenterNo" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddPayScale" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlRates" runat="server" Visible="false" Style="margin-left: 20px">
                                            <asp:GridView ID="gvPayRates" runat="server" DataKeyNames="RateTypeId" AutoGenerateColumns="false" OnRowDataBound="gvPayRates_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="RateTypeDescription" HeaderText="Rate Type" ItemStyle-Width="100px" />
                                                    <asp:TemplateField HeaderText="Rate">
                                                        <ItemTemplate>
                                                            $<asp:TextBox ID="txtRate" runat="server" onChange='<%# string.Format("UpdateRateType(this, {0}, true);", Eval("RateTypeId")) %>' Width="50px"
                                                                Style="text-align: right;" MaxLength="11"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEnabled" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIncludeInBase" runat="server" ToolTip="Include this rate in base pay." />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Apply Prog. Rate" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkApplyProgression" runat="server" ToolTip="Employee progression rate will be applied to this rate" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GuaranteedPay" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIncludeInGuaranteedPay" runat="server" ToolTip="Include this rate in Guaranteed Pay Calculation." />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <div id="autoprogressionTab" role="tabpanel" class="tab-pane">
                        <div class="panel panel-default">
                            <div class="panel-heading">Auto Progression Scheduler</div>
                            <div class="panel-body">
                                <asp:UpdatePanel ID="upAps" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddApsSygmaCenterNo" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cbAps" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnRefreshScheduler" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAddBandRefreshScheduler" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Button ID="btnRefreshScheduler" runat="server" Text="Refresh" OnClick="btnRefreshSchedule_Click" Style="display: none" />
                                        <asp:Button ID="btnAddBandRefreshScheduler" runat="server" Text="Refresh" OnClick="btnAddBandRefreshSchedule_Click" Style="display: none" />

                                        <div class="row">
                                            <div class="form-group col-md-2">
                                                <label for="ddApsSygmaCenterNo">Center</label>

                                                <asp:DropDownList ID="ddApsSygmaCenterNo" runat="server" DataTextField="CenterDisplay" DataValueField="SygmaCenterNo" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddApsSygmaCenterNo_SelectedIndexChanged" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="form-group col-md-2">
                                                <label for="cbAps">Use APS</label><br />
                                                <asp:CheckBox ID="cbAps" runat="server" Visible="false" onclick="UseApsChanged(this);" ToolTip="Include this Center in Auto Progression Schedule." AutoPostBack="True"></asp:CheckBox>
                                            </div>
                                            <div class="form-group col-md-5">
                                                <label for="cbAps">Preview Net Pay</label><br />

                                                <asp:Label ID="lblPreview" runat="server" Visible="false">100% Progression = $</asp:Label>
                                                <asp:TextBox ID="txtPrevNetPay" runat="server" Visible="false" autocomplete="off" onchange="SavePreviewNetPay(this)" ToolTip="Enter 2 decimal value for Net Pay Preview. Ex: 4.95" Width="40px" AutoPostBack="True"></asp:TextBox>
                                                <asp:TextBox ID="txtPrevNetPayCopy" runat="server" Style="display: none"></asp:TextBox>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlApsSchedule" runat="server" Visible="false" Style="margin-bottom: 10px; margin-top: 10px">
                                            <div class="row">

                                                <asp:Repeater ID="rptApsSchedule" runat="server" OnItemDataBound="rptApsSchedule_ItemDataBound">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered table-hover table-striped">
                                                            <thead>
                                                                <tr>
                                                                    <th>From (months)</th>
                                                                    <th>To (months)</th>
                                                                    <th>Rate</th>
                                                                    <th>Net Pay</th>
                                                                    <th>Pay Band Name</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><span>>=</span>
                                                                <asp:Label ID="lblApsBandFrom" runat="server"></asp:Label></td>
                                                            <td><
                                                    <asp:TextBox ID="txtApsBandTo" runat="server" Width="30px" autocomplete="off" Style="text-align: left"></asp:TextBox></td>
                                                            <td>
                                                                <asp:Label ID="lblApsProgRate" runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtApsProgRate" runat="server" Width="80px" autocomplete="off" Style="text-align: left"></asp:TextBox></td>
                                                            <td>$<asp:Label ID="lblNetPay" runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtNetPay" runat="server" Width="80px" autocomplete="off" Style="text-align: left"></asp:TextBox></td>
                                                            <td>
                                                                <asp:Label ID="lblPayBandName" runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtPayBandName" runat="server" Width="180px" autocomplete="off" Style="text-align: left"></asp:TextBox></td>
                                                        </tr>
                                                    </ItemTemplate>

                                                    <FooterTemplate>
                                                        </tbody>
                                                        <tfoot>
                                                        </tfoot>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>

                                            </div>

                                            <div class="row text-center">
                                                <table>
                                                    <tr>
                                                        <td style="width: 50px">
                                                            <asp:HyperLink ID="hlAddProgressionBand" runat="server" ImageUrl="~/Images/Icons/addSchedule.png" NavigateUrl="javascript: void(0);" onclick="AddProgressionBand();return false;" ToolTip="Click to Add one more progression band" Text="Add Progression Band" Style="display: none"></asp:HyperLink>
                                                        </td>
                                                        <td style="width: 50px">&nbsp;</td>
                                                        <td style="width: 50px">
                                                            <asp:HyperLink ID="hlRemoveLastBand" runat="server" ImageUrl="~/Images/Icons/removeSchedule.png" NavigateUrl="javascript: void(0);" onclick="RemoveProgressionBand();return false;" ToolTip="Click to remove last progression band" Text="Remove last Progression Band" Style="display: none"></asp:HyperLink>
                                                        </td>
                                                        <td style="width: 50px">&nbsp;</td>
                                                        <td style="width: 50px">
                                                            <asp:HyperLink ID="hlClearProgressionBand" runat="server" ImageUrl="~/Images/Icons/clearSchedule.png" NavigateUrl="javascript: void(0);" onclick="ClearProgressionBand();return false;" ToolTip="Click to clear the schedule for the selected center" Text="Clear All Progression Band" Style="display: none"></asp:HyperLink>
                                                        </td>
                                                        <td style="width: 50px">&nbsp;</td>
                                                        <td style="width: 50px">
                                                            <asp:HyperLink ID="hlUpdateNow" runat="server" ImageUrl="~/Images/Icons/UpdateNow.png" NavigateUrl="javascript: void(0);" onclick="UpdateProgressionRateNow();return false;" ToolTip="Click to Update Associate Progression Rates Now" Text="Update Associates Progression Rate Now" Style="display: none"></asp:HyperLink>
                                                        </td>
                                                        <td style="width: 50px">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>Add</td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <asp:Label ID="lblRemove" Text="Remove" runat="server" Style="display: none"></asp:Label></td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <asp:Label ID="lblClearAll" Text="Clear All" runat="server" Style="display: none"></asp:Label></td>
                                                        <td>&nbsp;</td>
                                                        <td style="text-align: center">
                                                            <asp:Label ID="lblUpdateNow" Text="Update Now" runat="server" Style="display: none"></asp:Label></td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>


                    </div>
                    <div id="hourlyRateTab" role="tabpanel" class="tab-pane">
                        <div class="panel panel-default">
                            <div class="panel-heading">Hourly Rate</div>
                            <div class="panel-body">
                                <asp:UpdatePanel ID="upHP" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddHPSygmaCenterNo" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddClassification" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="form-group col-md-2">
                                                <label for="ddHPSygmaCenterNo">Center</label>

                                                <asp:DropDownList ID="ddHPSygmaCenterNo" runat="server" DataTextField="CenterName" DataValueField="SygmaCenterNo" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddHPSygmaCenterNo_SelectedIndexChanged" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>



                                            <div class="form-group col-md-2">
                                                <label for="ddClassification">Classification</label>

                                                <asp:DropDownList ID="ddClassification" runat="server" DataTextField="ClassificationName" DataValueField="ClassificationId" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddClassification_SelectedIndexChanged" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="form-group col-md-3">
                                                <label for="txtHourlyRate">Rate</label><br />
                                                <asp:TextBox ID="txtHourlyRate" runat="server" Enabled="false" autocomplete="off" onchange="SaveHourlyRate(this)" ToolTip="Enter 4 decimal value for Net Pay Preview. Ex: 15.9500" CssClass="form-control" AutoPostBack="True"></asp:TextBox>
                                                <asp:TextBox ID="txtPrevHourlyPay" runat="server" Style="display: none"></asp:TextBox>
                                            </div>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
