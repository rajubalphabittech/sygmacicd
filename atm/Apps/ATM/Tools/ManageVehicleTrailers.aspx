<%@ Page Title="Manage Vehicles & Trailers" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Tools_ManageVehicleTrailers" CodeBehind="ManageVehicleTrailers.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<%@ Register Src="../../../UserControls/VehicleTrailerRowCountBar.ascx" TagName="VehicleTrailerRowCountBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ATMColumnOptionsSelector.ascx" TagName="ColumnOptions" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
	<script src="/Scripts/GridviewScroll/gridviewScroll.min.js" type="text/javascript"></script>
	<link href="/Scripts/GridviewScroll/GridviewScroll.css" rel="stylesheet" />
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

		.form-control-custom {
			height: 20px;
		}

		.maxWidthGridColumn160 {
			max-width: 160px;
			overflow: hidden;
		}

		.maxWidthGridColumn120 {
			max-width: 120px;
			overflow: hidden;
		}
	</style>
	<script type="text/javascript">

		$(window).resize(function () {
			pageLoad();
		});

		function pageLoad() {
			var gridWidth = $(window).width() - 100;

			$('#<%=gvVT.ClientID%>').gridviewScroll({
				width: gridWidth,
				height: 600
			});

			if (document.getElementById('body_gvVTPagerBottom') != null && document.getElementById('body_gvVTPagerBottom') != undefined) {
				var pagerWidth = document.getElementById('body_gvVTPagerBottom').getElementsByTagName('table')[0].offsetWidth;
				var desiredLeftPadding = ((gridWidth / 2) - (pagerWidth / 2));
				document.getElementById('body_gvVTPagerBottom').style.width = "100%";
				document.getElementById('body_gvVTPagerBottom').style.paddingLeft = desiredLeftPadding.toString() + "px";
			}

		}


		var gCenter;
		function UpdateVehicleTrailerDetails(c, fieldId, vid, isVehicle) {
			var val = c.value;
			AddAJAXRequest();
			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveVehicleTrailerDetails(gUserName, vid, fieldId, val, isVehicle, onChangeSuccess, onFailed);
		}

		function UpdateVehicleTrailerMake_Type_Status_ObjType(c, fieldId, vid, isVehicle) {
			var id = parseInt(c.value);
			AddAJAXRequest();
			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveVehicleTrailerMake_Type_Status_ObjType(gUserName, vid, fieldId, id, isVehicle, onChangeSuccess, onFailed);
		}

		function UpdateVehicleTrailerCenter(c, vid, isVehicle) {
			var vt = "";
			if (isVehicle) {
				vt = "Vehicle";
			}
			else {
				vt = "Trailer";
			}
			if (confirm('Are you sure you want to change the Center for this ' + vt + '? This will be applicable for whole system.')) {
				var id = parseInt(c.value);
				gCenter = id;
				AddAJAXRequest();
				PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
				PageMethods.PM_SaveVehicleTrailerCenter(gUserName, vid, id, isVehicle, updateVehicleSuccess(vid, id), onFailed);

			}
			else {
				c.value = gCenter;
			}
		}

		function SaveOriginalCenter(c) {
			gCenter = c.value;
		}

		function UpdateVehicleTrailerActiveChanged(c, vid, isVehicle) {
			AddAJAXRequest();
			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveVehicleTrailerActiveChanged(gUserName, vid, c.checked, isVehicle, onChangeSuccess, onFailed);
		}

		var updateVehicleSuccess = function (id, centerNo) {
			var lbl = getFirstByIdAndClass(id, 'center-number-label');
			lbl.innerHTML = centerNo;
			onChangeSuccess();
		}

		function onChangeSuccess() {
			RemoveAJAXRequest();
		}

		function onFailed(error) {
			RemoveAJAXRequest();
			alert('There was an error saving the field.  Please refresh the page and try again.');
		}

		function OpenAddRentalVehicle() {
			OpenWindow('AddVehicle.aspx?isRental=true', 925, 300, 1, 1, 1, 1, 'atm_add_Rental_Vehicle');
		}

		function OpenAddVehicle() {
			OpenWindow('AddVehicle.aspx?isRental=false', 925, 300, 1, 1, 1, 1, 'atm_add_Vehicle');
		}

		function OpenAddTrailer() {
			OpenWindow('AddTrailer.aspx', 925, 300, 1, 1, 1, 1, 'atm_add_Trailer');
		}
		$(document).ready(function () {
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

		function OpenColumnOptionsDialog() {
			$("#columnOptionsDialog").dialog("open");
			$("#columnOptionsDialog").dialog("option", "title", "Column Options");

			LoadColumnOptionsUserControl("MANAGEVEHICLESTRAILERS");

			return false;
		}

		function SaveColumnOptions(columnOptionsDialog) {
			var successfulSave = SaveColumnOptionsUserControl("MANAGEVEHICLESTRAILERS");

			if (successfulSave) {
				$(columnOptionsDialog).dialog("close");
				location.reload();
			} else {
				alert("Error saving user column preferences");
			}
		}

	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
	</asp:ScriptManager>
	<div class="container" style="width: 100%">
		<div class="pageTitle inlineBlock">
			Manage Vehicles & Trailers
		</div>
		<div class="pageSubtitle">
			View Transportation Equipment Details
		</div>
		<asp:UpdatePanel ID="upSettings" runat="server" UpdateMode="Conditional">
			<Triggers>
				<asp:PostBackTrigger ControlID="btnExport" />
			</Triggers>
			<ContentTemplate>
				<div class="panel panel-default">
					<div class="panel-heading">
						<div class="form-group">
							<div class="row">
								<div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
									<label for="ddProgSygmaCenterNo">Center</label>
									<asp:DropDownList ID="ddProgSygmaCenterNo" CssClass="form-control" runat="server" DataTextField="Center" DataValueField="SygmaCenterNo" AutoPostBack="True"
										OnSelectedIndexChanged="ddProgSygmaCenterNo_SelectedIndexChanged">
									</asp:DropDownList>
									<asp:RequiredFieldValidator
										ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddProgSygmaCenterNo"
										ErrorMessage="Please select one center" Display="Dynamic">
									</asp:RequiredFieldValidator>
								</div>
								<div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
									<label for="txtProgName">
										Search
									</label>
									<asp:TextBox ID="txtProgName" CssClass="form-control" runat="server" OnTextChanged="txtProgName_TextChanged" AutoPostBack="True"></asp:TextBox>
								</div>
								<div class="col-xs-6 col-sm-6 col-md-5 col-lg-5">
									<div class="btn-group btn-group-sm">
										<a href="javascript: void(0);" role="button" class="btn btn-secondary" onclick='OpenAddTrailer();'>Add Trailer</a>
										<%--<a href="javascript: void(0);" role="button" class="btn btn-secondary" onclick='OpenAddVehicle();'>Add Vehicle</a>--%>
										<a href="javascript: void(0);" role="button" class="btn btn-secondary" onclick='OpenAddRentalVehicle();'>Add Rental Vehicle</a>
										<a href="javascript: void(0);" role="button" class="btn btn-secondary" onclick='OpenColumnOptionsDialog();' id="anchorColumnOptions">Column Options</a>
									</div>
								</div>

							</div>
						</div>
						<div class="row">
							<div class="col-lg-2">
								<asp:ImageButton ID="btnExport" runat="server" ImageUrl="../../../Images/excel6.png" OnClick="btnExport_Click" CausesValidation="true" Autopostback="true" />
							</div>
						</div>
					</div>
					<div class="panel panel-body" id="pnlProgression" runat="server" visible="false">
						<uc1:VehicleTrailerRowCountBar ID="VehicleTrailerRowCountBar1" runat="server" Width="100%" IncludeAllSelection="True" OnPageSizeChanged="VehicleTrailerRowCountBar1_PageSizeChanged" />
						<asp:GridView ID="gvVT" runat="server" AllowSorting="true" AutoGenerateColumns="false" Width="100%"
							OnRowDataBound="gvVT_RowDataBound" OnSorting="gvVT_Sorting" AllowPaging="true" OnPageIndexChanging="gvVT_PageIndexChanging"
							EmptyDataText="No vehicles and trailers exist for this search." OnInit="gvVehiclesTrailers_Init">
							<Columns>
								<asp:BoundField HeaderText="VehicleName" DataField="VehicleName" SortExpression="VehicleName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%" />
								<asp:BoundField HeaderText="Unit#" DataField="UnitNo" SortExpression="UnitNo" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%" />
								<asp:TemplateField HeaderText="Center" ItemStyle-CssClass="Wrap" HeaderStyle-Width="100%">
									<ItemStyle HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:DropDownList ID="ddSygmaCenter" runat="server" DataTextField="Center" DataValueField="SygmaCenterNo" Style="text-align: right"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Center #" SortExpression="SygmaCenterNo" ItemStyle-CssClass="Wrap" HeaderStyle-Width="100%">
									<ItemTemplate>
										<asp:Label ID="lblCenterNumber" runat="server" HorizontalAlign="Center" CssClass="center-number-label"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Unit Attached #" SortExpression="UnitAttachedId" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%">
									<ItemStyle HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:TextBox ID="txtUnitAttached" runat="server" MaxLength="20"></asp:TextBox>
										<asp:Label ID="lblUnitAttached" runat="server" Visible="false">NA</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Make" SortExpression="Make" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" Width="100px" />
									<ItemTemplate>
										<asp:DropDownList ID="ddMake" runat="server" DataTextField="VehicleMake" DataValueField="MakeId" Style="text-align: left"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="VIN" SortExpression="VIN" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%" ControlStyle-CssClass="minWidthGridColumn-sm">
									<ItemStyle HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:TextBox ID="txtVIN" runat="server" Style="text-align: left" MaxLength="50"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Replaced" SortExpression="ReplacedYear" ItemStyle-CssClass="Wrap">
									<ItemStyle Width="100px" HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:DropDownList ID="ddReplacedYear" runat="server" DataTextField="ReplacedYear" DataValueField="ReplacedId" Width="100px" Style="text-align: left"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Model" SortExpression="Model" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%" ControlStyle-CssClass="minWidthGridColumn-sm">
									<ItemStyle HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:TextBox ID="txtModel" runat="server" Minimum-Width="80" Style="text-align: left" MaxLength="50"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Cab Type" SortExpression="Type" ItemStyle-CssClass="Wrap">
									<ItemStyle Width="100px" HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:DropDownList ID="ddType" runat="server" DataTextField="VehicleType" DataValueField="TypeId" Width="100px" Style="text-align: left"></asp:DropDownList>
										<asp:Label ID="lblType" runat="server" Visible="false">NA</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-CssClass="Wrap">
									<ItemStyle Width="100px" HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:DropDownList ID="ddStatus" runat="server" DataTextField="VehicleStatus" DataValueField="StatusId" Width="100px" Style="text-align: left"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Year" SortExpression="Year" ItemStyle-CssClass="Wrap">
									<ItemStyle Width="80px" HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:DropDownList ID="ddYear" runat="server" DataTextField="Year" DataValueField="Id" Style="text-align: left"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-CssClass="Wrap" ControlStyle-Width="100%" ControlStyle-CssClass="minWidthGridColumn-lg">
									<ItemStyle HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:TextBox ID="txtDescription" runat="server" Style="text-align: left"></asp:TextBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Object Type" SortExpression="Type" ItemStyle-CssClass="Wrap">
									<ItemStyle Width="100px" HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:DropDownList ID="ddObjType" runat="server" DataTextField="VehicleObjectType" DataValueField="ObjectTypeId" Width="100px" Style="text-align: left"></asp:DropDownList>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField HeaderText="Odometer/Hours" DataField="Odometer" SortExpression="Odometer" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
								<asp:BoundField HeaderText="Odometer Last Updated" DataField="OdometerAsOfDate" DataFormatString="{0:M/d/yyyy}" SortExpression="OdometerAsOfDate" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
								<asp:TemplateField HeaderText="Active" SortExpression="Active" HeaderStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
									<ItemTemplate>
										<asp:CheckBox ID="chkActive" runat="server" Width="40px" Enabled="true"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Depreciation" SortExpression="Active">
									<ItemStyle HorizontalAlign="Center" CssClass="Wrap" />
									<ItemTemplate>
										<asp:HyperLink ID="hlDepreciation" runat="server" Text="Click" Target="_blank"
											NavigateUrl='<%# string.Format("~/Apps/ATM/Tools/DepreciationInfo.aspx?vid={0}&isv={1}", 
                                                    HttpUtility.UrlEncode(Eval("VehicleId").ToString()), HttpUtility.UrlEncode(Eval("IsVehicle").ToString())) %>'>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Additional Info" SortExpression="Active">
									<ItemStyle HorizontalAlign="Center" CssClass="Wrap" />
									<ItemTemplate>
										<asp:HyperLink ID="hlAdditionalInfo" runat="server" Text="Click" Target="_blank"
											NavigateUrl='<%# string.Format("~/Apps/ATM/Tools/AdditionalInfo.aspx?vid={0}&isv={1}", 
                                                    HttpUtility.UrlEncode(Eval("VehicleId").ToString()), HttpUtility.UrlEncode(Eval("IsVehicle").ToString())) %>'>
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
							<HeaderStyle CssClass="GridviewScrollHeader-custom" />
							<PagerStyle CssClass="pagination-cs" />
						</asp:GridView>
					</div>
				</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
		<div id="columnOptionsDialog" visible="false">
			<uc2:ColumnOptions ID="ColumnOptionsData" runat="server" PageName="MANAGEVEHICLESTRAILERS" />
		</div>

		<asp:UpdateProgress ID="upGrid" runat="server" AssociatedUpdatePanelID="upSettings" DisplayAfter="100">
			<ProgressTemplate>
				<div class="disableBackground">
				</div>
				<div class="progressPane loading">
					<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
						Loading, please Wait...
					</div>
					<asp:Image ID="imgGridProgress" runat="server" ImageUrl="~/Images/animated_bar.gif" />
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>

	</div>
</asp:Content>
