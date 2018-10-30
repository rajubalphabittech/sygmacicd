using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;
using atm;

public partial class Apps_ATM_Security_Users : ATMPage {
	protected override void LoadATMPage() {
		SetPageVariables();
		if (!IsPostBack) {
			SetUsers();
			SetCenters();
		}
	}
	private void SetPageVariables() {
		AddClientVariable("gUserName", UserName);
		AddClientVariable("ddUser", ddUser.ClientID);
		//AddClientVariable("lbCenters", lbCenters.ClientID);
	}

	private void SetCenters() {
		lbCenters.DataSource = ATMDB.GetDataView("up_getCenters");
		lbCenters.DataBind();
		lbCenters.Items.Insert(0, new ListItem("All", "0"));
	}

	private void SetUsers() {
		ddUser.DataSource = ATMDB.GetDataView("up_sec_getUsers", UserName);
		ddUser.DataBind();
		ddUser.Items.Insert(0, new ListItem("Choose...", ""));
	}

	private void SetSelectedUser() {
		if (ddUser.SelectedValue != "") {
			pnlSelectedUser.Visible = true;
			DataSet ds = ATMDB.GetDataSet("up_sec_getUser", ddUser.SelectedValue);
			ds.Tables[0].TableName = "Centers";
			ds.Tables[1].TableName = "Sections";
			ds.Tables[2].TableName = "Functions";
            dlSections.DataSource = "";
            dlSections.DataBind();
			dlSections.DataSource = new DataView(ds.Tables[1], "", "DisplayOrder ASC", DataViewRowState.CurrentRows);
			dlSections.DataBind();

			lbCenters.ClearSelection();
			foreach (DataRowView center in ds.Tables["Centers"].DefaultView) {
				WebCommon.SelectListValue(lbCenters, center["SygmaCenterNo"].ToString(), true);
			}

		} else {
			pnlSelectedUser.Visible = false;
		}
	}

	protected void ddUser_SelectedIndexChanged(object sender, EventArgs e) {
		SetSelectedUser();
	}
	protected void dlSections_ItemDataBound(object sender, DataListItemEventArgs e) {
		switch (e.Item.ItemType) {
			case ListItemType.AlternatingItem:
			case ListItemType.Item:
				DataRowView sectionRow = (DataRowView)e.Item.DataItem;
				Repeater rptFunctions = (Repeater)e.Item.FindControl("rptFunctions");
				
				DataRow[] rows = sectionRow.DataView.Table.DataSet.Tables["Functions"].Select(string.Format("SectionId = {0}", sectionRow["SectionId"]), "DisplayOrder ASC");;
				rptFunctions.DataSource = rows;
				rptFunctions.DataBind();
				
				break;
		}
	}

	protected void rptFunctions_ItemDataBound(object sender, RepeaterItemEventArgs e) {
		switch (e.Item.ItemType) {
			case ListItemType.AlternatingItem:
			case ListItemType.Item:

				DataRow row = (DataRow)e.Item.DataItem;
				CheckBox chkEnabled = (CheckBox)e.Item.FindControl("chkEnabled");
				chkEnabled.Checked = Convert.ToBoolean(row["Enabled"]);
                chkEnabled.ID = row["FunctionId"].ToString();
				chkEnabled.Text = row["FunctionDescription"].ToString();
				chkEnabled.Attributes.Add("onClick", string.Format("SaveUserFunction(this,{0});", row["FunctionId"]));
              
				break;
		}
	}

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        SetSelectedUser();

    }

	[WebMethod]
	public static void PM_SaveUserFunction(string savingUserName, string userName, int functionId, bool enabled) {
		RunNonQueryFromStatic("ATM", "up_sec_setUserFunction", savingUserName, userName, functionId, enabled);
	}
	[WebMethod]
	public static void PM_SaveUserCenters(string savingUserName, string userName, string centers) {
		RunNonQueryFromStatic("ATM", "up_sec_setUserCenters", savingUserName, userName, centers);
	}
}