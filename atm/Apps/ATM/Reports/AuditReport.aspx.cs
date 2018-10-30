using System;
using System.Data;
using System.Threading;
using System.Web.UI.WebControls;
using atm;
using SygmaIntranet;

namespace atm.Apps.ATM.Reports
{
	public partial class AuditReport : ATMReportPage
	{
		protected override void LoadATMPage()
		{
			if (IsPostBack) return;

			var ds = ATMDB.GetDataSet("up_p_getFormCriteria", UserName);
			var count = ds.Tables[2].Rows.Count;
			//Retrieve all pages and populate the ddPageNames
			SetPageNames();
			//Retrieve Users and populate the ddUsers

			lbSygmaCenterNo.DataSource = ds.Tables[2].DefaultView;
			lbSygmaCenterNo.DataBind();

			if (count > 1)
			{
				lbSygmaCenterNo.Items.Insert(0, new ListItem("All Centers", "0"));
				lbSygmaCenterNo.ClearSelection();
				lbSygmaCenterNo.Height = 15 * (count + 2);
				lbSygmaCenterNo.SelectedIndex = 0;
			}
			else if (count == 1)
			{
				lbSygmaCenterNo.SelectedIndex = 0;
				lbSygmaCenterNo.Height = 45;
			}
			else
			{
				lbSygmaCenterNo.Width = 100;
			}

			if (!Page.IsPostBack)
			{
				ddlEmployeeStatus.Items.Add(new ListItem("Active", "0"));
				ddlEmployeeStatus.Items.Add(new ListItem("Inactive", "1"));
				ddlEmployeeStatus.Items.Add(new ListItem("Active & Inactive", "2"));
				ddlEmployeeStatus.SelectedValue = "0";
			}
		}

		private void SetPageNames()
		{
			//Retrieve the list of all ATM page names
			var pageListDs = ATMDB.GetDataSet("up_getAllAtmPageNames");
			//Populate the values into lblPageNames
			ddPageNames.DataSource = pageListDs.Tables[0];
			ddPageNames.DataBind();
			ddPageNames.Items.Insert(0, new ListItem("All Pages", "0"));
			ddPageNames.ClearSelection();
			ddPageNames.Height = 15 * (pageListDs.Tables[0].Rows.Count - 15);
			ddPageNames.SelectedIndex = 0;
		}

		protected void btnGenerate_Click(object sender, EventArgs e)
		{
			try
			{
				for (var i = 0; i < lbSygmaCenterNo.Items.Count; i++)
				{
					string centerValue;
					if (lbSygmaCenterNo.Items[0].Value == "0" &&
						lbSygmaCenterNo.Items[0].Selected &&
						i < (lbSygmaCenterNo.Items.Count - 1))
					{
						centerValue = lbSygmaCenterNo.Items[i + 1].Value;
						GetSelectedCenters(centerValue);
					}
					else if (lbSygmaCenterNo.Items[i].Selected)
					{
						centerValue = lbSygmaCenterNo.Items[i].Value;
						GetSelectedCenters(centerValue);
					}
				}

				var employeeStatusSubmitValue = (ddlEmployeeStatus.SelectedValue == "2") ? "0,1" : ddlEmployeeStatus.SelectedValue.ToString();

				//Get the FunctionId value for all pages
				var selectedPageIds = RetrievePageNamesSelectedValues();

				var reportHasRecords = BuildExcelReport("14", SygmaCenterNo,
					employeeStatusSubmitValue, selectedPageIds); // Report Id, CenterNumber, EmployeeStatus, FunctionId
				if (!reportHasRecords)
				{
					Javascript.Notify("No Records available for the selected input!!!");
				}
			}
			catch (ThreadAbortException) { }
			catch (Exception exp)
			{
				throw new Exception("Error generating report", exp);
			}
		}

		private string RetrievePageNamesSelectedValues()
		{
			//If All Pages option is Selected
			if (ddPageNames.Items[0].Selected)
			{
				return null;
			}
			//Else sort through the pages to find selected ones
			else
			{
				string selectedPageValues = "";
				for (var i = 0; i < ddPageNames.Items.Count; i++)
				{
					if (ddPageNames.Items[i].Selected)
					{
						selectedPageValues += (ddPageNames.Items[i].Value + ",");
					}
				}
				//If no pages were selected then return null
				if (selectedPageValues.Length == 0)
				{
					return null;
				}
				//Else return the string of page functionId values
				else
				{
					return selectedPageValues;
				}
			}
		}

	}
}