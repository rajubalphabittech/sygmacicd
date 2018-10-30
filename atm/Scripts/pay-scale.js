$(document).ready(function () {
	$("#sygmaCenterNo").on("change",
		function () {
			var center = $(this).val();
			var scaleId = $("#payScale").val();
			loadPayScaleData(center, scaleId);
		});

	$("#payScale").on("change",
		function () {
			var scaleId = $(this).val();
			var center = $("#sygmaCenterNo").val();
			loadPayScaleData(center, scaleId);
		});
});

var loadPayScaleData = function (center, scaleId) {
	if (center === '' || scaleId === '') {
		$("#rates").html("");
		return;
	}

	$.ajax({
		url: "/payscale/detail?sygmaCenterNo=" + center + "&payScaleId=" + scaleId,
		type: "GET",
		success: function (data) {
			$("#rates").html(data);
		},
		error: function (error) {
			$("#rates").html(error);

		}
	});
}