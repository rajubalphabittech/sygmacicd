// JScript File
function ChangeChecksCheckBox(cb, containerName, checkName) {
    var type = (cb.checked) ? 'all' : 'none';
    ChangeChecks(type, containerName, checkName);
}
function ChangeChecks(type, containerId, checkName, fireOnClick) {
    var isChecked = false;
    switch (type) {
        case 'all':
            isChecked = true;
        case 'none':
            var formElements = document.getElementsByTagName("input");
            var len = formElements.length;
            if (checkName != '')
                checkName = '_' + checkName;
            var j = checkName.length;
            for (var i = 0; i < len; i++) {
                var formElement = formElements[i];
                if (IsValidCheckBox(formElement, containerId, checkName, j, true)) {
                    //using this method so that the checkbox's onClick event is fired.
                    if (fireOnClick) {
                        if ((!isChecked && formElement.checked) || (isChecked && !formElement.checked))
                            formElement.click();
                    } else {
                        formElement.checked = isChecked;
                    }
                }
            }
            break;
    }
}

function ChangeCheckAllBox(clickedElement, allBoxId) {
    var isChecked = false;
    if (clickedElement.type != 'checkbox' || clickedElement.checked) {
        var containerId = clickedElement.id.substring(0, clickedElement.id.lastIndexOf('_ctl'));
        var checkName = clickedElement.id.substr(clickedElement.id.lastIndexOf('_') + 1);
        var formElements = document.getElementsByTagName("input");
        var len = formElements.length;
        if (checkName != '')
            checkName = '_' + checkName;
        var j = checkName.length;
        for (var i = 0; i < len; i++) {
            var formElement = formElements[i];
            if (IsValidCheckBox(formElement, containerId, checkName, j)) {
                isChecked = formElement.checked
                if (!isChecked)
                    break;
            }
        }
    }
    
    document.getElementById(allBoxId).checked = isChecked;
}
function IsValidCheckBox(formElement, containerId, checkName, checkNameLength, includeCheckAll) {
    if (includeCheckAll == null)
        includeCheckAll = false;
    var retVal = false;
    var formElementId = formElement.id;
    if (formElement.type == 'checkbox') {
        if (formElementId.indexOf(containerId) != -1) {
            var m = (formElementId.length - checkNameLength);
            if ((formElementId.indexOf(checkName) == m) || checkName == '' || (includeCheckAll && IsCheckAllBox(formElementId))) {
                retVal = true;
            }
        }
    }
    return retVal;
}
function IsCheckAllBox(formElementId) {
    return (formElementId.indexOf('chkCheckAll') == (formElementId.length - 11));
}
