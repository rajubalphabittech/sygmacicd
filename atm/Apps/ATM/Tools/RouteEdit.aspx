<%@ Page Title="Manage Routes" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Tools_RouteEdit" CodeBehind="RouteEdit.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/RowCountBar.ascx" TagName="RowCountBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ATMColumnOptionsSelector.ascx" TagName="ColumnOptions" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>

	<script src="../../../Scripts/GridviewScroll/gridviewScroll.min.js" type="text/javascript"></script>
	<link href="../../../Scripts/GridviewScroll/GridviewScroll.css" rel="stylesheet" />
	<style type="text/css">
		.hiddencol {
			display: none;
		}

		.showcol {
			display: block;
		}

		.pnlRoutes-custom {
			padding-bottom: 20px;
		}
	</style>
	<script type="text/javascript">

		$(window).resize(function () {
			pageLoad();
		});

		function pageLoad() {
			var gridWidth = $(window).width() - 100;

			$('#<%=gvRoutes.ClientID%>').gridviewScroll({
				width: gridWidth,
				height: 600
			});
			if (document.getElementById('body_gvRoutesPagerBottom') != null && document.getElementById('body_gvRoutesPagerBottom') != undefined) {
				var pagerWidth = document.getElementById('body_gvRoutesPagerBottom').getElementsByTagName('table')[0].offsetWidth;
				var desiredLeftPadding = ((gridWidth / 2) - (pagerWidth / 2));
				document.getElementById('body_gvRoutesPagerBottom').style.width = "100%";
				document.getElementById('body_gvRoutesPagerBottom').style.paddingLeft = desiredLeftPadding.toString() + "px";
			}

		}

		function RemoveRoute(c) {
			var routeid = c;
			//alert(routeid);
			var refreshButton = '#btnRefreshRoutes';
			document.getElementById('<%=btnRefreshRoutes.ClientID%>').fireEvent("onclick");
			PageMethods.PM_RemoveRoute(gUserName, routeid, onSuccessClickButton, onFailed, refreshButton);
			//$(c).dialog("close");
		}

		function OpenRouteDeleteDialog(routeid) {
			$("#routedeletedialog").dialog("open");
			$("#routedeletedialog").attr('routeid', routeid);
			return false;
		}

		function OpenRouteDialog() {
			ClearRouteDialog();
			$("#routedialog").dialog("open");
			$("#routedialog").dialog("option", "title", "Add New Route");
			//$("#txtAddFTNo").focus();
			return false;
		}

		function ClearRouteDialog() {
			$('#txtRouteNo').val("");
			$('#txtRouteName').val("");
			$('#<%=ddAddLocation.ClientID%>').val("");
					$('#<%=ddAddClassification.ClientID%>').val("");
					$('#txtAddMiles').val("");
					$('#<%=ddAddDriver.ClientID%>').val("");
						$('#<%=ddAddDriverHelper.ClientID%>').val("");
						$('#<%=ddAddDriverPayScale.ClientID%>').val("");
						$('#<%=ddAddHelperPayScale.ClientID%>').val("");
						$('#txtAddZipCode').val("");
						$('#<%=ddDepartDay.ClientID%>').val("");
						$('#txtDepartTime').val("");
						$('#txtDuration').val("");
						$("#<%=cbHolidayRoute.ClientID%>").attr('checked', false);
						$('#txtAltRouteNo').val("");
						$('#<%=ddAltDepartDay.ClientID%>').val("");
						$('#txtAltDepartTime').val("");
						$('#txtAltDuration').val("");
				}

				function RouteNoChanged(c) {
					var rotueNoVal = c.value;
					var centerno = GetOption($("#<%=ddSygmaCenterNo.ClientID%>"));
					var centerNoVal = centerno.val();
					AddAJAXRequest();
					PageMethods.PM_ValidateRouteNo(centerNoVal, rotueNoVal, onSuccessValidateRouteNo, onFailed, c);
				}

				function AddRoute(c) {
					var routeNo = $("#txtRouteNo");
					var routeNoVal = routeNo.val();
					var isDuplicate = $("#txtRouteNo").attr('OrigVal');
					var routeName = $("#txtRouteName");
					var routeNameVal = routeName.val();
					var miles = $("#txtAddMiles");
					var milesVal = miles.val();
					var zipCode = $("#txtAddZipCode");
					var zipCodeVal = zipCode.val();
					var zipRegex1 = /^(\d\d\d\d\d)$/;
					var zipRegex2 = /^(\d\d\d\d\d\-\d\d\d\d)$/;
					var location = GetOption($("#<%=ddAddLocation.ClientID%>"));
					var locationId = location.val();
					var type = GetOption($("#<%=ddAddClassification.ClientID%>"));
						var typeId = type.val();
						var driver = GetOption($("#<%=ddAddDriver.ClientID%>"));
						var driverId = driver.val();
						var helper = GetOption($("#<%=ddAddDriverHelper.ClientID%>"));
						var helperId = helper.val();
						var driverpayscale = GetOption($("#<%=ddAddDriverPayScale.ClientID%>"));
						var driverPayScaleId = driverpayscale.val();
						var helperpayscale = GetOption($("#<%=ddAddHelperPayScale.ClientID%>"));
						var helperPayScaleId = helperpayscale.val();
						var centerno = GetOption($("#<%=ddSygmaCenterNo.ClientID%>"));
						var centerNoVal = centerno.val();
						var departDay = GetOption($("#<%=ddDepartDay.ClientID%>"));
						var departDayVal = departDay.val();
						var departTime = $("#txtDepartTime");
						var departTimeVal = departTime.val();
						var duration = $("#txtDuration");
						var durationVal = duration.val();
            //var isHolidayRoute = GetOption($("#<%=cbHolidayRoute.ClientID%>"));
						var isHolidayRouteVal = 0;
						var altRouteNo = $("#txtAltRouteNo");
						var altRouteNoVal = altRouteNo.val();
						var altDepartDay = GetOption($("#<%=ddAltDepartDay.ClientID%>"));
						var altDepartDayVal = altDepartDay.val();
						var altDepartTime = $("#txtAltDepartTime");
						var altDepartTimeVal = altDepartTime.val();
						var altDuration = $("#txtAltDuration");
						var altDurationVal = altDuration.val();
						var routeReg = /^[a-zA-Z0-9]*$/;
						var departTimeReg = /^(([0-1][0-9])|(2[0-3])):([0-5][0-9])$/;

						if (departDayVal == "Choose...") {
							departDayVal = "";
						}

						if ($("#<%=cbHolidayRoute.ClientID%>").attr('checked')) {
							isHolidayRouteVal = 1;
						}

						if (altDepartDayVal == "Choose...") {
							altDepartDayVal = "";
						}

						if (routeNoVal == '') {
							alert("'Route No' is required!");
							$("#txtRouteNo").focus();
							return;
						}

						if (!routeNoVal.match(routeReg)) {
							alert("'Route No' should be Alpha Numeric!");
							$("#txtRouteNo").focus();
							return;
						}

						if (location.val() == "") {
							alert("'Location' is required");
							$("#<%=ddAddLocation.ClientID%>").focus();
							return;
						}

						if (type.val() == "") {
							alert("'Classification' is required");
							$("#<%=ddAddClassification.ClientID%>").focus();
							return;
						}

						if (milesVal == "") {
							alert("'Miles' is required");
							$("#txtAddMiles").focus();
							return;
						}

						if (!milesVal.isNumeric() || milesVal < 0 || milesVal.toString().indexOf('.') != -1) {
							alert("'Miles' should be Positive Whole Number!");
							$("#txtAddMiles").focus();
							return;
						}

//            if (driver.val() == "") {
//                alert("'Driver' is required");
//                $("#<%=ddAddDriver.ClientID%>").focus();
						//                return;
						//            }

						if (driverpayscale.val() == "") {
							alert("'driver pay scale' is required");
							$("#<%=ddAddDriverPayScale.ClientID%>").focus();
							return;
						}

						if (helperpayscale.val() == "") {
							alert("'driver helper pay scale' is required");
							$("#<%=ddAddHelperPayScale.ClientID%>").focus();
							return;
						}

						if (zipCodeVal == "") {
							alert("'Zip Code' is required");
							$("#txtAddZipCode").focus();
							return;
						}

						if (!zipCodeVal.match(zipRegex1) && !zipCodeVal.match(zipRegex2)) {
							alert("Invalid 'Zip Code' format! Please re enter correctly. Ex: 12345 or 12345-6789");
							$("#txtAddZipCode").focus();
							return;
						}

						if (driverId == "") {
							driverId = 0;
						}
						if (helperId == "") {
							helperId = 0;
						}

						if (durationVal == "") {
							durationVal = 0;
						}

						if (driverId == helperId) {
							if (driverId != 0) {
								alert('Default driver and Team-Driver/Helper cannot be the same Employee');
								$('#<%=ddAddDriver.ClientID%>').val("");
								$('#<%=ddAddDriverHelper.ClientID%>').val("");
								$("#ddAddDriverHelper").focus();
								return;
							}
						}

						if (departTimeVal != "") {
							if (!departTimeVal.match(departTimeReg)) {
								alert("Please enter a valid depart time. Eg: 15:30");
								$("#txtDepartTime").focus();
								return;
							}
						}

						if (durationVal != "") {
							if (!durationVal.isNumeric() || durationVal < 0) {
								alert("Please enter a valid duration. Eg: 90");
								$("#txtDuration").focus();
								return;
							}
						}

						if (altDepartTimeVal != "") {
							if (!altDepartTimeVal.match(departTimeReg)) {
								alert("Please enter a valid alt depart time. Eg: 15:30");
								$("#txtAltDepartTime").focus();
								return;
							}
						}

						if (altDurationVal != "") {
							if (!altDurationVal.isNumeric() || altDurationVal < 0) {
								alert("Please enter a valid alt duration. Eg: 90");
								$("#txtAltDuration").focus();
								return;
							}
						}

						if (isHolidayRouteVal == 0 && (altDepartDayVal != "" || altDepartTimeVal != "" || altDurationVal != "" || altRouteNoVal != "")) {
							if (!confirm("IsHolidayRoute Checkbox is not selected. Alt values will not be saved. Do you want to continue?")) {
								return;
							}
						}

						if (isDuplicate == 0) {
							AddAJAXRequest();
							var altDetail = altRouteNoVal + ',' + altDepartDayVal + ',' + altDepartTimeVal + ',' + altDurationVal;
							PageMethods.PM_AddRoute(gUserName, centerNoVal, routeNoVal, routeNameVal, milesVal, locationId, typeId, driverPayScaleId, helperPayScaleId, zipCodeVal, driverId, helperId, departDayVal, departTimeVal, durationVal, isHolidayRouteVal, altDetail, onSuccessAddRoute, onFailed, "#<%=btnRefreshRoutes.ClientID%>");
                //PageMethods.PM_AddRoute(gUserName, centerNoVal, routeNoVal, routeNameVal, milesVal, locationId, typeId, driverPayScaleId, helperPayScaleId, zipCodeVal, driverId, helperId, departDayVal, departTimeVal, durationVal, isHolidayRouteVal, altRouteNoVal, altDepartDay, altDepartTimeVal, altDurationVal, onSuccessAddRoute, onFailed, "#<%=btnRefreshRoutes.ClientID%>");
							$(c).dialog("close");
						}
						else {
							if (confirm('This route already exists.  Would you like to update the existing route with these settings?')) {
								AddAJAXRequest();
								var altDetail = altRouteNoVal + ',' + altDepartDayVal + ',' + altDepartTimeVal + ',' + altDurationVal;
								PageMethods.PM_AddRoute(gUserName, centerNoVal, routeNoVal, routeNameVal, milesVal, locationId, typeId, driverPayScaleId, helperPayScaleId, zipCodeVal, driverId, helperId, departDayVal, departTimeVal, durationVal, isHolidayRouteVal, altDetail, onSuccessAddRoute, onFailed, "#<%=btnRefreshRoutes.ClientID%>");
								$(c).dialog("close");
							}
							else {
								$("#txtRouteNo").focus();
								return;
							}
						}
				}

				function GetOption(c) {
					return $(c).find(":selected");
				}

				function UpdateRouteMiles(c, rid) {
					var val = c.value;
					if (!isNaN(val)) {
						if (val.isNumeric() && val > 0 && val.toString().indexOf('.') == -1) {
							AddAJAXRequest();
							PageMethods.PM_SaveRouteMiles(gUserName, rid, val, onMilesChangeSuccess, onFailed, c);
						}
						else {
							alert('\'Miles\' must be a valid whole number!');
							c.value = $(c).attr('OrigVal');
						}
					} else {
						alert('\'Miles\' must be a valid number!');
						c.value = $(c).attr('OrigVal');
					}
				}

				function UpdateZipCode(c, rid) {
					var val = c.value;
					var zipRegex1 = /^(\d\d\d\d\d)$/;
					var zipRegex2 = /^(\d\d\d\d\d\-\d\d\d\d)$/;
					if (val.match(zipRegex1) || val.match(zipRegex2)) {
						AddAJAXRequest();
						PageMethods.PM_SaveZipCode(gUserName, rid, val, onZipCodeChangeSuccess, onFailed, c);
					}
					else {
						alert('Invalid \'Zip Code\' format! Please re enter correctly. Ex: 12345 or 12345-6789');
						c.value = $(c).attr('OrigVal');
					}
				}

				function UpdateRouteRouteName(c, rid) {
					var val = c.value;
					AddAJAXRequest();
					PageMethods.PM_SaveRouteName(gUserName, rid, val, onChangeSuccess, onFailed);
				}

				function UpdateRouteAltRouteNo(c, rid) {
					var val = c.value;
					AddAJAXRequest();
					PageMethods.PM_SaveAltRouteNo(gUserName, rid, val, onChangeSuccess, onFailed);
				}

				function UpdateRouteDepartDay(c, isAlt, rid) {
					var val = c.value;
					AddAJAXRequest();
					PageMethods.PM_SaveRouteDepartDay(gUserName, rid, isAlt, val, onChangeSuccess, onFailed);
				}

				function UpdateDepartTime(c, isAlt, rid) {
					var departTime = c.value;
					var timeReg = /^(([0-1][0-9])|(2[0-3])):([0-5][0-9])$/;
					if (departTime.match(timeReg)) {
						AddAJAXRequest();
						PageMethods.PM_SaveRouteDepartTime(gUserName, rid, isAlt, departTime, onDepartTimeChangeSuccess, onFailed, c);
						//alert("valid");
					}
					else if (departTime == '') {
						AddAJAXRequest();
						PageMethods.PM_SaveRouteDepartTime(gUserName, rid, isAlt, 'null', onDepartTimeChangeSuccess, onFailed, c);
					}
					else {
						alert("Please enter a valid time. Eg: 15:30");
						c.value = $(c).attr('OrigVal');
					}
				}

				function onDepartTimeChangeSuccess(duration, c) {
					RemoveAJAXRequest();
					$(c).attr('OrigVal', duration);
				}

				function UpdateRouteDetails(c, detailId, rid) {
					var val = parseInt(c.value);
					AddAJAXRequest();
					PageMethods.PM_SaveRouteDetails(gUserName, rid, detailId, val, onChangeSuccess, onFailed);
				}


				function UpdateRouteDuration(c, detailId, rid) {
					var val = c.value;
					if (!isNaN(val)) {
						if (val.toString().indexOf('.') < 0) {
							if (val.length <= 4) {
								AddAJAXRequest();
								if (val == '') {
									val = 0;
								}
								PageMethods.PM_SaveRouteDuration(gUserName, rid, detailId, val, onDurationChangeSuccess, onFailed, c);
							}
							else {
								alert("Duration should be less than 4 digit. Eg: 150 or 1000");
								c.value = $(c).attr('OrigVal');
							}
						}
						else {
							alert("Please enter a valid number. Eg: 150");
							c.value = $(c).attr('OrigVal');
						}
					}
					else {
						alert("Please enter a valid number. Eg: 150");
						c.value = $(c).attr('OrigVal');
					}
				}

				function onDurationChangeSuccess(duration, c) {
					RemoveAJAXRequest();
					$(c).attr('OrigVal', duration);
				}

				function UpdateHolidayRouteFlag(c, rid) {
					AddAJAXRequest();
					if (c.checked) {
						PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
						PageMethods.PM_SaveHolidayRouteFlag(gUserName, rid, 1, onSetHRFlagSuccess, onFailed, c);
					}
					else {
						PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
						PageMethods.PM_SaveHolidayRouteFlag(gUserName, rid, 0, onSetHRFlagSuccess, onFailed, c);
					}
				}

				function UpdateActiveFlag(c, rid) {
					AddAJAXRequest();
					if (c.checked) {
						PageMethods.PM_SaveActiveFlag(gUserName, rid, 1, onChangeSuccess, onFailed, c);
					}
					else {
						PageMethods.PM_SaveActiveFlag(gUserName, rid, 0, onChangeSuccess, onFailed, c);
					}
				}

				function onSetHRFlagSuccess(isHolRoute, c) {
					RemoveAJAXRequest();

					var id = $(c).data("id");

					var altRouteNo = getFirstByIdAndClass(id, 'alt-route-number'); // $get(GetQualifier(c) + 'txtAltRouteNo');
					var altDepartDay = getFirstByIdAndClass(id, 'alt-depart-day'); // $get(GetQualifier(c) + 'ddAltDepartDay');
					var altDuration = getFirstByIdAndClass(id, 'alt-duration'); // $get(GetQualifier(c) + 'txtAltDuration');
					var altDepartTime = getFirstByIdAndClass(id, 'alt-depart-time'); // $get(GetQualifier(c) + 'txtAltDepartTime');

					if (isHolRoute == 1) {
						$(altRouteNo).prop("disabled", false);
						$(altDepartDay).prop("disabled", false);
						$(altDuration).prop("disabled", false);
						$(altDepartTime).prop("disabled", false);
					}
					else {
						$(altRouteNo).val('');
						//altDepartDay.selectedIndex = 0;
						$("#" + altDepartDay.id + " option:selected").prop("selected", false);
						$(altDuration).val('');
						$(altDepartTime).val('');

						$(altRouteNo).prop("disabled", true);
						$(altDepartDay).prop("disabled", true);
						$(altDuration).prop("disabled", true);
						$(altDepartTime).prop("disabled", true);
					}
				}

				function onChangeSuccess() {
					RemoveAJAXRequest();
				}

				function onSuccessValidateRouteNo(isDuplicate, c) {
					RemoveAJAXRequest();
					if (isDuplicate[0] == "1") {
						$(c).attr('OrigVal', 1);
					}
					else {
						$(c).attr('OrigVal', 0);
					}

					if (isDuplicate[1] != "No Match") {
						$('#txtAddZipCode').val(isDuplicate[1]);
					}
					else {
						$('#txtAddZipCode').val("");
					}
				}

				function onMilesChangeSuccess(bl, c) {
					$(c).attr('OrigVal', bl);
					RemoveAJAXRequest();
				}

				function onZipCodeChangeSuccess(bl, c) {
					var id = $(c).data("id");
					var val = c.value;
					$(c).attr('OrigVal', val);
					var lblCity = $('.city-state-label[data-id="' + id + '"]');
					lblCity.innerHTML = bl;
					RemoveAJAXRequest();
				}

				function onSuccessAddRoute(rv, button) {
					RemoveAJAXRequest();
					//alert(button);
					//alert(rv);
					if (rv == "1") {
						alert("Route added successfully");
						$(button).click();
					}
					else {
						alert("'Route No' already exists! Route is not added.");
					}
            //document.getElementById("btnRefreshRoutes").click();
            //document.getElementById('<%=btnRefreshRoutes.ClientID%>').fireEvent("onclick");
				}

				function onSuccessClickButton(rv, button) {
					RemoveAJAXRequest();
					alert(button);
					$(button).click();
            //document.getElementById("btnRefreshRoutes").click();
            //document.getElementById('<%=btnRefreshRoutes.ClientID%>').fireEvent("onclick");
				}

				function onFailed(error) {
					RemoveAJAXRequest();
					alert('There was an error saving the field.  Please refresh the page and try again.');
				}

				$(document).ready(function () {
					var showIcon = "../../../Images/Icons/show.png";
					var hideIcon = "../../../Images/Icons/hide.png";
					$("#routedialog").dialog({
						height: 580
						, width: 400
						, autoOpen: false
						, closeOnEscape: true
                        , modal: true
						, resizable: false
						, buttons: {
							"Ok": function () { AddRoute(this); }
							, "Cancel": function () { $(this).dialog("close"); }
						}
					});

					$("#columnOptionsDialog").dialog({
						height: 400
						, width: 565
						, autoOpen: false
						, closeOnEscape: true
                        , modal: true
						, resizable: false
						, buttons: {
							"Ok": {
								text: "Ok",
								id: "columnOptionsDialogOk",
								click: function () { SaveColumnOptions(this); }
							}
							, "Cancel": {
								text: "Cancel",
								id: "columnOptionsDialogCancel",
								click: function () { $(this).dialog("close"); }
							}
						}
					});
				});

				function ddAddDriver_onchange(b) {
					var helperC = GetOption($("#<%=ddAddDriverHelper.ClientID%>"));
					var helper = helperC.val();
					var driver = b.value;
					if ((helper.length != 0 && driver == helper)) {
						alert("Default driver and TeamDriver/Helper cannot be the same Employee, resetting the Helper selection");
						$('#<%=ddAddDriverHelper.ClientID%>').val("");
					}
					else if (helper.length != 0 && driver == 0) {
						alert("Helper cannot be selected without a driver, resetting the Helper selection");
						$('#<%=ddAddDriverHelper.ClientID%>').val("");
					}


				}
				function ddAddDriverHelper_onchange(b) {
					var helper = b.value;
					var driverC = GetOption($("#<%=ddAddDriver.ClientID%>"));
					var driver = driverC.val();
					if (driver == helper && driver != 0) {
						alert('Default driver and TeamDriver/Helper cannot be the same Employee');
                //$('#<%=ddAddDriver.ClientID%>').val("");
							$('#<%=ddAddDriverHelper.ClientID%>').val("");
							$('#<%=ddAddDriver.ClientID%>').focus;
						}
						if (driver == 0 && helper != 0) {
							alert("Please select driver before selecting helper");
							$('#<%=ddAddDriverHelper.ClientID%>').val("");
						}

				}

				function OpenColumnOptionsDialog() {
					$("#columnOptionsDialog").dialog("open");
					$("#columnOptionsDialog").dialog("option", "title", "Column Options");

					LoadColumnOptionsUserControl("MANAGEROUTE");

					return false;
				}

				function SaveColumnOptions(columnOptionsDialog) {
					var successfulSave = SaveColumnOptionsUserControl("MANAGEROUTE");

					if (successfulSave) {
						$(columnOptionsDialog).dialog("close");
						location.reload();
					} else {
						alert("Error saving user column preferences");
					}
				}
//        function UpdateHelper(b, c) {
//            alert('driver');
//            alert(b.toString());
//            alert(c.toString());
//        }
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
	</asp:ScriptManager>
	<div style="width: 100%" class="container">
		<div class="pageTitle">
			Manage Routes
		</div>
		<div class="pageSubtitle">
			View and Modify Planned Route Details
		</div>
		<div class="panel panel-default">
			<div class="panel-heading">
				<div class="input-group">
					<div class="input-group input-group-sm">
						<div class="row">
							<div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
								<label for="ddSygmaCenterNo">
									Center
								</label>
								<asp:DropDownList ID="ddSygmaCenterNo" runat="server" DataTextField="CenterDisplay" DataValueField="SygmaCenterNo" AutoPostBack="True"
									OnSelectedIndexChanged="ddSygmaCenterNo_SelectedIndexChanged" CssClass="form-control">
								</asp:DropDownList>
								<asp:Label ID="lblCenter" runat="server" Visible="False"></asp:Label>
							</div>
							<div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
								<label>
									Route Search
								</label>
								<asp:TextBox ID="txtRouteSearch" runat="server" OnTextChanged="txtRouteSearch_TextChanged" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
							</div>
							<div class="col-xs-3 col-sm-2 col-md-2 col-lg-2">
								<a href="javascript: void(0);" onclick='OpenColumnOptionsDialog();' id="anchorColumnOptions" class="btn btn-secondary btn-sm">Column Options
								</a>
							</div>
						</div>
					</div>
				</div>
			</div>

			<div class="panel panel-body" id="pnlProgression" runat="server" visible="false">
				<asp:UpdatePanel ID="upSettings" runat="server" UpdateMode="Conditional">
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnRefreshRoutes" EventName="Click" />
					</Triggers>
					<ContentTemplate>
						<asp:Button ID="btnRefreshRoutes" runat="server" Text="Refresh" OnClick="btnRefreshRoutes_Click" Style="display: none" />
						<asp:Panel ID="pnlRoutes" runat="server" Visible="false" CssClass="pnlRoutes-custom">
							<div class="inlineBlock" style="text-align: right;">
								<a href="javascript: void(0);" onclick="OpenRouteDialog();">
									<asp:Image ID="imgAddRoute" runat="server" ImageUrl="~/Images/Icons/document_add_32.png" ToolTip="Add New Route" />
								</a>
							</div>
							<uc1:RowCountBar ID="RowCountBar1" runat="server" Width="100%" OnPageSizeChanged="RowCountBar1_PageSizeChanged" />
							<asp:GridView ID="gvRoutes" runat="server" AllowSorting="true" PageSize="20"
								AllowPaging="true" AutoGenerateColumns="false" Width="100%" OnRowDataBound="gvRoutes_RowDataBound"
								OnSorting="gvRoutes_Sorting" OnRowCommand="GridView1_RowCommand"
								OnPageIndexChanging="gvRoutes_PageIndexChanging"
								CssClass="table-responsive table-striped table-hover"
								EmptyDataText="No Routes exist for this search." OnInit="gvRoutes_Init">
								<Columns>
									<asp:BoundField HeaderText="Route" DataField="RouteNo" SortExpression="RouteNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="Wrap" />
									<asp:TemplateField SortExpression="RouteName" HeaderText="Route Name" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtRouteName" runat="server" class="form-control-custom minWidthGridColumn-lg" ControlStyle-Width="100%"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Origin (Location)*" SortExpression="LocationId" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddLocation" runat="server" class="form-control-custom" DataTextField="LocationName" DataValueField="LocationId" Style="text-align: right"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Classification" SortExpression="ClassificationId" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddClassification" runat="server" DataTextField="ClassificationName" DataValueField="ClassificationId" Style="text-align: right" class="form-control-custom minWidthGridColumn-lg"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField SortExpression="Miles" HeaderText="Miles" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtMiles" runat="server" class="form-control-custom"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Default Driver" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddDriver" runat="server" DataTextField="DriverName" DataValueField="DriverId" OnSelectedIndexChanged="ddDriver_SelectedIndexChanged" AutoPostBack="true" Style="text-align: right" class="form-control-custom minWidthGridColumn-lg"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Default Team Driver/Helper" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddHelper" runat="server" DataTextField="DriverName" DataValueField="DriverId" OnSelectedIndexChanged="ddHelper_SelectedIndexChanged" AutoPostBack="true" Style="text-align: right" class="form-control-custom minWidthGridColumn-lg"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Driver Pay Scale*" SortExpression="DriverPayScaleID" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddDriverPayScale" runat="server" DataTextField="PayScaleDesignator" DataValueField="PayScaleId" Style="text-align: right"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Driver Helper Pay Scale*" SortExpression="HelperPayScaleID" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddHelperPayScale" runat="server" DataTextField="PayScaleDesignator" DataValueField="PayScaleId" Style="text-align: right" class="form-control-custom"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Zip Code" SortExpression="ZipCode" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtZipCode" runat="server" MaxLength="10" class="form-control-custom"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="City/State" SortExpression="CityState" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:Label ID="lblCityState" runat="server" class="form-control-custom city-state-label"></asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Depart Day" SortExpression="DepartDay" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddDepartDay" runat="server" Style="text-align: right" OnSelectedIndexChanged="ddDepartDay_SelectedIndexChanged" AutoPostBack="true" class="form-control-custom minWidthGridColumn-med"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Depart Time hh:mm" SortExpression="DepartTimeHour" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtDepartTime" runat="server" ToolTip="Format - hh:mm" class="form-control-custom"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Duration" SortExpression="Duration" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtDuration" runat="server" class="form-control-custom"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Is Holiday Route" ItemStyle-CssClass="Wrap">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:CheckBox ID="cbHolidayRoute" runat="server" class="form-control-custom" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField SortExpression="AltRouteNo" HeaderText="Alt Route No" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtAltRouteNo" runat="server" MaxLength="4" Enabled="false" CssClass="form-control-custom alt-route-number"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Alt Depart Day" SortExpression="AltDepartDay" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:DropDownList ID="ddAltDepartDay" runat="server" Style="text-align: right" Enabled="false" CssClass="form-control-custom minWidthGridColumn-med alt-depart-day"></asp:DropDownList>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Alt Depart Time hh:mm" SortExpression="AltDepartTime" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtAltDepartTime" runat="server" ToolTip="Format - hh:mm" Enabled="false" CssClass="form-control-custom alt-depart-time"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Alt Duration" SortExpression="AltDuration" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:TextBox ID="txtAltDuration" runat="server" Enabled="false" CssClass="form-control-custom alt-duration"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Is Active" ItemStyle-CssClass="Wrap">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:CheckBox ID="cbIsActive" runat="server" class="form-control-custom" />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Remove" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
										<ItemStyle HorizontalAlign="Center" />
										<ItemTemplate>
											<asp:LinkButton ID="lnkBtnDel" runat="server" CommandName="DeleteRow" OnClientClick="return confirm('Are you sure you want to Delete this record?');" class="form-control-custom" CommandArgument='<%#Eval("RouteDetailId") %>'>Delete</asp:LinkButton>
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
								<HeaderStyle CssClass="GridviewScrollHeader-custom" />
								<PagerStyle CssClass="pagination-cs" />
							</asp:GridView>
						</asp:Panel>
						</div>
                                    </div>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
		</div>
	</div>
	<div id="routedialog">
		<asp:UpdatePanel ID="upAddRoute" runat="server">
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="ddAddDriver" EventName="SelectedIndexChanged" />
				<asp:AsyncPostBackTrigger ControlID="ddAddDriverHelper" EventName="SelectedIndexChanged" />
			</Triggers>
			<ContentTemplate>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px;">
						Route No:
					</div>
					<input type="text" id="txtRouteNo" style="width: 100px" maxlength="4" class="email" onchange="RouteNoChanged(this);" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px;">
						Route Name(Optional):
					</div>
					<input type="text" id="txtRouteName" style="width: 100px" maxlength="100" class="email" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Location:
					</div>
					<asp:DropDownList ID="ddAddLocation" runat="server">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Classification:
					</div>
					<asp:DropDownList ID="ddAddClassification" runat="server">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Miles:
					</div>
					<input type="text" id="txtAddMiles" style="width: 60px" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Default Driver:
					</div>
					<asp:DropDownList ID="ddAddDriver" runat="server" onchange="ddAddDriver_onchange(this);">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Default Team Driver/Helper:
					</div>
					<asp:DropDownList ID="ddAddDriverHelper" runat="server" onchange="ddAddDriverHelper_onchange(this);">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Driver Pay Scale:
					</div>
					<asp:DropDownList ID="ddAddDriverPayScale" runat="server">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Driver Helper Pay Scale:
					</div>
					<asp:DropDownList ID="ddAddHelperPayScale" runat="server">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Zip Code:
					</div>
					<input type="text" id="txtAddZipCode" style="width: 70px" maxlength="10" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Depart Day:
					</div>
					<asp:DropDownList ID="ddDepartDay" runat="server">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Depart Time:
					</div>
					<input type="text" id="txtDepartTime" style="width: 50px" maxlength="5" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Duration:
					</div>
					<input type="text" id="txtDuration" style="width: 50px" maxlength="5" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Is Holiday Route:
					</div>
					<asp:CheckBox ID="cbHolidayRoute" runat="server" Width="90px" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Alt Route No:
					</div>
					<input type="text" id="txtAltRouteNo" style="width: 100px" maxlength="4" class="email" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Alt Depart Day:
					</div>
					<asp:DropDownList ID="ddAltDepartDay" runat="server">
					</asp:DropDownList>
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Alt Depart Time:
					</div>
					<input type="text" id="txtAltDepartTime" style="width: 50px" maxlength="5" />
				</div>
				<div style="margin-bottom: 5px">
					<div class="inlineBlock" style="font-weight: bold; width: 100px">
						Alt Duration:
					</div>
					<input type="text" id="txtAltDuration" style="width: 50px" maxlength="5" />
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>

	<div id="columnOptionsDialog">
		<uc2:ColumnOptions ID="ColumnOptionsData" runat="server" PageName="MANAGEROUTE" />
	</div>

	<asp:UpdateProgress ID="upGrid" runat="server" AssociatedUpdatePanelID="upSettings" DisplayAfter="100">
		<ProgressTemplate>
			<div class="disableBackground">
			</div>
			<div class="progressPane loading">
				<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
					Please Wait...
				</div>
				<asp:Image ID="imgGridProgress" runat="server" ImageUrl="~/Images/animated_bar.gif" />
			</div>

		</ProgressTemplate>
	</asp:UpdateProgress>
</asp:Content>

