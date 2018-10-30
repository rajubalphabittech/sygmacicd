using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using atm;

public partial class UserControls_RejectButton : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		btnReject.OnClientClick = string.Format("return GetReason('{0}');", hidRejectReason.ClientID);		
		//BasePage.AddClientVariable("gRejectReason", hidRejectReason.ClientID);
		
		//btnOK.OnClientClick = String.Format("mpClickOK('{0}','{1}')", btnOK.UniqueID, "");
		//string key =  "REJECT_SCRIPTS";
		//if (!Page.ClientScript.IsClientScriptBlockRegistered(key)) {
		//  string script = "function mpClickOK(sender, e){__doPostBack(sender, e);}";
		//  //StringBuilder script = new StringBuilder("function mpClickOK(sender, e){");
		//  //script.AppendFormat(" if (document.getElementById('{0}').value == '')", txtRejectReason.ClientID);
		//  //script.Append("{ alert('Please enter a reason'); return false; } else {__doPostBack(sender, e);}}"); //in case i ever feel like adding this to make the box focus "var clientid; function mpSetFocus(txtClientId) { clientid = txtClientId; setTimeout('mpFocus()', 400); } function mpFocus() { $get(clientid).focus(); } "
		//  Page.ClientScript.RegisterClientScriptBlock(typeof(Page), key, script, true);
		//}
	}

	public string ReasonText {
		get { return hidRejectReason.Value; }
		set { 
			hidRejectReason.Value = value; 
		}
	}
	private string gTitle = null;
	public string Title {
		get { return gTitle; }
		set { gTitle = value; }
	}
	
	public short TabIndex {
		get { return btnReject.TabIndex; }
		set { btnReject.TabIndex = value; }
	}

	public event RejectButtonOKClickEventHandler OKClick;

	protected void btnOK_Click(object sender, EventArgs e) {
		if (OKClick != null)
			OKClick(sender, new RejectButtonOKClickEventArgs(hidRejectReason.Value));
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
