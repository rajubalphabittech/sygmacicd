using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using atm;

public partial class Apps_ATM_Payroll_SetAsStart : ATMPage {

	protected override void LoadATMPage() {
		if (string.IsNullOrEmpty(Request.QueryString.Get("funcid")))
			ATMDB.RunNonQuery("up_user_setStartFunction", UserName, Request.QueryString.Get("funcid"));
	}
}