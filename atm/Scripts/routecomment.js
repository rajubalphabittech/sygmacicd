var loadCommentList = function (billTo, shipTo, centerNumber, routePlanId, stopNumber, screen, openDialog) {
	var url = "/routecomment/ListByBillToShipTo?billTo=" + billTo.toString() + "&shipTo=" + shipTo.toString() + "&stopNumber=" + stopNumber.toString() + "&routePlanId=" + routePlanId.toString() + "&centerNumber=" + centerNumber.toString() + "&screen=" + screen;
	getData(url,
		function () { loadCommentPreCall(stopNumber, openDialog); }, // preCall
		loadCommentDoneCallBack, // doneCallBack
		loadCommentFailCallBack, // failCallBack
		function () { }
	);
};

var loadCommentPreCall = function (stopNumber, openDialog) {
	$("#dialog-window").dialog({
		title: "ACTIVITY LOG FROM STOP " + stopNumber.toString(),
		width: "550px",
		modal: true,
		buttons: [
			{
				text: "Call Logging",
				click: function () {
					window.open("http://sygmanet/Departments/Customer_Service/Call_Logging/CallEntry.aspx", "_blank");
				}
			},
			{
				text: "OK",
				click: function () {
					$(this).dialog("close");
				}
			}
		]
	});
	if (openDialog) {
		$("#dialog-window").dialog("open");
	}
	$("#dialog-window").html("<i class='fa fa-spinner fa-spin loading' aria-hidden='true'></i> loading ... ");
};

var loadCommentDoneCallBack = function (data) {
	$("#dialog-window").html(data);
	$("#dialog-window .comment-table tr:even:not(:first)").addClass("comment-table-stripe");
};

var loadCommentFailCallBack = function (error) {
	$("#dialog-window").html(error);
	$("#dialog-window").dialog("open");
};

// --- save comment (create or update comment) related methods

var createComment = function (commentParameters) {
	var url = "/routecomment/create";
	postData(url,
		{
			longComment: commentParameters.comment, centerNumber: commentParameters.centerNumber, category: commentParameters.category, stopNumber: commentParameters.stopNumber,
			isInternal: commentParameters.isInternal, shipTo: commentParameters.shipTo, billTo: commentParameters.billTo, routePlanId: commentParameters.routePlanId, screen: commentParameters.screen
		},
		function () { $("#dialog-window").html("<i class='fa fa-spinner fa-5x fa-spin loading' aria-hidden='true'></i> &nbsp;&nbsp;loading"); }, // preCall
		function (data) { createCommentDoneCallBack(data, commentParameters); }, // doneCallBack
		function (error) { alert("failed: " + JSON.stringify(error)); }, // failCallBack
		function () { }
	);
};

var createCommentDoneCallBack = function (data, commentParameters) {
	if (commentParameters.category === "Customer Communication") {
		$(".comment-dialog-trigger[data-stop-number='" + commentParameters.stopNumber + "']").prev().text(commentParameters.comment);
	}
	$("#dialog-window").html(data);
	$("#dialog-window .comment-table tr:even:not(:first)").addClass("comment-table-stripe");
	$("#comment-box").val("");
	$("#dialog-window").dialog({
		title: "ACTIVITY LOG FROM STOP " + commentParameters.stopNumber.toString(),
		width: "550px",
		buttons: [
			{
				text: "Call Logging",
				click: function () {
					window.open("http://sygmanet/Departments/Customer_Service/Call_Logging/CallEntry.aspx", "_blank");
				}
			},
			{
				text: "Close",
				click: function () {
					$(this).dialog("close");
				}
			}
		]
	});
};

var updateComment = function (commentParameters) {
	var url = "/routecomment/edit/" + commentParameters.commentId;
	postData(url,
		{
			longComment: commentParameters.comment, category: commentParameters.category, isInternal: commentParameters.isInternal, stopNumber: commentParameters.stopNumber,
			centerNumber: commentParameters.centerNumber, shipTo: commentParameters.shipTo, billTo: commentParameters.billTo, screen: commentParameters.screen
		},
		function () { $("#dialog-window").html("<i class='fa fa-spinner fa-5x fa-spin loading' aria-hidden='true'></i> &nbsp;&nbsp;loading"); }, // preCall
		function (data) { updateCommentDoneCallBack(data, commentParameters); }, // doneCallBack
		function (error) { alert("failed: " + JSON.stringify(error)); }, // failCallBack
		function () { }
	);
};

var updateCommentDoneCallBack = function (data, commentParameters) {
	if (commentParameters.category === "Customer Communication") {
		$(".comment-dialog-trigger[data-stop-number='" + commentParameters.stopNumber + "']").prev().text(commentParameters.comment);
	} else {
		loadLastCustomerCommunication(commentParameters);
	}
	$("#dialog-window").html(data);
	$("#dialog-window .comment-table tr:even:not(:first)").addClass("comment-table-stripe");
	$("#comment-box").val("");
	$("#dialog-window").dialog({
		title: "ACTIVITY LOG FROM STOP " + commentParameters.stopNumber.toString(),
		width: "550px",
		buttons: [
			{
				text: "Call Logging",
				click: function () {
					window.open("http://sygmanet/Departments/Customer_Service/Call_Logging/CallEntry.aspx", "_blank");
				}
			},
			{
				text: "Close",
				click: function () {
					$(this).dialog("close");
				}
			}
		]
	});
};

var saveComment = function () {
	var comment = $("#comment-box").val();
	var shipTo = $("#comment-box").data("shipTo");
	var billTo = $("#comment-box").data("billTo");
	var centerNumber = $("#comment-box").data("centerNumber");
	var routePlanId = $("#comment-box").data("routePlanId");
	var commentId = $("#comment-box").data("commentId");
	var stopNumber = $("#comment-box").data("stopNumber");
	var screen = $("#comment-box").data("screen");

	var category = $("#comment-category").val();
	var isInternal = $("#comment-is-internal").is(':checked');

	if (comment === undefined || comment === null || comment.toString().trim() === "") {
		if (commentId == 0) { alert("Comment is required."); } else { alert("Comment cannot be removed."); }
	} else {
		var commentParameters = [];
		commentParameters.comment = comment; commentParameters.screen = screen;
		commentParameters.shipTo = shipTo; commentParameters.billTo = billTo;
		commentParameters.centerNumber = centerNumber; commentParameters.routePlanId = routePlanId; commentParameters.stopNumber = stopNumber;
		commentParameters.category = category; commentParameters.isInternal = isInternal;

		if (commentId == 0) {
			createComment(commentParameters);
		} else {
			commentParameters.commentId = commentId;
			updateComment(commentParameters);
		}
	}
};

// --- load comment related methods

var loadLastCustomerCommunication = function (commentParameters) {
	var url = "/routecomment/LastCustomerCommunication?routePlanId=" + commentParameters.routePlanId.toString() + "&centerNumber=" + commentParameters.centerNumber.toString();
	getData(url,
		function () { }, // preCall
		function (data) { loadLastCustomerCommunicationDoneCallBack(data, commentParameters); }, // doneCallBack
		function (error) { alert(error); }, // failCallBack
		function () { }
	);
};

var loadLastCustomerCommunicationDoneCallBack = function (data, commentParameters) {
	if (data && data !== null && data !== "null") {
		var obj = [];
		if (typeof data != 'object') {
			obj = JSON.parse(data);
		} else {
			obj = data;
		}
		$(".comment-dialog-trigger[data-stop-number='" + commentParameters.stopNumber + "']").prev().text(obj.LongComment);
	} else {
		$(".comment-dialog-trigger[data-stop-number='" + commentParameters.stopNumber + "']").prev().text('');
	}
};

// ---

$(document).ready(function () {
	$("#dialog-window").on("click", ".comment-add", function () {
		var shipTo = $(this).data("shipTo");
		var billTo = $(this).data("billTo");
		var centerNumber = $(this).data("centerNumber");
		var routePlanId = $(this).data("routePlanId");
		var stopNumber = $(this).data("stopNumber");
		var screen = $(this).data("screen");

		var url = "/routecomment/create?routePlanId=" + routePlanId.toString() + "&centerNumber=" + centerNumber.toString() + "&shipTo=" + shipTo.toString() + "&billTo=" + billTo.toString() + "&stopNumber=" + stopNumber.toString() + "&screen=" + screen;
		getData(url,
			function () { $("#dialog-window").html("<i class='fa fa-spinner fa-5x fa-spin loading' aria-hidden='true'></i> &nbsp;&nbsp;loading"); }, // preCall
			function (data) {
				$("#dialog-window").html(data);
				$("#dialog-window").dialog({
					buttons: [
						{
							text: "Close",
							click: function () {
								$(this).dialog("close");
							}
						},
						{
							text: "Cancel",
							click: function () {
								loadCommentList(billTo, shipTo, centerNumber, routePlanId, stopNumber, screen);
							}
						},
						{
							text: "Save",
							click: function () {
								// call save comment method here
								saveComment();
							}
						}
					]
				});
			}, // doneCallBack
			function (error) { }, // failCallBack
			function () { }
		);
	});

	$("#dialog-window").on("click", ".comment-edit", function () {
		var shipTo = $(this).data("shipTo");
		var billTo = $(this).data("billTo");
		var centerNumber = $(this).data("centerNumber");
		var routePlanId = $(this).data("routePlanId");
		var category = $(this).data("category");
		var comment = $(this).data("comment");
		var isInternal = $(this).data("isInternal");
		var commentid = $(this).data("commentid");
		var stopNumber = $(this).data("stopNumber");
		var screen = $(this).data("screen");

		var url = "/routecomment/edit/" + commentid + "?routePlanId=" + routePlanId.toString() + "&centerNumber=" + centerNumber.toString() +
			"&shipTo=" + shipTo.toString() + "&billTo=" + billTo.toString() + "&stopNumber=" + stopNumber.toString() +
			"&category=" + category + "&comment=" + comment + "&isInternal=" + isInternal + "&screen=" + screen;
		getData(url,
			function () { $("#dialog-window").html("<i class='fa fa-spinner fa-5x fa-spin loading' aria-hidden='true'></i> &nbsp;&nbsp;loading"); }, // preCall
			function (data) {
				$("#dialog-window").html(data);
				$("#dialog-window").dialog({
					buttons: [
						{
							text: "Close",
							click: function () {
								$(this).dialog("close");
							}
						},
						{
							text: "Cancel",
							click: function () {
								loadCommentList(billTo, shipTo, centerNumber, routePlanId, stopNumber, screen);
							}
						},
						{
							text: "Save",
							click: function () {
								// call save comment method here
								saveComment();
							}
						}
					]
				});
			}, // doneCallBack
			function (error) { }, // failCallBack
			function () { }
		);
	});
});