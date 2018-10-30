<%@ Page Title="User Maintenance" Language="C#" MasterPageFile="~/Masters/ATM.master" AutoEventWireup="true" Inherits="Apps_ATM_Security_Users" CodeBehind="Users.aspx.cs" %>

<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<script type="text/javascript">

		function SaveUserFunction(c, fid) {
			AddAJAXRequest();

			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveUserFunction(gUserName, GetSelectedValue(ddUser), fid, c.checked, onSuccess, onFailed);
			__doPostBack("<%= this.LinkButton1.UniqueID %>", "");
		}
		function SaveUserCenter(c) {
			var centers = '';
			for (var i = 0; i < c.options.length; i++) {
				if (c.options[i].selected) {
					var val = c.options[i].value;
					if (val == '0') {
						centers = '0';
						c.selectedIndex = 0;
						break;
					} else {
						centers += ',' + val;
					}
				}
			}
			AddAJAXRequest();

			PageMethods.set_path('<%=HttpContext.Current.Request.Url.AbsolutePath  %>');
			PageMethods.PM_SaveUserCenters(gUserName, GetSelectedValue(ddUser), centers.substring(1, centers.length), onSuccess, onFailed);
		}
		function onSuccess() {
			RemoveAJAXRequest();
		}
		function onFailed(e) {
			RemoveAJAXRequest();
			alert('An error occurred trying to save your change. Please refresh the window and try again.');
		}
	</script>
	<style type="text/css">
		div legend {
			font-weight: bold;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
	<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
	</asp:ScriptManager>

	<div class="container">
		<div class="pageTitle">
			User Maintenance
		</div>
		<div class="pageSubtitle">
			View and Modify ATM User Permissions
		</div>
		<div class="row" style="margin-top: 10px;">
			<div class="panel panel-default">
				<div class="panel-heading">
					<div class="input-group">
						<asp:DropDownList ID="ddUser" runat="server" DataTextField="WebDisplay" DataValueField="UserName" OnSelectedIndexChanged="ddUser_SelectedIndexChanged"
							AutoPostBack="true" CssClass="form-control">
						</asp:DropDownList>
					</div>
				</div>
				<div class="panel-body">
					<asp:UpdatePanel ID="upSelectedUser" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ddUser" EventName="SelectedIndexChanged" />
							<asp:AsyncPostBackTrigger ControlID="LinkButton1" EventName="Click" />
						</Triggers>
						<ContentTemplate>
							<asp:Panel ID="pnlSelectedUser" runat="server" Visible="false">
								<div class="row">
									<div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
										<div class="panel panel-default">
											<div class="panel-heading">Centers</div>
											<div class="panel-body">
												<asp:ListBox ID="lbCenters" runat="server" DataValueField="SygmaCenterNo" DataTextField="CenterDisplay" onchange="SaveUserCenter(this);" SelectionMode="Multiple" Height="400px" CssClass="form-control"></asp:ListBox>
											</div>
										</div>
									</div>
									<asp:DataList ID="dlSections" runat="server" RepeatLayout="Flow" DataMember="Sections" OnItemDataBound="dlSections_ItemDataBound">
										<%--<ItemStyle VerticalAlign="Top" />--%>
										<ItemTemplate>
											<div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
												<div class="panel panel-default">
													<div class="panel-heading"><%# Eval("SectionDescription") %></div>
													<div class="panel-body">
														<asp:Repeater ID="rptFunctions" runat="server" OnItemDataBound="rptFunctions_ItemDataBound">
															<ItemTemplate>
																<div>
																	<asp:CheckBox ID="chkEnabled" runat="server" />
																</div>
															</ItemTemplate>
														</asp:Repeater>
													</div>
												</div>
											</div>
										</ItemTemplate>
									</asp:DataList>
								</div>

							</asp:Panel>
							<asp:LinkButton ID="LinkButton1" runat="server" Style="display: none;"
								OnClick="LinkButton1_Click"> 
                        LinkButton 
							</asp:LinkButton>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
