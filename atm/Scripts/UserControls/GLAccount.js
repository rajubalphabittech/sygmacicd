var invalidGLMsg = '*** Invalid Account Number ***';

function OpenLookup(nq, fai) {
    var centerName = GN('ddGLCenter', nq);
    var deptName = GN('ddGLDepartment', nq);
    var center = GetCenter(nq);
    var ddGLDepartment = $get(deptName);
    var dept = ddGLDepartment.options[ddGLDepartment.selectedIndex].value;
    if (center == '' || dept == '') {
        alert('Please select a Center and Department to filter on!');
    } else {
        var url = 'AccountLookup.aspx?c=' + center + '&d=' + dept + '&q=' + nq;
        if (fai != null) url = url + '&fai=' + fai;
        var newWin = OpenWindow(url, 520, 450, 1, 1, 1, 1, 'AccountLookup');
        if (newWin != null)
            newWin.focus();
    }
}

function SetGLDescKeyPress(c, nq, pid, w, e) {
    if (e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 38 && e.keyCode != 40 && e.keyCode != 9 && e.keyCode != 16 && e.keyCode != 18 && e.keyCode != 17 && e.keyCode != 20 && e.keyCode != 92 && e.keyCode != 93) {
        if (c.value.trim().length == w) {
            SetGLDescription(nq, pid);
        }
    }
}

function SetGLDescAfterExit(c, nq, pid, w) {
    if (c.value.trim().length < w) {
        SetGLDescription(nq, pid);
    }
}

function SetGLDescription(nq, pid) {
    SetGLAccountDescription('', nq);
    var centerName = GN('ddGLCenter', nq);
    var deptName = GN('ddGLDepartment', nq);
    var acctName = GN('txtGLAccount', nq);
    var subAcctName = GN('txtGLSubAccount', nq);
    if ($get(deptName) != null) {
        var center = GetCenter(nq);
        var dept = $get(deptName).options[$get(deptName).selectedIndex].value;
        var accountNo = $get(acctName).value.trim();
        var subAccountNo = $get(subAcctName).value.trim();
        if (subAccountNo != '') {
            if (!isNaN(subAccountNo)) {
                if (subAccountNo.indexOf('.') > -1) {
                    alert('Invalid GL account # entered.  Please verify your account #!');
                    return false;
                } else {
                    if (subAccountNo.length < 4) {
                        subAccountNo = subAccountNo.padLeft(4, '0');
                        $get(subAcctName).value = subAccountNo;
                    }
                }
            } else {
                $get(subAcctName).value = '';
                subAccountNo = '';
            }
        }

        if (!isNaN(accountNo)) {
            if (accountNo.indexOf('.') > -1) {
                alert('Invalid GL account # entered.  Please verify your account #!');
                return false;
            }
        } else {
            $get(acctName).value = '';
            accountNo = '';
        }

        if (center != '' && dept != '' && accountNo != '' && subAccountNo != '') {
            PageMethods.PM_GetGLDescription(center, dept, accountNo, subAccountNo, gUserName, pid, OnSuccess, OnFailure, nq);
        } else {
            if (pid != 0) {
                PageMethods.PM_SaveGLAccount(center, dept, accountNo, subAccountNo, pid, gUserName, OnSuccess, OnFailure, nq);
            }
        }
    }
}

function GN(field, qualifier) {
    if (qualifier != '')
        return qualifier + '_' + field;
    return field;
}

function GetCenter(nq) {
    var ddGLCenter = $get(GN('ddGLCenter', nq));
    if (ddGLCenter != null) {
        return ddGLCenter.options[ddGLCenter.selectedIndex].value;
    } else {
        return $get(GN('lblGLCenter', nq)).innerHTML;
    }
}

function OnSuccess(retVal, context) {
    if (retVal == null)
        retVal = invalidGLMsg;
    SetGLAccountDescription(retVal, context);
}

function SetGLAccountDescription(desc, nq) {
    var glDescName = GN('lblGLDescription', nq);
    if (desc == invalidGLMsg || desc == '') {
        $get(glDescName).style.color = '#ff0000';
    } else {
        $get(glDescName).style.color = '#000000';
    }
    if (navigator.userAgent.indexOf('Firefox') > -1) {
        $get(glDescName).textContent = desc;
    } else {
        $get(glDescName).innerText = desc;
    }
}

function OnFailure(error) {
    alert('Error looking up account description.\nPlease check your account #.');
}

function HasValidGL(fid) {
    var c = $get(fid + '_lblGLDescription')
    if (c) {
        var val = c.innerHTML;
        if (val != '')
            return (val != invalidGLMsg);
    }
    return false;
}