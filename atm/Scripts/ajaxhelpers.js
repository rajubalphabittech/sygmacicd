var getData = function (url, preCall, doneCallBack, failCallBack, alwaysCallBack) {
	$.ajax({
		async: true,
		url: url,
		type: "GET",
		beforeSend: function (xhr) {
			preCall();
		}
	}).done(function (data) { doneCallBack(data); })
		.fail(function (error) { failCallBack(error); })
		.always(function () { alwaysCallBack(); });
};

var postData = function (url, data, preCall, doneCallBack, failCallBack, alwaysCallBack) {
	$.ajax({
		async: true,
		url: url,
		type: "POST",
		data: data,
		beforeSend: function (xhr) {
			preCall();
		}
	}).done(function (data) { doneCallBack(data); })
		.fail(function (error) { failCallBack(error); })
		.always(function () { alwaysCallBack(); });
};
