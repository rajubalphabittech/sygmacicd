var routeMapArray = []; var map; var defaultZoom = 11; var directionsService;
var directionDisplayRenderEngineArray = []; var iconColor = '';


var colorArrayLeft = [
	'#ffffff', // white
	'#f08080', // Light Coral
	'#00ffff', // cyan
	'#76ee00', // light green
	'#eeee00', // yellow
	'#ee82ee', // violet
	'#ffb5c5', // pink
	'#63b8ff', // steel blue
	'#ffa500', // orange
	'#9b30ff', // purple
	'#BDB76B' // Dark Kakhi
];

var colorArrayRight = [
	'#ffffff', // white
	'#BDB76B', // Dark Kakhi
	'#9b30ff', // purple
	'#ffa500', // orange
	'#63b8ff', // steel blue
	'#ffb5c5', // pink
	'#ee82ee', // violet
	'#eeee00', // yellow
	'#76ee00', // light green
	'#00ffff', // cyan
	'#ff0000' // red
];

// route & filter slider ui related

var openRoute = function (position) {
	$("#" + position + "Route").css(position, "0px");
	$("#" + position + "Route").css("width", "25%");
	$(".btn-collapse-" + position).hide();
	$("#" + position + "Route .restorebtn").hide();
	$("#" + position + "Route .minimizebtn").show();
	if (position === "right") {
		$("#" + position + "Route .restorebtn").css("left", "");
		$("#" + position + "Route .restorebtn").css("right", "25px");
		$("#" + position + "Route .closebtn").css("left", "");
		$("#" + position + "Route .closebtn").css("right", "5px");
	}
};

var closeRoute = function (position) {
	// close stop list slider
	closeStopListSlider(position);

	$("#" + position + "Route").css(position, "-30px");
	$("#" + position + "Route").css("width", "0%");
	$(".btn-collapse-" + position).show(800);
};

var minimizeRoute = function (position) {
	$("#" + position + "Route").css(position, "-20%");
	if ($(".stop-list[data-position='" + position + "']").hasClass("stop-list-is-displayed")) {
		$(".stop-list-is-displayed[data-position='" + position + "']").addClass("stop-list-is-stretched");
	}
	$("#" + position + "Route .minimizebtn").hide();
	$("#" + position + "Route .restorebtn").show();
	if (position === "right") {
		$("#" + position + "Route .restorebtn").css("right", "");
		$("#" + position + "Route .restorebtn").css("left", "5px");
		$("#" + position + "Route .closebtn").css("right", "");
		$("#" + position + "Route .closebtn").css("left", "25px");
	}
};

var restoreRoute = function (position) {
	openRoute(position);
	if ($(".stop-list[data-position='" + position + "']").hasClass("stop-list-is-stretched")) {
		$(".stop-list-is-displayed[data-position='" + position + "']").removeClass("stop-list-is-stretched");
	}
};

var closeStopListSlider = function (position) {
	if ($(".stop-list[data-position='" + position + "']").hasClass("stop-list-is-stretched")) {
		$(".stop-list[data-position='" + position + "']").removeClass("stop-list-is-stretched");
	}
	if ($(".stop-list[data-position='" + position + "']").hasClass("stop-list-is-displayed")) {
		$(".stop-list[data-position='" + position + "']").removeClass("stop-list-is-displayed").addClass("stop-list-is-collapsed");
	}

	// flip stop list arrow
	var reversePosition = position === "left" ? "right" : (position === "right" ? "left" : "right");
	$(".route-header[data-position='" + position + "'] .stop-list-toggle").find("i").removeClass("fa-chevron-" + position).addClass("fa-chevron-" + reversePosition).fadeTo("slow", 0.25);
};


// filter execution

var filterRoutes = function (position) {
	//initialize variables in case of empty parameters.
	var nameNumberFilter = $(".route-filter-name[data-position='" + position + "']").val();
	if (!(nameNumberFilter === '')) {
		nameNumberFilter = $(".route-filter-name[data-position='" + position + "']").val().toUpperCase();
	}

	var weightFilter = $(".checkbox :checkbox[data-position='" + position + "'][data-filtercategory='weight']").is(':checked');
	var cubesFilter = $(".checkbox :checkbox[data-position='" + position + "'][data-filtercategory='cubes']").is(':checked');

	var weightFilterOperator = $(".route-filter-weight-operator[data-position='" + position + "']").val();
	var weightThreshold = $(".route-filter-weight[data-position='" + position + "']").val();
	var cubesFilterOperator = $(".route-filter-cubes-operator[data-position='" + position + "']").val();
	var cubesThreshold = $(".route-filter-cubes[data-position='" + position + "']").val();

	var rows = $("#routeList" + position + ">div");

	rows.each(function (index) {
		var routeNumber = $(this).data("routenumber").toString().toUpperCase();
		var routeName = $(this).data("routename").toUpperCase();
		var weight = $(this).data("totalweight");
		var cubes = $(this).data("totalcubes");

		//Check the search field.  Empty will generate a hit for each record.
		if (routeNumber.indexOf(nameNumberFilter) > -1 || routeName.indexOf(nameNumberFilter) > -1) {
			var showRow = false;
			if (weightFilter) {
				if (filterRouteByParam(weight, weightFilterOperator, weightThreshold)) {
					if (cubesFilter) {
						if (filterRouteByParam(cubes, cubesFilterOperator, cubesThreshold)) {
							showRow = true;
						}
					} else {
						showRow = true;
					}
				}
			} else {
				if (cubesFilter) {
					if (filterRouteByParam(cubes, cubesFilterOperator, cubesThreshold)) {
						showRow = true;
					}
				} else {
					showRow = true;
				}
			}
			if (showRow) { $(this).show(); } else { $(this).hide(); }
		} else {
			$(this).hide();
		}
	});
};

var filterRouteByParam = function (param, operator, paramFilter) {
	var routeOK = false;
	switch (operator) {
		case "1":
			routeOK = parseInt(param) > parseInt(paramFilter);
			break;
		case "2":
			routeOK = parseInt(param) >= parseInt(paramFilter);
			break;
		case "3":
			routeOK = parseInt(param) < parseInt(paramFilter);
			break;
		case "4":
			routeOK = parseInt(param) <= parseInt(paramFilter);
			break;
		default:
			break;
	}

	return routeOK;
};

var clearRoutes = function (position) {
	// clearing selection 
	clearPanels(position);

	// clear routes from map
	clearMap(position);
	loadMap();
};

var clearPanels = function (position) {
	var reversePosition = position === "left" ? "right" : (position === "right" ? "left" : "right");

	// turn off bind to route checkbox to reduce loop-feedback
	$(".route-header[data-position='" + position + "'] :checkbox").off();
	$(".route-header[data-position='" + position + "'] .primary-route-toggle").off();
	$(".route-header[data-position='" + position + "'] .stop-list-toggle").off();

	// unchecked all
	$(".route-header[data-position='" + position + "'] :checkbox").prop('checked', false);

	// clear highlights
	$(".route-header[data-position='" + position + "']").css('background-color', 'white');

	// close all details
	$(".route-header[data-position='" + position + "']").each(function (index) {
		$(this).closest("div").next().hide();
	});

	// clear/hide all chevrons
	$(".route-header[data-position='" + position + "']").find(".primary-route-toggle").find("i").css('color', '#333333').fadeTo("slow", 0.15);
	$(".route-header[data-position='" + position + "']").find(".primary-route-toggle").hide();
	$(".route-header[data-position='" + position + "']").find(".stop-list-toggle").find("i").removeClass("fa-chevron-" + position).addClass("fa-chevron-" + reversePosition).fadeTo("slow", 0.25);
	$(".route-header[data-position='" + position + "']").find(".stop-list-toggle").hide();

	// clear primary marker
	$(".route-header[data-position='" + position + "'] .primary-route-toggle").removeClass("primary-route-for-proximity-search");

	// close all sliders
	closeStopListSlider(position);

	// rebind 
	bindRouteHeader(position);

	if (position === "left") clearPanels("right");
};


// event bindings 

var bindRouteFilters = function () {
	$(".route-filter-timing-type-selector").on("change", function () {
		var dateType = $(this).val();
		var position = $(this).data("position");
		if (dateType === "1") {
			$("#RouteWeekendingDate" + position).parent().show();
			$("#RouteDispatchStartDate" + position).parent().hide();

			var weekendingDate = new Date();
			while (weekendingDate.getDay() !== 6) {
				weekendingDate.setDate(weekendingDate.getDate() + 1);
			}
			var weekStartDate = addDays(weekendingDate, -6);
			$(".route-filter-dispatch-start-date[data-position='" + position + "']").val(formatDate(weekStartDate, "yyyy-MM-dd"));
			$(".route-filter-dispatch-end-date[data-position='" + position + "']").val(formatDate(weekendingDate, "yyyy-MM-dd"));
			$(".route-filter-weekending-date[data-position='" + position + "']").val(formatDate(weekendingDate, "yyyy-MM-dd"));
			$(".route-filter-weekending-date[data-position='" + position + "']").change();
		} else {
			clearRoutes(position);
			$(".route-filter-dispatch-start-date").off("change", "**");
			$("#RouteWeekendingDate" + position).parent().hide();
			$("#RouteDispatchStartDate" + position).parent().show();
			var endDate = new Date(); var startDate = new Date();
			if (dateType === "2") { // current week
				while (endDate.getDay() !== 6) { endDate = addDays(endDate, 1); }
				startDate = addDays(endDate, -6);
			} else if (dateType === "3") { // last week
				while (endDate.getDay() !== 0) { endDate = addDays(endDate, -1); }
				endDate = addDays(endDate, -1);
				startDate = addDays(endDate, -6);
			} else if (dateType === "4") { // tomorrow
				startDate = addDays(startDate, 1);
				endDate = addDays(startDate, 1);
			} else if (dateType === "5") { // today
				startDate = startDate;
				endDate = addDays(startDate, 1);
			} else if (dateType === "6") { // yesterday
				startDate = addDays(startDate, -1);
				endDate = endDate;
			} else if (dateType === "7") { // custom
				startDate = addDays(new Date(formatDate($(".route-filter-dispatch-start-date[data-position='" + position + "']").val(), "MM/dd/yyyy")), 1);
				endDate = addDays(new Date(formatDate($(".route-filter-dispatch-end-date[data-position='" + position + "']").val(), "MM/dd/yyyy")), 1);
			}

			$(".route-filter-dispatch-start-date[data-position='" + position + "']").val(formatDate(startDate, "yyyy-MM-dd"));
			$(".route-filter-dispatch-end-date[data-position='" + position + "']").val(formatDate(endDate, "yyyy-MM-dd"));
			bindStartEndDate();

			var center = $(".route-filter-center-selector").val();
			var sortField = $(".route-sorter[data-position='" + position + "']").val();
			var direction = $(".route-sorter-direction[data-position='" + position + "']").html() === "ASC" ? "1" : "2";
			var billTo = $(".route-filter-bill-to[data-position='" + position + "']").val();
			var shipTo = $(".route-filter-ship-to[data-position='" + position + "']").val();

			if (position === "right") {
				var selectedRouteId = $(".route-header[data-position='left'] .primary-route-for-proximity-search").data("routeid");
				var selectedRoutePlanId = $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").length > 0 ? $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").data("routeplanid") : "";
				loadRoutes(position, center, formatDate(startDate, "yyyy-MM-dd"), formatDate(endDate, "yyyy-MM-dd"), sortField, direction, selectedRouteId, selectedRoutePlanId, billTo, shipTo);
			} else {
				loadRoutes(position, center, formatDate(startDate, "yyyy-MM-dd"), formatDate(endDate, "yyyy-MM-dd"), sortField, direction, '', '', billTo, shipTo);
			}
		}
	});

	$(".route-filter-weekending-date").on("change", function () {
		if ($(".route-filter-center-selector").val() !== "") {
			if ($(this).val() !== undefined && $(this).val() !== null && $(this).val().trim() !== '') {
				var centerMap = routeMapArray[0];
				routeMapArray = [];
				routeMapArray.push(centerMap);
				var position = $(this).data("position");
				$(".route-filter-name[data-position='" + position + "']").val("");
				var center = $(".route-filter-center-selector").val();
				var endDate = new Date($(this).val());
				while (endDate.getDay() !== 6) { endDate = addDays(endDate, 1); }
				var startDate = new Date(endDate);
				startDate = addDays(startDate, -6);

				var direction = $(".route-sorter-direction[data-position='" + position + "']").html() === "ASC" ? "1" : "2";
				var sortField = $(".route-sorter[data-position='" + position + "']").val();
				var billTo = $(".route-filter-bill-to[data-position='" + position + "']").val();
				var shipTo = $(".route-filter-ship-to[data-position='" + position + "']").val();

				if (position === "right" && sortField === "9") {
					var selectedRouteId = $(".route-header[data-position='left'] .primary-route-for-proximity-search").data("routeid");
					var selectedRoutePlanId = $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").length > 0 ? $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").data("routeplanid") : "";
					loadRoutes(position, center, formatDate(startDate, "M/d/YYYY"), formatDate(endDate, "M/d/YYYY"), sortField, direction, selectedRouteId, selectedRoutePlanId, billTo, shipTo);
				} else {
					loadRoutes(position, center, formatDate(startDate, "M/d/YYYY"), formatDate(endDate, "M/d/YYYY"), sortField, direction, '', '', billTo, shipTo);
				}
			}
		}
	});

	// ajax handler for center location selection
	$(".route-filter-center-selector").on("change", function () {
		routeMapArray = [];
		var center = $(this).val();
		if (center === '') {
			$("#managerAddCommentButton").prop('disabled', true);
			$("#managerMoveStopButton").prop('disabled', true);
		} else {
			$("#managerAddCommentButton").prop('disabled', false);
			//$("#managerMoveStopButton").prop('disabled', false);
		}
		$(".center-block").data("centernumber", center);
		$(".route-filter-name[data-position='left']").val("");
		$(".route-filter-name[data-position='right']").val("");
		$(".route-filter-bill-to[data-position='left']").val("");
		$(".route-filter-ship-to[data-position='left']").val("");
		$(".route-filter-bill-to[data-position='right']").val("");
		$(".route-filter-ship-to[data-position='right']").val("");
		if ($(".route-filter-timing-type-selector[data-position='left']").val() !== 1) {
			closeStopListSlider('left');
			loadRoutes('left', center, $(".route-filter-dispatch-start-date[data-position='left']").val(), $(".route-filter-dispatch-end-date[data-position='left']").val(), $(".route-sorter[data-position='left']").val(), $(".route-sorter-direction[data-position='left']").html() === "ASC" ? "1" : "2");
		}
		if ($("#rightRoute").css('right') === "0px") {
			if ($(".route-filter-timing-type-selector[data-position='right']").val() !== 1) {
				var sortDirection = $(".route-sorter-direction[data-position='right']").html() === "ASC" ? "1" : "2";
				var sortField = $(".route-sorter[data-position='right']").val();
				closeStopListSlider('right');
				if (sortField === "9") {
					var selectedRouteId = $(".route-header[data-position='left'] .primary-route-for-proximity-search").data("routeid");
					var selectedRoutePlanId = $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").length > 0 ? $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").data("routeplanid") : "";
					loadRoutes('right', center, $(".route-filter-dispatch-start-date[data-position='right']").val(), $(".route-filter-dispatch-end-date[data-position='right']").val(), sortField, sortDirection, selectedRouteId, selectedRoutePlanId);
				} else {
					loadRoutes('right', center, $(".route-filter-dispatch-start-date[data-position='right']").val(), $(".route-filter-dispatch-end-date[data-position='right']").val(), sortField, sortDirection);
				}
			}
		}
		if ($("#leftRoute").css('left') !== "0px") {
			openRoute("left");
		}
	});

	// auto-filter routes as type in name-number filter box
	$(".route-filter-name").on("keyup", function () {
		filterRoutes($(this).data("position"));
	});

	// ajax handler for sorting
	$(".route-sorter").on("change", function () {
		var position = $(this).data("position");
		$(".route-filter-timing-type-selector[data-position='" + position + "']").change();
	});

	// ajax handler for sort direction
	$(".route-sorter-direction").on("click", function () {
		if ($(this).html() === "ASC") {
			$(this).html("DESC");
		} else {
			$(this).html("ASC");
		}
		var position = $(this).data("position");
		$(".route-filter-timing-type-selector[data-position='" + position + "']").change();
	});

	// ajax handler for search button
	$(".route-search-button").on("click", function () {
		var position = $(this).data("position");
		$(".route-filter-timing-type-selector[data-position='" + position + "']").change();
	});

	// toggling filter-category panel (accordion/up-down)
	$(".filter-header").click(function () {
		// collapse panel-body
		$(this).parent().parent().next().toggle();

		// toggle icon
		if ($(this).find("i").hasClass("fa-chevron-up")) {
			$(this).find("i").removeClass("fa-chevron-up").addClass("fa-chevron-down");
		} else {
			$(this).find("i").removeClass("fa-chevron-down").addClass("fa-chevron-up");
		}
	});

	// turn on/off weight/cubes
	$('.filter-category :checkbox').change(function () {
		filterRoutes($(this).data("position"));
	});

	// operator drop down change
	$('.route-filter-operator').change(function () {
		var position = $(this).data("position");
		var threshold = $(".route-filter-threshold[data-position='" + position + "']").val();
		if (threshold.length > 3) filterRoutes(position);
	});

	// weight/cubes filter value change
	$('.route-filter-threshold').change(function () {
		var position = $(this).data("position");
		var threshold = $(".route-filter-threshold[data-position='" + position + "']").val();
		if (threshold.length > 3) filterRoutes(position);
	});

	bindStartEndDate();

	function bindStartEndDate() {
		$(".route-filter-dispatch-start-date").on("change", function () {
			var center = $(".route-filter-center-selector").val();
			var position = $(this).data("position");
			clearRoutes(position);
			var startDate = $(this).val();
			var endDate = $(".route-filter-dispatch-end-date[data-position='" + position + "']").val();
			var sortField = $(".route-sorter[data-position='" + position + "']").val();
			var direction = $(".route-sorter-direction[data-position='" + position + "']").html() === "ASC" ? "1" : "2";
			var billTo = $(".route-filter-bill-to[data-position='" + position + "']").val();
			var shipTo = $(".route-filter-ship-to[data-position='" + position + "']").val();
			$(".route-filter-name[data-position='" + position + "']").val("");
			$(".route-filter-timing-type-selector[data-position='" + position + "']").val("7");
			if (position === "right" && sortField === "9") {
				var selectedRouteId = $(".route-header[data-position='left'] .primary-route-for-proximity-search").data("routeid");
				var selectedRoutePlanId = $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").length > 0 ? $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").data("routeplanid") : "";
				loadRoutes(position, center, startDate, endDate, sortField, direction, selectedRouteId, selectedRoutePlanId, billTo, shipTo);
			} else {
				loadRoutes(position, center, startDate, endDate, sortField, direction, '', '', billTo, shipTo);
			}
		});

		$(".route-filter-dispatch-end-date").on("change", function () {
			var center = $(".route-filter-center-selector").val();
			var position = $(this).data("position");
			clearRoutes(position);
			var startDate = $(".route-filter-dispatch-start-date[data-position='" + position + "']").val();
			var endDate = $(this).val();
			var sortField = $(".route-sorter[data-position='" + position + "']").val();
			var direction = $(".route-sorter-direction[data-position='" + position + "']").html() === "ASC" ? "1" : "2";
			var billTo = $(".route-filter-bill-to[data-position='" + position + "']").val();
			var shipTo = $(".route-filter-ship-to[data-position='" + position + "']").val();
			$(".route-filter-name[data-position='" + position + "']").val("");
			$(".route-filter-timing-type-selector[data-position='" + position + "']").val("7");
			if (position === "right" && sortField === "9") {
				var selectedRouteId = $(".route-header[data-position='left'] .primary-route-for-proximity-search").data("routeid");
				var selectedRoutePlanId = $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").length > 0 ? $(".stop-header[data-position='left'] .primary-stop-for-stop-proximity-search").data("routeplanid") : "";
				loadRoutes(position, center, startDate, endDate, sortField, direction, selectedRouteId, selectedRoutePlanId, billTo, shipTo);
			} else {
				loadRoutes(position, center, startDate, endDate, sortField, direction, '', '', billTo, shipTo);
			}
		});
	}
};

var bindRouteHeader = function (position) {
	var routeIdArray = [];

	$(".route-header[data-position='" + position + "'] :checkbox").on("change", function () {
		var $headerDiv = $(this).closest("div.route-header");
		var routeId = $headerDiv.data("routeid");
		var centerNumber = $(".route-filter-center-selector").val();
		var routeNumber = $headerDiv.data("routenumber");
		var primaryRouteChevron = $headerDiv.find(".primary-route-toggle");
		var stopListChevron = $headerDiv.find(".stop-list-toggle");
		var reversePosition = position === "left" ? "right" : (position === "right" ? "left" : "right");

		if ($(this).is(':checked')) {
			// if checked, add route to map and apply color
			if (routeIdArray.indexOf(routeId) === -1) {
				routeIdArray.push(routeId);
				addRouteToMap(routeId, centerNumber, routeNumber, position);
			}

			// toggle route detail underneath
			$headerDiv.closest("div").next().toggle();

			// show dimmed primary route chevron
			if (position === "left") primaryRouteChevron.show();

			// show stop list chevron
			stopListChevron.show();
		} else {
			// remove & reset color
			var removeIndex = routeIdArray.indexOf(routeId);
			if (removeIndex > -1) { routeIdArray.splice(removeIndex, 1); }
			$headerDiv.css('background-color', 'white');

			// toggle route detail underneath
			$headerDiv.closest("div").next().toggle();

			// hide stop list chevron
			stopListChevron.hide();

			// reset the stop list arrow chevron
			stopListChevron.find("i").removeClass("fa-chevron-" + position).addClass("fa-chevron-" + reversePosition);

			var clearRightMap = false;
			if (position === "left") {
				if ($headerDiv.find(".primary-route-toggle").hasClass("primary-route-for-proximity-search") || hexColor(primaryRouteChevron.find("i").css('color')) === "#000000") {
					clearRightMap = true;
				}
			}
			// remove from map
			removeRouteFromMap(routeId, position, clearRightMap);

			// close the stop list slider if stop list is holding the current unchecked route
			if ($(".stop-list-content[data-position='" + position + "']").data("routeid") === routeId) {
				closeStopListSlider(position);
			}

			if (position === "left") {
				// remove all routes on the right
				if ($headerDiv.find(".primary-route-toggle").hasClass("primary-route-for-proximity-search") || hexColor(primaryRouteChevron.find("i").css('color')) === "#000000") {
					clearRoutes("right");
					$(".route-list[data-position='right']").html("");
				}

				// reset & hide primary route chevron
				primaryRouteChevron.find("i").css('color', '#333333').fadeTo("slow", 0.15);
				primaryRouteChevron.hide();
			}
		}

		// color code the list on the left based on the route color
		$.each(routeIdArray, function (key, value) {
			$(".route-header[data-position='" + position + "'][data-routeid='" + value + "']").css("background-color", position === "left" ? colorArrayLeft[(key + 1) % 10] : colorArrayRight[(key + 1) % 10]);
		});
	});

	$(".route-header[data-position='" + position + "'] .primary-route-toggle").on("click", function () {
		var routeId = $(this).data("routeid");
		var chevronIcon = $(this).find("i");

		// turn off all stop header primary indicator
		$(".stop-header[data-position='" + position + "'] .primary-stop-toggle").find("i").css('color', '#333333').fadeTo("slow", 0.15);

		// turn off all route header primary indicator
		$(".route-header[data-position='" + position + "'] .primary-route-toggle").find("i").css('color', '#333333').fadeTo("slow", 0.15);

		// turn on the clicked one
		chevronIcon.css('color', '#337ab7').fadeTo("slow", 1.0);

		// remove mark from old selection
		$(".route-header[data-position='" + position + "'] .primary-route-toggle").removeClass("primary-route-for-proximity-search");

		// mark it
		$(this).addClass("primary-route-for-proximity-search");

		// do proximity search 
		loadProximityRoutes(routeId);
	});

	$(".route-header[data-position='" + position + "'] .stop-list-toggle").on("click", function (e) {
		var centerNumber = $(".route-filter-center-selector").val();
		var routeId = $(this).data("routeid");
		var routeNumber = $(this).data("routenumber");
		var chevronIcon = $(this).find("i");
		var reversePosition = position === "left" ? "right" : (position === "right" ? "left" : "right");

		if ($(this).find("i").hasClass("fa-chevron-" + reversePosition)) {
			// get stop list data
			loadRouteStops(routeId, centerNumber, routeNumber, position);

			// display stop list
			var $stopList = $(".stop-list[data-position='" + position + "']");
			if ($stopList.hasClass("stop-list-is-collapsed")) {
				if ($("#" + position + "Route").css(position) === "0px") {
					// route slider is in open/restored position
					$stopList
						.removeClass("stop-list-is-collapsed")
						.addClass("stop-list-is-displayed");
				} else {
					// route slider is in minimized position
					$stopList
						.removeClass("stop-list-is-collapsed")
						.addClass("stop-list-is-displayed")
						.addClass("stop-list-is-stretched");
				}
			}

			// reset all stop list arrow
			$(".route-header[data-position='" + position + "'] .stop-list-toggle").find("i").removeClass("fa-chevron-" + position).addClass("fa-chevron-" + reversePosition).fadeTo("slow", 0.25);

			// flip stop list arrow
			chevronIcon.removeClass("fa-chevron-" + reversePosition).addClass("fa-chevron-" + position).fadeTo("slow", 1.0);
		} else {
			// close stop list
			closeStopListSlider(position);
		}
	});
};

var bindStopHeader = function () {
	$(".stop-list").on("click", ".stop-header .primary-stop-toggle", function () {
		var routeId = $(this).data("routeid");
		var routePlanId = $(this).data("routeplanid");
		var position = $(this).data("position");
		var chevronIcon = $(this).find("i");

		// turn off all route header primary indicator
		$(".route-header[data-position='" + position + "'] .primary-route-toggle").find("i").css('color', '#333333').fadeTo("slow", 0.15);
		$(".route-header[data-position='" + position + "'] .primary-route-toggle").removeClass("primary-route-for-proximity-search");

		// turn on the route where the stop belong to
		$(".route-header[data-position='" + position + "'] .primary-route-toggle[data-routeid='" + routeId + "']").find("i").css('color', '#337ab7').fadeTo("slow", 1.0);
		$(".route-header[data-position='" + position + "'] .primary-route-toggle[data-routeid='" + routeId + "']").addClass("primary-route-for-proximity-search");

		// turn off all stop header primary indicator
		$(".stop-header[data-position='" + position + "'] .primary-stop-toggle").find("i").css('color', '#333333').fadeTo("slow", 0.15);

		// turn on the clicked one
		chevronIcon.css('color', '#337ab7').fadeTo("slow", 1.0);

		// remove mark from old selection
		$(".stop-header[data-position='" + position + "']").removeClass("primary-stop-for-stop-proximity-search");

		// mark it
		$(this).addClass("primary-stop-for-stop-proximity-search");

		loadProximityRoutes(routeId, routePlanId);
	});
};

var bindStopDetailToggles = function () {
	$(".stop-list").on("click", ".stop-detail-toggle", function () {
		$(this).closest(".stop-item").find(".stop-details[data-routeplanid='" + $(this).data("routeplanid") + "']").toggle();

		// toggle icon
		if ($(this).find("i").hasClass("fa-chevron-up")) {
			$(this).find("i").removeClass("fa-chevron-up").addClass("fa-chevron-down");
		} else {
			$(this).find("i").removeClass("fa-chevron-down").addClass("fa-chevron-up");
		}
	});

	$(".stop-list").on("click", ".all-stop-details-toggle", function () {
		if ($(this).find("i").hasClass("fa-chevron-circle-up")) {
			// flip stop-list chevron
			$(this).find("i").removeClass("fa-chevron-circle-up").addClass("fa-chevron-circle-down");
			// flip all stop-items chevron
			$(this).parent().parent().parent().next().find("i.fa-chevron-up").removeClass("fa-chevron-up").addClass("fa-chevron-down");
			// hide all stop-detail
			$(this).parent().parent().parent().next().find(".stop-details[data-routeid='" + $(this).data("routeid") + "']").hide();
		} else {
			// flip stop-list chevron
			$(this).find("i").removeClass("fa-chevron-circle-down ").addClass("fa-chevron-circle-up");
			// flip all stop-items chevron
			$(this).parent().parent().parent().next().find("i.fa-chevron-down").removeClass("fa-chevron-down").addClass("fa-chevron-up");
			// show all stop-detail
			$(this).parent().parent().parent().next().find(".stop-details[data-routeid='" + $(this).data("routeid") + "']").show();
		}
	});
};

var bindStopMove = function () {
	$("#managerMoveStopButton").prop('disabled', true);

	$(".stop-list").on("click", ".stop-move-btn[data-position='left'][data-canmove='True']", function (e) {
		var centerNumber = $(".route-filter-center-selector").val();
		var leftRouteId = $(this).data("routeid");
		var leftRoutePlanId = $(this).data("routeplanid");
		var leftDeliveryDateTime = $(this).data("planneddeliverydatetime");
		var rightRouteNumber = ($(".stop-list-content[data-position='right']").length > 0 ? $(".stop-list-content[data-position='right']").data("routenumber") : "");
		var rightRouteId = ($(".stop-list-content[data-position='right']").length > 0 ? $(".stop-list-content[data-position='right']").data("routeid") : "");
		loadStopMove(centerNumber, leftRouteId, leftRoutePlanId, leftDeliveryDateTime, rightRouteId, rightRouteNumber);
	});

	$("#managerMoveStopButton").on("click", function () {
		var centerNumber = $(".route-filter-center-selector").val();
		loadStopMove(centerNumber, 0, 0, "", 0, "");
	});

	$("#dialog-window").on("change", "#SourceRouteNumberDDL", function () {
		var routeId = $(this).val();
		var centerNumber = $("#centerNumber").val();
		var initialRoutePlanId = $("#SourceRoutePlanIdInitialValue").val();

		if (routeId !== "" && routeId !== "0") {
			$("#SourceStopNumberDDL").parent().next().find(".fa-spinner").removeClass("hidden");
			var url = "/routemanager/stopsdata/" + routeId + "?centerNumber=" + centerNumber;
			$.getJSON(url, null, function (data) {
				$("#SourceStopNumberDDL").empty();
				data = JSON.parse(data);
				$("#SourceTotalWeight").text(formatNumber(data.TotalWeight));
				$("#SourceTotalWeight").data('weight', data.TotalWeight);
				$("#SourceTotalWeightFreezer").text(formatNumber(data.TotalWeightFreezer));
				$("#SourceTotalWeightFreezer").data('weight', data.TotalWeightFreezer);
				$("#SourceTotalWeightCooler").text(formatNumber(data.TotalWeightCooler));
				$("#SourceTotalWeightCooler").data('weight', data.TotalWeightCooler);
				$("#SourceTotalWeightDry").text(formatNumber(data.TotalWeightDry));
				$("#SourceTotalWeightDry").data('weight', data.TotalWeightDry);

				$("#SourceTotalCubes").text(formatNumber(data.TotalCubes));
				$("#SourceTotalCubes").data('cubes', data.TotalCubes);
				$("#SourceTotalCubesFreezer").text(formatNumber(data.TotalCubesFreezer));
				$("#SourceTotalCubesFreezer").data('cubes', data.TotalCubesFreezer);
				$("#SourceTotalCubesCooler").text(formatNumber(data.TotalCubesCooler));
				$("#SourceTotalCubesCooler").data('cubes', data.TotalCubesCooler);
				$("#SourceTotalCubesDry").text(formatNumber(data.TotalCubesDry));
				$("#SourceTotalCubesDry").data('cubes', data.TotalCubesDry);

				$("#SourceTotalCases").text(formatNumber(data.TotalCases));
				$("#SourceTotalCases").data('cases', data.TotalCases);
				$("#SourceTotalCasesFreezer").text(formatNumber(data.TotalCasesFreezer));
				$("#SourceTotalCasesFreezer").data('cases', data.TotalCasesFreezer);
				$("#SourceTotalCasesCooler").text(formatNumber(data.TotalCasesCooler));
				$("#SourceTotalCasesCooler").data('cases', data.TotalCasesCooler);
				$("#SourceTotalCasesDry").text(formatNumber(data.TotalCasesDry));
				$("#SourceTotalCasesDry").data('cases', data.TotalCasesDry);

				if (data.RouteStops.length === 0 || (data.RouteStops.length === 1 && data.RouteStops[0].StopNumber.toString() === "0")) {
					$("#SourceStopNumberDDL").parent().next().find(".fa-spinner").addClass("hidden");
					$("#SourceStopNumberDDL").empty();
					alert("The selected SOURCE ROUTE cannot be moved because it has no Stops/Orders associated with it.");
					$("#SourceRouteNumberDDL").data('ready', "false");
				} else {
					if (data.HasDispatched) {
						$("#SourceStopNumberDDL").parent().next().find(".fa-spinner").addClass("hidden");
						$("#SourceStopNumberDDL").empty();
						alert("The selected SOURCE ROUTE cannot be moved because it has been dispatched.");
						$("#SourceRouteNumberDDL").data('ready', "false");
					} else {
						var i = 0;
						while (i < data.RouteStops.length) {
							var stop = data.RouteStops[i];
							if (stop.StopNumber.toString() !== "0") {
								var optionItem = "<option value='" + stop.RoutePlanId + "'" +
									" data-route-number='" + stop.RouteNumber + "'" +
									" data-route-id='" + stop.RouteId + "'" +
									" data-order-id='" + stop.OrderId + "'" +
									" data-planned-delivery-datetime='" + formatDate(new Date(stop.PlannedDeliveryDateTime), "MM/dd/yyyy hh:mm tt") + "'" +
									" data-scheduled-delivery-datetime='" + formatDate(new Date(stop.ScheduledDeliveryDateTime), "MM/dd/yyyy hh:mm tt") + "'" +
									" data-adjusted-delivery-datetime='" + formatDate(new Date(stop.AdjustedDeliveryDateTime), "MM/dd/yyyy hh:mm tt") + "'" +

									" data-weight-total='" + stop.Weight + "'" +
									" data-weight-freezer='" + stop.WeightFreezer + "'" +
									" data-weight-cooler='" + stop.WeightCooler + "'" +
									" data-weight-dry='" + stop.WeightDry + "'" +

									" data-cubes-total='" + stop.Cubes + "'" +
									" data-cubes-freezer='" + stop.CubesFreezer + "'" +
									" data-cubes-cooler='" + stop.CubesCooler + "'" +
									" data-cubes-dry='" + stop.CubesDry + "'" +

									" data-cases-total='" + stop.Cases + "'" +
									" data-cases-freezer='" + stop.CasesFreezer + "'" +
									" data-cases-cooler='" + stop.CasesCooler + "'" +
									" data-cases-dry='" + stop.CasesDry + "'" +

									" data-bill-to='" + stop.BillTo + "'" +
									" data-ship-to='" + stop.ShipTo + "'";

								if (initialRoutePlanId.toString() === stop.RoutePlanId.toString()) {
									optionItem = optionItem + " selected ";
								}

								optionItem = optionItem + ">" + stop.StopNumber + "</option>";
								$("#SourceStopNumberDDL").append(optionItem);
							}
							i++;
						}
						$("#SourceRouteName").text(data.RouteName);
						$("#SourceNumberOfStops").text(data.RouteStops.length);
						$("#SourceNumberOfStopsMinusOne").text(data.RouteStops.length - 1);
						$("#SourceStopNumberDDL").parent().next().find(".fa-spinner").addClass("hidden");
						if ($("#DestinationRouteNumberDDL").val() !== "" && $("#DestinationRouteNumberDDL").val() !== "0") {
							$(".ui-dialog-buttonpane :button:contains('Save')").prop("disabled", false).removeClass("ui-state-disabled");
						}

						//$("#SourceScheduledDeliveryDateTime").text(formatDate(new Date(stop.AdjustedDeliveryDateTime), "MM/dd/yyyy hh:mm tt"));
						$("#SourceStopNumberDDL").change();
						$("#SourceRouteNumberDDL").data('ready', "true");
					}
				}
				enableStopMoveSave();
			});
		} else {
			$("#SourceStopNumberDDL").empty();
			$("#SourceRouteNumberDDL").data('ready', "false");
			enableStopMoveSave();
		}
	});

	$("#dialog-window").on("change", "#SourceStopNumberDDL", function () {
		var $selectedSourceStop = $("#SourceStopNumberDDL option:selected");
		var sourceRoutePlanId = $(this).val();
		var sourceRouteId = $selectedSourceStop.data("routeId");
		var sourceStopNumber = $selectedSourceStop.text();
		var sourceRouteNumber = $selectedSourceStop.data("routeNumber");
		var sourceOrderId = $selectedSourceStop.data("orderId");

		if (sourceOrderId === null || sourceOrderId.toString() === "0" || sourceOrderId.toString() === "") {			
			alert("The selected SOURCE ROUTE & STOP NUMBER cannot be moved because it has no Orders associated with it.");
			$("#SourceStopNumberDDL").data('ready', "false");
		} else {
			var sourceRouteWeightTotal = $("#SourceTotalWeight").data("weight");
			var sourceRouteWeightFreezer = $("#SourceTotalWeightFreezer").data("weight");
			var sourceRouteWeightCooler = $("#SourceTotalWeightCooler").data("weight");
			var sourceRouteWeightDry = $("#SourceTotalWeightDry").data("weight");

			var sourceRouteCubesTotal = $("#SourceTotalCubes").data("cubes");
			var sourceRouteCubesFreezer = $("#SourceTotalCubesFreezer").data("cubes");
			var sourceRouteCubesCooler = $("#SourceTotalCubesCooler").data("cubes");
			var sourceRouteCubesDry = $("#SourceTotalCubesDry").data("cubes");

			var sourceRouteCasesTotal = $("#SourceTotalCases").data("cases");
			var sourceRouteCasesFreezer = $("#SourceTotalCasesFreezer").data("cases");
			var sourceRouteCasesCooler = $("#SourceTotalCasesCooler").data("cases");
			var sourceRouteCasesDry = $("#SourceTotalCasesDry").data("cases");

			var sourceStopPlannedDeliveryDatetime = $selectedSourceStop.data("plannedDeliveryDatetime");
			var sourceStopWeightTotal = $selectedSourceStop.data("weightTotal");
			var sourceStopWeightFreezer = $selectedSourceStop.data("weightFreezer");
			var sourceStopWeightCooler = $selectedSourceStop.data("weightCooler");
			var sourceStopWeightDry = $selectedSourceStop.data("weightDry");

			var sourceStopCubesTotal = $selectedSourceStop.data("cubesTotal");
			var sourceStopCubesFreezer = $selectedSourceStop.data("cubesFreezer");
			var sourceStopCubesCooler = $selectedSourceStop.data("cubesCooler");
			var sourceStopCubesDry = $selectedSourceStop.data("cubesDry");

			var sourceStopCasesTotal = $selectedSourceStop.data("casesTotal");
			var sourceStopCasesFreezer = $selectedSourceStop.data("casesFreezer");
			var sourceStopCasesCooler = $selectedSourceStop.data("casesCooler");
			var sourceStopCasesDry = $selectedSourceStop.data("casesDry");

			$(".stop-move-box-left").data("stopweighttotal", sourceStopWeightTotal);
			$(".stop-move-box-left").data("stopweightfreezer", sourceStopWeightFreezer);
			$(".stop-move-box-left").data("stopweightcooler", sourceStopWeightCooler);
			$(".stop-move-box-left").data("stopweightdry", sourceStopWeightDry);

			$(".stop-move-box-left").data("stopcubestotal", sourceStopCubesTotal);
			$(".stop-move-box-left").data("stopcubesfreezer", sourceStopCubesFreezer);
			$(".stop-move-box-left").data("stopcubescooler", sourceStopCubesCooler);
			$(".stop-move-box-left").data("stopcubesdry", sourceStopCubesDry);

			$(".stop-move-box-left").data("stopcasestotal", sourceStopCasesTotal);
			$(".stop-move-box-left").data("stopcasesfreezer", sourceStopCasesFreezer);
			$(".stop-move-box-left").data("stopcasescooler", sourceStopCasesCooler);
			$(".stop-move-box-left").data("stopcasesdry", sourceStopCasesDry);

			$('.source-route-plan-id').val(sourceRoutePlanId);
			$('.source-stop-number').val(sourceStopNumber);
			$('.source-stop-number').text(sourceStopNumber);
			$('.source-route-number').val(sourceRouteNumber);
			$('.source-route-number').text(sourceRouteNumber);
			$('.source-route-id').val(sourceRouteId);
			$('.source-stop-planned-delivery-datetime').val(sourceStopPlannedDeliveryDatetime);

			$('.source-stop-weight-total').text(formatNumber(sourceStopWeightTotal));
			$('.source-stop-weight-freezer').text(formatNumber(sourceStopWeightFreezer));
			$('.source-stop-weight-cooler').text(formatNumber(sourceStopWeightCooler));
			$('.source-stop-weight-dry').text(formatNumber(sourceStopWeightDry));

			$("#SourceTotalWeightUpdated").text(formatNumber(parseInt(sourceRouteWeightTotal) - parseInt(sourceStopWeightTotal)));
			$("#SourceTotalWeightFreezerUpdated").text(formatNumber(parseInt(sourceRouteWeightFreezer) - parseInt(sourceStopWeightFreezer)));
			$("#SourceTotalWeightCoolerUpdated").text(formatNumber(parseInt(sourceRouteWeightCooler) - parseInt(sourceStopWeightCooler)));
			$("#SourceTotalWeightDryUpdated").text(formatNumber(parseInt(sourceRouteWeightDry) - parseInt(sourceStopWeightDry)));

			$('.source-stop-cubes-total').text(formatNumber(sourceStopCubesTotal));
			$('.source-stop-cubes-freezer').text(formatNumber(sourceStopCubesFreezer));
			$('.source-stop-cubes-cooler').text(formatNumber(sourceStopCubesCooler));
			$('.source-stop-cubes-dry').text(formatNumber(sourceStopCubesDry));

			$("#SourceTotalCubesUpdated").text(formatNumber(parseInt(sourceRouteCubesTotal) - parseInt(sourceStopCubesTotal)));
			$("#SourceTotalCubesFreezerUpdated").text(formatNumber(parseInt(sourceRouteCubesFreezer) - parseInt(sourceStopCubesFreezer)));
			$("#SourceTotalCubesCoolerUpdated").text(formatNumber(parseInt(sourceRouteCubesCooler) - parseInt(sourceStopCubesCooler)));
			$("#SourceTotalCubesDryUpdated").text(formatNumber(parseInt(sourceRouteCubesDry) - parseInt(sourceStopCubesDry)));

			$('.source-stop-cases-total').text(formatNumber(sourceStopCasesTotal));
			$('.source-stop-cases-freezer').text(formatNumber(sourceStopCasesFreezer));
			$('.source-stop-cases-cooler').text(formatNumber(sourceStopCasesCooler));
			$('.source-stop-cases-dry').text(formatNumber(sourceStopCasesDry));

			$("#SourceTotalCasesUpdated").text(formatNumber(parseInt(sourceRouteCasesTotal) - parseInt(sourceStopCasesTotal)));
			$("#SourceTotalCasesFreezerUpdated").text(formatNumber(parseInt(sourceRouteCasesFreezer) - parseInt(sourceStopCasesFreezer)));
			$("#SourceTotalCasesCoolerUpdated").text(formatNumber(parseInt(sourceRouteCasesCooler) - parseInt(sourceStopCasesCooler)));
			$("#SourceTotalCasesDryUpdated").text(formatNumber(parseInt(sourceRouteCasesDry) - parseInt(sourceStopCasesDry)));

			$("#SourceScheduledDeliveryDateTime").text($selectedSourceStop.data("adjustedDeliveryDatetime"));
			$("#destinationDeliveryDateTime").val($selectedSourceStop.data("plannedDeliveryDatetime"));
			$("#SourceStopNumberDDL").data('ready', "true");
		}
		enableStopMoveSave();
	});

	$("#dialog-window").on("change", "#DestinationRouteNumberDDL", function () {
		var routeId = $(this).val();

		if (routeId !== "" && routeId !== "0") {
			var centerNumber = $("#centerNumber").val();
			var destinationRouteNumber = $("#DestinationRouteNumberDDL option:selected").text();
			var sourceStopPlannedDeliveryDateTime = $("#sourceStopPlannedDeliveryDateTime").val();
			if (sourceStopPlannedDeliveryDateTime === undefined || sourceStopPlannedDeliveryDateTime === null || sourceStopPlannedDeliveryDateTime === "") {
				sourceStopPlannedDeliveryDateTime = formatDate(new Date(), "MM/dd/yyyy hh:mm tt");
			}
			loadDestinationRoute(routeId, centerNumber, destinationRouteNumber, sourceStopPlannedDeliveryDateTime);
			$("#DestinationRouteNumberDDL").data('ready', "true");
		} else {
			$("#DestinationRouteNumberDDL").data('ready', "false");
		}
		enableStopMoveSave();
	});

	$("#dialog-window").on("change", "#destinationStopNumber", function () {
		enableStopMoveSave();
	});
};

var bindStopComment = function () {
	$("#managerAddCommentButton").prop('disabled', true);

	$("#managerAddCommentButton").on("click", function () {
		var centerNumber = $(".route-filter-center-selector").val();

		var url = "/routecomment/commoncreate?centerNumber=" + centerNumber.toString() + "&screen=RM";
		getData(url,
			function () {
				$("#dialog-window").dialog({
					title: "ACTIVITY LOG ",
					width: "550px",
					modal: true,
					buttons: [
						{
							text: "Close",
							click: function () {
								$(this).dialog("close");
							}
						},
						{
							text: "Save",
							id: "commonCreateCommentSaveButton",
							click: function () {
								// call save comment method here
								saveComment();
							}
						}
					]
				});
				$("#dialog-window").dialog("open");
				$("#dialog-window").html("<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading ... ");
				$("#commonCreateCommentSaveButton").attr("disabled", true).addClass("ui-state-disabled");
			}, // preCall
			function (data) {
				$("#dialog-window").html(data);
			}, // doneCallBack
			function (error) { $("#dialog-window").html(error); }, // failCallBack
			function () { }
		);
	});

	$(".stop-list").on("click", ".stop-comment-dialog-btn", function () {
		var shipTo = $(this).data("shipTo");
		var billTo = $(this).data("billTo");
		var centerNumber = $(this).data("centerNumber");
		var routePlanId = $(this).data("routePlanId");
		var stopNumber = $(this).data("stopNumber");

		loadCommentList(billTo, shipTo, centerNumber, routePlanId, stopNumber, "RM", true);
	});

	$(".stop-list").on("click", ".route-comment-readonly-dialog-trigger", function () {
		var routeId = $(this).data("routeId");
		var centerNumber = $(this).data("centerNumber");
		var routeNumber = $(this).data("routeNumber");

		var url = "/routecomment/ListByRouteId?routeId=" + routeId.toString() + "&centerNumber=" + centerNumber.toString();
		getData(url,
			function () {
				$("#dialog-window").dialog({
					title: "AGGREGATE ACTIVITY VIEWER FOR ROUTE " + routeNumber,
					width: "550px",
					modal: true,
					buttons: [
						{
							text: "OK",
							click: function () {
								$(this).dialog("close");
							}
						}
					]
				});
				$("#dialog-window").dialog("open");
				$("#dialog-window").html("<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading ... ");
			}, // preCall
			function (data) {
				$("#dialog-window").html(data);
				$("#dialog-window .comment-table tr:even:not(:first)").addClass("comment-table-stripe");
			}, // doneCallBack
			function (error) { $("#dialog-window").html(error); }, // failCallBack
			function () { }
		);
	});

};

var bindRouteHub = function () {
	// Declare a proxy to reference the hub.
	var notification = $.connection.routeHub;
	// Create a function that the hub can call to update status.
	notification.client.updateMoveStatus = function (routePlanId, status) {
		var iconClass = "";
		if (status == "1") { iconClass = "fa-pause"; }
		else if (status == "2") { iconClass = "fa-square-o"; }
		else if (status == "3") { iconClass = "fa-cog fa-spin"; }
		else if (status == "4") { iconClass = "fa-square"; }
		else if (status == "5") { iconClass = "fa-check-square"; }
		else if (status == "6") { iconClass = "fa-exclamation"; }
		else if (status == "7") { iconClass = "fa-ban"; }
		// change the status
		$(".move-status[data-route-plan-id='" + routePlanId + "'").removeClass().addClass("stop-move fa " + iconClass);
	};

	// Start the connection.
	$.connection.hub.start();
};

var enableStopMoveSave = function () {
	if ($("#SourceRouteNumberDDL").data('ready') === "true" && $("#SourceStopNumberDDL").data('ready') === "true" && $("#DestinationRouteNumberDDL").data('ready') === "true") {
		var destinationStopNumber = $("#destinationStopNumber").val();
		if (destinationStopNumber !== undefined && destinationStopNumber !== null && destinationStopNumber.toString().trim() !== "") {
			$(".ui-dialog-buttonpane :button:contains('Save')").prop("disabled", false).removeClass("ui-state-disabled");
		} else {
			$(".ui-dialog-buttonpane :button:contains('Save')").prop("disabled", true).addClass("ui-state-disabled");
		}
	}
};

// route data related

var loadDestinationRoute = function (destinationRouteId, centerNumber, destinationRouteNumber, sourceStopPlannedDeliveryDateTime) {
	var url = "/routemanager/destination/" + destinationRouteId + "?centerNumber=" + centerNumber + "&routeNumber=" + destinationRouteNumber + "&sourceStopPlannedDeliveryDateTime=" + sourceStopPlannedDeliveryDateTime;
	getData(url,
		function () { }, // preCall
		function (data) { loadDestinationRouteDoneCallBack(data); }, // doneCallBack
		function (error) { alert("loadDestinationRoute: " + error); }, // failCallBack
		function () { }
	);
};

var loadDestinationRouteDoneCallBack = function (data) {
	if (data && data !== null) {
		var route = JSON.parse(data);

		if (route.HasDispatched) {
			alert("The selected DESTINATION ROUTE cannot be moved because it has been dispatched.");
			$("#DestinationRouteNumberDDL").data('ready', "false");
		} else if (route.HasOrders === 1) {
			alert("The selected DESTINATION ROUTE cannot be moved because it has no Stops/Orders associated with it.");
			$("#DestinationRouteNumberDDL").data('ready', "false");
		} else {
			$("#destinationRouteNumber").val(route.RouteNumber);
			$("#destinationRouteId").val(route.RouteId);

			// destinationDtopNumber & destinationDeliveryDateTime are visible in UI, populated by user
			$("#DestinationRouteNumber").html(route.RouteNumber);
			$("#DestinationRouteName").html(route.RouteName);
			$("#DestinationNumberOfStops").html(route.NumberOfStops);
			$("#DestinationNumberOfStopsPlusOne").html(route.NumberOfStops + 1);

			var sourceWeightTotal = $(".stop-move-box-left").data("stopweighttotal");
			var sourceWeightFreezer = $(".stop-move-box-left").data("stopweightfreezer");
			var sourceWeightCooler = $(".stop-move-box-left").data("stopweightcooler");
			var sourceWeightDry = $(".stop-move-box-left").data("stopweightdry");

			$("#DestinationTotalWeight").html(Math.round(route.TotalWeight).toLocaleString());
			$("#DestinationTotalWeightUpdated").html(Math.round(route.TotalWeight + parseFloat(sourceWeightTotal)).toLocaleString());
			$("#DestinationTotalWeightFreezer").html(Math.round(route.TotalWeightFreezer).toLocaleString());
			$("#DestinationTotalWeightFreezerUpdated").html(Math.round(route.TotalWeightFreezer + parseFloat(sourceWeightFreezer)).toLocaleString());
			$("#DestinationTotalWeightCooler").html(Math.round(route.TotalWeightCooler).toLocaleString());
			$("#DestinationTotalWeightCoolerUpdated").html(Math.round(route.TotalWeightCooler + parseFloat(sourceWeightCooler)).toLocaleString());
			$("#DestinationTotalWeightDry").html(Math.round(route.TotalWeightDry).toLocaleString());
			$("#DestinationTotalWeightDryUpdated").html(Math.round(route.TotalWeightDry + parseFloat(sourceWeightDry)).toLocaleString());

			var sourceCubesTotal = $(".stop-move-box-left").data("stopcubestotal");
			var sourceCubesFreezer = $(".stop-move-box-left").data("stopcubesfreezer");
			var sourceCubesCooler = $(".stop-move-box-left").data("stopcubescooler");
			var sourceCubesDry = $(".stop-move-box-left").data("stopcubesdry");

			$("#DestinationTotalCubes").html(Math.round(route.TotalCubes).toLocaleString());
			$("#DestinationTotalCubesUpdated").html(Math.round(route.TotalCubes + parseFloat(sourceCubesTotal)).toLocaleString());
			$("#DestinationTotalCubesFreezer").html(Math.round(route.TotalCubesFreezer).toLocaleString());
			$("#DestinationTotalCubesFreezerUpdated").html(Math.round(route.TotalCubesFreezer + parseFloat(sourceCubesFreezer)).toLocaleString());
			$("#DestinationTotalCubesCooler").html(Math.round(route.TotalCubesCooler).toLocaleString());
			$("#DestinationTotalCubesCoolerUpdated").html(Math.round(route.TotalCubesCooler + parseFloat(sourceCubesCooler)).toLocaleString());
			$("#DestinationTotalCubesDry").html(Math.round(route.TotalCubesDry).toLocaleString());
			$("#DestinationTotalCubesDryUpdated").html(Math.round(route.TotalCubesDry + parseFloat(sourceCubesDry)).toLocaleString());

			var sourceCasesTotal = $(".stop-move-box-left").data("stopcasestotal");
			var sourceCasesFreezer = $(".stop-move-box-left").data("stopcasesfreezer");
			var sourceCasesCooler = $(".stop-move-box-left").data("stopcasescooler");
			var sourceCasesDry = $(".stop-move-box-left").data("stopcasesdry");

			$("#DestinationTotalCases").html(Math.round(route.TotalCases).toLocaleString());
			$("#DestinationTotalCasesUpdated").html(Math.round(route.TotalCases + parseFloat(sourceCasesTotal)).toLocaleString());
			$("#DestinationTotalCasesFreezer").html(Math.round(route.TotalCasesFreezer).toLocaleString());
			$("#DestinationTotalCasesFreezerUpdated").html(Math.round(route.TotalCasesFreezer + parseFloat(sourceCasesFreezer)).toLocaleString());
			$("#DestinationTotalCasesCooler").html(Math.round(route.TotalCasesCooler).toLocaleString());
			$("#DestinationTotalCasesCoolerUpdated").html(Math.round(route.TotalCasesCooler + parseFloat(sourceCasesCooler)).toLocaleString());
			$("#DestinationTotalCasesDry").html(Math.round(route.TotalCasesDry).toLocaleString());
			$("#DestinationTotalCasesDryUpdated").html(Math.round(route.TotalCasesDry + parseFloat(sourceCasesDry)).toLocaleString());
			$("#DestinationRouteNumberDDL").data('ready', "true");
		}
		enableStopMoveSave();
	}
};

var loadRoutes = function (position, center, startDate, endDate, sortField, sortDirection, nearByRouteId, nearByRoutePlanId, billTo, shipTo) {
	var url = "";
	// if there is no center, return empty map
	if (center === '') {
		$(".route-list[data-position='" + position + "']").html("");
		loadMap();
		return;
	}

	// assuming there is a center, add it to the marker collection to be drawn in the map
	if ($(".center-block").data("centernumber") != center || routeMapArray.length === 0) {
		addCenterToMap(center);
	}

	// get route list and display in the route-list
	url = "/routemanager/list?position=" + position + "&CenterNumber=" + center + "&FilterStartDate=" + startDate + "&FilterEndDate=" + endDate + "&SortField=" + sortField + "&SortDirection=" + sortDirection + "&NearRouteId=" + nearByRouteId + "&NearRoutePlanId=" + nearByRoutePlanId + "&billTo=" + billTo + "&shipTo=" + shipTo;
	getData(url,
		function () { $(".route-list[data-position='" + position + "']").html("<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading routes for center " + center); }, // preCall
		function (data) { loadRoutesDoneCallBack(data, position); }, // doneCallBack
		function (error) { $(".route-list[data-position='" + position + "']").html(error); }, // failCallBack
		function () { }
	);
};

var loadRoutesDoneCallBack = function (data, position) {
	$(".route-list[data-position='" + position + "']").html(data);
	bindRouteHeader(position);
	filterRoutes(position);

	// if this is called as a result of a stop move - pre-select route and pre-display stop list
	if (position === "left" && $("#dialog-window").html().indexOf("move-stop") > -1) {
		var arr = $("#dialog-window").html().split('|');
		var nearByRouteId = arr[1];
		// display in map
		$(".route-header[data-position='left'][data-routeid='" + nearByRouteId + "']").find(":checkbox").click();
		// open left stop list
		$(".route-header[data-position='left'] .stop-list-toggle[data-routeid='" + nearByRouteId + "']").click();
	}
};

var loadRouteStops = function (routeId, centerNumber, routeNumber, position) {
	var url = "/routemanager/stops/" + routeId + "?position=" + position + "&centerNumber=" + centerNumber + "&routeNumber=" + routeNumber;
	getData(url,
		function () { $(".stop-list[data-position='" + position + "']").html("<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading stops for route"); }, // preCall
		function (data) { loadRouteStopsDoneCallBack(data, position); }, // doneCallBack
		function (error) { alert("loadRouteStops: " + error); }, // failCallBack
		function () { }
	);
};

var loadRouteStopsDoneCallBack = function (data, position) {
	$(".stop-list[data-position='" + position + "']").html(data);

	// if this is called as a result of a stop move - pre-select proximity route-stop
	if (position === "left" && $("#dialog-window").html().indexOf("move-stop") > -1) {
		var arr = $("#dialog-window").html().split('|');
		var nearByRoutePlanId = arr[2];
		// click stop based proximity 
		$(".stop-header[data-position='left'] .primary-stop-toggle[data-routeplanid='" + nearByRoutePlanId + "']").click();

		// clear flag
		$("#dialog-window").html("");
	}
};

var loadStopMove = function (centerNumber, leftRouteId, leftRoutePlanId, leftDeliveryDateTime, rightRouteId, rightRouteNumber) {
	var url = "/routemanager/move/" + leftRouteId + "?routePlanId=" + leftRoutePlanId + "&centerNumber=" + centerNumber + "&destinationRouteId=" + (rightRouteId === "" ? 0 : rightRouteId);
	getData(url,
		function () { }, // preCall
		function (data) {
			loadStopMoveDoneCallBack(data, centerNumber, rightRouteId, rightRouteNumber, leftDeliveryDateTime);
			if (leftRoutePlanId === "" || leftRoutePlanId === 0 || leftRoutePlanId === "0" || ((rightRouteId === 0 || rightRouteId === "") && rightRouteNumber === "")) {
				$(".ui-dialog-buttonpane :button:contains('Save')").prop("disabled", true).addClass("ui-state-disabled");
			}
		}, // doneCallBack
		function (error) { alert("loadStopMove: " + error); }, // failCallBack
		function () { }
	);
};

var loadStopMoveDoneCallBack = function (data, centerNumber, rightRouteId, rightRouteNumber, leftDeliveryDateTime) {
	$("#dialog-window").html(data);
	$("#dialog-window").dialog({
		modal: true,
		width: 900,
		minHeight: 475,
		resizable: false,
		title: "STOP MOVE",
		buttons: [
			{
				text: "Cancel",
				click: function () {
					$(this).dialog("close");
				}
			},
			{
				text: "Save",
				click: function () {
					// call save comment method here
					saveStopMovements();
				}
			}
		]
	});
	$("#SourceRouteNumberDDL").change();
	$(".stop-context-menu").hide();

	if ((rightRouteId !== "" && rightRouteId !== 0) || rightRouteNumber !== "") {
		loadDestinationRoute(rightRouteId === "" ? 0 : rightRouteId, centerNumber, rightRouteNumber, leftDeliveryDateTime);
	}
};

var saveStopMovements = function () {
	var sourceRouteNumber = $("#sourceRouteNumber").val();
	var sourceStopNumber = $("#sourceStopNumber").val();
	var sourceRoutePlanId = $("#sourceRoutePlanId").val();
	var sourceRouteId = $("#sourceRouteId").val();
	var destinationRouteId = $("#destinationRouteId").val();
	var destinationRouteNumber = $("#destinationRouteNumber").val();
	var destinationStopNumber = $("#destinationStopNumber").val();
	var destinationDeliveryDateTime = $("#destinationDeliveryDateTime").val();
	var stopModificationComment = $("#stopModificationComment").val();
	var center = $(".route-filter-center-selector").val();

	var errorMessage = "";
	if (destinationRouteNumber === undefined || destinationRouteNumber === null || destinationRouteNumber.toString().trim() === "") {
		errorMessage = errorMessage + "New Route Number is required.\n";
	}
	if (destinationStopNumber === undefined || destinationStopNumber === null || destinationStopNumber.toString().trim() === "") {
		errorMessage = errorMessage + "Valid New Stop Number is required.\n";
	} else {
		var onlyDigitReg = /^\d+$/;

		if (!onlyDigitReg.test(destinationStopNumber)) {
			errorMessage = errorMessage + "Valid New Stop Number is required.\n";
		}
	}
	if (destinationDeliveryDateTime === undefined || destinationDeliveryDateTime === null || destinationDeliveryDateTime.toString().trim() === "") {
		errorMessage = errorMessage + "New Delivery Date/Time is required.\n";
	} else {
		var re = /[0-1]?\d\/[0-3]?\d\/\d{4} [0-1]\d:[0-5]\d [aApP][mM]/;

		if (!re.test(destinationDeliveryDateTime) || isNaN(Date.parse(destinationDeliveryDateTime))) {
			errorMessage = errorMessage + "New Delivery Date/Time is invalid.\n";
		}
	}
	if (stopModificationComment === undefined || stopModificationComment === null || stopModificationComment.toString().trim() === "") {
		errorMessage = errorMessage + "Comment is required.\n";
	}

	if (errorMessage !== "") { alert(errorMessage); return; }
	else {
		var url = "/routemanager/move";
		postData(url,
			{
				sourceRouteNumber: sourceRouteNumber, sourceStopNumber: sourceStopNumber, sourceRoutePlanId: sourceRoutePlanId, sourceRouteId: sourceRouteId,
				destinationRouteNumber: destinationRouteNumber, destinationStopNumber: destinationStopNumber, destinationRouteId: destinationRouteId, destinationDeliveryDateTime: destinationDeliveryDateTime,
				stopModificationComment: stopModificationComment, centerNumber: center
			},
			function () { }, // preCall
			function (data) { saveStopMovementsDoneCallBack(sourceRouteId, sourceRoutePlanId); }, // doneCallBack
			function (error) {
				if (error.status === 404) {
					alert("NEW ROUTE NUMBER is not valid for this Center. Please enter a valid Route Number.");
				}
				if (error.status === 409) {
					alert("NEW STOP NUMBER is already used. Please enter a different Stop Number.");
				}
			}, // failCallBack
			function () { }
		);
	}
};

var saveStopMovementsDoneCallBack = function (sourceRouteId, sourceRoutePlanId) {
	$("#dialog-window").dialog("close");
	$("#dialog-window").html("move-stop|" + sourceRouteId.toString() + "|" + sourceRoutePlanId.toString());

	// close stop list on both sides
	closeStopListSlider("left");
	closeStopListSlider("right");

	$("#toast-window").html("<span> ... Stop move is successful ... </span>");

	// show toast
	$("#toast-window").addClass("toast-is-shown");

	// reload route on the right
	$(".route-filter-timing-type-selector[data-position='right']").change();

	// reload route on the left
	$(".route-filter-timing-type-selector[data-position='left']").change();

	// hide toast
	setTimeout(function () {
		$("#toast-window").removeClass("toast-is-shown");
	}, 3000);
};

var loadProximityRoutes = function (proximitySearchFromRouteId, proximitySearchFromRoutePlanId) {
	var position = "left";
	var rightPosition = "right";

	// remove all routes on the right
	clearRoutes(rightPosition);

	// load nearby routes in the right side: search routes based on same center, same start/end date
	var center = $(".route-filter-center-selector").val();
	var startDate = $(".route-filter-dispatch-start-date[data-position='" + position + "']").val();
	var endDate = $(".route-filter-dispatch-end-date[data-position='" + position + "']").val();

	// set search date params: timing selection, weekending, start & end date
	$(".route-filter-timing-type-selector[data-position='" + rightPosition + "']").val($(".route-filter-timing-type-selector[data-position='" + position + "']").val());
	$(".route-filter-weekending-date[data-position='" + rightPosition + "']").val($(".route-filter-weekending-date[data-position='" + position + "']").val());
	$(".route-filter-dispatch-start-date[data-position='" + rightPosition + "']").val(startDate);
	$(".route-filter-dispatch-end-date[data-position='" + rightPosition + "']").val(endDate);
	if ($(".route-filter-timing-type-selector[data-position='" + rightPosition + "']").val() === "1") {
		$("#RouteWeekendingDate" + rightPosition).parent().show();
		$("#RouteDispatchStartDate" + rightPosition).parent().hide();
	} else {
		$("#RouteWeekendingDate" + rightPosition).parent().hide();
		$("#RouteDispatchStartDate" + rightPosition).parent().show();
	}
	// set sort param to proximity asc
	$(".route-sorter[data-position='" + rightPosition + "']").val('9');
	$(".route-sorter-direction[data-position='" + rightPosition + "']").html("ASC");
	// set filter params: name/number, cubes and weight
	$(".route-filter-name[data-position='" + rightPosition + "']").val("");
	$(".checkbox :checkbox[data-position='" + rightPosition + "'][data-filtercategory='weight']").removeAttr('checked');
	$(".checkbox :checkbox[data-position='" + rightPosition + "'][data-filtercategory='cubes']").removeAttr('checked');

	// clean filter or maybe preset filter 
	var sortField = "9"; // sortField = 9 -> proximity
	loadRoutes(rightPosition, center, startDate, endDate, sortField, "1", proximitySearchFromRouteId, proximitySearchFromRoutePlanId);

	// open right route
	openRoute(rightPosition);
};


// map related

function initMap() {
	routeMapArray = [];
	directionsService = new google.maps.DirectionsService();
	loadMap(new google.maps.LatLng(40.02, -83.02));

	// if there is only 1 center, auto-select it (default)
	if ($(".route-filter-center-selector").children('option').length === 2) {
		$(".route-filter-center-selector").prop('selectedIndex', 1);
		$(".route-filter-center-selector").change();
	}
}

var loadMap = function (center) {
	var mapOptions = {
		zoom: 11,
		mapTypeControl: false,
		streetViewControl: false,
		mapTypeId: google.maps.MapTypeId.ROADMAP
	};

	if (center !== undefined && center !== null) {
		mapOptions.center = center;
	}

	// Some basic map setup (from the API docs)
	map = new google.maps.Map(document.getElementById('map'), mapOptions);

	// Start the request making
	$(".clear-route-button").hide();
	var requestArray = generateRequests();
	processRequests(requestArray);
};

var removeRouteFromMap = function (routeId, position, clearRight) {
	var newRoutes = routeMapArray.filter(function (rData, rIndex) {
		return rData.routeId.toString() !== routeId.toString();
	});
	routeMapArray = newRoutes;

	// remove all routes on the right
	if (position === "left" && clearRight) {
		clearRightMap();
	}

	loadMap();
};

var addRouteToMap = function (routeId, centerNumber, routeNumber, position) {
	var url = "/routemanager/details/" + routeId + "?centerNumber=" + centerNumber + "&routeNumber=" + routeNumber;
	getData(url,
		function () { $(".global-spinner-layer").show(); }, // preCall
		function (data) {
			var routeData = $.parseJSON(data);
			routeData.position = position;
			routeMapArray.push(routeData);

			loadMap();
		}, // doneCallBack
		function (error) { alert("addRouteToMap: " + error); }, // failCallBack
		function () { $(".global-spinner-layer").hide(); }
	);
};

var addCenterToMap = function (center) {
	var url = "/routemanager/center?sygmaCenterNo=" + center;
	getData(url,
		function () { }, // preCall
		function (data) {
			var centerLocationRaw = $.parseJSON(data);
			if (routeMapArray.length === 0 || routeMapArray.length > 0 && routeMapArray[0].routeId !== centerLocationRaw.SygmaCenterNo) {
				var centerLocation = {};
				centerLocation.position = "";
				centerLocation.number = centerLocationRaw.Description;
				centerLocation.routeId = centerLocationRaw.SygmaCenterNo;
				centerLocation.stops = [];
				centerLocation.stops.push({ "lat": centerLocationRaw.Latitude, "lng": centerLocationRaw.Longitude, "address": "" });
				centerLatitude = centerLocationRaw.Latitude.toString();
				centerLongitude = centerLocationRaw.Longitude.toString();
				routeMapArray.push(centerLocation);

				loadMap(new google.maps.LatLng(centerLocationRaw.Latitude, centerLocationRaw.Longitude));
			}

			if (parseInt(centerLocationRaw.EnableStopMove) > 0) {
				$("#managerMoveStopButton").prop('disabled', false);
			} else {
				$("#managerMoveStopButton").prop('disabled', true);
			}
		}, // doneCallBack
		function (error) { alert("loadRoutes: " + error); }, // failCallBack
		function () { }
	);
};

var generateRequests = function () {
	var requestArray = [];

	for (var route in routeMapArray) {
		// Somewhere to store the wayoints
		var waypts = [];

		// lastpoint is used to ensure that duplicate waypoints are stripped
		var lastpoint = {};
		lastpoint.address = "";

		var data = routeMapArray[route].stops;

		// add center as first location
		if (route !== 0 && (routeMapArray[route].stops.length === 0 || routeMapArray[route].stops[0].stop !== undefined))
			data.unshift({ address: (centerLatitude.toString() + ',' + centerLongitude.toString()).replace(/ /g, '+') });

		for (var waypoint = 0; waypoint < data.length; waypoint++) {
			var locToUseForMap = "";
			if (data[waypoint].lat && data[waypoint].lng) {
				locToUseForMap = data[waypoint].lat.toString() + ',' + data[waypoint].lng.toString();
				if (data[waypoint].address.trim() === "") {
					data[waypoint].address = data[waypoint].lat.toString() + ',' + data[waypoint].lng.toString();
				}
			} else {
				if (data[waypoint].address.trim() !== "") {
					locToUseForMap = data[waypoint].address.trim();
				}
			}

			// Duplicate of of the last waypoint - don't bother

			if (data[waypoint].address === lastpoint.address) {
				if (data[waypoint].lat === lastpoint.lat && data[waypoint].lng === lastpoint.lng) { continue; }
			}

			// Prepare the lastpoint for the next loop
			lastpoint = data[waypoint];

			// Add this to waypoint to the array for making the request
			//waypts.push({ location: data[waypoint].address.replace(/ /g, '+'), stopover: true });
			waypts.push({ location: locToUseForMap, stopover: true });
		}

		// 'start' and 'finish' will be the routes origin and destination
		// Grab the first waypoint for the 'start' location
		var start = (waypts.shift()).location;
		// Grab the last waypoint for use as a 'finish' location
		var finish = waypts.pop();
		// Unless there was no finish location for some reason?
		if (finish === undefined) {
			finish = start;
		} else {
			finish = finish.location;
		}

		// Let's create the Google Maps request object
		var request = {
			origin: start,
			destination: finish,
			waypoints: waypts,
			travelMode: google.maps.TravelMode.DRIVING
		};

		// and save it in our requestArray
		requestArray.push({ "route": route, "request": request, "position": routeMapArray[route].position });
	}
	return requestArray;
};

var processRequests = function (requestArray) {
	var bounds = new google.maps.LatLngBounds();
	var infoWindow = new google.maps.InfoWindow;

	// Counter to track request submission and process one at a time;
	var i = 0; var iLeft = 0; var iRight = 0; var position = "";

	// custom markers related
	var overlayMarker;
	CustomMarker.prototype = new google.maps.OverlayView();

	// Used to submit the request 'i'
	var submitRequest = function () {
		if (requestArray.length === 0) return;
		position = requestArray[i].position;
		directionsService.route(requestArray[i].request, processDirectionResults);
	};

	function CustomMarker(opts) {
		this.setValues(opts);
	}

	CustomMarker.prototype.draw = function () {
		var self = this;
		var div = this.div;
		if (!div) {
			div = this.div = $('' +
				'<div>' +
				'<div class="shadow"></div>' +
				'<div class="pulse"></div>' +
				'<div class="pin-wrap">' +
				'<div class="pin" style="background-color: ' + this.color + ';"><div class="pin-number">' + this.stopNumber + '</span></div>' +
				'</div>' +
				'</div>' +
				'')[0];
			this.pinWrap = this.div.getElementsByClassName('pin-wrap');
			this.pin = this.div.getElementsByClassName('pin');
			this.pinShadow = this.div.getElementsByClassName('shadow');
			div.style.position = 'absolute';
			div.style.cursor = 'pointer';

			google.maps.event.addDomListener(this.pin[0], "click", function (event) {
				self.infoWindow.setContent(self.info);
				self.infoWindow.setPosition(self.position);
				self.infoWindow.open(self.map);
				google.maps.event.trigger(self, "click");
			});

			var panes = this.getPanes();
			panes.overlayMouseTarget.appendChild(div);
		}
		var point = this.getProjection().fromLatLngToDivPixel(this.position);
		if (point) {
			div.style.left = point.x + 'px';
			div.style.top = point.y + 'px';
		}
	};

	// Used as callback for the above request for current 'i'
	function processDirectionResults(result, status) {
		if (status === google.maps.DirectionsStatus.OK) {
			// Create a unique DirectionsRenderer 'i'
			directionDisplayRenderEngineArray[i] = new google.maps.DirectionsRenderer();
			directionDisplayRenderEngineArray[i].setMap(map);

			//if (result.geocoded_waypoints.length <= 2) {
			if (result.routes[0].legs.length === 1 && result.routes[0].legs[0].start_address === result.routes[0].legs[0].end_address) {
				iconColor = 'black';

				// Some unique options from the colorArray so we can see the center
				directionDisplayRenderEngineArray[i].setOptions({
					preserveViewport: true,
					polylineOptions: { strokeWeight: 1, strokeOpacity: 0.8, strokeColor: iconColor },
					markerOptions: { icon: { path: google.maps.SymbolPath.CIRCLE, scale: 8, strokeColor: 'black' } }
				});

				// Use this new renderer with the result
				directionDisplayRenderEngineArray[i].setDirections(result);
				bounds = result.routes[0].bounds;
			} else {

				if (i === 1) {
					bounds = new google.maps.LatLngBounds();
				}
				iconColor = position === "left" ? colorArrayLeft[(iLeft + 1) % 10] : (position === "right" ? colorArrayRight[(iRight + 1) % 10] : "#000000");

				directionDisplayRenderEngineArray[i].setOptions({
					preserveViewport: true,
					polylineOptions: { strokeWeight: 5, strokeOpacity: 0.8, strokeColor: iconColor },
					markerOptions: {},
					suppressMarkers: true
				});

				// Use this new renderer with the result
				directionDisplayRenderEngineArray[i].setDirections(result);

				if (routeMapArray.length > i) {
					// show waypoints with custom markers & info window
					showSteps(result, infoWindow, map, iconColor, i);
				}
			}

			map.fitBounds(bounds);
			if (map.zoom > defaultZoom) { map.setZoom(defaultZoom); }

			// and start the next request
			setTimeout(function () {
				if (position === "left") {
					iLeft++;
				} else if (position === "right") {
					iRight++;
				}
				nextRequest();
			}, 100);
		}
	}

	function showSteps(directionResult, infoWindow, map, iconColor, index) {
		var route = directionResult.routes[0];
		var routeInfo = routeMapArray[index];
		var markerArray = [];

		for (var t = 1; t < route.legs.length; t++) {
			var stopLabel = routeInfo.stops[t].stop;
			var contentString = '<div class="info-window">' +
				'<div class="info-window-content">' +
				'<h4>' + routeInfo.stops[t].label + ' (' + routeInfo.stops[t].billTo + '-' + routeInfo.stops[t].shipTo + ')</h4>' +
				'<p>Route ' + routeInfo.stops[t].id + ', Stop ' + routeInfo.stops[t].stop + '<br/>' +
				routeInfo.stops[t].address + '<br/>' +
				'Weight: ' + routeInfo.stops[t].weight + ' (F: ' + routeInfo.stops[t].weightFreezer + ', C: ' + routeInfo.stops[t].weightCooler + ', D: ' + routeInfo.stops[t].weightDry + ')<br/>' +
				'Cubes: ' + routeInfo.stops[t].cubes + ' (F: ' + routeInfo.stops[t].cubesFreezer + ', C: ' + routeInfo.stops[t].cubesCooler + ', D: ' + routeInfo.stops[t].cubesDry + ')<br/>' +
				'Cases: ' + routeInfo.stops[t].cases + ' (F: ' + routeInfo.stops[t].casesFreezer + ', C: ' + routeInfo.stops[t].casesCooler + ', D: ' + routeInfo.stops[t].casesDry + ')<br/>' +
				formatDate(routeInfo.stops[t].adjustedDeliveryDateTime, "MM/dd/yyyy hh:mm tt") + '</p>' +
				'</div></div>';

			var marker = new CustomMarker({
				position: route.legs[t].start_location,
				map: map,
				title: routeInfo.stops[t].label,
				stopNumber: stopLabel,
				color: iconColor,
				info: contentString,
				infoWindow: infoWindow
			});
			bounds.extend(new google.maps.LatLng(marker.position.lat(), marker.position.lng()));
		}

		// for last point
		var endStopLabel = routeInfo.stops[t].stop;
		var endContentString = '<div class="info-window">' +
			'<div class="info-window-content">' +
			'<h4>' + routeInfo.stops[t].label + ' (' + routeInfo.stops[t].billTo + '-' + routeInfo.stops[t].shipTo + ')</h4>' +
			'<p>Route ' + routeInfo.stops[t].id + ', Stop ' + routeInfo.stops[t].stop + '<br/>' +
			routeInfo.stops[t].address + '<br/>' +
			'Weight: ' + routeInfo.stops[t].weight + ' (F: ' + routeInfo.stops[t].weightFreezer + ', C: ' + routeInfo.stops[t].weightCooler + ', D: ' + routeInfo.stops[t].weightDry + ')<br/>' +
			'Cubes: ' + routeInfo.stops[t].cubes + ' (F: ' + routeInfo.stops[t].cubesFreezer + ', C: ' + routeInfo.stops[t].cubesCooler + ', D: ' + routeInfo.stops[t].cubesDry + ')<br/>' +
			'Cases: ' + routeInfo.stops[t].cases + ' (F: ' + routeInfo.stops[t].casesFreezer + ', C: ' + routeInfo.stops[t].casesCooler + ', D: ' + routeInfo.stops[t].casesDry + ')<br/>' +
			formatDate(routeInfo.stops[t].adjustedDeliveryDateTime, "MM/dd/yyyy hh:mm tt") + '</p>' +
			'</div></div>';

		var endMarker = new CustomMarker({
			position: route.legs[t - 1].end_location,
			map: map,
			title: routeInfo.stops[t].label,
			stopNumber: endStopLabel,
			color: iconColor,
			info: endContentString,
			infoWindow: infoWindow
		});
		bounds.extend(new google.maps.LatLng(endMarker.position.lat(), endMarker.position.lng()));
	}

	function nextRequest() {
		// Increase the counter
		i++;

		// Make sure we are still waiting for a request
		if (i >= requestArray.length) {
			// No more to do
			$(".clear-route-button").show();
			return;
		}

		// Submit another request
		submitRequest();
	}

	// This request is just to kick start the whole process
	submitRequest();
};

var clearRightMap = function () {
	clearMap("right");
};

var clearMap = function (position) {
	var routes = routeMapArray.filter(function (rData, rIndex) {
		return rData.position !== position;
	});
	routeMapArray = routes;

	if (position === "left") clearRightMap();
};


// utility functions

var getRandomColor = function () {
	var letters = '0123456789ABCDEF';
	var color = '#';
	for (var i = 0; i < 6; i++) {
		color += letters[Math.floor(Math.random() * 16)];
	}
	return color;
};

var hexColor = function (rgb) {
	rgb = String(rgb).match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);
	return (rgb && rgb.length === 4) ? ("#" +
		("0" + parseInt(rgb[1], 10).toString(16)).slice(-2) +
		("0" + parseInt(rgb[2], 10).toString(16)).slice(-2) +
		("0" + parseInt(rgb[3], 10).toString(16)).slice(-2)) : '';
};
