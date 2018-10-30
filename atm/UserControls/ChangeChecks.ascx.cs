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
using System.Text;
using Sygmanet;

public partial class UserControls_ChangeChecks : System.Web.UI.UserControl {

	private string gChangeCheckAllBoxScript;

	#region Page Event Handlers

	protected void Page_Load(object sender, EventArgs e) {
		SetChangeCheckAllScript();
	}

	private void SetChangeCheckAllScript() {
		gChangeCheckAllBoxScript = string.Format("ChangeCheckAllBox(this, '{0}');", chkCheckAll.ClientID);
	}

	protected void Page_PreRender(object sender, EventArgs e) {
		switch (DisplayType) {
			case ChangeChecksDisplayType.Links:
				viewLinks.Visible = true;
				SetLinks();
				break;
			case ChangeChecksDisplayType.CheckBox:
				chkCheckAll.Visible = true;
				string prefix = (ContainerName != this.Page.ClientID) ? ContainerName : "";
				string clickText = string.Format("ChangeChecksCheckBox(this, '{0}', '{1}');", prefix, ChecksName);
				if (onClick != "")
					clickText = string.Format("{0} {1}", clickText, onClick);
				chkCheckAll.Attributes.Add("onClick", clickText);
				break;
		}

	}

	private void SetLinks() {
		string containerName = "";
		if (!IgnoreContainerName)
			if (ContainerName != this.Page.ClientID)
				containerName = ContainerName;

		hlCheckAll.NavigateUrl = string.Format("javascript: ChangeChecks('all', '{0}', '{1}', {2});", containerName, ChecksName, FireOnClick.ToString().ToLower());
		hlCheckAll.Attributes.Add("onclick", onClick);
		hlUnCheckAll.NavigateUrl = string.Format("javascript: ChangeChecks('none', '{0}', '{1}', {2});", containerName, ChecksName, FireOnClick.ToString().ToLower());
		hlUnCheckAll.Attributes.Add("onclick", onClick);
	}

	#endregion


	#region Private Members

	#region Methods


	#endregion Methods

	#endregion Public Methods

	#region Public Members

	#region Methods

	#endregion Methods

	#region Properties

	public string ContainerName {
		get {
			if (ViewState["ContainerName"] == null) {
				object parent = this.NamingContainer;
				string cn = this.NamingContainer.ClientID;

				if (parent.GetType() == typeof(GridViewRow)) {
					GridViewRow row = (GridViewRow)parent;
					if (row.RowType == DataControlRowType.Header)
						cn = row.NamingContainer.ClientID;
				}

				ViewState.Add("ContainerName", cn);
			}
			return (string)ViewState["ContainerName"];
		}
		set { ViewState["ContainerName"] = value; }
	}
	public string ChecksName {
		get {
			if (ViewState["ChecksName"] == null)
				ViewState.Add("ChecksName", "");
			return (string)ViewState["ChecksName"];
		}
		set { ViewState["ChecksName"] = value; }

	}
	public ChangeChecksDisplayType DisplayType {
		get {
			if (ViewState["DisplayType"] == null)
				ViewState.Add("DisplayType", ChangeChecksDisplayType.Links);
			return (ChangeChecksDisplayType)ViewState["DisplayType"];
		}
		set { ViewState["DisplayType"] = value; }
	}
	public bool Checked {
		get { return chkCheckAll.Checked; }
		set { chkCheckAll.Checked = value; }
	}
	public string CheckBoxID {
		get { return chkCheckAll.ID; }
		set {
			chkCheckAll.ID = value;
			SetChangeCheckAllScript();
		}
	}
	private bool gIgnoreContainerName;

	public bool IgnoreContainerName {
		get { return gIgnoreContainerName; }
		set { gIgnoreContainerName = value; }
	}
		
	public string ChangeCheckAllBoxScript {
		get { return gChangeCheckAllBoxScript; }
	}

	public string Text {
		get { return chkCheckAll.Text; }
		set { chkCheckAll.Text = value; }
	}
	/// <summary>
	/// This causes each check that is changed to fire it's onClick event
	/// </summary>
	public bool FireOnClick {
		get {
			if (ViewState["FireOnClick"] == null)
				ViewState.Add("FireOnClick", false);
			return (bool)ViewState["FireOnClick"];
		}
		set { ViewState["FireOnClick"] = value; }
	}


	public bool Enabled {
		get {
			return chkCheckAll.Enabled;
		}
		set {
			chkCheckAll.Enabled = value;
			if (!value)
				viewLinks.Visible = false;
		}
	}
	public string onClick {
		get {
			if (ViewState["onClick"] == null)
				ViewState.Add("onClick", "");
			return (string)ViewState["onClick"];
		}
		set { ViewState["onClick"] = value; }
	}


	#endregion Properties

	#endregion Public Methods

	#region Event Handlers

	//User Code Goes Here

	#endregion
}
