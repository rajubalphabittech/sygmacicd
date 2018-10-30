using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SygmaFramework;

public partial class UserControls_LimitedTextBox : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {

	}

	protected void Page_PreRender(object sender, EventArgs e) {
		bool skipJS = false;
		ScriptManager sm = ScriptManager.GetCurrent(this.Page);
		if (sm != null)
			skipJS = sm.IsInAsyncPostBack;

		if (!skipJS) //if this is an asyncpost back then the scripts have already been written so skip it
			WriteScripts();
		SetAttributes();

		if (!Enabled || ReadOnly)
			DisplayCount = false;
		lblCount.Text = (MaxLength - txtText.Text.Length).ToString();
	}


	#region Constuructors Class Variables and Enums

	//User Code Goes here

	#endregion Constuructors, Class Variables and Enums

	#region Public Members

	#region Methods

	//User Code Goes here

	#endregion Methods

	#region Properties

	public int MaxLength {
		get {
			if (ViewState["MaxLength"] == null)
				ViewState.Add("MaxLength", 200);
			return (int)ViewState["MaxLength"];
		}
		set { ViewState["MaxLength"] = value; }
	}

	public string Text {
		get { return txtText.Text; }
		set { txtText.Text = value; }
	}


	public Unit Width {
		get {
			if (ViewState["Width"] == null)
				ViewState.Add("Width", new Unit(200, UnitType.Pixel));
			return (Unit)ViewState["Width"];
		}
		set { ViewState["Width"] = value; }
	}

	public Unit Height {
		get {
			if (ViewState["Height"] == null)
				ViewState.Add("Height", new Unit(100, UnitType.Pixel));
			return (Unit)ViewState["Height"];
		}
		set { ViewState["Height"] = value; }
	}


	public string Name {
		get { return lblName.Text; }
		set { lblName.Text = value; }
	}

	public string TextBoxClientID {
		get {
			return txtText.ClientID;
		}
	}

	public string NameCSSClass {
		get { return pnlName.CssClass; }
		set {
			if (pnlName.CssClass != "")
				pnlName.CssClass = pnlName.CssClass + string.Concat(" ", value);
			else
				pnlName.CssClass = value;
		}
	}

	

	public string CSSClass {
		get { return pnlMain.CssClass; }
		set { pnlMain.CssClass = value; }
	}
	

	public string HeaderCSSClass {
		get { return pnlHeader.CssClass; }
		set { pnlHeader.CssClass = value; }
	}

	public string BodyCSSClass {
		get { return pnlBody.CssClass; }
		set { pnlBody.CssClass = value; }
	}

	public bool DisplayName {
		get { return lblName.Visible; }
		set { lblName.Visible = value; }
	}

	//use the style display so that the count on the front end still works
	public bool DisplayCount {
		get { return pnlCount.Style[HtmlTextWriterStyle.Display] != "none"; }
		set {
			if (!value)
				pnlCount.Style.Add(HtmlTextWriterStyle.Display, "none");
			else
				pnlCount.Style.Remove(HtmlTextWriterStyle.Display);

		}
	}

	public bool Enabled {
		get { return txtText.Enabled; }
		set { txtText.Enabled = value; }
	}

	public bool ReadOnly {
		get { return txtText.ReadOnly; }
		set { txtText.ReadOnly = value; }
	}

	public short TabIndex {
		get { return txtText.TabIndex; }
		set { txtText.TabIndex = value; }
	}


	public bool AutoPostBack {
		get { return txtText.AutoPostBack; }
		set { txtText.AutoPostBack = value; }
	}

	#endregion Properties

	#endregion Public Members

	#region Private Members

	#region Methods

	private void WriteScripts() {

		string script = string.Format("var {0}_Max = {1};", this.ClientID, MaxLength);
		string scriptKey = string.Format("{0}_script", this.ClientID);
		Javascript.RegisterClientScriptBlock(this.Page, scriptKey, script, true);

		//Page.ClientScript.RegisterClientScriptBlock(typeof(Page), scriptKey, script, true);

		//in case there is 2 LimitTextBoxes only add this function once
		string limitFuncKey = "LimitFunction";
		if (!Page.ClientScript.IsClientScriptBlockRegistered(limitFuncKey)) {
			StringBuilder sb = new StringBuilder();
			sb.Append("function LimitTextArea(c, counterName, maxLen) {");
			sb.Append("var counter = document.getElementById(counterName);");
			sb.Append("if (counter != null){");
			sb.Append("var len = c.value.length;");
			sb.Append("if (len > maxLen) {");
			sb.Append("c.value = c.value.toString().substr(0, maxLen);");
			sb.Append("} else {");
			sb.Append("counter.innerHTML = maxLen - len;}}}");
			Javascript.RegisterClientScriptBlock(this.Page, limitFuncKey, sb.ToString(), true);
			//Page.ClientScript.RegisterClientScriptBlock(typeof(Page), limitFuncKey, sb.ToString(), true);
		}

		//set the start script
		string startScript = string.Format("if (document.getElementById('{0}') != null) document.getElementById('{0}').innerHTML = {2}_Max - document.getElementById('{1}').value.length;", lblCount.ClientID, txtText.ClientID, this.ClientID);
		string startKey = string.Format("{0}_start", this.ClientID);
		Javascript.RegisterStartupScript(this.Page, startKey, startScript, true);
		//Page.ClientScript.RegisterStartupScript(typeof(Page), startKey, startScript, true);

		txtText.Attributes.Add("onKeyUp", string.Format("LimitTextArea(this, '{0}', {1}_Max);", lblCount.ClientID, this.ClientID));
	}

	private void SetAttributes() {
		pnlMain.Width = Width;
		pnlHeader.Width = Width;
		//pnlName.Width = Width;
		//pnlCount.Width = Width;

		txtText.Height = Height;
		txtText.Width = new Unit(Width.Value - 15, Width.Type);
		//use the style display so that the count on the front end still works
		if (!DisplayCount && !DisplayName) {
			pnlHeader.Style.Add(HtmlTextWriterStyle.Display, "none");
		}
	}

	#endregion Methods

	#endregion Private Members

	#region Event Handlers

	//User Code Goes Here

	#endregion Event Handlers


	public event EventHandler TextChanged;
	protected void txtText_TextChanged(object sender, EventArgs e) {
		if (TextChanged != null)
			TextChanged(sender, e);
	}
}
