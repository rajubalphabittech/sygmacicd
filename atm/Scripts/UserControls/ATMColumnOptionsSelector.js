function LoadColumnOptionsUserControl(pageName) {
    $("#lstBoxAvailableColumns option").remove();
    $("#lstBoxSelectedColumns option").remove();
    $("#columnOptionsDialogOk").attr("disabled", true);

    $.ajax({
        type: "POST",
        url: "/Apps/ATM/WebServices/ATMColumnOptionsSelector.asmx/GetColumnOptionsData",
        data: JSON.stringify({ userName: gUserName, pageName: pageName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            if (response.d != null && response.d.length > 0) {
                var obj = jQuery.parseJSON(response.d);
                if (obj.length > 1) {
                    var lstBoxAvailableColumns = $("[id*=lstBoxAvailableColumns]");
                    lstBoxAvailableColumns.empty();
                    $.each(obj[1]["AvailableColumns"], function (key, value) {
                        $("<option>", { value: value.ID, html: value.ColumnIdentifier }).appendTo(lstBoxAvailableColumns);
                    });

                    var lstBoxSelectedColumns = $("[id*=lstBoxSelectedColumns]");
                    lstBoxSelectedColumns.empty();
                    $.each(obj[0]["Columns"], function (key, value) {
                        $("<option>", { value: value.ID, html: value.ColumnIdentifier }).appendTo(lstBoxSelectedColumns);
                    });
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Error loading user column preferences");
        }
    });
}

function btnSelectAvailableColumnOnClick() {
    $("#lstBoxAvailableColumns > option:selected").each(function () {
        $(this).remove().appendTo("#lstBoxSelectedColumns");
    });
    if ($("#lstBoxSelectedColumns > option").length > 0) {
        $("#columnOptionsDialogOk").attr("disabled", false);
    }
    $("#lstBoxSelectedColumns option").attr("selected", false);
}

function lstBoxAvailableColumnsOnDblClick() {
    $("#lstBoxAvailableColumns > option:selected").each(function () {
        $(this).remove().appendTo("#lstBoxSelectedColumns");
    });
    if ($("#lstBoxSelectedColumns > option").length > 0) {
        $("#columnOptionsDialogOk").attr("disabled", false);
    }
    $("#lstBoxSelectedColumns option").attr("selected", false);
}

function btnSelectAllAvailableColumnOnClick() {
    $("#lstBoxAvailableColumns > option").each(function () {
        $(this).remove().appendTo("#lstBoxSelectedColumns");
    });
    if ($("#lstBoxSelectedColumns > option").length > 0) {
        $("#columnOptionsDialogOk").attr("disabled", false);
    }
    $("#lstBoxSelectedColumns option").attr("selected", false);
}

function btnUnSelectSelectedColumnOnClick() {
    if ($("#lstBoxSelectedColumns > option").length == $("#lstBoxSelectedColumns > option:selected").length) {
        alert("You need to have atleast one column selected.");
    }
    else {
        if ($("#lstBoxSelectedColumns > option").length > 1) {
            $("#lstBoxSelectedColumns > option:selected").each(function () {
                $(this).remove().appendTo("#lstBoxAvailableColumns");
                $("#columnOptionsDialogOk").attr("disabled", false);
            });
        }
    }
    $("#lstBoxAvailableColumns option").attr("selected", false);
    $("#lstBoxSelectedColumns option").attr("selected", false);
}

function lstBoxSelectedColumnsOnDblClick() {
    if ($("#lstBoxSelectedColumns > option").length == $("#lstBoxSelectedColumns > option:selected").length) {
        alert("You need to have atleast one column selected.");
    }
    else {
        if ($("#lstBoxSelectedColumns > option").length > 1) {
            $("#lstBoxSelectedColumns > option:selected").each(function () {
                $(this).remove().appendTo("#lstBoxAvailableColumns");
                $("#columnOptionsDialogOk").attr("disabled", false);
            });
        }
    }
    $("#lstBoxAvailableColumns option").attr("selected", false);
    $("#lstBoxSelectedColumns option").attr("selected", false);
}

function btnSelectedColumnMoveDownOnClick() {
    var selectedOptions = $('#lstBoxSelectedColumns > option:selected');
    var nextOption = $('#lstBoxSelectedColumns > option:selected').next("option").last();

    if ($(nextOption).text() != "") {
        $(selectedOptions).remove();
        $(nextOption).after($(selectedOptions));
        $("#columnOptionsDialogOk").attr("disabled", false);
    }
}

function btnSelectedColumnMoveUpOnClick() {
    var selectedOptions = $('#lstBoxSelectedColumns > option:selected');
    var prevOption = $('#lstBoxSelectedColumns > option:selected').prev("option").first();

    if ($(prevOption).text() != "") {
        $(selectedOptions).remove();
        $(prevOption).before($(selectedOptions));
        $("#columnOptionsDialogOk").attr("disabled", false);
    }
}

function onlyNumbers(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function SaveColumnOptionsUserControl(pageName) {
    var successfulSave = false;
    var jsonString = '{"Columns":[';
    $('#lstBoxSelectedColumns > option').each(function (key, value) {
        var displayOrder = parseInt(key) + parseInt(1);
        var dataWidth = 0;
        var columnIdentifier = value.text;
        jsonString = jsonString + '{ "ID":"' + value.value + '", "Width":"' + dataWidth + '", "ColumnIdentifier":"' + columnIdentifier + '", "Visible":"true", "DisplayOrder":"' + displayOrder + '" },';
    });
    if ($('#lstBoxSelectedColumns > option').length > 0) {
        jsonString = jsonString.slice(0, -1);
    }
    jsonString = jsonString + ']}';

    $.ajax({
        type: "POST",
        url: "/Apps/ATM/WebServices/ATMColumnOptionsSelector.asmx/SaveColumnOptionsData",
        data: JSON.stringify({ userName: gUserName, columnsData: jsonString, pageName: pageName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            successfulSave = true;                
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            successfulSave = false;                
        }
    });

    return successfulSave;
}