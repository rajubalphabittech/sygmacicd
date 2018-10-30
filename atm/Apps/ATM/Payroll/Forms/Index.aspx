<%@ Page Language="C#" AutoEventWireup="true" Inherits="Apps_ATM_Payroll_Forms_Index" MasterPageFile="~/Masters/ATM.master"
	Title="Payroll Forms" CodeBehind="Index.aspx.cs" %>

<%@ Register Src="~/UserControls/Date.ascx" TagName="Date" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/RowCountBar.ascx" TagName="RowCountBar" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ChangeChecks.ascx" TagName="ChangeChecks" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/ATMColumnOptionsSelector.ascx" TagName="ColumnOptions" TagPrefix="uc5" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
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
	<script type="text/javascript" language="javascript">

		$(window).resize(function () {
			pageLoad();
		});

		function pageLoad() {
			var gridWidth = $(window).width() - 25;

			$('#<%=gvForms.ClientID%>').gridviewScroll({
				width: gridWidth,
				height: 620
			});

			if (document.getElementById('body_gvFormsPagerBottom') != null && document.getElementById('body_gvFormsPagerBottom') != undefined) {
				var pagerWidth = document.getElementById('body_gvFormsPagerBottom').getElementsByTagName('table')[0].offsetWidth;
				var desiredLeftPadding = ((gridWidth / 2) - (pagerWidth / 2));

				//document.getElementById('body_gvFormsPagerBottom').style.width = "100%";
				document.getElementById('body_gvFormsPagerBottom').style.paddingLeft = desiredLeftPadding.toString() + "px";
			}
		}

		var retVariable = false;
		function gridviewupdate() {


			var btn = document.getElementById('ctl00_body_btnRefresh');
			if (btn != null) {
				btn.click();

			}


		}
		function OpenForm(fid) {
			var url = 'AddUpdate.aspx';
			if (fid != null)
				url = url + '?fid=' + fid;
			OpenWindow(url, 980, 650, 1, 1, 1, 1, (fid != null) ? 'atmp_' + fid : null);
		}

		function ConfirmAction(c) {
			var elements = document.getElementsByTagName('input')
			var sel = c.options[c.selectedIndex];
			if (sel.value != '') {
				var isChecked = 0;
				for (var i = 0; i < elements.length; i++) {
					var element = elements[i];
					if (element.type == 'checkbox' && element.id.toString().indexOf('chkSelected') > -1) {
						if (element.checked) {

							isChecked = 1;
						}
					}
				}
				if (isChecked == 1) {
					if (confirm('Are you sure you want to ' + sel.text.toLowerCase() + ' these forms?')) {
						document.getElementById('<%=txtNotes.ClientID%>').value = '';
						if (sel.text.toLowerCase() == "reject") {
							var ret = $('#ndialog').dialog('open');
							$('#ndialog').dialog('option', 'title', 'Add ' + sel.text.toLowerCase() + ' Notes');
						}
						else {

							//document.getElementById('ctl00_body_btnInvisibleNotes').click();

							$(".invisible-notes-button")[0].click();

						}
						return true;
					}
					else {
						c.selectedIndex = 0;
						return false;
					}
				}
				else {
					alert('You must select at least 1 form!');
					c.selectedIndex = 0;
					return false;
				}
			}

		}

		function CheckNote(c) {
			var notesVal = document.getElementById('<%=txtNotes.ClientID%>').value;



			var allowTruncate = true;
			var val = notesVal

			if (val.length > 500) {
				allowTruncate = confirm('The notes section is limited to 500 characters.\n\nClick \'OK\' to save the first 500 characters.\nClick \'Cancel\' to cancel the save and trim the text manually.');
				if (allowTruncate) {
					val = notesVal.toString().substring(0, 500);
					notesVal = val;
				}
			}

			var notesVal1 = notesVal.replace(/\s/g, "");
			if (notesVal1.length == 0) {

				alert('Please enter the reason in notes field');

				return;
			}


			$("#ndialog").dialog("close");
			$('#ndialog').parent().appendTo($('form:first'));
			//document.getElementById('ctl00_body_btnInvisibleNotes').click();
			$(".invisible-notes-button")[0].click();



		}


		function SearchFormId(c) {
			if (c.value.trim() != '') {
				if (!isNaN(c.value) && c.value.indexOf('.') < 0) {
					var formId = parseInt(c.value);
					if (formId > 0) {
						var q = GetQualifier(c);
						$get(q + 'ddStatus').selectedIndex = 0;
						$get(q + 'ddFormType').selectedIndex = 0;
						$get(q + 'ddSygmaCenterNo').selectedIndex = 0;
						$get(q + 'txtRouteNo').value = '';
						$get(q + 'dteWeekending_txtDate').value = '';
						$get(q + 'txtFromDate').value = '';
						$get(q + 'txtToDate').value = '';
						$get(q + 'txtEmployee').value = '';
						$get(q + 'txtTractor').value = '';
						$get(q + 'txtTrailer').value = '';

						return true;
					}
				}
				alert('"' + c.value + '" is not a valid ID!');
				c.value = '';
				return false;
			}
			return true;
		}

		function OpenColumnOptionsDialog() {
			$("#columnOptionsDialog").dialog("open");
			$("#columnOptionsDialog").dialog("option", "title", "Column Options");

			LoadColumnOptionsUserControl("PROFILEFORMGRID");

			return false;
		}

		function SaveColumnOptions(columnOptionsDialog) {
			var successfulSave = SaveColumnOptionsUserControl("PROFILEFORMGRID");

			if (successfulSave) {
				$(columnOptionsDialog).dialog("close");
				location.reload();
			} else {
				alert("Error saving user column preferences");
			}
        }


		$(document).ready(function () {

			$("#ndialog").dialog({
				height: 210
				, width: 300
				, autoOpen: false
				, closeOnEscape: true
				, modal: true
				, resizable: false
				, buttons: {
					"Ok": function () { CheckNote(this); return true; }
					, "Cancel": function () { $(this).dialog("close"); return false; }
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

        function UpdateDriverDetails(c, detailId, formId, rowIndex) {
            if (detailId == 1) {
                var helperDropDown = document.getElementById('body_gvForms_ddHelper_' + rowIndex);
                if (c.value == 0) {
                    helperDropDown.disabled = true;
                    if (helperDropDown.style.backgroundColor == '') {
                        helperDropDown.classList.add("disabled-drop-down");
                    }
                    helperDropDown.value = 0;
                } else {
                    helperDropDown.disabled = false;
                    helperDropDown.classList.remove("disabled-drop-down");
                }    
            }
			var val = parseInt(c.value);
			AddAJAXRequest();
			PageMethods.set_path("/apps/atm/payroll/forms/index.aspx");
			PageMethods.PM_SaveDriverDetails(gUserName, formId, detailId, val, onChangeSuccess, onFailed);
        }

		function onChangeSuccess(ret) {
			RemoveAJAXRequest();
			if (ret == "1") {
				alert("Route has Started, Driver details cannot be updated");
			}
			else {
				//alert("Not Started");
			}
        }

		function onFailed(error) {
			RemoveAJAXRequest();
			alert('There was an error saving the field.  Please refresh the page and try again.');
		}

		$(document).ready(function () {
			$(document).on("change", ".check-all", function () {
				var isChecked = this.checked;
				$(".payroll-form-selector input:first-child").prop("checked", isChecked);
			});
		});
	</script>
	
	<style type="text/css">
		.largeField {
			overflow: hidden;
		}
	</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" ID="body" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
	</asp:ScriptManager>
	<div style="width: 100%">

		<div style="padding-bottom: 10px">
			<div style="width: 100%">
				<div class="pageTitle inlineBlock">
					Payroll Forms
				</div>
				<div class="pageSubtitle">
					View and Modify Payroll Details
				</div>
			</div>
		</div>
		<div>
			<asp:UpdatePanel ID="upValidationSummary" runat="server" UpdateMode="Conditional">
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="dteWeekending" EventName="DateChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtFromDate" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtToDate" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtFormId" EventName="TextChanged" />
				</Triggers>
				<ContentTemplate>
					<asp:ValidationSummary ID="ValidationSummary1" runat="server" class="alert alert-danger" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div>

			<div style="width: 99%; text-align: right;">
				<a href="javascript: void(0);" onclick='OpenColumnOptionsDialog();' id="anchorColumnOptions">Column Options</a>
				&nbsp;|&nbsp;
			<asp:LinkButton ID="btnRejected" runat="server" Text="Rejected"
				OnClick="btnRejected_Click"></asp:LinkButton>
				&nbsp;|&nbsp;
            <asp:LinkButton ID="btnUnApproved" runat="server" Text="Unapproved"
							OnClick="btnUnApproved_Click"></asp:LinkButton>
				&nbsp;|&nbsp;
            <asp:LinkButton ID="btnResetSearch" runat="server" Text="Reset" OnClick="btnResetSearch_Click"></asp:LinkButton>
				<%# (Eval("StatusId").ToString() == "3" ? "<i class='fa fa-thumbs-o-up' aria-hidden='true' title='Approved' alt='Approved'></i>" : "") %>
			</div>
			<asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
				<Triggers>
					<%--<asp:AsyncPostBackTrigger ControlID="btnRefreshPage" EventName="Click" />--%>
					<asp:AsyncPostBackTrigger ControlID="btnResetSearch" EventName="Click" />
					<asp:AsyncPostBackTrigger ControlID="btnUnApproved" EventName="Click" />
					<asp:AsyncPostBackTrigger ControlID="btnRejected" EventName="Click" />
				</Triggers>
				<ContentTemplate>
					<asp:Panel ID="pnlSearch" runat="server" Width="100%" Style="margin-bottom: 10px; margin-top: 10px; padding-bottom: 10px">
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								ID
							</div>
							<asp:TextBox ID="txtFormId" class="form-control-custom" runat="server" Width="50px" AutoPostBack="true" OnTextChanged="txtFormId_TextChanged" onchange="if (!SearchFormId(this)) return false;"></asp:TextBox>

						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								Status:
							</div>
							<asp:DropDownList ID="ddStatus" class="form-control-custom" Width="100px" runat="server" DataTextField="StatusDescription" DataValueField="StatusId" AutoPostBack="true" OnSelectedIndexChanged="ddStatus_SelectedIndexChanged">
							</asp:DropDownList>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								Form Type:
							</div>
							<asp:DropDownList ID="ddFormType" class="form-control-custom" Width="100px" runat="server" DataTextField="FormTypeDescription" DataValueField="FormTypeId" AutoPostBack="true"
								OnSelectedIndexChanged="ddFormType_SelectedIndexChanged">
							</asp:DropDownList>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								Center:
							</div>
							<asp:DropDownList ID="ddSygmaCenterNo" class="form-control-custom" Width="100px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSygmaCenterNo_SelectedIndexChanged"
								DataValueField="SygmaCenterNo" DataTextField="CenterDisplay">
							</asp:DropDownList>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								Route #
							</div>
							<asp:TextBox ID="txtRouteNo" class="form-control-custom" runat="server" Width="50px" MaxLength="4" AutoPostBack="true" OnTextChanged="txtRouteNo_TextChanged"></asp:TextBox>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<asp:UpdatePanel ID="upDateRange" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
								<ContentTemplate>
									<div class="inputHeader">
										Date Range
									</div>
									<asp:DropDownList ID="ddlDateRange" runat="server" AutoPostBack="True" CssClass="form-control-custom" OnSelectedIndexChanged="ddlDateRange_SelectedIndexChanged">
									</asp:DropDownList>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<asp:UpdatePanel ID="upWeekending" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
								<ContentTemplate>
									<div class="inputHeader">
										Weekending
									</div>
									<uc2:Date ID="dteWeekending" runat="server" ShowName="false" Name="Weekending" AutoPostBack="true" Required="false" OnDateChanged="dteWeekEnding_DateChanged" Visible="True" />
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<asp:UpdatePanel ID="upFromDate" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
								<ContentTemplate>
									<div class="inputHeader">
										From Date
									</div>
									<asp:TextBox ID="txtFromDate" class="form-control-custom" runat="server" Columns="9" MaxLength="10" AutoPostBack="True" OnTextChanged="dteFromDate_DateChanged" AutoCompleteType="None" autocomplete="off"></asp:TextBox>
									<cc1:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtFromDate" Format="M/d/yyyy" PopupPosition="Right">
									</cc1:CalendarExtender>
									<asp:RangeValidator CssClass="validator-message" ID="rngFromDate" runat="server" ControlToValidate="txtFromDate" MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true"
										Display="dynamic" Type="date" EnableClientScript="True" Enabled="True" ValidationGroup="ValidationSummary1">*</asp:RangeValidator>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<asp:UpdatePanel ID="upToDate" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
								<ContentTemplate>
									<div class="inputHeader">
										To Date
									</div>
									<asp:TextBox ID="txtToDate" class="form-control-custom" runat="server" Columns="9" MaxLength="10" AutoPostBack="true" OnTextChanged="dteToDate_DateChanged" AutoCompleteType="None" autocomplete="off"></asp:TextBox>
									<cc1:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtToDate" Format="M/d/yyyy" PopupPosition="Right">
									</cc1:CalendarExtender>
									<asp:CompareValidator CssClass="validator-message" ID="cvEndDate" runat="server"
										ControlToCompare="txtFromDate" ControlToValidate="txtToDate"
										ErrorMessage="'To date' cannot less than 'From date'" Operator="GreaterThanEqual"
										Type="Date" ValidationGroup="ValidationSummary1">*</asp:CompareValidator>
									<asp:RangeValidator CssClass="validator-message" ID="rngToDate" runat="server" ControlToValidate="txtToDate" MinimumValue="1/1/1900" MaximumValue="6/6/2079" SetFocusOnError="true"
										Display="dynamic" Type="date" EnableClientScript="True" Enabled="True" ValidationGroup="ValidationSummary1">*</asp:RangeValidator>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								Employee
							</div>
							<asp:TextBox ID="txtEmployee" class="form-control-custom" runat="server" MaxLength="100" Width="100px" AutoPostBack="true" OnTextChanged="txtEmployee_TextChanged"></asp:TextBox>
							<cc1:AutoCompleteExtender ID="txtEmployee_AutoCompleteExtender" runat="server" Enabled="True" ServicePath="~/Apps/ATM/WebServices/Employees.asmx"
								ServiceMethod="SearchEmployees" UseContextKey="True" TargetControlID="txtEmployee" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
								CompletionListHighlightedItemCssClass="autoCompleteListItemHover" MinimumPrefixLength="2" CompletionSetCount="20" CompletionInterval="100">
							</cc1:AutoCompleteExtender>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								Tractor
							</div>
							<asp:TextBox ID="txtTractor" class="form-control-custom" runat="server" MaxLength="100" Width="100px" AutoPostBack="true" OnTextChanged="txtTractor_TextChanged"></asp:TextBox>
							<cc1:AutoCompleteExtender ID="txtTractor_AutoCompleteExtender" runat="server" Enabled="True" ServicePath="~/Apps/ATM/WebServices/Tractors.asmx"
								ServiceMethod="SearchTractors" UseContextKey="True" TargetControlID="txtTractor" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
								CompletionListHighlightedItemCssClass="autoCompleteListItemHover" MinimumPrefixLength="2" CompletionSetCount="20" CompletionInterval="100">
							</cc1:AutoCompleteExtender>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px; vertical-align: top">
							<div class="inputHeader">
								Trailer
							</div>
							<asp:TextBox ID="txtTrailer" class="form-control-custom" runat="server" MaxLength="100" Width="100px" AutoPostBack="true" OnTextChanged="txtTrailer_TextChanged"></asp:TextBox>
							<cc1:AutoCompleteExtender ID="txtTrailer_AutoCompleteExtender" runat="server" Enabled="True" ServicePath="~/Apps/ATM/WebServices/Trailers.asmx"
								ServiceMethod="SearchTrailers" UseContextKey="True" TargetControlID="txtTrailer" CompletionListCssClass="autoCompleteList" CompletionListItemCssClass="autoCompleteListItem"
								CompletionListHighlightedItemCssClass="autoCompleteListItemHover" MinimumPrefixLength="2" CompletionSetCount="20" CompletionInterval="100">
							</cc1:AutoCompleteExtender>
						</div>
						<div class="inlineBlock" style="padding: 3px 3px 3px 3px;">
							<div class="inputHeader">
								Actuals? (AU)
							</div>
							<asp:DropDownList Width="100px" ID="ddActualsUpdated" class="form-control-custom" runat="server" OnSelectedIndexChanged="ddActualsUpdated_SelectedIndexChanged" AutoPostBack="true">
								<asp:ListItem Value="">All</asp:ListItem>
								<asp:ListItem Value="0">Not Updated</asp:ListItem>
								<asp:ListItem Value="1">Updated</asp:ListItem>
							</asp:DropDownList>
						</div>
					</asp:Panel>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div>
			<asp:UpdatePanel ID="upForms" runat="server" UpdateMode="Conditional">
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="txtFormId" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="ddSygmaCenterNo" EventName="SelectedIndexChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtRouteNo" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="dteWeekending" EventName="DateChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtFromDate" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtToDate" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="ddActualsUpdated" EventName="SelectedIndexChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtEmployee" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtTractor" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="txtTrailer" EventName="TextChanged" />
					<asp:AsyncPostBackTrigger ControlID="ddStatus" EventName="SelectedIndexChanged" />
					<asp:AsyncPostBackTrigger ControlID="ddFormType" EventName="SelectedIndexChanged" />
					<asp:AsyncPostBackTrigger ControlID="btnResetSearch" EventName="Click" />
					<%--<asp:AsyncPostBackTrigger ControlID="btnRefreshPage" EventName="Click" />--%>
					<asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
					<asp:AsyncPostBackTrigger ControlID="btnInvisibleNotes" EventName="Click" />

					<asp:AsyncPostBackTrigger ControlID="ddlDateRange" EventName="SelectedIndexChanged" />

				</Triggers>
				<ContentTemplate>
					<asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-secondary btn-xs" OnClick="btnRefresh_Click" Text="Refresh" />
					<div>
						<div class="inlineBlock" style="text-align: right;">
							<asp:HyperLink ID="hlCreateNew" runat="server" ImageUrl="~/Images/Icons/document_add_32.png" Text="Create New" NavigateUrl="javascript: OpenForm();"
								ToolTip="Create New Form"></asp:HyperLink>
						</div>

						<div class="inlineBlock" style="width: 100%">
							<uc3:RowCountBar ID="RowCountBar1" runat="server" Width="100%" />
						</div>
						<asp:GridView ID="gvForms" runat="server" EnableModelValidation="True"
							AutoGenerateColumns="False" OnRowDataBound="gvForms_RowDataBound"
							AllowPaging="True" AllowSorting="True"
							EmptyDataText="No forms exist for this search criteria!" DataKeyNames="FormId"
							Width="100%" DataSourceID="SqlDataSource1" OnInit="gvForms_Init">
							<Columns>
								<asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemTemplate>
										<%# (Eval("StatusId").ToString() == "0" ? "<i class='fa fa-lg fa-check payroll-forms' aria-hidden='true' title='Checked Out' alt='Checked Out'></i>" : "") %>
										<%# (Eval("StatusId").ToString() == "1" ? "<i class='fa fa-lock fa-lg payroll-forms' aria-hidden='true' title='Checked In' alt='Checked In'></i>" : "") %>
										<%# (Eval("StatusId").ToString() == "3" ? "<i class='fa fa-thumbs-up fa-lg payroll-forms' aria-hidden='true' title='Approved' alt='Approved'></i>" : "") %>
										<%# (Eval("StatusId").ToString() == "4" ? "<i class='fa fa-thumbs-down fa-lg payroll-forms' aria-hidden='true' title='Rejected' alt='Rejected'></i>" : "") %>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
								<asp:BoundField DataField="FormId" HeaderText="ID" SortExpression="FormId"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:TemplateField HeaderText="Type" SortExpression="FormTypeDescription" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemTemplate>
										<%# (Eval("FormTypeDescription").ToString().Length > 10) ? Eval("FormTypeDescription").ToString().Substring(0, 4) : Eval("FormTypeDescription").ToString() %>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>

								<asp:BoundField DataField="CenterDescription" HeaderText="Center" Itemstyle-CssClass="maxWidthGridColumn160" SortExpression="CenterDescription" >
                                </asp:BoundField>
								<asp:BoundField DataField="RouteNo" HeaderText="Route #"
									SortExpression="RouteNo"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:BoundField DataField="FiscalWeekEnding" HeaderText="Weekending"
									SortExpression="FiscalWeekEnding" DataFormatString="{0:M/d/yy}"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:BoundField DataField="RouteDepartDate" HeaderText="Depart Date"
									SortExpression="RouteDepartDate" DataFormatString="{0:M/d/yy}"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:TemplateField HeaderText="Employee 1" SortExpression="Employee1" ItemStyle-CssClass="Wrap">
									<ItemTemplate>
										<%# Eval("Employee1") %>
									</ItemTemplate>
									<ItemStyle />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Employee 2" SortExpression="Employee2" ItemStyle-CssClass="Wrap">
									<ItemTemplate>
										<%# Eval("Employee2") %>
									</ItemTemplate>
									<ItemStyle />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Tractor 1" SortExpression="VehicleName" ItemStyle-CssClass="Wrap">
									<ItemTemplate>
										<%# Eval("VehicleName")%>
									</ItemTemplate>
									<ItemStyle />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Trailer 1" SortExpression="TrailerSygmaId" ItemStyle-CssClass="Wrap">
									<ItemTemplate>
										<%# Eval("TrailerSygmaId")%>
									</ItemTemplate>
									<ItemStyle />
								</asp:TemplateField>
								<%--CHRIS--%>
								<asp:TemplateField HeaderText="Assigned Driver" Visible="true" ItemStyle-CssClass="Wrap" HeaderStyle-CssClass="maxWidthGridColumn120">
									<ItemStyle HorizontalAlign="Left" CssClass="maxWidthGridColumn120" />
									<ItemTemplate>
                                        <div onclick="event.cancelBubble=true;">
									        <asp:DropDownList ID="ddDriver" runat="server" DataTextField="DriverName" DataValueField="DriverId" AutoPostBack="false" Style="text-align: right; max-width: 110px"></asp:DropDownList>
                                        </div>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Assigned Team Driver/Helper" Visible="true" ItemStyle-CssClass="Wrap" HeaderStyle-CssClass="maxWidthGridColumn160">
									<ItemStyle HorizontalAlign="Left" CssClass="maxWidthGridColumn160" />
									<ItemTemplate>
										<div onclick="event.cancelBubble=true;">
											<asp:DropDownList ID="ddHelper" runat="server" DataTextField="DriverName" DataValueField="DriverId" AutoPostBack="false" Style="text-align: right; max-width: 150px"></asp:DropDownList>
										</div>
									</ItemTemplate>
								</asp:TemplateField>
								<%--CHRIS--%>
								<asp:TemplateField HeaderText="AU" ItemStyle-HorizontalAlign="Center" SortExpression="ActualsUpdated" ItemStyle-CssClass="Wrap">
									<ItemTemplate>
										<%# (Convert.ToBoolean(Eval("ActualsUpdated")))? "X" : "" %>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>

								<asp:BoundField DataField="Cases" HeaderText="Cases" SortExpression="Cases"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:BoundField DataField="Pounds" HeaderText="LBs" SortExpression="Pounds"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:BoundField DataField="Cubes" HeaderText="Cubes" SortExpression="Cubes"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:BoundField DataField="Miles" HeaderText="Miles" SortExpression="Miles"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:BoundField DataField="Stops" HeaderText="Stops" SortExpression="Stops"
									ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<ItemStyle HorizontalAlign="Center" />
								</asp:BoundField>
								<asp:TemplateField SortExpression="IsCompleted" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="Wrap">
									<HeaderTemplate>
										<input type="checkbox" id="checkAll" class="check-all" />
									</HeaderTemplate>
									<ItemTemplate>
										<div onclick="event.cancelBubble=true;">
											<asp:CheckBox ID="chkSelected" BackColor="" CssClass="payroll-form-selector" runat="server" />
										</div>
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
							</Columns>
							<HeaderStyle CssClass="GridviewScrollHeader-custom" />
							<PagerStyle CssClass="pagination-cs" />
						</asp:GridView>
						<asp:Panel ID="pnlButtons" runat="server" Style="text-align: right; padding-top: 10px" Width="100%">

							<asp:DropDownList ID="ddSelectedAction" runat="server" onChange="if (!ConfirmAction(this)){ return false;}"
								AutoPostBack="false">
							</asp:DropDownList>
						</asp:Panel>
					</div>
					<asp:SqlDataSource ID="SqlDataSource1" runat="server"
						ConnectionString="<%$ ConnectionStrings:ATM %>" ProviderName="<%$ ConnectionStrings:ATM.ProviderName %>"
						SelectCommand="up_p_getForms" SelectCommandType="StoredProcedure"
						CancelSelectOnNullParameter="False" OnSelecting="SqlDataSource1_Selecting"
						OnSelected="SqlDataSource1_Selected">
						<SelectParameters>
							<asp:ControlParameter ControlID="txtFormId" Name="formId" PropertyName="Text" Type="String" />
							<asp:ControlParameter ControlID="ddSygmaCenterNo" Name="sygmaCenterNo" PropertyName="SelectedValue" Type="Int32" />
							<asp:ControlParameter ControlID="txtRouteNo" Name="routeNo" PropertyName="Text" Type="String" />
							<asp:ControlParameter ControlID="dteWeekending" Name="weekending" PropertyName="ValueString" Type="String" Direction="InputOutput"
								Size="20" />
							<asp:ControlParameter ControlID="txtFromDate" Name="fromDate" PropertyName="Text" Type="String"
								Size="20" />
							<asp:ControlParameter ControlID="txtToDate" Name="toDate" PropertyName="Text" Type="String"
								Size="20" />
							<asp:ControlParameter ControlID="ddStatus" Name="statusId" PropertyName="SelectedValue" Type="Int32" />
							<asp:ControlParameter ControlID="ddFormType" Name="formTypeId" PropertyName="SelectedValue" Type="Int32" />
							<asp:ControlParameter ControlID="ddActualsUpdated" Name="actualsUpdated " PropertyName="SelectedValue" Type="string" ConvertEmptyStringToNull="true" />
							<asp:ControlParameter ControlID="txtEmployee" Name="employeeString" PropertyName="Text" Type="String" />
							<asp:ControlParameter ControlID="txtTractor" Name="tractorString" PropertyName="Text" Type="String" />
							<asp:ControlParameter ControlID="txtTrailer" Name="trailerString" PropertyName="Text" Type="String" />
							<asp:SessionParameter Name="userName" SessionField="sUserName" />
						</SelectParameters>
					</asp:SqlDataSource>
					<!---->


					<asp:Button ID="btnInvisibleNotes" runat="server" Style="display: none" OnClick="btnInvisibleNotes_Click" Text="Invisible" CssClass="invisible-notes-button" />

				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
	</div>
	<div id="columnOptionsDialog">
		<uc5:ColumnOptions ID="ColumnOptionsData" runat="server" PageName="PROFILEFORMGRID" />
	</div>

	<div id="ndialog">
		<div style="margin-bottom: 5px">
			<div class="inlineBlock" style="font-weight: bold; width: 100px;">
				Notes :
			</div>
			<asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="36px" Width="95%"></asp:TextBox>
		</div>
	</div>
	<asp:UpdateProgress ID="prgForms" runat="server" AssociatedUpdatePanelID="upSearch" DisplayAfter="600">
		<ProgressTemplate>
			<div class="disableBackground">
			</div>
			<div class="progressPane loading">
				<div style="font-weight: bold; font-size: 13px; padding: 10px 0px 2px 10px">
					Searching Forms...
				</div>
				<asp:Image ID="imgSearchProgress" runat="server" ImageUrl="~/Images/animated_bar.gif" />
			</div>

		</ProgressTemplate>
	</asp:UpdateProgress>
	<asp:UpdateProgress ID="upGrid" runat="server" AssociatedUpdatePanelID="upForms" DisplayAfter="600">
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

	<br />



</asp:Content>


