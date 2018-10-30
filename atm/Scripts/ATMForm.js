function SetPaymentQty(c, pid) {
    if (ValidateQuantity($(c), $(c).attr("allowQuarters"), $(c).attr("max"), true)) {
        AddAJAXRequest();
        PageMethods.PM_SetPaymentQty(gUserName, gFormId, pid, $(c).val(), onSetQtySuccess, onFailed, c.id);
    }
}

function SetWeekending(val) {
    var dd = new Date(val);
    if (dd.isValid()) {
        var di = dd.getDay();
        if (di != 6)
            dd.setDate(dd.getDate() + (6 - di));
        $get('dteWeekendingDate').value = dd.format("MM/dd/yyyy");
    } else {
        $get('dteWeekendingDate').value = '';
    }
}

function pullRouteInfo() {
    var departDate = $("#dteDepartDate");
    var routeNo = $("#txtRouteNo");
    var routeNoVal;
    var departDateVal = departDate.val();
    //alert($('#ddSygmaCenterNo').val());
    if (($('#ddFormType').val() == 0 || $('#ddFormType').val() == 1) && $('#ddSygmaCenterNo').val() != '') {
        dateReg = /^([1-9]|0[1-9]|1[012])[/]([1-9]|0[1-9]|[12][0-9]|3[01])[/](19|20)\d\d$/;
        if (departDateVal == '') {
            return;
        }
        if (!departDateVal.match(dateReg)) {
            return;
        }
        if (routeNo.val() == '') {
            $('#txtCasesOnCreate').val('');
            $('#txtPoundsOnCreate').val('');
            $('#txtCubesOnCreate').val('');
            $('#txtStopsOnCreate').val('');
            return;
        }
        if ($('#ddFormType').val() == 1)
            routeNoVal = 'S' + routeNo.val();
        else
            routeNoVal = routeNo.val();
        PageMethods.PM_GetRouteInfo(routeNoVal, $('#ddSygmaCenterNo').val(), departDateVal, onPullInfoSuccess, onFailed);
    }
}

function SetWeekendingDate(c) {
    var val = c.val();
    var dateReg = /^([1-9]|0[1-9]|1[012])[/]([1-9]|0[1-9]|[12][0-9]|3[01])[/](19|20)\d\d$/;
    var dd = new Date(val);
    if (val.match(dateReg) && dd.isValid()) {
        var di = dd.getDay();
        if (di != 6)
            dd.setDate(dd.getDate() + (6 - di));
        $get('dteWeekendingDate').value = dd.format("MM/dd/yyyy");
    } else {
        alert('Invalid Weekending Date!!!');
        $get('dteWeekendingDate').value = '';
    }
}

function SetFormWeekending(val) {
    var dd = new Date(val);
    if (dd.isValid()) {
        var di = dd.getDay();
        if (di != 6)
            dd.setDate(dd.getDate() + (6 - di));
        $get('lblWeekending').innerHTML = dd.format("MM/dd/yyyy");
        $get('dteFormWeekendingDate').value = dd.format("MM/dd/yyyy");
        UpdateWeekendingDate(dd.format("MM/dd/yyyy"));
    } else {
        alert('Error: Route# already exists in this Weekending!!!');
        $get('dteFormWeekendingDate').value = $get('lblWeekending').innerHTML;
    }
}

function SetRouteNoPrefix(c) {
    var copt = c.options[c.selectedIndex]
    var pre = copt.text.substring(0, 1);
    if (pre == 'L') {
        pre = 'H';
    }
    if (pre == 'R') {
        $get('lblRouteNoPrefix').innerHTML = '';
        $get('routeNo').title = '';
        var txtRoute = document.getElementById('txtRouteNo');
        txtRoute.value = "";
        txtRoute.maxLength = "4";
    }
    else {
        $get('lblRouteNoPrefix').innerHTML = pre + ' ';
        $get('routeNo').title = copt.text + ' forms must contain a route # that begins with an "' + pre + '"';
        var txtRoute = document.getElementById('txtRouteNo');
        txtRoute.value = "";
        txtRoute.maxLength = "3";
    }

}
function SavePlanInfo(c, vt) {
    if (c.value.isNumeric()) {
        var rval = Math.round(c.value);
        AddAJAXRequest();
        PageMethods.PM_SavePlanInfo(gUserName, gFormId, vt, rval, onSuccess, onFailed);

        c.value = rval;
    } else {
        alert('Value entered in "' + vt + '".  Please enter a valid number!');
        c.select();
    }
}
function ChangePayScale(c, eid, de) {
    AddAJAXRequest();
    PageMethods.PM_ChangePayScale(gUserName, gFormId, eid, c.options[c.selectedIndex].value, de, onPayScaleSuccess, onFailed, new Array(c.id, eid));
}

function ValidateFormDate(c) {
    var val = c.val();
    SetFormWeekending(val);
}

function ValidateFormWeekending(c) {
    var val = c.val();
    var departDate = $('#lblDepartDate').text();
    alert(departDate);
    if (isFormDate(val)) {
        var dd = new Date(val);
        if (dd.isValid()) {
            var di = dd.getDay();
            if (di != 6)
                dd.setDate(dd.getDate() + (6 - di));
            $get('dteFormWeekendingDate').value = dd.format("MM/dd/yyyy");
            $get('lblWeekending').innerHTML = dd.format("MM/dd/yyyy");
            UpdateWeekendingDate(dd.format("MM/dd/yyyy"));
        } else {
            alert('Error: Route# already exists in this Weekending!!!');
            $get('dteFormWeekendingDate').value = $get('lblWeekending').innerHTML;
        }
        $get('lblWeekending').innerHTML = $get('dteFormWeekendingDate').value;
    }
    else {
        alert('Invalid Date Format');
        $get('dteFormWeekendingDate').value = $get('lblWeekending').innerHTML;
    }
}

function UpdateWeekendingDate(txtWeekend) {
    var val = txtWeekend;
    PageMethods.PM_UpdateWeekendingDate(gUserName, gFormId, val, onWeekendingChangeSuccess, onFailed);
}

function isFormDate(txtDate) {


    var currVal = txtDate;
    if (currVal == '')
        return false;

    var rxDatePattern = /^(0[1-9]|1[012]|[1-9])[\/](0[1-9]|[12][0-9]|3[01]|[1-9])[\/](19|20)\d\d$/; //Declare Regex
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for MM/DD/YYYY format.

    dtMonth = dtArray[1];

    dtDay = dtArray[2];

    dtYear = dtArray[3];


    alert('Month' + dtMonth)
    alert('Day' + dtDay)
    alert('year' + dtYear)
    if (dtDay < 1 || dtDay > 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function ApplyHolidayPay(c) {
    var cbChecked = c.checked;
    var cTxtQuantity, cQuantity;
    if (cbChecked) {
        cTxtQuantity = $get('txtAddRateQuantity');
        cQuantity = cTxtQuantity.value;
        cTxtQuantity.value = cQuantity * 1.5;
        ValidateQuantity($('#txtAddRateQuantity'), GetOption($('#ddAddRateType')).attr('allowQuarters'), false);
    }
    else {
        cTxtQuantity = $get('txtAddRateQuantity');
        cQuantity = cTxtQuantity.value;
        cTxtQuantity.value = cQuantity / 1.5;
        ValidateQuantity($('#txtAddRateQuantity'), GetOption($('#ddAddRateType')).attr('allowQuarters'), false);
    }

}
function ValidateQuantity(c, aq, max, ve) {
    var val = c.val();
    var allowQ = +aq;
    if (val != "") {
        if (val.isNumeric()) {
            var qty = parseFloat(val);
            var rtVal = c.attr('rateTypeId');
            if (qty % 1 != 0) {
                if (aq == '2') {
                    qty = Math.round(qty * 10000) / 10000;
                }
                else if (aq == '1') {
                    qty = Round(qty, .25);
                }
                else {
                    qty = Round(qty, 1);
                }
                c.val(qty);
            }
            $('#paymentDialog').parent().appendTo($('form:first'));
            $('#btnRatePreview').click();
            return true;
        } else {
            RollbackField(c)
            alert('The value supplied is not a valid number!');
        }
    } else if (ve) {
        RollbackField(c);
        alert("'Quantity' is required!");

    } else {
        $('#btnRatePreview').click();
        return true;
    }
    return false;
}

function SaveOdometer(c, fvid, type) {
    var val = $(c).val();
    if (val != "") {
        val = val.replace(/\,/g, '');
        if (val.isNumeric && (val >= 0)) {
            var sibName = (type == "end") ? "#txtBeginOdometer" : "#txtEndOdometer";
            var sibVal = $(c).parents().eq(2).find(sibName).val().replace(/\,/g, '');
            if (sibVal != "") {
                if (type == "end" && (parseInt(val) < parseInt(sibVal))) {
                    RollbackField(c);
                    alert("'End Odometer' must be greater than or equal to 'Begin Odometer'!");
                    return;
                } else if (type == "begin" && (parseInt(val) > parseInt(sibVal))) {
                    RollbackField(c);
                    alert("'Begin Odometer' must be less than or equal to 'End Odometer'!");
                    return;
                }
                if (type == "end" && ((parseInt(val) - parseInt(sibVal)) > 1000000)) {
                    RollbackField(c);
                    alert("Warning: Difference between Begin and End Odometer is huge! Please enter correct odometer reading");
                    return;
                }
                else if (type == "begin" && ((parseInt(sibVal) - parseInt(val)) > 1000000)) {
                    RollbackField(c);
                    alert("Warning: Difference between Begin and End Odometer is huge! Please enter correct odometer reading");
                    return;
                }
                if (type == "end" && ((parseInt(val) - parseInt(sibVal)) > 2000)) {
                    alert("Warning: Actual Miles is greater than 2000! Please change if it needs any modification");
                }
                else if (type == "begin" && ((parseInt(sibVal) - parseInt(val)) > 2000)) {
                    alert("Warning: Actual Miles is greater than 2000! Please change if it needs any modification");
                }
            }
            if (sibVal == "" && sibName == "#txtBeginOdometer") {
                RollbackField(c);
                alert("Please enter 'Begin Odometer' before entering 'End Odometer'!");
                return;
            }
            c.value = FormatAsNumber(val);
            PageMethods.PM_SetVehicleOdometer(gUserName, gFormId, fvid, val, type, onOdometerSuccess, onFailed, $(c));
        } else {
            RollbackField(c);
            alert('Odometer is not a valid positive number!');
        }
    } else {
        RollbackField(c);
        alert('Odometer can not be empty!');
    }
}

function confirmUpdateMilesPayment() {
    if (confirm("There are one or more employees with miles payments. Do you wish to update them and add base miles payment to other employees in the form?")) {
        PageMethods.PM_UpdateEmployeesMilesPayment(gUserName, gFormId, onUpdMilesPaySuccess, onFailed, gFormId);
    }
}

function SaveExternalVehicleId(c, fvid) {
    PageMethods.PM_SetExternalVehicleID(gUserName, fvid, $(c).val(), onSuccess, onFailed);
}

function SaveHours(c, tid, type) {
    var val = c.value;
    if (val != "") {
        val = val.replace(/\,/g, '');
        if (val.isNumeric && (val >= 0)) {
            //if ((val.indexOf('.') == -1 && val.length <= 8) || (((val.length - val.indexOf('.')) - 1 < 2) && val.indexOf('.') <= 8) ) {
            if ((val.indexOf('.') == -1 && val.length <= 5) || (((val.length - val.indexOf('.')) - 1 < 2) && val.indexOf('.') <= 5)) {
                var sibName = (type == "end") ? "#txtBeginHours" : "#txtEndHours";
                var sibVal = $(c).parents().eq(2).find(sibName).val().replace(/\,/g, '');
                if (sibVal != "") {
                    if (type == "end" && (parseInt(val) < parseInt(sibVal))) {
                        RollbackField(c);
                        alert("'End Hours' must be greater than or equal to 'Begin Hours'!");
                        return;
                    } else if (type == "begin" && (parseInt(val) > parseInt(sibVal))) {
                        RollbackField(c);
                        alert("'Begin Hours' must be less than or equal to 'End Hours'!");
                        return;
                    }
                }
                c.value = FormatAsNumber(val);
                PageMethods.PM_SetTrailerHours(gUserName, gFormId, tid, val, type, onHoursSuccess, onFailed, $(c));
            }
            else {
                RollbackField(c);
                alert('Please enter a valid hours! Maximum allowed value is 99999.9! (Example:1.2 or 1)');
            }
        } else {
            RollbackField(c);
            alert('Hours is not a valid positive number!');
        }
    }
    else {
        RollbackField(c);
        alert('Hours can not be empty!');
    }
}
function SaveBackhaul(c, bid) {
    var pono = null, revenue = null;
    if (c.id == 'txtBHPoNo') {
        pono = c.value;
        if (pono.trim() == '') {
            alert('PO # can not be empty!');
            return false;
        }
    } else if (c.id == 'txtBHRevenue') {
        revenue = c.value;
        revenue = revenue.replace(/\,/g, '');
        if (revenue.trim() == '')
            revenue = 0;
        if (revenue.isNumeric) {
            c.value = FormatAsNumber(revenue);
        } else {
            alert('Revenue must be a valid positive number!');
            return false;
        }
    }
    PageMethods.PM_SaveBackhaul(gUserName, bid, pono, revenue, onSuccess, onFailed);
}

function onUpdMilesPaySuccess(ret, fid) {
    RemoveAJAXRequest();
    if (ret[1] == "1") {
        SetLastUpdated(ret[0]);
        $("#btnRefreshEmployees").click();
        alert("Miles Payment for all employees in the form#" + fid + " is updated successfully!");
    }
    else {
        alert("There is no pay scales of any employee in the form has marked Miles as base payment! \nOr there is no employees in the form!");
    }
}

function onPayScaleSuccess(msg, arr) {
    RemoveAJAXRequest();
    var dd = $get(arr[0]);
    if (!msg[0]) {
        if (!confirm(msg[1])) {
            dd.selectedIndex = dd.getAttribute('prev');
        } else {
            ChangePayScale(dd, arr[1], true);
        }
    } else {
        dd.setAttribute('prev', dd.selectedIndex);
        var thisThing = $('#btnUpdatePayScale');
        thisThing.click();
        SetLastUpdated(msg[1]);
        $("#btnRefreshEmployees").click();
        FocusAddPayment();
    }
}
function onSetQtySuccess(rv, cid) {
    RemoveAJAXRequest();
    var c = $get(cid);
    var cLineTotal = $get(cid.replace('txtQty', 'lblTotal'));
    var prevLineTotal = parseFloat(cLineTotal.innerHTML.replace(/\,/g, ''));
    var newLineTotal = parseFloat(rv[0]).toFixed(2);
    UpdateField(c);
    cLineTotal.innerHTML = FormatAsNumber(newLineTotal);

    var index = cid.indexOf('_txtQty');
    var eTotalId = cid.substring(0, index).replace('rptEmployeePayments', 'lblEmployeeTotal');
    var cGrandTotal = $get(eTotalId);
    var grandTotal = parseFloat(cGrandTotal.innerHTML.replace(/\,/g, ''));
    cGrandTotal.innerHTML = FormatAsNumber((grandTotal + (newLineTotal - prevLineTotal)).toFixed(2));
    SetLastUpdated(rv[1]);
    if (rv[2] == '1') {
        alert('Total Miles Quantity is greater than actual miles!');
    }
}
function SetLastUpdated(ld) {
    if (ld != null && ld != '')
        $('#lblLastUpdated').html(ld);
}
function onSuccess(ld) {
    RemoveAJAXRequest();
    SetLastUpdated(ld);
}
function onPullInfoSuccess(ri) {
    RemoveAJAXRequest();
    if (ri[0] != '0') {
        $('#txtCasesOnCreate').val(ri[1]);
        $('#txtPoundsOnCreate').val(ri[2]);
        $('#txtCubesOnCreate').val(ri[3]);
        $('#txtStopsOnCreate').val(ri[4]);
    }
    else {
        $('#txtCasesOnCreate').val('');
        $('#txtPoundsOnCreate').val('');
        $('#txtCubesOnCreate').val('');
        $('#txtStopsOnCreate').val('');
    }

}
function onSuccessInfo(qVal, c) {
    RemoveAJAXRequest();
    var rtOption = GetOption(c);
    var cQuantity;
    if (qVal != 0) {
        cQuantity = $get('txtAddRateQuantity');
        cQuantity.value = qVal;
        ValidateQuantity($('#txtAddRateQuantity'), rtOption.attr("allowQuarters"), false);
    }
    else {
        cQuantity = $get('txtAddRateQuantity');
        cQuantity.value = '';
        $('#btnCategoryRefresh').click();
    }
}
function onHoursSuccess(ld, c) {
    RemoveAJAXRequest();
    UpdateField(c);
    SetLastUpdated(ld);
}

var t;
function ShowMessage() {
    if (t)
        clearTimeout(t);
    var pnl = $get('pnlWarning');
    pnl.style.display = 'block';
    t = setTimeout("$get('pnlWarning').style.display = 'none';", 10000);
}


function onOdometerSuccess(rv, c) {
    RemoveAJAXRequest();
    $("#lblMiles").html(rv[0]);
    UpdateField(c);
    SetLastUpdated(rv[1]);
    if (rv[2] != '') {
        $(".hlOverlap" + rv[3]).attr('href', document.URL.split("?")[0] + '?fid=' + rv[2]);
        $(".hlOverlap" + rv[3]).text('Warning: odometer reading overlaps with Form ' + rv[2]);
        $(".hlOverlap" + rv[3]).show();
    }
    else {
        $(".hlOverlap" + rv[3]).hide();
    }
    if (rv[5] == "1") {
        confirmUpdateMilesPayment();
    }
    else if (rv[4] == "1") {
        $("#btnRefreshEmployees").click();
    }
}

function onSuccessAddPayment(rv, button) {
    RemoveAJAXRequest();
    SetLastUpdated(rv[0]);
    if (rv[1] != '0')
        alert('selected rate type cannot be applied for the pay scale of ' + rv[1]);
    if (rv[2] == '1') {
        alert('Total Miles Quantity for one or more employees is greater than actual miles!');
    }
    $(button).click();
    $("#mhcategory").hide();
}

function onSuccessClickButton(rv, button) {
    RemoveAJAXRequest();
    SetLastUpdated(rv);
    $(button).click();
}

function onSuccessAddClickButton(rv, button) {
    RemoveAJAXRequest();
    SetLastUpdated(rv[0]);
    $("#selFTdialog").attr("Add", 'F');
    $('#hfVFTDialogBoxCall').val("");
    $('#hfTFTDialogBoxCall').val("");
    if (rv[4] == 'Vehicle') {
        $('#hfVFTDialogBoxCall').val("OpenFTDialog('" + rv[1] + "', '" + rv[2] + "', '" + rv[3] + "', '" + rv[4] + "');");
    }
    else {
        $('#hfTFTDialogBoxCall').val("OpenFTDialog('" + rv[1] + "', '" + rv[2] + "', '" + rv[3] + "', '" + rv[4] + "');");
    }

    $(button).click();
    //$("[data-refreshTrigger='add_refresh_trigger_vehicle_" + rv[3] + "']").click();

    //alert(rv[1] + ' | ' + rv[2]  + ' | ' + rv[3]  + ' | ' + rv[4]);
    //setTimeout(OpenFTDialog(rv[1], rv[2], rv[3], rv[4]), 1000);
}

function onSuccessAddFT(rv, c) {
    RemoveAJAXRequest();
    var ftType = $(c).attr('fttype');
    var item = $(c).attr('item');
    var num = $('#txtAddFTNo').val();
    var date = $('#txtAddFTDate').val();
    var gallons = $('#txtAddFTGallons').val();
    var qty = $('#txtAddFTQty').val();
    var amount = $('#txtAddFTAmount').val();
    var eid = $(c).attr('eid');
    var feid = $(c).attr('feid');
    if (rv == 0) {
        refreshButton = "[data-refreshTrigger='add_refresh_trigger_" + ftType.toLowerCase() + "_" + eid + "']";
        AddAJAXRequest();
        PageMethods.PM_AddFuelTicket(gUserName, eid, num, date, gallons, qty, amount, feid, ftType, item, onSuccessAddClickButton, onFailed, refreshButton);
        $(c).dialog("close");
    }
    else {
        alert('A fuel ticket with this Odometer already exists!');
    }
}
function onSuccessUpdateFT(rv, c) {
    RemoveAJAXRequest();
    var ftType = $(c).attr('fttype');
    var num = $('#txtAddFTNo').val();
    var item = $(c).attr('item');
    var date = $('#txtAddFTDate').val();
    var gallons = $('#txtAddFTGallons').val();
    var qty = $('#txtAddFTQty').val();
    var amount = $('#txtAddFTAmount').val();
    var ftid = $(c).attr('ftid');
    var eid = $(c).attr('eid');
    if (rv == 0) {
        refreshButton = "[data-refreshTrigger='update_refresh_trigger_" + ftType.toLowerCase() + "_" + eid + "']";
        //refreshButton = '#' + item + '_btnRefresh' + ftType + 'FT';
        AddAJAXRequest();
        //alert(gUserName + ' ' + num + ' ' + date + ' ' + gallons + ' ' + qty + ' ' + amount + ' ' + ftid + ' ' + ftType);
        PageMethods.PM_UpdateFuelTicket(gUserName, num, date, gallons, qty, amount, ftid, ftType, onSuccessClickButton, onFailed, refreshButton);
        $(c).dialog("close");
    }
    else {
        alert('A fuel ticket with this Odometer already exists!');
    }
}

function onSuccessBkClickButton(rv, button) {
    RemoveAJAXRequest();
    SetLastUpdated(rv[0]);
    $('#hfOpenAddPayment').val(rv[1]);
    $(button).click();
    //            if(rv[1] == "1"){
    //                OpenPaymentDialog();
    //            }
}

function onSuccessTLClickButton(rv, button) {
    RemoveAJAXRequest();
    SetLastUpdated(rv);
    $(button).click();
}

function onFTSuccess(rv, item) {
    RemoveAJAXRequest();
    $('#' + item[0] + '_btnRefresh' + item[1] + 'FT').click();
}


function onStatusChangeSuccess(sid) {
    RemoveAJAXRequest();
    if (sid[0] != "3") {
        if (sid[1] != "1") {
            alert("Please enter odometer reading for all the vehicles in this form!");
        }
        else {
            $("#txtMessages").attr("value", "Form has been " + status + " Successfully");
            __doPostBack('txtMessages', '');
        }
    }
    else {
        $("#txtMessages").val("Form has been " + status + " Successfully");
        $('#lblStatus').html('Approved');
        $('#imgStatus').attr("src", "../../../../Images/Icons/Approved.png");
        $('#pnlChangeStatusAppr').css('visibility', 'hidden');
    }
    //RefreshOpener('ctl00_body_btnRefresh');

}

function onWeekendingChangeSuccess(rv) {
    if (rv == 1) {
        RemoveAJAXRequest();
    }
    else {
        alert('Weekending Date cannot be less than Depart Date!');
    }
}

function onFailed(error) {
    RemoveAJAXRequest();
    alert('There was an error saving the field.  Please refresh the page and try again.');
}

function onSuccessClose(rv) {
    RemoveAJAXRequest();
    RefreshOpener('ctl00_body_btnRefresh');
    window.close();
}

function OpenFTDialog(item, eid, feid, fttype) {
    ClearFTDialog();
    $("#ftdialog").dialog("open");
    $("#ftdialog").attr('eid', eid);
    $("#ftdialog").attr('feid', feid);
    $("#ftdialog").attr('item', item);
    $("#ftdialog").attr('fttype', fttype);
    $("#ftdialog").attr('focuselement', (fttype == "Vehicle") ? 'body .vehicleFTFocus' : 'body .trailerFTFocus'); //'#' + item + '_imgAddVFT' : '#hrefTest');
    $("#ftdialog").attr('isupdate', 0);
    $("#ftdialog").dialog("option", "title", "Add " + fttype + " Fuel Ticket");
    $("#ftQtyType").html((fttype == "Vehicle") ? 'Odometer' : 'Hour Reading');
    $("#txtAddFTNo").focus();
    OpenExistingAdd(eid, fttype, feid);
    return false;
}

function AddUpdateFT(c) {
    var ftType = $(c).attr('fttype');
    var isupdate = $(c).attr('isupdate');
    var item = $(c).attr('item');
    var num = $('#txtAddFTNo').val();
    var date = $('#txtAddFTDate').val();
    var gallons = $('#txtAddFTGallons').val();
    var qty = $('#txtAddFTQty').val();
    var amount = $('#txtAddFTAmount').val();
    var refreshButton;
    dateReg = /^([1-9]|0[1-9]|1[012])[/]([1-9]|0[1-9]|[12][0-9]|3[01])[/](19|20)\d\d$/;
    if (num != '') {
        if (date != '') {
            if (date.match(dateReg)) {
                if (gallons.isNumeric() && gallons > 0 && ((gallons.indexOf('.') == -1 && gallons.length < 9) || ((gallons.length - gallons.indexOf('.')) - 1 < 3 && gallons.indexOf('.') < 8))) {
                    if (qty.isNumeric() && qty > 0) {
                        if (amount.isNumeric() && amount > 0) {
                            if (isupdate != 1) {
                                var eid = $(c).attr('eid');
                                var feid = $(c).attr('feid');
                                //refreshButton = '#' + item + '_btnRefreshAdd' + ftType + 'FT';
                                refreshButton = "[data-refreshTrigger='add_refresh_trigger_" + ftType.toLowerCase() + "_" + eid + "']";
                                AddAJAXRequest();
                                if (ftType == "Vehicle") {
                                    PageMethods.PM_CheckFuelTicketsOdometer(feid, qty, 'ADD', onSuccessAddFT, onFailed, c);
                                }
                                else {
                                    PageMethods.PM_AddFuelTicket(gUserName, eid, num, date, gallons, qty, amount, feid, ftType, item, onSuccessAddClickButton, onFailed, refreshButton);
                                    $(c).dialog("close");
                                }
                            }
                            else {
                                var eid = $(c).attr('eid');
                                var ftid = $(c).attr('ftid');
                                //refreshButton = '#' + item + '_btnRefresh' + ftType + 'FT';
                                refreshButton = "[data-refreshTrigger='update_refresh_trigger_" + ftType.toLowerCase() + "_" + eid + "']";
                                AddAJAXRequest();
                                if (ftType == "Vehicle") {
                                    PageMethods.PM_CheckFuelTicketsOdometer(ftid, qty, 'UPD', onSuccessUpdateFT, onFailed, c);
                                }
                                else {
                                    PageMethods.PM_UpdateFuelTicket(gUserName, num, date, gallons, qty, amount, ftid, ftType, onSuccessClickButton, onFailed, refreshButton);
                                    $(c).dialog("close");
                                }
                            }
                        } else {
                            alert('Amount is not a valid positive number!');
                        }
                    } else {
                        alert(((ftType == 'Vehicle') ? 'Odometer reading ' : 'Hour Reading ') + 'is not a valid positive number!');
                    }
                } else {
                    alert('Gallons is not a valid positive number! Maximum 2 decimal only allowed!');
                }
            } else {
                alert('Date is invalid!');
            }
        } else {
            alert('Date is required!');
        }
    } else {
        alert('Fuel Ticket number is required!');
    }
}

function ClearFTDialog() {
    $('#txtAddFTNo').val('');
    $('#txtAddFTDate').val('');
    $('#txtAddFTGallons').val('');
    $('#txtAddFTQty').val('');
    $('#txtAddFTAmount').val('');
}

function OpenFTUpdateDialog(item, ftid, fttype, ftNo, ftDate, ftGallons, ftAmount, ftMiles, eid) {
    $('#txtAddFTNo').val(ftNo);
    $('#txtAddFTDate').val(ftDate);
    $('#txtAddFTGallons').val(ftGallons);
    $('#txtAddFTQty').val(ftMiles);
    $('#txtAddFTAmount').val(ftAmount);
    $("#ftdialog").dialog("open");
    $("#ftdialog").attr('ftid', ftid);
    $("#ftdialog").attr('item', item);
    $("#ftdialog").attr('isupdate', 1);
    $("#ftdialog").dialog("option", "title", "Update " + fttype + " Fuel Ticket");
    $("#ftQtyType").html((fttype == "Vehicle") ? 'Odometer' : 'Hour Reading');
    $("#ftdialog").attr('fttype', fttype);
    $("#ftdialog").attr('eid', eid);
    return false;
}

function OpenFTDeleteDialog(item, ftid, ftType, eid) {
    $("#ftdeletedialog").dialog("open");
    $("#ftdeletedialog").attr('ftid', ftid);
    $("#ftdeletedialog").attr('item', item);
    $("#ftdeletedialog").attr('fttype', ftType);
    $("#ftdeletedialog").attr('eid', eid);
    return false;
}

function RemoveFT(c, d) {
    var item = $(c).attr('item');
    var ftid = $(c).attr('ftid');
    var ftType = $(c).attr('ftType');
    var eid = $(c).attr('eid');
    //var refreshButton = '#' + item + '_btnRefresh' + ftType + 'FT';
    var refreshButton = "[data-refreshTrigger='update_refresh_trigger_" + ftType.toLowerCase() + "_" + eid + "']";
    PageMethods.PM_RemoveFuelTicket(gUserName, gFormId, ftid, d, ftType, onSuccessClickButton, onFailed, refreshButton);
    $(c).dialog("close");
}

function OpenTLDialog(item, eid) {
    ClearTLDialog();
    $("#tldialog").dialog("open");
    $("#tldialog").attr('eid', eid);
    $("#tldialog").attr('item', item);
    $("#tldialog").attr('isupdate', 0);
    $("#tldialog").dialog("option", "title", "Add Employee Time Log");
    return false;
}

function ClearTLDialog() {
    $('#txtAddTLStartDate').val(gRouteDepartDate);
    var year = new Date().getFullYear();
    $('#txtAddTLEndDate').val('/' + year);
    $("#ddAddStartHour").val(12);
    $("#ddAddStartMin").val(0);
    $("#ddAddStartAmPm").val("AM");
    $("#ddAddEndHour").val(12);
    $("#ddAddEndMin").val(0);
    $("#ddAddEndAmPm").val("AM");
    var lTotalHoursMin = $get('lblTotalHoursMin');
    lTotalHoursMin.innerHTML = '';
}

function OpenTLUpdateDialog(item, tlid, startDateTime, endDateTime, totalHours) {
    UpdateTLDialog(startDateTime, endDateTime, totalHours);
    $("#tldialog").dialog("open");
    $("#tldialog").attr('tlid', tlid);
    $("#tldialog").attr('item', item);
    $("#tldialog").attr('isupdate', 1);
    $("#tldialog").dialog("option", "title", "Edit Employee Time Log");
    return false;
}

function UpdateTLDialog(sDateTime, eDateTime, totalHours) {
    var year = new Date().getFullYear();
    var yearStr = '' + year;
    var sDate = sDateTime.substring(0, 6) + yearStr.substring(0, 2) + sDateTime.substring(6, 8);
    var sHour = sDateTime.substring(9, sDateTime.indexOf(":"));
    var sMin = sDateTime.substring(sDateTime.indexOf(":") + 1, sDateTime.indexOf(":") + 3);
    var sAmPm = sDateTime.substring(sDateTime.indexOf(":") + 3, sDateTime.indexOf(":") + 5);
    var eDate = eDateTime.substring(0, 6) + yearStr.substring(0, 2) + eDateTime.substring(6, 8);
    var eHour = eDateTime.substring(9, eDateTime.indexOf(":"));
    var eMin = eDateTime.substring(eDateTime.indexOf(":") + 1, eDateTime.indexOf(":") + 3);
    var eAmPm = eDateTime.substring(eDateTime.indexOf(":") + 3, eDateTime.indexOf(":") + 5);
    $('#txtAddTLStartDate').val(sDate);
    $('#txtAddTLEndDate').val(eDate);
    $("#ddAddStartHour").val(sHour);
    $("#ddAddStartMin").val(parseInt(sMin));
    $("#ddAddStartAmPm").val(sAmPm);
    $("#ddAddEndHour").val(eHour);
    $("#ddAddEndMin").val(parseInt(eMin));
    $("#ddAddEndAmPm").val(eAmPm);
    var lTotalHoursMin = $get('lblTotalHoursMin');
    lTotalHoursMin.innerHTML = totalHours.substring(0, totalHours.indexOf(".")) + ":" + String(Math.round(parseInt(totalHours.substring(totalHours.indexOf(".") + 1, totalHours.length)) * .6));
}

function AddUpdateTL(c) {
    var item = $(c).attr('item');
    var isupdate = $(c).attr('isupdate');
    var startDate = $('#txtAddTLStartDate').val();
    var ddStartDate = new Date(startDate);
    if (ddStartDate.getFullYear() < 1970) {
        ddStartDate.setFullYear(ddStartDate.getFullYear() + 100);
    }
    var endDate = $('#txtAddTLEndDate').val();
    var ddEndDate = new Date(endDate);
    if (ddEndDate.getFullYear() < 1970) {
        ddEndDate.setFullYear(ddEndDate.getFullYear() + 100);
    }
    var startHour = parseInt($('#ddAddStartHour').val());
    var startMin = parseInt($('#ddAddStartMin').val());
    var startAmPm = $('#ddAddStartAmPm').val();
    var endHour = parseInt($('#ddAddEndHour').val());
    var endMin = parseInt($('#ddAddEndMin').val());
    var endAmPm = $('#ddAddEndAmPm').val();
    var totalHoursMin = $('#lblTotalHoursMin').val();
    if (startAmPm == 'PM' && startHour != 12) {
        var sHour = parseInt(startHour);
        startHour = sHour + 12;
    }
    if (startAmPm == 'AM' && startHour == 12) {
        startHour = 0;
    }
    if (endAmPm == 'PM' && endHour != 12) {
        var eHour = parseInt(endHour);
        endHour = eHour + 12;
    }
    if (endAmPm == 'AM' && endHour == 12) {
        endHour = 0;
    }
    if (ddStartDate.isValid()) {
        if (ddEndDate.isValid()) {
            if (!startDate.match(/[a-z]/i) && startDate.length <= 10) {
                if (!endDate.match(/[a-z]/i) && endDate.length <= 10) {
                    if (String(ddStartDate) == String(ddEndDate)) {
                        if (startHour == endHour) {
                            if (startMin < endMin) {
                                if (isupdate == 0) {
                                    AddAJAXRequest();
                                    var refreshButton = '#' + item + '_btnRefreshEmployeeTL';
                                    var eid = $(c).attr('eid');
                                    PageMethods.PM_AddTimeLog(gUserName, eid, gFormId, startDate, endDate, startHour, endHour, startMin, endMin, onSuccessTLClickButton, onFailed, refreshButton);
                                    $(c).dialog("close");
                                }
                                else {
                                    AddAJAXRequest();
                                    var refreshButton = '#' + item + '_btnRefreshEmployeeTL';
                                    var tlid = $(c).attr('tlid');
                                    PageMethods.PM_UpdateTimeLog(gUserName, tlid, startDate, endDate, startHour, endHour, startMin, endMin, onSuccessTLClickButton, onFailed, refreshButton);
                                    $(c).dialog("close");
                                }
                            }
                            else {
                                alert('End Time should be greater than Start Time');
                            }
                        }
                        else if (startHour < endHour) {
                            if (isupdate == 0) {
                                AddAJAXRequest();
                                var refreshButton = '#' + item + '_btnRefreshEmployeeTL';
                                var eid = $(c).attr('eid');
                                PageMethods.PM_AddTimeLog(gUserName, eid, gFormId, startDate, endDate, startHour, endHour, startMin, endMin, onSuccessTLClickButton, onFailed, refreshButton);
                                $(c).dialog("close");
                            }
                            else {
                                AddAJAXRequest();
                                var refreshButton = '#' + item + '_btnRefreshEmployeeTL';
                                var tlid = $(c).attr('tlid');
                                PageMethods.PM_UpdateTimeLog(gUserName, tlid, startDate, endDate, startHour, endHour, startMin, endMin, onSuccessTLClickButton, onFailed, refreshButton);
                                $(c).dialog("close");
                            }
                        }
                        else {
                            alert('End Hour should be greater than Start Hour!!!');
                        }
                    }
                    else if (ddStartDate < ddEndDate) {
                        if (isupdate == 0) {
                            AddAJAXRequest();
                            var refreshButton = '#' + item + '_btnRefreshEmployeeTL';
                            var eid = $(c).attr('eid');
                            PageMethods.PM_AddTimeLog(gUserName, eid, gFormId, startDate, endDate, startHour, endHour, startMin, endMin, onSuccessTLClickButton, onFailed, refreshButton);
                            $(c).dialog("close");
                        }
                        else {
                            AddAJAXRequest();
                            var refreshButton = '#' + item + '_btnRefreshEmployeeTL';
                            var tlid = $(c).attr('tlid');
                            PageMethods.PM_UpdateTimeLog(gUserName, tlid, startDate, endDate, startHour, endHour, startMin, endMin, onSuccessTLClickButton, onFailed, refreshButton);
                            $(c).dialog("close");
                        }
                    }
                    else {
                        alert('End Date should be greater than or equal to Start Date!!!');
                    }
                }
                else {
                    alert('Enter Valid End Date!!! Date format is mm/dd/yyyy.');
                }
            }
            else {
                alert('Enter Valid Start Date!!! Date format is mm/dd/yyyy.');
            }
        }
        else {
            alert('Enter Valid End Date!!! Date format is mm/dd/yyyy.');
        }
    }
    else {
        alert('Enter Valid Start Date!!! Date format is mm/dd/yyyy.');
    }
}

function OpenTLDeleteDialog(item, tlid) {
    $("#tldeletedialog").dialog("open");
    $("#tldeletedialog").attr('tlid', tlid);
    $("#tldeletedialog").attr('item', item);
    return false;
}

function RemoveTL(c) {
    var item = $(c).attr('item');
    var tlid = $(c).attr('tlid');
    var refreshButton = '#' + item + '_btnRefreshEmployeeTL';
    PageMethods.PM_RemoveTimeLog(gUserName, gFormId, tlid, onSuccessTLClickButton, onFailed, refreshButton);
    $(c).dialog("close");
}

function OpenPaymentDialog() {
    ClearPaymentDialog();
    $("#paymentDialog").dialog("open");
}
function OpenBackhaulPaymentDialog() {
    ClearBackhaulPaymentDialog();
    $("#paymentDialog").dialog("open");
    $("#txtAddRateQuantity").focus();
}

function FocusAddPayment() {
    setTimeout(function () {
        $('#hlAddRateTypeDialog').focus();
    }, 500);
}

function FocusAddPaymentOnPayScaleChange() {
    if (document.activeElement == null) {
        setTimeout(function () {
            $('#hlAddRateTypeDialog').focus();
        }, 500);
    }
}

function FocusAddRateType() {
    $('#txtAddRateQuantity').removeClass("inputfocus");
    $('#cbHolidayPay').removeClass("inputfocus");
    setTimeout(function () {
        $('#ddAddRateType').focus();
    }, 500);
}

function FocusAddRateQuantity() {
    $('#cbHolidayPay').removeClass("inputfocus");
    $('#ddAddRateType').removeClass("inputfocus");
    $('#txtAddRateQuantity').removeClass("inputfocus");
    var cTxtQuantity = $get('txtAddRateQuantity');
    var cQuantity = cTxtQuantity.value;
    //alert(cQuantity);
    if (cQuantity == "") {
        setTimeout(function () {
            $('#txtAddRateQuantity').focus();
        }, 500);
    }
    else {
        setTimeout(function () {
            $('#txtAddRateNotes').focus();
        }, 500);
    }
}

function FocusHolidayCheckbox() {
    $('#txtAddRateQuantity').removeClass("inputfocus");
    setTimeout(function () {
        $('#cbHolidayPay').focus();
    }, 500);
}

function ClearPaymentDialog() {
    $("#ddAddRateType").val("");
    $("#ddAddRateTypeCategory").val("");
    $('#cbHolidayPay').attr('checked', false);
    $("#txtAddRateQuantity").val("");
    $("#txtAddRateNotes").val("");
    $("#lbAddRateApplyTos").val("");
    $("#mhcategory").hide();
    $('#btnRatePreview').click();
}
function ClearBackhaulPaymentDialog() {
    $("#ddAddRateType").val("6");
    $("#ddAddRateTypeCategory").val("");
    $('#cbHolidayPay').attr('checked', false);
    $("#txtAddRateQuantity").val("");
    $("#txtAddRateQuantity").focus();
    $("#txtAddRateNotes").val("");
    $("#lbAddRateApplyTos").val("");
    $("#mhcategory").hide();
    $('#btnRatePreview').click();
}

function AddPayment(c) {
    var rt = GetOption($("#ddAddRateType"));
    var cat = GetOption($("#ddAddRateTypeCategory"));
    var catVal = cat.val();
    var qty = $("#txtAddRateQuantity");
    var noteVal = $("#txtAddRateNotes").val().toString().trim();
    var cbHolPay = $('#cbHolidayPay').attr('checked');
    var isHolPay = 0;
    if ($('#cbHolidayPay').attr('checked')) {
        isHolPay = 1;
    }
    if (rt.val() == '') {
        alert('Must select a \'Rate Type\'!');
        return
    }

    if ((Boolean.parse(rt.attr("hasCategories"))) && (cat.val() == "")) {
        alert("Must select a 'Category!'");
        return;
    } else {
        catVal = cat.val();
    }

    if (!ValidateQuantity(qty, rt.attr("allowQuarters"), rt.attr("max"), true)) {
        return;
    }

    if (noteVal == "") {
        if (Boolean.parse(rt.attr("requireNote"))) {
            alert("'Note' is required!");
            setTimeout(function () {
                $("#txtAddRateNotes").focus();
            }, 500);
            return;
        } else {
            noteVal = null
        }
    }

    var applyToVals = $("#lbAddRateApplyTos").val();
    if (applyToVals == null) {
        applyToVals = new Array();
    }
    AddAJAXRequest();
    PageMethods.PM_AddPayment(gUserName, gFormId, rt.val(), catVal, qty.val(), noteVal, isHolPay, applyToVals, onSuccessAddPayment, onFailed, "#btnRefreshAddEmployees");
    $("#mhcategory").hide();
    $("#paymentDialog").dialog("close");
}

function AddBackhaul(c) {
    var poNo = $("#txtAddBackhaulPoNo").val();
    var revenue = $("#txtAddBackhaulRevenue").val();
    var addPayment = 0;
    if ($('#cbAddPayment').is(":checked")) {
        addPayment = 1;
    }
    if (poNo != "") {
        if (revenue != "") {
            if (revenue.toString().isNumeric() && (parseFloat(revenue) > 0)) {
                AddAJAXRequest();
                PageMethods.PM_AddBackhaul(gUserName, gFormId, poNo, revenue, addPayment, onSuccessBkClickButton, onFailed, "#btnRefreshBackhauls");
                $(c).dialog("close");
            } else {
                alert("'Revenue' must be a positive number!");
            }
        } else {
            alert("'Revenue' is required!");
        }
    } else {
        alert("'PO #' is required!");
    }
}
var status = ""; //Global Variable for getting the status.
function ChangeStatus(ns) {

    var action = "";
    if (ns == 0) {
        action = "Check out";
        status = "Checked out";

    }
    else if (ns == 1) {
        status = "Checked in";
        action = "Check in";
    }
    else if (ns == 3 || ns == 4) {
        action = (ns == 3 ? "Approve" : "Reject");
        status = (ns == 3 ? "Approved" : "Rejected");

    }
    StatusAction(ns, action);
}
function StatusAction(sid, type) {
    if (sid == 4) {
        $('#txtNotes').val('');
        $("#ndialog").dialog("open");
        $("#ndialog").attr('sid', sid);
        $("#ndialog").attr('type', type);
        $("#ndialog").dialog("option", "title", "Add " + type + " Notes");
        $("#txtNotes").focus();
        return true;
    }
    else {
        AddAJAXRequest();
        PageMethods.PM_ChangeStatus(gUserName, gFormId, sid, onStatusChangeSuccess, onFailed);
    }
}
function SaveNote(c) {
    var ns = $(c).attr('sid');
    var action = $(c).attr('type');
    var allowTruncate = true;
    var notesVal = $("#txtNotes").val();
    var notesVal1 = notesVal.replace(/\s/g, "");
    if (notesVal1.length == 0) {

        alert('Please enter the reason for ' + action + ' in notes field');

        return;
    }
    var val = notesVal.trim();
    if (val.length > 500) {
        allowTruncate = confirm('The notes section is limited to 500 characters.\n\nClick \'OK\' to save the first 500 characters.\nClick \'Cancel\' to cancel the save and trim the text manually.');
        if (allowTruncate) {
            val = notesVal.toString().substring(0, 500);

            notesVal = val;
        }
    }
    if (allowTruncate) {
        AddAJAXRequest();

        PageMethods.PM_ChangeStatusReject(gUserName, gFormId, ns, notesVal, onStatusChangeSuccess, onFailed);

    }
}



function DeleteForm() {
    if (confirm("Are you sure you want to delete this form?")) {
        AddAJAXRequest();
        PageMethods.PM_DeleteForm(gUserName, gFormId, onSuccessClose, onFailed);
    }
}

function RateTypeChanged(c) {
    var rtOption = GetOption(c)
    var rateTypeId = c.value
    if (Boolean.parse(rtOption.attr('hasCategories'))) {
        $('#paymentDialog').parent().appendTo($('form:first'));
        $("#mhcategory").show();
    } else {
        $("#mhcategory").hide();
    }
    if (Boolean.parse(rtOption.attr('requireNote'))) {
        var cRateTypeNotes = $("#lblAddRateTypeNotes");
        if (rateTypeId == 6) {
            cRateTypeNotes.html('Notes(PO#):');
        }
        else {
            cRateTypeNotes.html('Notes(Required):');
        }
    } else {
        var cRateTypeNotes = $("#lblAddRateTypeNotes");
        cRateTypeNotes.html('Notes:');
    }
    $('#cbHolidayPay').attr('checked', false);
    $('#txtAddRateQuantity').attr('ratetypeid', rateTypeId);
    if (rateTypeId != '') {
        if (rateTypeId == '2' || rateTypeId == '3' || rateTypeId == '4' || rateTypeId == '5' || rateTypeId == '7') {
            PageMethods.PM_GetFormPlanInfo(gFormId, rateTypeId, onSuccessInfo, onFailed, $(c));
        }
        else {
            var cQuantity = $get('txtAddRateQuantity');
            cQuantity.value = '';
            $('#btnCategoryRefresh').click();
        }
    }
    else {
        //PageMethods.PM_GetFormPlanInfo(gFormId, 0, onSuccessInfo, onFailed, $(c));
        var cQuantity = $get('txtAddRateQuantity');
        cQuantity.value = '';
        $('#btnCategoryRefresh').click();
    }
}

function GetOption(c) {
    return $(c).find(":selected");
}

function RollbackField(c) {
    if ($(c).attr('origVal') != null) {
        $(c).val($(c).attr('origVal'));
        return true;
    }
    return false;
}

function UpdateField(c) {
    if ($(c).attr('origVal') != null) {
        $(c).attr('origVal', $(c).val());
        return true;
    }
    return false;
}

function CopyStartDate(c) {
    var val = c.val();
    cTxtStartDate = $get('txtAddTLStartDate');
    cTxtStartDate.value = val;
}

function SetCursorPosition() {
    var txtEndDate = document.getElementById('txtAddTLEndDate');
    if (txtEndDate != null && txtEndDate.value.length < 6) {
        if (txtEndDate.createTextRange) {
            var FieldRange = txtEndDate.createTextRange();
            FieldRange.moveStart('character', 0);
            FieldRange.collapse();
            FieldRange.select();
        }
    }
}

function PreviewHoursMinutes() {
    var startDate = $('#txtAddTLStartDate').val();
    var ddStartDate = new Date(startDate);
    if (ddStartDate.getFullYear() < 1970) {
        ddStartDate.setFullYear(ddStartDate.getFullYear() + 100);
    }
    var endDate = $('#txtAddTLEndDate').val();
    var ddEndDate = new Date(endDate);
    if (ddEndDate.getFullYear() < 1970) {
        ddEndDate.setFullYear(ddEndDate.getFullYear() + 100);
    }
    var startHour = parseInt($('#ddAddStartHour').val());
    var startMin = parseInt($('#ddAddStartMin').val());
    var startAmPm = $('#ddAddStartAmPm').val();
    var endHour = parseInt($('#ddAddEndHour').val());
    var endMin = parseInt($('#ddAddEndMin').val());
    var endAmPm = $('#ddAddEndAmPm').val();
    var totalHoursMin = $('#lblTotalHoursMin').val();
    var lTotalHoursMin = $get('lblTotalHoursMin');
    var totalHours;
    var totalMinutes;
    if (startAmPm == 'PM' && startHour != 12) {
        var sHour = parseInt(startHour);
        startHour = sHour + 12;
    }
    if (startAmPm == 'AM' && startHour == 12) {
        startHour = 0;
    }
    if (endAmPm == 'PM' && endHour != 12) {
        var eHour = parseInt(endHour);
        endHour = eHour + 12;
    }
    if (endAmPm == 'AM' && endHour == 12) {
        endHour = 0;
    }
    if (ddStartDate.isValid()) {
        if (ddEndDate.isValid()) {
            if (!startDate.match(/[a-z]/i) && startDate.length <= 10) {
                if (!endDate.match(/[a-z]/i) && endDate.length <= 10) {
                    if (String(ddStartDate) == String(ddEndDate)) {
                        if (startHour == endHour) {
                            if (startMin < endMin) {
                                totalHours = 0;
                                totalMinutes = parseInt(endMin) - parseInt(startMin);
                                lTotalHoursMin.innerHTML = totalHours + ':' + totalMinutes;
                            }
                            else {
                                lTotalHoursMin.innerHTML = '';
                            }
                        }
                        else if (startHour < endHour) {
                            totalHours = parseInt(endHour) - parseInt(startHour);
                            if (startMin > endMin) {
                                totalHours = totalHours - 1;
                                totalMinutes = (60 - parseInt(startMin)) + parseInt(endMin);
                            }
                            else {
                                totalMinutes = parseInt(endMin) - parseInt(startMin);
                            }
                            lTotalHoursMin.innerHTML = totalHours + ':' + totalMinutes;
                        }
                        else {
                            lTotalHoursMin.innerHTML = '';
                        }
                    }
                    else if (ddStartDate < ddEndDate) {
                        totalHours = (ddEndDate - ddStartDate) / (3600 * 1000);
                        if (startHour > endHour) {
                            totalHours = totalHours - (parseInt(startHour) - parseInt(endHour));
                            if (startMin > endMin) {
                                totalHours = totalHours - 1;
                                totalMinutes = (60 - parseInt(startMin)) + parseInt(endMin);
                            }
                            else {
                                totalMinutes = parseInt(endMin) - parseInt(startMin);
                            }
                        }
                        else {
                            totalHours = totalHours + (parseInt(endHour) - parseInt(startHour));
                            if (startMin > endMin) {
                                totalHours = totalHours - 1;
                                totalMinutes = (60 - parseInt(startMin)) + parseInt(endMin);
                            }
                            else {
                                totalMinutes = parseInt(endMin) - parseInt(startMin);
                            }
                        }
                        lTotalHoursMin.innerHTML = totalHours + ':' + totalMinutes;
                    }
                    else {
                        lTotalHoursMin.innerHTML = '';
                    }
                }
                else {
                    lTotalHoursMin.innerHTML = '';
                }
            }
            else {
                lTotalHoursMin.innerHTML = '';
            }
        }
        else {
            lTotalHoursMin.innerHTML = '';
        }
    }
    else {
        lTotalHoursMin.innerHTML = '';
    }

}

function FocusAndCloseDialog(c) {
    var isupdate = $(c).attr('isupdate');
    var focuselement = $(c).attr('focuselement');
    var item = $(c).attr('item');
    var focusindex = item.substr(item.length - 2);
    var inputs = $(focuselement);
    $(c).dialog("close");
    if (isupdate != 1) {
        //alert(focuselement);
        //$(focuselement).focus();
        //$(focuselement).select();
        inputs[parseInt(focusindex)].focus();

    }
}

function FocusLastInputElement(c) {
    //            var filter = ':input:visible:not(:disabled):not([readonly]):(.' + c + ')';
    //            var inputs =  $(document).find(filter); //$(document).find(":input");
    //            inputs.eq(inputs.length - 1).focus();
    //            inputs.eq(inputs.length - 1).select();
    var inputs = $(c);
    inputs[inputs.length - 1].focus();
    inputs[inputs.length - 1].select();
}


$(document).ready(function () {
    var showIcon = "../../../../Images/Icons/show.png";
    var hideIcon = "../../../../Images/Icons/hide.png";
    //bindDatePickerAutoCompl();
    //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindDatePickerAutoCompl);
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_endRequest(bindDatePickerAutoCompl);

    $('input').on('focus', function () {
        $(this).addClass("inputfocus");
    });
    $('input').on('blur', function () {
        $(this).removeClass("inputfocus");
    });
    $('select').on('focus', function () {
        $(this).addClass("inputfocus");
    });
    $('select').on('blur', function () {
        $(this).removeClass("inputfocus");
    });
    $('a').on('focus', function () {
        $(this).addClass("linkfocus");
    });
    $('a').on('blur', function () {
        $(this).removeClass("linkfocus");
    });

    $(document).bind('keydown', 'alt+ctrl+p', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        $('#hlAddRateTypeDialog').click();
        return false;
    });
    $(document).bind('keydown', 'alt+ctrl+b', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        $('#imgAddBackhaul').click();
        return false;
    });
    $(document).bind('keydown', 'alt+ctrl+e', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        $('#txtAddEmployee').focus();
        $('#txtAddEmployee').val('');
        return false;
    });
    $(document).bind('keydown', 'alt+ctrl+v', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        $('#txtAddVehicle').focus();
        $('#txtAddVehicle').val('');
        return false;
    });
    $(document).bind('keydown', 'alt+ctrl+t', function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        $('#txtAddTrailer').focus();
        $('#txtAddTrailer').val('');
        return false;
    });
    $(document).bind('keydown', 'alt+ctrl+f', function () {
        var repeaterIndex = $(document.activeElement).parents().eq(2).find('#txtEndOdometer').attr('name');
        if (repeaterIndex != "" && !(typeof repeaterIndex === 'undefined') && document.activeElement.id != 'txtAddVehicle') {
            var inputs = $('body .vehicleFTFocus');
            inputs[parseInt(repeaterIndex)].click();
        }
        else {
            repeaterIndex = $(document.activeElement).parents().eq(2).find('#txtEndHours').attr('name');
            if (repeaterIndex != "" && !(typeof repeaterIndex === 'undefined') && document.activeElement.id != 'txtAddTrailer') {
                var inputs = $('body .trailerFTFocus');
                inputs[parseInt(repeaterIndex)].click();
            }
        }
    });

    $('#txtAddVehicle').focus(function () {
        $('#AddVehicleFocus').show();
    }).blur(function () {
        $('#AddVehicleFocus').hide();
    });

    $('#txtAddTrailer').focus(function () {
        $('#AddTrailerFocus').show();
    }).blur(function () {
        $('#AddTrailerFocus').hide();
    });

    $('#txtDriverName').focus(function () {
        $('#AddDriverFocus').show();
    }).blur(function () {
        $('#AddDriverFocus').hide();
    });

    $('#txtHelperName').focus(function () {
        $('#AddHelperFocus').show();
    }).blur(function () {
        $('#AddHelperFocus').hide();
    });


    $('input:checkbox').keypress(function (e) {
        if ((e.keyCode ? e.keyCode : e.which) == 13) {
            if ($(this).prop('checked') == false) {
                $(this).attr('checked', true).triggerHandler('click');
                setTimeout(function () {
                    $(this).focus();
                }, 500);
            }
            else {
                $(this).attr('checked', false).triggerHandler('click');
                setTimeout(function () {
                    $(this).focus();
                }, 500);
            }
            return false;
        }
    });


    //preload swappable images
    $('<img />')
        .attr('src', hideIcon)
        .load(function () {
            $('.profile').append($(this));
        });
    $('<img />')
        .attr('src', showIcon)
        .load(function () {
            $('.profile').append($(this));
        });





    $("#paymentDialog").dialog({
        height: 280
        , width: 450
        , autoOpen: false
        , title: "Add Payment to Employees"
        , closeOnEscape: true
        , modal: true
        , resizable: false
        , buttons: {
            "Ok": function () { AddPayment(this); }
            , "Cancel": function () { $("#mhcategory").hide(); $(this).dialog("close"); $('#hlAddRateTypeDialog').focus(); }
        }
    });

    $('#imgAddBackhaul').click(function (e) {
        $("#txtAddBackhaulPoNo").val("");
        $("#txtAddBackhaulRevenue").val("");
        $('#cbAddPayment').attr("checked", false);
        $("#addbackhaulDialog").dialog("open");
    });

    $("#addbackhaulDialog").dialog({
        height: 200
        , width: 220
        , autoOpen: false
        , title: "Add Backhaul"
        , closeOnEscape: true
        , modal: true
        , resizable: false
        , buttons: {
            "Ok": function () { AddBackhaul(this); }
            , "Cancel": function () { $(this).dialog("close"); }
        }
    });

    $("#ftdialog").dialog({
        height: 250
        , width: 300
        , autoOpen: false
        , closeOnEscape: true
        , modal: true
        , resizable: false
        , buttons: {
            "Ok": function () { AddUpdateFT(this); }
            , "Cancel": function () {
                FocusAndCloseDialog(this);
            }
            , "Add Existing": function () { OpenExisting(this); }
        }
        //, close: function () {
        //    FocusAndRefreshPage();
        //}

    });





    $("#tldialog").dialog({
        height: 250
        , width: 400
        , autoOpen: false
        , closeOnEscape: true
        , modal: true
        , resizable: false
        , buttons: {
            "Ok": function () { AddUpdateTL(this); }
            , "Cancel": function () { $(this).dialog("close"); }
        }
    });

    $("#ndialog").dialog({
        height: 210
        , width: 300
        , autoOpen: false
        , closeOnEscape: true
        , modal: true
        , resizable: false
        , buttons: {
            "Ok": function () { SaveNote(this); }
            , "Cancel": function () { $(this).dialog("close"); }
        }
    });
    $("#ftdeletedialog").dialog({
        height: 120
        , width: 350
        , autoOpen: false
        , title: "Remove Fuel Ticket"
        , modal: true
        , closeOnEscape: true
        , resizable: false
        , buttons: {
            "Remove": function () { RemoveFT(this, false); }
            , "Remove and Delete": function () { RemoveFT(this, true); }
            , "Cancel": function () { $(this).dialog("close"); }
        }
    });

    $("#tldeletedialog").dialog({
        height: 120
        , width: 400
        , autoOpen: false
        , title: "Remove Time Log"
        , modal: true
        , closeOnEscape: true
        , resizable: false
        , buttons: {
            "Remove": function () { RemoveTL(this); }
            , "Cancel": function () { $(this).dialog("close"); }
        }
    });

    $("#selFTdialog").dialog({
        height: 200
        , width: 700
        , title: "Select Existing Fuel Ticket"
        , autoOpen: false
        , closeOnEscape: true
        , modal: true
        , resizable: false
        , buttons: {
            "Ok": function () { SelectFT(this); }
            , "Cancel": function () { closeSelFTDialog(); }
        }
    });



    $("#dteDepartDate").datepicker({
        showButtonPanel: true
        , onSelect: function (dateText, inst) {
            pullRouteInfo();
        }

    });

    $("#dteWeekendingDate").datepicker({
        showButtonPanel: true,
				onSelect: function (dateText, inst) {
            SetWeekending(dateText);
        }

    });

    $("#txtAddEmployee").autocomplete({
        source: function (request, response) {
            var param = { prefixText: $('#txtAddEmployee').val(), formId: gFormId };
            $.ajax({
                url: "AddUpdate.aspx/GetEmployees",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.substring(0, item.indexOf("-")),
                            label: item.substring(item.indexOf("-") + 1)
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtAddEmployee").val(ui.item.label);
            $('#hfAddEmployee').val(ui.item.value);
            $('#btnAddEmployee').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
            //$("#txtAddEmployee").val(ui.item.label);
        },
        minLength: 2
    })
        .focus(function () {
            $('#AddEmployeeFocus').show();
        }).blur(function () {
            $("#txtAddEmployee").val("");
            $('#AddEmployeeFocus').hide();
        });

    $("#txtAddVehicle").autocomplete({
        source: function (request, response) {
            var param = { prefixText: $('#txtAddVehicle').val(), formId: gFormId };
            $.ajax({
                url: "AddUpdate.aspx/GetVehicles",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.split('-')[0],
                            label: item.split('-')[1]
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtAddVehicle").val(ui.item.label);
            $('#hfAddVehicle').val(ui.item.value);
            $('#btnAddVehicle').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
            //$("#txtAddVehicle").val(ui.item.label);
        },
        minLength: 2
    })
        .focus(function () {
            $('#AddVehicleFocus').show();
        }).blur(function () {
            $("#txtAddVehicle").val("");
            $('#AddVehicleFocus').hide();
        });

    $("#txtAddTrailer").autocomplete({
        source: function (request, response) {
            //var param = { prefixText: $('#txtAddTrailer').val(), formId: gFormId };
            var param = { prefixText: $('#txtAddTrailer').val(), formId: gFormId, centerNo: $("#ddlCenterList option:selected").val() };

            $.ajax({
                url: "AddUpdate.aspx/GetTrailers",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.split('-')[0],
                            label: item.split('-')[1]
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtAddTrailer").val(ui.item.label);
            $('#hfAddTrailer').val(ui.item.value);
            $('#btnAddTrailer').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
            //$("#txtAddTrailer").val(ui.item.label);
        },
        minLength: 2
    })
        .focus(function () {
            $('#AddTrailerFocus').show();
        }).blur(function () {
            $("#txtAddTrailer").val("");
            $('#AddTrailerFocus').hide();
        });

    //Chris
    $("#txtDriverName").autocomplete({
        source: function (request, response) {
            var param = { prefixText: $('#txtDriverName').val(), formId: gFormId };
            $.ajax({
                url: "AddUpdate.aspx/GetEmployees",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.split('-')[0],
                            label: item.split('-')[1]
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtAddEmployee").val(ui.item.label);
            $('#hfAddEmployee').val(ui.item.value);
            $('#btnAddEmployee').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
            //$("#txtAddEmployee").val(ui.item.label);
        },
        minLength: 2
    })
        .focus(function () {
            $('#AddEmployeeFocus').show();
        }).blur(function () {
            $("#txtAddEmployee").val("");
            $('#AddEmployeeFocus').hide();
        });
    //Chris

    $("#dteFormWeekendingDate").datepicker({
        showButtonPanel: true
        , onSelect: function (dateText, inst) {
            SetFormWeekending(dateText);
        }

    });

    $("#txtAddFTDate").datepicker({
        showButtonPanel: true
        , onSelect: function (dateText, inst) {
            $('#txtAddFTDate').datepicker("hide");
            $('#txtAddFTGallons').focus();
        }
    });

    $("#txtStartDate").datepicker({
        showButtonPanel: true
        , onSelect: function (dateText, inst) {
            cTxtStartDate = $get('txtAddTLStartDate');
            cTxtStartDate.value = dateText;
            PreviewHoursMinutes();
        }
    });

    $('#hlCalender').click(function (e) {
        $('#txtStartDate').datepicker("show");
        e.preventDefault();
    });

    $("#txtEndDate").datepicker({
        showButtonPanel: true
        , onSelect: function (dateText, inst) {
            cTxtEndDate = $get('txtAddTLEndDate');
            cTxtEndDate.value = dateText;
            PreviewHoursMinutes();
        }
    });

    $('#hlEndCalender').click(function (e) {
        $('#txtEndDate').datepicker("show");
        e.preventDefault();
    });

    $('.toggle')
        .click(function (e) {
            var img = $(this);
            if (img.attr("src") == hideIcon) {
                img.attr("src", showIcon);
                img.attr("alt", "show");
                img.attr("title", "Show Area");
                $(img.attr("entity")).hide("slide", { direction: "up" }, "fast");
            } else {
                img.attr("src", hideIcon);
                img.attr("alt", "hide");
                img.attr("title", "Hide Area");
                $(img.attr("entity")).show("slide", { direction: "up" }, "fast");
            }
        });

    $('input').on("keypress", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 13) {
            /* FOCUS ELEMENT */
            var inputs = $(this).closest('form').find(':input:visible:not(.imagebutton):not(:disabled):not([readonly])'); //$(document).find(":input");
            var idx = inputs.index(this);
            this.blur();
            var a = $(this).parents("div");
            if (a != null) {
                var b = $(a[1]).attr('id') + $(a[2]).attr('id') + $(a[3]).attr('id');
                if (typeof b == 'string') {
                    b = b.toUpperCase();
                    if (b.indexOf("DIALOG") > -1)// && b.indexOf("Dialog") > -1)
                    {
                        return false;
                    }
                }
            }

            if (this.type != 'submit' && this.type != 'checkbox' && this.type != 'undefined') {
                this.selectionStart = this.selectionEnd = -1;
            }
            else {
                if (this.type == 'submit') {
                    $(this).click();
                    return false;
                }
                return true;
            }
            if (idx == inputs.length - 1) {
                inputs.eq(0).focus();
                inputs.eq(0).select();
            } else {
                inputs.eq(idx + 1).focus(); //  handles submit buttons
                inputs.eq(idx + 1).select();
            }
            return false;

        }
    });
    $('select').on("keypress", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 13) {
            /* FOCUS ELEMENT */
            var inputs = $(this).closest('form').find(':input:visible:not(.imagebutton):not(:disabled):not([readonly])'); //$(document).find(":input");
            var idx = inputs.index(this);
            this.blur();
            if (this.type != 'submit') {
                this.selectionStart = this.selectionEnd = -1;
            }
            else {
                return true;
            }
            if (idx == inputs.length - 1) {
                inputs.eq(0).focus();
                inputs.eq(0).select();
            } else {
                inputs.eq(idx + 1).focus(); //  handles submit buttons
                inputs.eq(idx + 1).select();
            }
            return false;
        }
    });
    $('form').on("keypress", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 13 || e.which == 13) {
            //alert("before postback - " + document.activeElement.id);
            if (document.activeElement.id == "" || document.activeElement.id == "employees" || document.activeElement.id == "vehicles"
                || document.activeElement.id == "trailers" || document.activeElement.id == "backhauls" || document.activeElement.id == "routeNo") {
                var ae = document.activeElement;
                if (document.activeElement.type != "" && document.activeElement.type != "button" && ae != "[object HTMLBodyElement]") {
                    e.preventDefault();
                    return false;
                }
                return true;
            }
            return true;
        }
        return true;
    });


});
function InitializeRequest(sender, args) {
}
function bindDatePickerAutoCompl(sender, args) {
    $('input').on('focus', function () {
        $(this).addClass("inputfocus");
    });
    $('input').on('blur', function () {
        $(this).removeClass("inputfocus");
    });
    $('select').on('focus', function () {
        $(this).addClass("inputfocus");
    });
    $('select').on('blur', function () {
        $(this).removeClass("inputfocus");
    });
    $('a').on('focus', function () {
        $(this).addClass("linkfocus");
    });
    $('a').on('blur', function () {
        $(this).removeClass("linkfocus");
    });

    $('input').on("keypress", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 13) {
            /* FOCUS ELEMENT */
            var inputs = $(this).closest('form').find(':input:visible:not(.imagebutton):not(:disabled):not([readonly])'); //$(document).find(":input");
            var idx = inputs.index(this);
            this.blur;
            var a = $(this).parents("div");
            var b = null;
            if (a != null) {
                var b = $(a[1]).attr('id') + $(a[2]).attr('id') + $(a[3]).attr('id');
                if (typeof b == 'string') {
                    b = b.toUpperCase();
                    if (b.indexOf("DIALOG") > -1)// && b.indexOf("Dialog") > -1)
                    {
                        return false;
                    }
                }
            }
            if (this.type != 'submit' && this.type != 'checkbox' && this.type != 'undefined') {
                this.selectionStart = this.selectionEnd = -1;
            }
            else {
                if (this.type == 'submit') {
                    $(this).click();
                    return false;
                }
                return true;
            }
            if (idx == inputs.length - 1) {
                inputs.eq(0).focus();
                inputs.eq(0).select();
            } else {
                inputs.eq(idx + 1).focus(); //  handles submit buttons
                inputs.eq(idx + 1).select();
            }
            return false;
        }
    });
    $('select').on("keypress", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 13) {
            /* FOCUS ELEMENT */
            var inputs = $(this).closest('form').find(':input:visible:not(.imagebutton):not(:disabled):not([readonly])'); //$(document).find(":input");
            var idx = inputs.index(this);
            this.blur();
            if (this.type != 'submit') {
                this.selectionStart = this.selectionEnd = -1;
            }
            else {
                return true;
            }
            if (idx == inputs.length - 1) {
                inputs.eq(0).focus();
                inputs.eq(0).select();
            } else {
                inputs.eq(idx + 1).focus(); //  handles submit buttons
                inputs.eq(idx + 1).select();
            }
            return false;
        }
    });
    $("#dteDepartDate").datepicker({
        showButtonPanel: true
        , onSelect: function (dateText, inst) {
            pullRouteInfo();
        }

    });
    

    $("#txtAddEmployee").autocomplete({
        source: function (request, response) {
            var param = { prefixText: $('#txtAddEmployee').val(), formId: gFormId };
            $.ajax({
                url: "AddUpdate.aspx/GetEmployees",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.split('-')[0],
                            label: item.split('-')[1]
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtAddEmployee").val(ui.item.label);
            $('#hfAddEmployee').val(ui.item.value);
            $('#btnAddEmployee').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
            // $("#txtAddEmployee").val(ui.item.label);
        },
        minLength: 2
    })
        .focus(function () {
            $('#AddEmployeeFocus').show();
        }).blur(function () {
            $("#txtAddEmployee").val("");
            $('#AddEmployeeFocus').hide();
        });

    $("#txtAddVehicle").autocomplete({
        source: function (request, response) {

            var param = { prefixText: $('#txtAddVehicle').val(), formId: gFormId };
            $.ajax({
                url: "AddUpdate.aspx/GetVehicles",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.split('-')[0],
                            label: item.split('-')[1]
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtAddVehicle").val(ui.item.label);
            $('#hfAddVehicle').val(ui.item.value);
            $('#btnAddVehicle').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
            //$("#txtAddVehicle").val(ui.item.label);
        },
        minLength: 2
    })
        .focus(function () {
            $('#AddVehicleFocus').show();
        }).blur(function () {
            $("#txtAddVehicle").val("");
            $('#AddVehicleFocus').hide();
        });

    $("#txtAddTrailer").autocomplete({
        source: function (request, response) {
            var param = { prefixText: $('#txtAddTrailer').val(), formId: gFormId, centerNo: $("#ddlCenterList option:selected").val() };
            $.ajax({
                url: "AddUpdate.aspx/GetTrailers",
                data: JSON.stringify(param),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            value: item.split('-')[0],
                            label: item.split('-')[1]
                        }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        autoFocus: true,
        select: function (event, ui) {
            event.preventDefault();
            $("#txtAddTrailer").val(ui.item.label);
            $('#hfAddTrailer').val(ui.item.value);
            $('#btnAddTrailer').click();
        },
        focus: function (event, ui) {
            event.preventDefault();
            //$("#txtAddTrailer").val(ui.item.label);
        },
        minLength: 2
    })
        .focus(function () {
            $('#AddTrailerFocus').show();
        }).blur(function () {
            $('#txtAddTrailer').val("");
            $('#AddTrailerFocus').hide();
        });
    $('form').on("keypress", function (e) {
        /* ENTER PRESSED*/
        if (e.keyCode == 13 || e.which == 13) {
            //alert(document.activeElement.id);
            if (document.activeElement.id == "" || document.activeElement.id == "employees" || document.activeElement.id == "vehicles"
                || document.activeElement.id == "trailers" || document.activeElement.id == "backhauls" || document.activeElement.id == "routeNo") {
                var ae = document.activeElement;
                if (document.activeElement.type != "" && document.activeElement.type != "button" && ae != "[object HTMLBodyElement]") {
                    e.preventDefault();
                    return false;
                }
                return true;
            }
            return true;
        }
        return true;
    });
}
function UpdateDriverDetails(c, detailId, formId) {
    var val = parseInt(c.value);
    AddAJAXRequest();
    var a = PageMethods.PM_SaveDriverDetails(gUserName, formId, detailId, val, onChangeSuccess, onFailed);
}
function onChangeSuccess(ret) {
    RemoveAJAXRequest();
    //alert(ret);
    //alert(ret[1]);
    if (ret == "1") {
        alert("Route has Started, Driver details cannot be updated");
    }
    else {
        //alert("Not Started");
    }
}
function onFailed(error) {
    RemoveAJAXRequest();
    alert('There was an error saving the field.  Please refresh the page and try again.');
}

//        function UpdateRouteCategory (c, formId) {
//            var val = parseInt(c.value);
//            AddAJAXRequest();
//            var a = PageMethods.PM_SaveRouteCategory(gUserName, formId, val, onRCSuccess, onFailed);           
//        }
//        function onChangeSuccess(ret) {
//            RemoveAJAXRequest();            
//        } 
function OpenDialog() {
    alert("TEST");
}

function OpenExisting(c) {
    var eid = $(c).attr('eid');
    var feid = $(c).attr('feid');
    var ftType = $(c).attr('ftType');

    //alert('EID : ' + eid);
    //alert('FEID : ' + feid);
    //alert('ftType : ' + ftType);

    $("#selFTdialog").attr("VTid", eid);
    $("#selFTdialog").attr("FormVTid", feid);
    $("#selFTdialog").attr("Type", ftType);
    $("#selFTdialog").attr("Add", 'T');

    $('#hfVTId').val(eid);
    $('#hfType').val(ftType);
    $('#hfFVTId').val(feid);
    $('#hfCallFrom').val('Button');

    $('#selFTdialog').parent().appendTo($('form:first'));
    $('#btnLoadExisting').click();

    //$("#selFTdialog").dialog("open");
    //$("#lbSelectFuelTickets").focus();
    //$("#lbSelectFuelTickets").select();
    //openSelFTDialog();

}

function OpenExistingAdd(eid, ftType, feid) {

    $('#hfVTId').val(eid);
    $('#hfType').val(ftType);
    $('#hfFVTId').val(feid);
    $('#hfCallFrom').val('Open');

    $("#selFTdialog").attr("VTid", eid);
    $("#selFTdialog").attr("FormVTid", feid);
    $("#selFTdialog").attr("Type", ftType);
    //$("#selFTdialog").attr("Add", 'T');

    $('#selFTdialog').parent().appendTo($('form:first'));
    $('#btnLoadExisting').click();

    //$("#selFTdialog").dialog("open");
    //$("#lbSelectFuelTickets").focus();
    //$("#lbSelectFuelTickets").select();
    //openSelFTDialog();
}

function closeSelFTDialog() {
    $("#txtAddFTNo").focus();
    $("#txtAddFTNo").select();
    $("#selFTdialog").dialog("close");
}

function disableAndCloseSelFTDialog() {
    $(".ui-dialog-buttonpane button:contains('Add Existing')")
        .button(this.checked ? "enable" : "disable");
    closeSelFTDialog();
}

function openSelFTDialog() {
    var add = $('#selFTdialog').attr('Add');
    if ("undefined" === typeof add || add == 'T') {
        $("#selFTdialog").dialog("open");
        $("#lbSelectFuelTickets").focus();
        $("#lbSelectFuelTickets").select();
    }
}

//$('#selFTdialog').on("keypress", function (e) {
//    /* ENTER PRESSED*/
//    //if (e.keyCode === $.ui.keyCode.ESCAPE) {
//        alert('escape');
//   // }
//});

function SelectFT(c) {

    var VTid = $(c).attr('VTid');
    var FormVTid = $(c).attr('FormVTid');
    var Type = $(c).attr('Type');
    var selectedFT = $("#lbSelectFuelTickets").val();

    if (selectedFT == null) {
        selectedFT = new Array();
    }
    AddAJAXRequest();
    //alert(VTid + " " + FormVTid + " " + Type + " " + selectedFT);
    PageMethods.PM_MapFuelTickets(gUserName, FormVTid, Type, selectedFT, onSuccessFT, onFailed);
    $(c).dialog('close');
    $("#txtAddFTNo").focus();
    $("#txtAddFTNo").select();
}


function onSuccessFT(rv) {
    if (rv[0] == 1) {
        alert('Fuel Tickets added to the ' + rv[1]);
        if (rv[1] == 'Vehicle') {
            $('#selFTdialog').parent().appendTo($('form:first'));
            $('#btnRefreshVFT').click();
        }
        else {
            $('#selFTdialog').parent().appendTo($('form:first'));
            $('#btnRefreshTFT').click();
        }
    }
    else {
        alert('No Fuel Tickets added ' + rv[1]);
    }
    RemoveAJAXRequest();
}

function FocusAndRefreshPage() {
    //alert('Here');
    //AddAjaxRequest();
    //$('#selFTdialog').parent().appendTo($('form:first'));
    //$('#btnRefreshVFT').click();
    //RemoveAJAXRequest();
}