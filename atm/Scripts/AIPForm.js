var gPRCount = 0;
var dateFormat = 'M/d/yyyy';
var gIsLastInvoice = false;
function SizePDF() {
    var arr = GetWindowWidthHeight();
    var ew = arr[0] - 380;
    var eh = arr[1] - 127;
    if (document.getElementById('pnlInvoiceFile')) {
        document.getElementById('pnlInvoiceFile').style.width = ew + 'px';
        if (document.getElementById('pdfDoc'))
            document.getElementById('pdfDoc').style.width = (ew - 8) + 'px';
    }
}
function GetWindowWidthHeight() {
    var winW, winH;
    if (document.body && document.body.offsetWidth) {
        winW = document.body.offsetWidth;
        winH = document.body.offsetHeight;
    } else if (document.compatMode == 'CSS1Compat' &&
					document.documentElement &&
					document.documentElement.offsetWidth) {
        winW = document.documentElement.offsetWidth;
        winH = document.documentElement.offsetHeight;
    } else if (window.innerWidth && window.innerHeight) {
        winW = window.innerWidth;
        winH = window.innerHeight;
    }

    return new Array(winW, winH);
}

function OpenMaintenance() {
    var newWin = OpenWindow('../Maintenance/Vendors.aspx', 800, 500, 1, 1, 1, 1, 'AddVendor');
    if (newWin != null)
        newWin.focus();
}
function ShowHide(i) {
    var row = $get('Detail' + i);
    var ec = $get('ec_' + i);
    if (row.style.display == 'none') {
        row.style.display = 'table-row';
        ec.innerHTML = '-';
    } else {
        row.style.display = 'none';
        ec.innerHTML = '+';
    }
}
function Show(i) {
    var row = $get('Detail' + i);
    var ec = $get('ec_' + i);

    row.style.display = 'table-row';
    ec.innerHTML = '-';
}
function Hide(i) {
    var row = $get('Detail' + i);
    var ec = $get('ec_' + i);

    row.style.display = 'none';
    ec.innerHTML = '+';
}

function RejectVoucher(title) {
    DisableForm(true);
    var reason = prompt(title, '');
    if (reason == null) {
        DisableForm(false);
        return false;
    } else {
        if (reason == '') {
            return RejectVoucher('Why are you rejecting this voucher?  A reason is required!');
        } else {
            $get('hidRejectMessage').value = reason;
            return true;
        }
    }
}

function DisableForm(onOff) {
    GrayOut(onOff);
    if ($get('pdfDoc')) {
        if (!onOff) {
            $get('pdfDoc').style.display = 'block';
        } else {
            $get('pdfDoc').style.display = 'none';
        }
    }
}

var maxRetries = 20;
function ConfirmSubmit(msg, btn, retry) {
    if (retry == null)
        retry = 0;
    DisableForm(true);
    if (gPRCount <= 0) {
        var hasConfirmed = false;
        var txtAmountToPay = $get('txtAmountToPay');
        var txtInvoiceAmount = $get('txtInvoiceAmount');
        if (txtInvoiceAmount && txtAmountToPay) {
            if (IsValidForm()) {
                var amountToPay = txtAmountToPay.value;
                var invoiceAmount = txtInvoiceAmount.value;
                var taxAmount = $get('txtTax').value;
                if (amountToPay == '' || invoiceAmount == '') {
                    return Notify('\'Invoice Amount\' and \'Amount to Pay\' are required!');
                } else {
                    amountToPay = GetFloat('txtAmountToPay'); //  parseFloat(amountToPay.replace(/\,/g, ""));
                    invoiceAmount = GetFloat('txtInvoiceAmount'); //  parseFloat(invoiceAmount.replace(/\,/g, ""));
                    taxAmount = GetFloat('txtTax');  //parseFloat(taxAmount.replace(/\,/g, ""));
                    //alert(invoiceAmount + '  ' + taxAmount);
                    if (amountToPay > invoiceAmount) {
                        return Notify('The \'Amount to Pay\' cannot be more than the \'Invoice Amount\'!');
                    } else {
                        var glPaymentAmount = parseFloat($get('txtPaymentAmount').value);
                        if (HasValidGL('GLAccountAdd') && glPaymentAmount != 0) {
                            if (!confirm('You have entered "Payment Account" information but have NOT saved it to the voucher.\n\nIf you would like to add the account, click "Cancel" and click the "Add" button below, then re-submit the form.\n\nOtherwise click "OK" to submit the voucher without the account.')) {
                                DisableForm(false);
                                return false;
                            } else {
                                hasConfirmed = true;
                            }
                        }
                        if (Math.abs((amountToPay - taxAmount) * .1) < Math.abs(taxAmount)) {
                            if (!confirm('The \'Sales Tax\' is more than 10% of the \'Amount to Pay\'.\nAre you sure you wish to submit this voucher?')) {
                                DisableForm(false);
                                return false;
                            } else {
                                hasConfirmed = true;
                            }
                        }

                        if (amountToPay == 0) {
                            return Warn('This invoice contains a $0 \'Amount to Pay\'.\nAre you sure you wish to submit this as a non-payment voucher?', btn);
                        } else if (amountToPay < 0) {
                            return Warn('This invoice contains a negative \'Amount to Pay\'.\nAre you sure you wish to submit this as a credit?', btn);
                        }
                    }
                }
            } else {
                return false;
            }
        }
        if (!hasConfirmed) {
            return Confirm(msg, btn);
        } else {
            return Submit(btn);
        }

    } else {
        if (btn != null && retry < maxRetries) {
            retry++;
            var script = 'ConfirmSubmit(\'' + msg.replace(/\'/g, '\\\'') + '\',\'' + btn.replace(/\'/g, '\\\'') + '\',' + retry + ');';
            setTimeout(script, 100);
        } else {
            return Notify('There are pending saves to this voucher.  Please wait a few seconds before submitting.');
        }
        return false;
    }
}

function IsValidForm() {
    if ($get('hidVendor').value == '') {
        return Notify('Must select a Vendor!');
    } else if ($get('txtInvoiceNumber').value.trim() == '') {
        return Notify('Must enter an \'Invoice #\'!');
    } else if ($get('txtFormDescription').value.trim() == '') {
        return Notify('Must enter a \'Purpose\'!');
    }
    return true;
}
function Wait() {
    var i = 0;
    while (gPRCount > 0) {
        if (i++ >= 1000)
            break;
    }
}
function Notify(msg) {
    alert(msg);
    DisableForm(false);
    return false;
}
function Warn(msg, btn) {
    return Confirm('***Warning***\n' + msg, btn);
}
function Confirm(msg, btn) {
    if (confirm(msg)) {
        return Submit(btn);
    } else {
        DisableForm(false);
        return false;
    }
}
function Submit(btn) {
    if (btn != null) {
        eval(btn);
        return false;
    } else {
        return true;
    }
}
function CheckNumeric(c) {
    var len = c.value.length;
    if (c.value.charAt(len - 1) == '.')
        c.value = c.value.substring(0, len - 1);
    if (c.value.indexOf('.') != c.value.lastIndexOf('.')) {
        alert('This value has too many \'.\'.  Please remove one.');
        c.select();
        return false;
    } else {
        var val;
        if (c.value.indexOf('/') > -1 || c.value.indexOf('+') > -1 || c.value.indexOf('*') > -1 || c.value.indexOf('-') > 0) {
            val = NaN;
        } else {
            val = parseFloat(c.value)
        }

        if (isNaN(val)) {
            if (c.value != '')
                alert('"' + c.value + '" is not a valid amount for this field!');
            c.value = "0.00";
            c.select();
            return false;
        } else {
            if (Math.abs(val) > 99999999.99) {
                alert('"' + c.value + '" is too large for this field!');
                c.value = "0.00";
                c.select();
                return false;
            }
        }
        return true;
    }
}

function UpdateStatusNotification(c) {
    gPRCount++;
    PageMethods.PM_UpdateStatusNotification(c.checked, gFormId, gUserName, onSuccess, onFailure);
}
function OpenVendor() {
    OpenWindow('SelectVendor.aspx?fid=' + gFormId, 790, 630, 1, 1, 1, 1, 'SelectVendor');
}
function OpenCMMSVendor() {
    OpenWindow('SelectCMMSVendor.aspx?fid=' + gFormId, 790, 630, 1, 1, 1, 1, 'SelectCMMSVendor');
}
function SaveField(fn, c) {
    switch (fn) {
        case 'InvoiceDate':
            SaveInvoiceDate(c);
            break;
        case 'DueDate':
            SaveDueDate(c);
            break;
        case 'Terms':
            SaveTerms(c);
            break;
        case 'InvoiceNumber':
            gPRCount++;
            PageMethods.PM_SaveField(gFormId, gUserName, fn, c.value, refreshAll, onFailure);
            break;
        case 'PONumber':
            gPRCount++;
            PageMethods.PM_SaveField(gFormId, gUserName, fn, c.value, onSuccess, onFailure);
            break;
        case 'FormDescription':
            gPRCount++;
            PageMethods.PM_SaveField(gFormId, gUserName, fn, c.value, refreshOpener, onFailure);
            if ($get('txtPaymentNotes').value.toString().trim() == '')
                $get('txtPaymentNotes').value = c.value.substring(0, 25);
            break;
        case 'InvoiceAmount':
            if (CheckNumeric(c)) {
                var copyToATP = (GetFloat('txtAmountToPay') == 0);
                var invAmt = GetFloat(c);

                gPRCount++;
                PageMethods.PM_SaveInvoiceAmount(gFormId, gUserName, invAmt, copyToATP, invAmtSuccess, onFailure, copyToATP);

                var formattedAmt = FormatAsNumber(invAmt.toFixed(2));
                if (!copyToATP) {
                    SetAmountDiff();
                } else {
                    $get('txtAmountToPay').value = formattedAmt;
                    SetTaxRate();

                    $get('pnlAmountDifference').style.display = 'none';
                    SetPaymentAmount(invAmt);
                }
                c.value = formattedAmt;
                SetVariance();
            }
            break;
        case 'AmountToPay':
            if (CheckNumeric(c)) {
                var atpAmt = GetFloat(c);
                gPRCount++;
                PageMethods.PM_SaveField(gFormId, gUserName, fn, atpAmt, refreshOpener, onFailure);
                SetTaxRate();
                SetAmountDiff();
                SetPaymentAmount(atpAmt);
                //SetCMMSApprove(true);
                SetVariance();
                c.value = FormatAsNumber(atpAmt.toFixed(2));
            }
            break;
        case 'Tax':
            if (CheckNumeric(c)) {
                var taxAmt = GetFloat(c);
                gPRCount++;
                PageMethods.PM_SaveField(gFormId, gUserName, fn, taxAmt, onSuccess, onFailure);
                SetTaxRate();
                //SetCMMSApprove(true);
                SetVariance();
                c.value = FormatAsNumber(taxAmt.toFixed(2));
            }
            break;
        case 'AttachBackup':
        case 'DeliverToAssociate':
            gPRCount++;
            PageMethods.PM_SaveField(gFormId, gUserName, fn, c.checked, refreshOpener, onFailure);
            break;
    }
}
function SetPaymentAmount(atp) {
    var aa = GetFloat('hidAmountAllocated');
    $get('txtPaymentAmount').value = FormatAsNumber((atp - aa).toFixed(2))
}
function SaveInvoiceDate(c) {
    if (c.value.trim() != '') {
        var invDate = new Date(CleanDate($get('dteInvoiceDate_txtDate').value));
        if (invDate.isValid() && invDate >= new Date('1/1/1753')) {
            var dueDate = null;
            var terms = null;
            if ($get('pnlDueDateEnter') != null) {
                if ($get('dteDueDate_txtDate').value.trim() != '') {
                    dueDate = new Date($get('dteDueDate_txtDate').value);
                    terms = CalcTerms(invDate, dueDate);
                } else if ($get('txtTermsDays').value.trim() != '') {
                    terms = parseInt($get('txtTermsDays').value);
                    dueDate = CalcDueDate(invDate, terms);
                }
            } else if ($get('pnlDueDate400')) {
                CalcDueDate(invDate, parseInt($get('lblTermsDays400').innerHTML), true);
            }
            c.value = invDate.format("M/d/yyyy");
            gPRCount++;
            PageMethods.PM_SaveDates(gFormId, gUserName, invDate, dueDate, terms, refreshAll, onFailure);

        } else {
            alert('The \'Invoice Date\' is an invalid date and has not been saved.');
            //$get('dteInvoiceDate_txtDate').value = '';
        }
    }
}
function SaveDueDate(c) {
    if (c.value.trim() != '') {
        var dueDate = new Date(CleanDate($get('dteDueDate_txtDate').value));
        if (dueDate.isValid() && dueDate >= new Date('1/1/1753')) {
            var invDate = null;
            var terms = null;
            var cancelRefresh = true;
            if ($get('dteInvoiceDate_txtDate').value.trim() != '') {
                invDate = new Date($get('dteInvoiceDate_txtDate').value);
                terms = CalcTerms(invDate, dueDate);
            } else if ($get('txtTermsDays').value.trim() != '') {
                terms = parseInt($get('txtTermsDays').value);
                invDate = CalcInvoiceDate(dueDate, terms);
                cancelRefresh = false;
            }
            c.value = dueDate.format(dateFormat);
            gPRCount++;
            PageMethods.PM_SaveDates(gFormId, gUserName, invDate, dueDate, terms, refreshAll, onFailure, cancelRefresh);
        } else {
            alert('The \'Due Date\' is an invalid date and has not been saved.');
        }
    }
}
function SaveTerms(c) {
    var terms = parseInt(c.value);
    var invDate = null;
    var dueDate = null;
    var refresh = false;
    if (!isNaN(terms)) {
        if ($get('dteInvoiceDate_txtDate').value.trim() != '') {
            invDate = new Date($get('dteInvoiceDate_txtDate').value);
            dueDate = CalcDueDate(invDate, terms);
        } else if ($get('dteDueDate_txtDate').value.trim() != '') {
            dueDate = new Date($get('dteDueDate_txtDate').value);
            invDate = CalcInvoiceDate(dueDate, terms);
            refresh = true;
        }
        gPRCount++;
        PageMethods.PM_SaveDates(gFormId, gUserName, invDate, dueDate, terms, refreshAll, onFailure, refresh);
    } else {
        alert('The \'Terms\' is an invalid number of days and has not been saved.');
    }
}
function SetTaxRate() {
    var tax = parseFloat(GetFloat('txtTax').toFixed(2));
    var amt = parseFloat(GetFloat('txtAmountToPay').toFixed(2)) - tax;
    if (amt != 0 && tax != 0) {
        $get('lblTaxRate').innerHTML = '(' + ((tax / amt) * 100).toFixed(2) + ' %)';
    } else {
        $get('lblTaxRate').innerHTML = '';
    }
}
function SetAmountDiff() {
    var ia = parseFloat(GetFloat('txtInvoiceAmount').toFixed(2));
    var atp = parseFloat(GetFloat('txtAmountToPay').toFixed(2));
    var diff = ia - atp
    var pnlDiff = $get('pnlAmountDifference');
    var lblDiff = $get('lblAmountDifference');
    if (diff > 0) {
        pnlDiff.style.display = 'block';
        lblDiff.innerHTML = '-$' + FormatAsNumber(diff.toFixed(2));
    } else if (diff < 0) {
        pnlDiff.style.display = 'block';
        lblDiff.innerHTML = '+$' + FormatAsNumber(Math.abs(diff).toFixed(2));
    } else {
        pnlDiff.style.display = 'none';
    }
}
function SetVariance() {
    var isCMMSPO = $get('hidIsCMMSPO').value;
    var cmmsInvAmt = $get('lblCMMSInvoiceAmount');
    var pnlVariance = $get('pnlVariance');
    var pnlClosePO = $get('pnlClosePO');
    var chkClosePO = $get('chkClosePO');
    var hidClosePo = $get('hidClosePo');
    var pnlAmountDifference = $get('pnlAmountDifference');
    if (cmmsInvAmt != null) {
        if (isCMMSPO && cmmsInvAmt.innerText != "") {
            pnlAmountDifference.style.display = 'none';
            var ia = parseFloat(GetFloat('txtInvoiceAmount').toFixed(2));
            var cmmsia = parseFloat(cmmsInvAmt.innerText.substring(1));
            var variance = Math.abs(cmmsia - ia);
            pnlVariance.style.display = 'block';
            pnlClosePO.style.display = 'block';
            var lblVariance = $get('lblVariance');
            lblVariance.innerHTML = '$' + FormatAsNumber(variance.toFixed(2));
            //IsLastInvoice();
            var originalApprovalType = $get('hidNeedCMMSPOApproveType').value;
            if (variance == 0) {
                lblVariance.title = 'No Variance between CMMS Invoice Amount and Invoice Amount.';
                if (originalApprovalType != "Reviewer") {
                    $get('hidNeedCMMSPOApproveType').value = "Default";
                }
                SetCMMSApprove(false);
                IsLastInvoice();
            }
            else {
                //alert('originalApprovalType: ' + originalApprovalType);
                if (!IsVarianceInTolerance(variance, cmmsia)) {
                    if (originalApprovalType != "Reviewer") {
                        $get('hidNeedCMMSPOApproveType').value = "Approver";
                    }
                    //$get('hidNeedCMMSPOApproveType').value = "Approver";
                    SetCMMSApprove(true);
                    chkClosePO.checked = false;
                    hidClosePo.value = false;
                }
                else {
                    if (originalApprovalType != "Reviewer") {
                        $get('hidNeedCMMSPOApproveType').value = "Default";
                        SetCMMSApprove(false);
                    }
                    else {
                        SetCMMSApprove(true);
                    }
                    
                    IsLastInvoice();
                }
                lblVariance.title = 'The Variance between CMMS Invoice Amount and Invoice Amount is $' + FormatAsNumber(variance.toFixed(2)) + '.';
            }
        }
        else {
            pnlVariance.style.display = 'none';
            pnlClosePO.style.display = 'none';
            pnlAmountDifference.style.display = 'block';
        }
    }
}
function IsVarianceInTolerance(variance, cmmsia) {
    if (variance > $get('hidIPFixedAmount').value) {
        var variancePercentage = (variance / cmmsia) * 100;
        if (variancePercentage > $get('hidIPFixedPercentage').value) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}
function IsLastInvoice() {
    //var poNumber = $('#txtCMMSPONumber').val();
    //PageMethods.PM_IsLastInvoice(poNumber, onCheckSuccess);

    var param = { poNumber: $('#txtCMMSPONumber').val() };
    $.ajax({
        type: "POST",
        url: "AddUpdate.aspx/IsLastInvoice",
        data: JSON.stringify(param),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (isLastInvoice) {
            if (isLastInvoice.d == 1) {
                $('#chkClosePO').prop("checked", true);
                $get('hidClosePo').value = true;
            }
            else {
                $('#chkClosePO').prop("checked", false);
                $get('hidClosePo').value = true;
            }
        },
        change: function (event) {
            event.preventDefault();
        }
    });
}
function CheckTolerence() {
    var cmmsInvAmt = $get('lblCMMSInvoiceAmount');
    var ia = parseFloat(GetFloat('txtInvoiceAmount').toFixed(2));
    var cmmsia = parseFloat(cmmsInvAmt.innerText.substring(1));
    var variance = Math.abs(cmmsia - ia);
    if (variance == 0) {
        return true;
    }
    else {
        if (!IsVarianceInTolerance(variance, cmmsia)) {
            return false;
        }
        else {
            return true;
        }  
    }
}

function ClosePOCheckChange(c) {
    if (c.checked == ($get('hidClosePo').value.toLowerCase() == 'true')) {
        //alert("original current: " + $get('hidNeedCMMSPOApproveType').value);
        //$get('hidOriginalApproveType').value = $get('hidNeedCMMSPOApproveType').value;
        //var inTolerence = CheckTolerence();
        var approverType = $get('hidOriginalApproveType').value;
        //alert("original type:: " + approverType);
        $get('hidNeedCMMSPOApproveType').value = approverType;
        if (approverType != "Default") {
            SetCMMSApprove(true);
        }
        else {
            SetCMMSApprove(false);
        }

    }
    else {
        //alert("new: " + $get('hidNeedCMMSPOApproveType').value);
        //alert('CMMS' + gCMMSUser);
        $get('hidOriginalApproveType').value = $get('hidNeedCMMSPOApproveType').value;
        $get('hidNeedCMMSPOApproveType').value = "Reviewer";
        SetCMMSApprove(true);
    }
}

function SetCMMSApprove(needApprove) {
    var btnApprovePO = $get('btnApprovePO');
    var isCMMSPO = $get('hidIsCMMSPO').value;
    if (isCMMSPO) {
        $get('hidNeedCMMSPOApprove').value = needApprove;
        //alert("call" + needApprove);
        if (needApprove) {
            var approverType = $get('hidNeedCMMSPOApproveType').value;
            //alert("Final type: " + approverType);
            //alert("CMMSUser : " + $get('hidCMMSUserType').value);
            //alert("CMMSUser Type : " + typeof gCMMSUser);
            ////alert("CMMSUser 2 : " + gCMMSUser);
            if (approverType == "Reviewer") {
                if ($get('hidCMMSUserType').value == "IPCMMSReviewer") {
                    btnApprovePO.value = 'Sign & Send to Approver';
                    $get('hidNeedCMMSPOApproveType').value = "Approver";
                }
                else {
                    btnApprovePO.value = 'Sign & Send to Reviewer';
                }
            }
            else {
                btnApprovePO.value = 'Sign & Send to Approver';
            }
             
        }
        else {
            btnApprovePO.value = 'Sign & Approve';
        }
    }
}

function CalcDueDate(invDate, terms, static) {
    dueDate = new Date(invDate);
    dueDate.setDate(invDate.getDate() + terms);
    if (static)
        $get('lblDueDate400').innerHTML = dueDate.format(dateFormat);
    else
        $get('dteDueDate_txtDate').value = dueDate.format(dateFormat);
    return dueDate;
}
function CalcInvoiceDate(dueDate, terms) {
    invDate = new Date(dueDate);
    invDate.setDate(dueDate.getDate() - terms);
    $get('dteInvoiceDate_txtDate').value = invDate.format(dateFormat);
    return invDate;
}
function CalcTerms(invDate, dueDate) {
    terms = invDate.dayDiff(dueDate);
    $get('txtTermsDays').value = terms;
    return terms;
}
function RefreshDups() {
    var btnDups = $get('btnRefreshDups');
    if (btnDups != null)
        btnDups.click();
}
function CopyAmount() {
    $get('txtAmountToPay').value = $get('txtInvoiceAmount').value;
    SaveField('AmountToPay', $get('txtAmountToPay'));
}
function GetFloat(field) {
    if (typeof (field) == "string")
        field = $get(field)
    return parseFloat(field.value.replace(/\,/g, ""));
}
onbeforeunload = function () {
    if (gPRCount > 0)
        return 'There are still pending updates.  Exiting this page may cause them to not complete.';
}
//callback Mathods
function onSuccess() { gPRCount--; }
function refreshAll(msg, cancelRefresh) {
    gPRCount--;
    if (!cancelRefresh) {
        RefreshOpener();
        RefreshDups();
    }
}
function refreshOpener() {
    gPRCount--;
    RefreshOpener();

}

function invAmtSuccess(formattedValue, copyToATP) {
    gPRCount--;
    //    $get('txtInvoiceAmount').value = formattedValue;
    //    if (copyToATP) {
    //        $get('txtAmountToPay').value = formattedValue;
    //        SetTaxRate();
    //    }
    RefreshOpener();
    RefreshDups();
}
function onFailure(msg) {
    gPRCount--;
    alert('There was an error saving your change.  Please refresh the page and try again.');
}

$(document).ready(function () {
    $("#txtCMMSPONumber").autocomplete({
        source: function (request, response) {
            var param = { prefixText: $('#txtCMMSPONumber').val() };
            $.ajax({
                url: "AddUpdate.aspx/GetCMMSPONumber",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item,
                            label: item
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Unable to get POs.");
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtCMMSPONumber").val(ui.item.label);
            $('#btnGetCMMSPODetails').click();
        },
        change: function (event, ui) {
            event.preventDefault();
            $('#btnGetCMMSPODetails').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
        }
    })
        .focus(function () {
            $('#AddCMMSPOFocus').show();
        })
        .blur(function () {
            $('#AddCMMSPOFocus').hide();
        });
});