using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using atm;

public partial class Departments_Transportation_ATM_Payroll_Index : ATMPage {
	protected override void LoadATMPage() {
		//if funcid is passed in then it should be an ajax request
		if (!string.IsNullOrEmpty(Request.QueryString.Get("funcid"))) {
			ATMDB.RunNonQuery("up_user_setStartFunction", UserName, Request.QueryString.Get("funcid"));
		} else {
			DataView dv = ATMDB.GetDataView("up_user_getUserInfo", UserName);
			if (dv.Count > 0) {
				if (!Convert.IsDBNull(dv[0]["RootUrl"])) {
					Response.Redirect(dv[0]["RootUrl"].ToString());
				}
			}
		}
	}
}