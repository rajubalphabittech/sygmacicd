using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using atm;

public partial class Apps_ATM_Payroll_Setup_Employees : ATMPage
{
    protected override void LoadATMPage() {
			SetPageVariables();
			if (!IsPostBack) {
				SetEmployees();
			}
		}

		private void SetEmployees() {
			ddEmployee.DataSource = ATMDB.GetDataView("up_getEmployees", UserName);
			ddEmployee.DataBind();
			ddEmployee.Items.Insert(0, new ListItem("Choose...", ""));
		}

		private void SetPageVariables() {

		}

		protected void ddEmployee_SelectedIndexChanged(object sender, EventArgs e) {

		}
}