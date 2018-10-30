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

public partial class UserControls_JScript : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (gFileNames != null) {
			string scriptKey = string.Concat("CS_AppPath");
			string appPath = (gUseFullUrlPaths) ? ClientFullAppPath : Application["AppPath"].ToString();
			if (!Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType(), scriptKey))
				Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), scriptKey, string.Format("var AppPath = '{0}';", appPath), true);

			foreach (string fileName in gFileNames) {
				string fn = fileName.Trim();
				string includeScriptKey = string.Concat("Script_File_", System.IO.Path.GetFileNameWithoutExtension(fn));
				if (!Page.ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), includeScriptKey))
					Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), includeScriptKey, string.Format("{0}Scripts/{1}", appPath, fn));
			}
		}
	}

	private string[] gFileNames = new string[] { "Common.js" };

	public string FileNames {
		get { return string.Join(",", gFileNames); }
		set { gFileNames = value.Split(','); }
	}

	/// <summary>
	/// This is for legacy pages and should be deprecated when possible
	/// </summary>
	public string FileName {
		get { return gFileNames[0]; }
		set {
			string[] fn = new string[1];
			fn[0] = value;
			gFileNames = fn;
		}
	}


	private bool gUseFullUrlPaths;

	public bool UseFullUrlPaths {
		get { return gUseFullUrlPaths; }
		set { gUseFullUrlPaths = value; }
	}
	private string gClientFullAppPath;

	public string ClientFullAppPath {
		get { return (gClientFullAppPath != null) ? gClientFullAppPath : Application["AppPath"].ToString(); }
		set { gClientFullAppPath = value; }
	}
}
