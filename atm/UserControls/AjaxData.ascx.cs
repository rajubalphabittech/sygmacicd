using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SygmaFramework;

public partial class UserControls_AjaxData : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		this.Page.ClientScript.RegisterStartupScript(this.GetType(), "AJAX_KEY", "AjaxFunction();", true);
	}

	protected void Page_PreRender(object sender, EventArgs e) {
		
	}
				
	private string gDataPage;

	public string DataPage {
		get { return gDataPage; }
		set { gDataPage = Web.FormatUrl(value, Application["AppPath"].ToString()); }
	}

	private string gInitialValue = "";

	public string InitialValue {
		get { return gInitialValue; }
		set { gInitialValue = value; }
	}

	public bool EnableLoadingImage {
		get { return imgLoading.Visible; }
		set { imgLoading.Visible = value; }
	}

	public string LoadingImageUrl {
		get { return imgLoading.ImageUrl; }
		set { imgLoading.ImageUrl = value; }
	}

}
