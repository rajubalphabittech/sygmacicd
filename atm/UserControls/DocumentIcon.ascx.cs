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
using System.IO;
using SygmaFramework;

public partial class UserControls_DocumentIcon : System.Web.UI.UserControl {
	XMLConfig gConfig = new XMLConfig();
	protected void Page_Load(object sender, EventArgs e) {

	}

	protected void Page_PreRender(object sender, EventArgs e) {
		if (Extension == "")
			SetEmptyExtension();
		SetIcon();
	}

	#region Constructors, Class Variables and Enums

	public enum DocumentIconType {
		Web,
		File
	}

	#endregion Constuructors Class Variables and Enums

	#region Private Members

	#region Methods

	private void SetIcon() {
		string iconsDir = gConfig.GetAppSetting("fileicons", "icons", "dir");
		string iconsPath = Path.Combine(Request.PhysicalApplicationPath, iconsDir);
		string iconFormat = gConfig.GetAppSetting("fileicons", "icons", "fileformat");
		string iconName = string.Format(iconFormat, Extension);

		if (!File.Exists(Path.Combine(iconsPath, iconName))) {
			DocumentIconType dit = (IsWebExtension(Extension)) ? DocumentIconType.Web : DocumentIconType.File;
			iconName = string.Format(iconFormat, dit.ToString().ToLower());
		}
		imgIcon.ImageUrl = string.Format("~/{0}/{1}", iconsDir.Replace(@"\", "/"), iconName);
	}
	private bool HasInvalidChars(string value) {
		return value.IndexOfAny(Path.GetInvalidPathChars()) > 0;
	}
	private void SetEmptyExtension() {
		if (FileName != "") {

			if (IsWebExtension(Path.GetExtension(FileName))) {
				Extension = "url";
			} else {
				if (HasInvalidChars(FileName)) {
					int qsIndex = FileName.IndexOf('?');  //if it gets here the FileName has a querystring, strip the querystring and 
					if (qsIndex >= 0)
						Extension = Path.GetExtension(FileName.Substring(0, qsIndex));
				} else {
					string ext = Path.GetExtension(FileName).Trim().ToLower();
					Extension = (ext == "") ? WebCommon.IsWebUrl(FileName) ? "url" : "folder" : ext;
				}
			}
		}
	}
	private bool IsWebExtension(string ext) {
		return Array.IndexOf(WebExtensions, ext) > -1;
	}

	#endregion Methods

	#endregion Public Methods

	#region Public Members

	#region Methods

	#endregion Methods

	#region Properties

	public string FileName {
		get {
			if (ViewState["FileName"] == null)
				ViewState.Add("FileName", "");
			return ViewState["FileName"].ToString();
		}
		set { ViewState["FileName"] = value; }
	}
	public string Extension {
		get {
			if (ViewState["Extension"] == null)
				ViewState.Add("Extension", "");
			return ViewState["Extension"].ToString();
		}
		set {
			ViewState["Extension"] = (value.StartsWith(".")) ? value.Substring(1) : value;
		}
	}
	private string[] gWebExtensions;
	public string[] WebExtensions {
		get {
			if (gWebExtensions == null)
				gWebExtensions = gConfig.GetAppSetting("fileicons", "generics", "web").Split(';');
			return gWebExtensions;
		}
	}
	#endregion Properties

	#endregion Public Methods

	#region Event Handlers

	//User Code Goes Here

	#endregion





}
