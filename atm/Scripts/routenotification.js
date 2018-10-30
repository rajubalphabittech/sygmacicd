// --- route related methods

var initializeRoutes = function () {
	var today = new Date();
	var routeParameters = [];
	routeParameters.filterStartDate = formatDate(today, "yyyy-MM-dd");
	routeParameters.filterEndDate = formatDate(today, "yyyy-MM-dd");
	loadRoutes(routeParameters);

	function loadRoutes(routeParameters) {
		var url = "/routenotification/list?centerNumber=" + routeParameters.center + "&filterStartDate=" + routeParameters.filterStartDate + "&filterEndDate=" + routeParameters.filterEndDate;
		getData(url,
			function () { $("#routeList").html("<i class='fa fa-spinner fa-5x fa-spin loading' aria-hidden='true'></i> &nbsp;&nbsp;loading"); }, // preCall
			function (data) { $("#routeList").html(data); }, // doneCallBack
			function (error) { $("#routeList").html(error); }, // failCallBack
			function () { initializeRouteAndStopHandlers(); }
		);
	}
};

var initializeRouteAndStopHandlers = function () {
	$(".stopTable").on("click", ".comment-dialog-trigger", function () {
		var shipTo = $(this).data("shipTo");
		var billTo = $(this).data("billTo");
		var centerNumber = $(this).data("centerNumber");
		var routePlanId = $(this).data("routePlanId");
		var stopNumber = $(this).data("stopNumber");

		loadCommentList(billTo, shipTo, centerNumber, routePlanId, stopNumber, "RN", true);
	});

	$("#routes-table .select-all-routes:checkbox").on("change", function () {
		if ($(this).is(':checked')) {
			// check all 
			$("#routes-table .select-all-stops-in-route:checkbox").prop('checked', true);
			$("#routes-table .select-stop:checkbox").prop('checked', true);
		} else {
			// uncheck all 
			$("#routes-table .select-all-stops-in-route:checkbox").removeAttr('checked');
			$("#routes-table .select-stop:checkbox").removeAttr('checked');
		}
	});

	$("#routes-table .select-all-stops-in-route:checkbox").on("change", function () {
		var routeId = $(this).data("routeId");
		if ($(this).is(':checked')) {
			// check all 
			$("#routes-table .select-stop[data-route-id='" + routeId + "']:checkbox").prop('checked', true);
		} else {
			// uncheck all 
			$("#routes-table .select-stop[data-route-id='" + routeId + "']:checkbox").removeAttr('checked');
		}
	});
};

var initializeSendNotificationHandler = function () {
	$("#sendNotificationButton").click(function () {
		var $selectedStops = $("#routes-table .select-stop:checked");
		var emails = [];
		$selectedStops.each(function (index) {
			emails.push($(this).data("email"));
		});

		var url = "/routenotification/notify";
		jQuery.ajax({
			url: url,
			type: 'POST',
			data: { tos: emails }
		}).success(function (data) {
			$("#toast-window").html("<span> ... Notifications are sent successfully ... </span>");
			$("#toast-window").addClass("toast-is-shown");
			setTimeout(function () {
				$("#toast-window").removeClass("toast-is-shown");
			}, 3000);
		}).error(function (fData) {
			$("#toast-window").html("<span> ... Failed sending notifications ... </span>");
			$("#toast-window").addClass("toast-is-shown");
			setTimeout(function () {
				$("#toast-window").removeClass("toast-is-shown");
			}, 3000);
		});
	});
};
