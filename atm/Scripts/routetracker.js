

// ----- route related functions

var loadRoutes = function () {
	center = $("#selectCenterDropdown").val();
	if (center === '') {
		//Hide the search and filter fields unless a center is selected
		clearTotalLabel();
		$("#routeList").html("");
		$("#RouteTrackerFilterSearch").addClass("hidden");
		$("#EarlyLateSelectionDiv").addClass("hidden");
		$("#ShowConceptSelectionDiv").addClass("hidden");
		$("#RouteTrackerFilterBillTo").addClass("hidden");
		$("#RouteTrackerFilterShipTo").addClass("hidden");
		$("#RouteTrackerFilterButton").addClass("hidden");
		$("#RouteOrderSelectionDiv").addClass("hidden");
		return;
	}
	//Retrieve the Start Date and End Date fields
	var startDateString = $("#startDateInput").val();
	var endDateString = $("#endDateInput").val();
	//Save the values of these fields as a Date object. Convert to local time and then make it at noon.
	var filterStartDate = convertUTCDateToLocalDate(new Date(startDateString));
	filterStartDate.setHours(12);
	var filterEndDate = convertUTCDateToLocalDate(new Date(endDateString));
	filterEndDate.setHours(12);

	//If either Start Date or End Date wasn't a valid date
	if (!isDate(filterStartDate) || !isDate(filterEndDate)) {
		//Hide all routes and return
		$("#routeList").html("");
		return;
	}

	var billTo = $("#routeStopBillTo").val();
	var shipTo = $("#routeStopShipTo").val();

	$("#routeList").html("<i class='fa fa-spinner fa-5x fa-spin loading' aria-hidden='true'></i> &nbsp;&nbsp;loading routes for center " + center);

	$.ajax({
		async: true,
		url: "/routetracker/list?centerNumber=" + center + "&filterStartDate=" + filterStartDate.toISOString() + "&filterEndDate=" + filterEndDate.toISOString() + "&billTo=" + billTo + "&shipTo=" + shipTo,
		type: "GET",
		success: function (data) {
			$("#routeList").html(data);
			//Showing the Route Search & Modified Routes divs in the top bar
			$("#RouteTrackerFilterSearch").removeClass("hidden");
			$("#EarlyLateSelectionDiv").removeClass("hidden");
			$("#ShowConceptSelectionDiv").removeClass("hidden");
			$("#RouteTrackerFilterBillTo").removeClass("hidden");
			$("#RouteTrackerFilterShipTo").removeClass("hidden");
			$("#RouteTrackerFilterButton").removeClass("hidden");
			$("#RouteOrderSelectionDiv").removeClass("hidden");
			filterRoutes();
			updateTotalLabel();
			//Apply the custom columns selected to the Routes table
			//filterRouteTableColumns();
		},
		error: function (error) {
			$("#routeList").html(error);
		}
	});
};

var loadStopsForRoute = function (id) {
	$(".stop-list[data-route-id='" + id + "']").closest("tr").toggleClass("hidden");

	if ($(".stop-list[data-route-id='" + id + "']").closest("tr").hasClass("hidden")) return;

	var routeUI = $(".stop-list[data-route-id='" + id + "']");
	routeUI.html("<i class='fa fa-spinner fa-spin fa-5x loading' aria-hidden='true'></i> &nbsp;&nbsp;loading");

	$.ajax({
		async: true,
		url: "/routetracker/stops?routeId=" + id,
		type: "GET",
		success: function (data) {
			routeUI.html(data);

			//$('.scheduled-date').change();
			//$('.adjusted-date').change();

			$(".stop-list[data-route-id='" + id + "'] .time-boxes-picker").timepicker({
				timeFormat: 'hh:mm p',
				interval: 15,
				minTime: '12:00 AM',
				maxTime: '11:45 PM',
				dynamic: true,
				dropdown: true,
				scrollbar: false,
				change: function (time) {
					if (this[0].id.toUpperCase().indexOf("ADJUSTED") > -1)
						changeScheduledAdjustedDateTime(this, "adjusted");
					else
						changeScheduledAdjustedDateTime(this, "scheduled");
				}
			});
		},

		error: function (error) {
			routeUI.html(error);
		}
	});
};

var bindStopEvents = function () {
	$("#routeList").on("blur", ".stop-list .adjusted-minute-spinner", function (e) {
		//if the value is empty, reset it
		if ($(this).val() === "") {
			$(this).val("0.0");
			$(this).change();
		}
	});

	$("#routeList").on("change", ".stop-list .adjusted-minute-spinner", function (e) {
		if ($(this).val().trim() === "") {
			$(this).val("0.0");
		}

		var $grandParent = $(this).parent().parent();
		var adjustedDate = $grandParent.find('.adjusted-date');
		var adjustedTime = $grandParent.find('.adjusted-time');
		var scheduledDate = $grandParent.find('.scheduled-date');
		var scheduledTime = $grandParent.find('.scheduled-time');

		var dt = new Date(scheduledDate.val() + " " + scheduledTime.val());
		dt.setMinutes(dt.getMinutes() + (parseFloat($(this).val()) * 60));

		// set adjusted date to be scheduled + offset
		adjustedDate.val(formatDate(dt, "yyyy-MM-dd"));
		adjustedTime.val(formatDate(dt, "hh:mm tt"));
		adjustedDate.change();

		// mark field as editted
		adjustedDate.removeClass('route-stop-time-adjusted');
		adjustedTime.removeClass('route-stop-time-adjusted');

		if (parseFloat($(this).val()) !== 0) {
			$grandParent.find('.route-plan-modification-id').val(3);
			adjustedDate.addClass('route-stop-time-adjusted');
			adjustedTime.addClass('route-stop-time-adjusted');
		}

		if (adjustedDate[0].defaultValue !== adjustedDate[0].value || adjustedTime[0].defaultValue !== adjustedTime[0].value) {
			adjustedDate.addClass('route-stop-time-dirty');
			adjustedTime.addClass('route-stop-time-dirty');
			adjustedDate.closest("form").find('.operation-message').html('');
		} else {
			if (adjustedDate.hasClass('route-stop-time-dirty')) {
				adjustedDate.removeClass('route-stop-time-dirty');
				adjustedTime.removeClass('route-stop-time-dirty');
			}
		}

		$grandParent.find('.adjusted-date-time').val(formatDate(adjustedDate.val() + " " + adjustedTime.val(), "MM/dd/yy hh:mm tt"));
		$grandParent.find('.scheduled-date-time').val(formatDate(scheduledDate.val() + " " + scheduledTime.val(), "MM/dd/yy hh:mm tt"));
	});


	$("#routeList").on("blur", ".stop-list .adjusted-date", function (e) {
		//if the entered date isn't a valid date format, then reset it
		var adjustedDateTime = $(this).parent().find(".adjusted-date").val() + " " + $(this).parent().find(".adjusted-time").val();
		if (!isValidFormat(adjustedDateTime, "yyyy-MM-dd hh:mm tt")) {
			var minuteSpinner = $(this).parent().parent().find('.adjusted-minute-spinner');
			minuteSpinner.val("0.0");
			minuteSpinner.change();
		}
	});

	$("#routeList").on("blur", ".stop-list .adjusted-time", function (e) {
		//if the entered date isn't a valid date format, then reset it
		var adjustedDateTime = $(this).parent().find(".adjusted-date").val() + " " + $(this).parent().find(".adjusted-time").val();
		if (!isValidFormat(adjustedDateTime, "yyyy-MM-dd hh:mm tt")) {
			var minuteSpinner = $(this).parent().parent().find('.adjusted-minute-spinner');
			minuteSpinner.val("0.0");
			minuteSpinner.change();
		}
	});

	$("#routeList").on("change", ".stop-list .adjusted-date", function (e) {
		changeScheduledAdjustedDateTime(this, "adjusted");
	});

	$("#routeList").on("change", ".stop-list .adjusted-time", function (e) {
		changeScheduledAdjustedDateTime(this, "adjusted");
	});


	$("#routeList").on("blur", ".stop-list .scheduled-minute-spinner", function (e) {
		//if the value is empty, reset it
		if ($(this).val() === "") {
			$(this).val("0.0");
			$(this).change();
		}
	});

	$("#routeList").on("change", ".stop-list .scheduled-minute-spinner", function (e) {
		if ($(this).val().trim() === "") {
			$(this).val("0.0");
		}

		var $grandParent = $(this).parent().parent();
		var scheduledDate = $grandParent.find('.scheduled-date');
		var scheduledTime = $grandParent.find('.scheduled-time');
		var plannedDateTime = $grandParent.find('.planned-date-time');

		var dt = new Date(plannedDateTime.val());
		dt.setMinutes(dt.getMinutes() + (parseFloat($(this).val()) * 60));

		// set scheduled date to be planned + offset
		scheduledDate.val(formatDate(dt, "yyyy-MM-dd"));
		scheduledTime.val(formatDate(dt, "hh:mm tt"));
		scheduledDate.change();

		// mark field as editted
		scheduledDate.removeClass('route-stop-time-scheduled');
		scheduledTime.removeClass('route-stop-time-scheduled');

		if (parseFloat($(this).val()) !== 0) {
			$grandParent.find('.route-plan-modification-id').val(3);
			scheduledDate.addClass('route-stop-time-scheduled');
			scheduledTime.addClass('route-stop-time-scheduled');
		}

		if (scheduledDate[0].defaultValue !== scheduledDate[0].value || scheduledTime[0].defaultValue !== scheduledTime[0].value) {
			scheduledDate.addClass('route-stop-time-dirty');
			scheduledTime.addClass('route-stop-time-dirty');
			scheduledDate.closest("form").find('.operation-message').html('');
		} else {
			if (scheduledDate.hasClass('route-stop-time-dirty')) {
				scheduledDate.removeClass('route-stop-time-dirty');
				scheduledTime.removeClass('route-stop-time-dirty');
			}
		}

		$grandParent.find('.scheduled-date-time').val(formatDate(scheduledDate.val() + " " + scheduledTime.val(), "MM/dd/yy hh:mm tt"));
	});


	$("#routeList").on("blur", ".stop-list .scheduled-date", function (e) {
		//if the entered date isn't a valid date format, then reset it
		var scheduledDateTime = $(this).parent().find(".scheduled-date").val() + " " + $(this).parent().find(".scheduled-time").val();
		if (!isValidFormat(scheduledDateTime, "yyyy-MM-dd hh:mm tt")) {
			var defaultScheduledDate = $(this).parent().find(".scheduled-date")[0].defaultValue;
			$(this).parent().find(".scheduled-date").val(defaultScheduledDate);
			var defaultScheduledTime = $(this).parent().find(".scheduled-time")[0].defaultValue;
			$(this).parent().find(".scheduled-time").val(defaultScheduledTime);
			$(this).change();
		}
	});

	$("#routeList").on("blur", ".stop-list .scheduled-time", function (e) {
		//if the entered date isn't a valid date format, then reset it
		var scheduledDateTime = $(this).parent().find(".scheduled-date").val() + " " + $(this).parent().find(".scheduled-time").val();
		if (!isValidFormat(scheduledDateTime, "yyyy-MM-dd hh:mm tt")) {
			var defaultScheduledDate = $(this).parent().find(".scheduled-date")[0].defaultValue;
			$(this).parent().find(".scheduled-date").val(defaultScheduledDate);
			var defaultScheduledTime = $(this).parent().find(".scheduled-time")[0].defaultValue;
			$(this).parent().find(".scheduled-time").val(defaultScheduledTime);
			$(this).change();
		}
	});

	$("#routeList").on("change", ".stop-list .scheduled-date", function (e) {
		changeScheduledAdjustedDateTime(this, "scheduled");
	});

	$("#routeList").on("change", ".stop-list .scheduled-time", function (e) {
		changeScheduledAdjustedDateTime(this, "scheduled");
	});


	$("#routeList").on("focusin", ".stop-list .stop-comment", function () {
		$(this).prop('rows', '8');
	});

	$("#routeList").on("focusout", ".stop-list .stop-comment", function () {
		if (this.defaultValue != this.value) {
			$(this).addClass('route-stop-time-dirty');
			$(this).closest("form").find('.operation-message').html('');
		} else {
			if ($(this).hasClass('route-stop-time-dirty')) {
				$(this).removeClass('route-stop-time-dirty');
			}
		}
	});

	$("#routeList").on("mouseleave", ".stop-list .stop-comment", function () {
		if (this.defaultValue != this.value) {
			$(this).addClass('route-stop-time-dirty');
			$(this).closest("form").find('.operation-message').html('');
		} else {
			if ($(this).hasClass('route-stop-time-dirty')) {
				$(this).removeClass('route-stop-time-dirty');
			}
		}
	});

	$("#routeList").on("change", ".stop-list .stop-comment", function () {
		var parent = $(this).parent().parent();

		if (parent.find('.stop-comment').val() !== '') {
			parent.find('.route-plan-modification-id').val(3);
		}
	});
	
	$("#routeList").on("click", ".stop-list .moved-stop-dialog-trigger", function () {
		//SHOULD Open the route that the stop was moved to
		var routePlanId = $(this).data("routePlanId");
		var clickedRouteId = $(this).data("routeId");
		var modifiedType = $(this).data("stopModifiedType");

		//open modal, on success fill modal with data 
		$("#movedStopPopupContent").html('<i class="fa fa-spinner fa-spin fa-5x loading" aria-hidden="true"></i> &nbsp;&nbsp;loading');
		$("#stopMovedModal").modal('show');
		$.ajax({
			async: true,
			url: "/routetracker/MovedStopPopup?routePlanId=" + routePlanId + "&modifiedType=" + modifiedType,

			type: "GET",
			success: function (data) {
				$("#movedStopPopupContent").html(data);
			},
			error: function (error) {
				$("#stopMovedModal").modal('hide');
				alert("An error occurred while loading the route information.");
			}
		});
	});
};

var bindCommentEvents = function () {
	$('#trackerAddCommentButton').prop('disabled', true);

	$("#trackerAddCommentButton").on("click", function () {
		var centerNumber = $("#selectCenterDropdown").val();

		var url = "/routecomment/commoncreate?centerNumber=" + centerNumber.toString() + "&screen=RT";
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
						//{
						//	text: "Cancel",
						//	id: "commonCreateCommentCancelButton",
						//	click: function () {
						//		var shipTo = $("#comment-box").data("shipTo");
						//		var billTo = $("#comment-box").data("billTo");
						//		var centerNumber = $("#comment-box").data("centerNumber");
						//		var routePlanId = $("#comment-box").data("routePlanId");
						//		var stopNumber = $("#comment-box").data("stopNumber");
						//		loadCommentList(billTo, shipTo, centerNumber, routePlanId, stopNumber);
						//	}
						//},
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
				//$("#commonCreateCommentCancelButton").attr("disabled", true).addClass("ui-state-disabled");
				$("#commonCreateCommentSaveButton").attr("disabled", true).addClass("ui-state-disabled");
			}, // preCall
			function (data) {
				$("#dialog-window").html(data);
			}, // doneCallBack
			function (error) { $("#dialog-window").html(error); }, // failCallBack
			function () { }
		);
	});

	$("#routeList").on("click", ".route-comment-readonly-dialog-trigger", function () {
		var routeId = $(this).data("routeId");
		var centerNumber = $(this).data("centerNumber");
		var routeNumber = $(this).data("routeNumber");

		var url = "/routecomment/ListByRouteId?routeId=" + routeId.toString() + "&centerNumber=" + centerNumber.toString();
		getData(url,
			function () {
				$(".ui-dialog").css({ zIndex: '2000' });
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

	$("#routeList").on("click", ".stop-list .comment-dialog-trigger", function () {
		var shipTo = $(this).data("shipTo");
		var billTo = $(this).data("billTo");
		var centerNumber = $(this).data("centerNumber");
		var routePlanId = $(this).data("routePlanId");
		var stopNumber = $(this).data("stopNumber");

		loadCommentList(billTo, shipTo, centerNumber, routePlanId, stopNumber, "RT", true);
	});

	$("#stopMovedModal").on("click", ".route-comment-readonly-dialog-trigger", function () {
		var routeId = $(this).data("routeId");
		var centerNumber = $(this).data("centerNumber");
		var routeNumber = $(this).data("routeNumber");

		var url = "/routecomment/ListByRouteId?routeId=" + routeId.toString() + "&centerNumber=" + centerNumber.toString();
		getData(url,
			function () {
				$(".ui-dialog").css({ zIndex: '2000' });
				$("#dialog-window").dialog({
					title: "AGGREGATE ACTIVITY VIEWER FOR ROUTE " + routeNumber,
					width: "550px",
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

	$("#stopMovedModal").on("click", ".comment-dialog-trigger", function () {
		var shipTo = $(this).data("shipTo");
		var billTo = $(this).data("billTo");
		var centerNumber = $(this).data("centerNumber");
		var routePlanId = $(this).data("routePlanId");
		var stopNumber = $(this).data("stopNumber");

		loadCommentList(billTo, shipTo, centerNumber, routePlanId, stopNumber, "RT", true);
	});
};

var changeScheduledAdjustedDateTime = function (self, prefix) {
	var $grandParent = $(self).parent().parent();

	// get combined date & time 
	var $fieldDate, $fieldTime, fieldDateTime;
	$fieldDate = $(self).parent().find("." + prefix + "-date");
	$fieldTime = $(self).parent().find("." + prefix + "-time");
	fieldDateTime = $fieldDate.val() + " " + $fieldTime.val();

	if (prefix === "adjusted") {
		var scheduledDate, scheduledTime, scheduledDateTime;
		// get combined date & time for adjusted
		scheduledDate = $grandParent.find('.scheduled-date').val();
		scheduledTime = $grandParent.find('.scheduled-time').val();
		scheduledDateTime = new Date(scheduledDate + " " + scheduledTime);

		if (fieldDateTime.trim() === "") {
			// if adjusted is empty then reset to scheduled value
			$fieldDate.val(scheduledDate);
			$fieldTime.val(scheduledTime);
			fieldDateTime = $fieldDate.val() + " " + $fieldTime.val();
		} else {
			if (!isValidFormat(fieldDateTime, "yyyy-MM-dd hh:mm tt")) {
				// if adjusted is not formatted correctly then reset to scheduled value
				var minuteSpinner = $grandParent.find('.adjusted-minute-spinner');
				minuteSpinner.val("0.0");
				minuteSpinner.change();
				return;
			}
		}

		// get diff between adjusted & scheduled
		var adjustDate = new Date(fieldDateTime);
		var diff = adjustDate - scheduledDateTime;
		var hours = (diff / 3600000).toFixed(1);

		$grandParent.find('.adjusted-minute-spinner').val(hours);
		if (parseFloat(hours) !== 0) {
			// if adjustment has been made (adjusted != scheduled) then mark record with type=3 and highlight date & time input boxes
			$grandParent.find('.route-plan-modification-id').val(3);
			$fieldDate.addClass('route-stop-time-' + prefix);
			$fieldTime.addClass('route-stop-time-' + prefix);
		}

		// update route level adjusted dispatch date if stop 0 is modified
		if ($(self).parent().data("stopNumber").toString() === "0") {
			$(self).closest("form").parent().closest("tr").prev().find("#AdjustedDispatchTime_" + $(self).parent().data("routeId")).html(formatDate($fieldDate.val() + " " + $fieldTime.val(), "MM/dd/yy hh:mm tt"));
		}
		if ($fieldDate[0].defaultValue !== $fieldDate[0].value || $fieldTime[0].defaultValue !== $fieldTime[0].value) {
			$fieldDate.addClass('route-stop-time-dirty');
			$fieldTime.addClass('route-stop-time-dirty');
			$(self).closest("form").find('.operation-message').html('');
		} else {
			if ($fieldDate.hasClass('route-stop-time-dirty')) {
				$fieldDate.removeClass('route-stop-time-dirty');
				$fieldTime.removeClass('route-stop-time-dirty');
			}
		}

		$grandParent.find('.adjusted-date-time').val(formatDate($fieldDate.val() + " " + $fieldTime.val(), "MM/dd/yy hh:mm tt"));
		$grandParent.find('.scheduled-date-time').val(formatDate(scheduledDate + " " + scheduledTime, "MM/dd/yy hh:mm tt"));
	}
	else {
		if (fieldDateTime.trim() === "") {
			$fieldDate.val($fieldDate[0].defaultValue);
			$fieldTime.val($fieldTime[0].defaultValue);
			fieldDateTime = $fieldDate.val() + " " + $fieldTime.val();
		} else {
			if (!isValidFormat(fieldDateTime, "yyyy-MM-dd hh:mm tt")) {
				$fieldDate.val($fieldDate[0].defaultValue);
				$fieldTime.val($fieldTime[0].defaultValue);
				$fieldDate.change();
				return;
			}
		}

		// get diff between planned & scheduled
		var scheduleDate = new Date(fieldDateTime);
		var plannedDateTime = new Date($grandParent.find('.planned-date-time').val());
		var diffScheduled = scheduleDate - plannedDateTime;
		var scheduledDiffHour = (diffScheduled / 3600000).toFixed(1);
		$grandParent.find('.scheduled-minute-spinner').val(scheduledDiffHour);

		//set the adjusted time to the same as scheduled by making offset hours 0 then calling the minute-spinners auto-update change function
		$grandParent.find('.adjusted-minute-spinner').val("0.0");
		$grandParent.find('.adjusted-minute-spinner').change();

		// update route level adjusted dispatch date if stop 0 is modified
		if ($(self).parent().data("stopNumber").toString() === "0") {
			$(self).closest("form").parent().closest("tr").prev().find("#ScheduledDispatchTime_" + $(self).parent().data("routeId")).html(formatDate($fieldDate.val() + " " + $fieldTime.val(), "MM/dd/yy hh:mm tt"));
		}

		if ($fieldDate[0].defaultValue !== $fieldDate[0].value || $fieldTime[0].defaultValue !== $fieldTime[0].value) {
			$fieldDate.addClass('route-stop-time-dirty');
			$fieldTime.addClass('route-stop-time-dirty');
			$(self).closest("form").find('.operation-message').html('');
		} else {
			if ($fieldDate.hasClass('route-stop-time-dirty')) {
				$fieldDate.removeClass('route-stop-time-dirty');
				$fieldTime.removeClass('route-stop-time-dirty');
			}
		}

		$grandParent.find('.scheduled-date-time').val(formatDate($fieldDate.val() + " " + $fieldTime.val(), "MM/dd/yy hh:mm tt"));
	}
};

var cascadeChanges = function (buttonClickedId) {
	//get selected button    
	buttonClickedId = "#" + buttonClickedId;
	var button = $(buttonClickedId);
	var type = buttonClickedId.toUpperCase().indexOf("ADJUSTED") > -1 ? "adjusted" : "scheduled";

	//get selected row
	var buttonRow = button.parent().parent();

	//get all rows
	var allRows = buttonRow.parent().find('tr');

	//get selected row index
	var buttonRowIndex = parseInt(buttonRow[0].attributes["data-stop-index"].value);

	//get value of offset time
	var selectedSpinner = null;
	selectedSpinner = button.parent().parent().find('.' + type + '-minute-spinner');

	if (selectedSpinner == null) {
		return;
	}
	var selectedValue = selectedSpinner[0].value;

	//iterate through all rows
	var i = buttonRowIndex + 1;
	while (i < allRows.length) { //skip the last row since it houses the submit button
		//get current row and it's modification type
		var currentRow = allRows[i];
		var currentRowModificationType = $(currentRow)[0].attributes["data-stop-modified-type"].value;

		//if the stop is NOT removed
		if (currentRowModificationType != 1 && currentRowModificationType != 4) {
			//then cascade it's offset time
			var currentSpinner = null;
			currentSpinner = $(currentRow).find('.' + type + '-minute-spinner');

			if (currentSpinner != null) {
				currentSpinner[0].value = selectedValue;
				$(currentSpinner).trigger("change");
			}
		}

		i++;
	}
};

var filterRoutes = function () {
	var filterValue = $("#routeSearch").val();
	var filterModified = $("#modifiedTypeSelectList").val();
	//var filterByDispatchDayValues = $("#FilterRoutesByDepartDay").val() || [];
	var filterByLateEarly = $("#EarlyLateSelection").val();
	var filterConcept = $("#ConceptTypeList").val();
	var filterOrder = $("#RouteOrderSelection").val();

	var Routes = document.querySelector("#routes-table tbody");

	if (Routes != null) {
		var rows = Routes.rows;
		for (var i = 0; i < rows.length; i++) {
			//The is a hidden row in each route used for striping and this skips that row
			if ("data-number" in rows[i].attributes) {
				var routeNumber = rows[i].attributes["data-number"].value.toUpperCase();
				var formattedRouteNumber = routeNumber.trim().replace(/[^\d]/g, ''); // 1238
				var routeName = rows[i].attributes["data-name"].value.toUpperCase();
				var routeConceptNames = rows[i].attributes["data-route-concept-names"].value.toUpperCase();
				var isRouteModified = rows[i].attributes["data-route-modified"].value.toUpperCase();

				//If you pass all filters, then show the route
				if (filterBySearch(filterValue, routeNumber, routeName, routeConceptNames) &&
					filterByModifiedType(filterModified, rows[i]) &&
					filterByConceptType(filterConcept, rows[i]) &&
					filterByOrder(filterOrder, rows[i]) &&
					filterByLateEarlyType(filterByLateEarly, rows[i])) {
					//filterByDispatchDay(filterByDispatchDayValues, formattedRouteNumber)) {

					rows[i].style.display = "";
				}
				else {
					//Hide the route
					rows[i].style.display = "none";
				}
			}
		}
		updateTotalLabel();
	} else {
		clearTotalLabel();
	}

	function filterBySearch(filterString, routeNumber, routeName, routeConcepts) {
		var result = false;
		//If no search param given
		if (filterString === "") { result = true; }
		else {
			//Convert to uppercase so we can match regardless of case
			filterString = filterString.toUpperCase();
			result = (routeNumber.indexOf(filterString) > -1 || routeName.indexOf(filterString) > -1 || routeConcepts.indexOf(filterString) > -1);
		}
		return result;
	}

	function filterByOrder(orderFilters, currentRoute) {
		var result = false;
		//Flag is any type of modification was made
		var routeOrderStatus = currentRoute.attributes["data-route-has-order"].value;

		//User did not select any modified route filters
		if (orderFilters === null) { result = true; }
		else {
			result = orderFilters.includes(routeOrderStatus);
		}
		return result;
	}

	function filterByModifiedType(modifiedTypeFilters, currentRoute) {
		var result = false;
		//Flag is any type of modification was made
		var routeIsModified = currentRoute.attributes["data-route-modified"].value;
		var routeHasAddedStops = currentRoute.attributes["data-route-added-stops"].value;
		var routeHasRemovedStops = currentRoute.attributes["data-route-removed-stops"].value;
		var routeHasTimeChanges = currentRoute.attributes["data-route-time-changes"].value;

		//User did not select any modified route filters
		if (modifiedTypeFilters === null) { result = true; }
		else {
			result = ((modifiedTypeFilters.includes('timeChanged') && routeHasTimeChanges === "True") || (modifiedTypeFilters.includes('added') && routeHasAddedStops === "True") || (modifiedTypeFilters.includes('removed') && routeHasRemovedStops === "True"));
		}
		return result;
	}

	function filterByConceptType(conceptTypeFilters, currentRoute) {
		var result = true;
		var routeConceptString = currentRoute.attributes["data-route-concepts"].value;
		var routeConceptList = routeConceptString.split(",");
		//User did not select any concpet filters
		if (conceptTypeFilters === null) {
			result = true;
		}
		else {
			var i = 0;
			while (i < routeConceptList.length) {
				//if the item in the route concept list is in the filters return true for that item, otherwise check the next concept in the route list
				if (conceptTypeFilters.indexOf(routeConceptList[i]) >= 0) {
					return true;

				}
				i++;
			}
			return false;
		}
		return result;
	}

	function filterByLateEarlyType(earlyLateFilters, currentRoute) {
		var routeIsEarlyLate = currentRoute.attributes["data-route-is-early-late"].value;
		return (earlyLateFilters === null) || (earlyLateFilters.includes('-1') && routeIsEarlyLate === "-1") || (earlyLateFilters.includes('0') && routeIsEarlyLate === "0") || (earlyLateFilters.includes('1') && routeIsEarlyLate === "1");
	}

	function filterByDispatchDay(dispatchDayFilters, routeNumber) {
		var result = false;
		//If no Depart Days were specified then we show ALL days
		if (dispatchDayFilters.length === 0) {
			result = true;
		}
		else {
			//If any of the selected depart days match the routeNumber's first character
			result = (dispatchDayFilters.indexOf(routeNumber.charAt(0)) > -1);
		}
		return result;
	}
};

var updateTotalLabel = function () {

	var rows = document.querySelector("#routes-table tbody").rows;
	var total = 0;
	//go through each
	for (var i = 0; i < rows.length; i++) {
		if ("data-number" in rows[i].attributes && rows[i].style.display === "") {
			//if is a data row
			//and is visible
			total++;
			//count it
		}
	}
	//Divide by 2 to remove the rows which are route stops
	total = total / 2;
	//update label
	document.getElementById("totalRowsLabel").innerHTML = "Row Count: " + total;
};

var clearTotalLabel = function () {
	document.getElementById("totalRowsLabel").innerHTML = "Row Count: 0";
};

var submitStopChange = function (formid) {
	var type = 'POST';
	var url = '/RouteTracker/ChangeStopTime';
	var contentType = 'application/x-www-form-urlencoded';
	var form = $('#frm' + formid);
	var message = form.find('.operation-message');
	var stopList = form.closest(".stop-list");
	var cover = stopList.next();
	stopList.hide();
	cover.show();
	$.ajax({
		url: url,
		type: type,
		data: form.serialize(),
		contentType: contentType,
		cache: false,
		async: true,
		success: function (data) {
			$("#toast-window").html("<span> ... Successful ... </span>");
			$("#toast-window").addClass("toast-is-shown");
			setTimeout(function () {
				$("#toast-window").removeClass("toast-is-shown");
			}, 3000);

			stopList.find('.stop-comment').removeClass('route-stop-time-dirty');
			stopList.find('.adjusted-minute-spinner').removeClass('route-stop-time-dirty');
			stopList.find('.adjusted-date').removeClass('route-stop-time-dirty');
			stopList.find('.adjusted-time').removeClass('route-stop-time-dirty');
			stopList.find('.scheduled-date').removeClass('route-stop-time-dirty');
			stopList.find('.scheduled-time').removeClass('route-stop-time-dirty');

			$.ajax({
				async: true,
				url: "/routetracker/stops?routeId=" + formid,
				type: "GET",
				success: function (gData) {
					stopList.html(gData);
					stopList.show();
					stopList.find('.operation-message').html('Successful').show();
					cover.hide();
					//$('.scheduled-date').change();
					$('.adjusted-date').change();

					$(".stop-list[data-route-id='" + formid + "'] .time-boxes-picker").timepicker({
						timeFormat: 'hh:mm p',
						interval: 15,
						minTime: '12:00 AM',
						maxTime: '11:45 PM',
						dynamic: true,
						dropdown: true,
						scrollbar: false,
						change: function (time) {
							if (this[0].id.toUpperCase().indexOf("ADJUSTED") > -1)
								changeScheduledAdjustedDateTime(this, "adjusted");
							else
								changeScheduledAdjustedDateTime(this, "scheduled");
						}
					});
				},

				error: function (gError) {
					stopList.html(gError);
				}
			});
		},
		error: function (error) {
			message.html('Unsuccessful').show();
			stopList.show();
			cover.hide();
		}
	});
};

// ----- excel report export related functions

var generateExcelReport = function () {
	var centerNum = $("#selectCenterDropdown").val();
	var startDateString = $("#startDateInput").val();
	var endDateString = $("#endDateInput").val();
	//$("#routeSearch").val(); 

	//Convert the string date to a local time and update it to use noon as the time.
	var filterStartDate = convertUTCDateToLocalDate(new Date(startDateString));
	filterStartDate.setHours(12);
	var filterEndDate = convertUTCDateToLocalDate(new Date(endDateString));
	filterEndDate.setHours(12);

	//If either Start Date or End Date wasn't a valid date
	if (!isDate(filterStartDate) || !isDate(filterEndDate)) {
		//Hide all routes and return
		//TODO: notify user of the issue
		return;
	}

	window.location = "/routetracker/ExcelExport" + "?sygmaCenterNumber= " + centerNum + "&startDate=" + filterStartDate.toISOString() + "&stopDate=" + filterEndDate.toISOString();
};

// ----- date related functions

var generateDateRanges = function () {
	//Retrieve the selected value of the Date Range DD
	var dateRangeValue = $("#dateRangeDropdown").val();
	//Set the Start Date and End Date inputs to be disabled be default
	$("#startDateInput").prop('disabled', true);
	$("#endDateInput").prop('disabled', true);
	//Retrieve the current Start Date and save incase user selects custom
	var fromDate = new Date($("#startDateInput").val());
	//Retrieve the current End Date and save incase user selects custom
	var toDate = new Date($("#endDateInput").val());
	var today = new Date();
	switch (dateRangeValue) {
		//When Today Selected
		case "0":
			//Today
			fromDate = today;
			//Tomorrow
			toDate = addDays(today, 1);
			break;
		//When Tomorrow Selected
		case "1":
			//Tomorrow
			fromDate = addDays(today, 1);
			//Tomorrow + 1
			toDate = addDays(today, 2);
			break;
		//When Current Week Selected
		case "2":
			//The Next Saturday 
			toDate = getNextSaturday();
			//The Sunday of current weekending
			fromDate = addDays(getNextSaturday(), -6);
			break;
		//When yesterday Selected
		case "3":
			//yesterday
			fromDate = addDays(today, -1);
			//today
			toDate = today;
			break;
		//When Last Week Selected
		case "4":
			toDate = getLastSaturday();
			//Get Last Sunday
			fromDate = addDays(getLastSaturday(), -6);
			break;
		//When Custom Seected
		case "5":
			//Today
			fromDate = today;
			//Tomorrow
			toDate = addDays(today, 1);
			//Enable the Start Date and End date for the user to modify
			$("#startDateInput").prop('disabled', false);
			$("#endDateInput").prop('disabled', false);
			break;
	}
	//Set the from date and to date fields with the values we calculated
	$("#startDateInput").val(formatDate(fromDate));
	$("#endDateInput").val(formatDate(toDate));
};

// ----- Column Options related functions

var loadColumnOption = function () {
	var url = "/routetracker/ColumnOption";
	getData(url,
		loadColumnOptionPreCall, // preCall
		loadColumnOptionDoneCallBack, // doneCallBack
		loadColumnOptionFailCallBack, // failCallBack
		function () { }
	);
};

var loadColumnOptionPreCall = function () {
	$("#columnOptionsDialog").dialog("open");
	$("#columnOptionsDialog").html("<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading ... ");
};

var loadColumnOptionDoneCallBack = function (data) {
	$("#columnOptionsDialog").html(data);
};

var loadColumnOptionFailCallBack = function (error) {
	$("#columnOptionsDialog").html(error);
};

var bindColumnOptionButtons = function () {
	// dialog handlers
	$("#columnOptionsDialog").dialog({
		autoOpen: false,
		modal: true,
		resizable: false,
		position: {
			my: "top",
			at: "top",
			of: "#filterContainer"
		},
		title: "Column Options",
		width: 600,
		height: 550,
		buttons: {
			"Reset to Default": {
				text: "Reset to Default",
				id: "columnOptionsDialogReset",
				click: function () {
					// save column option
					saveColumnOptions(true);
				}
			},
			"Cancel": {
				text: "Cancel",
				id: "columnOptionsDialogCancel",
				click: function () { $(this).dialog("close"); }
			},
			"Ok": {
				text: "Ok",
				id: "columnOptionsDialogOk",
				click: function () {
					// save column option
					saveColumnOptions(false);

					// close dialog
					$(this).dialog("close");
				}
			}
		}
	});

	$("#columnOptionsButton").click(function () {
		loadColumnOption();
	});

	// route handlers
	$("#columnOptionsDialog").on("click", "#routeColumnMoveRight", function () {
		// move selected items from available list box and append to selected list box
		$("#routeAvailableColumns > option:selected").each(function () {
			$(this).remove().appendTo("#routeSelectedColumns");
		});

		// if there is nothing in the selected list box, disable OK/Save button
		if ($("#routeSelectedColumns > option").length > 0) {
			$("#columnOptionsDialogOk").attr("disabled", false);
		}

		// make sure none is selected on the selected list box
		$("#routeSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("click", "#routeColumnMoveLeft", function () {
		if ($("#routeSelectedColumns > option").length == $("#routeSelectedColumns > option:selected").length) {
			alert("You need to have at least one column selected.");
		}
		else {
			if ($("#routeSelectedColumns > option").length > 1) {
				$("#routeSelectedColumns > option:selected").each(function () {
					$(this).remove().appendTo("#routeAvailableColumns");
					$("#columnOptionsDialogOk").attr("disabled", false);
				});
			}
		}
		$("#routeAvailableColumns option").attr("selected", false);
		$("#routeSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("click", "#routeColumnMoveAllRight", function () {
		// move selected items from available list box and append to selected list box
		$("#routeAvailableColumns > option").each(function () {
			$(this).remove().appendTo("#routeSelectedColumns");
		});

		// if there is nothing in the selected list box, disable OK/Save button
		if ($("#routeSelectedColumns > option").length > 0) {
			$("#columnOptionsDialogOk").attr("disabled", false);
		}

		// make sure none is selected on the selected list box
		$("#routeSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("dblclick", "#routeAvailableColumns", function () {
		// move selected items from available list box and append to selected list box
		$("#routeAvailableColumns > option:selected").each(function () {
			$(this).remove().appendTo("#routeSelectedColumns");
		});

		// if there is nothing in the selected list box, disable OK/Save button
		if ($("#routeSelectedColumns > option").length > 0) {
			$("#columnOptionsDialogOk").attr("disabled", false);
		}

		// make sure none is selected on the selected list box
		$("#routeSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("dblclick", "#routeSelectedColumns", function () {
		if ($("#routeSelectedColumns > option").length == $("#routeSelectedColumns > option:selected").length) {
			alert("You need to have at least one column selected.");
		}
		else {
			if ($("#routeSelectedColumns > option").length > 1) {
				$("#routeSelectedColumns > option:selected").each(function () {
					$(this).remove().appendTo("#routeAvailableColumns");
					$("#columnOptionsDialogOk").attr("disabled", false);
				});
			}
		}
		$("#routeAvailableColumns option").attr("selected", false);
		$("#routeSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("click", "#routeColumnMoveDown", function () {
		var selectedOptions = $('#routeSelectedColumns > option:selected');
		if (selectedOptions.length > 0) {
			if (selectedOptions.last().index() < ($("#routeSelectedColumns > option").length - 1)) {
				var nextOption = $('#routeSelectedColumns > option:selected').next("option").last();

				if ($(nextOption).text() != "") {
					$(selectedOptions).remove();
					$(nextOption).after($(selectedOptions));
					$("#columnOptionsDialogOk").attr("disabled", false);
				}
			}
		}
	});

	$("#columnOptionsDialog").on("click", "#routeColumnMoveUp", function () {
		var selectedOptions = $('#routeSelectedColumns > option:selected');
		if (selectedOptions.length > 0) {
			if (selectedOptions[0].index > 0) {
				var prevOption = $('#routeSelectedColumns > option:selected').prev("option").first();

				if ($(prevOption).text() != "") {
					$(selectedOptions).remove();
					$(prevOption).before($(selectedOptions));
					$("#columnOptionsDialogOk").attr("disabled", false);
				}
			}
		}
	});

	// stop handlers
	$("#columnOptionsDialog").on("click", "#stopColumnMoveRight", function () {
		// move selected items from available list box and append to selected list box
		$("#stopAvailableColumns > option:selected").each(function () {
			$(this).remove().appendTo("#stopSelectedColumns");
		});

		// if there is nothing in the selected list box, disable OK/Save button
		if ($("#stopSelectedColumns > option").length > 0) {
			$("#columnOptionsDialogOk").attr("disabled", false);
		}

		// make sure none is selected on the selected list box
		$("#stopSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("click", "#stopColumnMoveLeft", function () {
		var alertUserForDeliveryColumns = false;
		if ($("#stopSelectedColumns > option").length == $("#stopSelectedColumns > option:selected").length) {
			alert("You need to have at least one column selected.");
		}
		else {
			if ($("#stopSelectedColumns > option").length > 1) {
				$("#stopSelectedColumns > option:selected").each(function () {
					if ($(this).val() == "31" || $(this).val() == "32" || $(this).val() == "33" || $(this).val() == "34" || $(this).val() == "35") {
						alertUserForDeliveryColumns = true;
					}
					else {
						$(this).remove().appendTo("#stopAvailableColumns");
						$("#columnOptionsDialogOk").attr("disabled", false);
					}
				});
			}
		}
		if (alertUserForDeliveryColumns) alert("Cannot remove delivery date fields from display.");
		$("#stopAvailableColumns option").attr("selected", false);
		$("#stopSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("click", "#stopColumnMoveAllRight", function () {
		// move selected items from available list box and append to selected list box
		$("#stopAvailableColumns > option").each(function () {
			$(this).remove().appendTo("#stopSelectedColumns");
		});

		// if there is nothing in the selected list box, disable OK/Save button
		if ($("#stopSelectedColumns > option").length > 0) {
			$("#columnOptionsDialogOk").attr("disabled", false);
		}

		// make sure none is selected on the selected list box
		$("#stopSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("dblclick", "#stopAvailableColumns", function () {
		// move selected items from available list box and append to selected list box
		$("#stopAvailableColumns > option:selected").each(function () {
			$(this).remove().appendTo("#stopSelectedColumns");
		});

		// if there is nothing in the selected list box, disable OK/Save button
		if ($("#stopSelectedColumns > option").length > 0) {
			$("#columnOptionsDialogOk").attr("disabled", false);
		}

		// make sure none is selected on the selected list box
		$("#stopSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("dblclick", "#stopSelectedColumns", function () {
		if ($("#stopSelectedColumns > option").length == $("#stopSelectedColumns > option:selected").length) {
			alert("You need to have at least one column selected.");
		}
		else {
			if ($("#stopSelectedColumns > option").length > 1) {
				$("#stopSelectedColumns > option:selected").each(function () {
					if ($(this).val() == "31" || $(this).val() == "32" || $(this).val() == "33" || $(this).val() == "34" || $(this).val() == "35") {
						alertUserForDeliveryColumns = true;
					}
					else {
						$(this).remove().appendTo("#stopAvailableColumns");
						$("#columnOptionsDialogOk").attr("disabled", false);
					}
				});
			}
		}
		$("#stopAvailableColumns option").attr("selected", false);
		$("#stopSelectedColumns option").attr("selected", false);
	});

	$("#columnOptionsDialog").on("click", "#stopColumnMoveDown", function () {
		var selectedOptions = $('#stopSelectedColumns > option:selected');
		if (selectedOptions.length > 0) {
			if (selectedOptions.last().index() < ($("#stopSelectedColumns > option").length - 1)) {
				var nextOption = $('#stopSelectedColumns > option:selected').next("option").last();

				if ($(nextOption).text() != "") {
					$(selectedOptions).remove();
					$(nextOption).after($(selectedOptions));
					$("#columnOptionsDialogOk").attr("disabled", false);
				}
			}
		}
	});

	$("#columnOptionsDialog").on("click", "#stopColumnMoveUp", function () {
		var selectedOptions = $('#stopSelectedColumns > option:selected');
		if (selectedOptions.length > 0) {
			if (selectedOptions[0].index > 0) {
				var prevOption = $('#stopSelectedColumns > option:selected').prev("option").first();

				if ($(prevOption).text() != "") {
					$(selectedOptions).remove();
					$(prevOption).before($(selectedOptions));
					$("#columnOptionsDialogOk").attr("disabled", false);
				}
			}
		}
	});
};

var saveColumnOptions = function (reset) {
	var returnObject = {};
	returnObject.routeColumnOption = {};
	returnObject.routeColumnOption.columns = [];
	returnObject.routeColumnOption.availableColumns = [];
	if (!reset) {
		$("#routeSelectedColumns > option").each(function (index) {
			var rColumn = {
				ID: $(this).val(),
				ColumnIdentifier: $(this).html(),
				DisplayOrder: (index + 1).toString(),
				Visible: "true",
				Width: "0"
			};
			returnObject.routeColumnOption.columns.push(rColumn);
		});

		$("#routeAvailableColumns > option").each(function (index) {
			var rAColumn = {
				ID: $(this).val(),
				ColumnIdentifier: $(this).html()
			};
			returnObject.routeColumnOption.availableColumns.push(rAColumn);
		});
	}

	returnObject.stopColumnOption = {};
	returnObject.stopColumnOption.columns = [];
	returnObject.stopColumnOption.availableColumns = [];
	if (!reset) {
		$("#stopSelectedColumns > option").each(function (index) {
			var sColumn = {
				ID: $(this).val(),
				ColumnIdentifier: $(this).html(),
				DisplayOrder: (index + 1).toString(),
				Visible: "true",
				Width: "0"
			};
			returnObject.stopColumnOption.columns.push(sColumn);
		});

		$("#stopAvailableColumns > option").each(function (index) {
			var sAColumn = {
				ID: $(this).val(),
				ColumnIdentifier: $(this).html()
			};
			returnObject.stopColumnOption.availableColumns.push(sAColumn);
		});
	}

	var url = "/routetracker/SaveColumnOption/";
	jQuery.ajax({
		url: url,
		type: 'POST',
		data: returnObject
	}).success(function (data) {
		if (!reset) {
			$("#columnOptionsDialog").dialog("close");
		} else {
			$.ajax({
				async: true,
				url: "/routetracker/ColumnOption",
				type: "GET",
				success: function (data) {
					$("#columnOptionsDialog").html(data);
				},
				error: function (error) {
					alert(error);
				}
			});
		}
		loadRoutes();
	}).error(function (fData) {
		alert("failed: " + JSON.stringify(fData));
	});
};

// ------

$(document).ready(function () {
	$('.multiselect-ui').multiselect({
		includeSelectAllOption: true,
		inheritClass: true
	});

	generateDateRanges();

	var lastSunday = addDays(getLastSaturday(), 1).toISOString().split('T')[0];
	var nextSaturday = getNextSaturday().toISOString().split('T')[0];
	$(".route-tracker-filter-date").prop('min', lastSunday);
	$(".route-tracker-filter-date").prop('max', nextSaturday);

	$("#selectCenterDropdown").on("change", function () {
		var center = $(this).val();
		if (center === '') {
			$('#excelExportButton').prop('disabled', true);
			$('#trackerAddCommentButton').prop('disabled', true);
		} else {
			$('#excelExportButton').prop('disabled', false);
			$('#trackerAddCommentButton').prop('disabled', false);

			var url = "RouteTracker/ConceptsForCenter?SygmaCenterNumber=" + center;
			$.getJSON(url, null, function (data) {
				$("#ConceptTypeList").empty();
				//use numeric iterator
				data = JSON.parse(data);

				var i = 0;
				while (i < data.length) {
					$("#ConceptTypeList").append("<option value='"
						+ data[i]["ConceptId"]
						+ "'>" + data[i]["Concept"]
						+ "</option>");
					i++;
				}
				$("#ConceptTypeList").multiselect('rebuild');
			});
		}
		var startDate = $("#startDateInput").val();
		var endDate = $("#endDateInput").val();
		//Clear out search field and all filters
		$("#routeSearch").val("");
		//Trigger loading the routes
		loadRoutes();
	});

	$("#export-dialog-message").dialog({
		modal: true,
		autoOpen: false
	});

	$("#routeNotificationButton").click(function () {
		location.href = "/routenotification/index";
	});

	$("#excelExportButton").click(function () {
		generateExcelReport();
		$("#export-dialog-message").dialog("open");
	});

	$(".tracker-search-button").click(function () {
		loadRoutes();
	});

	$(".strictly-numeric-only").keypress(function (event) {
		if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) {
			// should  be ok
		} else {
			event.preventDefault();
		}
	});

	$('#excelExportButton').prop('disabled', true);

	$(".route-tracker-filter").not("#routeSearch").change(function () {
		if ($(this).hasClass("route-tracker-filter-date")) {
			if ($(this).val() === "") {
				$(this).val("");
				return;
			}
			loadRoutes();
		} else if ($(this).hasClass("route-tracker-filter-date-range")) {
			generateDateRanges();
			loadRoutes();
			return;
		} else {
			filterRoutes();
		}
	});

	$("#routeSearch").on("keyup", function () {
		filterRoutes();
	});

	$("#routeList").on("click", ".show-hide-stops", function () {
		var id = $(this).data("id");

		if ($(this).find("i").hasClass('fa-plus')) {
			$(this).find("i").removeClass('fa-plus').addClass('fa-minus');
		}
		else {
			var originalScheduledDispatchDateTime = $(this).parent().parent().next().find("#stops_0_OriginalScheduledDeliveryDateTime").val();
			$(this).parent().parent().find("#ScheduledDispatchTime_" + id).html(originalScheduledDispatchDateTime);
			var originalAdjustedDispatchDateTime = $(this).parent().parent().next().find("#stops_0_OriginalAdjustedDeliveryDateTime").val();
			$(this).parent().parent().find("#AdjustedDispatchTime_" + id).html(originalAdjustedDispatchDateTime);

			$(this).find("i").removeClass('fa-minus').addClass('fa-plus');
		}

		loadStopsForRoute(id);
	});

	bindStopEvents();

	bindColumnOptionButtons();

	bindCommentEvents();

	if ($("#selectCenterDropdown option").length === 2) {
		var firstOption = $("#selectCenterDropdown option:eq(1)").val();
		//Then select it
		$("#selectCenterDropdown").val(firstOption);
		$("#selectCenterDropdown").change();
		//Make the Export to Excel button enabled
		$('#excelExportButton').prop('disabled', false);
		//Load the Routes table
		$("#selectCenterDropdown").change();
	}
});