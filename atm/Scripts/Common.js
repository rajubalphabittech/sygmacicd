// JScript File

function OpenWindow(page, width, height, resize, toolbar, scrollbars, menus, name, newWindow) {
    if (name == null)
        name = '_blank';
    if (page.charAt(0) == '/') {
        page = page.substring(1);
        page = AppPath + page;
    }
    if (navigator.appName != 'Microsoft Internet Explorer'
        && (page.indexOf('.doc') != -1 || page.indexOf('.xls') != -1 || page.indexOf('.pps') != -1)) {
        window.location.href = page;
    } else {
        if (resize == null) resize = 1;
        if (toolbar == null) toolbar = 1;
        if (scrollbars == null) scrollbars = resize;
        if (menus == null) menus = toolbar;
        if (width == -1 || width == null) width = screen.width - 100;  //-1 means max size
        if (height == -1 || height == null) height = screen.height - 200;  //-1 means max size

        var menuHeight = 0;
        var toolbarHeight = 0;
        var statusBar = 10;
        if (menus == 1) menuHeight = 30;
        if (toolbar == 1) toolbarHeight = 40;
        var left = (screen.width - width) / 2;
        var top = (((screen.height - height) / 2) - toolbarHeight) - menuHeight - statusBar;

        var parms = 'status=1' +
                ',width=' + width +
				',height=' + height +
				',top=' + top +
				',left=' + left +
				',toolbar=' + toolbar +
				',menubar=' + menus +
				',scrollbars=' + scrollbars +
				',resizable=' + resize +
				',location=' + toolbar;

        newWindow = window.open(page, name, parms);
        if (newWindow)
            newWindow.focus();
    }
}

function openDatePickerWindow(fieldName, setAutoPostBack) {
    var curVal = document.getElementById(fieldName).value;
    var URL = "/UserControls/PopUpCalendar.aspx?curdate=" + curVal + "&control=" + fieldName + "&soc=" + setAutoPostBack;
    var theHeight = 212;     //192;
    var theWidth = 252;
    var theTop = (screen.height / 2) - (theHeight / 2)
    var theLeft = (screen.width / 2) - (theWidth / 2)
    OpenWindow(URL, theWidth, theHeight, 0, 0, 0, 0);
    return;
}
function AutoTab(input, len, e) {
    var keyCode = e.keyCode;
    var filter = [0, 8, 9, 16, 17, 18, 37, 38, 39, 40, 46];
    if (input.value.length >= len && !containsElement(filter, keyCode)) {
        input.value = input.value.slice(0, len);
        input.form[(getIndex(input) + 1) % input.form.length].focus();
        input.form[(getIndex(input) + 1) % input.form.length].select();
    }
    function containsElement(arr, ele) {
        var found = false, index = 0;
        while (!found && index < arr.length)
            if (arr[index] == ele)
                found = true;
            else
                index++;
        return found;
    }
    function getIndex(input) {
        var index = -1, i = 0, found = false;
        while (i < input.form.length && index == -1)
            if (input.form[i] == input) index = i;
            else i++;
        return index;
    }
    return true;
}
function GetXmlHttpObject() {
    try {    // Firefox, Opera 8.0+, Safari
        return new XMLHttpRequest();
    } catch (e) {    // Internet Explorer    
        try {
            return new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                return new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) {
                alert("Your browser does not support AJAX!");
                return null;
            }
        }
    }
}
function Trim(str) {
    return this.replace(/^\s+|\s+$/g, "");
}
function LTrim(str) {
    return this.replace(/^\s+/, "");
}
function RTrim(str) {
    return this.replace(/\s+$/, "");
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}
String.prototype.lTrim = function () {
    return this.replace(/^\s+/, "");
}
String.prototype.rTrim = function () {
    return this.replace(/\s+$/, "");
}


String.prototype.padLeft = function (length, padChar) {
    t = '';
    if (length > this.length) {
        for (i = 0; i < length - this.length; i++) {
            t += padChar;
        }
    }
    return t + this;
}
String.prototype.padRight = function (length, padChar) {
    t = this;
    if (length > this.length) {
        for (i = 0; i < length - this.length; i++) {
            t += padChar;
        }
    }
    return t;
}
String.prototype.isNumeric = function () {
    return (this == parseFloat(this));
}

//Date.prototype.toShortDateString = function (separator) {
//    if (separator == null)
//        separator = '/';
//    return (parseInt(this.getMonth() + 1).toString() + separator + parseInt(this.getDate()).toString() + separator + this.getFullYear().toString());
//}

Date.prototype.dayDiff = function (compareDate) {
    var date1 = Date.UTC(this.getFullYear(), this.getMonth(), this.getDate());
    var date2 = Date.UTC(compareDate.getFullYear(), compareDate.getMonth(), compareDate.getDate());
    return Math.floor((date2 - date1) / (1000 * 60 * 60 * 24));
}
Date.prototype.isValid = function () {
    return !isNaN(this.getTime());
}

function CleanDate(strDate) {
    date = strDate.replace(new RegExp('-', 'gm'), '/').replace(/\./g, '/');
    if (date.indexOf('/') > -1 && (date.indexOf('/') == date.lastIndexOf('/'))) {
        date += '/' + new Date().getFullYear().toString();
    }
    return (
        date.replace(new RegExp("/(\\d{2})$", ""), function ($0, $1) {
            if ($1.match(new RegExp("^[01234]{1}", ""))) {
                return ("/20" + $1);
            } else {
                return ("/19" + $1);
            }
        })
    );
}

function FormatAsNumber(n, sep) {
    var sRegExp = new RegExp('(-?[0-9]+)([0-9]{3})'),
	     sValue = n + '',
         ss = null;

    if (sValue.indexOf('.') > -1) {
        ss = sValue.toString().split('.');
        sValue = ss[0];
    }

    if (sep === undefined) { sep = ','; }
    while (sRegExp.test(sValue)) {
        sValue = sValue.replace(sRegExp, '$1' + sep + '$2');
    }
    if (ss == null)
        return sValue;
    else
        return sValue + '.' + ss[1];
}

function GrayOut(isOn, options) {
    // Pass true to gray out screen, false to ungray
    // options are optional.  This is a JSON object with the following (optional) properties
    // opacity:0-100         // Lower number = less grayout higher = more of a blackout 
    // zindex: #             // HTML elements with a higher zindex appear on top of the gray out
    // bgcolor: (#xxxxxx)    // Standard RGB Hex color code
    // grayOut(true, {'zindex':'50', 'bgcolor':'#0000FF', 'opacity':'70'});
    // Because options is JSON opacity/zindex/bgcolor are all optional and can appear
    // in any order.  Pass only the properties you need to set.
    var dark = document.getElementById('darkenScreenObject');
    if (isOn) {
        if (!dark) {
            // The dark layer doesn't exist, it's never been created.  So we'll
            // create it here and apply some basic styles.
            // If you are getting errors in IE see: http://support.microsoft.com/default.aspx/kb/927917
            var tbody = document.getElementsByTagName("body")[0];
            var tnode = document.createElement('div');           // Create the layer.
            tnode.style.position = 'fixed';                 // Position absolutely
            tnode.style.top = '0px';                           // In the top
            tnode.style.left = '0px';                          // Left corner of the page
            tnode.style.overflow = 'hidden';                   // Try to avoid making scroll bars            
            tnode.style.display = 'none';                      // Start out Hidden
            tnode.id = 'darkenScreenObject';                   // Name it so we can find it later
            tbody.appendChild(tnode);                            // Add it to the web page
            dark = document.getElementById('darkenScreenObject');  // Get the object.
        }

        var options = options || {};
        var zindex = options.zindex || 50;
        var opacity = options.opacity || 70;
        var opaque = (opacity / 100);
        var bgcolor = options.bgcolor || '#000000';
        // Calculate the page width and height 
        if (document.body && (document.body.scrollWidth || document.body.scrollHeight)) {
            var pageWidth = document.body.scrollWidth + 'px';
            var pageHeight = document.body.scrollHeight + 'px';
        } else if (document.body.offsetWidth) {
            var pageWidth = document.body.offsetWidth + 'px';
            var pageHeight = document.body.offsetHeight + 'px';
        } else {
            var pageWidth = '100%';
            var pageHeight = '100%';
        }
        //set the shader to cover the entire page and make it visible.
        dark.style.opacity = opaque;
        dark.style.MozOpacity = opaque;
        dark.style.filter = 'alpha(opacity=' + opacity + ')';
        dark.style.zIndex = zindex;
        dark.style.backgroundColor = bgcolor;
        dark.style.width = '100%'; //  pageWidth;
        dark.style.height = '100%'; //  pageHeight;
        dark.style.display = 'block';
    } else {
        if (dark)  //if exists hide it.  otherwise do nothing
            dark.style.display = 'none';
    }
}

function SetMaxHeight(cname, maxHeight) {
    if ($get(cname)) {
        if ($get(cname).offsetHeight > maxHeight) {
            $get(cname).style.height = maxHeight + 'px';
            $get(cname).style.overflow = 'scroll';
        } else {
            $get(cname).style.height = 'auto';
            $get(cname).style.overflow = 'auto';
        }
    }
}
function ChangeUser(un) {
    var user = prompt('User: ' + un + '\nChange?  Leave blank to use default user.', '');
    if (user != null) {
        if (user == '') {
            user = 'clear';
        }
        var url = self.location.href;
        var sep = (url.indexOf('?') > -1) ? '&' : '?';
        self.location.href = url + sep + 'user=' + user;
    }
}
function ClearDropDownItems(cn, dl) {
    var c = $get(cn)
    c.options.length = 0;
    if (dl) {
        var loading = new Option('Loading...', '')
        loading.disabled = true;
        c.add(loading);
    }
}
function GetRequiredPrompt(q, reqMsg, dt) {
    if (dt == null)
        dt = '';
    var text = prompt(q, dt);
    if (text == '') {
        alert(reqMsg);
        return GetRequiredPrompt(q, reqMsg, dt);
    } else {
        return text;
    }
}
function RefreshOpener(buttonId) {
    if (opener && !opener.closed) {
        var btn = window.opener.document.getElementById((buttonId == null) ? 'btnRefresh' : buttonId);
        if (btn != null) {
            btn.click();
            //opener.blur();
        }
    }
}
function CacheValue(c) {
    c.cachedValue = c.value;
}
function IfChanged(c, cs) {
    if (c.cachedValue != undefined) {
        if (c.value != c.cachedValue) {
            eval(cs);
            //SaveField(ss, c);
        }
    }
}

var getFirstByIdAndClass = function(id, className)
{
	return $('.' + className + '[data-id="' + id + '"]')[0];
}

function GetQualifier(c,tu) {
    if (typeof (c) != 'string')
        c = c.id;
    var i = c.lastIndexOf('_', c);
    if (!tu)
        i++;
    return c.substring(0, i);
}

function GetSelectedValue(id) {
    var dd = document.getElementById(id);
    return dd.options[dd.selectedIndex].value;
}

function Round(value, increment) {
    // value will be the number we'll round
    // increment will be the 'round to' - in your case, .25
    var remain;
    var roundvalue;
    var result;
    remain = value % increment; // this will be somewhere between 0 and .25
    roundvalue = increment / 2;

    if (remain >= roundvalue) { // rounding up
        result = value - remain;
        result += increment;
    } else { // rounding down
        result = value - remain;
    }
    return result;
}