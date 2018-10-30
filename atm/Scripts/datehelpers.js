
var formatDate = function (date, formatType) {
	var d = new Date(date),
		month = '' + (d.getMonth() + 1), day = '' + d.getDate(), year = '' + d.getFullYear(),
		hour = '' + d.getHours(), minute = '' + d.getMinutes();
	var h = '', t = '';

	if (month.length < 2) month = '0' + month;
	if (day.length < 2) day = '0' + day;
	if (hour.length < 2) hour = hour === '0' ? '12' : '0' + hour;
	if (minute.length < 2) minute = '0' + minute;

	if (formatType === undefined) formatType = "yyyy-MM-dd";

	if (formatType === "yyyy-MM-dd")
		return [year, month, day].join('-');
	else if (formatType === "MM/dd/yyyy")
		return [month, day, year].join('/');
	else if (formatType === "MM/dd/yyyy HH:mm")
		return [month, day, year].join('/') + ' ' + hour + ':' + minute;
	else if (formatType === "MM/dd/yyyy hh:mm") {
		h = d.getHours() === 0 ? '12' : '' + (d.getHours() > 12 ? d.getHours() - 12 : d.getHours());
		if (h.length < 2) h = '0' + h;
		return [month, day, year].join('/') + ' ' + h + ':' + minute;
	}
	else if (formatType === "MM/dd/yyyy hh:mm tt") {
		h = d.getHours() === 0 ? '12' : '' + (d.getHours() > 12 ? d.getHours() - 12 : d.getHours());
		if (h.length < 2) h = '0' + h;
		t = d.getHours() >= 12 ? 'PM' : 'AM';
		return [month, day, year].join('/') + ' ' + h + ':' + minute + ' ' + t;
	}
	else if (formatType === "MM/dd/yy")
		return [month, day, year.substr(-2)].join('/');
	else if (formatType === "MM/dd/yy HH:mm")
		return [month, day, year.substr(-2)].join('/') + ' ' + hour + ':' + minute;
	else if (formatType === "MM/dd/yy hh:mm") {
		h = d.getHours() === 0 ? '12' : '' + (d.getHours() > 12 ? d.getHours() - 12 : d.getHours());
		if (h.length < 2) h = '0' + h;
		return [month, day, year.substr(-2)].join('/') + ' ' + h + ':' + minute;
	}
	else if (formatType === "MM/dd/yy hh:mm tt") {
		h = d.getHours() === 0 ? '12' : '' + (d.getHours() > 12 ? d.getHours() - 12 : d.getHours());
		if (h.length < 2) h = '0' + h;
		t = d.getHours() >= 12 ? 'PM' : 'AM';
		return [month, day, year.substr(-2)].join('/') + ' ' + h + ':' + minute + ' ' + t;
	}
	else if (formatType === "HH:mm") {
		h = d.getHours() === 0 ? '00' : '' + d.getHours();
		if (h.length < 2) h = '0' + h;
		return h + ':' + minute;
	}
		
	else if (formatType === "hh:mm tt") {
		h = d.getHours() === 0 ? '12' : '' + (d.getHours() > 12 ? d.getHours() - 12 : d.getHours());
		if (h.length < 2) h = '0' + h;
		t = d.getHours() >= 12 ? 'PM' : 'AM';
		return h + ':' + minute + ' ' + t;
	}
};

var isValidFormat = function (date, formatType) {
	// default is "MM/dd/yy hh:mm tt"
	var expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9] [0-1][0-9]:[0-5][0-9] [paPA][Mm]";
	if (formatType === "MM/dd/yy hh:mm tt") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9] [0-1][0-9]:[0-5][0-9] [paPA][Mm]";
	}
	else if (formatType === "MM/dd/yy hh:mm") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9] [0-1][0-9]:[0-5][0-9]";
	}
	else if (formatType === "MM/dd/yy HH:mm") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9] [0-2][0-9]:[0-5][0-9]";
	}
	else if (formatType === "MM/dd/yy") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9]";
	}
	else if (formatType === "MM/dd/yyyy hh:mm tt") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9][0-9][0-9] [0-1][0-9]:[0-5][0-9] [paPA][Mm]";
	}
	else if (formatType === "MM/dd/yyyy hh:mm") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9][0-9][0-9] [0-1][0-9]:[0-5][0-9]";
	}
	else if (formatType === "MM/dd/yyyy HH:mm") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9][0-9][0-9] [0-2][0-9]:[0-5][0-9]";
	}
	else if (formatType === "MM/dd/yyyy") {
		expr = "[0-1][0-9]\/[0-3][0-9]\/[0-9][0-9][0-9][0-9]";
	}
	else if (formatType === "yyyy-MM-dd") {
		expr = /[0-9][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9]/;
	}
	else if (formatType === "yyyy-MM-dd HH:mm") {
		expr = /[0-9][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9] [0-2][0-9]:[0-5][0-9]/;
	}
	else if (formatType === "yyyy-MM-dd hh:mm tt") {
		expr = /[0-9][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9] [0-2][0-9]:[0-5][0-9] [paPA][Mm]/;
	}
	
	var dateExp = new RegExp(expr);
	return dateExp.test(date);
};

var addDays = function (date, daysToAdd) {
	var returnDate = new Date(date);
	returnDate.setDate(returnDate.getDate() + daysToAdd);
	return returnDate;
};

var getLastSaturday = function () {
	var today = new Date();
	var lastSaturday = new Date(today.setDate(today.getDate() - (today.getDay() + 1)));
	return lastSaturday;
};

var getNextSaturday = function () {
	var today = new Date();
	var nextSaturday = new Date(today.setDate(today.getDate() + (6 - today.getDay())));
	return nextSaturday;
};

var isDate = function (dateToValidate) {
	if (Object.prototype.toString.call(dateToValidate) === "[object Date]") {
		// it is a date
		if (isNaN(dateToValidate.getTime())) {
			return false;
		}
		else {
			return true;
		}
	}
	else {
		return false;
	}
};

var convertUTCDateToLocalDate = function (date) {
	var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);
	return newDate;
};

var formatNumber = function (str) {
	return parseFloat(parseFloat(str).toFixed(0)).toLocaleString();
};