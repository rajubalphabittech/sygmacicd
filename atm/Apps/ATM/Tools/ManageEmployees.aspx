<%@ Page Title="Manage Employees" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Tools_ManageEmployees" CodeBehind="ManageEmployees.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/JScript.ascx" TagName="JScript" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ATMColumnOptionsSelector.ascx" TagName="ColumnOptions" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="google" content="notranslate"/>
    <meta http-equiv="Content-Language" content="en"/>

	<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
	<script src="/Scripts/jquery-ui.min.js" type="text/javascript"></script>
	<style type="text/css">
		.hiddencol {
			display: none;
		}

		.showcol {
			display: block;
		}
	</style>
	<script type="text/javascript">

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

		function UpdateProgressionRate(c, eid) {
			var val = parseFloat(c.value);

			if (!isNaN(val)) {
				if (val.toString().indexOf('.') > -1) {
					var scale = val.toString().length - val.toString().indexOf('.') - 1;
					if (scale < 5) {
						val = val.toFixed(scale);
					}
					else {
						val = val.toFixed(4);
					}
					c.value = val;
				}
				AddAJAXRequest();
				PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
				PageMethods.PM_SaveProgressionRate(gUserName, eid, val, onChangeSuccess, onFailed, c);
			} else {
				alert('\'Rate\' must be a valid percentage!');
			}
		}

		function UpdateGuaranteedPay(c, eid) {
			var val = parseFloat(c.value);
			if (!isNaN(val)) {
				if (val.toString().indexOf('.') > -1) {
					val = val.toFixed(2)
					c.value = val;
				}
				AddAJAXRequest();
				PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
				PageMethods.PM_SaveGuaranteedPay(gUserName, eid, val, onGPChangeSuccess, onFailed, c);
			} else {
				alert('\'Guaranteed Pay\' must be a valid number!');
				c.value = $(c).attr('OrigVal');
			}
		}

		function UpdateEffectiveHireDate(c, eid) {
			var effHireDate = c.value;
			var dateReg = /^([1-9]|0[1-9]|1[012])[/]([1-9]|0[1-9]|[12][0-9]|3[01])[/](19|20)\d\d$/;
			if (effHireDate.match(dateReg)) {
				AddAJAXRequest();
				PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
				PageMethods.PM_SaveEffectiveHireDate(gUserName, eid, effHireDate, onEHDChangeSuccess, onEHDateFailed, c);
			}
			else {
				alert("\'Effective Hire Date\' is not valid!");
				c.value = $(c).attr('OrigVal');
			}
		}

		function ApplyEHDateChanged(c, eid) {
			var id = $(c).data("id");
			var eHireDate = getFirstByIdAndClass(id, 'eff-hire-date');//  $('.eff-hire-date[data-id="' + id + '"]');

			if (c.checked) {
				$(eHireDate).prop("disabled", false);  // .disabled = false;

			} else {
				$(eHireDate).prop("disabled", true); //.disabled = true;
			}
			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveApplyEHDateChanged(gUserName, eid, c.checked, onEHDateFlagSuccess, onEHDateFailed, c);
		}

		function ApsExceptionChanged(c, rid) {
			var id = $(c).data("id");

			var txtPR = getFirstByIdAndClass(id, 'prog-rate');// $('.prog-rate[data-id="' + id + '"]')[0];
			var lblPR = getFirstByIdAndClass(id, 'prog-rate-label');// $('.prog-rate-label[data-id="' + id + '"]')[0];

			if (c.checked) {
				$(txtPR).visible = true; // .css({ "display": "block"});
				$(lblPR).visible = false; //.css({"display": "none"});
			} else {
				$(txtPR).visible = false; //.css({ "display": "none" });
				$(lblPR).visible = true; //.css({ "display": "block" });
			}
			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveApsExceptionEnabled(gUserName, rid, c.checked, onSuccess, onfocus);
		}

		function GpExceptionChanged(c, rid) {
			var id = $(c).data("id");

			var txtGP = $('.guaranteed-pay[data-id="' + id + '"]')[0];
			if (c.checked) {
				txtGP.disabled = false;
				txtGP.style.backgroundColor = "White";
			} else {
				txtGP.disabled = true;
				txtGP.style.backgroundColor = "lightgrey";
				$('.guaranteed-pay[data-id="' + id + '"]')[0].value = '';
			}
			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveGpExceptionEnabled(gUserName, rid, c.checked, onSuccess, onfocus);
		}

		function UpdateEmployeeClassification(c, eid) {
			var val = parseInt(c.value);
			AddAJAXRequest();
			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveEmployeeClassification(gUserName, eid, val, onSuccess, onFailed, c);
		}

		function onChangeSuccess(bl, c) {
			var id = $(c).data("id");

			var lblRT = getFirstByIdAndClass(id, 'prog-rate-label');
			lblRT.innerText = $(c).val();
			RemoveAJAXRequest();
		}

		function onGPChangeSuccess(bl, c) {
			$(c).attr('OrigVal', bl);
			RemoveAJAXRequest();
		}

		function onEHDChangeSuccess(bl, c) {
			var id = $(c).data("id");
			var lblTen = $('.tenure-label[data-id="' + id + '"]')[0];
			var txtPR = $('.prog-rate[data-id="' + id + '"]')[0];
			var lblPR = $('.prog-rate-label[data-id="' + id + '"]')[0];

			$(c).attr('OrigVal', bl[0]);
			lblTen.innerHTML = bl[1];
			txtPR.value = bl[2];
			txtPR.setAttribute('OrigVal', bl[2]);
			lblPR.innerHTML = bl[2];
			RemoveAJAXRequest();
		}

		function onSuccess() {
			RemoveAJAXRequest();
		}

		function onEHDateFlagSuccess(msg, c) {
			var id = $(c).data("id");
			var txtHD = $('.eff-hire-date[data-id="' + id + '"]')[0];
			var lblTen = $('.tenure-label[data-id="' + id + '"]')[0];
			var txtPR = $('.prog-rate[data-id="' + id + '"]')[0];
			var lblPR = $('.prog-rate-label[data-id="' + id + '"]')[0];

			txtHD.value = msg[0];
			txtHD.setAttribute('OrigVal', msg[0]);

			lblTen.innerHTML = msg[1];

			txtPR.value = msg[2];
			txtPR.setAttribute('OrigVal', msg[2]);

			lblPR.innerHTML = msg[2];
			RemoveAJAXRequest();
		}

		function onFailed(error) {
			RemoveAJAXRequest();
			alert('There was an error updating the rate.  Please close the window and try again.');
		}

		function onEHDateFailed(error) {
			RemoveAJAXRequest();
			alert('There was an error updating the Effective Hire Date.  Please close the window and try again.');
		}

		function OpenPreview(eid) {
			OpenWindow('../Payroll/Setup/PayRatesPreview.aspx?eid=' + eid, 925, 300, 1, 1, 1, 1, 'atM_emp_preview');
		}

		function OpenAddEmployee() {
			OpenWindow('AddEmployee.aspx', 925, 300, 1, 1, 1, 1, 'atm_add_employee');
		}

		function OpenColumnOptionsDialog() {
			$("#columnOptionsDialog").dialog("open");
			$("#columnOptionsDialog").dialog("option", "title", "Column Options");

			LoadColumnOptionsUserControl("MANAGEEMPLOYEES");

			return false;
		}

		function SaveColumnOptions(columnOptionsDialog) {
			var successfulSave = SaveColumnOptionsUserControl("MANAGEEMPLOYEES");

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
	<div class="container">
		<div class="pageTitle">
			Manage Employees
		</div>
		<div class="pageSubtitle">
			View and Modify Employee Details, Including Tenure, Rate (%), Etc
		</div>
		<div class="row">
			<div class="panel panel-default">
				<div class="panel-heading">
					<div class="row form-group">
						<div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
							<label for="ddProgSygmaCenterNo">
								Center
							</label>
							<asp:DropDownList ID="ddProgSygmaCenterNo" runat="server" DataTextField="CenterDisplay" DataValueField="SygmaCenterNo" AutoPostBack="True"
								OnSelectedIndexChanged="ddProgSygmaCenterNo_SelectedIndexChanged" CssClass="form-control">
							</asp:DropDownList>
							<asp:Label ID="lblCenter" runat="server" Visible="False"></asp:Label>
						</div>
						<div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
							<label for="txtProgName">
								Name Search
							</label>
							<asp:TextBox ID="txtProgName" runat="server" OnTextChanged="txtProgName_TextChanged" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
						</div>
						<div class="col-xs-3">
							<div class="btn-group btn-group-sm">
								<a href="javascript: void(0);" class="btn btn-secondary" onclick='OpenColumnOptionsDialog();' id="anchorColumnOptions">Column Options</a>

								<a href="javascript: void(0);" class="btn btn-secondary" onclick='OpenAddEmployee();'>Add Employee
								</a>
							</div>
						</div>
					</div>
				</div>
				<div class="row table-responsive">
					<asp:Panel ID="pnlProgression" CssClass="panel-body" runat="server" Visible="false">
						<asp:UpdatePanel ID="upSettings" runat="server" UpdateMode="Conditional">
							<Triggers>
								<asp:AsyncPostBackTrigger ControlID="btnRefreshEmployees" EventName="Click" />
							</Triggers>
							<ContentTemplate>
								<asp:Button ID="btnRefreshEmployees" runat="server" Text="Refresh" OnClick="btnRefreshEmployees_Click" Style="display: none" />
								<div>
									Employees <span style="font-weight: normal">(<asp:Label ID="lblEmployeeCount" runat="server"></asp:Label>)</span>
								</div>
								<asp:GridView ID="gvEmployees" runat="server" AllowSorting="true" AutoGenerateColumns="false" OnRowDataBound="gvEmployees_RowDataBound"
									OnSorting="gvEmployees_Sorting" CssClass="table-responsive table-striped table-hover" EmptyDataText="No employees exist for this search." OnInit="gvEmployees_Init">
									<Columns>
										<asp:BoundField HeaderText="Employee" DataField="WebDisplay" SortExpression="WebDisplay" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
										<asp:TemplateField HeaderText="Q. Driver" SortExpression="IsQualifiedDriver">
											<ItemStyle HorizontalAlign="Center" />
											<ItemTemplate>
												<%# Convert.ToBoolean(Eval("IsQualifiedDriver"))? "X" : "" %>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:BoundField HeaderText="Hire Date" DataField="HireDate" DataFormatString="{0:M/d/yyyy}" SortExpression="HireDate" ItemStyle-HorizontalAlign="Center" />
										<asp:TemplateField SortExpression="EffectiveHireDate" HeaderText="Eff Hire Date">
											<ItemStyle HorizontalAlign="Center" />
											<ItemTemplate>
												<asp:CheckBox ID="chkAllowHireDateChange" runat="server" />
												<asp:TextBox ID="txtEffHireDate" runat="server" CssClass="eff-hire-date"></asp:TextBox>
												<cc1:CalendarExtender ID="txtEffHireDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtEffHireDate" Format="M/d/yyyy" PopupPosition="Right">
												</cc1:CalendarExtender>
											</ItemTemplate>
										</asp:TemplateField>

										<asp:TemplateField SortExpression="HireDate" HeaderText="Tenure">
											<ItemStyle HorizontalAlign="Center" />
											<ItemTemplate>
												<asp:Label ID="lblTenure" runat="server" CssClass="tenure-label"></asp:Label>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField SortExpression="ProgressionRate" HeaderText="Rate (%)">
											<ItemStyle HorizontalAlign="Right" />
											<ItemTemplate>
												<asp:TextBox ID="txtProgRate" runat="server" Style="display: block; text-align: right" CssClass="prog-rate"></asp:TextBox>
												<asp:Label ID="lblProgRate" runat="server" Style="display: block; text-align: right" CssClass="prog-rate-label"></asp:Label>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField SortExpression="ProgressionRate" HeaderText="APS EXCPTN">
											<ItemStyle HorizontalAlign="Center" />
											<ItemTemplate>
												<asp:CheckBox ID="chkAPSExcptn" runat="server"></asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:TemplateField SortExpression="GuaranteedPay" HeaderText="Guaranteed Pay ($)">
											<ItemStyle HorizontalAlign="Center" />
											<ItemTemplate>
												<div>
													<asp:CheckBox ID="chkGPExcptn" runat="server" />
													<asp:TextBox ID="txtGuaranteedPay" runat="server" CssClass="guaranteed-pay">
													</asp:TextBox>
												</div>
											</ItemTemplate>
										</asp:TemplateField>

										<asp:TemplateField SortExpression="ClassificationId" HeaderText="Classification">
											<ItemStyle HorizontalAlign="Center" />
											<ItemTemplate>
												<asp:DropDownList ID="ddClassification" runat="server" DataTextField="ClassificationName" DataValueField="ClassificationId" Style="text-align: right"></asp:DropDownList>
											</ItemTemplate>
										</asp:TemplateField>

										<asp:TemplateField SortExpression="ProgressionRate" HeaderText="RTP">
											<ItemStyle HorizontalAlign="Center" />
											<ItemTemplate>
												<a href="javascript: void(0);" onclick='OpenPreview(<%#Eval("EmployeeId") %>);'>
													<i class="fa fa-play-circle fa-lg"></i>
												</a>
											</ItemTemplate>
										</asp:TemplateField>
									</Columns>
								</asp:GridView>
								</div>
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>

					<div id="columnOptionsDialog">
						<uc5:ColumnOptions ID="ColumnOptionsData" runat="server" PageName="PROFILEFORMGRID" />
					</div>
				</div>
			</div>
		</div>

	</div>
</asp:Content>

