using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using atm;

public partial class UserControls_ItemSearch : System.Web.UI.UserControl {
	public event EventHandler TextChanged;

	protected void Page_Load(object sender, EventArgs e) {

	}

	private string[] GetItems(string searchString, int count) {
		return WebCommon.GetItems(searchString, count);
	}

	protected void txtItem_TextChanged(object sender, EventArgs e) {
		if (TextChanged != null)
			TextChanged(sender, e);
	}

	public bool ParseItem(out int itemNo) {
		itemNo = 0;
		string text = txtItem.Text.Trim();
		if (text != "") {
			text = WebCommon.ParseItemNo(text);
			if (Int32.TryParse(text, out itemNo)) {
				SetItemDisplay(itemNo);
				return true;
			}
		}
		return false;
	}

	public void SetItemDisplay(int itemNo) {
		if (itemNo != 0) {
			object oItemDisplay = BasePage.IntranetDB.GetScalar("up_getItemDisplay", itemNo);
			if (oItemDisplay != null) {
				string itemDisplay = oItemDisplay.ToString();
				txtItem.Text = itemDisplay;
			}
		}
	}


	public void Validate() {
		rfvItem.Validate();
	}

	public string Text {
		get { return txtItem.Text; }
		set { txtItem.Text = value; }
	}

	public bool AutoPostBack {
		get { return txtItem.AutoPostBack; }
		set { txtItem.AutoPostBack = value; }
	}

	#region Validation Properties

	public bool IsRequired {
		get { return rfvItem.Visible; }
		set { rfvItem.Visible = value; }
	}

	public string RequiredText {
		get { return rfvItem.Text; }
		set { rfvItem.Text = value; }
	}

	public ValidatorDisplay RequiredDisplay {
		get { return rfvItem.Display; }
		set { rfvItem.Display = value; }
	}

	public string ValidationGroup {
		get { return rfvItem.ValidationGroup; }
		set { rfvItem.ValidationGroup = value; }
	}

	public string RequiredErrorMessage {
		get { return rfvItem.ErrorMessage; }
		set { rfvItem.ErrorMessage = value; }
	}

	public bool RequiredEnableClientScript {
		get { return rfvItem.EnableClientScript; }
		set { rfvItem.EnableClientScript = value; }
	}

	public short TabIndex {
		get { return txtItem.TabIndex; }
		set { txtItem.TabIndex = value; }
	}

	public bool IsValid {
		get { return rfvItem.IsValid; }
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


	#endregion Validation Properties

	//this is the template for the ajax hook
	//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
	//public static string[] GetItemList(string prefixText, int count, string contextKey) {
	//  return WebCommon.GetItems(prefixText, count);
	//}

}
