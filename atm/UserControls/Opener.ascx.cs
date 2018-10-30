using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using atm;
using SygmaFramework;

public partial class UserControls_Opener : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		StringBuilder sb = new StringBuilder();
		sb.Append("try{");
		sb.AppendFormat("if (window.opener != null)document.getElementById('{0}').value = window.opener.location.href;", hidOpenerUrl.ClientID);
		sb.Append("}catch(er){}");
		Page.ClientScript.RegisterStartupScript(typeof(Page), "OPENER_URL", sb.ToString(), true);
	}

	public void Refresh() {
		Javascript.RegisterStartupScript(this.Page, "REFRESH_OPENER", "if (window.opener != null) window.opener.location.href=window.opener.location.href;", true);
	}

	public string Url {
		get { return hidOpenerUrl.Value; }
	}

	
	public bool HasOpener {
		get { return hidOpenerUrl.Value != ""; }
	}
	
	private BasePage gBasePage;
	public BasePage BasePage {
		get {
			try {
				if (gBasePage == null)
					gBasePage = (BasePage)this.Page;
				return gBasePage;
			} catch (Exception exp) {
				throw new InvalidBasePageException(exp);
			}
		}
	}

}
